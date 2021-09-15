using System;
using System.Collections.Generic;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using OsuParsers.Enums.Beatmaps;
using UnityEngine;

public class OsuBeatmap : MonoBehaviour
{
    public static OsuBeatmap instance;

    void Awake()
    {
        instance = this;
    }

    public void PushTimings()
    {
        /*foreach (var t in LevelDataContainer.Instance.levelData.timings)
        {
            t.time += 8;
        }
        
        foreach (var t in LevelDataContainer.Instance.levelData.events)
        {
            t.time += 8;
        }
        
        foreach (var t in LevelDataContainer.Instance.levelData.notes)
        {
            t.time += 8;
            if (t.type == NoteType.Chain || t.type == NoteType.Hold)
            {
                t.endTime += 8;
            }
        }*/


        for (var i = 0; i < LevelDataContainer.Instance.levelData.events.Count; i++)
        {
            var t = LevelDataContainer.Instance.levelData.events[i];
            
            if (t.type == "move2d")
            {
                if (t.target == "game1")
                {
                    if (t.position[1] == 453.5f)
                    {
                        var n = new Events()
                        {
                            time = t.time,
                            type = "move2d",
                            transformMode = "local",
                            position = new[] { -251.0f, 617.0f },
                            target = "charObj",
                            duration = 1.0f,
                            ease = "oQ",
                            tweenId = ""
                        };
                        LevelDataContainer.Instance.levelData.events.Add(n);
                    }
                    else if (t.position[1] == 253.5f)
                    {
                        var n = new Events()
                        {
                            time = t.time,
                            type = "move2d",
                            transformMode = "local",
                            position = new[] { -251.0f, 417.0f },
                            target = "charObj",
                            duration = 1.0f,
                            ease = "oQ",
                            tweenId = ""
                        };
                        LevelDataContainer.Instance.levelData.events.Add(n);
                    }
                }
            }
        }
    }

    public void Import(string path)
    {
        Beatmap beatmap = BeatmapDecoder.Decode(path);

        var offset = LevelDataContainer.Instance.levelData.timings[0];

        LevelDataContainer.Instance.levelData.timings = new List<Timings>();
        LevelDataContainer.Instance.levelData.notes = new List<Notes>();

        foreach (var point in beatmap.TimingPoints)
        {
            if (point.BeatLength != -100)
            {
                double bpm = 0;
                string signature = "";
                
                if (point.TimeSignature == TimeSignature.SimpleQuadruple)
                {
                    bpm = 60 / point.BeatLength * (4 / 4) * 1000;
                    signature = "4/4";
                }
                else if (point.TimeSignature == TimeSignature.SimpleTriple)
                {
                    bpm = 60 / point.BeatLength * (3 / 4) * 1000;
                    signature = "3/4";
                }
                
                Timings timings = new Timings { beat = signature, bpm = bpm, time = point.Offset};
                LevelDataContainer.Instance.levelData.timings.Add(timings);
            }
        }

        /*var timingDifferent = LevelDataContainer.Instance.levelData.timings[0].time - offset.time;
        foreach (var evt in LevelDataContainer.Instance.levelData.events)
        {
            evt.time += timingDifferent;
        }*/

        for (int i = 0; i < beatmap.HitObjects.Count; i++)
        {
            NoteType type;
            
            if (beatmap.HitObjects[i].Position.X == 0)
            {
                type = NoteType.Normal;
                
                var note = new Notes
                {
                    customCurveTag = null, duration = 2, ease = "L", time = beatmap.HitObjects[i].StartTime,
                    hitSoundTag = "h1",
                    endTime = null, type = type, noteNum = (uint)(i + 1), splitEase = 1, gameBarTag = new []{"game1"}
                };
                LevelDataContainer.Instance.levelData.notes.Add(note);
            }
            else if (beatmap.HitObjects[i].Position.X == 192)
            {
                type = NoteType.Chain;
                
                var note = new Notes
                {
                    customCurveTag = null, duration = 2, ease = "L", time = beatmap.HitObjects[i].StartTime,
                    hitSoundTag = "h1",
                    endTime = beatmap.HitObjects[i].EndTime, type = type, noteNum = (uint)(i + 1), splitEase = 1, gameBarTag = new []{"game1"}
                };
                LevelDataContainer.Instance.levelData.notes.Add(note);
            }
            else if (beatmap.HitObjects[i].Position.X == 320)
            {
                type = NoteType.Flick;
                
                var note = new Notes
                {
                    customCurveTag = null, duration = 2, ease = "L", time = beatmap.HitObjects[i].StartTime,
                    hitSoundTag = "h1",
                    endTime = null, type = type, noteNum = (uint)(i + 1), splitEase = 1, gameBarTag = new []{"game1"}
                };
                LevelDataContainer.Instance.levelData.notes.Add(note);
            }
            else if (beatmap.HitObjects[i].Position.X == 448)
            {
                var charAnim = new Events
                {
                    time = beatmap.HitObjects[i].StartTime,
                    animType = "blinking1",
                    type = "playCharAnim",
                    parsedTime = new[]
                    {
                        0.0f, 0.2f, 0.4f, 0.6f
                    },
                    target = "charObj",
                    tweenId = ""
                };
                
                LevelDataContainer.Instance.levelData.events.Add(charAnim);
            }
        }
        
        ListMaker.instance.MakeLists();
    }
}