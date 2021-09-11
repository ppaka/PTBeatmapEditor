﻿/*
UniBpmAnalyzer
Copyright (c) 2016 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using UnityEngine;

public class UniBpmAnalyzer
{
	static readonly BpmMatchData[] bpmMatchDatas = new BpmMatchData[MAX_BPM - MIN_BPM + 1];

    /// <summary>
    ///     Analyze BPM from an audio clip
    /// </summary>
    /// <param name="clip">target audio clip</param>
    /// <returns>bpm</returns>
    public static int AnalyzeBpm(AudioClip clip)
	{
		for (int i = 0; i < bpmMatchDatas.Length; i++) bpmMatchDatas[i].match = 0f;
		if (clip == null) return -1;
		Debug.Log("AnalyzeBpm audioClipName : " + clip.name);

		int frequency = clip.frequency;
		Debug.Log("Frequency : " + frequency);

		int channels = clip.channels;
		Debug.Log("Channels : " + channels);

		int splitFrameSize = Mathf.FloorToInt(frequency / (float) BASE_FREQUENCY * (channels / (float) BASE_CHANNELS) *
		                                      BASE_SPLIT_SAMPLE_SIZE);

		// Get all sample data from audioclip
		float[] allSamples = new float[clip.samples * channels];
		clip.GetData(allSamples, 0);

		// Create volume array from all sample data
		float[] volumeArr = CreateVolumeArray(allSamples, splitFrameSize);

		// Search bpm from volume array
		int bpm = SearchBpm(volumeArr, frequency, splitFrameSize);
		Debug.Log("Matched BPM : " + bpm);

		StringBuilder strBuilder = new StringBuilder("BPM Match Data List\n");
		for (int i = 0; i < bpmMatchDatas.Length; i++)
			strBuilder.Append("bpm : " + bpmMatchDatas[i].bpm + ", match : " +
			                  Mathf.FloorToInt(bpmMatchDatas[i].match * 10000f) + "\n");
		Debug.Log(strBuilder.ToString());

		return bpm;
	}

    /// <summary>
    ///     Analyze BPM from an audio clip and Doesn't Logging
    /// </summary>
    /// <param name="clip">target audio clip</param>
    /// <returns>bpm</returns>
    public static int AnalyzeBpmWithoutLogging(AudioClip clip)
	{
		for (int i = 0; i < bpmMatchDatas.Length; i++) bpmMatchDatas[i].match = 0f;
		if (clip == null) return -1;

		int frequency = clip.frequency;

		int channels = clip.channels;

		int splitFrameSize = Mathf.FloorToInt(frequency / (float) BASE_FREQUENCY * (channels / (float) BASE_CHANNELS) *
		                                      BASE_SPLIT_SAMPLE_SIZE);

		// Get all sample data from audioclip
		float[] allSamples = new float[clip.samples * channels];
		clip.GetData(allSamples, 0);

		// Create volume array from all sample data
		float[] volumeArr = CreateVolumeArray(allSamples, splitFrameSize);

		Debug.Log(clip.samples);
		Debug.Log(channels);
		Debug.Log(allSamples.Length);
		
		// Search bpm from volume array
		int bpm = SearchBpm(volumeArr, frequency, splitFrameSize);

		return bpm;
	}
    
    /// <summary>
    ///     Analyze BPM from an audio for NAudio
    /// </summary>
    /// <param name="clip">target audio clip</param>
    /// <returns>bpm</returns>
    public static int AnalyzeBpmWithoutLoggingNAudio(AudioFileReader clip)
    {
	    for (int i = 0; i < bpmMatchDatas.Length; i++) bpmMatchDatas[i].match = 0f;
	    if (clip == null) return -1;

	    int frequency = clip.WaveFormat.AverageBytesPerSecond * 8 / 1000;

	    int channels = clip.WaveFormat.Channels;

	    int splitFrameSize = Mathf.FloorToInt(frequency / (float) BASE_FREQUENCY * (channels / (float) BASE_CHANNELS) *
	                                          BASE_SPLIT_SAMPLE_SIZE);

	    Debug.Log(clip.WaveFormat.SampleRate);
	    clip.Length.Log();
	    // Get all sample data from audioclip
	    Debug.Log(channels);
	    float[] allSamples = new float[clip.WaveFormat.SampleRate * channels];
	    Debug.Log(allSamples.Length);
	    clip.Read(allSamples, 0, allSamples.Length);

	    var wholeFile = new List<float>((int) (clip.Length / 4));
	    
	    // Create volume array from all sample data
	    float[] volumeArr = CreateVolumeArray(allSamples, splitFrameSize);

	    // Search bpm from volume array
	    int bpm = SearchBpm(volumeArr, frequency, splitFrameSize);

	    return bpm;
    }

    /// <summary>
    ///     Create volume array from all sample data
    /// </summary>
    static float[] CreateVolumeArray(float[] allSamples, int splitFrameSize)
	{
		// Initialize volume array
		float[] volumeArr = new float[Mathf.CeilToInt(allSamples.Length / (float) splitFrameSize)];
		int powerIndex = 0;

		// Sample data analysis start
		for (int sampleIndex = 0; sampleIndex < allSamples.Length; sampleIndex += splitFrameSize)
		{
			float sum = 0f;
			for (int frameIndex = sampleIndex; frameIndex < sampleIndex + splitFrameSize; frameIndex++)
			{
				if (allSamples.Length <= frameIndex) break;
				// Use the absolute value, because left and right value is -1 to 1
				float absValue = Mathf.Abs(allSamples[frameIndex]);
				if (absValue > 1f) continue;

				// Calculate the amplitude square sum
				sum += absValue * absValue;
			}

			// Set volume value
			volumeArr[powerIndex] = Mathf.Sqrt(sum / splitFrameSize);
			powerIndex++;
		}

		// Representing a volume value from 0 to 1
		float maxVolume = volumeArr.Max();
		for (int i = 0; i < volumeArr.Length; i++) volumeArr[i] = volumeArr[i] / maxVolume;

		return volumeArr;
	}

    /// <summary>
    ///     Search bpm from volume array
    /// </summary>
    static int SearchBpm(float[] volumeArr, int frequency, int splitFrameSize)
	{
		// Create volume diff list
		var diffList = new List<float>();
		for (int i = 1; i < volumeArr.Length; i++) diffList.Add(Mathf.Max(volumeArr[i] - volumeArr[i - 1], 0f));

		// Calculate the degree of coincidence in each BPM
		int index = 0;
		float splitFrequency = frequency / (float) splitFrameSize;
		for (int bpm = MIN_BPM; bpm <= MAX_BPM; bpm++)
		{
			float sinMatch = 0f;
			float cosMatch = 0f;
			float bps = bpm / 60f;

			if (diffList.Count > 0)
			{
				for (int i = 0; i < diffList.Count; i++)
				{
					sinMatch += diffList[i] * Mathf.Cos(i * 2f * Mathf.PI * bps / splitFrequency);
					cosMatch += diffList[i] * Mathf.Sin(i * 2f * Mathf.PI * bps / splitFrequency);
				}

				sinMatch *= 1f / diffList.Count;
				cosMatch *= 1f / diffList.Count;
			}

			float match = Mathf.Sqrt(sinMatch * sinMatch + cosMatch * cosMatch);

			bpmMatchDatas[index].bpm = bpm;
			bpmMatchDatas[index].match = match;
			index++;
		}

		// Returns a high degree of coincidence BPM
		int matchIndex = Array.FindIndex(bpmMatchDatas, x => x.match == bpmMatchDatas.Max(y => y.match));

		return bpmMatchDatas[matchIndex].bpm;
	}

	public struct BpmMatchData
	{
		public int bpm;
		public float match;
	}

	#region CONST

	// BPM search range
	const int MIN_BPM = 60;

	const int MAX_BPM = 400;

	// Base frequency (44.1kbps)
	const int BASE_FREQUENCY = 44100;

	// Base channels (2ch)
	const int BASE_CHANNELS = 2;

	// Base split size of sample data (case of 44.1kbps & 2ch)
	const int BASE_SPLIT_SAMPLE_SIZE = 2205;

	#endregion
}