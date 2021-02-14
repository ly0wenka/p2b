using FPLibrary;
using UnityEngine;

[System.Serializable]
public class BlockArea
{
    public int activeFramesBegin;
    public int activeFramesEnds;

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

    [HideInInspector] public FPVector position;
}