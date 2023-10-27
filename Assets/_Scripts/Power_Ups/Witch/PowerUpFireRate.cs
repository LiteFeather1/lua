using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Fire Rate")]
public class PowerUpFireRate : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.ShootTime.AddModifier(_modifier);
    }
}