using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListMaker : MonoBehaviour
{
	public static ListMaker instance;

	public GameObject itemPrefab, noteListParent, eventListParent;
	public LevelDataContainer ldc;
	public SongTime songTime;

	public Action<ItemData> selectAction;

	public List<ItemData> notes = new List<ItemData>();
	public List<ItemData> events = new List<ItemData>();
	public List<ItemData> noteEvents = new List<ItemData>();

	void Awake()
	{
		instance = this;
		LoadEvents.levelLoadComplete += MakeLists;
		selectAction += SelectData;
	}

	void OnDisable()
	{
		LoadEvents.levelLoadComplete -= MakeLists;
	}

	void MakeLists()
	{
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

	void SelectData(ItemData iData)
	{
		if (iData.itemType == ItemData.ItemType.Note)
			Debug.Log(ldc.levelData.notes[iData.index].time);
		else if (iData.itemType == ItemData.ItemType.Event)
			Debug.Log(ldc.levelData.events[iData.index].time);
		else if (iData.itemType == ItemData.ItemType.NoteEvent)
			Debug.Log(ldc.levelData.noteEvents[iData.index].noteNum);
	}

	public void AddNoteData()
	{
		int time = (int) songTime.audioSource.time * 1000 - ldc.levelData.settings.noteOffset;
		
		ItemData iData = Instantiate(itemPrefab, noteListParent.transform).GetComponent<ItemData>();
		iData.itemType = ItemData.ItemType.Note;

		if (ldc.levelData.notes.Count == 0)
		{
			Notes data = new Notes {noteNum = 1, duration = 1, time = time, ease = "L", type = "normal"};
			iData.index = 0;
			iData.UpdateText(data);
			
			ldc.levelData.notes.Add(data);
		}
		else
		{
			int previousIndex = -1;

			int indexingTime = time;

			while (true)
			{
				previousIndex = ldc.levelData.notes.FindLastIndex(note => note.time == indexingTime);
				if (previousIndex != -1) break;
				if (indexingTime <= 0)
				{
					previousIndex = -1;
					break;
				}

				indexingTime -= 1;
			}

			int index = previousIndex + 1;
			iData.index = index;

			for (int i = index; i < notes.Count; i++)
			{
				ItemData item = notes[i];
				Notes note = ldc.levelData.notes[i];
				
				note.noteNum += 1;
				item.index += 1;

				item.UpdateText(note);
			}
			
			notes.Add(iData);
			notes = notes.OrderBy(x => x.index.ToString(), new StringAsNumericComparer()).ToList();

			Notes newData = new Notes {noteNum = 1, duration = 1, time = time, ease = "L", type = "normal"};

			if (previousIndex != -1)
			{
				newData = new Notes {noteNum = ldc.levelData.notes[previousIndex].noteNum + 1, duration = 1, time = time, ease = "L", type = "normal"};
			}

			
			iData.text.text = newData.noteNum + "|" + newData.time + "|" + newData.duration + "|" + newData.ease + "|" + newData.type;

			ldc.levelData.notes.Add(newData);
			ldc.levelData.notes = ldc.levelData.notes.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer())
				.ThenBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();
			iData.transform.SetSiblingIndex(index);
		}
	}

	public void AddData(ItemData.ItemType type)
	{
		int time = (int) songTime.audioSource.time * 1000;

		if (type == ItemData.ItemType.Note)
		{
			ItemData iData = Instantiate(itemPrefab, noteListParent.transform).GetComponent<ItemData>();
			notes.Add(iData);
			iData.itemType = ItemData.ItemType.Note;

			if (ldc.levelData.notes.Count == 0)
			{
			}
			else
			{
				int previousIndex = -1;

				int indexingTime = time;

				while (true)
				{
					previousIndex = ldc.levelData.notes.FindLastIndex(note => note.time == indexingTime);
					if (previousIndex != -1) break;
					if (indexingTime <= 0)
					{
						previousIndex = -1;
						break;
					}

					indexingTime -= 1;
				}

				int index = previousIndex + 1;
				iData.index = index;

				for (int i = index + 1; i < notes.Count; i++)
				{
					var item = notes[i];
					item.index += 1;
				}

				for (int i = index; i < ldc.levelData.notes.Count; i++)
				{
					var note = ldc.levelData.notes[i];
					note.noteNum += 1;
				}

				Notes data = new Notes {noteNum = (uint) index, duration = 1, time = time, ease = "L", type = "normal"};

				iData.text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.ease + "|" +
				                  data.type;

				ldc.levelData.notes.Add(data);
				iData.transform.SetSiblingIndex(index);
			}
		}
	}

	public void DeleteData(ItemData iData)
	{
		int index = iData.index;

		switch (iData.itemType)
		{
			case ItemData.ItemType.Note:
			{
				int lIndex = notes.FindIndex(data => data == iData);

				for (int i = index + 1; i < notes.Count; i++)
				{
					var item = notes[i];
					item.index -= 1;
				}

				for (int i = index; i < ldc.levelData.notes.Count; i++)
				{
					var note = ldc.levelData.notes[i];
					note.noteNum -= 1;
				}

				notes.RemoveAt(lIndex);
				ldc.levelData.notes.RemoveAt(index);
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
	}
}