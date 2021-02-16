using FPLibrary;
using UnityEngine;

public struct TransformState
{
    public FPVector fpPosition;
    public FPQuaternion fpRotation;
    public Vector3 position;
    public Vector3 localPosition;
    public Quaternion rotation;
    public Quaternion localRotation;
    public Vector3 localScale;
    public bool active;
}