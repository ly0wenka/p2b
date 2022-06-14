using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class PlayerHighKick : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;

    public float _nextHighKickIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfPlayerIsHighKicking;
    // Start is called before the first frame update
    void Start()
    {
        _opponentImpactPoint = Vector3.zero;

        _headHitCollider = GetComponent<Collider>();
        _headHitCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _returnIfPlayerIsHighKicking = PlayerOneMovement._playerIsKickingHigh;
        gameObject.GetComponentInChildren<PlayerHighKick>().enabled = !_returnIfPlayerIsHighKicking;
        _headHitCollider.enabled = _returnIfPlayerIsHighKicking;
    }

    private void OnTriggerStay(Collider _opponentHeadKick)
    {
        if (!_returnIfPlayerIsHighKicking)
        {
            return;
        }
        if (_opponentHeadKick.CompareTag(("OpponentHeadHit")) && Time.time >= _nextHighKickIsAllowed)
        {
            HeadKick();
            
            _nextHighKickIsAllowed = Time.time + _attackDelay;
        }

        _opponentHeadKick.ClosestPointOnBounds(transform.position);

        _opponentImpactPoint = _opponentHeadKick.transform.position;
    }

    private void HeadKick()
    {
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitByHighKick;

        OpponentHealth _tempDamage = FightCamera._opponent.GetComponent<OpponentHealth>();

        if (ChooseCharacterManager._robotBlack)
        {
            _tempDamage.OpponentHighKickDamage(BlackRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotBlue)
        {
            _tempDamage.OpponentHighKickDamage(BlueRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotBrown)
        {
            _tempDamage.OpponentHighKickDamage(BrownRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotGold)
        {
            _tempDamage.OpponentHighKickDamage(GoldRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotGreen)
        {
            _tempDamage.OpponentHighKickDamage(GreenRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotPink)
        {
            _tempDamage.OpponentHighKickDamage(PinkRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotRed)
        {
            _tempDamage.OpponentHighKickDamage(RedRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotWhite)
        {
            _tempDamage.OpponentHighKickDamage(WhiteRobotStats._highKickDamage);
        }
    }
}
