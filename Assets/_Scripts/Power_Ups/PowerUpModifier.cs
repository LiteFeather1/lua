using UnityEngine;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps
{
    public abstract class PowerUpModifier : PowerUp
    {
        [Header("Power Up Modifier")]
        [SerializeField] private CompositeValueModifier _modifier;
        [SerializeField] private LTF.ValueGeneric.ValueFloat _valueToRemove;

        public CompositeValueModifier Modifier => _modifier;

        protected override string Num
        {
            get
            {
                return _modifier.Type switch
                {
                    CompositeValueModifierType.Flat => FlatModifier(),
                    CompositeValueModifierType.PercentAdditive => PercentModifer(),
                    CompositeValueModifierType.PercentMultiplier => PercentModifer(),
                    _ => ""
                };
            }
        }

        protected abstract CompositeValue ValueToModify(Cards.CardManager cm);

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            var compositeValue = ValueToModify(cm);
            compositeValue.AddModifier(_modifier);

            if (_valueToRemove == null)
                return;
            // is Maxed
            if (_modifier > 0f ? compositeValue >= _valueToRemove : compositeValue <= _valueToRemove)
                Remove(cm);
        }

        private string FlatModifier()
        {
            if (Mathf.Abs(_modifier) < 1f)
                return PercentModifer();

            return _modifier.Value.ToString();
        }

        private string PercentModifer()
        {
            if (_modifier >= 0f)
                return _modifier.Value.ToString("0.0%");
            else
                return (-_modifier.Value).ToString("0.0%");
        }
    }
}
