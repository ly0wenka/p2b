using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public Texture2D controllerNotDetectedTexture;

    public bool pS4Controller;

    public bool xBOXController;

    public bool controllerDetected;

    public static bool startUpFinished;
    
    private void Awake()
    {
        pS4Controller = false;
        xBOXController = false;
        controllerDetected = false;
        startUpFinished = false;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // if (controllerDetected)
        //     return;
    }

    void LateUpdate()
    {
        string[] joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (joystickNames[i].Length == 19)
            {
                pS4Controller = true;
                controllerDetected = true;
            }
            
            if (joystickNames[i].Length == 33)
            {
                xBOXController = true;
                controllerDetected = true;
            }
            
            if (joystickNames[i].Length != 0)
                return;

            if (string.IsNullOrEmpty(joystickNames[i]))
                controllerDetected = false;
        }
    }

    private void OnGUI()
    {
        if (!startUpFinished)
            return;
        
        if (controllerDetected)
            return;
        
        if(!controllerDetected)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), controllerNotDetectedTexture);
    }

}
