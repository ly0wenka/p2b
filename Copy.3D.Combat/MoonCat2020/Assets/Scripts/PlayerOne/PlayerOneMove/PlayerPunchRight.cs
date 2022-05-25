using System;
using System.Collections;
using System.Collections.Generic;
using Opponent.OpponentAIState;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class PlayerPunchRight : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;

    public float _nextRightPunchIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfPlayerIsPunchingRight;
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
        _returnIfPlayerIsPunchingRight = PlayerOneMovement._playerIsPunchingRight;

        if (_returnIfPlayerIsPunchingRight)
        {
            _headHitCollider.enabled = true;
        }

        if (!_returnIfPlayerIsPunchingRight)
        {
            _headHitCollider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider _opponentHeadHit)
    {
        if (!_returnIfPlayerIsPunchingRight)
        {
            return;
        }
        
        if (_opponentHeadHit.CompareTag(("HeadHit")) && Time.time >= _nextRightPunchIsAllowed)
        {
            HeadStruck();
            
            _nextRightPunchIsAllowed = Time.time + _attackDelay;
        }

        _opponentHeadHit.ClosestPointOnBounds(transform.position);

        _opponentImpactPoint = _opponentHeadHit.transform.position;
    }

    private void HeadStruck()
    {
        OpponentAI._opponentAIState = new OpponentAIStateOpponentHitByRightPunch();

        OpponentHealth _tempDamage = FightCamera._opponent.GetComponent<OpponentHealth>();

        if (ChooseCharacterManager._robotBlack)
        {
            _tempDamage.OpponentRightPunchDamage(BlackRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotBlue)
        {
            _tempDamage.OpponentRightPunchDamage(BlueRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotBrown)
        {
            _tempDamage.OpponentRightPunchDamage(BrownRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotGold)
        {
            _tempDamage.OpponentRightPunchDamage(GoldRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotGreen)
        {
            _tempDamage.OpponentRightPunchDamage(GreenRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotPink)
        {
            _tempDamage.OpponentRightPunchDamage(PinkRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotRed)
        {
            _tempDamage.OpponentRightPunchDamage(RedRobotStats._rightPunchDamage);
        }

        if (ChooseCharacterManager._robotWhite)
        {
            _tempDamage.OpponentRightPunchDamage(WhiteRobotStats._rightPunchDamage);
        }
    }
}
