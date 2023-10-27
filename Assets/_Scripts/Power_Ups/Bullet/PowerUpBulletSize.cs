using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Size")]
public class PowerUpBulletSize : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.Size.AddModifier(_modifier);
    }
}
