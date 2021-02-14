using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class BasicMoveInfo : ICloneable
{
    public AnimationClip clip1;
    public AnimationClip clip2;
    public AnimationClip clip3;
    public AnimationClip clip4;
    public AnimationClip clip5;
    public AnimationClip clip6;
    public SerializedAnimationMap[] animMap = new SerializedAnimationMap[6];
    public float animationSpeed = 1;
    public Fix64 _animationSpeed = 1;
    public WrapMode wrapMode;

    public bool autoSpeed = true;
    public float restingClipInterval = 6;
    public Fix64 _restingClipInterval = 6;
    public bool overrideBlendingIn = false;
    public bool overrideBlendingOut = false;
    public float blendingIn = 0;
    public Fix64 _blendingIn = 0;
    public float blendingOut = 0;
    public Fix64 _blendingOut = 0;
    public bool invincible;
    public bool disableHeadLook;
    public bool applyRootMotion;
    public bool downClip;
    public AudioClip[] soundEffects = new AudioClip[0];
    public bool continuousSound;
    public ParticleInfo particleEffect = new ParticleInfo();
    public BasicMoveReference reference;


    [HideInInspector] public string name;
    [HideInInspector] public bool editorToggle;
    [HideInInspector] public bool soundEffectsToggle;

    public object Clone() => CloneObject.Clone(this);
}