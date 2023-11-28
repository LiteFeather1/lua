using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/On Card Played/Damage Enemies")]
public class PowerUpCardPlayedDamageEnemies : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.OnCardPlayedHeal;
    }
}