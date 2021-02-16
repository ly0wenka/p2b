using System.Collections.Generic;
using FPLibrary;

public struct GlobalState
{
    // UFE
    public bool freeCamera;
    public bool freezePhysics;
    public bool newRoundCasted;
    public bool normalizedCam;
    public bool pauseTimer;
    public Fix64 timer;
    public List<DelayedAction> delayedActions;
    public List<InstantiatedGameObjectState> instantiatedObjects;

    // GlobalInfo
    public int currentRound;
    public bool lockInputs;
    public bool lockMovements;
    public Fix64 timeScale;
}