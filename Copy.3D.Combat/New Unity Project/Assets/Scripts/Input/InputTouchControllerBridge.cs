using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputTouchControllerBridge : MonoBehaviour
{
    virtual public void Init() { }
    abstract public float GetAxis(string axisName);
    abstract public float GetAxisRaw(string axisName);
    abstract public bool GetButton(string axisName);
    abstract public void ShowBattleControls(bool visible, bool animate);
    private bool prevBattleGUI,
        prevGamePaused;

    private void OnEnable()
    {
        MainScript.OnGameEnds += this.OnGameEnds;
        MainScript.OnRoundBegins += this.OnRoundBegins;
        MainScript.OnRoundEnds += this.OnRoundEnds;
        MainScript.OnGamePaused += this.OnGamePaused;
        MainScript.OnScreenChanged += this.OnScreenChanged;

        this.prevBattleGUI = false;
        this.prevGamePaused = false;

        this.Init();
    }

    private void OnDisable()
    {
        MainScript.OnGameEnds -= this.OnGameEnds;
        MainScript.OnRoundBegins -= this.OnRoundBegins;
        MainScript.OnRoundEnds -= this.OnRoundEnds;
        MainScript.OnGamePaused -= this.OnGamePaused;
        MainScript.OnScreenChanged -= this.OnScreenChanged;
    }

    public void DoFixedUpdate()
    {
        bool battleGUI = (MainScript.battleGUI != null);
        bool gamePaused = MainScript.isPaused();
    }

    private void OnScreenChanged(CombatScreen previousScreen, CombatScreen newScreen)
    {
    }

    private void OnGamePaused(bool isPaused)
    {
    }

    private void OnRoundEnds(CharacterInfo winner, CharacterInfo loser)
    {
    }

    private void OnRoundBegins(int roundNum)
    {
    }

    private void OnGameEnds(CharacterInfo winner, CharacterInfo loser)
    {
        this.ShowBattleControls(false, false);
    }
}
