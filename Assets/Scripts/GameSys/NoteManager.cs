using System;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
	Normal,
	Flick,
	Chain
}

public class NoteManager : MonoBehaviour
{
	public RectTransform tfNoteAppear, tfNotePerfect;
	public Transform noteSpawnParent;

	[Header("Scripts", order = 3)] public SongTime songTime;

	public EffectScript effectScript;
	public LevelEventManager levelEventManager;

	public Note normalNote, flickNote, chainNote;
	
	float _delay;

	List<Note> _notes = new List<Note>();

	Dictionary<uint, Dictionary<string, List<NoteEvents>>> NoteEvents;

	void OnEnable()
	{
		SystemEvents.levelLoadComplete += Load;
		SystemEvents.noteAdded += MakeNote;
		SystemEvents.noteRemoved += DeleteNote;
		SystemEvents.noteRemoveAll += DeleteAll;
	}

	void OnDisable()
	{
		SystemEvents.levelLoadComplete -= Load;
		SystemEvents.noteAdded -= MakeNote;
		SystemEvents.noteRemoved -= DeleteNote;
		SystemEvents.noteRemoveAll -= DeleteAll;
	}

	void Load()
	{
		NoteEvents = LevelDataContainer.instance.noteEvents;
		_delay = LevelDataContainer.instance.levelData.settings.noteOffset * 0.001f;
		Init();
	}

	void Update()
	{
		if (_notes.Count == 0) return;

		foreach (var data in _notes)
		{
			if (data.noteType != NoteType.Chain)
			{
				if (data.playingTime >= 0 && songTime.audioSource.time <= data.perfectTime)
				{
					data.thisCanvasGroup.alpha = 1;
				}
                else
                {
                	data.thisCanvasGroup.alpha = 0;
                }
			}
			else
			{
				if (data.playingTime >= 0 && songTime.audioSource.time <= data.noteEndTime)
				{
					data.thisCanvasGroup.alpha = 1;
				}
				else
				{
					data.thisCanvasGroup.alpha = 0;
				}
			}
		}
	}

	void Init()
	{
		//if (LevelDataContainer.Instance.levelData.notes.Count == 0) return;
		//Notes data = LevelDataContainer.Instance.levelData.notes[0];

		//if (!(songTime.audioSource.time >= data.time * 0.001f - data.duration + _delay)) return;

		//LevelDataContainer.Instance.levelData.notes.Remove(data);

		foreach (var data in LevelDataContainer.instance.levelData.notes)
		{
			MakeNote(data);
		}
	}

	void MakeNote(Notes data)
	{
		uint num = data.noteNum;
		//bool isLastNote = LevelDataContainer.Instance.levelData.notes.Count == 0;
		bool isLastNote = false;
		Note obj;

		switch (data.type)
		{
			case "normal":
			{
				//obj = ObjectPooler.SpawnFromPool<Note>("Normal Note", tfNoteAppear.position);
				obj = Instantiate(normalNote, tfNoteAppear.position, Quaternion.identity);

				obj.transform.SetParent(noteSpawnParent, true);
				obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.noteType = NoteType.Normal;
				AnimationCurve curve = data.ease == "custom"
					? levelEventManager.CurveDictionary[data.customCurveTag]
					: null;
				obj.SetData(songTime, tfNoteAppear, tfNotePerfect, data.time * 0.001f + _delay,
					data.time * 0.001f - data.duration + _delay, data.duration, effectScript.noteEndTweenRect,
					noteSpawnParent, isLastNote, data.splitEase, num, null, data.ease, curve);
				if (NoteEvents.ContainsKey(num))
					obj.SetNoteEvents(NoteEvents[num]["perfect"], NoteEvents[num]["good"], NoteEvents[num]["miss"]);

				//timingSystem.notes.Add(obj);
				_notes.Add(obj);
				break;
			}
			case "flick":
			{
				//obj = ObjectPooler.SpawnFromPool<Note>("Flick Note", tfNoteAppear.position);
				obj = Instantiate(flickNote, tfNoteAppear.position, Quaternion.identity);

				obj.transform.SetParent(noteSpawnParent, true);
				obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.noteType = NoteType.Flick;
				AnimationCurve curve = data.ease == "custom"
					? levelEventManager.CurveDictionary[data.customCurveTag]
					: null;
				obj.SetData(songTime, tfNoteAppear, tfNotePerfect, data.time * 0.001f + _delay,
					data.time * 0.001f - data.duration + _delay, data.duration, effectScript.noteEndTweenRect,
					noteSpawnParent, isLastNote, data.splitEase, num, null, data.ease, curve);
				if (NoteEvents.ContainsKey(num))
					obj.SetNoteEvents(NoteEvents[num]["perfect"], NoteEvents[num]["good"], NoteEvents[num]["miss"]);

				//timingSystem.notes.Add(obj);
				_notes.Add(obj);
				break;
			}
			case "chain":
			{
				//obj = ObjectPooler.SpawnFromPool<Note>("Chain Note", tfNoteAppear.position);
				obj = Instantiate(chainNote, tfNoteAppear.position, Quaternion.identity);
				
				obj.transform.SetParent(noteSpawnParent, true);
				obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.noteType = NoteType.Chain;
				AnimationCurve curve = data.ease == "custom"
					? levelEventManager.CurveDictionary[data.customCurveTag]
					: null;
				obj.SetData(songTime, tfNoteAppear, tfNotePerfect, data.time * 0.001f + _delay,
					data.time * 0.001f - data.duration + _delay, data.duration, effectScript.longNoteEndTweenRect,
					noteSpawnParent, isLastNote, data.splitEase, num, data.endTime * 0.001f + _delay, data.ease, curve);
				obj.SetLongNoteLength(data.time, (int) data.endTime, data.duration);
				if (NoteEvents.ContainsKey(num))
					obj.SetNoteEvents(NoteEvents[num]["perfect"], NoteEvents[num]["good"], NoteEvents[num]["miss"]);

				//timingSystem.notes.Add(obj);
				_notes.Add(obj);
				break;
			}
		}
	}

	void DeleteNote(Notes data)
	{
		int index = _notes.FindIndex(note => note.number == data.noteNum);
		Destroy(_notes[index]);
		_notes.RemoveAt(index);
	}

	void DeleteAll()
	{
		foreach (var data in _notes)
		{
			Destroy(data);
		}

		_notes = null;
	}
}