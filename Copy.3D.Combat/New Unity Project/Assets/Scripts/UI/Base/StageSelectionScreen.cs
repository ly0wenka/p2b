using UnityEngine;
using System;
using System.Reflection;

public class StageSelectionScreen : CombatScreen
{
    #region public instance properties

    public AudioClip selectSound;
    public AudioClip cancelSound;
    public bool fadeBeforeGoingToLoadingBattleScreen = false;

    #endregion

    #region protected instance fields

    protected bool closing = false;
    protected int stageHoverIndex = 0;

    #endregion

    #region public instance methods

    public virtual void GoToCharacterSelectionScreen()
    {
        if (MainScript.gameMode == GameMode.NetworkGame)
        {
            MainScript.config.selectedStage = null;
            this.TrySelectStage(-1);
        }
        else
        {
            this.StartLoadingCharacterSelectionScreen();
        }
    }

    public virtual void GoToLoadingBattleScreen()
    {
        this.StartLoadingBattleScreen();
    }

    public virtual void SetHoverIndex(int stageIndex)
    {
        if (!this.closing && stageIndex >= 0 && stageIndex < MainScript.config.stages.Length)
        {
            this.stageHoverIndex = stageIndex;
        }
    }

    public void OnStageSelectionAllowed(int stageIndex)
    {
        if (!this.closing)
        {
            if (stageIndex >= 0 && stageIndex < MainScript.config.stages.Length)
            {
                if (this.selectSound != null) MainScript.PlaySound(this.selectSound);
                this.SetHoverIndex(stageIndex);

                MainScript.config.selectedStage = MainScript.config.stages[stageIndex];
                this.StartLoadingBattleScreen();
            }
            else if (stageIndex < 0)
            {
                if (MainScript.config.selectedStage != null)
                {
                    if (this.cancelSound != null) MainScript.PlaySound(this.cancelSound);
                    MainScript.config.selectedStage = null;
                }
                else
                {
                    if (this.cancelSound != null) MainScript.PlaySound(this.cancelSound);
                    this.StartLoadingCharacterSelectionScreen();
                }
            }
        }
    }

    public void TryDeselectStage()
    {
        this.TrySelectStage(-1);
    }

    public void TrySelectStage()
    {
        this.TrySelectStage(this.stageHoverIndex);
    }

    public void TrySelectStage(int stageIndex)
    {
        // Check if he was playing online or not...
        if (!MainScript.isConnected)
        {
            // If it's a local game, update the corresponding stage immediately...
            this.OnStageSelectionAllowed(stageIndex);
        }
        else
        {
            // If it's an online game, we only select the stage if it has been requested by Player 1...
            // But if player 2 wants to come back to character selection screen, we also allow that...
            int localPlayer = MainScript.GetLocalPlayer();
            if (localPlayer == 1 || stageIndex < 0)
            {
                // We don't invoke the OnstageSelected() method immediately because we are using the frame-delay 
                // algorithm to keep players synchronized, so we can't invoke the OnstageSelected() method
                // until the other player has received the message with our choice.
                MainScript.fluxCapacitor.RequestOptionSelection(localPlayer, (sbyte) stageIndex);
            }
        }
    }

    #endregion

    #region public override methods

    public override void OnShow()
    {
        MainScript.config.selectedStage = null;
        this.closing = false;
    }

    public override void SelectOption(int option, int player)
    {
        this.OnStageSelectionAllowed(option);
    }

    #endregion

    #region protected instance method

    protected virtual void StartLoadingCharacterSelectionScreen()
    {
        this.closing = true;
        MainScript.StartCharacterSelectionScreen();
    }

    protected virtual void StartLoadingBattleScreen()
    {
        this.closing = true;
        if (this.fadeBeforeGoingToLoadingBattleScreen)
        {
            MainScript.StartLoadingBattleScreen();
        }
        else
        {
            MainScript.StartLoadingBattleScreen(0f);
        }
    }

    #endregion
}