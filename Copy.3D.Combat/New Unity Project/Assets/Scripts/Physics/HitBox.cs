using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class HitBox : ICloneable
{
    public bool defaultVisibility = true;
    public BodyPart bodyPart;
    public HitBoxType type;
    public HitBoxShape shape;
    public Rect rect = new Rect(0, 0, 4, 4);
    public FPRect _rect = new FPRect();
    public bool followXBounds;
    public bool followYBounds;
    public float radius = .5f;
    public Fix64 _radius = .5;
    public Vector2 offSet;
    public FPVector _offSet;

    public CollisionType collisionType;
    public Transform position;
    public FPVector mappedPosition;

    #region trackable definitions

    public int state { get; set; }
    public Rect rendererBounds { get; set; }
    public bool hide { get; set; } // Whether the hit box collisions will be detected
    public bool visibility { get; set; } // Whether the GameObject will be active in the hierarchy

    #endregion

    public object Clone() => CloneObject.Clone(this);
}