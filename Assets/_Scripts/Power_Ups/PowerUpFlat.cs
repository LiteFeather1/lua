using System;
using UnityEngine;

public abstract class PowerUpFlat : PowerUp
{
    [SerializeField] private int _amount;
    [SerializeField] private ValueInt _valueToRemove;

    protected override string Num => _amount.ToString();

    // ToDo : Maybe this could be better with a interface??
    protected abstract Func<int, int> ModifyValue(GameManager gm);

    protected override void ApplyEffect(GameManager gm)
    {
        var amount = ModifyValue(gm)(_amount);

        if (_valueToRemove == null)
            return;

        // Is Maxed
        if (_amount > 0 ? amount >= _valueToRemove : amount <= _valueToRemove)
            Remove(gm.CardManager);
    }   
}
