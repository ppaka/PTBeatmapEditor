using System;
using System.Threading.Tasks;
using Squirrel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquirrelUpdateManager : MonoBehaviour
{
    public TMP_Text updateText;
    public Image progImage;
    public Button button;

    private const string URL = @"https://github.com/PT-GAME/PTBeatmapEditor";
    // https://api.github.com/repos/PT-GAME/PTBeatmapEditor/releases/latest

#if !UNITY_EDITOR
    async void Start()
    {
        await Task.Run(CheckForUpdate);
    }
#endif

    async Task CheckForUpdate()
    {
        UnityMainThread.wkr.AddJob(() => { updateText.text = "업데이트 확인 중"; });

        try
        {
            using (UpdateManager manager = await UpdateManager.GitHubUpdateManager(URL))
            {
                UnityMainThread.wkr.AddJob(() => updateText.text = "업데이트 확인 중");

                if (manager.IsInstalledApp == false)
                {
                    UnityMainThread.wkr.AddJob(() => updateText.text = "Squirrel 호환 앱이 아닙니다");
                    return;
                }

                UpdateInfo info = await manager.CheckForUpdate();

                if (info.ReleasesToApply.Count == 0)
                {
                    UnityMainThread.wkr.AddJob(() => updateText.text = "최신버전 입니다");
                    return;
                }

                UnityMainThread.wkr.AddJob(() => progImage.fillAmount = 0);
                UnityMainThread.wkr.AddJob(() => updateText.text = "업데이트 다운로드중...");

                await manager.DownloadReleases(info.ReleasesToApply,
                    p => { UnityMainThread.wkr.AddJob(() => progImage.fillAmount = p / 100f); });

                UnityMainThread.wkr.AddJob(() => progImage.fillAmount = 0);
                UnityMainThread.wkr.AddJob(() => updateText.text = "업데이트 설치중...");

                await manager.ApplyReleases(info, p => progImage.fillAmount = p / 100f);

                UnityMainThread.wkr.AddJob(() => progImage.fillAmount = 0);
                UnityMainThread.wkr.AddJob(() => updateText.text = "재시작하여 업데이트를 완료해주세요!");
            }
        }
        catch (Exception e)
        {
            UnityMainThread.wkr.AddJob(() => updateText.text = "업데이트를 시도하는 도중 문제가 발생했습니다.");
            throw new Exception(e.Message);
        }
    }
}