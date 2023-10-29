using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Refund Drawer")]
public class PowerUpRefundDrawerOnRececly : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.CardManager.RefundOnDiscard.AddModifier(_modifier);
        gm.CardManager.RefundCooldown();
    }
}