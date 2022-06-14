using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    private int _selectedButton = 0;
    private float _timeBetweenButtonPress = 0.1f;
    private float _timeDelay;

    private float _mainMenuVerticalInputTimer;
    private float _mainMenuVerticalInputDelay = 1f;

    public Texture2D _mainMenuBackground;
    public Texture2D _mainMenuTitle;

    private AudioSource _mainMenuAudio;
    public AudioClip _mainMenuMusic;
    public AudioClip _mainMenuStartButtonAudio;
    public AudioClip _mainMenuQuitButtonAudio;

    private float _mainMenuFadeValue;
    private float _mainMenuFadeSpeed = .5f;

    private float _mainMenuButtonWidth = 100f;
    private float _mainMenuButtonHeight = 25f;
    private float _mainMenuGUIOffset = 10f;

    private bool _startingOnePlayerGame;
    private bool _startingTwoPlayerGame;
    private bool _quittingGame;
    
    private bool _ps4Controller;
    private bool _xBOXController;
    private string[] _mainMenuButtons = new []
    {
        "_onePlayer",
        "_twoPlayer",
        "_quit"
    };

    private MainMenuController _mainMenuController;
    // Start is called before the first frame update
    void Start()
    {
        _ps4Controller = false;
        _xBOXController = false;
        _mainMenuController = MainMenuController.MainMenuFadeIn;

        _mainMenuFadeValue = 0;
        
        _mainMenuAudio = GetComponent<AudioSource>();

        _mainMenuAudio.volume = 0;
        _mainMenuAudio.clip = _mainMenuMusic;
        _mainMenuAudio.loop = true;
        _mainMenuAudio.Play();
        
        StartCoroutine(MainMenuManager());
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainMenuFadeValue > 1)
        {
            return;
        }

    
        var _joyStickNames = Input.GetJoystickNames();
        for (int i = 0; i < _joyStickNames.Length; i++)
        {
            if (_joyStickNames[i].Length == 0)
            {
                return;
            }

            if (_joyStickNames[i].Length == 19)
            {
                _ps4Controller = true;
            }

            if (_joyStickNames[i].Length == 33)
            {
                _xBOXController = true;
            }
        }

        if (_mainMenuVerticalInputTimer > 0)
            _mainMenuVerticalInputTimer -= 1f * Time.deltaTime;

        if (Input.GetAxis("Vertical") > 0f && _selectedButton == 0)
        {
            return;
        }

        if (Input.GetAxis("Vertical") > 0f && _selectedButton == 1)
        {
            if (_mainMenuVerticalInputTimer > 0)
                return;
            _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
            _selectedButton = 0;
        }
        
        if (Input.GetAxis("Vertical") > 0f && _selectedButton == 2)
        {
            if (_mainMenuVerticalInputTimer > 0)
                return;
            _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
            _selectedButton = 1;
        }

        if (Input.GetAxis("Vertical") < 0f && _selectedButton == 2)
        {
            return;
        }
        
        if (Input.GetAxis("Vertical") < 0f && _selectedButton == 0)
        {
            if (_mainMenuVerticalInputTimer > 0)
                return;
            _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
            _selectedButton = 1;
        }
        
        if (Input.GetAxis("Vertical") < 0f && _selectedButton == 1)
        {
            if (_mainMenuVerticalInputTimer > 0)
                return;
            _mainMenuVerticalInputTimer = _mainMenuVerticalInputDelay;
            _selectedButton = 2;
        }
        
        if (Time.deltaTime >= _timeDelay && (Input.GetButton("Fire1")))
        {
            StartCoroutine(nameof(MainMenuButtonPress));
            _timeDelay = Time.deltaTime + _timeBetweenButtonPress;
        }
    }

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

        if (_mainMenuFadeValue == 1)
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

    private void MainMenuButtonPress()
    {
        Debug.Log(nameof(MainMenuFadeIn));
        GUI.FocusControl(_mainMenuButtons[_selectedButton]);

        switch (_selectedButton)
        {
            case 0:
                _mainMenuAudio.PlayOneShot(_mainMenuStartButtonAudio);
                _startingOnePlayerGame = true;
                EnableOnePlayerManager();
                break;
            case 1:
                _mainMenuAudio.PlayOneShot(_mainMenuStartButtonAudio);
                _startingTwoPlayerGame = true;
                EnableTwoPlayerManager();
                break;
            case 2:
                _mainMenuAudio.PlayOneShot(_mainMenuQuitButtonAudio);
                _quittingGame = true;
                break;
        }
    }

    private static void EnableTwoPlayerManager()
    {
        GameObject.FindGameObjectWithTag("TwoPlayerManager").GetComponent<TwoPlayerManager>().enabled = true;
    }

    private static void EnableOnePlayerManager()
    {
        GameObject.FindGameObjectWithTag("OnePlayerManager").GetComponent<OnePlayerManager>().enabled = true;
    }

    private void OnGUI()
    {
        
        GUI.DrawTexture(new Rect(
            0,0,
            Screen.width, Screen.height),
            _mainMenuBackground);
        
        GUI.DrawTexture(new Rect(
            0,0,
            Screen.width, Screen.height),
            _mainMenuTitle);

        GUI.color = new Color(1, 1, 1, _mainMenuFadeValue);
        
        GUI.BeginGroup(new Rect(
            Screen.width / 2 - _mainMenuButtonWidth / 2,
            Screen.height / 1.5f,
            _mainMenuButtonWidth,
            _mainMenuButtonHeight * 3 + _mainMenuGUIOffset * 2));
        
        CreateButton(0,"One Player",0, 0);
        CreateButton(1,"Two Player",1, _mainMenuButtonHeight + _mainMenuGUIOffset);
        CreateButton(2,"Quit",2, _mainMenuButtonHeight * 2 + _mainMenuGUIOffset * 2);

        
        GUI.EndGroup();

        if (_ps4Controller || _xBOXController)
        {
            GUI.FocusControl(_mainMenuButtons[_selectedButton]);
        }
    }

    private void CreateButton(int indexMainMenuButton, string buttonText, int selectedButton, float rectY)
    {
        GUI.SetNextControlName(_mainMenuButtons[indexMainMenuButton]); // _onePlayer

        if (GUI.Button(new Rect(
                0, rectY,
                _mainMenuButtonWidth, _mainMenuButtonHeight),
            buttonText))
        {
            _selectedButton = selectedButton;
            MainMenuButtonPress();
        }
    }
}
