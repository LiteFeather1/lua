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
                CompositeValueModifierType.Flat => FlatModifier(),
                CompositeValueModifierType.PercentAdditive => PercentModifer(),
                CompositeValueModifierType.PercentMultiplier => PercentModifer(),
                _ => ""
            };
        }
    }

    private string FlatModifier()
    {
        if (_modifier.Value < 1f)
            return PercentModifer();
        return $"+{_modifier.Value}";
    }

    private string PercentModifer()
    {
        return $"+{Mathf.Abs(_modifier.Value) * 100f:0.00}%";
    }
}
