using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Crit Chance")]
public class PowerUpCritChance : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.CritChance;
    }
}
