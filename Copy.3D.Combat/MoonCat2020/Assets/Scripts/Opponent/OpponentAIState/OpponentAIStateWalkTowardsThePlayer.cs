using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateWalkTowardsThePlayer : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            WalkTowardsThePlayer();
        }

        private void WalkTowardsThePlayer()
        {
            // Debug.Log(nameof(WalkTowardsThePlayer));

            if (Mathf.Abs(_playerOne.transform.position.x - _opponent.transform.position.x) <= _chooseAttackDistanceModifier)
            {
                _opponentAIState = new OpponentAIStateChooseAttackState();
            }

            _opponentMoveDirection =
                (_playersPosition - transform.position)
                * _opponentWalkSpeed;

            _opponentMoveDirection.Normalize();

            _opponentMoveDirection.y = 0;
            _opponentMoveDirection.z = 0;

            _collisionFlagsOpponent =
                _opponentController.Move(
                    _opponentMoveDirection * Time.deltaTime);

            OpponentWalkAnimation();
        }
    }
}