using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentPunchLeft : MonoBehaviour
{
    public static Vector3 _playerOneImpactPoint;

    public float _nextLeftPunchIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfOpponentIsPunchLeft;

    void Start()
    {
        _playerOneImpactPoint = Vector3.zero;

        _headHitCollider = GetComponent<Collider>();
        _headHitCollider.enabled = false;
    }

    void Update()
    {
        _returnIfOpponentIsPunchLeft = OpponentAI._ooponentIsPunchingLeft;
    }

    private void OnTriggerStay(Collider _playerOneHeadHit)
    {
        if (_playerOneHeadHit.tag == "PlayerHeadHit" && Time.time >= _nextLeftPunchIsAllowed)
        {

            _nextLeftPunchIsAllowed = Time.time + _attackDelay;
        }

        _playerOneHeadHit.ClosestPointOnBounds(transform.position);

        _playerOneImpactPoint = _playerOneHeadHit.transform.position;
    }
}