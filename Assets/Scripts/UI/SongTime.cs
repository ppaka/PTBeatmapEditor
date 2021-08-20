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
				time = new DateTime((long)((ldc.levelData.settings.noteOffset * 0.001f - songTime) * TimeSpan.TicksPerSecond));
				songTimeTextField.text = $"-{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
			}
			else
			{
				time = new DateTime((long)((songTime - ldc.levelData.settings.noteOffset * 0.001f) * TimeSpan.TicksPerSecond));
				songTimeTextField.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00};{time.Millisecond:000}";
			}
		}
		catch
		{
			//
		}
	}
}