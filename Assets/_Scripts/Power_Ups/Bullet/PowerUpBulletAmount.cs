using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Amount")]
public class PowerUpBulletAmount : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddBulletAmount(_amount);
    }
}
