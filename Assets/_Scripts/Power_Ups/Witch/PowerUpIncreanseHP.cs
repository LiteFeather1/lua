using System;
using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Increase HP")]
    public class PowerUpIncreanseHP : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(GameManager gm)
        {
            return gm.Witch.Health.IncreaseMaxHP;
        }
    }
}
