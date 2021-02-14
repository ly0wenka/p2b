using System;
using FPLibrary;

[System.Serializable]
public class SlowMoEffect : ICloneable
{
    public int castingFrame;
    public float duration;
    public Fix64 _duration;
    public float percentage;
    public Fix64 _percentage;

    #region trackable definitions
    public bool casted{get; set;}
    #endregion

    public object Clone() => CloneObject.Clone(this);
}