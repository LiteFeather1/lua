using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Life Steal")]
public class PowerUpLifeSteal : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.LifeStealPercent.AddModifier(_modifier);
    }
}
