using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public SongSlider songSlider;

    public void Awake()
    {
        LoadEvents.audioLoadComplete += () =>
        {
            audioSource.Play();
            AudioStop();
        };
    }

    public void OnDisable()
    {
        LoadEvents.audioLoadComplete -= AudioStop;
    }

    public void AudioStop()
    {
        audioSource.Pause();
        audioSource.time = 0;
        songSlider.slider.value = 0;
    }
}
