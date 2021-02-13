using System;

[System.Serializable]
public class FrameLink : ICloneable
{
    public LinkType linkType = LinkType.NoConditions;
    public bool allowBuffer = true;
    public bool onStrike = true;
    public bool onBlock = true;
    public bool onParry = true;
    public int activeFramesBegins;
    public int activeFramesEnds;
    public CounterMoveType counterMoveType;
    public MoveInfo counterMoveFilter;
    public bool disableHitImpact = true;
    public bool anyHitStrength = true;
    public HitStrengh hitStrengh;
    public bool anyStrokeHitBox = true;
    public HitBoxType hitBoxType;

    public object Clone() => CloneObject.Clone(this);
}
