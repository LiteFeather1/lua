using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Speed")]
public class PowerUpBulletSpeed : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Gun.BulletSpeed;
    }
}
