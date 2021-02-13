using FPLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveInputs
{
    public bool chargeMove;
    public Fix64 _chargeTiming = .7;
    public bool allowInputLeniency;
    public bool allowNegativeEdge = true;
    public int leniencyBuffer = 3;
    public bool onReleaseExecution;
    public bool requireButtonPress = true;
    public bool onPressExecution = true;
    public ButtonPress[] buttonSequence = new ButtonPress[0];
    public ButtonPress[] buttonExecution = new ButtonPress[0];
    [HideInInspector] public bool editorToggle = false;
    [HideInInspector] public bool buttonSequenceToggle = false;
    [HideInInspector] public bool buttonExecutionToggle = false;
}
