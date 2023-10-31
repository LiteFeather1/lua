using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Crit Damage")]
public class PowerUpCritDamage : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.CritMultiplier.AddModifier(_modifier);
    }
}
