using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        splashScreenAudio = GetComponent<AudioSource>();

        splashScreenAudio.volume = 0;
        splashScreenAudio.clip = splashScreenMusic;
        splashScreenAudio.loop = true;
        splashScreenAudio.Play();

        splashScreenController = SplashScreenController.SplashScreenFadeIn;

        StartCoroutine("SplashScreenManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SplashScreenFadeIn()
    {
        Debug.Log("SplashScreenFadeIn");
    }

    private void SplasScreenFadeOut()
    {
        Debug.Log("SplashScreenFadeOut");
    }
}
