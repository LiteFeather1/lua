using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Size")]
public class PowerUpBulletSize : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Gun.Size;
    }
}
