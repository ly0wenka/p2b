using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomControls
{
    public bool enabled = false;
    public bool overrideInputs = false;
    public ButtonPress walkForward = ButtonPress.Forward;
    public ButtonPress walkBack = ButtonPress.Back;
}
