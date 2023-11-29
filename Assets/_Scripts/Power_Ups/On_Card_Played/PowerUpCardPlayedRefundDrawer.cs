using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/On Card Played/Refund Drawer")]
public class PowerUpCardPlayedRefundDrawer : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.OnCardPlayedRefund;
    }
}