using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Drawer/Drawing Speed")]
public class PowerUpDrawingSpeed : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.CardManager.TimeToDrawCard;
    }
}
