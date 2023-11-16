using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Pierce")]
public class PowerUpPierce : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.Gun.AddPierce;
    }
}
