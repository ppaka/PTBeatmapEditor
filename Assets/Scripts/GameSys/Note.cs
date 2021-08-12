using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
	[HideInInspector] public NoteType noteType;
	public Image[] images;
	public Sprite[] sprites;
	[HideInInspector] public bool isLastNote;

	public SongTime songTime;

	[HideInInspector] public List<NoteEvents> perfectEvt, goodEvt, missEvt;
	public Image thisImage;
	public RectTransform modelImage;
	[HideInInspector] public float startTime, perfectTime;
	[HideInInspector] public float? noteEndTime;
	[HideInInspector] public Transform startTransform;
	[HideInInspector] public float duration;
	[HideInInspector] public Vector3 myPos, perfectPos;
	public bool cleared;
	public uint number;
	public float separate = 1;
	Vector3 _clearAnimPos;
	AnimationCurve _curve;
	bool _isKeepGoing;
	[HideInInspector] public float playingTime;
	public CanvasGroup thisCanvasGroup;

	bool _spawned;
	int _tries = 1;
	bool _usingCustomEase;

	Ease ease;

	void Update()
	{
		//if (!_spawned) return;

		playingTime = songTime.audioSource.time - startTime - duration / separate * (_tries - 1);

		// 미스내기
		/*if (!cleared)
		{
			switch (noteType)
			{
				case NoteType.Chain:
				{
					if (songTime.audioSource.time > noteEndTime)
					{
						_spawned = false;
						//timingSystem.MissAction(this, NoteType.Chain);
						//gameObject.SetActive(false);
					}

					break;
				}
				case NoteType.Normal:
				{
					if (songTime.audioSource.time > perfectTime + 0.250f)
					{
						_spawned = false;
						//timingSystem.MissAction(this, NoteType.Normal);
						//gameObject.SetActive(false);
					}

					break;
				}
				case NoteType.Flick:
				{
					if (songTime.audioSource.time > perfectTime + 0.250f)
					{
						_spawned = false;
						//timingSystem.MissAction(this, NoteType.Flick);
						//gameObject.SetActive(false);
					}

					break;
				}
			}
		}*/
		if (noteType != NoteType.Chain) // 체인노트가 아니면
		{
			//if (cleared) return;
			if (duration / separate <= playingTime && _tries != separate && _tries != separate * 2)
			{
				_tries += 1;
				myPos = transform.localPosition;
			}

			
			//if (_playingTime >= 0 && _playingTime <= duration / _separate)
			{
				MoveLocal(transform, myPos,
					GetDistance(startTransform.localPosition.x, 0, perfectPos.x, 0) * -1 / separate, playingTime,
					duration / separate);
			}
			
		}
		else // 체인노트면
		{
			/*if (songTime.audioSource.time > noteEndTime && cleared)
			{
				_spawned = false;
				//timingSystem.LongNoteAction(this);
				//HideNote();
			}*/

			if (duration / separate <= playingTime && _tries != separate && _tries != separate * 2)
			{
				_tries += 1;
				myPos = transform.localPosition;
			}
			else if (playingTime >= duration / separate && playingTime <= duration / separate * 2 && !_isKeepGoing)
			{
				if (ease != Ease.Linear)
				{
					_tries += 1;
					myPos = perfectPos;
				}

				_isKeepGoing = true;
			}

			//if (_playingTime >= 0 && _playingTime <= duration / _separate && _tries <= _separate * 2)
			{
				MoveLocal(transform, myPos,
					GetDistance(startTransform.localPosition.x, 0, perfectPos.x, 0) * -1 / separate, playingTime,
					duration / separate);
				
			}
		}
	}

	void OnEnable()
	{
		if (songTime == null)
			FindObjectOfType<SongTime>();
		perfectEvt = null;
		goodEvt = null;
		missEvt = null;

		cleared = false;
		_tries = 1;
		_isKeepGoing = false;

		modelImage.localPosition = new Vector3(0, 0, 0);
	}

	void OnDisable()
	{
		//ObjectPooler.ReturnToPool(gameObject);
	}

	public void SetData(SongTime songTime, Transform startTf, Transform movePerfectPos,
		float perfectTime, float timeToStart, float moveDuration, Transform clearTweenEndTf, Transform spawnParent,
		bool isLast, float splitValue, uint noteNumber, float? noteEndTime = null, string ease = "L", AnimationCurve curve = null)
	{
		for (int i = 0; i < images.Length; i++)
			images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1);

		this.songTime = songTime;
		startTransform = startTf;
		perfectPos = movePerfectPos.localPosition;
		transform.localPosition = myPos = startTransform.localPosition;
		this.perfectTime = perfectTime;
		startTime = timeToStart;
		duration = moveDuration;
		//_clearAnimPos = clearTweenEndTf.position;
		isLastNote = isLast;
		separate = splitValue;
		this.noteEndTime = noteEndTime;
		number = noteNumber;
		_spawned = true;
		if (ease != "custom")
		{
			this.ease = EffectScript.GetEase(ease);
		}
		else
		{
			_usingCustomEase = true;
			_curve = curve;
		}

		//Debug.Log($"{perfectTime}");
	}

	void MoveLocal(Transform target, Vector3 startPos, float distance, float time, float duration)
	{
		if (!_usingCustomEase)
			target.localPosition = new Vector3(
				startPos.x + distance * EaseManager.Evaluate(ease, (f, d, a, p) => 0, time, duration,
					DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod), transform.localPosition.y,
				transform.localPosition.z);
		else
			target.localPosition = new Vector3(
				startPos.x + distance * new EaseCurve(_curve).Evaluate(time, duration,
					DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod), transform.localPosition.y,
				transform.localPosition.z);
	}

	float GetDistance(float x1, float y1, float x2, float y2)
	{
		float width = x2 - x1;
		float height = y2 - y1;

		float distance = width * width + height * height;
		distance = Mathf.Sqrt(distance);

		return distance;
	}

	public void SetNoteEvents(List<NoteEvents> perfect, List<NoteEvents> good, List<NoteEvents> miss)
	{
		perfectEvt = perfect;
		goodEvt = good;
		missEvt = miss;
	}

	public void SetLongNoteLength(int startTime, int endTime, float duration)
	{
		float holdTime = (float) (endTime - startTime);
		RectTransform rt = images[1].GetComponent<RectTransform>();
		Vector3 rtLocalScale = rt.localScale;
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rtLocalScale.x * holdTime / duration);
		rt.anchoredPosition = new Vector2(rtLocalScale.x * holdTime / duration / 2 + rt.anchoredPosition.x, 0);
	}

	public void ChangeDuration(float changeValue)
	{
		duration = changeValue;
	}

	public void HideNote()
	{
		if (noteType == NoteType.Chain)
		{
			transform.DOKill();

			for (int i = 0; i < images.Length; i++) images[i].DOFade(0, 0.5f);

			transform.DOMoveY(_clearAnimPos.y, 0.5f).SetEase(Ease.OutQuad)
				.OnComplete(() => gameObject.SetActive(false));
		}
		else if (noteType == NoteType.Flick)
		{
			transform.DOKill();

			thisImage.sprite = sprites[1];
			thisImage.DOFade(0, 0.5f);

			transform.DOMove(_clearAnimPos, 0.5f).SetEase(Ease.InBack, 0.15f)
				.OnComplete(() => gameObject.SetActive(false));
		}
		else
		{
			transform.DOKill();

			thisImage.DOFade(0, 0.5f);

			transform.DOMove(_clearAnimPos, 0.5f).SetEase(Ease.InBack, 0.15f)
				.OnComplete(() => gameObject.SetActive(false));
		}
	}
}