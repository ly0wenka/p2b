using System.Collections;
using System.Collections.Generic;
using Opponent.OpponentAIState;
using UnityEngine;

public class OpponentAIStateStateInitialise : OpponentAI, IOpponentAIState
{
    public void Do()
    {
        // Debug.Log(nameof(Initialise));

        _decideAggressionPriority = 2; //Random.Range(1, 9);

        if (_punchKickPivotValue == 0)
        {
            _punchKickPivotValue = 3;
        }

        if (_leftPunchRangeMin != 0)
        {
            _leftPunchRangeMin = 0;
        }

        if (_leftPunchRangeMax == 0)
        {
            _leftPunchRangeMax = 1;
        }

        if (_rightPunchRangeMin == 0)
        {
            _rightPunchRangeMin = 2;
        }

        if (_rightPunchRangeMax == 0)
        {
            _rightPunchRangeMax = 3;
        }

        if (_lowKickRangeMin == 0)
        {
            _lowKickRangeMin = 4;
        }

        if (_lowKickRangeMax == 0)
        {
            _lowKickRangeMax = 5;
        }

        if (_highKickRangeMin == 0)
        {
            _highKickRangeMin = 6;
        }

        if (_highKickRangeMax == 0)
        {
            _highKickRangeMax = 7;
        }

        _opponentAIState = new OpponentAIStateOpponentIdle();
    }
}
