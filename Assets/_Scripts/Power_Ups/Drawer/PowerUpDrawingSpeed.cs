using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Drawer/Drawing Speed")]
    public class PowerUpDrawingSpeed : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.TimeToDrawCard;
        }
    }
}
