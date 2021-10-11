using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _pinkRobotLeftPunchDamage;
        _rightPunchDamage = _pinkRobotRightPunchDamage;
        _lowKickDamage = _pinkRobotLowKickDamage;
        _highKickDamage = _pinkRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
