using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Crit Chance")]
    public class PowerUpCritChance : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.CritChance;
        }
    }
}
