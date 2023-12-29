using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Size")]
    public class PowerUpBulletSize : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Gun.Size;
        }
    }
}
