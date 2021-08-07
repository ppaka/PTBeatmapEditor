using System;
using UnityEngine;
using UnityEngine.UI;

public class ListMaker : MonoBehaviour
{
	public static ListMaker instance;

	public GameObject itemPrefab, noteListParent, eventListParent;
	public LevelDataContainer ldc;
	public InputField time, duration;
	public Dropdown noteType, ease;

	private void Awake()
	{
		instance = this;
		LoadEvents.levelLoadComplete += MakeLists;
		selectAction += SelectData;
	}

	private void OnDisable()
	{
		LoadEvents.levelLoadComplete -= MakeLists;
	}

	private void MakeLists()
	{
		foreach (var data in ldc.levelData.notes)
		{
			var cache = Instantiate(itemPrefab, noteListParent.transform).GetComponent<ItemData>();
			cache.itemType = ItemData.ItemType.Note;
			cache.index = ldc.levelData.notes.FindIndex(note => note == data);
			if (data.type == "Chain")
			{
				if (data.ease == "custom")
				{
					cache.text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.customCurveTag +
					                  "|" + data.type + "|" + data.endTime;
				}
				else
				{
					cache.text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.ease + "|" +
					                  data.type + "|" + data.endTime;
				}
			}
			else
			{
				if (data.ease == "custom")
				{
					cache.text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.customCurveTag +
					                  "|" + data.type;
				}
				else
				{
					cache.text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.ease + "|" +
					                  data.type;
				}
			}
		}

		foreach (var data in ldc.levelData.events)
		{
			var cache = Instantiate(itemPrefab, eventListParent.transform).GetComponent<ItemData>();
			cache.itemType = ItemData.ItemType.Event;
			cache.index = ldc.levelData.events.FindIndex(events => events == data);
			cache.text.text = data.time + "|" + data.duration + "|" + data.type + "|" + data.ease;
		}
	}

	public Action<ItemData> selectAction;

	private void SelectData(ItemData iData)
	{
		if (iData.itemType == ItemData.ItemType.Note)
			Debug.Log(ldc.levelData.notes[iData.index].time);
		else if (iData.itemType == ItemData.ItemType.Event)
			Debug.Log(ldc.levelData.events[iData.index].time);
		else if (iData.itemType == ItemData.ItemType.NoteEvent)
			Debug.Log(ldc.levelData.noteEvents[iData.index].noteNum);
	}

	public void AddData(ItemData.ItemType type)
	{
		
	}

	public void DeleteData(ItemData iData)
	{
		if (iData.itemType == ItemData.ItemType.Note)
			ldc.levelData.notes.RemoveAt(iData.index);
		else if (iData.itemType == ItemData.ItemType.Event)
			ldc.levelData.events.RemoveAt(iData.index);
		else if (iData.itemType == ItemData.ItemType.NoteEvent)
			ldc.levelData.noteEvents.RemoveAt(iData.index);
	}
}