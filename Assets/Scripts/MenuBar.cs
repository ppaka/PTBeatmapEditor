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
    public CanvasGroup settingsCg;

    private void Start()
    {
        _menuGroup = GetComponent<CanvasGroup>();
        _fileManager = FindObjectOfType<FileManager>();
        dropdown.value = 9;
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

    public void ShowToolMenu(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void CloseToolMenu(GameObject obj)
    {
        obj.SetActive(true);
    }
    
    public void ValueChange()
    {
        
        switch (dropdown.value)
        {
            case 0:
                dropdown.value = 9;
                settingsCg.gameObject.SetActive(true);
                break;
            case 1:
                dropdown.value = 9;
                _fileManager.LoadLevel();
                break;
            case 2:
                dropdown.value = 9;
                _fileManager.SaveLevel();
                break;
            case 3:
                dropdown.value = 9;
                _fileManager.SaveLevelAs();
                break;
            case 4:
                dropdown.value = 9;
                Application.Quit();
                break;
        }
    }
}