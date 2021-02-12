using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook
{
    public bool enabled = false;
    public BendingSegment[] segments;
    public NonAffectedJoints[] nonAffectedJoints = new NonAffectedJoints[0];
    public BodyPart target = BodyPart.head;
    public float effect = 1;
    public bool overrideAnimation = true;
    public bool disableOnHit = true;
}
