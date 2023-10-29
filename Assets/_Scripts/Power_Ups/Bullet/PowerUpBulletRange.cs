using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Range")]
public class PowerUpBulletRange : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.BulletDuration.AddModifier(_modifier);
    }
}
