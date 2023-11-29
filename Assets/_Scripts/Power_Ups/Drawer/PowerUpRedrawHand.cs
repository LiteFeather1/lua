using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Drawer/Redraw Hand")]
public class PowerUpRedrawHand : PowerUp
{
    protected override string Num => string.Empty;

    protected override void ApplyEffect(GameManager gm)
    {
        gm.StartCoroutine(gm.CardManager.RedrawHand());
    }
}
