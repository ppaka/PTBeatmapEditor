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

	public void SelectThis()
	{
		ListMaker.instance.selectAction?.Invoke(this);
	}
}