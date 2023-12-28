using System;
using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Heal")]
    public class PowerUpHeal : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(GameManager gm)
        {
            return gm.Witch.Health.Heal;
        }
    }
}