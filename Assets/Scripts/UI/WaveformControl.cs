using UnityEngine;
using UnityEngine.UI;

public class WaveformControl : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height = 150;
    [SerializeField] private Color waveformColor;
    [SerializeField] private Image img;
    private AudioSource _audio;

    private void Start()
    {
        LoadEvents.audioLoadComplete += UpdateWaveform;
        _audio = FindObjectOfType<AudioSource>();
    }

    public void UpdateWaveform()
    {
        if (_audio.clip != null)
        {
            width = 4096;
            var texture = Waveform.PaintWaveformSpectrum(_audio.clip, width, height);
            img.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            img.color = waveformColor;
        }
    }
}