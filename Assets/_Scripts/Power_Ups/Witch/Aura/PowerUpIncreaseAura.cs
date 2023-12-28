using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Aura/Aura Size")]
    public class PowerUpIncreaseAura : PowerUp
    {
        [Header("Increase Witch Aura")]
        [SerializeField] private int _auraIncrease;

        protected override string Num => (_auraIncrease * 2).ToString();

        protected override void ApplyEffect(GameManager gm)
        {
            if (gm.Witch.Aura.IncreaseAura(_auraIncrease))
                Remove(gm.CardManager);
        }
    }
}
