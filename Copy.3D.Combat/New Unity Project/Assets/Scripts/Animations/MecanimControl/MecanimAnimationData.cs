using FPLibrary;
using UnityEngine;

[System.Serializable]
public class MecanimAnimationData
{
    public AnimationClip clip;
    public string clipName;
    public WrapMode wrapMode;
    public bool applyRootMotion;
    public Fix64 length = 1;
    public Fix64 originalSpeed = 1;

    public Fix64 transitionDuration = -1;
    public Fix64 normalizedSpeed = 1;
    public string stateName;

    #region trackable definitions
    public Fix64 normalizedTime = 1;
    public Fix64 secondsPlayed = 0;
    public int timesPlayed = 0;
    public Fix64 speed = 1;
    #endregion
}