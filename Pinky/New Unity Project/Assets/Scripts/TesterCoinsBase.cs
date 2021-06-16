using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterCoinsBase : MonoBehaviour
{
    private int coinsMin = 5;
    private int coinsMax = 100;
    private Bank bank;

    public TesterCoinsBase(Bank bank)
    {
        this.bank = bank;
    }

    public void TestAddCoinsToBank()
    {
        var rCoins = Random.Range(coinsMin, coinsMax);
        bank.AddCoins(this, rCoins);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
