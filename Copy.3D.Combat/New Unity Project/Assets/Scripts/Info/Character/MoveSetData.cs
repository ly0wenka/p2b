using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveSetData : ICloneable
{
    public CombatStance combatStance = CombatStance.Stance1;
    public MoveInfo cinematicIntro;
    public object Clone() => CloneObject.Clone(this);
}
