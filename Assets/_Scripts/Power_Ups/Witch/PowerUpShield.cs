using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Shield")]
public class PowerUpShield : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Health.AddShield(_amount);
    }
}
