using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Explosion Damage")]
public class PowerEnemyExplosionDamage : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.SpawnManager.ExplosionDamage.AddModifier(_modifier);
    }
}
