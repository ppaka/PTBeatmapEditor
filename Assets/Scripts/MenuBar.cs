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
        switch (dropdown.value)
        {
            case 1:
                dropdown.value = -1;
                _fileManager.LoadLevel();
                break;
            case 2:
                dropdown.value = -1;
                _fileManager.SaveLevel();
                break;
            case 3:
                dropdown.value = -1;
                _fileManager.SaveLevelAs();
                break;
            case 4:
                dropdown.value = -1;
                Application.Quit();
                break;
        }
    }
}