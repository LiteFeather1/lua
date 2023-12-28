using UnityEngine;
using CompositeValues;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/On Card Played/Heal")]
    public class PowerUpCardPlayedHeal : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.OnCardPlayedHeal;
        }
    }
}
