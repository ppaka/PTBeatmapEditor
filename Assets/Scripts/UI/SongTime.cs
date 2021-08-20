using System;
using System.Globalization;
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

		if (0 < ldc.levelData.settings.noteOffset * 0.001f)
		{
			time = new DateTime((long) (((decimal)(ldc.levelData.settings.noteOffset * 0.001f) - (decimal) 0) * TimeSpan.TicksPerSecond));
			songTimeTextField.text = $"-{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
		}
		else if (0 >= ldc.levelData.settings.noteOffset * 0.001f)
		{
			time = new DateTime((long) (((decimal) 0 - (decimal) (ldc.levelData.settings.noteOffset * 0.001f)) * TimeSpan.TicksPerSecond));
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
		if (int.TryParse(value, NumberStyles.Integer, null, out int time))
		{
			time += ldc.levelData.settings.noteOffset;
			UpdateTime(time * 0.001f, true);
			slider.slider.value = audioSource.time / audioSource.clip.length;
		}
		else
		{
			UpdateTime(audioSource.time, false);
		}
	}

	public void UpdateTime(float songTime, bool changeAudioSourceTime = true)
	{
		try
		{
			if (changeAudioSourceTime)
				audioSource.time = songTime;
			
			DateTime time;

			if (songTime < ldc.levelData.settings.noteOffset * 0.001f)
			{
				time = new DateTime((long) (((decimal)(ldc.levelData.settings.noteOffset * 0.001f) - (decimal) songTime) * TimeSpan.TicksPerSecond));
				songTimeTextField.text = $"-{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
			}
			else if (songTime >= ldc.levelData.settings.noteOffset * 0.001f)
			{
				time = new DateTime((long) (((decimal) songTime - (decimal) (ldc.levelData.settings.noteOffset * 0.001f)) * TimeSpan.TicksPerSecond));
				songTimeTextField.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
			}
		}
		catch
		{
			//
		}
	}
}