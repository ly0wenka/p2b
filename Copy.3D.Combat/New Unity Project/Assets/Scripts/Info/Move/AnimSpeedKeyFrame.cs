using FPLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeedKeyFrame : ICloneable
{
    public int castingFrame = 0;
    public float speed = 1;
    public Fix64 _speed = 1;

    public object Clone() => CloneObject.Clone(this);
}
