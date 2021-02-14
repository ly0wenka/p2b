using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class HurtBox : ICloneable
{
    public BodyPart bodyPart;
    public HitBoxShape shape;
    public Rect rect = new Rect(0, 0, 4, 4);
    public FPRect _rect = new FPRect();
    public bool followXBounds;
    public bool followYBounds;
    public float radius = .5f;
    public Fix64 _radius = .5;
    public Vector2 offSet;
    public FPVector _offSet;

    #region trackable definitions
    public bool isBlock { get; set; }
    public FPVector position { get; set; }
    public Rect rendererBounds { get; set; }
    #endregion

    public object Clone() => CloneObject.Clone(this);
}