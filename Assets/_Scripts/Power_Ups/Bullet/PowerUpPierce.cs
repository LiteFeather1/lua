using System;
using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Bullet/Pierce")]
    public class PowerUpPierce : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(GameManager gm)
        {
            return gm.Witch.Gun.AddPierce;
        }
    }
}
