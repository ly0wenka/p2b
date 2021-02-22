using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public Texture2D splashScreenBackground;
    public Texture2D splashScreenText;

    private AudioSource splashScreenAudio;
    public AudioClip splashScreenMusic;

    private float splashScreenFadeValue;
    private float splashScreenFadeSpeed = 0.15f;

    private SplashScreenController splashScreenController;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
