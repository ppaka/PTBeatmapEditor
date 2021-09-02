using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquirrelUpdateManager : MonoBehaviour
{
    private const string URL = @"https://github.com/PT-GAME/PTBeatmapEditor";
// https://api.github.com/repos/PT-GAME/PTBeatmapEditor/releases/latest
    public Sprite downloadIcon, errorIcon;
    public AlertObject alertObject;

#if !UNITY_EDITOR
    async void Start()
    {
        await Task.Run(CheckForUpdate);
    }
#endif

    async Task CheckForUpdate()
    {
        try
        {
            using (UpdateManager manager = await UpdateManager.GitHubUpdateManager(URL))
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    Alert alert = new Alert
                    {
                        title = "업데이터",
                        description = "업데이트 확인 중",
                        behaviour = ClickBehaviour.None,
                        icon = downloadIcon,
                        color = new Color(0.6f, 0.3f, 0.3f, 1)
                    };
                    alertObject = AlertManager.instance.ShowAlert(alert);
                });

                UpdateInfo info = await manager.CheckForUpdate();

                if (info.ReleasesToApply.Count == 0)
                {
                    UnityMainThread.wkr.AddJob(() =>
                    {
                        alertObject.layer.color = new Color(0, 0, 0, 0.0f);
                        alertObject.layer.fillAmount = 1;
                        alertObject.descriptionText.text = "업데이트가 없습니다";
                    });
                    return;
                }

                UnityMainThread.wkr.AddJob(() =>
                {
                    alertObject.layer.fillAmount = 100;
                    alertObject.descriptionText.text = "다운로드 중...";
                });

                await manager.DownloadReleases(info.ReleasesToApply,
                    p => { UnityMainThread.wkr.AddJob(() => alertObject.layer.fillAmount = (100 - p) / 100f); });

                UnityMainThread.wkr.AddJob(() =>
                {
                    alertObject.layer.fillAmount = 100;
                    alertObject.descriptionText.text = "설치 중...";
                });

                await manager.ApplyReleases(info,
                    p => { UnityMainThread.wkr.AddJob(() => alertObject.layer.fillAmount = (100 - p) / 100f); });

                UnityMainThread.wkr.AddJob(() =>
                {
                    alertObject.layer.fillAmount = 0;
                    alertObject.descriptionText.text = "작업이 완료 되었습니다.\n여기를 클릭하여 재시작 해주세요!";
                    alertObject.alert.behaviour = ClickBehaviour.ShowPopup;
                    Action action = () => { Application.Quit(); };
                    PopupButtonElements button = new PopupButtonElements()
                        { clickAction = action, color = Color.cyan, text = "종료할래요" };
                    PopupButtonElements button2 = new PopupButtonElements()
                        { clickAction = action, color = Color.red, text = "아니에요! 하지 말아주세요!" };
                    List<PopupButtonElements> list = new List<PopupButtonElements>();
                    list.Add(button);
                    list.Add(button2);
                    alertObject.alert.popup = new AlertOnClickPopup()
                    {
                        buttons = list,
                        description = "...",
                        title = "에디터 종료"
                    };
                    alertObject.button.onClick.AddListener(() => AlertManager.instance.OnClickAlert(alertObject.alert.behaviour, alertObject.alert.popup));
                });
            }
        }
        catch (Exception e)
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                alertObject.icon.sprite = errorIcon;
                alertObject.layer.fillAmount = 0;
                alertObject.descriptionText.text = "작업 중 오류가 발생하였습니다...";
                alertObject.button.onClick.RemoveAllListeners();
            });
            throw new Exception(e.Message);
        }
    }
}