using System;
using System.Collections;
using System.Collections.Generic;
using Opponent.OpponentAIState;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerPunchLeft : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;

    public float _nextLeftPunchIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfPlayerIsPunchingLeft;

    private int _leftPunchDamageValue;
    // Start is called before the first frame update
    void Start()
    {
        _opponentImpactPoint = Vector3.zero;

        _headHitCollider = GetComponent<Collider>();
        _headHitCollider.enabled = false;

        LeftPunchDamageSetup();
    }

    // Update is called once per frame
    void Update()
    {
        _returnIfPlayerIsPunchingLeft = PlayerOneMovement._playerIsPunchingLeft;
        
        _headHitCollider.enabled = _returnIfPlayerIsPunchingLeft;
    }

    private void OnTriggerStay(Collider _opponentHeadHit)
    {
        if (!_returnIfPlayerIsPunchingLeft)
        {
            return;
        }
        
        if (_opponentHeadHit.CompareTag(("HeadHit")) && Time.time >= _nextLeftPunchIsAllowed)
        {
            HeadStruck();

            _nextLeftPunchIsAllowed = Time.time + _attackDelay;
        }

        _opponentHeadHit.ClosestPointOnBounds(transform.position);

        _opponentImpactPoint = _opponentHeadHit.transform.position;
    }

    private void HeadStruck()
    {
        OpponentAI._opponentAIState = new OpponentAIStateOpponentHitByLeftPunch();

        OpponentHealth _tempDamage = FightCamera._opponent.GetComponent<OpponentHealth>();

        if (ChooseCharacterManager._robotBlack)
        {
            _tempDamage.OpponentLeftPunchDamage(BlackRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotBlue)
        {
            _tempDamage.OpponentLeftPunchDamage(BlueRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotBrown)
        {
            _tempDamage.OpponentLeftPunchDamage(BrownRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotGold)
        {
            _tempDamage.OpponentLeftPunchDamage(GoldRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotGreen)
        {
            _tempDamage.OpponentLeftPunchDamage(GreenRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotPink)
        {
            _tempDamage.OpponentLeftPunchDamage(PinkRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotRed)
        {
            _tempDamage.OpponentLeftPunchDamage(RedRobotStats._leftPunchDamage);
        }

        if (ChooseCharacterManager._robotWhite)
        {
            _tempDamage.OpponentLeftPunchDamage(WhiteRobotStats._leftPunchDamage);
        }
    }

    private void LeftPunchDamageSetup()
    {
        Debug.Log(nameof(LeftPunchDamageSetup));

        if (ChooseCharacterManager._robotBlack)
        {
            _leftPunchDamageValue = BlackRobotStats._leftPunchDamage;
        }
    }
}
