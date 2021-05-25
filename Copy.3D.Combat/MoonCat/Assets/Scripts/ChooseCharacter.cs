using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChooseCharacterManager;

[RequireComponent(typeof(AudioSource))]
public class ChooseCharacter : MonoBehaviour
{
    public float _chooseCharacterInputTimer;
    public float _chooseCharacterInputDelay = 1f;

    private GameObject _characterDemo;

    public int _characterSelectState;
    
    
    // Start is called before the first frame update
    void Start()
    {
        CharacterSelectManager();
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

            _characterSelectState--;
            CharacterSelectManager();
            _chooseCharacterInputTimer = _chooseCharacterInputDelay;
        }
        
        if (Input.GetAxis("Horizontal") > -.5f)
        {
            if (_characterSelectState == 7)
            {
                return;
            }

            _characterSelectState++;
            CharacterSelectManager();
            _chooseCharacterInputTimer = _chooseCharacterInputDelay;
        }
    }
    
    private void BlackRobot()
    {
        Debug.Log(nameof(BlackRobot));
        Destroy(_characterDemo);
        _characterDemo = Instantiate(Resources.Load("BlackRobot")) as GameObject;
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);

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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);
        
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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);

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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);

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
        _characterDemo.transform.position =
            new Vector3(-.5f, 0, -7);

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
        //TODO:
    }
}
