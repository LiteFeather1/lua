using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dragon/Teen Dragon")]
    public class PowerUpDragonTeen : PowerUp
    {
        protected override string Num => "";

        protected override void ApplyEffect(GameManager gm)
        {
            gm.Dragon.Grow(.75f);
            Remove(gm.CardManager);
        }
    }
}
