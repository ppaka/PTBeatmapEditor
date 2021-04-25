using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup _menuGroup;
    private FileManager _fileManager;
    public Dropdown dropdown;

    private void Start()
    {
        _menuGroup = GetComponent<CanvasGroup>();
        _fileManager = FindObjectOfType<FileManager>();
        dropdown.value = -1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _menuGroup.DOKill();
        
        _menuGroup.DOFade(0.9f, 0.5f).SetEase(Ease.OutQuad);
        _menuGroup.interactable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _menuGroup.DOKill();
        
        _menuGroup.DOFade(0, 0.5f).SetEase(Ease.OutQuad);
        _menuGroup.interactable = false;
    }

    public void ValueChange()
    {
        if (dropdown.value == 1)
        {
            _fileManager.LoadLevel();
            dropdown.value = -1;
        }
        
        if (dropdown.value == 2)
        {
            _fileManager.SaveLevel();
            dropdown.value = -1;
        }
        
        if (dropdown.value == 3)
        {
            _fileManager.SaveLevelAs();
            dropdown.value = -1;
        }
    }
}
