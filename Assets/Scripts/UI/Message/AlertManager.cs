using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum ClickBehaviour
{
    ShowPopup,
    None
}

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    void Awake()
    {
        instance = this;
    }

    public RectTransform scrollView, show, hide;

    public GameObject contents;
    public AlertObject alertObject;

    public Sprite testIco;

    void Start()
    {
        Alert alert = new Alert
        {
            title = "업데이터",
            description = "업데이트 확인 중",
            behaviour = ClickBehaviour.None,
            icon = testIco,
            color = new Color(0.8f, 0.3f, 0.3f, 1)
        };
        alertObject = ShowAlert(alert);
    }

    bool _isOpen;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!_isOpen)
                {
                    scrollView.transform.DOKill();
                    scrollView.transform.DOMoveY(show.transform.position.y, 0.25f).SetEase(Ease.InOutSine);
                    _isOpen = true;
                }
                else
                {
                    scrollView.transform.DOKill();
                    scrollView.transform.DOMoveY(hide.transform.position.y, 0.25f).SetEase(Ease.InOutSine);
                    _isOpen = false;
                }
            }
    }

    public AlertObject ShowAlert(Alert alert)
    {
        AlertObject obj = Instantiate(alertObject.gameObject, contents.transform).GetComponent<AlertObject>();
        obj.alert = alert;
        obj.button.onClick.AddListener(() => OnClickAlert(obj.alert.behaviour, obj.alert.popup));
        obj.bg.color = alert.color;
        obj.icon.sprite = alert.icon;
        obj.titleText.text = alert.title;
        obj.descriptionText.text = alert.description;
        return obj;
    }

    public void OnClickAlert(ClickBehaviour behaviour, AlertOnClickPopup popup = null)
    {
        if (behaviour == ClickBehaviour.ShowPopup)
        {
            ShowPopup(popup);
        }
    }

    public PopupObject popupObject;
    public CanvasGroup popupOverlayCanvas;
    public PopupButton popupButton;

    public void ShowPopup(AlertOnClickPopup popup)
    {
        var data = Instantiate(popupObject.gameObject, popupOverlayCanvas.transform).GetComponent<PopupObject>();
        data.titleText.text = popup.title;
        data.descriptionText.text = popup.description;
        for (int i = 0; i < popup.buttons.Count; i++)
        {
            var buttonObject = Instantiate(popupButton.gameObject, data.contents.transform).GetComponent<PopupButton>();
            buttonObject.button.image.color = popup.buttons[i].color;
            buttonObject.text.text = popup.buttons[i].text;
            int i1 = i;
            buttonObject.button.onClick.AddListener(() => popup.buttons[i1].clickAction.Invoke());
        }
    }
}