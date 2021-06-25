using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static ChooseCharacterManager;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class ChooseCharacter : MonoBehaviour
{
    public Texture2D _selectCharacterTextBackground;
    public Texture2D _selectCharacterTextForeground;
    public Texture2D _selectCharacterText;
    
    public Texture2D _selectCharacterXboxLeft;
    public Texture2D _selectCharacterKeyboardLeft;
    public Texture2D _selectCharacterKeyboardRight;
    public Texture2D _selectCharacterXboxRight;
    public Texture2D _selectCharacterPSLeft;
    public Texture2D _selectCharacterPSRight;

    private float _foregroundTextWidth;
    private float _foregroundTextHeight;
    private float _leftRightControllerIconSize;
    
    
    private float _chooseCharacterInputTimer;
    private float _chooseCharacterInputDelay = 1f;

    public AudioClip _cycleCharacterButtonPress;

    private GameObject _characterDemo;
    public static bool _demoPlayer;

    private int _pickRandomCharacter;
    
    public int _yRot = 180;

    private GameObject _switchCharacterParticleSystem;
    
    private int _characterSelectState;
    
    // Start is called before the first frame update
    void Start()
    {
        CharacterSelectManager();

        _demoPlayer = true;

        _foregroundTextWidth = Screen.width / 1.5f;
        _foregroundTextHeight = Screen.height / 10f;
        _leftRightControllerIconSize = Screen.height / 10f;
    }

    private void CharacterSelectManager()
    {
        switch (_characterSelectState)
        {
            default:
            case 0:
                BlackRobot();
                break;
            case 1:
                WhiteRobot();
                break;
            case 2:
                RedRobot();
                break;
            case 3:
                BlueRobot();
                break;
            case 4:
                BrownRobot();
                break;
            case 5:
                GreenRobot();
                break;
            case 6:
                PinkRobot();
                break;
            case 7:
                GoldRobot();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _demoPlayer = false;
            SendMessageSceneBackgroundLoad();
        }
        
        if (_chooseCharacterInputTimer > 0)
        {
            _chooseCharacterInputTimer -= 1f * Time.deltaTime;
        }

        if (_chooseCharacterInputTimer > 0)
        {
            return;
        }

        if (Input.GetAxis("Horizontal") < -.5f)
        {
            if (_characterSelectState == 0)
            {
                return;
            }

            GetComponent<AudioSource>();
            PlayOneShot(_cycleCharacterButtonPress);

            _characterSelectState--;
            InstantiateSwitchCharParticle();
            CharacterSelectManager();
            _chooseCharacterInputTimer = _chooseCharacterInputDelay;
        }
        
        if (Input.GetAxis("Horizontal") > .5f)
        {
            if (_characterSelectState == 7)
            {
                return;
            }
            
            GetComponent<AudioSource>();
            PlayOneShot(_cycleCharacterButtonPress);

            _characterSelectState++;
            InstantiateSwitchCharParticle();
            CharacterSelectManager();
            _chooseCharacterInputTimer = _chooseCharacterInputDelay;
        }

        if (Input.GetButtonDown("Select"))
        {
            _pickRandomCharacter = Random.Range(0, 7);

            _characterSelectState = _pickRandomCharacter;
            
            GetComponent<AudioSource>().PlayOneShot(_cycleCharacterButtonPress);
            
            InstantiateSwitchCharParticle();
            CharacterSelectManager();
        }
    }

    private void InstantiateSwitchCharParticle()
    {
        _switchCharacterParticleSystem = Instantiate(Resources.Load("SwitchCharParticle")) as GameObject;

        _switchCharacterParticleSystem.transform.position = new Vector3(-.5f, 0, -8);
    }

    private void SendMessageSceneBackgroundLoad()
    {
        GameObject.FindGameObjectWithTag(nameof(BackgroundManager))
            .GetComponent<BackgroundManager>()
            .SendMessage("SceneBackgroundLoad");
    }

    private void PlayOneShot(AudioClip cycleCharacterButtonPress)
    {
        //TODO:
    }

    private void BlackRobot()
    {
        Debug.Log(nameof(BlackRobot));
        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("BlackRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);

        _robotBlack = true;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = false;
    }
    private void WhiteRobot()
    {
        Debug.Log(nameof(WhiteRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("WhiteRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);
        
        _robotBlack = false;
        _robotWhite = true;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = false;
    }
    private void RedRobot()
    {
        Debug.Log(nameof(RedRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("RedRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);
        
        _robotBlack = false;
        _robotWhite = false;
        _robotRed = true;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = false;
    }
    private void BlueRobot()
    {
        Debug.Log(nameof(BlueRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("BlueRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);
        
        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = true;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = false;
    }
    private void BrownRobot()
    {
        Debug.Log(nameof(BrownRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("BrownRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);
        
        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = true;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = false;
    }
    private void GreenRobot()
    {
        Debug.Log(nameof(GreenRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("GreenRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);

        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = true;
        _robotPink = false;
        _robotGold = false;
    }
    private void PinkRobot()
    {
        Debug.Log(nameof(PinkRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("PinkRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);

        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = true;
        _robotGold = false;
    }
    private void GoldRobot()
    {
        Debug.Log(nameof(GoldRobot));

        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("GoldRobot")) as GameObject;

        _characterDemo.GetComponent<OpponentAI>().enabled = false;
        _characterDemo.GetComponent<OpponentHealth>().enabled = false;
        
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
        _characterDemo.transform.eulerAngles =
            new Vector3(0, _yRot, 0);

        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = true;
    }

    private void OnGUI()
    {
        
        GUI.DrawTexture(new Rect(
            0, 0,
            Screen.width, Screen.height / 10),
            _selectCharacterTextBackground);
            
        GUI.DrawTexture(new Rect(
                Screen.width / 2 - (_foregroundTextWidth / 2), 
                0,
                _foregroundTextWidth, _foregroundTextHeight),
            _selectCharacterTextForeground);
            
        GUI.DrawTexture(new Rect(
                Screen.width / 2 - (_foregroundTextWidth / 2), 
                0,
                _foregroundTextWidth, _foregroundTextHeight),
            _selectCharacterText);


        if (GameObject.FindGameObjectWithTag(nameof(ControllerManager)).GetComponent<ControllerManager>()
            .xBOXController)
        {
            GUI.DrawTexture(new Rect(
                    Screen.width / 2 - (_foregroundTextWidth / 2) - _leftRightControllerIconSize, 
                    0,
                    _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterXboxLeft);
            
            GUI.DrawTexture(new Rect(
                    Screen.width / 2 + (_foregroundTextWidth / 2), 
                    0,
                    _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterXboxRight);
        }


        else if (GameObject.FindGameObjectWithTag(nameof(ControllerManager)).GetComponent<ControllerManager>()
            .pS4Controller)
        {
            GUI.DrawTexture(new Rect(
                    Screen.width / 2 - (_foregroundTextWidth / 2) - _leftRightControllerIconSize, 
                    0,
                    _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterPSLeft);
            
            GUI.DrawTexture(new Rect(
                    Screen.width / 2 + (_foregroundTextWidth / 2), 
                    0,
                    _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterPSRight);
        }
        else
        {
            GUI.DrawTexture(new Rect(
                    Screen.width / 2 - (_foregroundTextWidth / 2) - _leftRightControllerIconSize, 
                    0,
                    _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterKeyboardLeft);
            
            GUI.DrawTexture(new Rect(
                    Screen.width / 2 + (_foregroundTextWidth / 2), 
                    0,
                    _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterKeyboardRight);
        }
    }
}
