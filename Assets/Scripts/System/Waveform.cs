using UnityEngine;

public class Waveform : MonoBehaviour
{
	public static float[] GetWaveform(AudioClip audio, int size, float sat)
	{
		float[] samples = new float[audio.channels * audio.samples];
		float[] waveform = new float[size];
		audio.GetData(samples, 0);
		int packSize = audio.samples * audio.channels / size;
		float max = 0f;
		int c = 0;
		int s = 0;

		for (int i = 0; i < audio.channels * audio.samples; i++)
		{
			waveform[c] += Mathf.Abs(samples[i]);
			s++;

			if (s <= packSize) continue;
			if (max < waveform[c])
				max = waveform[c];
			c++;
			s = 0;
		}

		for (int i = 0; i < size; i++)
		{
			waveform[i] /= max * sat;
			if (waveform[i] > 1f)
				waveform[i] = 1f;
		}

		return waveform;
	}

	public static Texture2D PaintWaveformSpectrum(AudioClip audio, int width, int height)
	{
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
		float[] samples = new float[audio.samples * audio.channels];
		float[] waveform = new float[width];
		audio.GetData(samples, 0);
		int packSize = samples.Length / width + 1;
		int s = 0;

		for (int i = 0; i < samples.Length; i += packSize)
		{
			waveform[s] = Mathf.Abs(samples[i]);
			s++;
		}

		for (int x = 0; x < width; x++)
		for (int y = 0; y < height; y++)
			tex.SetPixel(x, y, Color.clear);

		for (int x = 0; x < waveform.Length; x++)
		for (int y = 0; y <= waveform[x] * (height * .75f); y++)
		{
			tex.SetPixel(x, height / 2 + y, Color.white);
			tex.SetPixel(x, height / 2 - y, Color.white);
		}

		tex.filterMode = FilterMode.Trilinear;
		tex.Apply();

		return tex;
	}
}