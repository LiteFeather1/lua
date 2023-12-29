using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/On Card Played/Refund Drawer")]
    public class PowerUpCardPlayedRefundDrawer : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.OnCardPlayedRefund;
        }
    }
}