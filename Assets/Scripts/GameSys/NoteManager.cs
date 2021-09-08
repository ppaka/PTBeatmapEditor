using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public RectTransform tfNoteAppear, tfNotePerfect;
    public Transform noteSpawnParent;

    [Header("Scripts", order = 3)] public SongTime songTime;
    public EffectScript effectScript;
    public LevelEventManager levelEventManager;
    public GameBar gameBar;

    public Note normalNote, flickNote, chainNote;

    List<Note> _notes = new List<Note>();

    Dictionary<uint, Dictionary<string, List<NoteEvents>>> NoteEvents;

    void OnEnable()
    {
        SystemEvents.levelLoadComplete += Load;
        SystemEvents.noteRemoved += DeleteNote;
        SystemEvents.noteRemoveAll += DeleteAll;
        SystemEvents.noteAdded += AddNote;
        SystemEvents.noteEdited += EditNote;
    }

    void OnDisable()
    {
        SystemEvents.levelLoadComplete -= Load;
        SystemEvents.noteRemoved -= DeleteNote;
        SystemEvents.noteRemoveAll -= DeleteAll;
        SystemEvents.noteAdded -= AddNote;
        SystemEvents.noteEdited -= EditNote;
    }

    void Load()
    {
        NoteEvents = LevelDataContainer.Instance.noteEvents;
        Init();
    }

    void Update()
    {
        if (_notes == null || _notes.Count == 0) return;

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

        DeleteAll();

        _notes = new List<Note>();

        foreach (var data in LevelDataContainer.Instance.levelData.notes)
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
            case NoteType.Normal:
            {
                //obj = ObjectPooler.SpawnFromPool<Note>("Normal Note", tfNoteAppear.position);
                obj = Instantiate(normalNote, tfNoteAppear.position, Quaternion.identity);

                obj.transform.SetParent(noteSpawnParent, true);
                obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                obj.noteType = NoteType.Normal;
                AnimationCurve curve = data.ease == "custom"
                    ? levelEventManager.CurveDictionary[data.customCurveTag]
                    : null;
                obj.SetData(songTime, gameBar, tfNoteAppear, tfNotePerfect, data.time * 0.001f,
                    data.time * 0.001f - data.duration, data.duration, effectScript.noteEndTweenRect,
                    noteSpawnParent, isLastNote, data.splitEase, num, null, data.ease, curve);
                /*if (NoteEvents.ContainsKey(num))
                    obj.SetNoteEvents(NoteEvents[num]["perfect"], NoteEvents[num]["good"], NoteEvents[num]["miss"]);*/

                //timingSystem.notes.Add(obj);
                _notes.Add(obj);
                break;
            }
            case NoteType.Flick:
            {
                //obj = ObjectPooler.SpawnFromPool<Note>("Flick Note", tfNoteAppear.position);
                obj = Instantiate(flickNote, tfNoteAppear.position, Quaternion.identity);

                obj.transform.SetParent(noteSpawnParent, true);
                obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                obj.noteType = NoteType.Flick;
                AnimationCurve curve = data.ease == "custom"
                    ? levelEventManager.CurveDictionary[data.customCurveTag]
                    : null;
                obj.SetData(songTime, gameBar, tfNoteAppear, tfNotePerfect, data.time * 0.001f,
                    data.time * 0.001f - data.duration, data.duration, effectScript.noteEndTweenRect,
                    noteSpawnParent, isLastNote, data.splitEase, num, null, data.ease, curve);
                /*if (NoteEvents.ContainsKey(num))
                    obj.SetNoteEvents(NoteEvents[num]["perfect"], NoteEvents[num]["good"], NoteEvents[num]["miss"]);*/

                //timingSystem.notes.Add(obj);
                _notes.Add(obj);
                break;
            }
            case NoteType.Chain:
            {
                //obj = ObjectPooler.SpawnFromPool<Note>("Chain Note", tfNoteAppear.position);
                obj = Instantiate(chainNote, tfNoteAppear.position, Quaternion.identity);

                obj.transform.SetParent(noteSpawnParent, true);
                obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                obj.noteType = NoteType.Chain;
                AnimationCurve curve = data.ease == "custom"
                    ? levelEventManager.CurveDictionary[data.customCurveTag]
                    : null;
                obj.SetData(songTime, gameBar, tfNoteAppear, tfNotePerfect, data.time * 0.001f,
                    data.time * 0.001f - data.duration, data.duration, gameBar.longNoteEndTweenRect,
                    noteSpawnParent, isLastNote, data.splitEase, num, (int)data.endTime * 0.001f, data.ease, curve);
                obj.SetLongNoteLength(data.time, (int)data.endTime, data.duration);
                /*if (NoteEvents.ContainsKey(num))
                    obj.SetNoteEvents(NoteEvents[num]["perfect"], NoteEvents[num]["good"], NoteEvents[num]["miss"]);*/

                //timingSystem.notes.Add(obj);
                _notes.Add(obj);
                break;
            }
        }
    }

    void AddNote(Notes data)
    {
        _notes ??= new List<Note>();

        foreach (var note in _notes)
        {
            if (note.number >= data.noteNum)
                note.number += 1;
        }

        MakeNote(data);
    }

    void EditNote(Notes data)
    {
        int index = _notes.FindIndex(note => note.number == data.noteNum);
        Note obj = _notes[index];

        switch (obj.noteType)
        {
            case NoteType.Normal:
            {
                AnimationCurve curve = data.ease == "custom"
                    ? levelEventManager.CurveDictionary[data.customCurveTag]
                    : null;
                obj.SetData(songTime, gameBar, tfNoteAppear, tfNotePerfect, data.time * 0.001f,
                    data.time * 0.001f - data.duration, data.duration, effectScript.noteEndTweenRect,
                    noteSpawnParent, obj.isLastNote, data.splitEase, obj.number, null, data.ease, curve);
                break;
            }
            case NoteType.Flick:
            {
                AnimationCurve curve = data.ease == "custom"
                    ? levelEventManager.CurveDictionary[data.customCurveTag]
                    : null;
                obj.SetData(songTime, gameBar, tfNoteAppear, tfNotePerfect, data.time * 0.001f,
                    data.time * 0.001f - data.duration, data.duration, effectScript.noteEndTweenRect,
                    noteSpawnParent, obj.isLastNote, data.splitEase, obj.number, null, data.ease, curve);
                break;
            }
            case NoteType.Chain:
            {
                AnimationCurve curve = data.ease == "custom"
                    ? levelEventManager.CurveDictionary[data.customCurveTag]
                    : null;
                obj.SetData(songTime, gameBar, tfNoteAppear, tfNotePerfect, data.time * 0.001f,
                    data.time * 0.001f - data.duration, data.duration, null,
                    noteSpawnParent, obj.isLastNote, data.splitEase, obj.number, data.endTime * 0.001f,
                    data.ease, curve);
                obj.SetLongNoteLength(data.time, (int)data.endTime, data.duration);
                break;
            }
        }
    }

    void DeleteNote(Notes data)
    {
        int index = _notes.FindIndex(note => note.number == data.noteNum);
        Destroy(_notes[index].gameObject);

        for (int i = index + 1; i < _notes.Count; i++)
        {
            Note note = _notes[i];
            note.number = note.number - 1;
        }

        _notes.RemoveAt(index);
    }

    void DeleteAll()
    {
        if (_notes == null) return;

        foreach (Note data in _notes)
        {
            Destroy(data.gameObject);
        }

        _notes = null;
    }
}