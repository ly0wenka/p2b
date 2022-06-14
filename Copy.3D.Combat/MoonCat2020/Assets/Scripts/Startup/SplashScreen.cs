using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Startup
{
    [RequireComponent(typeof(AudioSource))]
    public class SplashScreen : MonoBehaviour
    {
        public Texture2D splashScreenBackground;
        public Texture2D splashScreenText;

        private AudioSource _splashScreenAudio;
        public AudioClip splashScreenMusic;

        private float _splashScreenFadeValue;
        private float splashScreenFadeSpeed = .5f;

        private SplashScreenController _splashScreenController;

        void Awake()
        {
            _splashScreenFadeValue = 0;
        }

        void Start()
        {
            // Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked;

            _splashScreenAudio = GetComponent<AudioSource>();

            _splashScreenAudio.volume = 0;
            _splashScreenAudio.clip = splashScreenMusic;
            _splashScreenAudio.loop = true;
            _splashScreenAudio.Play();

            _splashScreenController = SplashScreenController.SplashScreenFadeIn;

            StartCoroutine(SplashScreenManager());
        }

        private IEnumerator SplashScreenManager()
        {
            while (true)
            {
                switch (_splashScreenController)
                {
                    case SplashScreenController.SplashScreenFadeIn:
                        SplashScreenFadeIn();
                        break;
                    case SplashScreenController.SplashScreenFadeOut:
                        SplashScreenFadeOut();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return null;
            }
        }

        private void SplashScreenFadeIn()
        {
            Debug.Log(nameof(SplashScreenFadeIn));

            IncreaseVolume();

            if (_splashScreenFadeValue > 1)
                _splashScreenFadeValue = 1;

            const double tolerance = 0.01;
            if (Math.Abs(_splashScreenFadeValue - 1) < tolerance)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    
        private void IncreaseVolume()
        {
            _splashScreenAudio.volume += splashScreenFadeSpeed * Time.deltaTime;
            _splashScreenFadeValue += splashScreenFadeSpeed * Time.deltaTime;
        }

        private void SplashScreenFadeOut()
        {
            Debug.Log(nameof(SplashScreenFadeOut));

            DecreaseVolume();
        
            if (_splashScreenFadeValue < 0)
                _splashScreenFadeValue = 0;

            if (_splashScreenFadeValue == 0)
                SceneManager.LoadScene("ControllerWarning");
        }

        private void DecreaseVolume()
        {
            _splashScreenAudio.volume -= splashScreenFadeSpeed * Time.deltaTime;
            _splashScreenFadeValue -= splashScreenFadeSpeed * Time.deltaTime;
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(0,0,
                    Screen.width, Screen.height),
                splashScreenBackground);

            GUI.color = new Color(1, 1, 1, _splashScreenFadeValue);
        
            GUI.DrawTexture(new Rect(0,0,
                    Screen.width, Screen.height),
                splashScreenText);
        }
    }
}
