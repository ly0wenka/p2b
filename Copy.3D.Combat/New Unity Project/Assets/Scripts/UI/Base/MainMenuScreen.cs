using UnityEngine;
using System.Collections;

public class MainMenuScreen : CombatScreen
{
    public virtual void Quit() => MainScript.Quit();

    public virtual void GoToBluetoothPlayScreen() => MainScript.StartBluetoothGameScreen();

    public virtual void GoToSearchMatchScreen() => MainScript.StartSearchMatchScreen();

    public virtual void GoToStoryModeScreen() => MainScript.StartStoryMode();

    public virtual void GoToVersusModeScreen() => MainScript.StartVersusModeScreen();

    public virtual void GoToTrainingModeScreen() => MainScript.StartTrainingMode();

    public virtual void GoToNetworkPlayScreen() => MainScript.StartNetworkGameScreen();

    public virtual void GoToOptionsScreen() => MainScript.StartOptionsScreen();

    public virtual void GoToCreditsScreen() => MainScript.StartCreditsScreen();
}