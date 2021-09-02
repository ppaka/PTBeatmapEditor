using System;
using System.Collections.Generic;
using UnityEngine;

public class Alert
{
    public string title;
    public string description;
    public Sprite icon;
    public Color color;
    public AlertOnClickPopup popup;
    public ClickBehaviour behaviour;
}

public class AlertOnClickPopup
{
    public string title;
    public string description;
    public List<PopupButtonElements> buttons;
}

public class PopupButtonElements
{
    public string text;
    public Color color;
    public Action clickAction;
}