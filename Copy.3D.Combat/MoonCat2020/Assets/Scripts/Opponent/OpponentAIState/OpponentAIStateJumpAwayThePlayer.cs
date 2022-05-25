using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateJumpAwayThePlayer : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            JumpAwayThePlayer();
        }

        private void JumpAwayThePlayer()
        {
            Debug.Log(nameof(JumpAwayThePlayer));

            OpponentJumpAnimation();
            _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
            _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
            _opponentMoveDirection *= _opponentJumpSpeed;

            _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

            if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
            {
                _opponentAIState = new OpponentAIStateOpponentComeDownBackwards();
            }
        }
    }
}