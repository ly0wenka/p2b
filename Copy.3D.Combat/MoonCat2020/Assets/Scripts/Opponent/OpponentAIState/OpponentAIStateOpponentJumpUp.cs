using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentJumpUp : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentJumpUp();
        }

        private void OpponentJumpUp()
        {
            Debug.Log(nameof(OpponentJumpUp));

            OpponentJumpAnimation();
            _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
            _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
            _opponentMoveDirection *= _opponentJumpSpeed;

            _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

            if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
            {
                _opponentAIState = new OpponentAIStateOpponentComeDown();
            }
        }
    }
}