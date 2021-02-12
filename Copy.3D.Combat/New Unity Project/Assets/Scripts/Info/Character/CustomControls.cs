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
    public ButtonPress crouch = ButtonPress.Down;
    public ButtonPress jump = ButtonPress.Up;
    public ButtonPress button1 = ButtonPress.Button1;
    public ButtonPress button2 = ButtonPress.Button2;
    public ButtonPress button3 = ButtonPress.Button3;
    public ButtonPress button4 = ButtonPress.Button4;
    public ButtonPress button5 = ButtonPress.Button5;
    public ButtonPress button6 = ButtonPress.Button6;
    public ButtonPress button7 = ButtonPress.Button7;
    public ButtonPress button8 = ButtonPress.Button8;
    public ButtonPress button9 = ButtonPress.Button9;
    public ButtonPress button10 = ButtonPress.Button10;
    public ButtonPress button11 = ButtonPress.Button11;
    public ButtonPress button12 = ButtonPress.Button12;
    public bool overrideControlFreak = false;
    public InputTouchControllerBridge controlFreak2Prefab = null;
}
