using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentHitByLeftPunch : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentHitByLeftPunch();
        }

        private void OpponentHitByLeftPunch()
        {
            // Debug.Log(nameof(OpponentHitByLeftPunch));
            //OpponentIsMovesInit();
            OpponentHitHeadAnimation();

            _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

            var _impactPoint = PlayerPunchLeft._opponentImpactPoint;

            var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
                Quaternion.identity) as GameObject;

            _opponentAIState = new OpponentAIStateWaitForHitAnimations();
        }
    }
}