using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class MoveParticleEffect : ICloneable
{
    public int castingFrame;
    public ParticleInfo particleEffect;

    public bool casted { get; set; }
    
    public object Clone() => CloneObject.Clone(this);
}
