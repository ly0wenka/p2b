using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletViewModel : ViewModel
{
    [Model] private Wallet _wallet = new Wallet();
    [Project] public int Coins => _wallet.Coins;
    [Command] public void AddCoin() => _wallet.AddCoin();
    
    private class ModelAttribute : Attribute
    {
        
    }

    public class ProjectAttribute : Attribute
    {
    }

    public class CommandAttribute : Attribute
    {
    }
}
