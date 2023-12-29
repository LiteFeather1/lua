using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dragon/Adult Dragon")]
    public class PowerUpDragonAdult : PowerUp
    {
        protected override string Num => "";

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            cm.GameManager.Dragon.Grow(1f);
            Remove(cm);
        }
    }
}