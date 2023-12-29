using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/On Card Played/Heal")]
    public class PowerUpCardPlayedHeal : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.OnCardPlayedHeal;
        }
    }
}
