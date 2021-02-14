using System;
using FPLibrary;
using UnityEngine;

[System.Serializable]
public class StageOptions : ICloneable
{
    public string stageName;
    public string stageResourcePath;
    public string musicResourcePath;
    public Texture2D screenshot;
    public GameObject prefab;
    public AudioClip music;
    public float groundFriction = 100;
    public Fix64 _groundFriction = 100;
    public float leftBoundary = -38;
    public Fix64 _leftBoundary = -38;
    public float rightBoundary = 38;
    public Fix64 _rightBoundary = 38;
    public Fix64 _groundHeight = 0;

    public object Clone() => CloneObject.Clone(this);
}