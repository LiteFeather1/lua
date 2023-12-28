using UnityEngine;
using CompositeValues;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Recycle/Damage Enemy")]
    public class PowerUpDamageEnemyOnRecycle : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.OnRecycleDamageEnemies;
        }
    }
}
