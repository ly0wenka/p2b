using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SplashScreen : MonoBehaviour
{
    public Texture2D splashScreenBackground;
    public Texture2D splashScreenText;

    private AudioSource splashScreenAudio;
    public AudioClip splashScreenMusic;

    private float splashScreenFadeValue;
    private float splashScreenFadeSpeed = 0.15f;

    private SplashScreenController splashScreenController;

    void Awake()
    {
        splashScreenFadeValue = 0;
    }

    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        splashScreenAudio = GetComponent<AudioSource>();

        splashScreenAudio.volume = 0;
        splashScreenAudio.clip = splashScreenMusic;
        splashScreenAudio.loop = true;
        splashScreenAudio.Play();

        splashScreenController = SplashScreenController.SplashScreenFadeIn;

        StartCoroutine(SplashScreenManager());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SplashScreenManager()
    {
        while (true)
        {
            switch (splashScreenController)
            {
                case SplashScreenController.SplashScreenFadeIn:
                    SplashScreenFadeIn();
                    break;
                case SplashScreenController.SplashScreenFadeOut:
                    SplashScreenFadeOut();
                    break;
            }

            yield return null;
        }
    }

    private void SplashScreenFadeIn()
    {
        Debug.Log("SplashScreenFadeIn");

        IncreaseVolume();

        if (splashScreenFadeValue > 1)
            splashScreenFadeValue = 1;
    }
    
    private void IncreaseVolume()
    {
        splashScreenAudio.volume += splashScreenFadeSpeed * Time.deltaTime;
        splashScreenFadeValue += splashScreenFadeSpeed * Time.deltaTime;
    }

    private void SplashScreenFadeOut()
    {
        Debug.Log("SplashScreenFadeOut");

        DecreaseVolume();
        
        if (splashScreenFadeValue < 0)
            splashScreenFadeValue = 0;

        if (splashScreenFadeValue == 0)
            SceneManager.LoadScene("ControllerWarning");
    }

    private void DecreaseVolume()
    {
        splashScreenAudio.volume -= splashScreenFadeSpeed * Time.deltaTime;
        splashScreenFadeValue -= splashScreenFadeSpeed * Time.deltaTime;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0,0,
                Screen.width, Screen.height),
            splashScreenBackground);

        GUI.color = new Color(1, 1, 1, splashScreenFadeValue);
        
        GUI.DrawTexture(new Rect(0,0,
            Screen.width, Screen.height),
            splashScreenText);
    }
}
