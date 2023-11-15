using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Enemy Explosion/Chance To Enemy Explode")]
public class PowerChanceToEnemyExplode : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.SpawnManager.ChanceDamageExplosion.AddModifier(_modifier);
    }
}
