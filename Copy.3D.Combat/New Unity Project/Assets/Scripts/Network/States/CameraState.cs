using UnityEngine;

public struct CameraState
{
    // Transform
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 position;
    public Quaternion rotation;

    public bool cameraScript;
    public bool cinematicFreeze;
    public Vector3 currentLookAtPosition;
    public bool enabled;
    public float fieldOfView;
    public float freeCameraSpeed;
    public string lastOwner;
    public bool killCamMove;
    public float movementSpeed;
    public float rotationSpeed;
    public float standardDistance;
    public float standardGroundHeight;
    public Vector3 targetPosition;
    public Quaternion targetRotation;
    public float targetFieldOfView;

    // Camera Fade
    public bool cameraFade;
    public Color currentScreenOverlayColor;
}