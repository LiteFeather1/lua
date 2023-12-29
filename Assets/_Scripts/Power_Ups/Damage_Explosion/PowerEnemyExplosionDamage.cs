using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Explosion Damage")]
    public class PowerEnemyExplosionDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.SpawnManager.ExplosionDamage;
        }
    }
}
