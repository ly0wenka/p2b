using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _blueRobotLeftPunchDamage;
        _rightPunchDamage = _blueRobotRightPunchDamage;
        _lowKickDamage = _blueRobotLowKickDamage;
        _highKickDamage = _blueRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
