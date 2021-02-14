using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleGUI : CombatScreen
{
    #region public class definitions

    [Serializable]
    public class PlayerInfo
    {
        public CharacterInfo character;
        public float targetLife;
        public float totalLife;
        public int wonRounds;
        public bool winner;
    }

    #endregion

    #region protected instance properties

    protected PlayerInfo player1 = new PlayerInfo();
    protected PlayerInfo player2 = new PlayerInfo();
    protected bool isRunning;

    #endregion

    //GameObject leftZoneRegular;
    //GameObject rightZoneRegular;
    //GameObject leftZoneMirror;
    //GameObject rightZoneMirror;
    //int trycount;


    #region public override methods

    public override void DoFixedUpdate(
        IDictionary<InputReferences, InputEvents> player1PreviousInputs,
        IDictionary<InputReferences, InputEvents> player1CurrentInputs,
        IDictionary<InputReferences, InputEvents> player2PreviousInputs,
        IDictionary<InputReferences, InputEvents> player2CurrentInputs
    )
    {
        /*if (leftZoneRegular == null && MainScript.controlFreakPrefab != null && trycount < 10) {
                //leftZoneRegular = GameObject.Find("/CF2 Swipe(Clone)/CF2-Canvas/CF2-Panel/TouchZone-Left-Tap");
                leftZoneRegular = GameObject.Find("/CF2-Panel/TouchZone-Left-Tap");
                rightZoneRegular = GameObject.Find("/CF2-Panel/TouchZone-Right-Tap");
                leftZoneMirror = GameObject.Find("/CF2-Panel/TouchZone-Left-Tap-Mirror");
                rightZoneMirror = GameObject.Find("/CF2-Panel/TouchZone-Right-Tap-Mirror");
                Debug.Log(leftZoneRegular);

                leftZoneRegular.SetActive(true);
                rightZoneRegular.SetActive(true);
                leftZoneMirror.SetActive(false);
                rightZoneMirror.SetActive(false);
                trycount ++;
            }
        }*/
        base.DoFixedUpdate(player1PreviousInputs, player1CurrentInputs, player2PreviousInputs, player2CurrentInputs);
    }

    public override void OnShow()
    {
        base.OnShow();

        /* Subscribe to UFE events:
        /* Possible Events:
         * OnLifePointsChange(float newLifePoints, CharacterInfo player)
         * OnNewAlert(string alertMessage, CharacterInfo player)
         * OnHit(MoveInfo move, CharacterInfo hitter)
         * OnMove(MoveInfo move, CharacterInfo player)
         * OnRoundEnds(CharacterInfo winner, CharacterInfo loser)
         * OnRoundBegins(int roundNumber)
         * OnGameEnds(CharacterInfo winner, CharacterInfo loser)
         * OnGameBegins(CharacterInfo player1, CharacterInfo player2, StageOptions stage)
         * 
         * usage:
         * MainScript.OnMove += YourFunctionHere;
         * .
         * .
         * void YourFunctionHere(T param1, T param2){...}
         * 
         * The following code bellow show more usage examples
         */

        // Global Events
        MainScript.OnGameBegin += this.OnGameBegin;
        MainScript.OnGameEnds += this.OnGameEnd;
        MainScript.OnGamePaused += this.OnGamePaused;
        MainScript.OnRoundBegins += this.OnRoundBegin;
        MainScript.OnRoundEnds += this.OnRoundEnd;
        MainScript.OnLifePointsChange += this.OnLifePointsChange;
        MainScript.OnNewAlert += this.OnNewAlert;
        MainScript.OnHit += this.OnHit;
        MainScript.OnBlock += this.OnBlock;
        MainScript.OnParry += this.OnParry;
        MainScript.OnMove += this.OnMove;
        MainScript.OnBasicMove += this.OnBasicMove;
        MainScript.OnButton += this.OnButtonPress;
        MainScript.OnTimer += this.OnTimer;
        MainScript.OnTimeOver += this.OnTimeOver;
        MainScript.OnInput += this.OnInput;

        // Move Events
        MainScript.OnBodyVisibilityChange += this.OnBodyVisibilityChange;
        MainScript.OnParticleEffects += this.OnParticleEffects;
        MainScript.OnSideSwitch += this.OnSideSwitch;
    }

    public override void OnHide()
    {
        MainScript.OnGameBegin -= this.OnGameBegin;
        MainScript.OnGameEnds -= this.OnGameEnd;
        MainScript.OnGamePaused -= this.OnGamePaused;
        MainScript.OnRoundBegins -= this.OnRoundBegin;
        MainScript.OnRoundEnds -= this.OnRoundEnd;
        MainScript.OnLifePointsChange -= this.OnLifePointsChange;
        MainScript.OnNewAlert -= this.OnNewAlert;
        MainScript.OnHit -= this.OnHit;
        MainScript.OnBlock -= this.OnBlock;
        MainScript.OnParry -= this.OnParry;
        MainScript.OnMove -= this.OnMove;
        MainScript.OnBasicMove -= this.OnBasicMove;
        MainScript.OnButton -= this.OnButtonPress;
        MainScript.OnTimer -= this.OnTimer;
        MainScript.OnTimeOver -= this.OnTimeOver;
        MainScript.OnInput -= this.OnInput;

        MainScript.OnBodyVisibilityChange -= this.OnBodyVisibilityChange;
        MainScript.OnParticleEffects -= this.OnParticleEffects;
        MainScript.OnSideSwitch -= this.OnSideSwitch;

        base.OnHide();
    }

    #endregion

    #region protected instance methods

    protected virtual void OnGameBegin(CharacterInfo player1, CharacterInfo player2, StageOptions stage)
    {
        this.player1.character = player1;
        this.player1.targetLife = player1.lifePoints;
        this.player1.totalLife = player1.lifePoints;
        this.player1.wonRounds = 0;

        this.player2.character = player2;
        this.player2.targetLife = player2.lifePoints;
        this.player2.totalLife = player2.lifePoints;
        this.player2.wonRounds = 0;

        MainScript.PlayMusic(stage.music);
        this.isRunning = true;
    }

    protected virtual void OnGameEnd(CharacterInfo winner, CharacterInfo loser)
    {
        this.isRunning = false;
        if (winner == this.player1.character) this.player1.winner = true;
        if (winner == this.player2.character) this.player2.winner = true;

        MainScript.DelaySynchronizedAction(this.OpenMenuAfterBattle, 3.5);
    }

    protected void OpenMenuAfterBattle()
    {
        if (MainScript.gameMode == GameMode.VersusMode || MainScript.gameMode == GameMode.ChallengeMode ||
            MainScript.gameMode == GameMode.NetworkGame)
        {
            MainScript.StartVersusModeAfterBattleScreen();
        }
        else if (MainScript.gameMode == GameMode.StoryMode)
        {
            if (this.player1.winner)
            {
                MainScript.WonStoryModeBattle();
            }
            else
            {
                MainScript.StartStoryModeContinueScreen();
            }
        }
        else
        {
            MainScript.StartMainMenuScreen();
        }
    }

    protected virtual void OnGamePaused(bool isPaused)
    {
    }

    protected virtual void OnRoundBegin(int roundNumber)
    {
    }

    protected virtual void OnRoundEnd(CharacterInfo winner, CharacterInfo loser)
    {
        //++this.player1WonRounds;
        //++this.playe21WonRounds;
    }

    protected virtual void OnLifePointsChange(float newFloat, CharacterInfo player)
    {
        // You can use this to have your own custom events when a player's life points changes
        // player.playerNum = 1 or 2
    }

    protected virtual void OnNewAlert(string msg, CharacterInfo player)
    {
        // You can use this to have your own custom events when a new text alert is fired from the engine
        // player.playerNum = 1 or 2
    }

    protected virtual void OnHit(HitBox strokeHitBox, MoveInfo move, CharacterInfo player)
    {
        // player.playerNum = 1 or 2
        // You can use this to have your own custom events when a character gets hit
    }

    protected virtual void OnBlock(HitBox strokeHitBox, MoveInfo move, CharacterInfo player)
    {
        // You can use this to have your own custom events when a player blocks.
        // player.playerNum = 1 or 2
        // player = character blocking
    }

    protected virtual void OnParry(HitBox strokeHitBox, MoveInfo move, CharacterInfo player)
    {
        // You can use this to have your own custom events when a character parries an attack
        // player.playerNum = 1 or 2
        // player = character parrying
    }

    protected virtual void OnMove(MoveInfo move, CharacterInfo player)
    {
        // Fires when a player successfully executes a move
        // player.playerNum = 1 or 2
    }

    protected virtual void OnBasicMove(BasicMoveReference basicMove, CharacterInfo player)
    {
        // Fires when a player successfully executes a move
        // player.playerNum = 1 or 2
    }

    protected virtual void OnButtonPress(ButtonPress buttonPress, CharacterInfo player)
    {
        // Fires when a player successfully executes a move
        // player.playerNum = 1 or 2
    }

    protected virtual void OnBodyVisibilityChange(MoveInfo move, CharacterInfo player,
        BodyPartVisibilityChange bodyPartVisibilityChange, HitBox hitBox)
    {
        // Fires when a move casts a body part visibility change
        // player.playerNum = 1 or 2
    }

    protected virtual void OnParticleEffects(MoveInfo move, CharacterInfo player,
        MoveParticleEffect particleEffects)
    {
        // Fires when a move casts a particle effect
        // player.playerNum = 1 or 2
    }

    protected virtual void OnSideSwitch(int side, CharacterInfo player)
    {
        // Fires when a character switches orientation
        // player.playerNum = 1 or 2
        /*if (player.playerNum == 1) {
            leftZoneRegular.SetActive(false);
            rightZoneRegular.SetActive(false);
            leftZoneMirror.SetActive(false);
            rightZoneMirror.SetActive(false);

            if (side == -1) {
                leftZoneMirror.SetActive(true);
                rightZoneMirror.SetActive(true);
            } else {
                leftZoneRegular.SetActive(true);
                rightZoneRegular.SetActive(true);
            }
        }*/
    }

    protected virtual void OnTimer(FPLibrary.Fix64 time)
    {
    }

    protected virtual void OnTimeOver()
    {
    }

    protected virtual void OnInput(InputReferences[] inputReferences, int player)
    {
    }

    #endregion
}