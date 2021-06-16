using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterCoinsPanel : MonoBehaviour
{
    private Bank bank;

    public TesterCoinsPanel(Bank bank)
    {
        this.bank = bank;
        this.bank.OnCoinsValueChangedEvent += OnCoinsValueChanged;
        this.bank.OnConCoinsValueChangedAction += OnCoinsValueChangedAction;
    }

    private void OnCoinsValueChangedAction(object sender, int oldCoinsValue, int newcoinsvalue)
    {
        Debug.Log($"Coins received from ACTION {sender.GetType()}, oldValue = {oldCoinsValue}, newValue = {newcoinsvalue}");
    }

    private void OnCoinsValueChanged(object sender, int oldCoinsValue, int newcoinsvalue)
    {
        Debug.Log($"Coins received from {sender.GetType()}, oldValue = {oldCoinsValue}, newValue = {newcoinsvalue}");
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
