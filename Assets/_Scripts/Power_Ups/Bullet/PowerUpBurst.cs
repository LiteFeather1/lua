using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Burst")]
public class PowerUpBurst : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.Gun.AddBurst;
    }
}
