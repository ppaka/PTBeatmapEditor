using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuBar : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup _menuGroup;
    public GameObject[] toolMenus;

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
        foreach (var i in toolMenus)
        {
            i.SetActive(false);
        }
    }
}