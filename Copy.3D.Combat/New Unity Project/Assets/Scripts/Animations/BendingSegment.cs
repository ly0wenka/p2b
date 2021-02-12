using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BendingSegment : ICloneable
{
    public Transform firstTransform;
    public Transform lastTransform;
    public BodyPart bodyPart = BodyPart.head;
    public float tresholdAngleDifference = 0;
    public float bendingMultiplier = 0.7f;
    public float maxAngleDifference = 30;
    public float maxBendingAngle = 80;
    public float responsiveness = 4;
    internal float angleH;
    internal float angleV;
    internal Vector3 dirUp;
    internal Vector3 referenceLookDir;
    internal Vector3 referenceUpDir;
    internal int chainLength;
    internal Quaternion[] origRotations;

    public BendingSegment()
    {
    }

    public BendingSegment(Transform firstTransform, Transform lastTransform, BodyPart bodyPart, float tresholdAngleDifference, float bendingMultiplier, float maxAngleDifference, float maxBendingAngle, float responsiveness, float angleH, float angleV, Vector3 dirUp, Vector3 referenceLookDir, Vector3 referenceUpDir, int chainLength, Quaternion[] origRotations)
    {
        this.firstTransform = firstTransform;
        this.lastTransform = lastTransform;
        this.bodyPart = bodyPart;
        this.tresholdAngleDifference = tresholdAngleDifference;
        this.bendingMultiplier = bendingMultiplier;
        this.maxAngleDifference = maxAngleDifference;
        this.maxBendingAngle = maxBendingAngle;
        this.responsiveness = responsiveness;
        this.angleH = angleH;
        this.angleV = angleV;
        this.dirUp = dirUp;
        this.referenceLookDir = referenceLookDir;
        this.referenceUpDir = referenceUpDir;
        this.chainLength = chainLength;
        this.origRotations = origRotations;
    }

    public BendingSegment(BendingSegment other)
    {
        this.firstTransform = other.firstTransform;
        this.lastTransform = other.lastTransform;
        this.bodyPart = other.bodyPart;
        this.tresholdAngleDifference = other.tresholdAngleDifference;
        this.bendingMultiplier = other.bendingMultiplier;
        this.maxAngleDifference = other.maxAngleDifference;
        this.maxBendingAngle = other.maxBendingAngle;
        this.responsiveness = other.responsiveness;
        this.angleH = other.angleH;
        this.angleV = other.angleV;
        this.dirUp = other.dirUp;
        this.referenceLookDir = other.referenceLookDir;
        this.referenceUpDir = other.referenceUpDir;
        this.chainLength = other.chainLength;
        this.origRotations = new Quaternion[other.origRotations.Length];

        for (int i = 0; i < this.origRotations.Length; ++i)
        {
            this.origRotations[i] = other.origRotations[i];
        }
    }

    public object Clone() => CloneObject.Clone(this);
}
