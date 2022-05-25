using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateJumpTowardsThePlayer : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            throw new System.NotImplementedException();
        }

        private void JumpTowardsThePlayer()
        {
            Debug.Log(nameof(JumpTowardsThePlayer));

            OpponentJumpAnimation();

            _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
            _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
            _opponentMoveDirection *= _opponentJumpSpeed;

            _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

            if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
            {
                _opponentAIState = new OpponentAIStateOpponentComeDownForward();
            }
        }
    }
}