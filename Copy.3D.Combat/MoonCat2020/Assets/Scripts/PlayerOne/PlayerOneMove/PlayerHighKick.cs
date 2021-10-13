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
            _tempDamage.OpponentRightPunchDamage(BlackRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotBlue)
        {
            _tempDamage.OpponentRightPunchDamage(BlueRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotBrown)
        {
            _tempDamage.OpponentRightPunchDamage(BrownRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotGold)
        {
            _tempDamage.OpponentRightPunchDamage(GoldRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotGreen)
        {
            _tempDamage.OpponentRightPunchDamage(GreenRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotPink)
        {
            _tempDamage.OpponentRightPunchDamage(PinkRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotRed)
        {
            _tempDamage.OpponentRightPunchDamage(RedRobotStats._highKickDamage);
        }

        if (ChooseCharacterManager._robotWhite)
        {
            _tempDamage.OpponentRightPunchDamage(WhiteRobotStats._highKickDamage);
        }
    }
}
