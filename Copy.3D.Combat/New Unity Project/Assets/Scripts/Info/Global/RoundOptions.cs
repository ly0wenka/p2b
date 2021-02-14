using FPLibrary;
using UnityEngine;

[System.Serializable]
public class RoundOptions
{
    public int totalRounds = 3;
    public bool hasTimer = true;
    public float timer = 99;
    public Fix64 _timer = 99;
    public float timerSpeed = 100;
    public Fix64 _timerSpeed = 100;
    public float p1XPosition = -5;
    public Fix64 _p1XPosition = -5;
    public float p2XPosition = 5;
    public Fix64 _p2XPosition = 5;
    public float endGameDelay = 4;
    public Fix64 _endGameDelay = 4;
    public float newRoundDelay = 1;
    public Fix64 _newRoundDelay = 1;
    public float slowMoTimer = 3;
    public Fix64 _slowMoTimer = 3;
    public float slowMoSpeed = .2f;
    public Fix64 _slowMoSpeed = .2;
    public AudioClip victoryMusic;
    public bool resetLifePoints = true;
    public bool resetPositions = true;
    public bool allowMovementStart = true;
    public bool allowMovementEnd = true;
    public bool inhibitGaugeGain = true;
    public bool rotateBodyKO = true;
    public bool slowMotionKO = true;
    public bool cameraZoomKO = true;
    public bool freezeCamAfterOutro = true;
}