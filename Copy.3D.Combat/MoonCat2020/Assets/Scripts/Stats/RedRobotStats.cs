using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _redRobotLeftPunchDamage;
        _rightPunchDamage = _redRobotRightPunchDamage;
        _lowKickDamage = _redRobotLowKickDamage;
        _highKickDamage = _redRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
