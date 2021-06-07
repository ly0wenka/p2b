using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public float angular;
    public Vector3 linear;
    public Steering(float angular = 0.0f, Vector3 linear = new Vector3())
    {
        this.angular = angular;
        this.linear = linear;
    }
}
