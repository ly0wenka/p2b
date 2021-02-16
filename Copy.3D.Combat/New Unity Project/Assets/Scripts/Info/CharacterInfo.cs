using System.Collections;
using System.Collections.Generic;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class CharacterInfo : ScriptableObject
{
    public float version;
    public Texture2D profilePictureSmall;
    public Texture2D profilePictureBig;
    public string characterName;
    public Gender gender;
    public string characterDescription;
    public AnimationClip selectionAnimation;
    public AudioClip selectionSound;
    public AudioClip deathSound;
    public float height;
    public int age;
    public string bloodType;
    public int lifePoints = 1000;
    public int maxGaugePoints;
    public StorageMode characterPrefabStorage = StorageMode.Legacy;
    public GameObject characterPrefab;
    public string prefabResourcePath;
    public AltCostume[] alternativeCostumes;
    public FPVector initialPosition;
    public FPQuaternion initialRotation;

    public PhysicsData physics;
    public HeadLook headLook;
    public CustomControls customControls;

    public float executingTiming = .3f;
    public Fix64 _executionTiming = .3;
    public int possibleAirMoves = 1;
    public float blendingTime = .1f;
    public Fix64 _blendingTime = .1;

    public AnimationType animationType;
    public Avatar avatar;
    public bool applyRootMotion;
    public AnimationFlow animationFlow;
    public bool useAnimationMaps;

    public string[] stanceResourcePath = new string[0];
    public MoveSetData[] moves = new MoveSetData[0];
    public AIInstructionsSet[] aiInstructionsSet = new AIInstructionsSet[0];

    public int playerNum { get; set; }
    public bool isAlt { get; set; }
    public int selectedCostume { get; set; }
    public MoveSetData[] loadedMoves { get; set; }

    #region trackable definitions
    public CombatStance currentCombatStance { get; set; }
    public Fix64 currentLifePoints { get; set; }
    public Fix64 currentGaugePoints { get; set; }
    #endregion
}
