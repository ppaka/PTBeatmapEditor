using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public enum KillTweenType
{
    DoNotKill,
    KillButDoNotComplete,
    KillAndComplete
}

public enum TransformRelativeMode
{
    Local,
    Global
}

public class EffectScript : MonoBehaviour
{
    public RectTransform noteEndTweenRect, shakeRect;
    public PostProcessVolume accuracyPpVolume, effectPpVolume;

    [Header("Scripts", order = 1)] [SerializeField]
    private LevelEventManager levelEventManager;

    private ChromaticAberration _accuracyChromaticAberration;
    private Vignette _accuracyVignette;
    private Bloom _bloom;
    private ChromaticAberration _chromaticAberration;
    //private ColorAdjustments _colorAdjustments;
    private DepthOfField _depthOfField;
    //private FilmGrain _filmGrain;
    private LensDistortion _lensDistortion;

    private MotionBlur _motionBlur;
    private Vignette _vignette;
    //private WhiteBalance _whiteBalance;

    private void Awake()
    {
        //GetVolumeData();
    }

    private void GetVolumeData()
    {
        effectPpVolume.profile.TryGetSettings(out _motionBlur);
        effectPpVolume.profile.TryGetSettings(out _bloom);
        effectPpVolume.profile.TryGetSettings(out _chromaticAberration);
        //effectPpVolume.profile.TryGetSettings(out _colorAdjustments);
        //effectPpVolume.profile.TryGetSettings(out _filmGrain);
        effectPpVolume.profile.TryGetSettings(out _lensDistortion);
        effectPpVolume.profile.TryGetSettings(out _vignette);
        //effectPpVolume.profile.TryGetSettings(out _whiteBalance);
        effectPpVolume.profile.TryGetSettings(out _depthOfField);

        accuracyPpVolume.profile.TryGetSettings(out _accuracyVignette);
        accuracyPpVolume.profile.TryGetSettings(out _accuracyChromaticAberration);
    }

    private AnimationCurve GetCurve(string curveTag)
    {
        return levelEventManager.CurveDictionary[curveTag];
    }

    public static Ease GetEase(string ease)
    {
        return ease switch
        {
            "L" => Ease.Linear,
            "iS" => Ease.InSine,
            "oS" => Ease.OutSine,
            "ioS" => Ease.InOutSine,
            "iQ" => Ease.InQuad,
            "oQ" => Ease.OutQuad,
            "ioQ" => Ease.InOutQuad,
            "iC" => Ease.InCubic,
            "oC" => Ease.OutCubic,
            "ioC" => Ease.InOutCubic,
            "iQa" => Ease.InQuart,
            "oQa" => Ease.OutQuart,
            "ioQa" => Ease.InOutQuart,
            "iQi" => Ease.InQuint,
            "oQi" => Ease.OutQuint,
            "ioQi" => Ease.InOutQuint,
            "iE" => Ease.InExpo,
            "oE" => Ease.OutExpo,
            "ioE" => Ease.InOutExpo,
            "iCi" => Ease.InCirc,
            "oCi" => Ease.OutCirc,
            "ioCi" => Ease.InOutCirc,
            "iB" => Ease.InBack,
            "oB" => Ease.OutBack,
            "ioB" => Ease.InOutBack,
            "iEl" => Ease.InElastic,
            "oEl" => Ease.OutElastic,
            "ioEl" => Ease.InOutElastic,
            "iBo" => Ease.InBounce,
            "oBo" => Ease.OutBounce,
            "ioBo" => Ease.InOutBounce,
            _ => Ease.Unset
        };
    }

