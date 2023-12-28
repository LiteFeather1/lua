using UnityEngine;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/On Card Played/Damage Enemies")]
    public class PowerUpCardPlayedDamageEnemies : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.OnCardPlayedDamageEnemies;
        }
    }
}
