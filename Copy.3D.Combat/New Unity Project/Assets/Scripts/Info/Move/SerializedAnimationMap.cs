using FPLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedAnimationMap
{
    public AnimationMap[] animationMaps = new AnimationMap[0];
    public AnimationClip clip;
    public Fix64 length;
    public bool bakeSpeed = false;
}
