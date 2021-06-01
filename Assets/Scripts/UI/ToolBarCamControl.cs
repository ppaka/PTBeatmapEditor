using DG.Tweening;
using UnityEngine;

public class ToolBarCamControl : MonoBehaviour
{
    public Camera mainCam;
    private bool _isOn;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_isOn == false)
                {
                    mainCam.DORect(new Rect(0, 0, 1, 0.94f), 0.2f).SetEase(Ease.OutQuint);
                    _isOn = true;
                }
                else
                {
                    mainCam.DORect(new Rect(0, 0, 1, 1), 0.2f).SetEase(Ease.OutQuint);
                    _isOn = false;
                }
            }
        }
    }
}
