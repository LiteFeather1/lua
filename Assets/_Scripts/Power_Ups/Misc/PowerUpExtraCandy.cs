using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Misc/Extra Candy")]
public class PowerUpExtraCandy : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.DamageEnemiesOnRecycle.AddModifier(_modifier);
    }
}
