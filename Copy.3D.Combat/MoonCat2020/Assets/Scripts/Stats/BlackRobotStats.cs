using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRobotStats : Stats
{
    public static int _leftPunchDamage;
    public static int _rightPunchDamage;
    public static int _lowKickDamage;
    public static int _highKickDamage;

    private delegate void RefreshStatsHandler();
    private event RefreshStatsHandler OnRefreshStats;
    // Start is called before the first frame update
    void Start()
    {
        _leftPunchDamage = _blackRobotLeftPunchDamage;
        _rightPunchDamage = _blackRobotRightPunchDamage;
        _lowKickDamage = _blackRobotLowKickDamage;
        _highKickDamage = _blackRobotHighKickDamage;
    }

    private void OnEnable()
    {
        OnRefreshStats += RefreshStats;
    }

    private void OnDisable()
    {
        OnRefreshStats -= RefreshStats;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown("space"))
        {
            OnRefreshStats?.Invoke();
        }

        Debug.Log(_leftPunchDamage);
#endif
    }

    private void RefreshStats()
    {
        Debug.Log(nameof(RefreshStats));
        
        _leftPunchDamage = _blackRobotLeftPunchDamage;
        _rightPunchDamage = _blackRobotRightPunchDamage;
        _lowKickDamage = _blackRobotLowKickDamage;
        _highKickDamage = _blackRobotHighKickDamage;
    }
}
