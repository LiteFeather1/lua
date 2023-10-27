using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Increase HP")]
public class PowerUpIncreanseHP : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Health.IncreaseMaxHP(_amount);
    }
}
