using UnityEngine;
using LTF.CompositeValue;
    
namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Recycle/Refund Drawer")]
    public class PowerUpRefundDrawerOnRecycle : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.OnRecycleRefund;
        }
    }
}
