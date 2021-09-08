using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// ReSharper disable SuggestVarOrType_SimpleTypes

[Serializable]
public class AccuracyEffect
{
    public int perfectEffect;
    public int goodEffect;
    public int missEffect;
    public float duration;
}

public class LevelEventManager : MonoBehaviour
{
    [Header("Scripts", order = 4)] public LevelObjectManager levelObjectManager;
    public AccuracyEffect accuracyEffects;
    public EffectScript effectScript;
    public SongTime songTime;

    float _delay;
    List<Events> _eventsList;
    List<Curves> _curvesList;

    private Dictionary<string, List<int>> particles = new Dictionary<string, List<int>>();

    public Dictionary<string, AnimationCurve> CurveDictionary = new Dictionary<string, AnimationCurve>();
    private Sequence eventSequence;

    void Update()
    {
        if (songTime.audioSource.isPlaying)
        {
            eventSequence.GotoWithCallbacks(songTime.audioSource.time, false);
        }
    }

    public void Load()
    {
        _eventsList = LevelDataContainer.Instance.levelData.events;
        _curvesList = LevelDataContainer.Instance.levelData.curves;
        var settings = LevelDataContainer.Instance.levelData.settings;

        foreach (var curve in _curvesList) CurveDictionary.Add(curve.tag, curve.data);

        (accuracyEffects.perfectEffect, accuracyEffects.goodEffect, accuracyEffects.missEffect,
            accuracyEffects.duration) = (settings.perfectAccuracyEffect, settings.goodAccuracyEffect,
            settings.missAccuracyEffect, settings.accuracyEffectDuration);
        Add();
    }

