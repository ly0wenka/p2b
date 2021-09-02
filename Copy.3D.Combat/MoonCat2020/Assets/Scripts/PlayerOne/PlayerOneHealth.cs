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

    private void OnEnable() => OnDecreaseCurrentOpponentHealth += DecreaseCurrentOpponentHealth;

    private void OnDisable() => OnDecreaseCurrentOpponentHealth -= DecreaseCurrentOpponentHealth;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _currentPlayerHealth = _maximumPlayerHealth;

        _isPlayerDefeated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDamage(int damageDealtToPlayer)
    {
        Debug.Log(nameof(PlayerDamage));
        
        if (_isPlayerDefeated)
        {
            return;
        }

        OnDecreaseCurrentOpponentHealth?.Invoke(damageDealtToPlayer);

        if (_currentPlayerHealth < _minimumPlayerHealth)
        {
            _currentPlayerHealth = _minimumPlayerHealth;
        }

        if (_currentPlayerHealth == _minimumPlayerHealth)
        {
            _isPlayerDefeated = true;
            
            SendMessage("SetPlayerDefeated", SendMessageOptions.DontRequireReceiver);
        }
    }

    protected virtual void DecreaseCurrentOpponentHealth(int damageDealtToPlayer)
    {
        _currentPlayerHealth -= damageDealtToPlayer;
    }
}
