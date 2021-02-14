using FPLibrary;
using UnityEngine;

[System.Serializable]
public class BlockOptions {
    public BlockType blockType;
    public bool allowAirBlock;
    public bool ignoreAppliedForceBlock;
    public bool allowMoveCancel;

    public GameObject blockPrefab;
    public float blockKillTime;
    public AudioClip blockSound;
    public bool overrideBlockHitEffects;
    public HitTypeOptions blockHitEffects;



    public ParryType parryType;
    public float parryTiming;
    public Fix64 _parryTiming;
    public ParryStunType parryStunType;
    public int parryStunFrames = 10;
	
    public GameObject parryPrefab;
    public float parryKillTime;
    public AudioClip parrySound;
    public bool overrideParryHitEffects;
    public HitTypeOptions parryHitEffects;


    public Color parryColor;
    public bool allowAirParry;
    public bool highlightWhenParry;
    public bool resetButtonSequence;
    public bool easyParry;
    public bool ignoreAppliedForceParry;
    public Sizes blockPushForce; // TODO
    public ButtonPress[] pushBlockButtons; // TODO
}