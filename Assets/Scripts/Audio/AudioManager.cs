using System;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudioBPM;
using NAudioMetronome;
using SFB;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public SongSlider songSlider;
	public SongTime songTime;
	
	public WaveOutEvent outputDevice;
	public AudioFileReader audioFile;
	private AudioEngine metronomeEngine;
	private Thread _thread;

	public void OnEnable()
	{
		SystemEvents.audioLoadComplete += ResetAudio;
	}

	public void OnDisable()
	{
		SystemEvents.audioLoadComplete -= ResetAudio;
	}

	public float AnalyzeBpm()
	{
		return UniBpmAnalyzer.AnalyzeBpmWithoutLoggingNAudio(audioFile);
	}

	void OnApplicationQuit()
	{
		_thread?.Abort();
		outputDevice.Stop();
		metronomeEngine.Stop();
	}

	void ResetAudio()
	{
		outputDevice.Init(audioFile);
		metronomeEngine.Stop();
	}

	public float bpm;

	public void AudioPlay()
	{
		outputDevice = new WaveOutEvent();
		string[] path =
			StandaloneFileBrowser.OpenFilePanel("음악 파일 불러오기", null, new[]{new ExtensionFilter("음악 파일", "mp3")}, false);
		audioFile = new AudioFileReader(new Url(path[0]).Value);
		
		BPMDetector bpmDetector = new BPMDetector(new Url(path[0]).Value, 0, 0);
		if (bpmDetector.Groups.Length > 0) bpm = bpmDetector.Groups[0].Tempo;
		if (bpmDetector.Groups.Length > 0)
		{
			print(string.Format("Most probable BPM is {0} ({1} samples)", bpmDetector.Groups[0].Tempo, bpmDetector.Groups[0].Count));
			if (bpmDetector.Groups.Length > 1)
			{
				print("Other options are:");
				for (int i = 1; i < bpmDetector.Groups.Length; ++i)
				{
					print(string.Format("{0} BPM ({1} samples)", bpmDetector.Groups[i].Tempo, bpmDetector.Groups[i].Count));
				}
			}
		}
		
		_thread?.Abort();

		_thread = new Thread(Run);
		_thread.Start();
	}

	void Run()
	{
		outputDevice.Init(audioFile);
		outputDevice.Volume = 0.07f;
		metronomeEngine = new AudioEngine();
		
		outputDevice.Play();
		metronomeEngine.Play((int) bpm, 4, 1.48f);
	}

	public void AudioStop()
	{
		outputDevice.Stop();
		metronomeEngine.Stop();
		songSlider.slider.value = 0;
		
		songTime.songTimeTextField.readOnly = false;
	}
}