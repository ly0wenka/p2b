using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Black Robot")]
    public int _blackRobotLeftPunchDamage = 5;
    public int _blackRobotRightPunchDamage = 7;
    public int _blackRobotLowKickDamage = 3;
    public int _blackRobotHighKickDamage = 9;
    [Header("Blue Robot")]
    public int _blueRobotLeftPunchDamage = 5;
    public int _blueRobotRightPunchDamage = 7;
    public int _blueRobotLowKickDamage = 3;
    public int _blueRobotHighKickDamage = 9;
    [Header("Brown Robot")]
    public int _brownRobotLeftPunchDamage = 5;
    public int _brownRobotRightPunchDamage = 7;
    public int _brownRobotLowKickDamage = 3;
    public int _brownRobotHighKickDamage = 9;
    [Header("Gold Robot")]
    public int _goldRobotLeftPunchDamage = 5;
    public int _goldRobotRightPunchDamage = 7;
    public int _goldRobotLowKickDamage = 3;
    public int _goldRobotHighKickDamage = 9;
    [Header("Green Robot")]
    public int _greenRobotLeftPunchDamage = 5;
    public int _greenRobotRightPunchDamage = 7;
    public int _greenRobotLowKickDamage = 3;
    public int _greenRobotHighKickDamage = 9;
    [Header("Pink Robot")]
    public int _pinkRobotLeftPunchDamage = 5;
    public int _pinkRobotRightPunchDamage = 7;
    public int _pinkRobotLowKickDamage = 3;
    public int _pinkRobotHighKickDamage = 9;
    [Header("Red Robot")]
    public int _redRobotLeftPunchDamage = 5;
    public int _redRobotRightPunchDamage = 7;
    public int _redRobotLowKickDamage = 3;
    public int _redRobotHighKickDamage = 9;
    [Header("White Robot")]
    public int _whiteRobotLeftPunchDamage = 5;
    public int _whiteRobotRightPunchDamage = 7;
    public int _whiteRobotLowKickDamage = 3;
    public int _whiteRobotHighKickDamage = 9;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
