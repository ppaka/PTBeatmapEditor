using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class SongTime : MonoBehaviour
{
    public LevelDataContainer ldc;
    public SongSlider slider;
    public AudioSource audioSource;
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
        
        bool isPositive = 0 >= (decimal)(ldc.levelData.settings.noteOffset * 0.001f);

        if (!isPositive)
        {
            time = new DateTime((long)(((decimal)(ldc.levelData.settings.noteOffset * 0.001f) - 0)
                                       * TimeSpan.TicksPerSecond));
            songTimeTextField.text = $"-{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
        }
        else
        {
            time = new DateTime((long)((0 - (decimal)(ldc.levelData.settings.noteOffset * 0.001f)) *
                                       TimeSpan.TicksPerSecond));
            songTimeTextField.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
        }
    }

    void Update()
    {
        if (changed || !audioSource.isPlaying) return;
        UpdateTime(audioSource.time, false);
    }

    public void MoveTime(string value)
    {
        if (songTimeTextField.readOnly) return;

        if (int.TryParse(value, NumberStyles.Integer, null, out int time))
        {
            time += ldc.levelData.settings.noteOffset;
            UpdateTime(time * 0.001f);
            slider.slider.value = audioSource.time / audioSource.clip.length;
        }
        else if (SongTimeConverter.ToInt(value, out time))
        {
            time += ldc.levelData.settings.noteOffset;
            UpdateTime(time * 0.001f);
            slider.slider.value = audioSource.time / audioSource.clip.length;
        }
        else
        {
            UpdateTime(audioSource.time, false);
        }
    }

    public void UpdateTime(float songTime, bool changeAudioSourceTime = true)
    {
        if (changeAudioSourceTime)
            audioSource.time = songTime;

        DateTime time;

        bool isMinus = 0 > (decimal)songTime - (decimal)(ldc.levelData.settings.noteOffset * 0.001f);

        if (isMinus)
        {
            time = new DateTime((long)(((decimal)(ldc.levelData.settings.noteOffset * 0.001f) - (decimal)songTime)
                                       * TimeSpan.TicksPerSecond));
            songTimeTextField.text = $"-{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
        }
        else
        {
            time = new DateTime((long)(((decimal)songTime - (decimal)(ldc.levelData.settings.noteOffset * 0.001f)) *
                                       TimeSpan.TicksPerSecond));
            songTimeTextField.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
        }
    }
}