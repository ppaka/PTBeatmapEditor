using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Squirrel;
using Squirrel.SimpleSplat;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class SquirrelUpdateManager : MonoBehaviour
{
    public TMP_Text updateText;
    public Image progImage;
    public Button button;

    private const string URL = @"https://github.com/PT-GAME/PTBeatmapEditor";
    // https://api.github.com/repos/PT-GAME/PTBeatmapEditor/releases/latest

    readonly ConcurrentQueue<string> _stQueue = new ConcurrentQueue<string>();
    ConcurrentQueue<float> _flQueue = new ConcurrentQueue<float>();
    bool isComplete;

    async void Start()
    {
        await Task.Run(CheckForUpdate);
    }

    void Update()
    {
        if (_stQueue.Count != 0)
        {
            _stQueue.TryDequeue(out string uiText);
            updateText.text = uiText;
        }

        if (_stQueue.Count != 0)
        {
            _flQueue.TryDequeue(out float percent);
            progImage.fillAmount = percent;
        }

        if (isComplete)
        {
            isComplete = false;
            button.onClick.AddListener(() => { UpdateManager.RestartApp(); });
        }
    }

    async Task CheckForUpdate()
    {
        Squirrel.SimpleSplat.DebugLogger logger = new DebugLogger
        {
            Level = LogLevel.Debug
        };
        logger.Write("Running Task", LogLevel.Debug);

        using (UpdateManager manager = await UpdateManager.GitHubUpdateManager(URL))
        {
            _stQueue.Enqueue("업데이트 확인 중");
            
            if (manager.IsInstalledApp == false)
            {
                _stQueue.Enqueue("Squirrel 호환 앱이 아닙니다");
                return;
            }
            
            UpdateInfo info = await manager.CheckForUpdate();

            if (info.ReleasesToApply.Count == 0)
            {
                _stQueue.Enqueue("최신버전 입니다");
                return;
            }

            _flQueue.Enqueue(0);
            _stQueue.Enqueue("업데이트 다운로드중...");

            await manager.DownloadReleases(info.ReleasesToApply, p => _flQueue.Enqueue(p / 100f));

            _flQueue.Enqueue(0);
            _stQueue.Enqueue("업데이트 설치중...");

            await manager.ApplyReleases(info, p => _flQueue.Enqueue(p / 100f));

            _flQueue.Enqueue(0);
            _stQueue.Enqueue("재시작하여 업데이트를 완료 해주세요!");
            isComplete = true;
        }
    }
}