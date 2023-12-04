using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Dragon/Adult Dragon")]
public class PowerUpDragonAdult : PowerUp
{
    protected override string Num => "";

    protected override void ApplyEffect(GameManager gm)
    {
        gm.Dragon.Grow(1f);
        Remove(gm.CardManager);
    }
}