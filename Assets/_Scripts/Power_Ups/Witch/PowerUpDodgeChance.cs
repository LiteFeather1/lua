﻿using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Dodge Ch]ance")]
public class PowerUpDodgeChance : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.CritChance;
    }
}
