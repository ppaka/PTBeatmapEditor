using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum KillTweenType
{
	DoNotKill,
	KillButDoNotComplete,
	KillAndComplete
}

public enum SetValueType
{
	Set,
	Add
}

public enum TransformRelativeMode
{
	Local,
	Global
}

public class EffectScript : MonoBehaviour
{
	public RectTransform noteEndTweenRect, longNoteEndTweenRect, shakeRect;
	public Volume accuracyPpVolume, effectPpVolume;

	[Header("Scripts", order = 1)] [SerializeField]
	LevelEventManager levelEventManager;

	ChromaticAberration _accuracyChromaticAberration;
	Vignette _accuracyVignette;
	Bloom _bloom;
	ChromaticAberration _chromaticAberration;
	ColorAdjustments _colorAdjustments;
	DepthOfField _depthOfField;
	FilmGrain _filmGrain;
	LensDistortion _lensDistortion;

	MotionBlur _motionBlur;
	Vignette _vignette;
	WhiteBalance _whiteBalance;

	void Awake()
	{
		GetVolumeData();
	}

	void GetVolumeData()
	{
		/*effectPpVolume.profile.TryGet(out _motionBlur);
		effectPpVolume.profile.TryGet(out _bloom);
		effectPpVolume.profile.TryGet(out _chromaticAberration);
		effectPpVolume.profile.TryGet(out _colorAdjustments);
		effectPpVolume.profile.TryGet(out _filmGrain);
		effectPpVolume.profile.TryGet(out _lensDistortion);
		effectPpVolume.profile.TryGet(out _vignette);
		effectPpVolume.profile.TryGet(out _whiteBalance);
		effectPpVolume.profile.TryGet(out _depthOfField);

		accuracyPpVolume.profile.TryGet(out _accuracyVignette);
		accuracyPpVolume.profile.TryGet(out _accuracyChromaticAberration);*/
	}

	AnimationCurve GetCurve(string curveTag)
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

	public void FindAndKillTween(string id, KillTweenType killType = KillTweenType.KillButDoNotComplete)
	{
		switch (killType)
		{
			case KillTweenType.DoNotKill:
				break;
			case KillTweenType.KillAndComplete:
				DOTween.Kill(id, true);
				break;
			case KillTweenType.KillButDoNotComplete:
				DOTween.Kill(id);
				break;
		}
	}

	public void ActivatePostProcessing(string type, bool active)
	{
		switch (type)
		{
			case "motionBlur":
				_motionBlur.active = active;
				break;
			case "bloom":
				_bloom.active = active;
				break;
			case "chromaticAberration":
				_chromaticAberration.active = active;
				break;
			case "colorAdjustments":
				_colorAdjustments.active = active;
				break;
			case "filmGrain":
				_filmGrain.active = active;
				break;
			case "lensDistortion":
				_lensDistortion.active = active;
				break;
			case "vignette":
				_vignette.active = active;
				break;
			case "whiteBalance":
				_whiteBalance.active = active;
				break;
			case "depthOfField":
				_depthOfField.active = active;
				break;
		}
	}

