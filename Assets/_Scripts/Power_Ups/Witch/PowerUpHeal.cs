using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Heal")]
public class PowerUpHeal : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Health.Heal(_amount);
    }
}
