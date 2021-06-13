using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] toolMenus;
    public Toggle filesToggle;
    private CanvasGroup _menuGroup;

    private void Start()
    {
        _menuGroup = GetComponent<CanvasGroup>();
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     _menuGroup.DOKill();
    //
    //     _menuGroup.DOFade(0.9f, 0.5f).SetEase(Ease.OutQuad);
    //     _menuGroup.interactable = true;
    // }
    //
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     _menuGroup.DOKill();
    //
    //     _menuGroup.DOFade(0, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
    //     {
    //         HideToolMenus();
    //     });
    //     _menuGroup.interactable = false;
    // }

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