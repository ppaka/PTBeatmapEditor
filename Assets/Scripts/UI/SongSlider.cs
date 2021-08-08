using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public AudioSource audioSource;
	public Slider slider;
	public SongTime songTime;
	public bool doScrubbing;

	public Metronome metronome;
	AudioClip _clip;
	bool _dragging;

	void Update()
	{
		switch (_dragging)
		{
			case false when audioSource.isPlaying:
				slider.value = audioSource.time / audioSource.clip.length;
				break;
			case false when !audioSource.isPlaying && audioSource.time != 0:
				break;
		}
	}

	void OnEnable()
	{
		global::LoadEvents.audioLoadComplete += LoadEvents;
	}

	void OnDisable()
	{
		global::LoadEvents.audioLoadComplete -= LoadEvents;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!slider.interactable) return;
		songTime.changed = true;
		_dragging = true;
		float value = Mathf.Clamp(slider.value * _clip.length, 0, _clip.length);
		songTime.UpdateTime(value, doScrubbing);

		if (doScrubbing) metronome.isSongPositionMove = true;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		songTime.changed = true;
		_dragging = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!slider.interactable) return;
		float value = Mathf.Clamp(slider.value * _clip.length, 0f, _clip.length);
		songTime.UpdateTime(value);

		songTime.changed = false;
		_dragging = false;

		metronome.isSongPositionMove = true;
		metronome.StartMet();
	}

	void LoadEvents()
	{
		slider.interactable = true;
		_clip = audioSource.clip;
	}
}