    void Add()
    {
        if (_eventsList.Count == 0) return;
        eventSequence = DOTween.Sequence().SetAutoKill(false).SetRecyclable(true).Pause();
        foreach (var data in _eventsList)
        {
            //data.time * 0.001f + _delay

            switch (data.type)
            {
                case "killTween":
                {
                    KillTweenType type = data.killType switch
                    {
                        "DoNotKill" => KillTweenType.DoNotKill,
                        "KillAndComplete" => KillTweenType.KillAndComplete,
                        "KillButDoNotComplete" => KillTweenType.KillButDoNotComplete,
                        _ => KillTweenType.DoNotKill
                    };
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        effectScript.FindAndKillTween(data.tweenId, type).Play());
                    break;
                }
                case "postProcessing":
                {
                    ColorUtility.TryParseHtmlString("#" + data.ppColor, out var color);
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        effectScript.ActivatePostProcessing(data.ppType, (bool)data.turnOn).Play());
                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.SetPostProcessing(data.ppType,
                        data.ppIntensity, data.ppThreshold, data.ppContrast,
                        color,
                        data.ppHueShift, data.ppSaturation, data.ppXMultiplier, data.ppYMultiplier, data.ppCenter,
                        data.ppScale, data.ppSmoothness, data.ppRounded, data.ppTemperature, data.ppTint,
                        data.ppFocusDistance, data.ppFocalLength, data.ppFilmGrainType, (float)data.duration, data.ease,
                        data.customCurveTag, data.tweenId).Play());
                    break;
                }
                case "accuracyEffect":
                {
                    /*(accuracyEffects.perfectEffect, accuracyEffects.goodEffect, accuracyEffects.missEffect,
                        accuracyEffects.duration) = ((int)data.perfectEffect, (int)data.goodEffect,
                        (int)data.missEffect,
                        (float)data.duration);*/
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        DOTween.To(() => accuracyEffects.perfectEffect, value => accuracyEffects.perfectEffect = value,
                            (int)data.perfectEffect, 0.001f));
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        DOTween.To(() => accuracyEffects.goodEffect, value => accuracyEffects.goodEffect = value,
                            (int)data.goodEffect, 0.001f));
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        DOTween.To(() => accuracyEffects.missEffect, value => accuracyEffects.missEffect = value,
                            (int)data.missEffect, 0.001f));
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        DOTween.To(() => accuracyEffects.duration, value => accuracyEffects.duration = value,
                            (int)data.duration, 0.001f));
                    break;
                }
                case "setParticle":
                {
                    int enable = 0;

                    eventSequence.Insert(data.time * 0.001f + _delay, DOTween.To(() => enable, value => enable = value,
                        (bool)data.turnOn ? 1 : 0, 0.001f).OnUpdate(() =>
                    {
                        effectScript.SetParticle(levelObjectManager.Obj[data.target], enable,
                            data.speed ?? 1);
                    }));

                    /*eventSequence.InsertCallback(data.time * 0.001f + _delay, () =>
                        effectScript.SetParticle(levelObjectManager.Obj[data.target], (bool)data.turnOn,
                            data.speed ?? 1));*/
                    break;
                }
                case "setParticleColor":
                {
                    ColorUtility.TryParseHtmlString("#" + data.color, out var color);
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        effectScript.SetParticleColor(levelObjectManager.Obj[data.target], color, (float)data.duration,
                            data.ease, data.customCurveTag, data.tweenId));
                    break;
                }
                case "clearParticle":
                {
                    eventSequence.Insert(data.time * 0.001f + _delay,
                        effectScript.ClearParticle(levelObjectManager.Obj[data.target]));
                    break;
                }
                case "setColor":
                {
                    ColorUtility.TryParseHtmlString("#" + data.color, out var color);
                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.SetColor(
                        levelObjectManager.Obj[data.target], color, (float)data.duration, data.ease,
                        data.tweenId));
                    break;
                }
                case "setScreenColor":
                {
                    ColorUtility.TryParseHtmlString("#" + data.color, out var color);
                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.SetScreenColor(
                        levelObjectManager.Obj[data.target], color, (float)data.duration,
                        data.ease,
                        data.tweenId));
                    break;
                }
                case "move2d":
                {
                    var vector = new Vector2(data.position[0], data.position[1]);
                    TransformRelativeMode relativeMode = data.transformMode == "global"
                        ? TransformRelativeMode.Global
                        : TransformRelativeMode.Local;

                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.MoveRect(
                        levelObjectManager.Obj[data.target], vector, (float)data.duration, data.ease,
                        data.customCurveTag, relativeMode, data.tweenId).Play());
                    break;
                }
                case "rotate":
                {
                    var vector = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
                    TransformRelativeMode relativeMode = data.transformMode == "global"
                        ? TransformRelativeMode.Global
                        : TransformRelativeMode.Local;

                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.RotateRect(
                        levelObjectManager.Obj[data.target], vector, (float)data.duration,
                        data.ease,
                        data.customCurveTag, relativeMode, data.tweenId).Play());
                    break;
                }
                case "scale2d":
                {
                    var vector = new Vector2(data.scale[0], data.scale[1]);

                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.ScaleRect(
                        levelObjectManager.Obj[data.target], vector, (float)data.duration, data.ease,
                        data.customCurveTag, data.tweenId).Play());
                    break;
                }
                case "shake2d":
                {
                    eventSequence.Insert(data.time * 0.001f + _delay, effectScript.SetShake(
                        levelObjectManager.Obj[data.target], (float)data.duration,
                        (float)data.strength,
                        (int)data.vibrato, (bool)data.fadeout, data.tweenId).Play());
                    break;
                }
            }
        }
    }

    public void NoteEvents(IEnumerable<NoteEvents> events)
    {
        var seq = DOTween.Sequence().Pause();
        foreach (var data in events)
        {
            switch (data.type)
            {
                case "killTween":
                {
                    KillTweenType type = data.killType switch
                    {
                        "DoNotKill" => KillTweenType.DoNotKill,
                        "KillAndComplete" => KillTweenType.KillAndComplete,
                        "KillButDoNotComplete" => KillTweenType.KillButDoNotComplete,
                        _ => KillTweenType.DoNotKill
                    };
                    seq.Insert(0, effectScript.FindAndKillTween(data.tweenId, type).Play());
                    break;
                }
                case "postProcessing":
                {
                    ColorUtility.TryParseHtmlString("#" + data.ppColor, out var color);
                    seq.Insert(0, effectScript.ActivatePostProcessing(data.ppType, (bool)data.turnOn).Play());
                    seq.Insert(0, effectScript.SetPostProcessing(data.ppType, data.ppIntensity,
                        data.ppThreshold, data.ppContrast,
                        color,
                        data.ppHueShift, data.ppSaturation, data.ppXMultiplier, data.ppYMultiplier, data.ppCenter,
                        data.ppScale, data.ppSmoothness, data.ppRounded, data.ppTemperature, data.ppTint,
                        data.ppFocusDistance, data.ppFocalLength, data.ppFilmGrainType, (float)data.duration, data.ease,
                        data.customCurveTag, data.tweenId).Play());
                    break;
                }
                case "accuracyEffect":
                {
                    seq.InsertCallback(0, () =>
                    {
                        (accuracyEffects.perfectEffect, accuracyEffects.goodEffect, accuracyEffects.missEffect,
                            accuracyEffects.duration) = ((int)data.perfectEffect, (int)data.goodEffect,
                            (int)data.missEffect,
                            (float)data.duration);
                    });
                    break;
                }
                case "setParticle":
                {
                    int enable = 0;
                    if ((bool)data.turnOn) enable = 1;

                    eventSequence.InsertCallback(0, () =>
                        effectScript.SetParticle(levelObjectManager.Obj[data.target], enable,
                            data.speed ?? 1));
                    break;
                }
                case "setParticleColor":
                {
                    ColorUtility.TryParseHtmlString("#" + data.color, out var color);
                    seq.Insert(0,
                        effectScript.SetParticleColor(levelObjectManager.Obj[data.target], color, (float)data.duration,
                            data.ease, data.customCurveTag, data.tweenId).Play());
                    break;
                }
                case "clearParticle":
                {
                    seq.Insert(0,
                        effectScript.ClearParticle(levelObjectManager.Obj[data.target]).Play());
                    break;
                }
                case "setColor":
                {
                    ColorUtility.TryParseHtmlString("#" + data.color, out var color);
                    seq.Insert(0, effectScript.SetColor(levelObjectManager.Obj[data.target], color,
                        (float)data.duration, data.ease,
                        data.tweenId).Play());
                    break;
                }
                case "setLightColor":
                {
                    ColorUtility.TryParseHtmlString("#" + data.color, out var color);
                    seq.Insert(0, effectScript.SetScreenColor(levelObjectManager.Obj[data.target], color,
                        (float)data.duration,
                        data.ease,
                        data.tweenId).Play());
                    break;
                }
                case "move2d":
                {
                    var vector = new Vector2(data.position[0], data.position[1]);
                    TransformRelativeMode relativeMode = data.transformMode == "global"
                        ? TransformRelativeMode.Global
                        : TransformRelativeMode.Local;
                    seq.Insert(0, effectScript.MoveRect(levelObjectManager.Obj[data.target], vector,
                        (float)data.duration, data.ease,
                        data.customCurveTag, relativeMode, data.tweenId).Play());
                    break;
                }
                case "rotate":
                {
                    var vector = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
                    TransformRelativeMode relativeMode = data.transformMode == "global"
                        ? TransformRelativeMode.Global
                        : TransformRelativeMode.Local;
                    seq.Insert(0, effectScript.RotateRect(levelObjectManager.Obj[data.target], vector,
                        (float)data.duration,
                        data.ease,
                        data.customCurveTag, relativeMode, data.tweenId).Play());
                    break;
                }
                case "scale2d":
                {
                    var vector = new Vector2(data.scale[0], data.scale[1]);
                    seq.Insert(0, effectScript.ScaleRect(levelObjectManager.Obj[data.target], vector,
                        (float)data.duration, data.ease,
                        data.customCurveTag, data.tweenId).Play());
                    break;
                }
                case "shake2d":
                {
                    seq.Insert(0, effectScript.SetShake(levelObjectManager.Obj[data.target],
                        (float)data.duration,
                        (float)data.strength,
                        (int)data.vibrato, (bool)data.fadeout, data.tweenId).Play());
                    break;
                }
            }
        }

        seq.Play();
    }
}