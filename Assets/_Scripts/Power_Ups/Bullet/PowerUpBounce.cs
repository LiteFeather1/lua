using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Bullet/Bounce")]
    public class PowerUpBounce : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Gun.AddBounce;
        }
    }
}
