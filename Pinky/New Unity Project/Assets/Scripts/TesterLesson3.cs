using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterLesson3 : MonoBehaviour
{
    private List<TesterCoinsBase> testers;

    private Bank bank;
    private TesterCoinsPanel panel;

    private void Awake()
    {
        bank = new Bank();
        panel = new TesterCoinsPanel(bank);
        testers = new List<TesterCoinsBase>();
        testers.Add(new TesterCoin(bank));
        testers.Add(new TesterReward(bank));
        testers.Add(new TesterPurchase(bank));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddRandomCoins();
        }
    }

    private void AddRandomCoins()
    {
        var rIndex = Random.Range(0, testers.Count);
        var rTester = testers[rIndex];
        rTester.TestAddCoinsToBank();
    }
}
