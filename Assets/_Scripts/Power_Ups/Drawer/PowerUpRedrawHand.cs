using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Drawer/Redraw Hand")]
    public class PowerUpRedrawHand : PowerUp
    {
        protected override string Num => string.Empty;

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            cm.StartCoroutine(cm.RedrawHand());
        }
    }
}
