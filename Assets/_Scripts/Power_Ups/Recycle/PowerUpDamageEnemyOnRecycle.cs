using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Damage Enemy")]
public class PowerUpDamageEnemyOnRecycle : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.DamageEnemiesOnRecycle.AddModifier(_modifier);
    }
}