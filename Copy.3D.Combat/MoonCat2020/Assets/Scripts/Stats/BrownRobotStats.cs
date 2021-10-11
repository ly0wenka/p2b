using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _brownRobotLeftPunchDamage;
        _rightPunchDamage = _brownRobotRightPunchDamage;
        _lowKickDamage = _brownRobotLowKickDamage;
        _highKickDamage = _brownRobotHighKickDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
