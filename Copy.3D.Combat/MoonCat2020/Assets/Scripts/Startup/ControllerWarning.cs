using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ControllerWarning : ControllerManager
{
    public Texture2D controllerWarningBackground;
    public Texture2D controllerWarningText;
    public Texture2D controllerDetectedText;

    private float controllerWarningFadeValue;
    private float controllerWarningFadeSpeed = .33f;
    private bool controllerConditionsMet;

    void Start()
    {
        controllerWarningFadeValue = 1;
        controllerConditionsMet = false;
    }

    void Update()
    {
        if (controllerDetected)
            StartCoroutine(WaitToLoadMainMenu());
        
        if(!controllerConditionsMet)
            return;
        else
        {
            controllerWarningFadeValue -= controllerWarningFadeSpeed * Time.deltaTime;

            if (controllerWarningFadeValue < 0)
                controllerWarningFadeValue = 0;

            if (controllerWarningFadeValue == 0)
            {
                startUpFinished = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private IEnumerator WaitToLoadMainMenu()
    {
        yield return new WaitForSeconds(2);

        controllerConditionsMet = true;
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), controllerWarningBackground);
        GUI.color = new Color(1,1,1, controllerWarningFadeValue);
        
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), controllerWarningText);
        
        if (controllerDetected)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), controllerDetectedText);
    }
}
