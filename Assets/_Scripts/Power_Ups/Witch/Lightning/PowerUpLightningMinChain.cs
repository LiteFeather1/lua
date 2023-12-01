using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Lightining/Lightning Min Chain")]
public class PowerUpLightningMinChain : PowerUpFlat
{
    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        return gm.Witch.ChangeLightningMinChain;
    }
}
