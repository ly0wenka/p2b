﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class FluxPlayer{
    #region public instance properties
    public FluxPlayerInputBuffer inputBuffer{
        get{
            return this._inputBuffer;
        }
    }

    public CombatController inputController{
        get{
            return MainScript.GetController(this.player);
        }
    }

    public bool isLocalPlayer{
        get{
//			int localPlayer = MainScript.GetLocalPlayer();
//			return localPlayer <= 0 && localPlayer == this.player;

            return !this.isRemotePlayer;
        }
    }

    public bool isRemotePlayer{
        get{
            return this.player == MainScript.GetRemotePlayer();
        }
    }

    public ControlsScript controlsScript{
        get{
            return MainScript.GetControlsScript(this.player);
        }
    }

    public int player{
        get{
            return this._player;
        }
    }
    #endregion

    #region private instance properties
    public FluxPlayerInputBuffer _inputBuffer = new FluxPlayerInputBuffer();
    public int _player;
    #endregion

    #region public instance constructors
    public FluxPlayer(int player) : this(player, 0){}

    public FluxPlayer(int player, int currentFrame) : this(player, currentFrame, -1){}

    public FluxPlayer(int player, int currentFrame, int maxBufferSize){
        this._player = player;
        this.Initialize(currentFrame, maxBufferSize);
    }
    #endregion

    #region public instance methods
    public virtual void Initialize(){
        this.Initialize(0);
    }

    public virtual void Initialize(long currentFrame){
        this.Initialize(currentFrame, -1);
    }

    public virtual void Initialize(long currentFrame, int maxBufferSize){
        this._inputBuffer.Initialize(currentFrame, maxBufferSize);
    }

    public bool RemoveNextInput(){
        return this._inputBuffer.RemoveNextInput();
    }
    #endregion
}