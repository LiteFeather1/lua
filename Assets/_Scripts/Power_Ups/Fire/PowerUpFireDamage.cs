using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Fire/Damage Mulplier")]
public class PowerUpFireDamage : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.EffectCreatorFire.DamageMultiplier;
    }
}
