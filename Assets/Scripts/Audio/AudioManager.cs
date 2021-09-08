using System.Security.Policy;
using NAudio.Wave;
using SFB;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource audioSource;
	public SongSlider songSlider;
	public SongTime songTime;
	
	private WaveOutEvent outputDevice;
	private AudioFileReader audioFile;

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
		return UniBpmAnalyzer.AnalyzeBpmWithoutLogging(audioSource.clip);
	}

	void ResetAudio()
	{
		audioSource.Play();
		AudioStop();
	}

	public void AudioPlay()
	{
		outputDevice = new WaveOutEvent();
		string[] path =
			StandaloneFileBrowser.OpenFilePanel("음악 파일 불러오기", null, new[]{new ExtensionFilter("음악 파일", "mp3")}, false);
		audioFile = new AudioFileReader(@"C:\Users\asj02\Desktop\song\dsa.mp3");
		outputDevice.Init(audioFile);
		outputDevice.Play();
	}

	public void AudioStop()
	{
		outputDevice?.Stop();
		audioSource.Pause();
		audioSource.time = 0;
		songSlider.slider.value = 0;
		
		songTime.songTimeTextField.readOnly = false;
	}
}