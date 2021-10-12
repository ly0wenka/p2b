using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGUI : MonoBehaviour
{
    private int _returnedCurrentTimerValue;

    public float _returnMaximumPlayerHealth;
    public float _returnCurrentPlayerHealth;
    public float _playerHealthBarLength;
    public float _returnMaximumOpponentHealth;
    public float _returnCurrentOpponentHealth;
    public float _opponentHealthBarLength;

    private Vector2 _healthBarSize;
    private Vector2 _fightGUITimerSize;

    public Texture2D _healthBarMinTexture;
    public Texture2D _healthBarMaxTexture;
    public Texture2D _healthBarOutlineTexture;

    public float _fightGUIHeightPos;
    private float _fightGUIOffSet;
    private float _fightGUIPosModifier;
    
    private GUIStyle _fightGUISkin;

    // Start is called before the first frame update
    void Start()
    {
        _fightGUIOffSet = Screen.width / 12;

        _fightGUITimerSize = new Vector2(Screen.width / 7.5f, Screen.height / 7.5f);

        _healthBarSize = new Vector2(Screen.width / 2.25f, Screen.height / 15f);
    }

    // Update is called once per frame
    void Update()
    {
        _returnCurrentPlayerHealth = PlayerOneHealth._currentPlayerHealth;
        _returnCurrentOpponentHealth = OpponentHealth._currentOpponentHealth;
        _returnMaximumPlayerHealth = PlayerOneHealth._maximumPlayerHealth;
        _returnMaximumOpponentHealth = OpponentHealth._maximumOpponentHealth;

        _playerHealthBarLength = (_returnCurrentPlayerHealth / _returnMaximumPlayerHealth);

        _opponentHealthBarLength = (_returnCurrentOpponentHealth / _returnMaximumOpponentHealth);
    }

    private void LateUpdate()
    {
        _returnedCurrentTimerValue = FightManager._currentFightTimer;
    }

    private void OnGUI()
    {
        _fightGUISkin = new GUIStyle(GUI.skin.GetStyle("Label"));

        _fightGUISkin.fontSize = Screen.width / 10;
        _fightGUISkin.alignment = TextAnchor.MiddleCenter;

        _fightGUIHeightPos = _fightGUIOffSet / 50;

        if (_returnedCurrentTimerValue >= 10)
        {
            GUI.Label(new Rect(
                    Screen.width / 2 - (_fightGUITimerSize.x / 2),
                    _fightGUIHeightPos,
                    _fightGUITimerSize.x, _fightGUITimerSize.y),
                "0" + _returnedCurrentTimerValue.ToString(), _fightGUISkin);
        }

        if (_returnedCurrentTimerValue < 10)
        {
            GUI.Label(new Rect(
                    Screen.width / 2 - (_fightGUITimerSize.x / 2),
                    _fightGUIHeightPos,
                    _fightGUITimerSize.x, _fightGUITimerSize.y),
                "0" + _returnedCurrentTimerValue.ToString(), _fightGUISkin);
        }

        DrawPlayerHealth();
        DrawOpponentHealth();
    }

    private void DrawPlayerHealth()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        GUI.DrawTexture(new Rect(
                _fightGUIOffSet, _fightGUIOffSet / 2,
                _healthBarSize.x, _healthBarSize.y),
            _healthBarMinTexture);

        GUI.DrawTexture(new Rect(
                _fightGUIOffSet, _fightGUIOffSet / 2,
                _healthBarSize.x * _playerHealthBarLength, _healthBarSize.y),
            _healthBarMaxTexture);

        GUI.DrawTexture(new Rect(
                _fightGUIOffSet - _fightGUIPosModifier / 2, 
                _fightGUIOffSet / 2 - _fightGUIPosModifier / 2,
                _healthBarSize.x + _fightGUIPosModifier,
                _healthBarSize.y + _fightGUIPosModifier),
            _healthBarOutlineTexture);

        GUI.EndGroup();
    }

    private void DrawOpponentHealth()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        GUI.DrawTexture(new Rect(
                Screen.width - (_fightGUIOffSet + _healthBarSize.x),
                _fightGUIOffSet / 2,
                _healthBarSize.x, _healthBarSize.y),
            _healthBarMinTexture);

        GUI.DrawTexture(new Rect(
                Screen.width - (_fightGUIOffSet + _healthBarSize.x),
                _fightGUIOffSet / 2,
                _healthBarSize.x + _opponentHealthBarLength, _healthBarSize.y),
            _healthBarMaxTexture);

        GUI.DrawTexture(new Rect(
                Screen.width - (
                    _fightGUIOffSet + _healthBarSize.x + _fightGUIPosModifier / 2),
                _fightGUIOffSet / 2 - _fightGUIPosModifier / 2,
                _healthBarSize.x + _fightGUIPosModifier, 
                _healthBarSize.y + _fightGUIPosModifier),
            _healthBarMinTexture);

        GUI.EndGroup();
    }
}