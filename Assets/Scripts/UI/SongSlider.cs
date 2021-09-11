using NAudio.Wave;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public Slider slider;
	public SongTime songTime;
	public bool doScrubbing;

	bool _dragging;

	void Update()
	{
		if (AudioManager.Instance.outputDevice != null)
		{
			switch (_dragging)
			{
				case false when AudioManager.Instance.outputDevice.PlaybackState == PlaybackState.Playing:
					slider.value = AudioManager.Instance.audioFile.Position;
					break;
				case false when AudioManager.Instance.outputDevice.PlaybackState != PlaybackState.Playing && AudioManager.Instance.audioFile.Position != 0:
					break;
			}
		}
	}

	void OnEnable()
	{
		SystemEvents.audioLoadComplete += LoadEvents;
	}

	void OnDisable()
	{
		SystemEvents.audioLoadComplete -= LoadEvents;
	}

	public void SetBool(bool value) => doScrubbing = value;
	
	public void OnDrag(PointerEventData eventData)
	{
		if (!slider.interactable) return;
		songTime.changed = true;
		_dragging = true;
		float value = Mathf.Clamp(slider.value, 0, AudioManager.Instance.audioFile.Length);
		songTime.UpdateTime((long)value, doScrubbing);

		/*if (doScrubbing) metronome.isSongPositionMove = true;*/
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		songTime.changed = true;
		_dragging = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!slider.interactable) return;
		float value = Mathf.Clamp(slider.value * AudioManager.Instance.audioFile.Length, 0f, AudioManager.Instance.audioFile.Length);
		songTime.UpdateTime((long)value);

		songTime.changed = false;
		_dragging = false;

		/*metronome.isSongPositionMove = true;
		metronome.StartMet();*/
	}

	void LoadEvents()
	{
		slider.interactable = true;
	}
}