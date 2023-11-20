using UnityEngine;

[System.Serializable]
public class EffectCreatorFire
{
    [field: SerializeField] public CompositeValue Chance { get; private set; }
    [field: SerializeField] public CompositeValue Duration { get; private set; } = new(3f);
    [field: SerializeField] public CompositeValue DamageMultiplier { get; private set; } = new(10f);
    [field: SerializeField] public int TickAmount { get; private set; } = 10;

    public DamageEffectFire Get(float initialDamage)
    {
        return new(Duration.Value, initialDamage * DamageMultiplier.Value, TickAmount);
    }
}
