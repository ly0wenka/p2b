using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentComeDown : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentComeDown();
        }

        private void OpponentComeDown()
        {
            Debug.Log(nameof(OpponentComeDown));

            //OpponentJumpAnimation();
            _opponentMoveDirection = new Vector3(0, _opponentVerticalSpeed, 0);
            _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection);
            _opponentMoveDirection *= _opponentJumpSpeed;

            _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

            // SetIdleToState();
            if (OpponentIsGrounded())
            {
                _opponentAIState = new OpponentAIStateOpponentIdle();
            }
        }
    }
}