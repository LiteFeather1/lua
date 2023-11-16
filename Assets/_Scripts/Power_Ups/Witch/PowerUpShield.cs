using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Shield")]
public class PowerUpShield : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.Health.AddShield;
    }
}
