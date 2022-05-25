using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentHitByHighKick : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentHitByHighKick();
        }

        private void OpponentHitByHighKick()
        {
            // Debug.Log(nameof(OpponentHitByHighKick));
            //OpponentIsMovesInit();
            OpponentHitBodyAnimation();

            _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

            var _impactPoint = PlayerHighKick._opponentImpactPoint;

            var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
                Quaternion.identity) as GameObject;

            _opponentAIState = new OpponentAIStateWaitForHitAnimations();
        }
    }
}