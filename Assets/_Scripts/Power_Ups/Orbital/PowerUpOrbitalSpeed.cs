using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Orbital/Rotation Speed")]
    public class PowerUpOrbitalSpeed : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.OrbitalGun.RotationSpeed;
        }
    }
}
