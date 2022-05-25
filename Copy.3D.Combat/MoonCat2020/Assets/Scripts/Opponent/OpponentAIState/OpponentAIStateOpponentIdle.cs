namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentIdle : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            // Debug.Log(nameof(OpponentIdle));
            OpponentIsMovesInit();
            // if (OpponentIsGrounded())
            // {
            //     return;
            // }

            OpponentIdleAnimation();

            OpponentGravityIdle();

            _returnFightIntroFinished = FightIntro._fightIntroFinished;

            if (_returnFightIntroFinished != true)
            {
                return;
            }

            // Delete below for testing only
            _opponentAIState = new OpponentAIStateWalkTowardsThePlayer();

            if (_decideAggressionPriority < _assessPriority.start)
            {
                _opponentAIState = new OpponentAIStateAdvanceOnThePlayer();
            }

            if (_decideAggressionPriority > _offensivePriority.end
                && _decideAggressionPriority < _defensivePriority.start)
            {
                if (_assessingThePlayer)
                {
                    return;
                }

                _assessingThePlayer = true;
                StartCoroutine(AssessThePlayer());
            }

            if (_decideAggressionPriority == _defensivePriority.start)
            {
                _opponentAIState = new OpponentAIStateRetreatFromThePlayer();
            }
        }
    }
}