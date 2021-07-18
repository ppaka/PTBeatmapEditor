using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public Settings settings;
    public List<Objects> objects = new List<Objects>();
    public List<Curves> curves = new List<Curves>();
    public List<Events> events = new List<Events>();
    public List<NoteEvents> noteEvents = new List<NoteEvents>();
    public List<Notes> notes = new List<Notes>();
}

[Serializable]
public class Events
{
    public int time;
    public string type;

    public int? perfectEffect;
    public int? goodEffect;
    public int? missEffect;

    public string setValueType;
    public string transformMode;
    public float[] position;

    public float[] scale;

    public bool? turnOn;
    public float? speed;

    public string killType;

    public float? strength;
    public int? vibrato;
    public bool? fadeout;

    public string target;
    public string color;
    public float? duration;
    public string ease;
    public string customCurveTag;
    public string tweenId;
}

[Serializable]
public class Notes
{
    public uint noteNum;
    public int time;
    public float duration;
    public string type;
    public string ease;
    public string customCurveTag;

    public int? endTime;
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

    public float[] position;

    public float[] scale;

    public bool? turnOn;
    public float? speed;

    public string target;
    public string color;
    public float? duration;
    public string ease;
    public string customCurveTag;
    public string tweenId;

    public string setValueType;
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
    public string songFilename = "";
    public uint volume = 100;
    public uint pitch = 100;
    public uint songPreviewStart;
    public uint songPreviewLength;
    public int songStartDelay;
    public int noteOffset;
    public int eventOffset;
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
    public float[] position;
    public float[] scale;
    public float[] rotation;
    public int? depth;
    public bool? behindButtons;
    public bool? visible;

    public string imageType;
    public float? pixelPerUnitMultiplier;
}

[Serializable]
public class Curves
{
    public string tag;
    public AnimationCurve data;
}