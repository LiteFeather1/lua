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

        if (_valueToRemove == null)
            return;

        bool isMaxed = _amount > 0 
                       ? amount >= _valueToRemove.Value 
                       : amount <= _valueToRemove.Value;

        if (isMaxed)
            Remove(gm.CardManager);
    }   
}
