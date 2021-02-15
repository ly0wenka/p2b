﻿
using FPLibrary;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class LegacyControl : MonoBehaviour
{

    public LegacyAnimationData[] animations = new LegacyAnimationData[0];
    public bool debugMode = false;
    public bool overrideAnimatorUpdate = false;
    public Animation animator;

    #region trackable definitions
    [HideInInspector] public LegacyAnimationData currentAnimationData;
    [HideInInspector] public bool currentMirror;
    [HideInInspector] public Fix64 globalSpeed = 1;
    [HideInInspector] public Vector3 lastPosition;
    public Vector3 deltaDisplacement;
    #endregion

    void Awake()
    {
        animator = gameObject.GetComponent<Animation>();
        lastPosition = transform.position;
    }

    void Start()
    {
        if (animations[0] == null) Debug.LogWarning("No animation found!");
        currentAnimationData = animations[0];

        if (overrideAnimatorUpdate)
        {
            foreach (AnimationState animState in animator)
            {
                animState.speed = 0;
            }
        }
    }

    public void DoFixedUpdate()
    {
        if (animator == null || currentAnimationData == null || !animator.isPlaying || !overrideAnimatorUpdate) return;

        currentAnimationData.secondsPlayed += (MainScript.fixedDeltaTime * globalSpeed);
        currentAnimationData.realSecondsPlayed += MainScript.fixedDeltaTime;
        currentAnimationData.animState.time = (float)currentAnimationData.secondsPlayed;
        if (currentAnimationData.secondsPlayed >= currentAnimationData.length && currentAnimationData.clip.wrapMode == WrapMode.Loop) SetCurrentClipPosition(0);
        animator.Sample();
    }

    void OnGUI()
    {
        //Toggle debug mode to see the live data in action
        if (debugMode)
        {
            GUI.Box(new Rect(Screen.width - 340, 40, 340, 300), "Animation Data");
            GUI.BeginGroup(new Rect(Screen.width - 330, 60, 400, 300));
            {
                GUILayout.Label("Global Speed: " + globalSpeed);
                GUILayout.Label("Current Animation Data");
                GUILayout.Label("-Clip Name: " + currentAnimationData.clipName);
                GUILayout.Label("-Speed: " + currentAnimationData.speed);
                GUILayout.Label("-Normalized Speed: " + currentAnimationData.normalizedSpeed);
                GUILayout.Label("Animation State");
                GUILayout.Label("-Time: " + currentAnimationData.animState.time);
                GUILayout.Label("-Normalized Time: " + currentAnimationData.animState.normalizedTime);
                GUILayout.Label("-Lengh: " + currentAnimationData.animState.length);
                GUILayout.Label("-Speed: " + currentAnimationData.animState.speed);
            }
            GUI.EndGroup();
        }
    }



    // LEGACY CONTROL METHODS
    public void RemoveClip(string name)
    {
        List<LegacyAnimationData> animationDataList = new List<LegacyAnimationData>(animations);
        animationDataList.Remove(GetAnimationData(name));
        animations = animationDataList.ToArray();
    }

    public void RemoveClip(AnimationClip clip)
    {
        List<LegacyAnimationData> animationDataList = new List<LegacyAnimationData>(animations);
        animationDataList.Remove(GetAnimationData(clip));
        animations = animationDataList.ToArray();
    }

    public void RemoveAllClips()
    {
        animations = new LegacyAnimationData[0];
    }

    public void AddClip(AnimationClip clip, string newName)
    {
        AddClip(clip, newName, 1, animator.wrapMode);
    }

    public void AddClip(AnimationClip clip, string newName, Fix64 speed, WrapMode wrapMode)
    {
        AddClip(clip, newName, speed, wrapMode, clip.length);
    }

    public void AddClip(AnimationClip clip, string newName, Fix64 speed, WrapMode wrapMode, Fix64 length)
    {
        if (GetAnimationData(newName) != null) Debug.LogWarning("An animation with the name '" + newName + "' already exists.");
        LegacyAnimationData animData = new LegacyAnimationData();
        animData.clip = (AnimationClip)Instantiate(clip);
        if (wrapMode == WrapMode.Default) wrapMode = animator.wrapMode;
        animData.clip.wrapMode = wrapMode;
        animData.clip.name = newName;
        animData.clipName = newName;
        animData.speed = speed;
        animData.originalSpeed = speed;
        animData.length = length;
        animData.wrapMode = wrapMode;

        List<LegacyAnimationData> animationDataList = new List<LegacyAnimationData>(animations);
        animationDataList.Add(animData);
        animations = animationDataList.ToArray();

        animator.AddClip(clip, newName);
        animator[newName].speed = (float)speed;
        animator[newName].wrapMode = wrapMode;


        foreach (AnimationState animState in animator)
        {
            if (animState.name == newName) animData.animState = animState;
        }
    }

    public LegacyAnimationData GetAnimationData(string clipName)
    {
        foreach (LegacyAnimationData animData in animations)
        {
            if (animData.clipName == clipName)
            {
                return animData;
            }
        }
        return null;
    }

    public LegacyAnimationData GetAnimationData(AnimationClip clip)
    {
        foreach (LegacyAnimationData animData in animations)
        {
            if (animData.clip == clip)
            {
                return animData;
            }
        }
        return null;
    }

    public bool IsPlaying(string clipName)
    {
        if (currentAnimationData == GetAnimationData(clipName) && currentAnimationData.wrapMode == WrapMode.ClampForever) return true;
        return (animator.IsPlaying(clipName));
    }

    public bool IsPlaying(LegacyAnimationData animData)
    {
        return (currentAnimationData == animData);
    }

    public void Play(string animationName, Fix64 blendingTime, Fix64 normalizedTime)
    {
        Play(GetAnimationData(animationName), blendingTime, normalizedTime);
    }

    public void Play()
    {
        if (animations[0] == null)
        {
            Debug.LogError("No animation found.");
            return;
        }
        Play(animations[0], 0, 0);
    }

    public void Play(LegacyAnimationData animData, Fix64 blendingTime, Fix64 normalizedTime)
    {
        if (animData == null) return;

        if (currentAnimationData != null)
        {
            currentAnimationData.speed = currentAnimationData.originalSpeed;
            currentAnimationData.normalizedSpeed = 1;
        }

        currentAnimationData = animData;

        if (blendingTime == 0 ||
            ((MainScript.isConnected || MainScript.config.debugOptions.emulateNetwork) && MainScript.config.networkOptions.disableBlending))
        {
            animator.Play(currentAnimationData.clipName);
        }
        else
        {
            animator.CrossFade(currentAnimationData.clipName, (float)blendingTime);
        }

        SetSpeed(currentAnimationData.speed);
        deltaDisplacement = new Vector3();

        SetCurrentClipPosition(normalizedTime);
    }

    public void SetCurrentClipPosition(Fix64 normalizedTime)
    {
        SetCurrentClipPosition(normalizedTime, false);
    }

    public void SetCurrentClipPosition(Fix64 normalizedTime, bool pause)
    {
        normalizedTime = FPMath.Clamp(normalizedTime, 0, 1);
        currentAnimationData.secondsPlayed = normalizedTime * currentAnimationData.length;
        currentAnimationData.normalizedTime = normalizedTime;
        currentAnimationData.animState.normalizedTime = (float)normalizedTime;
        animator.Sample();

        if (pause) Pause();
    }

    public Fix64 GetCurrentClipPosition()
    {
        return currentAnimationData.animState.normalizedTime;
    }

    public Fix64 GetCurrentClipTime(bool realSeconds = false)
    {
        if (realSeconds) return currentAnimationData.realSecondsPlayed;
        return currentAnimationData.secondsPlayed;
    }

    public int GetCurrentClipFrame()
    {
        return (int)FPLibrary.FPMath.Round(currentAnimationData.animState.time * MainScript.config.fps);
    }

    public string GetCurrentClipName()
    {
        if (currentAnimationData == null) return null;
        return currentAnimationData.clipName;
    }

    public Vector3 GetDeltaDisplacement()
    {
        deltaDisplacement += GetDeltaPosition();
        return deltaDisplacement;
    }

    public Vector3 GetDeltaPosition()
    {
        Vector3 deltaPosition = transform.position - lastPosition;
        lastPosition = transform.position;
        return deltaPosition;
    }

    public void Stop()
    {
        animator.Stop();
    }

    public void Stop(string animName)
    {
        animator.Stop(animName);
    }

    public void Pause()
    {
        globalSpeed = 0;
    }

    public void SetSpeed(AnimationClip clip, Fix64 speed)
    {
        SetSpeed(GetAnimationData(clip), speed);
    }

    public void SetSpeed(string clipName, Fix64 speed)
    {
        SetSpeed(GetAnimationData(clipName), speed);
    }

    public void SetSpeed(LegacyAnimationData animData, Fix64 speed)
    {
        if (animData != null)
        {
            animData.speed = speed;
            animData.normalizedSpeed = speed / animData.originalSpeed;
            if (IsPlaying(animData)) SetSpeed(speed);
        }
    }

    public void SetSpeed(Fix64 speed)
    {
        globalSpeed = speed;

        if (!overrideAnimatorUpdate)
        {
            foreach (AnimationState animState in animator)
            {
                animState.speed = (float)speed;
            }
        }
    }

    public void SetNormalizedSpeed(AnimationClip clip, Fix64 normalizedSpeed)
    {
        SetNormalizedSpeed(GetAnimationData(clip), normalizedSpeed);
    }

    public void SetNormalizedSpeed(string clipName, Fix64 normalizedSpeed)
    {
        SetNormalizedSpeed(GetAnimationData(clipName), normalizedSpeed);
    }

    public void SetNormalizedSpeed(LegacyAnimationData animData, Fix64 normalizedSpeed)
    {
        animData.normalizedSpeed = normalizedSpeed;
        animData.speed = animData.originalSpeed * animData.normalizedSpeed;
        if (IsPlaying(animData)) SetSpeed(animData.speed);
    }

    public Fix64 GetSpeed(AnimationClip clip) => GetSpeed(GetAnimationData(clip));

    public Fix64 GetSpeed(string clipName) => GetSpeed(GetAnimationData(clipName));

    public Fix64 GetSpeed(LegacyAnimationData animData) => animData.speed;

    public Fix64 GetSpeed()
    {
        return globalSpeed;
    }

    public Fix64 GetNormalizedSpeed(AnimationClip clip)
    {
        return GetNormalizedSpeed(GetAnimationData(clip));
    }

    public Fix64 GetNormalizedSpeed(string clipName)
    {
        return GetNormalizedSpeed(GetAnimationData(clipName));
    }

    public Fix64 GetNormalizedSpeed(LegacyAnimationData animData)
    {
        return animData.normalizedSpeed;
    }

    public void RestoreSpeed()
    {
        SetSpeed(currentAnimationData.speed);

        if (!overrideAnimatorUpdate)
        {
            foreach (AnimationState animState in animator)
            {
                animState.speed = (float)GetAnimationData(animState.name).speed;
            }
        }
    }
}