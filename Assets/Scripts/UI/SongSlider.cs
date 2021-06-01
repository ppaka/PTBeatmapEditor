using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public AudioSource audioSource;
    public Slider slider;
    public SongTime songTime;
    private bool _dragging;
    private AudioClip _clip;

    private void OnEnable()
    {
        global::LoadEvents.audioLoadComplete += LoadEvents;
    }

    private void OnDisable()
    {
        global::LoadEvents.audioLoadComplete -= LoadEvents;
    }

    private void LoadEvents()
    {
        slider.interactable = true;
        _clip = audioSource.clip;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        songTime.changed = true;
        _dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!slider.interactable) return;
        var value = Mathf.Clamp(slider.value * _clip.length, 0f, _clip.length);
        songTime.UpdateTime(value);
        
        songTime.changed = false;
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

    public void OnDrag(PointerEventData eventData)
    {
        if (!slider.interactable) return;
        songTime.changed = true;
        _dragging = true;
        var value = Mathf.Clamp(slider.value * _clip.length, 0, _clip.length);
        songTime.UpdateTime(value);
    }
}