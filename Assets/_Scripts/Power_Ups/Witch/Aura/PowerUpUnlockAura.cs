using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Aura/Aura Unlock")]
    public class PowerUpUnlockAura : PowerUp
    {
        [Header("Unlock Witch Aura")]
        [SerializeField] private Sprite _auraSprite;

        protected override string Num => "";

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            cm.GameManager.Witch.Aura.SetAura(_auraSprite);
            cm.GameManager.Witch.Damage.Recalculate();
            cm.GameManager.Witch.CritChance.Recalculate();
            cm.GameManager.Witch.CritMultiplier.Recalculate();
            Remove(cm);
        }
    }
}
