using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Amount")]
public class PowerUpBulletAmount : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.Gun.AddBulletAmount;
    }
}
