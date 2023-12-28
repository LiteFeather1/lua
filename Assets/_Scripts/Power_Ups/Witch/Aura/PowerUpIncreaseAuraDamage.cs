using UnityEngine;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Aura/Aura Damage")]
    public class PowerUpIncreaseAuraDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.Aura.DamagePercent;
        }

        protected override void ApplyEffect(GameManager gm)
        {
            base.ApplyEffect(gm);
            gm.Witch.Aura.DamagePercent.ForceRecalculate();
        }
    }
}