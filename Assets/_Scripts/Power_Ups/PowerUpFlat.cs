using System;
using UnityEngine;

public abstract class PowerUpFlat : PowerUp
{
    [SerializeField] private int _amount;
    [SerializeField] private ValueInt _valueToRemove;
    protected override string Num => $"+{_amount}";

    protected abstract Func<int, int> ModifyValue(GameManager gm);

    protected override void ApplyEffect(GameManager gm)
    {
        var amount = ModifyValue(gm)(_amount);

        if (_valueToRemove != null)
            return;

        bool isMaxed;
        if (_amount > 0)
            isMaxed = amount >= _valueToRemove.Value;
        else
            isMaxed = amount <= _valueToRemove.Value;

        if (isMaxed)
            gm.CardManager.RemoveCardsOfType(PowerUpType);
    }
}
