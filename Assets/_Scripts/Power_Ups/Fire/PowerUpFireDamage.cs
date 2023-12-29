using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Fire/Damage Mulplier")]
    public class PowerUpFireDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.EffectCreatorFire.DamageMultiplier;
        }
    }
}
