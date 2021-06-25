using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class OpponentAI : MonoBehaviour
{
    private Transform _opponentTransform;
    private CharacterController _opponentController;

    private Animator _opponentAnimator;

    public AnimationClip _opponentIdleAnim;
    public AnimationClip _opponentHitBodyAnim;
    public AnimationClip _opponentHitHeadAnim;
    public AnimationClip _opponentDefeatedFinalHitAnim;

    private AudioSource _opponentAIAudioSource;
    public AudioClip _opponentHeadHitAudio;
    public AudioClip _opponentBodyHitAudio;

    public GameObject _hitSparks;

    private Vector3 _opponentMoveDirection = Vector3.zero;

    private float _opponentGravity = 5f;
    private float _opponentGravityModifier = 5f;
    private float _opponentVerticalSpeed = .0f;

    public RangeInt _offensivePriority = new RangeInt(1, 3);
    public RangeInt _assessPriority = new RangeInt(4, 6);
    public RangeInt _defensivePriority = new RangeInt(7, 9);

    private int _decideForwardMovement;

    private int _decideAggressionPriority;
    
    private bool _assessingThePlayer;
    public float _assessingTime = 3;
    private bool _returnFightIntroFinished;

    private CollisionFlags _collisionFlagsOpponent;

    public static OpponentAIState _opponentAIState;
    
    // Start is called before the first frame update
    void Start()
    {
        _opponentController = GetComponent<CharacterController>();

        _opponentAnimator = GetComponent<Animator>();

        _opponentAIAudioSource = GetComponent<AudioSource>();

        _opponentTransform = transform;
        
        _opponentMoveDirection = Vector3.zero;

        _decideAggressionPriority = 0;

        _assessingThePlayer = false;
        
        StartCoroutine(OpponentFSM());
    }

    // Update is called once per frame
    void Update()
    {
        ApplyOpponentGravity();
    }

    private IEnumerator OpponentFSM()
    {
        while (true)
        {
            switch (_opponentAIState)
            {
                case OpponentAIState.Initialise:
                    Initialise();
                    break;
                case OpponentAIState.OpponentIdle:
                    OpponentIdle();
                    break;
                case OpponentAIState.OpponentHitByLowKick:
                    OpponentHitByLowKick();
                    break;
                case OpponentAIState.OpponentHitByLeftPunch:
                    OpponentHitByLeftPunch();
                    break;
                case OpponentAIState.WaitForHitAnimations:
                    WaitForHitAnimations();
                    break;
                case OpponentAIState.DefeatedFinalHit:
                    OpponentDefeated();
                    break;
                case OpponentAIState.OpponentHitByRightPunch:
                    OpponentHitByRightPunch();
                    break;
                case OpponentAIState.OpponentHitByHighKick:
                    OpponentHitByHighKick();
                    break;
                case OpponentAIState.AttackThePlayer:
                    AttackThePlayer();
                    break;
                case OpponentAIState.RetreatFromThePlayer:
                    RetreatFromThePlayer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return null;
        }
    }

    private void OpponentIdle()
    {
        Debug.Log(nameof(OpponentIdle));

        if (OpponentIsGrounded())
        {
            return;
        }

        OpponentIdleAnimation();
        
        OpponentGravityIdle();
        
        _returnFightIntroFinished = FightIntro._fightIntroFinished;

        if (_returnFightIntroFinished != true)
        {
            return;
        }

        if (_decideAggressionPriority < _assessPriority.start)
        {
            _opponentAIState = OpponentAIState.AttackThePlayer;
        }

        if (_decideAggressionPriority > _offensivePriority.end
         && _decideAggressionPriority < _defensivePriority.start)
        {
            if (_assessingThePlayer)
            {
                return;
            }

            _assessingThePlayer = true;
            AssessThePlayer();
        }

        if (_decideAggressionPriority == _defensivePriority.start)
        {
            _opponentAIState = OpponentAIState.RetreatFromThePlayer;
        }
    }

    private void OpponentGravityIdle()
    {
        _opponentMoveDirection = new Vector3(0, _opponentVerticalSpeed, 0);

        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection);

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);
    }

    private void OpponentIdleAnimation()
    {
        Debug.Log(nameof(OpponentIdleAnimation));
        
        _opponentAnimator.CrossFade(_opponentIdleAnim.name);
    }

    private void Initialise()
    {
        Debug.Log(nameof(Initialise));

        _decideAggressionPriority = Random.Range(1, 9);

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void InitialiseAnimation()
    {
        Debug.Log(nameof(InitialiseAnimation));

        
    }

    private void AttackThePlayer()
    {
        Debug.Log(nameof(AttackThePlayer));

        
    }

    private void RetreatFromThePlayer()
    {
        Debug.Log(nameof(RetreatFromThePlayer));

        
    }

    private void AssessThePlayer()
    {
        Debug.Log(nameof(AssessThePlayer));

        
    }

    private void OpponentHitByLowKick()
    {
        Debug.Log(nameof(OpponentHitByLowKick));
        
        OpponentHitHeadAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);
        
        var _impactPoint = PlayerLowKick._opponentImpactPoint;

        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f), Quaternion.identity) as GameObject;
        
        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitByHighKick()
    {
        Debug.Log(nameof(OpponentHitByHighKick));
        
        OpponentHitHeadAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);
        
        var _impactPoint = PlayerHighKick._opponentImpactPoint;

        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f), Quaternion.identity) as GameObject;

        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitBodyAnimation()
    {
        Debug.Log(nameof(OpponentHitBodyAnimation));

        _opponentAnimator.CrossFade(_opponentHitBodyAnim.name);
    }

    public float _temp = 0;

    private void OpponentHitByLeftPunch()
    {
        Debug.Log(nameof(OpponentHitByLeftPunch));

        OpponentHitHeadAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerPunchLeft._opponentImpactPoint;
        
        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f), Quaternion.identity) as GameObject;
        
        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitByRightPunch()
    {
        Debug.Log(nameof(OpponentHitByRightPunch));

        OpponentHitHeadAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerPunchRight._opponentImpactPoint;
        
        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f), Quaternion.identity) as GameObject;
        
        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitHeadAnimation()
    {
        Debug.Log(nameof(OpponentHitHeadAnimation));

        _opponentAnimator.CrossFade(_opponentHitHeadAnim.name);
    }

    private void WaitForHitAnimations()
    {
        Debug.Log(nameof(WaitForHitAnimations));
    }

    private void WaitForHitAnimationsAnimation()
    {
        Debug.Log(nameof(WaitForHitAnimationsAnimation));

        if (_opponentAnimator.IsPlaying(_opponentHitBodyAnim.name))
        {
            return;
        }
        if (_opponentAnimator.IsPlaying(_opponentHitHeadAnim.name))
        {
            return;
        }

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void OpponentDefeated()
    {
        Debug.Log(nameof(OpponentDefeated));
        
        OpponentGravityIdle();

        if (_opponentAnimator.IsPlaying(_opponentDefeatedFinalHitAnim.name))
        {
            return;
        }
        
        StopCoroutine(OpponentFSM());
    }

    private void SetOpponentDefeated()
    {
        Debug.Log(nameof(SetOpponentDefeated));
        
        OpponentFinalHitAnimation();

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void OpponentFinalHitAnimation()
    {
        Debug.Log(nameof(OpponentFinalHitAnimation));
        
        _opponentAnimator.CrossFade(_opponentDefeatedFinalHitAnim.name);
    }

    private void DefeatedFinalHit()
    {
        Debug.Log(nameof(DefeatedFinalHit));
    }

    private void DefeatedFinalHitAnimation()
    {
        Debug.Log(nameof(DefeatedFinalHitAnimation));

        
    }

    private void ApplyOpponentGravity()
    {
        if (OpponentIsGrounded())
		{
			_opponentVerticalSpeed = .0f;
		}
		else
        {
	        _opponentVerticalSpeed -= _opponentGravity * _opponentGravityModifier * Time.deltaTime;
        }
    }

    public bool OpponentIsGrounded() => (_collisionFlagsOpponent & CollisionFlags.Below) != 0;
}
