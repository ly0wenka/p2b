using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _goldRobotLeftPunchDamage;
        _rightPunchDamage = _goldRobotRightPunchDamage;
        _lowKickDamage = _goldRobotLowKickDamage;
        _highKickDamage = _goldRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}