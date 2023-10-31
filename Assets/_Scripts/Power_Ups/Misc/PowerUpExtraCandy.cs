using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Misc/Extra Candy")]
public class PowerUpExtraCandy : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.SpawnManager.ChanceToExtraCandy.AddModifier(_modifier);
    }
}
