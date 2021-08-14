using System;
using EasyButtons;
using UnityEngine;

public class ScalingCanvas : MonoBehaviour
{
	public enum ScreenMatchMode
	{
		MatchWidthOrHeight = 0,
		Expand = 1,
		Shrink = 2
	}

	void OnEnable()
	{
		m_Canvas = GetComponent<Canvas>();
		HandleScaleWithScreenSize();
	}

	void OnDisable()
	{
		SetScaleFactor(1);
		// SetReferencePixelsPerUnit(100);
	}

	[Range(0, 1)] [SerializeField] protected float m_MatchWidthOrHeight = 0;
	private const float kLogBase = 2;
	[SerializeField] protected ScreenMatchMode m_ScreenMatchMode = ScreenMatchMode.MatchWidthOrHeight;

	public ScreenMatchMode screenMatchMode
	{
		get { return m_ScreenMatchMode; }
		set { m_ScreenMatchMode = value; }
	}

	// [SerializeField] protected float m_ReferencePixelsPerUnit = 100;
	[SerializeField] protected Vector2 m_ReferenceResolution = new Vector2(800, 600);
	public Canvas m_Canvas;
	[System.NonSerialized] private float m_PrevScaleFactor = 1;
	// [System.NonSerialized] private float m_PrevReferencePixelsPerUnit = 100;

	/*void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
	{
		if (referencePixelsPerUnit == m_PrevReferencePixelsPerUnit)
			return;

		m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
		m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
	}*/

	void SetScaleFactor(float scaleFactor)
	{
		if (scaleFactor == m_PrevScaleFactor)
			return;

		m_Canvas.scaleFactor = scaleFactor;
		m_PrevScaleFactor = scaleFactor;
	}

	[Button]
	public void HandleScaleWithScreenSize()
	{
		Vector2 screenSize = m_Canvas.renderingDisplaySize;

		// Multiple display support only when not the main display. For display 0 the reported
		// resolution is always the desktops resolution since its part of the display API,
		// so we use the standard none multiple display method. (case 741751)
		int displayIndex = m_Canvas.targetDisplay;
		if (displayIndex > 0 && displayIndex < Display.displays.Length)
		{
			Display disp = Display.displays[displayIndex];
			screenSize = new Vector2(disp.renderingWidth, disp.renderingHeight);
		}


		float scaleFactor = 0;
		switch (m_ScreenMatchMode)
		{
			case ScreenMatchMode.MatchWidthOrHeight:
			{
				// We take the log of the relative width and height before taking the average.
				// Then we transform it back in the original space.
				// the reason to transform in and out of logarithmic space is to have better behavior.
				// If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
				// In normal space the average would be (0.5 + 2) / 2 = 1.25
				// In logarithmic space the average is (-1 + 1) / 2 = 0
				float logWidth = Mathf.Log(screenSize.x / m_ReferenceResolution.x, kLogBase);
				float logHeight = Mathf.Log(screenSize.y / m_ReferenceResolution.y, kLogBase);
				float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, m_MatchWidthOrHeight);
				scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
				break;
			}
			case ScreenMatchMode.Expand:
			{
				scaleFactor = Mathf.Min(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
				break;
			}
			case ScreenMatchMode.Shrink:
			{
				scaleFactor = Mathf.Max(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
				break;
			}
		}

		SetScaleFactor(scaleFactor);
		//SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
	}
}