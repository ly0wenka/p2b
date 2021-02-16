    using System.Collections.Generic;
    using FPLibrary;
    using UnityEngine;

    public struct CharacterState {
		// ControlsScript
		public bool controlsScript;

        // Transforms
        public TransformState shellTransform;
        public TransformState characterTransform;

        // Global Properties
        public CombatStance combatStance;
        public Fix64 life;
        public Fix64 gauge;

        // Control
        public Fix64 afkTimer;
        public int airJuggleHits;
        public AirRecoveryType airRecoveryType;
        public bool applyRootMotion;
        public bool blockStunned;
        public Fix64 comboDamage;
        public Fix64 comboHitDamage;
        public int comboHits;
        public int consecutiveCrumple;
        public BasicMoveReference currentBasicMove;
        public Fix64 currentDrained;
        public Hit currentHit;
        public string currentHitAnimation;
        public PossibleStates currentState;
        public SubStates currentSubState;
        public MoveInfo DCMove;
        public CombatStance DCStance;
        public bool firstHit;
        public Fix64 gaugeDPS;
        public bool hitDetected;
        public Fix64 hitAnimationSpeed;
        public Fix64 hitStunDeceleration;
        public Fix64 horizontalForce;
        public bool inhibitGainWhileDraining;
        public bool isAirRecovering;
        public bool isBlocking;
        public bool isDead;
        public bool ignoreCollisionMass;
        public bool introPlayed;
        public bool lit;
        public int mirror;
        public Fix64 normalizedDistance;
        public Fix64 normalizedJumpArc;
        public bool outroPlayed;
        public bool potentialBlock;
        public Fix64 potentialParry;
        public bool roundMsgCasted;
        public int roundsWon;
        public bool shakeCamera;
        public bool shakeCharacter;
        public Fix64 shakeDensity;
        public Fix64 shakeCameraDensity;
        public StandUpOptions standUpOverride;
        public Fix64 standardYRotation;
        public Fix64 storedMoveTime;
        public Fix64 stunTime;
        public Fix64 totalDrain;

        // Sub Classes
        public PullInState activePullIn;
        public MoveState currentMove;
        public MoveState storedMove;

        // Core Scripts
        public PhysicsState physics;
        public MoveSetState moveSet;
        public HitBoxesState hitBoxes;
        public AnimatiorState animator;

        // Arrays
        //public List<ProjectileMoveScript> projectiles;
        public Dictionary<ButtonPress, Fix64> inputHeldDown;
		public List<ProjectileMoveScript> projectiles;

        // Nested Structs
        public struct PullInState {
            public PullIn pullIn;
            public FPVector position;
        }

        public struct MoveState {
            public MoveInfo move;
            //public bool cancelable;
            public bool kill;
            public int armorHits;
            public int currentFrame;
            public int overrideStartupFrame;
            public Fix64 animationSpeedTemp;
            public Fix64 currentTick;
            public bool hitConfirmOnBlock;
            public bool hitConfirmOnParry;
            public bool hitConfirmOnStrike;
            public bool hitAnimationOverride;
            public StandUpOptions standUpOptions;
            public CurrentFrameData currentFrameData;
            public bool[] hitStates;
            public bool[] frameLinkStates;
            public bool[] castedBodyPartVisibilityChange;
            public bool[] castedProjectile;
            public bool[] castedAppliedForce;
            public bool[] castedMoveParticleEffect;
            public bool[] castedSlowMoEffect;
            public bool[] castedSoundEffect;
            public bool[] castedInGameAlert;
            public bool[] castedStanceChange;
            public bool[] castedCameraMovement;
            public Fix64[] cameraTime;
            public bool[] cameraOver;
            public bool[] castedOpponentOverride;
        }

        public struct PhysicsState {
            public Fix64 airTime;
            public Fix64 appliedGravity;
            public int currentAirJumps;
            public bool freeze;
            public int groundBounceTimes;
            public Fix64 horizontalForce;
            public bool isGroundBouncing;
            public bool isLanding;
            public bool isTakingOff;
            public bool isWallBouncing;
            public Fix64 moveDirection;
            public bool overrideAirAnimation;
            public BasicMoveInfo overrideStunAnimation;
            public Fix64 verticalForce;
            public Fix64 verticalTotalForce;
            public int wallBounceTimes;

        }

        public struct MoveSetState {
            public int totalAirMoves;
            public bool animationPaused;
            public Fix64 overrideNextBlendingValue;
            public Fix64 lastTimePress;
            public AnimatiorState animator;

            public Dictionary<ButtonPress, Fix64> chargeValues;
            public List<ButtonSequenceRecord> lastButtonPresses;
        }

        public struct HitBoxesState {
            public bool isHit;
            public HitConfirmType hitConfirmType;
            public Fix64 collisionBoxSize;
            public bool currentMirror;
            public bool bakeSpeed;
            public AnimationMap[] animationMaps;

            public HitBoxState[] hitBoxes;
            public HurtBoxState[] activeHurtBoxes;
            public BlockAreaState blockableArea;
        }

        public struct HitBoxState {
            public HitBox hitBox;
            public Rect rendererBounds;
            public int state;
            public bool hide;
            public bool visibility;
        }

        public struct HurtBoxState {
            public HurtBox hurtBox;
            public Rect rendererBounds;
            public bool isBlock;
            public FPVector position;
        }

        public struct BlockAreaState {
            public BlockArea blockArea;
            public FPVector position;
        }

        public struct AnimatiorState {
            public AnimationDataState currentAnimationData;
            public bool currentMirror;

            // Mecanim Control
            public Fix64 currentNormalizedTime;
            public string currentState;
            public Fix64 currentSpeed;
            public RuntimeAnimatorController overrideController;

            // MC3
            public int currentInput;
            public int transitionDuration;
            public int transitionTime;
            public Fix64[] weightList;
            public Fix64[] speedList;
            public Fix64[] timeList;

            // Legacy
            public Fix64 globalSpeed;
            public Vector3 lastPosition;
        }

        public struct AnimationDataState {
            public LegacyAnimationData legacyAnimationData;
            //public MC3AnimationData mecanimAnimationData;
            public MecanimAnimationData mecanimAnimationData;

            // Legacy
            public AnimationState animState;

            // Mecanim
            public Fix64 secondsPlayed;
            public int timesPlayed;
            public Fix64 speed;

            // Both
            public Fix64 normalizedTime;
        }
    }
