using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentPunchRight : MonoBehaviour
{
    public static Vector3 _playerOneImpactPoint;
    
    public float _nextHighKickIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfOpponentIsRightPunch;
    void Start()
    {
        _playerOneImpactPoint = Vector3.zero;

        _headHitCollider = GetComponent<Collider>();
        _headHitCollider.enabled = false;
    }

    void Update()
    {
        _returnIfOpponentIsRightPunch = PlayerOneMovement._playerIsKickingHigh;

        _headHitCollider.enabled = _returnIfOpponentIsRightPunch;
    }
    
    private void OnTriggerStay(Collider _playerOneHeadKick)
    {
        if (_playerOneHeadKick.CompareTag(("HeadHit")) && Time.time >= _nextHighKickIsAllowed)
        {
            HeadKick();
            
            _nextHighKickIsAllowed = Time.time + _attackDelay;
        }

        _playerOneHeadKick.ClosestPointOnBounds(transform.position);

        _playerOneImpactPoint = _playerOneHeadKick.transform.position;
    }

    private void HeadKick()
    {
        PlayerOneMovement._playerOneStates = PlayerOneStates.PlayerHitByHighKick;
    }
}