	public void SetPostProcessing(string type, float? intensity, float? threshold, float? contrast, Color? color,
		float? hueShift, float? saturation, float? xMultiplier, float? yMultiplier, float[] center, float? scale,
		float? smoothness, bool? rounded, float? temperature, float? tint, float? focusDistance, float? focalLength,
		string filmGrainType, float duration, string ease, string curveTag = "", string tweenId = "")
	{
		switch (type)
		{
			case "motionBlur" when ease == "custom":
				DOTween.To(() => _motionBlur.intensity.value, x => _motionBlur.intensity.value = x, (float) intensity,
					duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				break;
			case "motionBlur":
				DOTween.To(() => _motionBlur.intensity.value, x => _motionBlur.intensity.value = x, (float) intensity,
					duration).SetEase(GetEase(ease)).SetId(tweenId);
				break;
			case "bloom" when ease == "custom":
				DOTween.To(() => _bloom.threshold.value, x => _bloom.threshold.value = x, (float) threshold, duration)
					.SetEase(GetCurve(curveTag)).SetId(tweenId);
				DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, (float) intensity, duration)
					.SetEase(GetCurve(curveTag)).SetId(tweenId);
				DOTween.To(() => _bloom.tint.value, x => _bloom.tint.value = x, (Color) color, duration)
					.SetEase(GetCurve(curveTag)).SetId(tweenId);
				break;
			case "bloom":
				DOTween.To(() => _bloom.threshold.value, x => _bloom.threshold.value = x, (float) threshold, duration)
					.SetEase(GetEase(ease)).SetId(tweenId);
				DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, (float) intensity, duration)
					.SetEase(GetEase(ease)).SetId(tweenId);
				DOTween.To(() => _bloom.tint.value, x => _bloom.tint.value = x, (Color) color, duration)
					.SetEase(GetEase(ease)).SetId(tweenId);
				break;
			case "chromaticAberration" when ease == "custom":
				DOTween.To(() => _chromaticAberration.intensity.value, x => _chromaticAberration.intensity.value = x,
					(float) intensity, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				break;
			case "chromaticAberration":
				DOTween.To(() => _chromaticAberration.intensity.value, x => _chromaticAberration.intensity.value = x,
					(float) intensity, duration).SetEase(GetEase(ease)).SetId(tweenId);
				break;
			case "colorAdjustments" when ease == "custom":
				DOTween.To(() => _colorAdjustments.postExposure.value, x => _colorAdjustments.postExposure.value = x,
					(float) contrast, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				DOTween.To(() => _colorAdjustments.colorFilter.value, x => _colorAdjustments.colorFilter.value = x,
					(Color) color, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				DOTween.To(() => _colorAdjustments.hueShift.value, x => _colorAdjustments.hueShift.value = x,
					(float) hueShift, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
					(float) saturation, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				break;
			case "colorAdjustments":
				DOTween.To(() => _colorAdjustments.postExposure.value, x => _colorAdjustments.postExposure.value = x,
					(float) contrast, duration).SetEase(GetEase(ease)).SetId(tweenId);
				DOTween.To(() => _colorAdjustments.colorFilter.value, x => _colorAdjustments.colorFilter.value = x,
					(Color) color, duration).SetEase(GetEase(ease)).SetId(tweenId);
				DOTween.To(() => _colorAdjustments.hueShift.value, x => _colorAdjustments.hueShift.value = x,
					(float) hueShift, duration).SetEase(GetEase(ease)).SetId(tweenId);
				DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x,
					(float) saturation, duration).SetEase(GetEase(ease)).SetId(tweenId);
				break;
			case "filmGrain":
			{
				_filmGrain.type.value = filmGrainType switch
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
					DOTween.To(() => _filmGrain.intensity.value, x => _filmGrain.intensity.value = x, (float) intensity,
						duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				else
					DOTween.To(() => _filmGrain.intensity.value, x => _filmGrain.intensity.value = x, (float) intensity,
						duration).SetEase(GetEase(ease)).SetId(tweenId);

				break;
			}
			case "lensDistortion" when ease == "custom":
				DOTween.To(() => _lensDistortion.intensity.value, x => _lensDistortion.intensity.value = x,
					(float) intensity, duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _lensDistortion.xMultiplier.value, x => _lensDistortion.xMultiplier.value = x,
					(float) xMultiplier, duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _lensDistortion.yMultiplier.value, x => _lensDistortion.yMultiplier.value = x,
					(float) yMultiplier, duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _lensDistortion.center.value, x => _lensDistortion.center.value = x,
					new Vector2(center[0], center[1]), duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _lensDistortion.scale.value, x => _lensDistortion.scale.value = x, (float) scale,
					duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				break;
			case "lensDistortion":
				DOTween.To(() => _lensDistortion.intensity.value, x => _lensDistortion.intensity.value = x,
					(float) intensity, duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _lensDistortion.xMultiplier.value, x => _lensDistortion.xMultiplier.value = x,
					(float) xMultiplier, duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _lensDistortion.yMultiplier.value, x => _lensDistortion.yMultiplier.value = x,
					(float) yMultiplier, duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _lensDistortion.center.value, x => _lensDistortion.center.value = x,
					new Vector2(center[0], center[1]), duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _lensDistortion.scale.value, x => _lensDistortion.scale.value = x, (float) scale,
					duration).SetId(tweenId).SetEase(GetEase(ease));
				break;
			case "vignette" when ease == "custom":
				DOTween.To(() => _vignette.color.value, x => _vignette.color.value = x, (Color) color, duration)
					.SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _vignette.center.value, x => _vignette.center.value = x,
					new Vector2(center[0], center[1]), duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, (float) intensity,
					duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _vignette.smoothness.value, x => _vignette.smoothness.value = x, (float) smoothness,
					duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				_vignette.rounded.value = (bool) rounded;
				break;
			case "vignette":
				DOTween.To(() => _vignette.color.value, x => _vignette.color.value = x, (Color) color, duration)
					.SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _vignette.center.value, x => _vignette.center.value = x,
					new Vector2(center[0], center[1]), duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, (float) intensity,
					duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _vignette.smoothness.value, x => _vignette.smoothness.value = x, (float) smoothness,
					duration).SetId(tweenId).SetEase(GetEase(ease));
				_vignette.rounded.value = (bool) rounded;
				break;
			case "whiteBalance" when ease == "custom":
				DOTween.To(() => _whiteBalance.temperature.value, x => _whiteBalance.temperature.value = x,
					(float) temperature, duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _whiteBalance.tint.value, x => _whiteBalance.tint.value = x, (float) tint, duration)
					.SetId(tweenId).SetEase(GetCurve(curveTag));
				break;
			case "whiteBalance":
				DOTween.To(() => _whiteBalance.temperature.value, x => _whiteBalance.temperature.value = x,
					(float) temperature, duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _whiteBalance.tint.value, x => _whiteBalance.tint.value = x, (float) tint, duration)
					.SetId(tweenId).SetEase(GetEase(ease));
				break;
			case "depthOfField" when ease == "custom":
				DOTween.To(() => _depthOfField.focusDistance.value, x => _depthOfField.focusDistance.value = x,
					(float) focusDistance, duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				DOTween.To(() => _depthOfField.focalLength.value, x => _depthOfField.focalLength.value = x,
					(float) focalLength, duration).SetId(tweenId).SetEase(GetCurve(curveTag));
				break;
			case "depthOfField":
				DOTween.To(() => _depthOfField.focusDistance.value, x => _depthOfField.focusDistance.value = x,
					(float) focusDistance, duration).SetId(tweenId).SetEase(GetEase(ease));
				DOTween.To(() => _depthOfField.focalLength.value, x => _depthOfField.focalLength.value = x,
					(float) focalLength, duration).SetId(tweenId).SetEase(GetEase(ease));
				break;
		}
	}

	public void MoveRect(GameObject obj, Vector2 pos, float duration, string ease, string curveTag = "",
		TransformRelativeMode relativeMode = TransformRelativeMode.Local, SetValueType setType = SetValueType.Add,
		string tweenId = "")
	{
		RectTransform tf = obj.GetComponent<RectTransform>();

		if (ease.Equals("custom"))
		{
			if (setType == SetValueType.Add)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalMove(tf.localPosition + new Vector3(pos.x, pos.y, 0), duration)
						.SetEase(GetCurve(curveTag)).SetId(tweenId);
				else
					tf.DOMove(tf.position + new Vector3(pos.x, pos.y, 0), duration).SetEase(GetCurve(curveTag))
						.SetId(tweenId);
			}
			else if (setType == SetValueType.Set)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
				else
					tf.DOMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
			}
		}
		else
		{
			if (setType == SetValueType.Add)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalMove(tf.localPosition + new Vector3(pos.x, pos.y, 0), duration).SetEase(GetEase(ease))
						.SetId(tweenId);
				else
					tf.DOMove(tf.position + new Vector3(pos.x, pos.y, 0), duration).SetEase(GetEase(ease))
						.SetId(tweenId);
			}
			else if (setType == SetValueType.Set)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetEase(ease)).SetId(tweenId);
				else
					tf.DOMove(new Vector3(pos.x, pos.y, 0), duration).SetEase(GetEase(ease)).SetId(tweenId);
			}
		}
	}

	public void RotateRect(GameObject obj, Vector3 pos, float duration, string ease, string curveTag = "",
		TransformRelativeMode relativeMode = TransformRelativeMode.Local, SetValueType setType = SetValueType.Add,
		string tweenId = "")
	{
		RectTransform tf = obj.GetComponent<RectTransform>();

		if (ease.Equals("custom"))
		{
			if (setType == SetValueType.Add)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalRotate(tf.localRotation.eulerAngles + new Vector3(pos.x, pos.y, pos.z), duration)
						.SetEase(GetCurve(curveTag)).SetId(tweenId);
				else
					tf.DORotate(tf.rotation.eulerAngles + new Vector3(pos.x, pos.y, pos.z), duration)
						.SetEase(GetCurve(curveTag)).SetId(tweenId);
			}
			else if (setType == SetValueType.Set)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalRotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetCurve(curveTag))
						.SetId(tweenId);
				else
					tf.DORotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
			}
		}
		else
		{
			if (setType == SetValueType.Add)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalRotate(tf.localRotation.eulerAngles + new Vector3(pos.x, pos.y, pos.z), duration)
						.SetEase(GetEase(ease)).SetId(tweenId);
				else
					tf.DORotate(tf.rotation.eulerAngles + new Vector3(pos.x, pos.y, pos.z), duration)
						.SetEase(GetEase(ease)).SetId(tweenId);
			}
			else if (setType == SetValueType.Set)
			{
				if (relativeMode == TransformRelativeMode.Local)
					tf.DOLocalRotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetEase(ease)).SetId(tweenId);
				else
					tf.DORotate(new Vector3(pos.x, pos.y, pos.z), duration).SetEase(GetEase(ease)).SetId(tweenId);
			}
		}
	}

	public void ScaleRect(GameObject obj, Vector2 pos, float duration, string ease, string curveTag = "",
		SetValueType setType = SetValueType.Set, string tweenId = "")
	{
		RectTransform tf = obj.GetComponent<RectTransform>();

		if (ease.Equals("custom"))
		{
			if (setType == SetValueType.Add)
				tf.DOScale(tf.localScale + new Vector3(pos.x, pos.y, 1), duration).SetEase(GetCurve(curveTag))
					.SetId(tweenId);
			else if (setType == SetValueType.Set)
				tf.DOScale(new Vector3(pos.x, pos.y, 1), duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
		}
		else
		{
			if (setType == SetValueType.Add)
				tf.DOScale(tf.localScale + new Vector3(pos.x, pos.y, 1), duration).SetEase(GetEase(ease))
					.SetId(tweenId);
			else if (setType == SetValueType.Set)
				tf.DOScale(new Vector3(pos.x, pos.y, 1), duration).SetEase(GetEase(ease)).SetId(tweenId);
		}
	}

	public void SetColor(GameObject obj, Color value, float duration, string ease, string curveTag = "",
		string tweenId = "")
	{
		Image img = obj.GetComponent<Image>();

		if (ease.Equals("custom"))
			img.DOColor(value, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
		else
			img.DOColor(value, duration).SetEase(GetEase(ease)).SetId(tweenId);
	}

	public void SetLightColor(GameObject obj, Color value, float duration, string ease, string curveTag = "",
		string tweenId = "")
	{
		Light2D light2D = obj.GetComponent<Light2D>();

		if (ease.Equals("custom"))
			DOTween.To(() => light2D.color, x => light2D.color = x, value, duration).SetEase(GetCurve(curveTag))
				.SetId(tweenId);
		else
			DOTween.To(() => light2D.color, x => light2D.color = x, value, duration).SetEase(GetEase(ease))
				.SetId(tweenId);
	}

	public void SetParticle(GameObject obj, bool play, float speed)
	{
		ParticleSystem particle = obj.GetComponent<ParticleSystem>();

		if (play)
		{
			ParticleSystem.MainModule particleMain = particle.main;
			if (speed < 0) speed = 1;
			particleMain.simulationSpeed = speed;

			particle.Play();
		}
		else
		{
			particle.Stop();
		}
	}

	public void SetParticleColor(GameObject obj, Color value, float duration, string ease, string curveTag = "",
		string tweenId = "")
	{
		ParticleSystem particle = obj.GetComponent<ParticleSystem>();
		Color color = particle.main.startColor.color;

		if (ease.Equals("custom"))
			DOTween.To(() => color, x => color = x, value, duration).SetEase(GetCurve(curveTag)).SetId(tweenId);
		else
			DOTween.To(() => color, x => color = x, value, duration).SetEase(GetEase(ease)).SetId(tweenId);
	}

	public void ClearParticle(GameObject obj)
	{
		ParticleSystem particle = obj.GetComponent<ParticleSystem>();

		particle.Clear();
	}

	public void SetShake(GameObject obj, float duration, float strength, int vibrato, bool fadeOut, string tweenId = "")
	{
		RectTransform tf = obj.GetComponent<RectTransform>();

		tf.DOShakePosition(duration, strength, vibrato, fadeOut: fadeOut).SetId(tweenId);
	}

	public void AccuracyEffect(string accuracy)
	{
		AccuracyEffect accuracyEffect = levelEventManager.accuracyEffects;
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
						accuracyEffect.duration, (int) (6 / accuracyEffect.duration), 0.1f).SetEase(Ease.OutBack)
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
						accuracyEffect.duration, (int) (6 / accuracyEffect.duration), 0.1f).SetEase(Ease.OutBack)
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