using UnityEngine;

namespace Opponent.OpponentAIState
{
    public class OpponentAIStateOpponentHitByRightPunch : OpponentAI, IOpponentAIState
    {
        public void Do()
        {
            OpponentHitByRightPunch();
        }

        private void OpponentHitByRightPunch()
        {
            // Debug.Log(nameof(OpponentHitByRightPunch));
            //OpponentIsMovesInit();
            OpponentHitHeadAnimation();

            _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

            var _impactPoint = PlayerPunchRight._opponentImpactPoint;

            var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
                Quaternion.identity) as GameObject;

            _opponentAIState = new OpponentAIStateWaitForHitAnimations();
        }
    }
}