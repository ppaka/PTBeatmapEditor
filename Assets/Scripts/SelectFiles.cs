using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFiles : MonoBehaviour
{
    public LevelDataContainer ldc;
    public AudioSource audioSource;

    public Text selectClipText, selectBeatmapText;

    private void FixedUpdate()
    {
        if (audioSource.clip == null)
        {
            selectClipText.text = "아직 불러온 음악이 없습니다!";
        }
        else
        {
            selectClipText.text = "음악 선택됨";
        }

        if (!ldc.level.Contains("[Info]"))
        {
            selectBeatmapText.text = "비트맵을 선택해주세요!";
        }
        else
        {
            selectBeatmapText.text = "비트맵 선택됨";
        }
    }
}
