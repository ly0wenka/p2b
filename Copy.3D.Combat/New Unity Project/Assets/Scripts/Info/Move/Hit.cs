using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class Hit : ICloneable
{
    public int activeFramesBegin;
    public int activeFramesEnds;
    public HitConfirmType hitConfirmType;
    public MoveInfo throwMove;
    public MoveInfo techMove;
    public bool teachable = true;
    public bool resetHitAnimations = true;
    public bool forceStand = false;
    public bool armorBreaker;
    public bool continuousHit;
    public bool unblockable;
    public Sizes spaceBetweenHits;
    public bool groundHit = true;
    public bool crouchingHit = true;
    public bool airHit = true;
    public bool stunHit = true;
    public PlayerConditions opponentConditions = new PlayerConditions();

    public bool downHit;
    public bool resetPreviousHitStun;
    public bool resetCrumples;
    public bool customStunValues;

    public bool overrideHitEffects;
    public HitTypeOptions hitEffects;
    public bool overrideHitEffectsBlock;
    public HitTypeOptions hitEffectsBlock;
    public bool overrideEffectSpawnPoint;
    public HitEffectSpawnPoint spawnPoint = HitEffectSpawnPoint.StrokeHitBox;
    public bool overrideHitAnimationBlend;
    public float newHitBlendingIn;
    public Fix64 _newHitBlendingIn;
    public bool overrideJuggleWeight;
    public float newJuggleWeight;
    public Fix64 _newJuggleWeight;
    public bool overrideAirRecoveryType;
    public AirRecoveryType newAirRecoveryType = AirRecoveryType.AllowMoves;
    public bool instantAirRecovery;

    public bool overrideHitAnimation;

    //public bool overrideHitAcceleration = true; // deprecated
    public BasicMoveReference newHitAnimation = BasicMoveReference.HitKnockBack;

    public HitStrengh hitStrength;
    public HitStunType hitStunType = HitStunType.Frames;
    public float hitStunOnHit;
    public Fix64 _hitStunOnHit;
    public float hitStunOnBlock;
    public Fix64 _hitStunOnBlock;
    public int frameAdvantageOnHit;
    public int frameAdvantageOnBlock;
    public bool damageScaling;
    public DamageType damageType;
    public float damageOnHit;
    public Fix64 _damageOnHit;
    public float damageOnBlock;
    public Fix64 _damageOnBlock;
    public bool doesntKill;
    public HitType hitType;

    public bool resetPreviousHorizontalPush;
    public bool resetPreviousVerticalPush;
    public bool applyDifferentAirForce;
    public bool applyDifferentBlockForce;
    public Vector2 pushForce;
    public FPVector _pushForce;
    public Vector2 pushForceAir;
    public FPVector _pushForceAir;
    public FPVector _pushForceBlock;
    public bool resetPreviousHorizontal;
    public bool resetPreviousVertical;
    public Vector2 appliedForce;
    public FPVector _appliedForce;

    public bool cornerPush = true;

    public bool groundBounce = true;
    public bool overrideForcesOnGroundBounce = false;
    public bool resetGroundBounceHorizontalPush;
    public bool resetGroundBounceVerticalPush;
    public Vector2 groundBouncePushForce;
    public FPVector _groundBouncePushForce;

    public bool wallBounce = false;
    public bool knockOutOnWallBounce = false;
    public bool overrideForcesOnWallBounce = false;
    public bool resetWallBounceHorizontalPush;
    public bool resetWallBounceVerticalPush;
    public Vector2 wallBouncePushForce;
    public FPVector _wallBouncePushForce;
    public bool bounceOnCameraEdge = false;
    public bool overrideCameraSpeed = false;
    public float newMovementSpeed;
    public Fix64 _newMovementSpeed;
    public float newRotationSpeed;
    public Fix64 _newRotationSpeed;
    public float cameraSpeedDuration;
    public Fix64 _cameraSpeedDuration;

    public PullIn pullEnemyIn;
    public PullIn pullSelfIn;

    [HideInInspector] public bool damageOptionsToggle;
    [HideInInspector] public bool hitStunOptionsToggle;
    [HideInInspector] public bool forceOptionsToggle;
    [HideInInspector] public bool opponentForceToggle;
    [HideInInspector] public bool selfForceToggle;
    [HideInInspector] public bool stageReactionsToggle;
    [HideInInspector] public bool overrideEventsToggle;
    [HideInInspector] public bool hitConditionsToggle;
    [HideInInspector] public bool pullInToggle;
    [HideInInspector] public bool hurtBoxesToggle;
    [HideInInspector] public bool wallBounceToggle;
    [HideInInspector] public bool groundBounceToggle;

    #region trackable definitions

    public HurtBox[] hurtBoxes = new HurtBox[0];
    public bool disabled { get; set; }

    #endregion

    public object Clone()
    {
        return CloneObject.Clone(this);
    }
}