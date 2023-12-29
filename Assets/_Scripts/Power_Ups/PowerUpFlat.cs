using System;
using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    public abstract class PowerUpFlat : PowerUp
    {
        [SerializeField] private int _amount;
        [SerializeField] private LTF.ValueGeneric.ValueInt _valueToRemove;

        protected override string Num => _amount.ToString();

        // ToDo : Maybe this could be better with a interface??
        protected abstract Func<int, int> ModifyValue(Cards.CardManager cm);

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            var amount = ModifyValue(cm)(_amount);

            if (_valueToRemove == null)
                return;

            // Is Maxed
            if (_amount > 0 ? amount >= _valueToRemove : amount <= _valueToRemove)
                Remove(cm);
        }   
    }
}
