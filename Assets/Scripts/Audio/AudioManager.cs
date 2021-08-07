using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public SongSlider songSlider;

    public void OnEnable()
    {
        LoadEvents.audioLoadComplete += ResetAudio;
        LoadEvents.audioLoadComplete += AnalyzeBpm;
    }

    public void OnDisable()
    {
        LoadEvents.audioLoadComplete -= ResetAudio;
        LoadEvents.audioLoadComplete -= AnalyzeBpm;
    }

    public void AnalyzeBpm()
    {
        Debug.Log(UniBpmAnalyzer.AnalyzeBpm(audioSource.clip));
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