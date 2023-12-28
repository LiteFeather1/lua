using UnityEngine;
using CompositeValues;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Explosion Damage")]
    public class PowerEnemyExplosionDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.SpawnManager.ExplosionDamage;
        }
    }
}
