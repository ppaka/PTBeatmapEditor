using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
	public GameObject[] toolMenus;
	public Toggle filesToggle;
	CanvasGroup _menuGroup;

	void Start()
	{
		_menuGroup = GetComponent<CanvasGroup>();
	}

	public void ShowToolMenu(GameObject obj)
	{
		obj.SetActive(!obj.activeSelf);
	}

	public void HideToolMenus()
	{
		filesToggle.isOn = false;
		foreach (GameObject i in toolMenus) i.SetActive(false);
	}
}