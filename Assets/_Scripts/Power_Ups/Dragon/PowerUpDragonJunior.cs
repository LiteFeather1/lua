using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dragon/Junior Dragon")]
    public class PowerUpDragonJunior: PowerUp
    {
        protected override string Num => "";

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            cm.GameManager.Dragon.Grow(.5f);
            Remove(cm);
        }
    }
}
