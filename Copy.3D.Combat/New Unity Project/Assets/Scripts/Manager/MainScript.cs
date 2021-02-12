using FPLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
public class MainScript : MonoBehaviour
{
    public static Fix64 timeScale;
    #region screen definitions
    public static CombatScreen currentScreen { get; protected set; }
    public static CombatScreen battleGUI { get; protected set; }
    public static GameObject gameEngine { get; protected set; }
    #endregion

    public static bool isPaused() => MainScript.timeScale <= 0;

    public delegate void ScreenChangedHandler(CombatScreen previousScreen, CombatScreen newScreen);
    public static event ScreenChangedHandler OnScreenChanged;

    public delegate void GameEndsHandler(CharacterInfo winner, CharacterInfo loser);
    public static event GameEndsHandler OnGameEnds;
    public static event GameEndsHandler OnRoundEnds;

    public delegate void IntHandler(int newInt);
    public static event IntHandler OnRoundBegins;

    public delegate void GamePausedHandler(bool isPaused);
    public static event GamePausedHandler OnGamePaused;
}
