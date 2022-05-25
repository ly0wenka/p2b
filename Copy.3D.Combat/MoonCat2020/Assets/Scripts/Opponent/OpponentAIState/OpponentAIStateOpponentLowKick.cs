namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentLowKick : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentLowKick();
        }

        private void OpponentLowKick()
        {
            // Debug.Log(nameof(OpponentLowKick));
            _opponentIsKickingLow = true;
            OpponentKickLowAnimation();
            _opponentAIState = new OpponentAIStateWaitForStrikeAnimations();
        }
    }
}