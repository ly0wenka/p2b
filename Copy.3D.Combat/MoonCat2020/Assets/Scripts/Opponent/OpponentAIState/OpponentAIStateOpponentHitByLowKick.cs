using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentHitByLowKick : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            // Debug.Log(nameof(OpponentHitByLowKick));
            //OpponentIsMovesInit();
            OpponentHitBodyAnimation();

            _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

            var _impactPoint = PlayerLowKick._opponentImpactPoint;

            var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
                Quaternion.identity) as GameObject;

            _opponentAIState = new OpponentAIStateWaitForHitAnimations();
        }
    }
}