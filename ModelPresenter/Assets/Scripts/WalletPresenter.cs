using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UIElements;

public class WalletPresenter
{
    private ClampedAmountWithIcon _view;
    private Wallet _model;

    public WalletPresenter(ClampedAmountWithIcon view, Wallet model)
    {
        _view = view;
        _model = model;
    }

    // Start is called before the first frame update
    void Enable()
    {
        _model.CoinsChanged += OnCoinsChanged;
        _view.Click += OnClick;
    }

    // Update is called once per frame
    void Disable()
    {
        _model.CoinsChanged -= OnCoinsChanged;
        _view.Click -= OnClick;
    }

    void OnCoinsChanged()
    {
        _view.SetAmount(_model.Coins, _model.MaxCoins);
    }

    void OnClick()
    {
        
    }
}
