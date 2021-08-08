using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarCamControl : MonoBehaviour
{
	public Camera mainCam;
	public ScalingCanvas scaler;
	public CanvasGroup canvasGroup;

	Coroutine _coroutine;
	bool _isOn;

	void Update()
	{
		if (Input.GetKey(KeyCode.LeftControl))
			if (Input.GetKeyDown(KeyCode.T))
			{
				if (_coroutine != null)
					StopCoroutine(_coroutine);

				_coroutine = StartCoroutine(ScalingCanvas());
			}
	}

	IEnumerator ScalingCanvas()
	{
		if (_isOn == false)
		{
			mainCam.DOKill();
			mainCam.DORect(new Rect(0, 0, 1, 0.94f), 0.2f).SetEase(Ease.OutQuint);
			canvasGroup.DOKill();
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;

			Tween t = canvasGroup.DOFade(0.6f, 0.4f).SetEase(Ease.Linear);
			_isOn = true;
			yield return new DOTweenCYInstruction.WaitForCompletion(t);

			canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.Linear);
			canvasGroup.blocksRaycasts = true;
			canvasGroup.interactable = true;
			scaler.HandleScaleWithScreenSize();
		}
		else
		{
			mainCam.DOKill();
			mainCam.DORect(new Rect(0, 0, 1, 1), 0.2f).SetEase(Ease.OutQuint);
			canvasGroup.DOKill();
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;

			Tween t = canvasGroup.DOFade(0.6f, 0.4f).SetEase(Ease.Linear);
			_isOn = false;
			yield return new DOTweenCYInstruction.WaitForCompletion(t);

			canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.Linear);
			canvasGroup.blocksRaycasts = true;
			canvasGroup.interactable = true;
			scaler.HandleScaleWithScreenSize();
		}
	}
}