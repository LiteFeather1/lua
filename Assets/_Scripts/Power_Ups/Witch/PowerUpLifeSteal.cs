using UnityEngine;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Life Steal")]
    public class PowerUpLifeSteal : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.LifeStealPercent;
        }
    }
}
