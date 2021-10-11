using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _blackRobotLeftPunchDamage;
        _rightPunchDamage = _blackRobotRightPunchDamage;
        _lowKickDamage = _blackRobotLowKickDamage;
        _highKickDamage = _blackRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
