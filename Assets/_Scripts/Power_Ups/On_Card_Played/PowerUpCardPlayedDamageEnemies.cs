using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/On Card Played/Damage Enemies")]
    public class PowerUpCardPlayedDamageEnemies : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.OnCardPlayedDamageEnemies;
        }
    }
}
