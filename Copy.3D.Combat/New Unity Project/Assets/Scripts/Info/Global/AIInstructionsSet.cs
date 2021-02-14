using System;
using UnityEngine;

[System.Serializable]
public class AIInstructionsSet : ICloneable
{
    public ScriptableObject aiInfo;
    public AIBehaviour behaviour;

    public object Clone() => CloneObject.Clone(this);
}