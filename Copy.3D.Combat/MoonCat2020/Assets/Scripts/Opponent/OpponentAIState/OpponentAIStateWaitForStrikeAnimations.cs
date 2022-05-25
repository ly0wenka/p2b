namespace Opponent.OpponentAIState
{
    public class OpponentAIStateWaitForStrikeAnimations : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            WaitForStrikeAnimations();
        }

        private void WaitForStrikeAnimations()
        {
            // Debug.Log(nameof(WaitForStrikeAnimations));
            OpponentIsMovesInit();

            if (_opponentAnimator.IsPlaying(_opponentPunchLeftAnim.name))
            {
                return;
            }

            if (_opponentAnimator.IsPlaying(_opponentPunchRightAnim.name))
            {
                return;
            }

            if (_opponentAnimator.IsPlaying(_opponentKickLowAnim.name))
            {
                return;
            }

            if (_opponentAnimator.IsPlaying(_opponentKickHighAnim.name))
            {
                return;
            }
        
            _ooponentIsPunchingLeft = false;
            _opponentIsPunchingRight = false;
            _opponentIsKickingLow = false;
            _ooponentIsKickingHigh = false;
        
            _opponentAIState = new OpponentAIStateOpponentIdle();
        }
    }
}