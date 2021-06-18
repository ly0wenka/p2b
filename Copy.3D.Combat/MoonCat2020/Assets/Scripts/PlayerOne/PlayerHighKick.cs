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

        _headHitCollider.enabled = _returnIfPlayerIsHighKicking;
    }

    private void OnTriggerStay(Collider _opponentHeadKick)
    {
        if (_opponentHeadKick.CompareTag(("HeadHit")) && Time.time >= _nextHighKickIsAllowed)
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
    }
}
