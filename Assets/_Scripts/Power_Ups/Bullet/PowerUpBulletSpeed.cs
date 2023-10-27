using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Speed")]
public class PowerUpBulletSpeed : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.BulletSpeed.AddModifier(_modifier);
    }
}
