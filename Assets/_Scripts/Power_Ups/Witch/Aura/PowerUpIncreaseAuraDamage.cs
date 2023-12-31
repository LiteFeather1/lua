using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Aura/Aura Damage")]
    public class PowerUpIncreaseAuraDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Aura.DamagePercent;
        }

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            base.ApplyEffect(cm);
            cm.GameManager.Witch.Aura.DamagePercent.Recalculate();
        }
    }
}