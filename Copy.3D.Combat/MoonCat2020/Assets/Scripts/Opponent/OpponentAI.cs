using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OpponentAI : MonoBehaviour
{
    private Transform _opponentTransform;
    private CharacterController _opponentController;

    private Animation _opponentAnim;

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
    
    private bool _returnFightIntroFinished;

    private CollisionFlags _collisionFlagsOpponent;

    public static OpponentAIState _opponentAIState;
    
    // Start is called before the first frame update
    void Start()
    {
        _opponentController = GetComponent<CharacterController>();

        _opponentAnim = GetComponent<Animation>();

        _opponentAIAudioSource = GetComponent<AudioSource>();

        _opponentTransform = transform;
        
        _opponentMoveDirection = Vector3.zero;

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
                case OpponentAIState.OpponentHitBody:
                    OpponentHitBody();
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

        OpponentMove();
        
        _returnFightIntroFinished = FightIntro._fightIntroFinished;

        if (_returnFightIntroFinished != true)
        {
            return;
        }
    }

    private void OpponentMove()
    {
        _opponentMoveDirection = new Vector3(0, _opponentVerticalSpeed, 0);

        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection);

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);
    }

    private void OpponentIdleAnimation()
    {
        Debug.Log(nameof(OpponentIdleAnimation));
        
        _opponentAnim.CrossFade(_opponentIdleAnim.name);
    }

    private void Initialise()
    {
        Debug.Log(nameof(Initialise));

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void InitialiseAnimation()
    {
        Debug.Log(nameof(InitialiseAnimation));

        
    }

    private void OpponentHitBody()
    {
        Debug.Log(nameof(OpponentHitBody));
        
        OpponentHitBodyAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentBodyHitAudio);
        
        var _hs = Instantiate(_hitSparks, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        
        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitBodyAnimation()
    {
        Debug.Log(nameof(OpponentHitBodyAnimation));

        _opponentAnim.CrossFade(_opponentHitBodyAnim.name);
    }

    public float _temp = 0;

    private void OpponentHitByLeftPunch()
    {
        Debug.Log(nameof(OpponentHitByLeftPunch));

        OpponentHitHeadAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerPunchLeft._opponentImpactPoint;
        
        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y + 1, _impactPoint.z + -.2f), Quaternion.identity) as GameObject;
        
        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitByRightPunch()
    {
        Debug.Log(nameof(OpponentHitByRightPunch));

        OpponentHitHeadAnimation();
        
        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerPunchRight._opponentImpactPoint;
        
        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y + 1, _impactPoint.z + -.2f), Quaternion.identity) as GameObject;
        
        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitHeadAnimation()
    {
        Debug.Log(nameof(OpponentHitHeadAnimation));

        _opponentAnim.CrossFade(_opponentHitHeadAnim.name);
    }

    private void WaitForHitAnimations()
    {
        Debug.Log(nameof(WaitForHitAnimations));
    }

    private void WaitForHitAnimationsAnimation()
    {
        Debug.Log(nameof(WaitForHitAnimationsAnimation));

        if (_opponentAnim.IsPlaying(_opponentHitBodyAnim.name))
        {
            return;
        }
        if (_opponentAnim.IsPlaying(_opponentHitHeadAnim.name))
        {
            return;
        }

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void OpponentDefeated()
    {
        Debug.Log(nameof(OpponentDefeated));
        
        OpponentMove();

        if (_opponentAnim.IsPlaying(_opponentDefeatedFinalHitAnim.name))
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
        
        _opponentAnim.CrossFade(_opponentDefeatedFinalHitAnim.name);
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
