using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class PlayerLowKick : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;

    public float _nextLowKickIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _bodyHitCollider;

    private bool _returnIfPlayerIsLowKicking;
    // Start is called before the first frame update
    void Start()
    {
        _opponentImpactPoint = Vector3.zero;

        _bodyHitCollider = GetComponent<Collider>();
        _bodyHitCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _returnIfPlayerIsLowKicking = PlayerOneMovement._playerIsKickingLow;
        gameObject.GetComponentInChildren<PlayerLowKick>().enabled = !_returnIfPlayerIsLowKicking;
        _bodyHitCollider.enabled = _returnIfPlayerIsLowKicking;
    }
    
    private void OnTriggerEnter(Collider _opponentHeadKick)
    {
        
        if (!_returnIfPlayerIsLowKicking)
        {
            return;
        }
        
        if (_opponentHeadKick.CompareTag(("OpponentHeadHit")) && Time.time >= _nextLowKickIsAllowed)
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

        OpponentHealth _tempDamage = FightCamera._opponent.GetComponent<OpponentHealth>();

        if (ChooseCharacterManager._robotBlack)
        {
            _tempDamage.OpponentLowKickDamage(BlackRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotBlue)
        {
            _tempDamage.OpponentLowKickDamage(BlueRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotBrown)
        {
            _tempDamage.OpponentLowKickDamage(BrownRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotGold)
        {
            _tempDamage.OpponentLowKickDamage(GoldRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotGreen)
        {
            _tempDamage.OpponentLowKickDamage(GreenRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotPink)
        {
            _tempDamage.OpponentLowKickDamage(PinkRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotRed)
        {
            _tempDamage.OpponentLowKickDamage(RedRobotStats._lowKickDamage);
        }

        if (ChooseCharacterManager._robotWhite)
        {
            _tempDamage.OpponentLowKickDamage(WhiteRobotStats._lowKickDamage);
        }
    }
}
