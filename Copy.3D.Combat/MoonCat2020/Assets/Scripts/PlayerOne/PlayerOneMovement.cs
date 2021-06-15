using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerOneMovement : MonoBehaviour
{
    private Transform _playerOneTransform;
    private CharacterController _playerController;

    public float _playerWalkSpeed = 1f;
    public float _playerRetreatSpeed = .75f;
    public float _playerJumpHeight = 5f;
    public float _playerJumpSpeed = 5f;
    public float _playerJumpHorizontal = 5f;

    private Animation _playerOneAnim;

    public AnimationClip _playerOneIdleAnim;
    public AnimationClip _playerOneWalkAnim;
    public AnimationClip _playerOneJumpAnim;
    public AnimationClip _playerOneDemoAnim;
    public AnimationClip[] _playerAttackAnim;

    public float _controllerDeadZonePos = .1f;
    public float _controllerDeadZoneNeg = -.1f;

    public float _playersGravity = 20f;
    public float _playerGravityModifier = 5f;
    public float _playersSpeedYAxis;

    private bool _returnDemoState;
    private int _demoRotationValue = 75;

    private bool _returnFightIntroFinished;

    private Vector3 _playerOneMoveDirection = Vector3.zero;

    private CollisionFlags _collisionFlags;

    private PlayerOneStates _playerOneStates;

    // Start is called before the first frame update
    void Start()
    {
        _playerOneTransform = transform;

        _playerOneMoveDirection = Vector3.zero;

        _playersSpeedYAxis = 0;

        _playerController = GetComponent<CharacterController>();

        _playerOneAnim = GetComponent<Animation>();

        foreach (var animClip in _playerAttackAnim)
        {
            _playerOneAnim[animClip.name].wrapMode = WrapMode.Once;
        }

        StartCoroutine(nameof(PlayerOneFSM));

        _returnDemoState = false;

        _returnDemoState = ChooseCharacter._demoPlayer;

        if (_returnDemoState)
        {
            _playerOneStates = PlayerOneStates.PlayerDemo;
        }
    }

    private IEnumerator PlayerOneFSM()
    {
        while (true)
        {
            switch (_playerOneStates)
            {
                case PlayerOneStates.PlayerOneIdle:
                    PlayerOneIdle();
                    break;
                case PlayerOneStates.PlayerWalkLeft:
                    PlayerOneWalkLeft();
                    break;
                case PlayerOneStates.PlayerWalkRight:
                    PlayerOneWalkRight();
                    break;
                case PlayerOneStates.PlayerJump:
                    PlayerOneJump();
                    break;
                case PlayerOneStates.PlayerJumpForward:
                    PlayerOneJumpForward();
                    break;
                case PlayerOneStates.PlayerJumpBackwards:
                    PlayerOneJumpBackwards();
                    break;
                case PlayerOneStates.ComeDownForwards:
                    PlayerOneComeDownForwards();
                    break;
                case PlayerOneStates.ComeDownBackwards:
                    PlayerOneComeDownBackwards();
                    break;
                case PlayerOneStates.ComeDown:
                    PlayerOneComeDown();
                    break;
                case PlayerOneStates.PlayerHighPunch:
                    PlayerHighPunch();
                    break;
                case PlayerOneStates.PlayerLowPunch:
                    PlayerLowPunch();
                    break;
                case PlayerOneStates.PlayerHighKick:
                    PlayerHighKick();
                    break;
                case PlayerOneStates.PlayerLowKick:
                    PlayerLowKick();
                    break;
                case PlayerOneStates.WaitForAnimations:
                    WaitForAnimations();
                    break;
                case PlayerOneStates.PlayerDemo:
                    PlayerDemo();
                    break;
            }

            yield return null;
        }
    }

    private void PlayerOneJumpForward()
    {
        Debug.Log(nameof(PlayerOneJumpForward));

        PlayerOneJumpForwardAnim();

        _playerOneMoveDirection = new Vector3(-_playerJumpHorizontal, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (_playerOneTransform.transform.position.y >= _playerJumpHeight)
        {
            _playerOneStates = PlayerOneStates.ComeDownForwards;
        }
    }

    private void PlayerOneJumpForwardAnim()
    {
        Debug.Log(nameof(PlayerOneJumpForwardAnim));
    }

    private void PlayerOneJumpBackwards()
    {
        Debug.Log(nameof(PlayerOneJumpBackwards));

        PlayerOneJumpBackwardsAnim();

        _playerOneMoveDirection = new Vector3(+_playerJumpHorizontal, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (_playerOneTransform.transform.position.y >= _playerJumpHeight)
        {
            _playerOneStates = PlayerOneStates.ComeDownBackwards;
        }
    }

    private void PlayerOneJumpBackwardsAnim()
    {
        Debug.Log(nameof(PlayerOneJumpBackwardsAnim));
    }

    private void PlayerOneComeDownForwards()
    {
        Debug.Log(nameof(PlayerOneComeDownForwards));

        PlayerOneComeDownForwardsAnim();

        _playerOneMoveDirection = new Vector3(-_playerJumpHorizontal, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (PlayerOneIsGrounded())
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneComeDownForwardsAnim()
    {
        Debug.Log(nameof(PlayerOneComeDownForwardsAnim));
    }

    private void PlayerOneComeDownBackwards()
    {
        Debug.Log(nameof(PlayerOneComeDownBackwards));

        PlayerOneComeDownBackwardsAnim();

        _playerOneMoveDirection = new Vector3(+_playerJumpHorizontal, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (PlayerOneIsGrounded())
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneComeDownBackwardsAnim()
    {
        Debug.Log(nameof(PlayerOneComeDownBackwardsAnim));
    }

    private void WaitForAnimations()
    {
        Debug.Log(nameof(WaitForAnimations));

        WaitForAnimationsAnim();

        for (int i = 0; i < _playerAttackAnim.Length; i++)
        {
            if (_playerOneAnim.IsPlaying(_playerAttackAnim[i].name))
            {
                return;
            }
        }

        _playerOneStates = PlayerOneStates.PlayerOneIdle;
    }

    private void WaitForAnimationsAnim()
    {
        Debug.Log(nameof(WaitForAnimationsAnim));
    }

    private void PlayerDemo()
    {
        Debug.Log(nameof(PlayerDemo));

        PlayerDemoAnim();

        if (Input.GetAxis("LeftTrigger") > .1f)
        {
            transform.Rotate(Vector3.up * _demoRotationValue * Time.deltaTime);
        }

        if (Input.GetAxis("RightTrigger") < -.1f)
        {
            transform.Rotate(Vector3.down * _demoRotationValue * Time.deltaTime);
        }
    }

    private void PlayerDemoAnim()
    {
        Debug.Log(nameof(PlayerDemoAnim));

        _playerOneAnim.CrossFade(_playerOneDemoAnim.name);
    }


    private void PlayerOneIdle()
    {
        Debug.Log(nameof(PlayerOneIdle));

        PlayerOneIdleAnim();

        if (PlayerOneIsGrounded())
        {
            return;
        }

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);
    }

    private void PlayerOneIdleAnim()
    {
        Debug.Log(nameof(PlayerOneIdleAnim));

        _playerOneAnim.CrossFade(_playerOneIdleAnim.name);
    }

    private void PlayerOneWalkLeft()
    {
        Debug.Log(nameof(PlayerOneWalkLeft));

        PlayerOneRetreatAnim();

        _playerOneMoveDirection = new Vector3(+_playerWalkSpeed, 0, 0);
        MoveDirection();

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        SetIdleToState();
    }

    private void MoveDirection()
    {
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerWalkSpeed;
    }

    private void SetIdleToState()
    {
        if (Input.GetAxis("Horizontal") == 0f)
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneWalkRight()
    {
        Debug.Log(nameof(PlayerOneWalkRight));

        PlayerOneWalkAnim();

        _playerOneMoveDirection = new Vector3(-_playerWalkSpeed, 0, 0);
        MoveDirection();

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        SetIdleToState();
    }

    private void PlayerOneWalkAnim()
    {
        Debug.Log(nameof(PlayerOneWalkAnim));

        _playerOneAnim.CrossFade(_playerOneWalkAnim.name);

        if (Math.Abs(_playerOneAnim[_playerOneWalkAnim.name].speed - _playerWalkSpeed) < 0.01f)
        {
            return;
        }

        if (_playerOneAnim[_playerOneWalkAnim.name].speed < _playerWalkSpeed)
        {
            _playerOneAnim[_playerOneAnim.name].speed = _playerWalkSpeed;
        }
    }

    private void PlayerOneJump()
    {
        Debug.Log(nameof(PlayerOneJump));

        PlayerOneJumpAnim();

        _playerOneMoveDirection = new Vector3(0, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (_playerOneTransform.transform.position.y >= _playerJumpHeight)
        {
            _playerOneStates = PlayerOneStates.ComeDown;
        }
    }

    private void PlayerOneComeDown()
    {
        Debug.Log(nameof(PlayerOneComeDown));

        PlayerOneComeDownAnim();

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (PlayerOneIsGrounded())
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneJumpAnim()
    {
        Debug.Log(nameof(PlayerOneJumpAnim));

        _playerOneAnim.CrossFade(_playerOneJumpAnim.name);
    }

    private void PlayerHighPunchAnim()
    {
        Debug.Log(nameof(PlayerHighPunchAnim));

        _playerOneAnim.CrossFade(_playerAttackAnim[0].name);
    }

    private void PlayerLowPunchAnim()
    {
        Debug.Log(nameof(PlayerLowPunchAnim));

        _playerOneAnim.CrossFade(_playerAttackAnim[1].name);
    }

    private void PlayerHighKickAnim()
    {
        Debug.Log(nameof(PlayerHighKickAnim));

        _playerOneAnim.CrossFade(_playerAttackAnim[2].name);
    }

    private void PlayerLowKickAnim()
    {
        Debug.Log(nameof(PlayerLowKickAnim));

        _playerOneAnim.CrossFade(_playerAttackAnim[3].name);
    }

    private void PlayerOneRetreatAnim()
    {
        Debug.Log(nameof(PlayerOneRetreatAnim));

        _playerOneAnim.CrossFade(_playerOneWalkAnim.name);

        if (Math.Abs(_playerOneAnim[_playerOneWalkAnim.name].speed - _playerRetreatSpeed) < 0.01f)
        {
            return;
        }

        if (_playerOneAnim[_playerOneWalkAnim.name].speed > _playerRetreatSpeed)
        {
            _playerOneAnim[_playerOneAnim.name].speed = _playerRetreatSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();

        if (_playerAttackAnim.Any(clip => _playerOneAnim.IsPlaying(clip.name)))
        {
            return;
        }

        _returnFightIntroFinished = FightIntro._fightIntroFinished;

        if (_returnFightIntroFinished != true)
        {
            return;
        }

        if (PlayerOneIsGrounded())
        {
            HorizontalInputManager();
            AttackInputManager();
            StandardInputManager();
        }
    }

    private void PlayerHighPunch()
    {
        Debug.Log(nameof(PlayerHighPunch));

        PlayerHighPunchAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void PlayerLowPunch()
    {
        Debug.Log(nameof(PlayerLowPunch));

        PlayerLowPunchAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void PlayerHighKick()
    {
        Debug.Log(nameof(PlayerHighKick));

        PlayerHighKickAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void PlayerLowKick()
    {
        Debug.Log(nameof(PlayerLowKick));

        PlayerLowKickAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void AttackInputManager()
    {
        Debug.Log(nameof(AttackInputManager));

        if (Input.GetButtonDown("Fire1"))
        {
            _playerOneStates = PlayerOneStates.PlayerLowKick;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _playerOneStates = PlayerOneStates.PlayerHighKick;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            _playerOneStates = PlayerOneStates.PlayerHighPunch;
        }

        if (Input.GetButtonDown("Fire4"))
        {
            _playerOneStates = PlayerOneStates.PlayerLowPunch;
        }
    }

    private void HorizontalInputManager()
    {
        Debug.Log(nameof(HorizontalInputManager));

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos && Input.GetAxis("Horizontal") > _controllerDeadZoneNeg)
        {
            _playerOneStates = PlayerOneStates.PlayerJumpForward;
        }

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos && Input.GetAxis("Horizontal") < _controllerDeadZoneNeg)
        {
            _playerOneStates = PlayerOneStates.PlayerJumpBackwards;
        }
    }

    private void HorizontalInputManagerAnim()
    {
        Debug.Log(nameof(HorizontalInputManagerAnim));
    }

    private void StandardInputManager()
    {
        Debug.Log(nameof(StandardInputManager));

        if (Input.GetAxis("Horizontal") < _controllerDeadZoneNeg)
        {
            _playerOneStates = PlayerOneStates.PlayerWalkLeft;
        }

        if (Input.GetAxis("Horizontal") > _controllerDeadZonePos)
        {
            _playerOneStates = PlayerOneStates.PlayerWalkRight;
        }

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos)
        {
            _playerOneStates = PlayerOneStates.PlayerJump;
        }
    }

    private void StandardInputManagerAnim()
    {
        Debug.Log(nameof(StandardInputManagerAnim));
    }


    private void AttackInputManagerAnim()
    {
        Debug.Log(nameof(AttackInputManagerAnim));
    }


    private void ApplyGravity()
    {
        Debug.Log(nameof(ApplyGravity));

        if (PlayerOneIsGrounded())
        {
            _playersSpeedYAxis = 0f;
        }
        else
        {
            _playersSpeedYAxis -= _playersGravity * _playerGravityModifier * Time.deltaTime;
        }
    }

    private bool PlayerOneIsGrounded() => (_collisionFlags & CollisionFlags.CollidedBelow) != 0;

    private void PlayerOneComeDownAnim()
    {
        Debug.Log(nameof(PlayerOneComeDownAnim));
    }
}