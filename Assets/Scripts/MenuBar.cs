using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup menuGroup;
    private FileManager _fileManager;
    public Dropdown dropdown;

    private void Start()
    {
        menuGroup = GetComponent<CanvasGroup>();
        _fileManager = FindObjectOfType<FileManager>();
        dropdown.value = -1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuGroup.DOKill();
        
        menuGroup.DOFade(0.9f, 0.5f).SetEase(Ease.OutQuad);
        menuGroup.interactable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menuGroup.DOKill();
        
        menuGroup.DOFade(0, 0.5f).SetEase(Ease.OutQuad);
        menuGroup.interactable = false;
    }

    public void ValueChange()
    {
        if (dropdown.value == 1)
        {
            _fileManager.LoadLevel();
            dropdown.value = -1;
        }
    }
}
