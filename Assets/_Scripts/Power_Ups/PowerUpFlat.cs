using LTFUtils;
using System;
using UnityEngine;

public abstract class PowerUpFlat : PowerUp
{
    [SerializeField] protected int _amount;
    [SerializeField] private OptionalValue<int> _removeValue;
    protected override string Num => $"+{_amount}";

    protected abstract Func<int, int> ModifyValue(GameManager gm);

    protected override void ApplyEffect(GameManager gm)
    {
        var amount = ModifyValue(gm)(_amount);

        bool isMaxed;
        if (_removeValue.Value > 0)
            isMaxed = amount >= _removeValue.Value;
        else
            isMaxed = amount <= _removeValue.Value;

        if (_removeValue.Enabled && isMaxed)
            gm.CardManager.RemoveCardsOfType(PowerUpType);
    }
}
