namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentLeftPunch : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentLeftPunch();
        }
        
        private void OpponentLeftPunch()
        {
            // Debug.Log(nameof(OpponentLeftPunch));
            _ooponentIsPunchingLeft = true;
            OpponentPunchLeftAnimation();
            _opponentAIState = new OpponentAIStateWaitForStrikeAnimations();
        }
    }
}