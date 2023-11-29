using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Damage Enemy")]
public class PowerUpDamageEnemyOnRecycle : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.OnRecycleDamageEnemies;
    }
}
