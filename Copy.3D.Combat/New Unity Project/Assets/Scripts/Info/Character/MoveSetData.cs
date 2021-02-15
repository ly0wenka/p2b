using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveSetData : ICloneable
{
    public CombatStance combatStance = CombatStance.Stance1;
    public MoveInfo cinematicIntro;
    public MoveInfo cinematicOutro;

    public BasicMoves basicMoves = new BasicMoves(); // List of basic moves
    public MoveInfo[] attackMoves = new MoveInfo[0]; // List of attack moves

    [HideInInspector] public bool enabledBasicMovesToggle;
    [HideInInspector] public bool basicMovesToggle;
    [HideInInspector] public bool attackMovesToggle;

    public StanceInfo ConvertData() => new StanceInfo
    {
        combatStance = this.combatStance,
        cinematicIntro = this.cinematicIntro,
        cinematicOutro = this.cinematicOutro,
        basicMoves = this.basicMoves,
        attackMoves = this.attackMoves
    };

    public object Clone() => CloneObject.Clone(this);
}
