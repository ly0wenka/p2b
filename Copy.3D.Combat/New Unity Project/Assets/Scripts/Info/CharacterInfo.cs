using System.Collections;
using System.Collections.Generic;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class CharacterInfo : ScriptableObject
{
    #region public fields

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

    #endregion
}
