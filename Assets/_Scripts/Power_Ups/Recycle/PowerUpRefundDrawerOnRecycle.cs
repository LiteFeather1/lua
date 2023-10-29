using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Refund Drawer")]
public class PowerUpRefundDrawerOnRecycle : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.CardManager.RefundOnDiscard.AddModifier(_modifier);
    }
}
