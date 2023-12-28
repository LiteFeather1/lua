using UnityEngine;
using CompositeValues;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Chance To Enemy Explode")]
    public class PowerChanceToEnemyExplode : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.SpawnManager.ChanceDamageExplosion;
        }
    }
}
