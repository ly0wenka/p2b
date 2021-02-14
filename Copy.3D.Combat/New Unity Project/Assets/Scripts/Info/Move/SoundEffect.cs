using System;
using UnityEngine;

[System.Serializable]
public class SoundEffect : ICloneable
{
    public int castingFrame;
    public AudioClip[] sounds = new AudioClip[0];

    [HideInInspector] public bool soundEffectsToggle;

    #region trackable definitions
    public bool casted { get; set; }
    #endregion

    public object Clone() => CloneObject.Clone(this);
}