using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dragon/Teen Dragon")]
    public class PowerUpDragonTeen : PowerUp
    {
        protected override string Num => "";

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            cm.GameManager.Dragon.Grow(.75f);
            Remove(cm);
        }
    }
}
