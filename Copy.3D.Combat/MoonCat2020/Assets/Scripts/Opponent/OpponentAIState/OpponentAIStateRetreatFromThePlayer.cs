using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateRetreatFromThePlayer : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            RetreatFromThePlayer();
        }

        private void RetreatFromThePlayer()
        {
            // Debug.Log(nameof(RetreatFromThePlayer));

            _decideForwardMovement = Random.Range(1, 10);

            if (_decideBackwardMovement >= _minimumDecideValue
                && _decideForwardMovement <= _tippingPointDecideValue)
            {
            }

            if (_decideBackwardMovement <= _maximumDecideValue
                && _decideForwardMovement > _tippingPointDecideValue)
            {
            }
        }
    }
}