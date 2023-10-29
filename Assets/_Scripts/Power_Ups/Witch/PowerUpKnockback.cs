using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Knockback")]
public class PowerUpKnockback : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Knockback.AddModifier(_modifier);
    }
}
