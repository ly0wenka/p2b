using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneMovement : MonoBehaviour
{
    public AnimationClip _playerOneIdleAnim;
    private Animation _playerOneAnim;

    private PlayerOneStates _playerOneStates;

    // Start is called before the first frame update
    void Start()
    {
        _playerOneAnim = GetComponent<Animation>();

        StartCoroutine(nameof(PlayerOneFSM));
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
            }
        }

        yield return null;
    }

    private void PlayerOneIdle()
    {
        Debug.Log(nameof(PlayerOneIdle));

        PlayerOneIdleAnim();
    }

    private void PlayerOneIdleAnim()
    {
        Debug.Log(nameof(PlayerOneIdleAnim));

        _playerOneAnim.CrossFade(_playerOneIdleAnim.name);
    }

    // Update is called once per frame
    void Update()
    {
    }
}