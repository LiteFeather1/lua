using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Crit Damage")]
public class PowerUpCritDamage : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.CritMultiplier;
    }
}
