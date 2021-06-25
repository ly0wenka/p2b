using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimatorHelper
{
    public static bool IsPlaying(this Animator animator, string stateName){
        return animator.IsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    
    public static AnimatorStateInfo State(this Animator animator, string stateName){
        return animator.GetCurrentAnimatorStateInfo(0);
    }
    
    public static float StateSpeed(this Animator animator){
        return animator.GetCurrentAnimatorStateInfo(0).speed;
    }
    
    public static Animator GetAnimator(this Animator animator, string stateName)
    {
        return animator;
    }

    public static void CrossFade(this Animator animator, string stateName)
    {
        if (animator.IsPlaying(stateName))
        {
            return;
        }
        animator.Play(stateName);
    }
    
    private static bool IsPlaying(this Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private static float GetAnimationsClipsLength(this Animator animator, string stateName)
    {
        return animator.runtimeAnimatorController
            .animationClips
            .First(a => a.name == stateName)
            .length;
    }
}
