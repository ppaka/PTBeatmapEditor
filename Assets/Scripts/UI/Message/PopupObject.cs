using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupObject : MonoBehaviour
{
    [SerializeField] Image _image;
    
    public TMP_Text titleText, descriptionText;
    public GameObject contents;

    public void Show(RectTransform startRect, RectTransform endRect)
    {
        _image.rectTransform.DOAnchorPos(endRect.position, 1).OnStart(() =>
        {
            startRect.DOAnchorPos(startRect.position, 0f);
        }).Play();
    }
}