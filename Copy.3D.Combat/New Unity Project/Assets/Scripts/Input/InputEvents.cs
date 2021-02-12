using FPLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents
{
    public static InputEvents Default
    {
        get { return InputEvents._Default}
    }

    private static InputEvents _Default = new InputEvents();

    public Fix64 axisRaw { get; protected set; }
    public bool button { get; protected set; }

    public InputEvents() : this(0f, false) {}

    public InputEvents(bool button) : this(0f, button) {}

    public InputEvents(Fix64 axisRaw) : this(axisRaw, axisRaw != 0f) {}

    public InputEvents(InputEvents other) : this(other.axisRaw, other.button) {}

    protected InputEvents(Fix64 axisRaw, bool button)
    {
        this.axisRaw = axisRaw;
        this.button = button;
    }
}
