using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _whiteRobotLeftPunchDamage;
        _rightPunchDamage = _whiteRobotRightPunchDamage;
        _lowKickDamage = _whiteRobotLowKickDamage;
        _highKickDamage = _whiteRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
