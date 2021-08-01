using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    public GameObject[] toolMenus;
    public Toggle filesToggle;
    private CanvasGroup _menuGroup;

    private void Start()
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
        foreach (var i in toolMenus) i.SetActive(false);
    }
}