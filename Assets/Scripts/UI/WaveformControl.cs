using UnityEngine;
using UnityEngine.UI;

public class WaveformControl : MonoBehaviour
{
	[SerializeField] int width;
	[SerializeField] int height = 150;
	[SerializeField] Color waveformColor;
	[SerializeField] Image img;
	[SerializeField] AudioSource audioSource;

	void Start()
	{
		SystemEvents.audioLoadComplete += UpdateWaveform;
	}

	public void UpdateWaveform()
	{
		if (audioSource.clip != null)
		{
			width = 4096;
			Texture2D texture = Waveform.PaintWaveformSpectrum(audioSource.clip, width, height);
			img.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
				new Vector2(0.5f, 0.5f));
			img.sprite.texture.wrapMode = TextureWrapMode.Clamp;
			img.color = waveformColor;
		}
	}
}