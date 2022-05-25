namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentHighKick : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentHighKick();
        }

        private void OpponentHighKick()
        {
            // Debug.Log(nameof(OpponentHighKick));
            _ooponentIsKickingHigh = true;
            OpponentHighKick();
            _opponentAIState = new OpponentAIStateWaitForStrikeAnimations();
        }
    }
}