using System;

[System.Serializable]
public class StanceChange : ICloneable
{
    public int castingFrame;
    public CombatStances newStance;

    #region trackable definitions
    public bool casted { get; set; }
    #endregion

    public object Clone() => CloneObject.Clone(this);
}