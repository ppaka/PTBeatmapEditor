using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public CanvasGroup beatmapSettings;
    
    public void OpenBeatmapSettingsPanel()
    {
        beatmapSettings.gameObject.SetActive(true);
    }
    
    public void CloseBeatmapSettingsPanel()
    {
        beatmapSettings.gameObject.SetActive(false);
    }
}
