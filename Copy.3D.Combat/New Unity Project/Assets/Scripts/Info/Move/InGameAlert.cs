using System;

[System.Serializable]
public class InGameAlert : ICloneable
{
    public int castingFrame;
    public string alert;

    #region trackable definitions
    public bool casted { get; set; }
    #endregion

    public object Clone() => CloneObject.Clone(this);
}