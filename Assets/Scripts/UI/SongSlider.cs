using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioSource audioSource;
    public Slider slider;
    private bool _dragging;

    private void OnEnable()
    {
        global::LoadEvents.AudioLoadComplete += () =>
        {
            LoadEvents();
        };
    }

    private void OnDisable()
    {
        global::LoadEvents.AudioLoadComplete -= () =>
        {
            LoadEvents();
        };
    }

    private void LoadEvents()
    {
        slider.interactable = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var clip = audioSource.clip;
        audioSource.time = Mathf.Clamp(slider.value * clip.length, 0f, clip.length);
        _dragging = false;
    }

    private void Update()
    {
        switch (_dragging)
        {
            case false when audioSource.isPlaying:
                slider.value = audioSource.time / audioSource.clip.length;
                break;
            case false when !audioSource.isPlaying && audioSource.time != 0:
                break;
        }
    }
}