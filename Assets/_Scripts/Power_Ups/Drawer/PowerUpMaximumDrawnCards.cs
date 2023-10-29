using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Drawer/Maximum Cards")]
public class PowerUpMaximumDrawnCards : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.CardManager.AddCard();
    }
}
