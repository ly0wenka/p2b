using System.Linq;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class MoveInfo : ScriptableObject
{
    public float version;
    public AnimationClip animationClip;
    public SerializedAnimationMap animMap = new SerializedAnimationMap();
    public float animationSpeed = 1;
    public Fix64 _animationSpeed = 1;
    public WrapMode wrapMode;

    public GameObject characterPrefab;
    public string moveName;
    public string description;
    public int fps = 60;
    public bool ignoreGravity;
    public bool ignoreFriction;
    public bool cancelMoveWhenLanding;
    public bool forceMirrorLeft;
    public bool forceMirrorRight;
    public bool invertRotationLeft;
    public bool invertRotationRight;
    public bool autoCorrectRotation;
    public int frameWindowRotation;

    public bool gaugeToggle;
    public bool startDrainingGauge;
    public bool inhibitGainWhileDraining;
    public bool stopDrainingGauge;
    public float gaugeDPS;
    public Fix64 _gaugeDPS;
    public float totalDrain;
    public Fix64 _totalDrain;
    public float gaugeRequired;
    public Fix64 _gaugeRequired;
    public float gaugeUsage;
    public Fix64 _gaugeUsage;
    public float gaugeGainOnMiss;
    public Fix64 _gaugeGainOnMiss;
    public float gaugeGainOnHit;
    public Fix64 _gaugeGainOnHit;
    public float gaugeGainOnBlock;
    public Fix64 _gaugeGainOnBlock;
    public float opGaugeGainOnBlock;
    public Fix64 _opGaugeGainOnBlock;
    public float opGaugeGainOnParry;
    public Fix64 _opGaugeGainOnParry;
    public float opGaugeGainOnHit;
    public Fix64 _opGaugeGainOnHit;
    public MoveInfo DCMove;
    public CombatStance DCStance;

    public bool disableHeadLook = true;
    public bool fixedSpeed = true;
    public AnimSpeedKeyFrame[] animSpeedKeyFrame = new AnimSpeedKeyFrame[0];
    public int totalFrames = 15;

    public int startUpFrames = 0;
    public int activeFrames = 1;
    public int recoverFrames = 2;
    public bool applyRootMotion = false;
    public bool forceGrounded = false;
    public BodyPart rootMotionNode = BodyPart.none;
    public bool overrideBlendingIn = true;
    public bool overrideBlendingOut = false;
    public float blendingIn = 0;
    public Fix64 _blendingIn = 0;
    public float blendingOut = 0;
    public Fix64 _blendingOut = 0;

    public bool chargeMove;
    public float chargeTiming = .7f;
    public Fix64 _chargeTiming = .7;
    public bool allowInputLaniency;
    public bool allowNegativeEdge = true;
    public int leniencyBuffer = 3;
    public bool onReleaseExecution;
    public bool requireButtonPress = true;
    public bool onPressExecution = true;
    public ButtonPress[] buttonSequence = new ButtonPress[0];
    public ButtonPress[] buttonExecution = new ButtonPress[0];

    public MoveInputs defaultInputs = new MoveInputs();
    public MoveInputs altInputs = new MoveInputs();

    public MoveInfo[] previousMoves = new MoveInfo[0];
    public PlayerConditions opponentConditions = new PlayerConditions();
    public PlayerConditions selfConditions = new PlayerConditions();
    public MoveClassification moveClassification;

    public ButtonPress[][] simulatedInputs;

    #region trackable definitions
    public FrameLink[] frameLinks = new FrameLink[0];
    public MoveParticleEffect[] particleEffects = new MoveParticleEffect[0];
    public AppliedForce[] appliedForces = new AppliedForce[0];
    public SlowMoEffect[] slowMoEffects = new SlowMoEffect[0];
    public BodyPartVisibilityChange[] bodyPartVisibilityChanges = new BodyPartVisibilityChange[0];
    public OpponentOverride[] opponentOverride = new OpponentOverride[0];
    public SoundEffect[] soundEffects = new SoundEffect[0];
    public InGameAlert[] inGameAlert = new InGameAlert[0];
    public StanceChange[] stanceChanges = new StanceChange[0];
    public CameraMovement[] cameraMovements = new CameraMovement[0];
    public Hit[] hits = new Hit[0];
    public BlockArea blockableArea;
    public InvincibleBodyParts[] invincibleBodyParts = new InvincibleBodyParts[0];
    public ArmorOptions armorOptions;
    public Projectile[] projectiles = new Projectile[0];

    public bool cancelable { get; set; }
    public bool kill { get; set; }
    public int currentFrame { get; set; }
    public int overrideStartupFrame { get; set; }
    public Fix64 animationSpeedTemp { get; set; }
    public Fix64 currentTick { get; set; }
    public bool hitConfirmOnBlock { get; set; }
    public bool hitConfirmOnParry { get; set; }
    public bool hitConfirmOnStrike { get; set; }
    public bool hitAnimationOverride { get; set; }
    public StandUpOptions standUpOptions { get; set; }
    public CurrentFrameData currentFrameData { get; set; }
    #endregion

    public bool IsThrow(bool teachable) 
        => hits.Any(hit 
            => hit.hitConfirmType == HitConfirmType.Throw 
               && hit.teachable == teachable);

    public MoveInfo GetTechMove()
        => hits.FirstOrDefault(hit
            => hit.hitConfirmType == HitConfirmType.Throw 
               && hit.teachable)?.techMove;
}
