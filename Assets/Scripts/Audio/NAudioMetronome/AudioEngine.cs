using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using UnityEngine;

namespace NAudioMetronome
{
    class AudioEngine
    {
        public WaveOut outputDevice;
        private readonly MixingSampleProvider mixer;

        private string accentedBeatPath = Application.streamingAssetsPath + "/Sounds/snare.wav";
        private string normalBeatPath = Application.streamingAssetsPath + "/Sounds/hi-hat.wav";

        PatternEngine patternEngine = new PatternEngine();
        SampleSource Pattern { get; set; }

        public AudioEngine(int sampleRate = 44100, int channelCount = 2)
        {
            patternEngine.AccentedBeat = new SampleSource(accentedBeatPath);
            patternEngine.NormalBeat = new SampleSource(normalBeatPath);
            Pattern = patternEngine.CreatePattern(120, 4);

            outputDevice = new WaveOut();
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            
            outputDevice.Init(mixer);
        }

        public void Play(int bpm, int measure, double offset = 0)
        {
            Pattern = patternEngine.CreatePattern(bpm, measure);
            mixer.RemoveAllMixerInputs();
            mixer.AddMixerInput(new OffsetSampleProvider(new SampleSourceProvider(Pattern))
            {
                DelayBy = TimeSpan.FromSeconds(offset)
            });
            outputDevice.Play();
        }

        public void Stop()
        {
            mixer.RemoveAllMixerInputs();
            outputDevice.Stop();
        }

        public void Update(int bpm, int measure, double offset = 0)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Stop();
                Play(bpm, measure, offset);
            }
            else
            {
                Pattern = patternEngine.CreatePattern(bpm, measure);
                mixer.RemoveAllMixerInputs();
                mixer.AddMixerInput(new OffsetSampleProvider(new SampleSourceProvider(Pattern))
                {
                    DelayBy = TimeSpan.FromSeconds(offset)
                });
            }
        }
    }
}