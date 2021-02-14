using FPLibrary;
using UnityEngine;

[System.Serializable]
public class CameraOptions {
    public Vector3 initialDistance;
    public Vector3 initialRotation;
    public float initialFieldOfView;
    public bool followJumpingCharacter;
    public float movementSpeed = 15;
    public float minZoom = 38;
    public float maxZoom = 54;
    public float maxDistance = 22;
    public Fix64 _maxDistance = 22;
    public bool enableLookAt;
    public float rotationSpeed = 20;
    public float heightOffSet = 4;
    public Vector3 rotationOffSet = new Vector3(0, 4, 0);
    public MotionSensor motionSensor;
    public float motionSensibility = 1;
}