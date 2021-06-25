using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class OpponentManager : MonoBehaviour
{
    public GameObject _currentOpponent;

    public string _selectedOpponent = String.Empty;

    private bool _returnRobotBlack;
    private bool _returnRobotWhite;
    private bool _returnRobotRed;
    private bool _returnRobotBlue;
    private bool _returnRobotBrown;
    private bool _returnRobotGreen;
    private bool _returnRobotPink;
    private bool _returnRobotGold;

    public int _opponentCounter;

    public string[] _opponentOrder = new[]
    {
        "BlackRobotOpponent",
        "WhiteRobotOpponent",
        "RedRobotOpponent",
        "BlueRobotOpponent",
        "BrownRobotOpponent",
        "GreenRobotOpponent",
        "PinkRobotOpponent",
        "GoldRobotOpponent",
    };
    
    private int _yRot = 0;
        
    // Start is called before the first frame update
    void Start()
    {
        _opponentCounter = 0;

        _selectedOpponent = _opponentOrder.First();

        for (var i = 0; i < _opponentOrder.Length; i++)
        {
            var _opTemp = _opponentOrder[i];
            var _randomOrder = Random.Range(i, _opponentOrder.Length);
            _opponentOrder[i] = _opponentOrder[_randomOrder];
            _opponentOrder[_randomOrder] = _opTemp;
        }

        // foreach (var opponent in _opponentOrder)
        // {
        //     print(opponent);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCurrentOpponent()
    {
        InitReturnRobots();

        if(TryInstantiateAltOpponent("BlackRobotOpponent", "BlackRobot", "BlackRobotAlt", _returnRobotBlack))
        {
            return;
        }
        if(TryInstantiateAltOpponent("WhiteRobotOpponent", "WhiteRobot", "WhiteRobotAlt", _returnRobotWhite))
        {
            return;
        }
        if(TryInstantiateAltOpponent("RedRobotOpponent", "RedRobot", "RedRobotAlt", _returnRobotRed))
        {
            return;
        }
        if(TryInstantiateAltOpponent("BlueRobotOpponent", "BlueRobot", "BlueRobotAlt", _returnRobotBlue))
        {
            return;
        }
        if(TryInstantiateAltOpponent("BrownRobotOpponent", "BrownRobot", "BrownRobotAlt", _returnRobotBrown))
        {
            return;
        }
        if(TryInstantiateAltOpponent("GreenRobotOpponent", "GreenRobot", "GreenRobotAlt", _returnRobotGreen))
        {
            return;
        }
        if(TryInstantiateAltOpponent("PinkRobotOpponent", "PinkRobot", "PinkRobotAlt", _returnRobotPink))
        {
            return;
        }
        if(TryInstantiateAltOpponent("GoldRobotOpponent", "GoldRobot", "GoldRobotAlt", _returnRobotGold))
        {
            return;
        }
        SetOpponent();
    }

    private void InitReturnRobots()
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

    private void SetOpponent()
    {
        _currentOpponent.transform.position = new Vector3(1, 0, -7);
        _currentOpponent.transform.eulerAngles = new Vector3(0, _yRot, 0);
        
        FightCamera._opponent = _currentOpponent;

        _currentOpponent.GetComponent<PlayerOneMovement>().enabled = false;
        _currentOpponent.GetComponent<PlayerOneHealth>().enabled = false;
        
        _currentOpponent.GetComponent<OpponentAI>().enabled = true;
        _currentOpponent.GetComponent<OpponentHealth>().enabled = true;
    }

    private bool TryInstantiateAltOpponent(string selectedOpponent, string pathResource, string altPathResource, bool isReturnOpponent)
    {
        if (_selectedOpponent == selectedOpponent && !isReturnOpponent)
        {
            _currentOpponent = Instantiate(Resources.Load(pathResource)) as GameObject;
        }
        else if (_selectedOpponent == selectedOpponent && isReturnOpponent)
        {
            _currentOpponent = Instantiate(Resources.Load(altPathResource)) as GameObject;
            SetOpponent();
            return true;
        }

        return false;
    }
}
