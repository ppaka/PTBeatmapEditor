using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveformControl : MonoBehaviour
{
    [HideInInspector] public int width;
    [HideInInspector] public int height = 150;
    public Color waveformColor;
    [HideInInspector] public float sat = 0.5f;

    public Image img;
    private AudioSource _audio;

    public Action AudioLoadComplete;

    private void Start()
    {
        AudioLoadComplete += UpdateWaveform;
        _audio = FindObjectOfType<AudioSource>();
    }

    public void UpdateWaveform()
    {
        if (_audio.clip != null)
        {
            width = 4096;
            var texture = Waveform.PaintWaveformSpectrum(_audio.clip, sat, width, height, waveformColor);
            img.overrideSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }
    }
}