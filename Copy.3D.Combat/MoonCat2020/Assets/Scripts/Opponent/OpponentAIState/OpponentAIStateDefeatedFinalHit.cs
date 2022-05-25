namespace Opponent.OpponentAIState
{
    public class OpponentAIStateDefeatedFinalHit : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentDefeated();
        }

        private void OpponentDefeated()
        {
            // Debug.Log(nameof(OpponentDefeated));

            OpponentGravityIdle();

            if (_opponentAnimator.IsPlaying(_opponentDefeatedFinalHitAnim.name))
            {
                return;
            }

            StopCoroutine(OpponentFSM());
        }
    }
}