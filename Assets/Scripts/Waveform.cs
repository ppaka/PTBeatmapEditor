using UnityEngine;

public class Waveform : MonoBehaviour
{
    public static float[] GetWaveform(AudioClip audio, int size, float sat)
    {
        var samples = new float[audio.channels * audio.samples];
        var waveform = new float[size];
        audio.GetData(samples, 0);
        var packSize = audio.samples * audio.channels / size;
        var max = 0f;
        var c = 0;
        var s = 0;

        for (var i = 0; i < audio.channels * audio.samples; i++)
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
            waveform[i] /= (max * sat);
            if (waveform[i] > 1f)
                waveform[i] = 1f;
        }

        return waveform;
    }

    public static Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col)
    {
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var samples = new float[audio.samples * audio.channels];
        var waveform = new float[width];
        audio.GetData(samples, 0);
        var packSize = (samples.Length / width) + 1;
        var s = 0;

        for (var i = 0; i < samples.Length; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }

        for (var x = 0; x < waveform.Length; x++)
        {
            for (var y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();

        return tex;
    }
}
