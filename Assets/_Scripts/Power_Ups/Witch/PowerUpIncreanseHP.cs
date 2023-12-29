using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Increase HP")]
    public class PowerUpIncreanseHP : PowerUpFlat
    {
        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Health.IncreaseMaxHP;
        }
    }
}
