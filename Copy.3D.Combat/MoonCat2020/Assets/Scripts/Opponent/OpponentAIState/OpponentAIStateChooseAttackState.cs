using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateChooseAttackState : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
             // Debug.Log(nameof(ChooseAttackState));
                    
                    OpponentIdleAnimation();
                    
                    _switchAttackValue = Random.Range(0, 7);
            
                    if (_switchAttackValue >= _leftPunchRangeMin 
                        && _switchAttackValue <= _leftPunchRangeMax)
                    {
                        _switchAttackStateValue = 0;
                    }
            
                    if (_switchAttackValue >= _rightPunchRangeMin 
                        && _switchAttackValue <= _rightPunchRangeMax)
                    {
                        _switchAttackStateValue = 1;
                    }
            
                    if (_switchAttackValue >= _lowKickRangeMin 
                        && _switchAttackValue <= _lowKickRangeMax)
                    {
                        _switchAttackStateValue = 2;
                    }
            
                    if (_switchAttackValue >= _highKickRangeMin 
                        && _switchAttackValue <= _highKickRangeMax)
                    {
                        _switchAttackStateValue = 3;
                    }
            
                    switch (_switchAttackStateValue)
                    {
                        case 0:
                            _chooseAttack = ChooseAttack.LeftPunch;
                            break;
                        case 1:
                            _chooseAttack = ChooseAttack.RightPunch;
                            break;
                        case 2:
                            _chooseAttack = ChooseAttack.LowKick;
                            break;
                        case 3:
                            _chooseAttack = ChooseAttack.HighKick;
                            break;
                    }
            
                    if (_chooseAttack == ChooseAttack.LeftPunch)
                    {
                        _opponentAIState = new OpponentAIStateOpponentLeftPunch();
                    }
            
                    if (_chooseAttack == ChooseAttack.RightPunch)
                    {
                        _opponentAIState = new OpponentAIStateOpponentRightPunch();
                    }
            
                    if (_chooseAttack == ChooseAttack.LowKick)
                    {
                        _opponentAIState = new OpponentAIStateOpponentLowKick();
                    }
            
                    if (_chooseAttack == ChooseAttack.HighKick)
                    {
                        _opponentAIState = new OpponentAIStateOpponentHighKick();
                    }
        }
    }
}