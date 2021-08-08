using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public SongSlider songSlider;

    public void OnEnable()
    {
        LoadEvents.audioLoadComplete += ResetAudio;
    }

    public void OnDisable()
    {
        LoadEvents.audioLoadComplete -= ResetAudio;
    }

    public float AnalyzeBpm()
    {
        return UniBpmAnalyzer.AnalyzeBpmWithoutLogging(audioSource.clip);
    }

    private void ResetAudio()
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