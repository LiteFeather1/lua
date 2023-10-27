using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Knockback")]
public class PowerUpKnockback : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.Knockback.AddModifier(_modifier);
    }
}
