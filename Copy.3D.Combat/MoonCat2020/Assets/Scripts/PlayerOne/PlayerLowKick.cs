using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class PlayerLowKick : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;
    public static Vector3 _playerImpactPoint;

    public float _nextLowKickIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfPlayerIsLowKicking;
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
        _returnIfPlayerIsLowKicking = PlayerOneMovement._playerIsKickingLow;
        
        _headHitCollider.enabled = _returnIfPlayerIsLowKicking;
    }
    
    private void OnTriggerEnter(Collider _opponentHeadKick)
    {
        if (_opponentHeadKick.CompareTag(("HeadHit")) && Time.time >= _nextLowKickIsAllowed)
        {
            HeadKick();
            
            _nextLowKickIsAllowed = Time.time + _attackDelay;
        }

        _opponentHeadKick.ClosestPointOnBounds(transform.position);

        _opponentImpactPoint = _opponentHeadKick.transform.position;
    }

    private void HeadKick()
    {
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitByLowKick;
    }
}
