using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFiles : MonoBehaviour
{
    public AudioSource audioSource;

    public Text selectClipText;

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
        
        
    }
}
