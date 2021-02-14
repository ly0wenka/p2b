using UnityEngine;
using System.Collections;

public class VersusModeScreen : CombatScreen
{
    public virtual void SelectPlayerVersusPlayer() => MainScript.StartPlayerVersusPlayer();

    public virtual void SelectPlayerVersusCpu() => MainScript.StartPlayerVersusCpu();

    public virtual void SelectCpuVersusCpu() => MainScript.StartCpuVersusCpu();

    public virtual void GoToMainMenu() => MainScript.StartMainMenuScreen();
}