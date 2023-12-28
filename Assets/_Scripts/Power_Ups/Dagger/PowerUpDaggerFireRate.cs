using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dagger/Fire Rate")]
    public class PowerUpDaggerFireRate : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.DaggerShootTime;
        }
    }
}
