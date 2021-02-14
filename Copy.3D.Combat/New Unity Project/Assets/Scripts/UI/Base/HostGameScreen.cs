using UnityEngine;
using System.Collections;

public class HostGameScreen : CombatScreen
{
    public virtual void GoToNetworkGameScreen() => MainScript.StartNetworkGameScreen();

    public virtual void GoToConnectionLostScreen() => MainScript.StartConnectionLostScreen();

    public virtual void StartHostGame() => MainScript.HostGame();
}