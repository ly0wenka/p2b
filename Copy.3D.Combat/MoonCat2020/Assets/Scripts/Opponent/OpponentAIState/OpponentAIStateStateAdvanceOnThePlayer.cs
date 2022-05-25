using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateStateAdvanceOnThePlayer : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            _decideForwardMovement = Random.Range(1, 10);

            if (_decideBackwardMovement >= _minimumDecideValue
                && _decideForwardMovement <= _tippingPointDecideValue)
            {
                _opponentAIState = new OpponentAIStateWalkTowardsThePlayer();
            }

            if (_decideBackwardMovement <= _maximumDecideValue
                && _decideForwardMovement > _tippingPointDecideValue)
            {
                if (_positionDifference.x >= _positionDifferenceModifier)
                {
                    _opponentAIState = new OpponentAIStateJumpTowardsThePlayer();
                }
                else
                {
                    _opponentAIState = new OpponentAIStateWalkTowardsThePlayer(); 
                }
            }
        }
    }
}