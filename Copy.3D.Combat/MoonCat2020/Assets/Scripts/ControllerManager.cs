using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ControllerManager : MonoBehaviour
{
    public Texture2D controllerNotDetectedTexture;

    public bool pS4Controller;

    public bool xBOXController;

    public bool controllerDetected;

    public static bool startUpFinished;

    private AudioSource _cmAudio;
    public AudioClip _controllerDetectedAudioClip;
    
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
        if (startUpFinished)
        {
            Time.timeScale = 0;
        }
    }

    void LateUpdate()
    {
        if (startUpFinished)
        {
            _cmAudio = GetComponent<AudioSource>();
        }
        
        string[] joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (joystickNames[i].Length == 19)
            {
                pS4Controller = true;
                
                // if (controllerDetected){
                //     return;}
                if (startUpFinished)
                {
                    _cmAudio.PlayOneShot(_controllerDetectedAudioClip);
                }

                Time.timeScale = 1;
                
                controllerDetected = true;
            }
            
            if (joystickNames[i].Length == 33)
            {
                xBOXController = true;
                
                // if (controllerDetected){
                //     return;}
                if (startUpFinished)
                {
                    _cmAudio.PlayOneShot(_controllerDetectedAudioClip);
                }

                Time.timeScale = 1;
                
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
