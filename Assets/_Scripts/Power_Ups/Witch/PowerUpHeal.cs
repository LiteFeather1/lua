using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Heal")]
public class PowerUpHeal : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.Health.Heal;
    }
}