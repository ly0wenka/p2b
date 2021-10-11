using System;
using System.Collections;
using System.Collections.Generic;
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
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitByLeftPunch;
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
