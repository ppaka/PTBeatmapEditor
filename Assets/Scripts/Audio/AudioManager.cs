using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource audioSource;
	public SongSlider songSlider;

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

	public void AudioStop()
	{
		audioSource.Pause();
		audioSource.time = 0;
		songSlider.slider.value = 0;
	}
}