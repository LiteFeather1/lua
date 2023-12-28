using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dragon/Junior Dragon")]
    public class PowerUpDragonJunior: PowerUp
    {
        protected override string Num => "";

        protected override void ApplyEffect(GameManager gm)
        {
            gm.Dragon.Grow(.5f);
            Remove(gm.CardManager);
        }
    }
}
