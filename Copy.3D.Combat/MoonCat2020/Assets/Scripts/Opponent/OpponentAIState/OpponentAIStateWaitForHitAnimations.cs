namespace Opponent.OpponentAIState
{
    public class OpponentAIStateWaitForHitAnimations : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            WaitForHitAnimations();
        }
        
        private void WaitForHitAnimations()
        {
            // Debug.Log(nameof(WaitForHitAnimations));
            OpponentIsMovesInit();

            if (_opponentAnimator.IsPlaying(_opponentHitBodyAnim.name))
            {
                return;
            }

            if (_opponentAnimator.IsPlaying(_opponentHitHeadAnim.name))
            {
                return;
            }

            _opponentAIState = new OpponentAIStateOpponentIdle();
        }
    }
}