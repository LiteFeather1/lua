using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bounce")]
public class PowerUpBounce : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.Gun.AddBounce;
    }
}
