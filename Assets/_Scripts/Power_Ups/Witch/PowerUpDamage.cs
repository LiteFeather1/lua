using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Damage")]
public class PowerUpDamage : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Damage;
    }
}
