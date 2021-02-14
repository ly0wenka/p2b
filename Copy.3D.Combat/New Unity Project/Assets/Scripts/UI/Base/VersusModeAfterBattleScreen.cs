using UnityEngine;
using System;
using System.Reflection;

public class VersusModeAfterBattleScreen : CombatScreen
{
    #region protected enum definitions

    protected enum Option
    {
        RepeatBattle = 0,
        CharacterSelectionScreen = 1,
        StageSelectionScreen = 2,
        MainMenu = 3,
    }

    #endregion

    #region public instance methods

    public virtual void GoToCharacterSelectionScreen() =>
        this.TrySelectOption((int) VersusModeAfterBattleScreen.Option.CharacterSelectionScreen,
            MainScript.GetLocalPlayer());

    public virtual void GoToMainMenu() =>
        this.TrySelectOption((int) VersusModeAfterBattleScreen.Option.MainMenu, MainScript.GetLocalPlayer());

    public virtual void GoToStageSelectionScreen() =>
        this.TrySelectOption((int) VersusModeAfterBattleScreen.Option.StageSelectionScreen,
            MainScript.GetLocalPlayer());

    public virtual void RepeatBattle() => this.TrySelectOption((int) VersusModeAfterBattleScreen.Option.RepeatBattle,
        MainScript.GetLocalPlayer());

    #endregion

    #region public override methods

    public override void SelectOption(int option, int player)
    {
        VersusModeAfterBattleScreen.Option selectedOption = (VersusModeAfterBattleScreen.Option) option;
        if (selectedOption == VersusModeAfterBattleScreen.Option.CharacterSelectionScreen)
        {
            MainScript.StartCharacterSelectionScreen();
        }
        else if (selectedOption == VersusModeAfterBattleScreen.Option.MainMenu)
        {
            MainScript.StartMainMenuScreen();
        }
        else if (selectedOption == VersusModeAfterBattleScreen.Option.StageSelectionScreen)
        {
            MainScript.StartStageSelectionScreen();
        }
        else if (selectedOption == VersusModeAfterBattleScreen.Option.RepeatBattle)
        {
            MainScript.StartLoadingBattleScreen();
        }
    }

    #endregion

    #region protected virtual methods

    protected virtual void TrySelectOption(int option, int player)
    {
        // Check if he was playing online or not...
        if (!MainScript.isConnected)
        {
            // If it's a local game, go to the selected screen immediately...
            this.SelectOption(option, player);
        }
        else
        {
            // If it's an online game, we need to inform the other client about the screen we want to go...
            int localPlayer = MainScript.GetLocalPlayer();
            if (localPlayer == player)
            {
                // We don't invoke the SelectOption() method immediately because we are using the frame-delay 
                // algorithm to keep players synchronized, so we can't invoke the SelectOption() method
                // until the other player has received the message with our choice.
                MainScript.fluxCapacitor.RequestOptionSelection(player, (sbyte) option);
            }
        }
    }

    #endregion
}