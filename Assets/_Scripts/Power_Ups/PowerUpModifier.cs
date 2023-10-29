using UnityEngine;

public abstract class PowerUpModifier : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    protected override string Num
    {
        get
        {
            return _modifier.Type switch
            {
                CompositeValueModifierType.Flat => $"+{_modifier.Value}",
                CompositeValueModifierType.PercentAdditive => $"+{Mathf.Abs(_modifier.Value) * 100f:0.00}%",
                CompositeValueModifierType.PercentMultiplier => $"+{Mathf.Abs(_modifier.Value) * 100f:0.00}%",
                _ => ""
            };
        }
    }
}
