using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
	public enum ItemType
	{
		Note,
		Event,
		NoteEvent
	}

	public ItemType itemType;

	public int index;

	public TMP_Text text;

	public void UpdateText(Notes data)
	{
		if (data.type == "Chain")
		{
			if (data.ease == "custom")
				text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.customCurveTag + "|" +
				            data.type + "|" + data.endTime;
			else
				text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.ease + "|" + data.type +
				            "|" + data.endTime;
		}
		else
		{
			if (data.ease == "custom")
				text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.customCurveTag + "|" +
				            data.type;
			else
				text.text = data.noteNum + "|" + data.time + "|" + data.duration + "|" + data.ease + "|" + data.type;
		}
	}

	public void SelectThis()
	{
		ListMaker.instance.selectAction?.Invoke(this);
	}
}