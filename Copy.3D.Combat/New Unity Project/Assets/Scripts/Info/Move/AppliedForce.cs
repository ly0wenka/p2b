using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class AppliedForce : ICloneable
{
    public int castingFrame;
    public bool resetPreviousVertical;
    public bool resetPreviousHorizontal;
    public Vector2 force;
    public FPVector _force;

    #region trackable definitions
    public bool casted{get; set;}
    #endregion

    public object Clone() => CloneObject.Clone(this);
}