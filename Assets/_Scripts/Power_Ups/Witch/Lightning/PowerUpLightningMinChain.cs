using System;
using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Lightining/Lightning Min Chain")]
    public class PowerUpLightningMinChain : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(GameManager gm)
        {
            return gm.Witch.ChangeLightningMinChain;
        }
    }
}
