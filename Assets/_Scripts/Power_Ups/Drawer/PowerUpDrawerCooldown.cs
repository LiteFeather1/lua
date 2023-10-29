using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Drawer/Cooldown")]
public class PowerUpDrawerCooldown : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.CardManager.TimeToDrawCard.AddModifier(_modifier);
    }
}
