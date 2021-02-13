using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerConditions
{
    public BasicMoveReference[] basicMoveLimitation = new BasicMoveReference[0];
    public BasicMoveReference[] possibleMoveStates = new BasicMoveReference[0];
    [HideInInspector] public bool basicMovesToggle = false;
    [HideInInspector] public bool statesToggle = false;
}
