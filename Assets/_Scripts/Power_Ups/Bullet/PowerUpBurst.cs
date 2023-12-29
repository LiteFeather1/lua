using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Bullet/Burst")]
    public class PowerUpBurst : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Gun.AddBurst;
        }
    }
}
