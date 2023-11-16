using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Range")]
public class PowerUpBulletRange : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Gun.BulletDuration;
    }
}
