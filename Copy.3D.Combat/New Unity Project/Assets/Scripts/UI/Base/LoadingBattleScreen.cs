using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBattleScreen : CombatScreen
{
    #region public instance methods

    public virtual void StartBattle()
    {
        MainScript.StartGame((float) MainScript.config.gameGUI.gameFadeDuration);
    }

    #endregion
}