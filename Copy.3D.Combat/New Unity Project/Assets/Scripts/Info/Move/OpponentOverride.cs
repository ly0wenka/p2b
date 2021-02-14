using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class OpponentOverride : ICloneable
{
    public Vector3 position;
    public FPVector _position;
    public int castingFrame;
    public int blendSpeed = 80;
    public bool stun;
    public float stunTime;
    public Fix64 _stunTime;
    public bool overrideHitAnimations;
    public bool resetAppliedForces;

    // End Options
    public StandUpOptions standUpOptions;

    // Options
    public bool characterSpecific;

    // Move
    public MoveInfo move;

    // Character Specific Moves
    public CharacterSpecificMoves[] characterSpecificMoves = new CharacterSpecificMoves[0];

    [HideInInspector] public bool animationPreview = false;
    [HideInInspector] public bool movesToggle = false;

    public bool casted { get; set; }

    public object Clone() => CloneObject.Clone(this);
}