using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Chance To Enemy Explode")]
    public class PowerChanceToEnemyExplode : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.SpawnManager.ChanceDamageExplosion;
        }
    }
}
