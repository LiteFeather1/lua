using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Lightining/Lightning Chance")]
public class PowerUpLightningChance : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.LightningChance;
    }
}
