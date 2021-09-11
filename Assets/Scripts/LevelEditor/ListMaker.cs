using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ListMaker : MonoBehaviour
{
    public static ListMaker instance;

    public GameObject itemPrefab, noteListParent, eventListParent, eventDataListParent;
    public LevelDataContainer ldc;
    public SongTime songTime;

    public Transform dataViewHide, dataViewShow, dataViewTf;

    public Action<ItemData> selectAction;

    public ItemData selectedData;

    public GameObject noteNumberView, timeView, durationView, noteEndTimeView;
    public TMP_InputField noteNumberInput, timeInput, durationInput, noteEndTimeInput;
    public TMP_Dropdown noteTypeDropdown, easeDropdown;

    public List<string> eases = new List<string>();
    public List<ItemData> notes = new List<ItemData>();
    public List<ItemData> events = new List<ItemData>();
    public List<ItemData> noteEvents = new List<ItemData>();

    void Awake()
    {
        instance = this;
        SystemEvents.levelLoadComplete += MakeLists;
        selectAction += SelectData;
    }

    void Start()
    {
        foreach (var ease in Enum.GetNames(typeof(Ease)))
        {
            if (ease == Enum.GetName(typeof(Ease), Ease.Unset)) continue;
            if (ease == Enum.GetName(typeof(Ease), Ease.INTERNAL_Zero)) continue;
            if (ease == Enum.GetName(typeof(Ease), Ease.INTERNAL_Custom)) continue;
            eases.Add(ease);
        }
        easeDropdown.AddOptions(eases);
    }

    void OnDisable()
    {
        SystemEvents.levelLoadComplete -= MakeLists;
    }

    public void ClearItems()
    {
        dataViewTf.DOMoveX(dataViewHide.position.x, 0.15f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Fixed, true);

        foreach (var obj in noteListParent.GetComponentsInChildren<ItemData>())
        {
            Destroy(obj.gameObject);
        }

        notes = new List<ItemData>();
    }

    public void MakeLists()
    {
        selectedData = null;
        dataViewTf.DOMoveX(dataViewHide.position.x, 0.15f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Fixed, true);
        ClearItems();

        notes = new List<ItemData>();
        events = new List<ItemData>();
        noteEvents = new List<ItemData>();

        foreach (Notes data in ldc.levelData.notes)
        {
            ItemData cache = Instantiate(itemPrefab, noteListParent.transform).GetComponent<ItemData>();
            notes.Add(cache);
            cache.itemType = ItemData.ItemType.Note;
            cache.index = ldc.levelData.notes.FindIndex(note => note == data);
            cache.UpdateText(data);
        }

        foreach (Events data in ldc.levelData.events)
        {
            ItemData cache = Instantiate(itemPrefab, eventListParent.transform).GetComponent<ItemData>();
            events.Add(cache);
            cache.itemType = ItemData.ItemType.Event;
            cache.index = ldc.levelData.events.FindIndex(events => events == data);
            if (data.ease == "custom")
                cache.text.text = data.time + "|" + data.duration + "|" + data.customCurveTag + "|" + data.type;
            else
                cache.text.text = data.time + "|" + data.duration + "|" + data.ease + "|" + data.type;
        }
    }

    public void EditTime(string value)
    {
        if (selectedData == null) return;
        if (selectedData.itemType == ItemData.ItemType.Note)
        {
            EditData(value, ItemData.ItemType.Note, UniversalEditingDataType.Time);
        }
    }
    
    public void EditDuration(string value)
    {
        if (selectedData == null) return;
        if (selectedData.itemType == ItemData.ItemType.Note)
        {
            EditData(value, ItemData.ItemType.Note, UniversalEditingDataType.Duration);
        }
    }
    
    public void EditNoteEndTime(string value)
    {
        if (selectedData == null) return;
        if (selectedData.itemType == ItemData.ItemType.Note)
        {
            EditData(value, ItemData.ItemType.Note, UniversalEditingDataType.EndTime);
        }
    }

    enum UniversalEditingDataType
    {
        Time,
        Duration,
        EndTime,
        Ease
    }

    void EditData(string value, ItemData.ItemType type, UniversalEditingDataType editingDataType)
    {
        if (type == ItemData.ItemType.Note)
        {
            switch (editingDataType)
            {
                case UniversalEditingDataType.Time:
                    ldc.levelData.notes[selectedData.index].time = Convert.ToInt32(value);
                    SystemEvents.noteEdited?.Invoke(ldc.levelData.notes[selectedData.index]);
                    break;
                case UniversalEditingDataType.Duration:
                    ldc.levelData.notes[selectedData.index].duration = Convert.ToSingle(value);
                    SystemEvents.noteEdited?.Invoke(ldc.levelData.notes[selectedData.index]);
                    break;
                case UniversalEditingDataType.EndTime:
                    ldc.levelData.notes[selectedData.index].endTime = Convert.ToInt32(value);
                    SystemEvents.noteEdited?.Invoke(ldc.levelData.notes[selectedData.index]);
                    break;
            }
            
            selectedData.UpdateText(ldc.levelData.notes[selectedData.index]);
        }
    }

    void SelectData(ItemData iData)
    {
        if (selectedData == null)
        {
            dataViewTf.DOMoveX(dataViewShow.position.x, 0.15f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Fixed, true);
        }

        selectedData = iData;

        if (iData.itemType == ItemData.ItemType.Note)
        {
            noteNumberInput.readOnly = true;

            Notes note = ldc.levelData.notes[iData.index];

            noteNumberInput.text = note.noteNum.ToString();
            timeInput.text = note.time.ToString();
            durationInput.text = note.duration.ToString();
            noteEndTimeInput.text = "";

            noteNumberView.SetActive(true);
            timeView.SetActive(true);
            durationView.SetActive(true);
            noteEndTimeView.SetActive(false);
            
            if (note.type == NoteType.Chain)
            {
                noteEndTimeInput.text = note.endTime.ToString();
                noteEndTimeView.SetActive(true);
            }
        }
        else if (iData.itemType == ItemData.ItemType.Event)
        {
            noteNumberInput.readOnly = true;

            var evt = ldc.levelData.events[iData.index];
            
            noteNumberInput.text = "";
            timeInput.text = evt.time.ToString();
            durationInput.text = evt.duration.ToString();
            noteEndTimeInput.text = "";

            noteNumberView.SetActive(false);
            timeView.SetActive(true);
            durationView.SetActive(true);
            noteEndTimeView.SetActive(false);
        }
        else if (iData.itemType == ItemData.ItemType.NoteEvent)
        {
            noteNumberInput.readOnly = false;

            var evt = ldc.levelData.noteEvents[iData.index];
            
            noteNumberInput.text = evt.noteNum.ToString();
            timeInput.text = "";
            durationInput.text = evt.duration.ToString();
            noteEndTimeInput.text = "";

            noteNumberView.SetActive(true);
            timeView.SetActive(false);
            durationView.SetActive(true);
            noteEndTimeView.SetActive(false);
        }
    }

    public void AddNoteData()
    {
        int time = Mathf.RoundToInt(AudioManager.Instance.audioFile.Position * AudioManager.Instance.audioFile.WaveFormat.AverageBytesPerSecond * 1000) - LevelTimings.startOffset;

        ItemData iData = Instantiate(itemPrefab, noteListParent.transform).GetComponent<ItemData>();
        iData.itemType = ItemData.ItemType.Note;

        NoteType type = NoteType.Normal;
        type = noteTypeDropdown.value switch
        {
            (int)NoteType.Normal => NoteType.Normal,
            (int)NoteType.Flick => NoteType.Flick,
            (int)NoteType.Chain => NoteType.Chain,
            _ => type
        };

        if (ldc.levelData.notes.Count == 0)
        {
            Notes data = new Notes { noteNum = 1, duration = 1, time = time, ease = "L", type = type };
            iData.index = 0;
            iData.UpdateText(data);
            notes.Add(iData);

            if (type == NoteType.Chain)
                data.endTime = time + 1;

            ldc.levelData.notes.Add(data);

            SystemEvents.noteAdded?.Invoke(data);
        }
        else
        {
            int previousIndex = -1;

            int indexingTime = time;

            while (true)
            {
                previousIndex = ldc.levelData.notes.FindLastIndex(note => note.time == indexingTime);
                
                if (previousIndex != -1)
                {
                    break;
                }

                if (indexingTime < -1 * LevelTimings.startOffset)
                {
                    previousIndex = -1;
                    break;
                }

                indexingTime -= 1;
            }

            int index = previousIndex + 1;
            iData.index = index;
            
            print("노트 밀기");

            if (index < notes.Count && index < ldc.levelData.notes.Count)
            {
                for (int i = index; i < notes.Count; i++)
                {
                    notes[i].index += 1;
                    ldc.levelData.notes[i].noteNum += 1;
                }
            }

            notes.Add(iData);
            notes = notes.OrderBy(x => x.index.ToString(), new StringAsNumericComparer()).ToList();

            Notes newData = new Notes { noteNum = 1, duration = 1, time = time, ease = "L", type = type };

            print("노트 데이터 생성");
            
            if (previousIndex != -1)
            {
                newData.noteNum = ldc.levelData.notes[previousIndex].noteNum + 1;
            }
            
            if (type == NoteType.Chain)
                newData.endTime = time + 1;
            
            iData.transform.SetSiblingIndex(index);

            ldc.levelData.notes.Add(newData);
            ldc.levelData.notes = ldc.levelData.notes.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer())
                .ThenBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();

            foreach (var itemData in notes)
            {
                itemData.UpdateText(ldc.levelData.notes[itemData.index]);
            }

            SystemEvents.noteAdded?.Invoke(newData);
        }
    }

    public void AddEventData()
    {
    }

    public void DeleteData()
    {
        if (selectedData == null) return;

        ItemData iData = selectedData;
        int index = iData.index;

        switch (iData.itemType)
        {
            case ItemData.ItemType.Note:
            {
                int lIndex = notes.FindIndex(data => data == iData);

                if (notes.Count != 1 && ldc.levelData.notes.Count != 1)
                {
                    for (int i = index + 1; i < notes.Count; i++)
                    {
                        ItemData item = notes[i];
                        Notes note = ldc.levelData.notes[i];

                        note.noteNum -= 1;
                        item.index -= 1;

                        item.UpdateText(note);
                    }
                }

                notes.RemoveAt(lIndex);

                Notes selectedNote = ldc.levelData.notes[index];
                ldc.levelData.notes.RemoveAt(index);
                SystemEvents.noteRemoved?.Invoke(selectedNote);
                break;
            }
            case ItemData.ItemType.Event:
            {
                int lIndex = events.FindIndex(data => data == iData);

                for (int i = index + 1; i < events.Count; i++)
                {
                    var item = events[i];
                    item.index -= 1;
                }

                events.RemoveAt(lIndex);
                ldc.levelData.events.RemoveAt(index);
                break;
            }
            case ItemData.ItemType.NoteEvent:
            {
                int lIndex = noteEvents.FindIndex(data => data == iData);

                for (int i = index + 1; i < noteEvents.Count; i++)
                {
                    var item = noteEvents[i];
                    item.index -= 1;
                }

                for (int i = index; i < ldc.levelData.noteEvents.Count; i++)
                {
                    var note = ldc.levelData.noteEvents[i];
                    note.noteNum -= 1;
                }

                noteEvents.RemoveAt(lIndex);
                ldc.levelData.noteEvents.RemoveAt(index);
                break;
            }
        }

        Destroy(iData.gameObject);
        selectedData = null;
        dataViewTf.DOMoveX(dataViewHide.position.x, 0.15f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Fixed, true);
    }
}