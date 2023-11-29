using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Random Bullet Fire Rate")]
public class PowerUpRandomBulletFireRate : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.RandomBulletShootTime;
    }
}
