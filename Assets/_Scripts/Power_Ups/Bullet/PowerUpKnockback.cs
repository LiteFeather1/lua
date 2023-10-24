using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Knockback")]
public class PowerUpKnockback : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.Gun.Knockback.AddModifier(_modifier);
    }
}
