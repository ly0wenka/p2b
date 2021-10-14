using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public event Action CoinsChanged;
    public int Coins { get; set; }
    public int MaxCoins { get; set; }

    protected virtual void OnCoinsChanged()
    {
        CoinsChanged?.Invoke();
    }

    public void AddCoin()
    {
        if (Coins < MaxCoins)
        {
            Coins++;
        }
    }
}
