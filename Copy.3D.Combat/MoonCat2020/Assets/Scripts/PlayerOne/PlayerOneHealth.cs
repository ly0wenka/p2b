using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHealth : MonoBehaviour
{
    public static int _minimumPlayerHealth = 0;
    public static int _maximumPlayerHealth = 100;
    public static int _currentPlayerHealth = 100;

    private bool _isPlayerDefeated;
    
    #region events
    
    public delegate void OpponentDamageHandler(int damageDealtToOpponent);
    public event OpponentDamageHandler OnDecreaseCurrentOpponentHealth;
    
    public delegate void PlayerOneHealthDepletedCheckHandler();
    public event PlayerOneHealthDepletedCheckHandler OnPlayerOneHealthDepletedCheck;

    private void OnEnable()
    {
        OnDecreaseCurrentOpponentHealth += DecreaseCurrentOpponentHealth;
        OnPlayerOneHealthDepletedCheck += PlayerOneHealthDepletedCheck;
    }

    private void OnDisable()
    {
        OnDecreaseCurrentOpponentHealth -= DecreaseCurrentOpponentHealth;
        OnPlayerOneHealthDepletedCheck -= PlayerOneHealthDepletedCheck;
    }

    #endregion
    
    void Start()
    {
        _currentPlayerHealth = _maximumPlayerHealth;

        _isPlayerDefeated = false;
    }
    
    public void PlayerOneLowKickDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(PlayerOneLowKickDamage));
        
        if (_isPlayerDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);

        SendMessageUpwards("PlayerHitByLowKick", SendMessageOptions.DontRequireReceiver);
        
        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    public void PlayerOneHighKickDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(PlayerOneHighKickDamage));
        
        if (_isPlayerDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);

        SendMessageUpwards("PlayerHitByHighKick", SendMessageOptions.DontRequireReceiver);
        
        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    public void PlayerOneLeftPunchDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(PlayerOneLeftPunchDamage));
        
        if (_isPlayerDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);

        SendMessageUpwards("PlayerHitByLeftPunch", SendMessageOptions.DontRequireReceiver);

        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    public void PlayerOneRightPunchDamage(int damageDealtToOpponent)
    {
        Debug.Log(nameof(PlayerOneLeftPunchDamage));
        
        if (_isPlayerDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToOpponent);
        
        SendMessageUpwards("PlayerHitByRightPunch", SendMessageOptions.DontRequireReceiver);

        OnPlayerOneHealthDepletedCheck?.Invoke();
    }

    private void PlayerOneHealthDepletedCheck()
    {
        Debug.Log(nameof(PlayerOneHealthDepletedCheck));

        if (_currentPlayerHealth < _minimumPlayerHealth)
        {
            _currentPlayerHealth = _minimumPlayerHealth;
        }

        if (_currentPlayerHealth == _minimumPlayerHealth)
        {
            _isPlayerDefeated = true;
            
            SendMessageUpwards("SetPlayerDefeated", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void DecreaseCurrentOpponentHealth(int damageDealtToPlayer)
    {
        _currentPlayerHealth -= damageDealtToPlayer;
    }
}
