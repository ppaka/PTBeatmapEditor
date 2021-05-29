using System;
using UnityEngine;

public static class LoadEvents
{
    public static Action AudioLoadComplete;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public SongSlider songSlider;
    
    public void AudioStop()
    {
        audioSource.Stop();
        audioSource.time = 0;
        songSlider.slider.value = 0;
    }
}
