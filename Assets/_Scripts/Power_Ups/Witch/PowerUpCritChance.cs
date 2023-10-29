using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Crit Chance")]
public class PowerUpCritChance : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.CritChance.AddModifier(_modifier);
    }
}
