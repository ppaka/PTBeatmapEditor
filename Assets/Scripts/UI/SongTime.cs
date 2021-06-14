using System;
using TMPro;
using UnityEngine;

public class SongTime : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text text;

    public bool changed;
    private int _hour;
    private int _min;

    private int _mSec;
    private float _nowTime;
    private int _sec;

    private void Start()
    {
        _nowTime = (float) (Math.Truncate(audioSource.time * 1) / 1);
    }

    private void Update()
    {
        if (changed) return;
        UpdateTime(audioSource.time, false);
    }

    public void UpdateTime(float songTime, bool changeAudioSourceTime = true)
    {
        try
        {
            if (changeAudioSourceTime)
                audioSource.time = _nowTime = songTime;
            else
                _nowTime = songTime;

            _mSec = (int) ((_nowTime - (int) _nowTime) * 100);
            _sec = (int) (_nowTime % 60);
            _min = (int) (_nowTime / 60 % 60);
            _hour = (int) (_nowTime / 60 / 60 % 60);

            text.text = $"{_hour:00}:{_min:00}:{_sec:00};{_mSec:00}";
        }
        catch
        {
            //
        }
    }
}