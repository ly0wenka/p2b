using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OpponentLowKick : MonoBehaviour
{
    public static Vector3 _playerOneImpactPoint;
    
    public float _nextLowKickIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _bodyHitCollider;

    private bool _returnIfOpponentIsLowKick;

    void Start()
    {
        _playerOneImpactPoint = Vector3.zero;

        _bodyHitCollider = GetComponent<Collider>();
        _bodyHitCollider.enabled = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider _playerOneBodyHit)
    {
        if (_playerOneBodyHit.CompareTag("BodyHit")
            && Time.time >= _nextLowKickIsAllowed)
        {
            _nextLowKickIsAllowed = Time.time + _attackDelay;
        }
    }
}
