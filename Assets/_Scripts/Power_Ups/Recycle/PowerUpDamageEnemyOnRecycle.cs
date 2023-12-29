using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Recycle/Damage Enemy")]
    public class PowerUpDamageEnemyOnRecycle : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.OnRecycleDamageEnemies;
        }
    }
}
