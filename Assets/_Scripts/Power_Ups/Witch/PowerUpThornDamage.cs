using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Thorn Damage")]
    public class PowerUpThornDamage : PowerUpModifier
    {
        [Header(nameof(PowerUpThornDamage))]
        [SerializeField, TextArea] private string _unlockText = "When Taking Damage, Damage Enemies Around Lua \n Based on Lua's Defence and Shields";
        [SerializeField] private CompositeValueModifier _defenceDamageMultiplier;

        private static bool _picked = false;

        public override string Effect => _picked ? base.Effect.Replace("$2", _defenceDamageMultiplier.Value.ToString("+0%; -#%")) : _unlockText;

        public override void Reset()
        {
            base.Reset();
            _picked = false;
        }

        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.ThornBaseDamage;
        }

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            base.ApplyEffect(cm);
            _picked = true;
            cm.GameManager.Witch.ThornDefenceDamageMultiplier.AddModifier(_defenceDamageMultiplier);
        }
    }
}
