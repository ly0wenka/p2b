using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Startup
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenu : MonoBehaviour
    {
        public int selectedButton = 0;
        private readonly float _timeBetweenButtonPress = 0.1f;
        private float _timeDelay;

        private float _mainMenuVerticalInputTimer;
        private readonly float _mainMenuVerticalInputDelay = 1f;

        public Texture2D mainMenuBackground;
        public Texture2D mainMenuTitle;

        private AudioSource _mainMenuAudio;
        public AudioClip mainMenuMusic;
        public AudioClip mainMenuStartButtonAudio;
        public AudioClip mainMenuQuitButtonAudio;

        private float _mainMenuFadeValue;
        private float _mainMenuFadeSpeed = .5f;

        private float _mainMenuButtonWidth = 100f;
        private float _mainMenuButtonHeight = 25f;
        private float _mainMenuGUIOffset = 10f;

        private bool _startingOnePlayerGame;
        [SerializeField] private bool _startingTwoPlayerGame;
        private bool _quittingGame;
    
        private bool _ps4Controller;
        private bool _xBoxController;

        private readonly string[] _mainMenuButtons = new []
        {
            "_onePlayer",
            "_twoPlayer",
            "_quit"
        };

        private MainMenuController _mainMenuController;
        
        void Start()
        {
            _ps4Controller = false;
            _xBoxController = false;
            _mainMenuController = MainMenuController.MainMenuFadeIn;

            _mainMenuFadeValue = 0;
        
            _mainMenuAudio = GetComponent<AudioSource>();

            _mainMenuAudio.volume = 0;
            _mainMenuAudio.clip = mainMenuMusic;
            _mainMenuAudio.loop = true;
            _mainMenuAudio.Play();
        
            StartCoroutine(MainMenuManager());
        }
        
        void Update()
        {
            if (_mainMenuFadeValue > 1)
            {
                return;
            }

    
            var joyStickNames = Input.GetJoystickNames() ?? throw new ArgumentNullException("Input.GetJoystickNames()");
            foreach (var joyStickName in joyStickNames)
            {
                switch (joyStickName.Length)
                {
                    case 0:
                        return;
                    case 19:
                        _ps4Controller = true;
                        break;
                    case 33:
                        _xBoxController = true;
                        break;
                }
            }

            if (_mainMenuVerticalInputTimer > 0)
                _mainMenuVerticalInputTimer -= 1f * Time.deltaTime;

            if (IsVerticalAxisMoreThan0() && SelectedButton0())
            {
                return;
            }

            if (IsVerticalAxisMoreThan0() && selectedButton == 1)
            {
                if (_mainMenuVerticalInputTimer > 0)
                    return;
                _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
                selectedButton = 0;
            }
        
            if (IsVerticalAxisMoreThan0() && selectedButton == 2)
            {
                if (_mainMenuVerticalInputTimer > 0)
                {
                    return;
                }

                _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
                selectedButton = 1;
            }

            if (IsVerticalAxisMoreThan0() && selectedButton == 2) return;

            if (IsVerticalAxisMoreThan0() && selectedButton == 0)
            {
                if (_mainMenuVerticalInputTimer > 0)
                    return;
                _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
                selectedButton = 1;
            }

            if (IsVerticalAxisMoreThan0() && selectedButton == 1)
            {
                if (_mainMenuVerticalInputTimer > 0) return;

                _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
                selectedButton = 2;
            }

            if (!(Time.deltaTime >= _timeDelay) || (!Input.GetButton("Fire1"))) return;
            
            StartCoroutine(nameof(MainMenuButtonPress));
            _timeDelay = Time.deltaTime + _timeBetweenButtonPress;

            bool SelectedButton0()
            {
                return selectedButton == 0;
            }

            bool SelectedButton1()
            {
                return selectedButton == 1;
            }

            bool SelectedButton2()
            {
                return selectedButton == 2;
            }
        }

        private static bool IsVerticalAxisMoreThan0() => Input.GetAxis("Vertical") > 0f;

        private IEnumerator MainMenuManager()
        {
            while (true)
            {
                switch (_mainMenuController)
                {
                    case MainMenuController.MainMenuFadeIn:
                        MainMenuFadeIn();
                        break;
                    case MainMenuController.MainMenuAtIdle:
                        MainMenuAtIdle();
                        break;
                    case MainMenuController.MainMenuFadeOut:
                        MainMenuFadeOut();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return null;
            }
        }

        private void MainMenuFadeIn()
        {
            Debug.Log(nameof(MainMenuFadeIn));

            _mainMenuAudio.volume += _mainMenuFadeSpeed * Time.deltaTime;

            _mainMenuFadeValue += _mainMenuFadeSpeed * Time.deltaTime;

            if (_mainMenuFadeValue > 1)
                _mainMenuFadeValue = 1;

            const double tolerance = 0.01;
            if (Math.Abs(_mainMenuFadeValue - 1) < tolerance)
            {
                _mainMenuController = MainMenuController.MainMenuAtIdle;
            }
        }

        private void MainMenuAtIdle()
        {
            Debug.Log(nameof(MainMenuAtIdle));

            if (_startingOnePlayerGame || _quittingGame)
            {
                _mainMenuController = MainMenuController.MainMenuFadeOut;
            }
        }

        private void MainMenuFadeOut()
        {
            Debug.Log(nameof(MainMenuFadeOut));
        
            _mainMenuAudio.volume -= _mainMenuFadeSpeed * Time.deltaTime;

            _mainMenuFadeValue -= _mainMenuFadeSpeed * Time.deltaTime;

            if (_mainMenuFadeValue < 0)
                _mainMenuFadeValue = 0;

            if (_mainMenuFadeValue == 0 && _startingOnePlayerGame == true)
            {
                SceneManager.LoadScene("ChooseCharacter");
            }
        }

        public void MainMenuButtonPress()
        {
            Debug.Log(nameof(MainMenuFadeIn));
            GUI.FocusControl(_mainMenuButtons[selectedButton]);

            switch (selectedButton)
            {
                case 0:
                    _mainMenuAudio.PlayOneShot(mainMenuStartButtonAudio);
                    _startingOnePlayerGame = true;
                    EnableOnePlayerManager();
                    break;
                case 1:
                    _mainMenuAudio.PlayOneShot(mainMenuStartButtonAudio);
                    _startingTwoPlayerGame = true;
                    EnableTwoPlayerManager();
                    break;
                case 2:
                    _mainMenuAudio.PlayOneShot(mainMenuQuitButtonAudio);
                    _quittingGame = true;
                    break;
            }
        }

        private static void EnableTwoPlayerManager() => GameObject.FindGameObjectWithTag("TwoPlayerManager").GetComponent<TwoPlayerManager>().enabled = true;

        private static void EnableOnePlayerManager() => GameObject.FindGameObjectWithTag("OnePlayerManager").GetComponent<OnePlayerManager>().enabled = true;

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(
                    0,0,
                    Screen.width, Screen.height),
                mainMenuBackground);
        
            GUI.DrawTexture(new Rect(
                    0,0,
                    Screen.width, Screen.height),
                mainMenuTitle);

            GUI.color = new Color(1, 1, 1, _mainMenuFadeValue);
        
            CreateButtonsGroup();

            if (_ps4Controller || _xBoxController)
            {
                GUI.FocusControl(_mainMenuButtons[selectedButton]);
            }
        }

        private void CreateButtonsGroup()
        {
            GUI.BeginGroup(new Rect(
                Screen.width / 2 - _mainMenuButtonWidth / 2,
                Screen.height / 1.5f,
                _mainMenuButtonWidth,
                _mainMenuButtonHeight * 3 + _mainMenuGUIOffset * 2));

            var buttonTexts = new [] { "One Player", "Two Player", "Quit"};
            var rectYs = new[] { 0f, _mainMenuButtonHeight + _mainMenuGUIOffset,
                _mainMenuButtonHeight * 2 + _mainMenuGUIOffset * 2};
            
            for (int i = 0; i < buttonTexts.Length; ++i)
            {
                CreateButton(i, buttonTexts[i], i, rectYs[i]);
            }

            GUI.EndGroup();
        }

        private void CreateButton(int indexMainMenuButton, string buttonText, int selectedButton, float rectY)
        {
            GUI.SetNextControlName(_mainMenuButtons[indexMainMenuButton]); // _onePlayer

            if (GUI.Button(new Rect(
                        0, rectY,
                        _mainMenuButtonWidth, _mainMenuButtonHeight),
                    buttonText))
            {
                this.selectedButton = selectedButton;
                MainMenuButtonPress();
            }
        }
    }
}