    public Tween FindAndKillTween(string id, KillTweenType killType = KillTweenType.KillButDoNotComplete)
    {
        switch (killType)
        {
            case KillTweenType.DoNotKill:
                break;
            case KillTweenType.KillAndComplete:
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => DOTween.Kill(id, true));
                return seq;
            }
            case KillTweenType.KillButDoNotComplete:
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => DOTween.Kill(id));
                return seq;
            }
        }

        return null;
    }

    public Tween ActivatePostProcessing(string type, bool active)
    {
        switch (type)
        {
            case "motionBlur":
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _motionBlur.active = active);
                return seq;
            }
            case "bloom":
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _bloom.active = active);
                return seq;
            }
            case "chromaticAberration":
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _chromaticAberration.active = active);
                return seq;
            }
            case "colorAdjustments":
            {
                /*var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _colorAdjustments.active = active);
                return seq;*/
                return null;
            }
            case "filmGrain":
            {
                /*var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _filmGrain.active = active);
                return seq;*/
                return null;
            }
            case "lensDistortion":
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _lensDistortion.active = active);
                return seq;
            }
            case "vignette":
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _vignette.active = active);
                return seq;
            }
            case "whiteBalance":
            {
                /*var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _whiteBalance.active = active);
                return seq;*/
                return null;
            }
            case "depthOfField":
            {
                var seq = DOTween.Sequence()
                    .InsertCallback(0, () => _depthOfField.active = active);
                return seq;
            }
        }

        return null;
    }

    public Tween SetPostProcessing(string type, float? intensity, float? threshold, float? contrast, Color? color,
        float? hueShift, float? saturation, float? xMultiplier, float? yMultiplier, float[] center, float? scale,
        float? smoothness, bool? rounded, float? temperature, float? tint, float? focusDistance, float? focalLength,
        string filmGrainType, float duration, string ease, string curveTag = "", string tweenId = "")
    {
        switch (type)
        {
            case "motionBlur" when ease == "custom":
                return DOTween.To(() => _motionBlur.shutterAngle.value, x => _motionBlur.shutterAngle.value = x,
                    (float)intensity,
                    duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
            case "motionBlur":
                return DOTween.To(() => _motionBlur.shutterAngle.value, x => _motionBlur.shutterAngle.value = x,
                    (float)intensity,
                    duration).SetEase(GetEase(ease)).SetId(tweenId);
            case "bloom" when ease == "custom":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _bloom.threshold.value, x => _bloom.threshold.value = x,
                            (float)threshold, duration)
                        .SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x,
                            (float)intensity, duration)
                        .SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _bloom.color.value, x => _bloom.color.value = x, (Color)color, duration)
                        .SetEase(GetCurve(curveTag)));
                return seq;
            }
            case "bloom":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _bloom.threshold.value, x => _bloom.threshold.value = x,
                            (float)threshold, duration)
                        .SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x,
                            (float)intensity, duration)
                        .SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _bloom.color.value, x => _bloom.color.value = x, (Color)color, duration)
                        .SetEase(GetEase(ease)));
                return seq;
            }
            case "chromaticAberration" when ease == "custom":
                return DOTween.To(() => _chromaticAberration.intensity.value,
                    x => _chromaticAberration.intensity.value = x,
                    (float)intensity, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
            case "chromaticAberration":
                return DOTween.To(() => _chromaticAberration.intensity.value,
                    x => _chromaticAberration.intensity.value = x,
                    (float)intensity, duration).SetEase(GetEase(ease)).SetId(tweenId);
            case "colorAdjustments" when ease == "custom":
            {
                /*var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _colorAdjustments.postExposure.value,
                        x => _colorAdjustments.postExposure.value = x,
                        (float)contrast, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _colorAdjustments.colorFilter.value,
                        x => _colorAdjustments.colorFilter.value = x,
                        (Color)color, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _colorAdjustments.hueShift.value,
                        x => _colorAdjustments.hueShift.value = x,
                        (float)hueShift, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _colorAdjustments.saturation.value,
                        x => _colorAdjustments.saturation.value = x,
                        (float)saturation, duration).SetEase(GetCurve(curveTag)));
                return seq;*/
                return null;
            }
            case "colorAdjustments":
            {
                /*var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _colorAdjustments.postExposure.value,
                        x => _colorAdjustments.postExposure.value = x,
                        (float)contrast, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _colorAdjustments.colorFilter.value,
                        x => _colorAdjustments.colorFilter.value = x,
                        (Color)color, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _colorAdjustments.hueShift.value,
                        x => _colorAdjustments.hueShift.value = x,
                        (float)hueShift, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _colorAdjustments.saturation.value,
                        x => _colorAdjustments.saturation.value = x,
                        (float)saturation, duration).SetEase(GetEase(ease)));
                return seq;*/
                return null;
            }
            case "filmGrain":
            {
                /*_filmGrain.type.value = filmGrainType switch
                {
                    "thin1" => FilmGrainLookup.Thin1,
                    "thin2" => FilmGrainLookup.Thin2,
                    "medium1" => FilmGrainLookup.Medium1,
                    "medium2" => FilmGrainLookup.Medium2,
                    "medium3" => FilmGrainLookup.Medium3,
                    "medium4" => FilmGrainLookup.Medium4,
                    "medium5" => FilmGrainLookup.Medium5,
                    "medium6" => FilmGrainLookup.Medium6,
                    "large01" => FilmGrainLookup.Large01,
                    "large02" => FilmGrainLookup.Large02,
                    _ => _filmGrain.type.value
                };
                if (ease == "custom")
                    return DOTween.To(() => _filmGrain.intensity.value, x => _filmGrain.intensity.value = x,
                        (float)intensity,
                        duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
                return DOTween.To(() => _filmGrain.intensity.value, x => _filmGrain.intensity.value = x,
                    (float)intensity,
                    duration).SetEase(GetEase(ease)).SetId(tweenId);*/
                return null;
            }
            case "lensDistortion" when ease == "custom":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _lensDistortion.intensity.value,
                        x => _lensDistortion.intensity.value = x,
                        (float)intensity, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _lensDistortion.intensityX.value,
                        x => _lensDistortion.intensityX.value = x,
                        (float)xMultiplier, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _lensDistortion.intensityY.value,
                        x => _lensDistortion.intensityY.value = x,
                        (float)yMultiplier, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _lensDistortion.centerX.value, x => _lensDistortion.centerX.value = x,
                        center[0], duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _lensDistortion.centerY.value, x => _lensDistortion.centerY.value = x,
                        center[1], duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _lensDistortion.scale.value, x => _lensDistortion.scale.value = x,
                        (float)scale,
                        duration).SetEase(GetCurve(curveTag)));
                return seq;
            }
            case "lensDistortion":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _lensDistortion.intensity.value,
                        x => _lensDistortion.intensity.value = x,
                        (float)intensity, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _lensDistortion.intensityX.value,
                        x => _lensDistortion.intensityX.value = x,
                        (float)xMultiplier, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _lensDistortion.intensityY.value,
                        x => _lensDistortion.intensityY.value = x,
                        (float)yMultiplier, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _lensDistortion.centerX.value, x => _lensDistortion.centerX.value = x,
                        center[0], duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _lensDistortion.centerY.value, x => _lensDistortion.centerY.value = x,
                        center[1], duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _lensDistortion.scale.value, x => _lensDistortion.scale.value = x,
                        (float)scale,
                        duration).SetEase(GetEase(ease)));
                return seq;
            }
            case "vignette" when ease == "custom":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _vignette.color.value, x => _vignette.color.value = x, (Color)color,
                            duration)
                        .SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _vignette.center.value, x => _vignette.center.value = x,
                        new Vector2(center[0], center[1]), duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x,
                        (float)intensity,
                        duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _vignette.smoothness.value, x => _vignette.smoothness.value = x,
                        (float)smoothness,
                        duration).SetEase(GetCurve(curveTag)))
                    .InsertCallback(0, () => _vignette.rounded.value = (bool)rounded);
                return seq;
            }
            case "vignette":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _vignette.color.value, x => _vignette.color.value = x, (Color)color,
                            duration)
                        .SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _vignette.center.value, x => _vignette.center.value = x,
                        new Vector2(center[0], center[1]), duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x,
                        (float)intensity,
                        duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _vignette.smoothness.value, x => _vignette.smoothness.value = x,
                        (float)smoothness,
                        duration).SetEase(GetEase(ease)))
                    .InsertCallback(0, () => _vignette.rounded.value = (bool)rounded);
                return seq;
            }
            case "whiteBalance" when ease == "custom":
            {
                /*var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _whiteBalance.temperature.value,
                        x => _whiteBalance.temperature.value = x,
                        (float)temperature, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _whiteBalance.tint.value, x => _whiteBalance.tint.value = x,
                            (float)tint, duration)
                        .SetEase(GetCurve(curveTag)));
                return seq;*/
                return null;
            }
            case "whiteBalance":
            {
                /*var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _whiteBalance.temperature.value,
                        x => _whiteBalance.temperature.value = x,
                        (float)temperature, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _whiteBalance.tint.value, x => _whiteBalance.tint.value = x,
                        (float)tint, duration).SetEase(GetEase(ease)));
                return seq;*/
                return null;
            }
            case "depthOfField" when ease == "custom":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _depthOfField.focusDistance.value,
                        x => _depthOfField.focusDistance.value = x,
                        (float)focusDistance, duration).SetEase(GetCurve(curveTag)))
                    .Insert(0, DOTween.To(() => _depthOfField.focalLength.value,
                        x => _depthOfField.focalLength.value = x,
                        (float)focalLength, duration).SetEase(GetCurve(curveTag)));
                return seq;
            }
            case "depthOfField":
            {
                var seq = DOTween.Sequence().SetId(tweenId)
                    .Insert(0, DOTween.To(() => _depthOfField.focusDistance.value,
                        x => _depthOfField.focusDistance.value = x,
                        (float)focusDistance, duration).SetEase(GetEase(ease)))
                    .Insert(0, DOTween.To(() => _depthOfField.focalLength.value,
                        x => _depthOfField.focalLength.value = x,
                        (float)focalLength, duration).SetEase(GetEase(ease)));
                return seq;
            }
        }

        return null;
    }

    public Tween MoveRect(GameObject obj, Vector2 pos, float duration, string ease, string curveTag = "",
        TransformRelativeMode relativeMode = TransformRelativeMode.Local, string tweenId = "")
    {
        var tf = obj.GetComponent<RectTransform>();

        if (ease.Equals("custom"))
        {
            if (relativeMode == TransformRelativeMode.Local)
                return tf.DOLocalMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetCurve(curveTag))
                    .SetId(tweenId);
            return tf.DOMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
        }

        if (relativeMode == TransformRelativeMode.Local)
            return tf.DOLocalMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetEase(ease)).SetId(tweenId);
        return tf.DOMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetEase(ease)).SetId(tweenId);
    }

    public Tween RotateRect(GameObject obj, Vector3 pos, float duration, string ease, string curveTag = "",
        TransformRelativeMode relativeMode = TransformRelativeMode.Local,
        string tweenId = "")
    {
        var tf = obj.GetComponent<RectTransform>();

        if (ease.Equals("custom"))
        {
            if (relativeMode == TransformRelativeMode.Local)
                return tf.DOLocalRotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetCurve(curveTag))
                    .SetId(tweenId);
            return tf.DORotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
        }

        if (relativeMode == TransformRelativeMode.Local)
            return tf.DOLocalRotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetEase(ease)).SetId(tweenId);
        return tf.DORotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetEase(ease)).SetId(tweenId);
    }

    public Tween ScaleRect(GameObject obj, Vector2 pos, float duration, string ease, string curveTag = "",
        string tweenId = "")
    {
        var tf = obj.GetComponent<RectTransform>();

        if (ease.Equals("custom"))
            return tf.DOScale(new Vector3(pos.x, pos.y, 1), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);

        return tf.DOScale(new Vector3(pos.x, pos.y, 1), duration).SetEase(GetEase(ease)).SetId(tweenId);
    }

    public Tween SetColor(GameObject obj, Color value, float duration, string ease, string curveTag = "",
        string tweenId = "")
    {
        var img = obj.GetComponent<Image>();

        if (ease.Equals("custom"))
            return img.DOColor(value, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
        return img.DOColor(value, duration).SetEase(GetEase(ease)).SetId(tweenId);
    }

    public Tween SetScreenColor(GameObject obj, Color value, float duration, string ease, string curveTag = "",
        string tweenId = "")
    {
        var image = obj.GetComponent<Image>();

        if (ease.Equals("custom"))
            return DOTween.To(() => image.color, x => image.color = x, value, duration).SetEase(GetCurve(curveTag))
                .SetId(tweenId);
        return DOTween.To(() => image.color, x => image.color = x, value, duration).SetEase(GetEase(ease))
            .SetId(tweenId);
    }

    public void SetParticle(GameObject obj, int play, float speed)
    {
        if (play == 1)
        {
            var particleObj = obj.GetComponent<ParticleSystem>();
            var particleMain = particleObj.main;
            if (speed < 0) speed = 1;
            particleMain.simulationSpeed = speed;

            particleObj.Play();
        }
        else
        {
            var particle = obj.GetComponent<ParticleSystem>();
            particle.Stop();
        }
    }

    public Tween SetParticleColor(GameObject obj, Color value, float duration, string ease, string curveTag = "",
        string tweenId = "")
    {
        var particle = obj.GetComponent<ParticleSystem>();

        if (ease.Equals("custom"))
        {
            var color = particle.main.startColor.color;
            return DOTween.To(() => color, x => color = x, value, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
        }
        else
        {
            var color = particle.main.startColor.color;
            return DOTween.To(() => color, x => color = x, value, duration).SetEase(GetEase(ease)).SetId(tweenId);
        }
    }

    public Tween ClearParticle(GameObject obj)
    {
        var particle = obj.GetComponent<ParticleSystem>();

        var seq = DOTween.Sequence()
            .InsertCallback(0, () => particle.Clear());
        return seq;
    }

    public Tween SetShake(GameObject obj, float duration, float strength, int vibrato, bool fadeOut,
        string tweenId = "")
    {
        var tf = obj.GetComponent<RectTransform>();

        return tf.DOShakePosition(duration, strength, vibrato, fadeOut: fadeOut).SetId(tweenId);
    }

    public void AccuracyEffect(string accuracy)
    {
        var accuracyEffect = levelEventManager.accuracyEffects;
        if (accuracy == "perfect")
        {
            if (accuracyEffect.perfectEffect == 2)
            {
                DOTween.Kill("vignetteAccu");
                DOTween.Kill("chromaticAberrationAccu");

                if (accuracyEffect.missEffect == 2) DOTween.Kill("shakeAccu", true);

                _accuracyVignette.color.value = Color.cyan;
                _accuracyVignette.intensity.value = 0.5f;
                _accuracyChromaticAberration.intensity.value = 0;

                DOTween.To(() => _accuracyVignette.intensity.value, x => _accuracyVignette.intensity.value = x, 0,
                    accuracyEffect.duration).SetId("vignetteAccu");
                DOTween.Punch(() => new Vector3(_accuracyChromaticAberration.intensity.value, 0, 0),
                        x => _accuracyChromaticAberration.intensity.value = x.x, new Vector3(0.7f, 0, 0),
                        accuracyEffect.duration, Mathf.FloorToInt(6 / accuracyEffect.duration), 0.1f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => _accuracyChromaticAberration.intensity.value = 0)
                    .SetId("chromaticAberrationAccu");
            }

            if (accuracyEffect.perfectEffect == 1)
            {
                DOTween.Kill("vignetteAccu");
                DOTween.Kill("chromaticAberrationAccu");
                DOTween.Kill("shakeAccu", true);


                _accuracyVignette.color.value = Color.cyan;
                _accuracyVignette.intensity.value = 0.5f;
                _accuracyChromaticAberration.intensity.value = 0;

                DOTween.To(() => _accuracyVignette.intensity.value, x => _accuracyVignette.intensity.value = x, 0,
                    accuracyEffect.duration).SetId("vignetteAccu");
            }
        }

        if (accuracy == "good")
        {
            if (accuracyEffect.goodEffect == 2)
            {
                DOTween.Kill("vignetteAccu");
                DOTween.Kill("chromaticAberrationAccu");
                DOTween.Kill("shakeAccu", true);

                _accuracyVignette.color.value = Color.yellow;
                _accuracyVignette.intensity.value = 0.5f;
                _accuracyChromaticAberration.intensity.value = 0;

                DOTween.To(() => _accuracyVignette.intensity.value, x => _accuracyVignette.intensity.value = x, 0,
                    accuracyEffect.duration).SetId("vignetteAccu");
                DOTween.Punch(() => new Vector3(_accuracyChromaticAberration.intensity.value, 0, 0),
                        x => _accuracyChromaticAberration.intensity.value = x.x, new Vector3(0.7f, 0, 0),
                        accuracyEffect.duration, Mathf.FloorToInt(6 / accuracyEffect.duration), 0.1f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => _accuracyChromaticAberration.intensity.value = 0)
                    .SetId("chromaticAberrationAccu");
            }

            if (accuracyEffect.goodEffect == 1)
            {
                DOTween.Kill("vignetteAccu");
                DOTween.Kill("chromaticAberrationAccu");
                DOTween.Kill("shakeAccu", true);

                _accuracyVignette.color.value = Color.yellow;
                _accuracyVignette.intensity.value = 0.5f;
                _accuracyChromaticAberration.intensity.value = 0;

                DOTween.To(() => _accuracyVignette.intensity.value, x => _accuracyVignette.intensity.value = x, 0,
                    accuracyEffect.duration).SetId("vignetteAccu");
            }
        }

        if (accuracy == "miss")
        {
            if (accuracyEffect.missEffect == 2)
            {
                DOTween.Kill("vignetteAccu");
                DOTween.Kill("chromaticAberrationAccu");
                DOTween.Kill("shakeAccu", true);

                _accuracyVignette.color.value = Color.red;
                _accuracyVignette.intensity.value = 0.5f;
                _accuracyChromaticAberration.intensity.value = 0;

                DOTween.To(() => _accuracyVignette.intensity.value, x => _accuracyVignette.intensity.value = x, 0,
                    accuracyEffect.duration).SetId("vignetteAccu");
                shakeRect.DOShakeAnchorPos(0.6f * accuracyEffect.duration, 11, 12, 90, false, false).SetId("shakeAccu");
            }

            if (accuracyEffect.missEffect == 1)
            {
                DOTween.Kill("vignetteAccu");
                DOTween.Kill("chromaticAberrationAccu");
                DOTween.Kill("shakeAccu", true);

                _accuracyVignette.color.value = Color.red;
                _accuracyVignette.intensity.value = 0.5f;
                _accuracyChromaticAberration.intensity.value = 0;

                DOTween.To(() => _accuracyVignette.intensity.value, x => _accuracyVignette.intensity.value = x, 0,
                    accuracyEffect.duration).SetId("vignetteAccu");
            }
        }
    }
}