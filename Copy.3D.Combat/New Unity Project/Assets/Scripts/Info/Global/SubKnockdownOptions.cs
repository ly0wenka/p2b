using FPLibrary;
using UnityEngine;

[System.Serializable]
public class SubKnockdownOptions
{
    public float knockedOutTime = 2;
    public Fix64 _knockedOutTime = 2;
    public float standUpTime = .6f;
    public Fix64 _standUpTime = .6;
    public int hideHitBoxesOnFrame = 10;
    public bool hideHitBoxes;
    public bool editorToggle;
    public bool hasQuickStand;
    public Vector2 predefinedPushForce;
    public FPVector _predefinedPushForce;
    public ButtonPress[] quickStandButtons = new ButtonPress[0]; // TODO
    public Fix64 minQuickStandTime; // TODO
    public bool hasDelayedStand;
    public ButtonPress[] delayedStandButtons = new ButtonPress[0]; // TODO
    public Fix64 maxDelayedStandTime; // TODO
}