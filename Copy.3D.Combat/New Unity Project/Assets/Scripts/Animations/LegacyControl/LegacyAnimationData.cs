using FPLibrary;
using UnityEngine;

[System.Serializable]
public class LegacyAnimationData
{
    public AnimationClip clip;
    public string clipName;
    public WrapMode wrapMode;
    public Fix64 length = 0;
    public Fix64 originalSpeed = 1;
    public Fix64 normalizedSpeed = 1;

    #region trackable definitions
    public Fix64 normalizedTime = 1;
    public Fix64 secondsPlayed = 0;
    public Fix64 realSecondsPlayed = 0;
    public int timesPlayed = 0;
    public Fix64 speed = 1;
    #endregion
    [HideInInspector] public AnimationState animState;
}