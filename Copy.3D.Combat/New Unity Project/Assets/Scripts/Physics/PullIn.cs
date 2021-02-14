using System;
using FPLibrary;

[System.Serializable]
public class PullIn : ICloneable
{
    public int speed = 50;
    public bool forceStand = true;
    public BodyPart characterBodyPart;
    public BodyPart enemyBodyPart;
    public float targetDistance = .5f;
    public Fix64 _targetDistance = .5;

    public FPVector position;

    public object Clone() => CloneObject.Clone(this);
}