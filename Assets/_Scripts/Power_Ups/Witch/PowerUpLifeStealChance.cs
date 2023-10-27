using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Life Steal Chance")]
public class PowerUpLifeStealChance : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.ChanceToLifeSteal.AddModifier(_modifier);
    }
}
