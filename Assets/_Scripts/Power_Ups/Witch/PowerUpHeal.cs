using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Heal")]
    public class PowerUpHeal : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Health.Heal;
        }
    }
}