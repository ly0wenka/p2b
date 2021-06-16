using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public delegate void BankHandler(object sender, int oldCoinsValue, int newCoinsValue);

    public event BankHandler OnCoinsValueChangedEvent;

    public event Action<object, int, int> OnConCoinsValueChangedAction;
    
    public int coins { get; private set; }

    public void AddCoins(object sender, int amount)
    {
        var oldCoinsValue = this.coins;
        this.coins += amount;
        
        this.OnCoinsValueChangedEvent?.Invoke(sender, oldCoinsValue, this.coins);
        this.OnConCoinsValueChangedAction?.Invoke(sender, oldCoinsValue, this.coins);
    }

    public void SpendCoins(object sender, int amount)
    {
        var oldCoinsValue = this.coins;
        this.coins -= amount;
        
        this.OnCoinsValueChangedEvent?.Invoke(sender, oldCoinsValue, this.coins);
        this.OnConCoinsValueChangedAction?.Invoke(sender, oldCoinsValue, this.coins);
    }

    public bool HasEnoughCoins(int amount) => amount <= coins;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
