using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarCamControl : MonoBehaviour
{
    public Camera mainCam;
    public CanvasScaler scaler;
    private bool _isOn;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_isOn == false)
                {
                    mainCam.DORect(new Rect(0, 0, 1, 0.94f), 0.2f).SetEase(Ease.OutQuint);
                    //scaler.scaleFactor = 0.94f;
                    _isOn = true;
                }
                else
                {
                    mainCam.DORect(new Rect(0, 0, 1, 1), 0.2f).SetEase(Ease.OutQuint);
                    //scaler.scaleFactor = 1f;
                    _isOn = false;
                }
            }
    }
}