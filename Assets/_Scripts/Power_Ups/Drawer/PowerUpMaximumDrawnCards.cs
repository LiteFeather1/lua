using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Drawer/Maximum Cards")]
public class PowerUpMaximumDrawnCards : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.CardManager.AddCard;
    }
}
