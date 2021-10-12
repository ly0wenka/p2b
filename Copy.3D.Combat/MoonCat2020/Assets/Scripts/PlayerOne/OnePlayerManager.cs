using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerManager : GameManager
{
    private GameObject _playerOneCharacter;

    private bool _returnRobotBlack;
    private bool _returnRobotWhite;
    private bool _returnRobotRed;
    private bool _returnRobotBlue;
    private bool _returnRobotBrown;
    private bool _returnRobotGreen;
    private bool _returnRobotPink;
    private bool _returnRobotGold;

    private GameObject _playerHeadHit;
    private GameObject _playerBodyHit;
    private GameObject _opponentHeadHit;
    private GameObject _opponentBodyHit;
    
    // Start is called before the first frame update
    void Start()
    {
        _returnRobotBlack = ChooseCharacterManager._robotBlack;
        _returnRobotWhite = ChooseCharacterManager._robotWhite;
        _returnRobotRed = ChooseCharacterManager._robotRed;
        _returnRobotBlue = ChooseCharacterManager._robotBlue;
        _returnRobotBrown = ChooseCharacterManager._robotBrown;
        _returnRobotGreen = ChooseCharacterManager._robotGreen;
        _returnRobotPink = ChooseCharacterManager._robotPink;
        _returnRobotGold = ChooseCharacterManager._robotGold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayerOneCharacter()
    {
        Debug.Log(nameof(LoadPlayerOneCharacter));

        if (_playerOneCharacter != null)
        {
            return;
        }
        
        _returnRobotBlack = ChooseCharacterManager._robotBlack;
        _returnRobotWhite = ChooseCharacterManager._robotWhite;
        _returnRobotRed = ChooseCharacterManager._robotRed;
        _returnRobotBlue = ChooseCharacterManager._robotBlue;
        _returnRobotBrown = ChooseCharacterManager._robotBrown;
        _returnRobotGreen = ChooseCharacterManager._robotGreen;
        _returnRobotPink = ChooseCharacterManager._robotPink;
        _returnRobotGold = ChooseCharacterManager._robotGold;
        
        if (_returnRobotBlack)
        {
            _playerOneCharacter = Instantiate(Resources.Load("BlackRobot")) as GameObject;
        }

        if (_returnRobotWhite)
        {
            _playerOneCharacter = Instantiate(Resources.Load("WhiteRobot")) as GameObject;
        }

        if (_returnRobotRed)
        {
            _playerOneCharacter = Instantiate(Resources.Load("RedRobot")) as GameObject;
        }

        if (_returnRobotBlue)
        {
            _playerOneCharacter = Instantiate(Resources.Load("BlueRobot")) as GameObject;
        }

        if (_returnRobotBrown)
        {
            _playerOneCharacter = Instantiate(Resources.Load("BrownRobot")) as GameObject;
        }

        if (_returnRobotGreen)
        {
            _playerOneCharacter = Instantiate(Resources.Load("GreenRobot")) as GameObject;
        }

        if (_returnRobotPink)
        {
            _playerOneCharacter = Instantiate(Resources.Load("PinkRobot")) as GameObject;
        }

        if (_returnRobotGold)
        {
            _playerOneCharacter = Instantiate(Resources.Load("GoldRobot")) as GameObject;
        }

        SetPlayerOneCharacterTransform();

        FightCamera._playerOne = _playerOneCharacter;
        OpponentAI._playerOne = _playerOneCharacter;
        PlayerOneMovement._playerOne = _playerOneCharacter;
        
        SetPlayerScriptsToActive();
        SetOpponentScriptsToInactive();
        SetPlayerGameObjectsToActive();
        SetPlayerGameObjectsToInactive();
    }

    private void SetPlayerScriptsToActive()
    {
        _playerOneCharacter.GetComponent<PlayerOneMovement>().enabled = true;
        _playerOneCharacter.GetComponent<PlayerOneHealth>().enabled = true;
        _playerOneCharacter.GetComponentInChildren<PlayerHighKick>().enabled = true;
        _playerOneCharacter.GetComponentInChildren<PlayerLowKick>().enabled = true;
        _playerOneCharacter.GetComponentInChildren<PlayerPunchLeft>().enabled = true;
        _playerOneCharacter.GetComponentInChildren<PlayerPunchRight>().enabled = true;
    }

    private void SetOpponentScriptsToInactive()
    {
        _playerOneCharacter.GetComponent<OpponentAI>().enabled = false;
        _playerOneCharacter.GetComponent<OpponentHealth>().enabled = false;
        _playerOneCharacter.GetComponentInChildren<OpponentHighKick>().enabled = false;
        _playerOneCharacter.GetComponentInChildren<OpponentLowKick>().enabled = false;
        _playerOneCharacter.GetComponentInChildren<OpponentPunchLeft>().enabled = false;
        _playerOneCharacter.GetComponentInChildren<OpponentPunchRight>().enabled = false;
    }

    private void SetPlayerOneCharacterTransform()
    {
        _playerOneCharacter.transform.position = _playerStartingPosition;
        _playerOneCharacter.transform.eulerAngles = _playerStartingRotation;
    }

    private void SetPlayerGameObjectsToActive()
    {
        _playerHeadHit = _playerOneCharacter.transform.Find("PlayerHeadHit").gameObject;
        _playerBodyHit = _playerOneCharacter.transform.Find("PlayerBodyHit").gameObject;
        _playerHeadHit.SetActive(true);
        _playerBodyHit.SetActive(true);
    }

    private void SetPlayerGameObjectsToInactive()
    {
        _opponentHeadHit = _playerOneCharacter.transform.Find("OpponentHeadHit").gameObject;
        _opponentBodyHit = _playerOneCharacter.transform.Find("OpponentBodyHit").gameObject;
        _opponentHeadHit.SetActive(false);
        _opponentBodyHit.SetActive(false);
    }
}
