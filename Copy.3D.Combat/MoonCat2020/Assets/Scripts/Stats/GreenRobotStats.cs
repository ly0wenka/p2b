using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _greenRobotLeftPunchDamage;
        _rightPunchDamage = _greenRobotRightPunchDamage;
        _lowKickDamage = _greenRobotLowKickDamage;
        _highKickDamage = _greenRobotHighKickDamage;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
