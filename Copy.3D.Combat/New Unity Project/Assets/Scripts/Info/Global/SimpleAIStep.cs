using System;
using UnityEngine;

[System.Serializable]
public class SimpleAIStep{
    public ButtonPress[] buttons = new ButtonPress[0];
    public int frames;

    [HideInInspector]
    public bool showInInspector;
}