using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NonAffectedJoints : ICloneable
{
    public Transform joint;
    public BodyPart bodyPart;
    public float effect = 0;

    public NonAffectedJoints() { }

    public NonAffectedJoints(NonAffectedJoints other)
    {
        this.joint = other.joint;
        this.bodyPart = other.bodyPart;
        this.effect = other.effect;
    }

    public object Clone() => CloneObject.Clone(this);
}
