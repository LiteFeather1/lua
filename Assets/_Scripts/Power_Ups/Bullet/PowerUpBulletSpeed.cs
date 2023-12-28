using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Speed")]
    public class PowerUpBulletSpeed : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.Gun.BulletSpeed;
        }
    }
}
