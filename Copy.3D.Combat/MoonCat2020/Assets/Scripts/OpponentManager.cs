﻿using System;
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
    };
        
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
        
        _opponentCounter = 0;

        _selectedOpponent = _opponentOrder.First();

        for (var i = 0; i < _opponentOrder.Length; i++)
        {
            var _opTemp = _opponentOrder[i];
            var _randomOrder = Random.Range(i, _opponentOrder.Length);
            _opponentOrder[i] = _opponentOrder[_randomOrder];
            _opponentOrder[_randomOrder] = _opTemp;
        }

        foreach (var opponent in _opponentOrder)
        {
            print(opponent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadCurrentOpponent()
    {
        if (_selectedOpponent == "BlackRobotOpponent" && !_returnRobotBlack)
        {
            _currentOpponent = Instantiate(Resources.Load("BlackRobot")) as GameObject;
        }
        else if (_selectedOpponent == "BlackRobotOpponent" && !_returnRobotBlack)
        {
            
        }
        
    }
}
