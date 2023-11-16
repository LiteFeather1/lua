using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Heal")]
public class PowerUpHealOnRecycle : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.HealOnRecycle;
    }
}