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

    private bool _returnIfOpponentIsHighKicking;

    void Start()
    {
        _playerOneImpactPoint = Vector3.zero;

        _headHitCollider = GetComponent<Collider>();
        _headHitCollider.enabled = false;
    }

    void Update()
    {
    }

    private void OnTriggerStay(Collider _playerOneHeadHit)
    {
        if (_playerOneHeadHit.tag == "HeadHit" && Time.time >= _nextLeftPunchIsAllowed)
        {

            _nextLeftPunchIsAllowed = Time.time + _attackDelay;
        }

        _playerOneHeadHit.ClosestPointOnBounds(transform.position);

        _playerOneImpactPoint = _playerOneHeadHit.transform.position;
    }
}