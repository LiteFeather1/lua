using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Fire/Damage Mulplier")]
    public class PowerUpFireDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.EffectCreatorFire.DamageMultiplier;
        }
    }
}
