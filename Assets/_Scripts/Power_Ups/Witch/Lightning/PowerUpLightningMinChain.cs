using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Lightining/Lightning Min Chain")]
    public class PowerUpLightningMinChain : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.ChangeLightningMinChain;
        }
    }
}
