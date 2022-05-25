using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateWalkAwayFromThePlayer : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            WalkAwayFromThePlayer();
        }

        private void WalkAwayFromThePlayer()
        {
            // Debug.Log(nameof(WalkAwayFromThePlayer));

            _opponentMoveDirection =
                (transform.position - _playersPosition)
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