using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Explosion Damage")]
public class PowerEnemyExplosionDamage : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.SpawnManager.ExplosionDamage;
    }
}
