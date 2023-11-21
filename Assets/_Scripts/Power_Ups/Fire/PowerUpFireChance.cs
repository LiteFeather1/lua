using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Fire/Fire Chance")]
public class PowerUpFireChance : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.EffectCreatorFire.Chance;
    }
}
