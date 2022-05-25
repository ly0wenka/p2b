namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentRightPunch : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentRightPunch();
        }

        private void OpponentRightPunch()
        {
            // Debug.Log(nameof(OpponentRightPunch));
            _opponentIsPunchingRight = true;
            OpponentPunchRightAnimation();
            _opponentAIState = new OpponentAIStateWaitForStrikeAnimations();
        }
    }
}