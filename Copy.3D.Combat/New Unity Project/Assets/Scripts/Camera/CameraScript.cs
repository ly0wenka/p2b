using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region trackable definitions

    public bool cinematicFreeze;
    public Vector3 currentLookAtPosition;
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

    #endregion


    public GameObject playerLight;
    public Transform player1;
    public Transform player2;


    void Start()
    {
        playerLight = GameObject.Find("Player Light");
        player1 = GameObject.Find("Player1").transform;
        player2 = GameObject.Find("Player2").transform;

        ResetCam();
        //standardZoom = MainScript.config.cameraOptions.initialDistance.z;
        standardDistance = Vector3.Distance(player1.position, player2.position);
        movementSpeed = MainScript.config.cameraOptions.movementSpeed;
        rotationSpeed = MainScript.config.cameraOptions.rotationSpeed;
        MainScript.freeCamera = false;
    }

    public void ResetCam()
    {
        Camera.main.transform.localPosition = MainScript.config.cameraOptions.initialDistance;
        Camera.main.transform.position = MainScript.config.cameraOptions.initialDistance;
        Camera.main.transform.localRotation = Quaternion.Euler(MainScript.config.cameraOptions.initialRotation);
        Camera.main.fieldOfView = MainScript.config.cameraOptions.initialFieldOfView;
        //standardGroundHeight = Camera.main.transform.position.y;
    }

    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float speed)
    {
        Vector3 P = speed * (float) MainScript.fixedDeltaTime * Vector3.Normalize(B - A) + A;
        return P;
    }

    public void DoFixedUpdate()
    {
        if (killCamMove) return;
        if (MainScript.freeCamera)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFieldOfView,
                (float) MainScript.fixedDeltaTime * freeCameraSpeed * 1.8f);
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, targetPosition,
                (float) MainScript.fixedDeltaTime * freeCameraSpeed * 1.8f);
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, targetRotation,
                (float) MainScript.fixedDeltaTime * freeCameraSpeed * 1.8f);
        }
        else
        {
            Vector3 newPosition = ((player1.position + player2.position) / 2) +
                                  MainScript.config.cameraOptions.initialDistance;
            if (MainScript.config.cameraOptions.followJumpingCharacter)
                newPosition.y += Mathf.Abs(player1.position.y - player2.position.y) / 2;

            newPosition.x = Mathf.Clamp(newPosition.x,
                (float) MainScript.config.selectedStage._leftBoundary + 8,
                (float) MainScript.config.selectedStage._rightBoundary - 8);

            newPosition.z = MainScript.config.cameraOptions.initialDistance.z -
                Vector3.Distance(player1.position, player2.position) + standardDistance;
            newPosition.z = Mathf.Clamp(newPosition.z, -MainScript.config.cameraOptions.maxZoom,
                -MainScript.config.cameraOptions.minZoom);

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,
                MainScript.config.cameraOptions.initialFieldOfView, (float) MainScript.fixedDeltaTime * movementSpeed);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition,
                (float) MainScript.fixedDeltaTime * movementSpeed);
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation,
                Quaternion.Euler(MainScript.config.cameraOptions.initialRotation),
                (float) MainScript.fixedDeltaTime * MainScript.config.cameraOptions.movementSpeed);

            if (Camera.main.transform.localRotation ==
                Quaternion.Euler(MainScript.config.cameraOptions.initialRotation))
                MainScript.normalizedCam = true;

            if (playerLight != null) playerLight.GetComponent<Light>().enabled = false;

            if (MainScript.config.cameraOptions.enableLookAt)
            {
                //Vector3 lookAtPosition = ((player1.position + player2.position)/2);
                //lookAtPosition.y += MainScript.config.cameraOptions.heightOffSet;

                Vector3 newLookAtPosition = ((player1.position + player2.position) / 2) +
                                            MainScript.config.cameraOptions.rotationOffSet;

                if (MainScript.config.cameraOptions.motionSensor != MotionSensor.None)
                {
                    Vector3 acceleration = Input.acceleration;
                    if (MainScript.config.cameraOptions.motionSensor == MotionSensor.Gyroscope &&
                        SystemInfo.supportsGyroscope) acceleration = Input.gyro.gravity;

#if UNITY_STANDALONE || UNITY_EDITOR
                    if (Input.mousePresent)
                    {
                        Vector3 mouseXY = new Vector3(Input.mousePosition.x - Screen.width / 2,
                            Input.mousePosition.y - Screen.height / 2, 0);
                        acceleration = mouseXY / 1000;
                    }
#endif
                    acceleration *= MainScript.config.cameraOptions.motionSensibility;
                    newLookAtPosition -= acceleration;

                    newPosition.y += acceleration.y;
                    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition,
                        (float) MainScript.fixedDeltaTime * movementSpeed);
                }

                currentLookAtPosition = Vector3.Lerp(currentLookAtPosition,
                    newLookAtPosition,
                    (float) MainScript.fixedDeltaTime * rotationSpeed);


                Camera.main.transform.LookAt(currentLookAtPosition, Vector3.up);
            }
        }
    }

    public void MoveCameraToLocation(Vector3 targetPos, Vector3 targetRot, float targetFOV, float speed, string owner)
    {
        targetFieldOfView = targetFOV;
        targetPosition = targetPos;
        targetRotation = Quaternion.Euler(targetRot);
        freeCameraSpeed = speed;
        MainScript.freeCamera = true;
        MainScript.normalizedCam = false;
        lastOwner = owner;
        if (playerLight != null) playerLight.GetComponent<Light>().enabled = true;
    }

    public void DisableCam()
    {
        Camera.main.enabled = false;
    }

    public void ReleaseCam()
    {
        Camera.main.enabled = true;
        cinematicFreeze = false;
        MainScript.freeCamera = false;
        lastOwner = "";
    }

    public void OverrideSpeed(float newMovement, float newRotation)
    {
        movementSpeed = newMovement;
        rotationSpeed = newRotation;
    }

    public void RestoreSpeed()
    {
        movementSpeed = MainScript.config.cameraOptions.movementSpeed;
        rotationSpeed = MainScript.config.cameraOptions.rotationSpeed;
    }

    public void SetCameraOwner(string owner)
    {
        lastOwner = owner;
    }

    public string GetCameraOwner()
    {
        return lastOwner;
    }

    public Vector3 GetRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }

    void OnDrawGizmos()
    {
        Vector3 cameraLeftBounds =
            Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        Vector3 cameraRightBounds =
            Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));

        cameraLeftBounds.x = Camera.main.transform.position.x -
                             ((float) MainScript.config.cameraOptions._maxDistance / 2);
        cameraRightBounds.x = Camera.main.transform.position.x +
                              ((float) MainScript.config.cameraOptions._maxDistance / 2);

        Gizmos.DrawLine(cameraLeftBounds, cameraLeftBounds + new Vector3(0, 15, 0));
        Gizmos.DrawLine(cameraRightBounds, cameraRightBounds + new Vector3(0, 15, 0));
        //Gizmos.DrawWireSphere(cameraRightBounds, 1);
    }
}