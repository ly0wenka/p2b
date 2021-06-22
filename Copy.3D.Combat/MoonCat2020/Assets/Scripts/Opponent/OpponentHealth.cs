using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHealth : MonoBehaviour
{
    public static int _minimumOpponentHealth = 0;
    public static int _maximumOpponentHealth = 100;
    public static int _currentOpponentHealth = 100;

    private bool _isOpponentDefeated;
    
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
}
