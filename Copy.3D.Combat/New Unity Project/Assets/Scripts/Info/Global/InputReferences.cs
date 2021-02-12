using System;
using UnityEngine;

public class InputReferences : ICloneable
{
    public InputType inputType;
    public string inputButtonName;
    public ButtonPress engineRelatedButton;

    public string joystickAxisName;

    public string cInputPositiveKeyName;
    public string cInputPositiveDefaultKey;
    public string cInputPositiveAlternativeKey;

    public string cInputNegativeKeyName;
    public string cInputNegativeDefaultKey;
    public string cInputNegativeAlternativeKey;

    public Texture2D inputViewerIcon1;
    public Texture2D inputViewerIcon2;
    public Texture2D activeIcon;

    public object Clone() => CloneObject.Clone(this);
}
