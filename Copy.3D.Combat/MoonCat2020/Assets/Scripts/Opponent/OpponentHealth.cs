using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHealth : MonoBehaviour
{
    public static int _minimumOpponentHealth = 0;
    public static int _maximumOpponentHealth = 100;
    public static int _currentOpponentHealth = 100;

    private bool _isOpponentDefeated;

    #region events

    public delegate void OpponentDamageHandler(int damageDealtToOpponent);
    public event OpponentDamageHandler OnDecreaseCurrentOpponentHealth;

    public delegate void OpponentHealthDepletedCheckHandler();
    public event OpponentHealthDepletedCheckHandler OnPlayerOneHealthDepletedCheck;

    private void OnEnable()
    {
        OnDecreaseCurrentOpponentHealth += DecreaseCurrentOpponentHealth;
        OnPlayerOneHealthDepletedCheck += OpponentHealthDepletedCheck;
    }

    private void OnDisable()
    {
        OnDecreaseCurrentOpponentHealth -= DecreaseCurrentOpponentHealth;
        OnPlayerOneHealthDepletedCheck -= OpponentHealthDepletedCheck;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _currentOpponentHealth = _maximumOpponentHealth;

        _isOpponentDefeated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpponentDefeated)
        {
            return;
        }

        if (_currentOpponentHealth < _minimumOpponentHealth)
        {
            _currentOpponentHealth = _minimumOpponentHealth;
        }

        if (_currentOpponentHealth == _minimumOpponentHealth)
        {
            _isOpponentDefeated = true;
            
            SendMessage("SetOpponentDefeated", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OpponentLowKickDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(OpponentLowKickDamage));
        
        if (_isOpponentDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);
        
        SendMessageUpwards("OpponentHitByLowKick",
            SendMessageOptions.DontRequireReceiver);
        
        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    public void OpponentHighKickDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(OpponentHighKickDamage));
        
        if (_isOpponentDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);
        
        SendMessageUpwards("OpponentHitByHighKick",
            SendMessageOptions.DontRequireReceiver);
        
        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    public void OpponentRightPunchDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(OpponentRightPunchDamage));
        
        if (_isOpponentDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);
        
        SendMessageUpwards("OpponentHitByRightPunch",
            SendMessageOptions.DontRequireReceiver);
        
        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    public void OpponentLeftPunchDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(OpponentLeftPunchDamage));
        
        if (_isOpponentDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);
        
        SendMessageUpwards("OpponentHitByLeftPunch",
            SendMessageOptions.DontRequireReceiver);
        
        OnPlayerOneHealthDepletedCheck?.Invoke();
    }
    
    private void OpponentHealthDepletedCheck()
    {
        if (_currentOpponentHealth < _minimumOpponentHealth)
        {
            _currentOpponentHealth = _minimumOpponentHealth;
        }

        if (_currentOpponentHealth == _minimumOpponentHealth)
        {
            _isOpponentDefeated = true;
            
            SendMessage("SetOpponentDefeated", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void DecreaseCurrentOpponentHealth(int damageDealtToOpponent) => _currentOpponentHealth -= damageDealtToOpponent;
}
