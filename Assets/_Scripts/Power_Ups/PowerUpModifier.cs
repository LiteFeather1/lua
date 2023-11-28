using LTFUtils;
using UnityEngine;

public abstract class PowerUpModifier : PowerUp
{
    [Header("Power Up Modifier")]
    [SerializeField] private CompositeValueModifier _modifier;
    [SerializeField] private ValueFloat _valueToRemove;

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

    protected abstract CompositeValue ValueToModify(GameManager gm);

    protected override void ApplyEffect(GameManager gm)
    {
        var compositeValue = ValueToModify(gm);
        compositeValue.AddModifier(_modifier);

        if (_valueToRemove == null)
            return;

        bool isMaxed = _modifier.Value > 0f
                       ? compositeValue.Value >= _valueToRemove.Value
                       : compositeValue.Value <= _valueToRemove.Value;

        if (isMaxed)
            Remove(gm.CardManager);
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
