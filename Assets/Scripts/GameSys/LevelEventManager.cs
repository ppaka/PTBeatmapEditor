using System;
using System.Collections.Generic;
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
	public AccuracyEffect accuracyEffects;

	[Header("Scripts", order = 4)] public LevelObjectManager levelObjectManager;

	public EffectScript effectScript;
	public SongTime songTime;
	List<Curves> _curvesList;

	float _delay;

	List<Events> _eventsList;

	public Dictionary<string, AnimationCurve> CurveDictionary = new Dictionary<string, AnimationCurve>();

	void OnEnable()
	{
		SystemEvents.levelLoadComplete += Load;
	}

	void OnDisable()
	{
		SystemEvents.levelLoadComplete -= Load;
	}

	public void Load()
	{
		CurveDictionary = new Dictionary<string, AnimationCurve>();
		_eventsList = LevelDataContainer.Instance.levelData.events;
		_curvesList = LevelDataContainer.Instance.levelData.curves;
		var settings = LevelDataContainer.Instance.levelData.settings;
		_delay = settings.eventOffset * 0.001f;

		foreach (var curve in _curvesList) CurveDictionary.Add(curve.tag, curve.data);

		(accuracyEffects.perfectEffect, accuracyEffects.goodEffect, accuracyEffects.missEffect,
			accuracyEffects.duration) = (settings.perfectAccuracyEffect, settings.goodAccuracyEffect,
			settings.missAccuracyEffect, settings.accuracyEffectDuration);
	}

	/*void Update()
	{
		if (_eventsList.Count == 0) return;
		var data = _eventsList[0];
		if (!(songTime.audioSource.time >= data.time * 0.001f + _delay)) return;
		_eventsList.Remove(data);
		
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
				effectScript.FindAndKillTween(data.tweenId, type);
				break;
			}
			case "postProcessing":
			{
				ColorUtility.TryParseHtmlString("#" + data.ppColor, out var color);
				effectScript.ActivatePostProcessing(data.ppType, (bool) data.turnOn);
				effectScript.SetPostProcessing(data.ppType, data.ppIntensity, data.ppThreshold, data.ppContrast, color,
					data.ppHueShift, data.ppSaturation, data.ppXMultiplier, data.ppYMultiplier, data.ppCenter,
					data.ppScale, data.ppSmoothness, data.ppRounded, data.ppTemperature, data.ppTint,
					data.ppFocusDistance, data.ppFocalLength, data.ppFilmGrainType, (float) data.duration, data.ease,
					data.customCurveTag, data.tweenId);
				break;
			}
			case "accuracyEffect":
			{
				(accuracyEffects.perfectEffect, accuracyEffects.goodEffect, accuracyEffects.missEffect,
					accuracyEffects.duration) = ((int) data.perfectEffect, (int) data.goodEffect, (int) data.missEffect,
					(float) data.duration);
				break;
			}
			case "setParticle":
			{
				effectScript.SetParticle(levelObjectManager.Obj[data.target], (bool) data.turnOn, data.speed ?? 1);
				break;
			}
			case "clearParticle":
			{
				effectScript.ClearParticle(levelObjectManager.Obj[data.target]);
				break;
			}
			case "setColor":
			{
				ColorUtility.TryParseHtmlString("#" + data.color, out var color);
				effectScript.SetColor(levelObjectManager.Obj[data.target], color, (float) data.duration, data.ease,
					data.tweenId);
				break;
			}
			case "setLightColor":
			{
				ColorUtility.TryParseHtmlString("#" + data.color, out var color);
				effectScript.SetLightColor(levelObjectManager.Obj[data.target], color, (float) data.duration, data.ease,
					data.tweenId);
				break;
			}
			case "move2d":
			{
				var vector = new Vector2(data.position[0], data.position[1]);
				TransformRelativeMode relativeMode = data.transformMode == "global"
					? TransformRelativeMode.Global
					: TransformRelativeMode.Local;
				SetValueType valueType = data.setValueType == "Set" ? SetValueType.Set : SetValueType.Add;

				effectScript.MoveRect(levelObjectManager.Obj[data.target], vector, (float) data.duration, data.ease,
					data.customCurveTag, relativeMode, valueType, data.tweenId);
				break;
			}
			case "rotate":
			{
				var vector = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
				TransformRelativeMode relativeMode = data.transformMode == "global"
					? TransformRelativeMode.Global
					: TransformRelativeMode.Local;
				SetValueType valueType = data.setValueType == "Set" ? SetValueType.Set : SetValueType.Add;

				effectScript.RotateRect(levelObjectManager.Obj[data.target], vector, (float) data.duration, data.ease,
					data.customCurveTag, relativeMode, valueType, data.tweenId);
				break;
			}
			case "scale2d":
			{
				var vector = new Vector2(data.scale[0], data.scale[1]);
				SetValueType valueType = data.setValueType == "Set" ? SetValueType.Set : SetValueType.Add;

				effectScript.ScaleRect(levelObjectManager.Obj[data.target], vector, (float) data.duration, data.ease,
					data.customCurveTag, valueType, data.tweenId);
				break;
			}
			case "shake2d":
			{
				effectScript.SetShake(levelObjectManager.Obj[data.target], (float) data.duration, (float) data.strength,
					(int) data.vibrato, (bool) data.fadeout, data.tweenId);
				break;
			}
		}
	}*/

	public void NoteEvents(IEnumerable<NoteEvents> events)
	{
		foreach (var data in events)
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
					effectScript.FindAndKillTween(data.tweenId, type);
					break;
				}
				case "postProcessing":
				{
					ColorUtility.TryParseHtmlString("#" + data.ppColor, out var color);
					effectScript.ActivatePostProcessing(data.ppType, (bool) data.turnOn);
					effectScript.SetPostProcessing(data.ppType, data.ppIntensity, data.ppThreshold, data.ppContrast,
						color, data.ppHueShift, data.ppSaturation, data.ppXMultiplier, data.ppYMultiplier,
						data.ppCenter, data.ppScale, data.ppSmoothness, data.ppRounded, data.ppTemperature, data.ppTint,
						data.ppFocusDistance, data.ppFocalLength, data.ppFilmGrainType, (float) data.duration,
						data.ease, data.customCurveTag, data.tweenId);
					break;
				}
				case "accuracyEffect":
				{
					(accuracyEffects.perfectEffect, accuracyEffects.goodEffect, accuracyEffects.missEffect,
						accuracyEffects.duration) = ((int) data.perfectEffect, (int) data.goodEffect,
						(int) data.missEffect, (float) data.duration);
					break;
				}
				case "setParticle":
				{
					effectScript.SetParticle(levelObjectManager.Obj[data.target], (bool) data.turnOn, data.speed ?? 1);
					break;
				}
				case "clearParticle":
				{
					effectScript.ClearParticle(levelObjectManager.Obj[data.target]);
					break;
				}
				case "setColor":
				{
					ColorUtility.TryParseHtmlString("#" + data.color, out var color);
					effectScript.SetColor(levelObjectManager.Obj[data.target], color, (float) data.duration, data.ease,
						data.tweenId);
					break;
				}
				case "setLightColor":
				{
					ColorUtility.TryParseHtmlString("#" + data.color, out var color);
					effectScript.SetLightColor(levelObjectManager.Obj[data.target], color, (float) data.duration,
						data.ease, data.tweenId);
					break;
				}
				case "move2d":
				{
					var vector = new Vector2(data.position[0], data.position[1]);
					TransformRelativeMode relativeMode = data.transformMode == "global"
						? TransformRelativeMode.Global
						: TransformRelativeMode.Local;
					SetValueType valueType = data.setValueType == "Set" ? SetValueType.Set : SetValueType.Add;

					effectScript.MoveRect(levelObjectManager.Obj[data.target], vector, (float) data.duration, data.ease,
						data.customCurveTag, relativeMode, valueType, data.tweenId);
					break;
				}
				case "rotate":
				{
					var vector = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
					TransformRelativeMode relativeMode = data.transformMode == "global"
						? TransformRelativeMode.Global
						: TransformRelativeMode.Local;
					SetValueType valueType = data.setValueType == "Set" ? SetValueType.Set : SetValueType.Add;

					effectScript.RotateRect(levelObjectManager.Obj[data.target], vector, (float) data.duration,
						data.ease, data.customCurveTag, relativeMode, valueType, data.tweenId);
					break;
				}
				case "scale2d":
				{
					var vector = new Vector2(data.scale[0], data.scale[1]);
					SetValueType valueType = data.setValueType == "Set" ? SetValueType.Set : SetValueType.Add;

					effectScript.ScaleRect(levelObjectManager.Obj[data.target], vector, (float) data.duration,
						data.ease, data.customCurveTag, valueType, data.tweenId);
					break;
				}
				case "shake2d":
				{
					effectScript.SetShake(levelObjectManager.Obj[data.target], (float) data.duration,
						(float) data.strength, (int) data.vibrato, (bool) data.fadeout, data.tweenId);
					break;
				}
			}
	}
}