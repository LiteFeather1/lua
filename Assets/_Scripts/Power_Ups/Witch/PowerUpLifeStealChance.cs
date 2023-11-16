using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Life Steal Chance")]
public class PowerUpLifeStealChance : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.ChanceToLifeSteal;
    }
}
