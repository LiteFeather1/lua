using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Lightining/Lightning Chance")]
    public class PowerUpLightningChance : PowerUpModifier
    {
        [Header("Power Up Lightining Chance")]
        [SerializeField, TextArea] private string _unlockText;
        private static bool _unlocked;

        public override string Effect => _unlocked ? base.Effect : _unlockText;

        public override void Reset()
        {   
            base.Reset();
            _unlocked = false;
        }

        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            _unlocked = true;
            return cm.GameManager.Witch.LightningChance;
        }
    }
}
