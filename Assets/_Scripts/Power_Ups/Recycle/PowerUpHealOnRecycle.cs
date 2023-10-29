using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Heal")]
public class PowerUpHealOnRecycle : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.HealOnRecycle.AddModifier(_modifier);
    }
}