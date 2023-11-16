using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Knockback 1")]
public class PowerUpKnockback : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Knockback;
    }
}
