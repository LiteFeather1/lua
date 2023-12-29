using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Aura/Aura Size")]
    public class PowerUpIncreaseAura : PowerUp
    {
        [Header("Increase Witch Aura")]
        [SerializeField] private int _auraIncrease;

        protected override string Num => (_auraIncrease * 2).ToString();

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            if (cm.GameManager.Witch.Aura.IncreaseAura(_auraIncrease))
                Remove(cm);
        }
    }
}
