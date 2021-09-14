using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[Serializable]
public class LevelData
{
    public Settings settings;
    public List<Objects> objects = new List<Objects>();
    public List<Curves> curves = new List<Curves>();
    public List<Timings> timings = new List<Timings>();
    public List<Events> events = new List<Events>();
    public List<NoteEvents> noteEvents = new List<NoteEvents>();
    public List<Notes> notes = new List<Notes>();
}

[Serializable]
public class Timings
{
    public int time;
    public double bpm;
    public string beat;
}

[Serializable]
public class Events
{
    public int time;
    public string type;

    public int? perfectEffect;
    public int? goodEffect;
    public int? missEffect;

    // 포스트 프로세싱
    public string ppType;
    public float? ppIntensity;
    public float? ppThreshold;
    public float? ppPostExposure;
    public float? ppContrast;
    public float? ppHueShift;
    public float? ppSaturation;
    public string ppFilmGrainType;
    public float? ppXMultiplier;
    public float? ppYMultiplier;
    public float[] ppCenter;
    public string ppColor;
    public float? ppTint;
    public float? ppTemperature;
    public float? ppScale;
    public float? ppSmoothness;
    public bool? ppRounded;
    public float? ppFocusDistance;
    public float? ppFocalLength;

    public string transformMode;
    public float[] position;
    public float[] rotation;
    public float[] scale;

    public bool? turnOn;
    public float? speed;

    public string killType;

    public float? strength;
    public int? vibrato;
    public bool? fadeout;

    public float? jumpPower;
    public int? jumps;

    public string animType;
    public float[] parsedTime;

    public string target;
    public string color;
    public float? duration;
    public string ease;
    public string customCurveTag;
    public string tweenId;
}

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum NoteType
{
    Normal = 0,
    Flick = 1,
    Chain = 2,
    Hold = 3
}

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum EventTypes
{
    killTween,
    postProcessing,
    accuracyEffect,
    setParticle,
    setParticleColor,
    clearParticle,
    setColor,
    setScreenColor,
    move2d,
    jump2d,
    rotate,
    scale2d,
    shake2d,
    playCharAnim
}

[Serializable]
public class Notes
{
    public uint noteNum;
    public int time;
    public float duration;
    public NoteType type;
    public string ease;
    public string customCurveTag;
    public string[] gameBarTag;
    public float? splitEase;

    public int? endTime;
    public string hitSoundTag;
}

[Serializable]
public class NoteEvents
{
    public uint noteNum;
    public string accuracy;
    public string type;

    public int? perfectEffect;
    public int? goodEffect;
    public int? missEffect;

    // 포스트 프로세싱
    public string ppType;
    public float? ppIntensity;
    public float? ppThreshold;
    public float? ppPostExposure;
    public float? ppContrast;
    public float? ppHueShift;
    public float? ppSaturation;
    public string ppFilmGrainType;
    public float? ppXMultiplier;
    public float? ppYMultiplier;
    public float[] ppCenter;
    public string ppColor;
    public float? ppTint;
    public float? ppTemperature;
    public float? ppScale;
    public float? ppSmoothness;
    public bool? ppRounded;
    public float? ppFocusDistance;
    public float? ppFocalLength;

    public float[] position;
    public float[] rotation;
    public float[] scale;

    public bool? turnOn;
    public float? speed;

    public string target;
    public string color;
    public float? duration;
    public string ease;
    public string customCurveTag;
    public string tweenId;

    public string transformMode;

    public string killType;

    public float? strength;
    public int? vibrato;
    public bool? fadeout;
}

[Serializable]
public class Settings
{
    public uint version = 1;
    public string title = "";
    public string artist = "";
    public string author = "";
    public uint difficulty = 1;
    public uint accurateDifficulty = 1;
    public string songFilename = "";
    public uint volume = 100;
    public uint pitch = 100;
    public uint songPreviewStart;
    public uint songPreviewEnd;
    public string bgFilename = "";
    public string bgType = "";
    public string bgColor = "";
    public float bgPixelPerUnitMultiplier = 1;
    public float bgDimMultiplier = 0.5f;
    public int perfectAccuracyEffect = 1;
    public int goodAccuracyEffect = 1;
    public int missAccuracyEffect = 1;
    public float accuracyEffectDuration = 1;
}

[Serializable]
public class Objects
{
    public string type;
    public string tag;

    public string particleType;

    public string filename;
    public string color;
    public float? fade;
    public float[] position;
    public float[] scale;
    public float[] rotation;
    public int? depth;
    public ShowOnCanvas? behindButtons;
    public bool? visible;

    public string imageType;
    public float? pixelPerUnitMultiplier;
}

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum ShowOnCanvas
{
    Normal,
    GameBar,
    Button
}

[Serializable]
public class Curves
{
    public string tag;
    public AnimationCurve data;
}