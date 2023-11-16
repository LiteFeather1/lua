using LTFUtils;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class PowerUpModifier : PowerUp
{
    [Header("Power Up Modifier")]
    [SerializeField] protected CompositeValueModifier _modifier;
    [SerializeField] protected OptionalValue<float> _removeValue;

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

        bool isMaxed;
        if (_modifier.Value > 0f)
            isMaxed = compositeValue.Value >= _removeValue.Value;
        else
            isMaxed = compositeValue.Value <= _removeValue.Value;

        if (_removeValue.Enabled && isMaxed)
            gm.CardManager.RemoveCardsOfType(PowerUpType);
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
