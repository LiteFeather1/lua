using UnityEngine;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Life Steal Chance")]
    public class PowerUpLifeStealChance : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.ChanceToLifeSteal;
        }
    }
}
