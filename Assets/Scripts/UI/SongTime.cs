using System;
using System.Globalization;
using NAudio.Utils;
using NAudio.Wave;
using TMPro;
using UnityEngine;

public class SongTime : MonoBehaviour
{
    public LevelDataContainer ldc;
    public SongSlider slider;
    public TMP_InputField songTimeTextField;

    public bool changed;

    void OnEnable()
    {
        SystemEvents.audioLoadComplete += UpdateFirstTime;
    }

    void OnDisable()
    {
        SystemEvents.audioLoadComplete -= UpdateFirstTime;
    }

    void UpdateFirstTime()
    {
        DateTime time;


        /*time = new DateTime((long)((0 - (decimal)(LevelTimings.startOffset * 0.001f)) * TimeSpan.TicksPerSecond));*/
        time = new DateTime((long)(0 * TimeSpan.TicksPerSecond));
        songTimeTextField.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
    }

    void Update()
    {
        if (AudioManager.Instance.outputDevice== null) return;
        if (changed || AudioManager.Instance.outputDevice.PlaybackState != PlaybackState.Playing) return;
        UpdateTime((float) AudioManager.Instance.outputDevice.GetPosition(), false);
    }

    public void MoveTime(string value)
    {
        if (songTimeTextField.readOnly) return;

        if (int.TryParse(value, NumberStyles.Integer, null, out int time))
        {
            UpdateTime(time * AudioManager.Instance.audioFile.WaveFormat.AverageBytesPerSecond);
            slider.slider.value = AudioManager.Instance.audioFile.Position;
        }
        else if (SongTimeConverter.ToInt(value, out time))
        {
            UpdateTime(time * AudioManager.Instance.audioFile.WaveFormat.AverageBytesPerSecond);
            slider.slider.value = AudioManager.Instance.audioFile.Position;
        }
        else
        {
            UpdateTime(AudioManager.Instance.audioFile.Position, false);
        }
    }

    public void UpdateTime(float songTime, bool changeAudioSourceTime = true)
    {
        if (changeAudioSourceTime) AudioManager.Instance.audioFile.Position = (long) songTime;

        var audioTime = songTime / (float) AudioManager.Instance.audioFile.WaveFormat.AverageBytesPerSecond;
        DateTime time;
        /*time = new DateTime((long)(((decimal)songTime - (decimal)(LevelTimings.startOffset * 0.001f)) *
                                   TimeSpan.TicksPerSecond));*/
        time = new DateTime((long)((decimal)audioTime * TimeSpan.TicksPerSecond));
        songTimeTextField.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
    }
}