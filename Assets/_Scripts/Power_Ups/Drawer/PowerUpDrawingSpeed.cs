using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Drawer/Drawing Speed")]
public class PowerUpDrawingSpeed : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.CardManager.TimeToDrawCard.AddModifier(_modifier);
    }
}
