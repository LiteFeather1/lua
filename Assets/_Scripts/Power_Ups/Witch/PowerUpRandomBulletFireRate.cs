using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Random Bullet Fire Rate")]
public class PowerUpRandomBulletFireRate : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.RandomBulletShootTime.AddModifier(_modifier);
    }
}