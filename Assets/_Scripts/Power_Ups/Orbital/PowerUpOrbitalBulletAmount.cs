using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Orbital/Amount")]
public class PowerUpOrbitalBulletAmount : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.OrbitalGun.AddOrbitalAmount;
    }
}
