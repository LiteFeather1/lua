using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Damage")]
public class PowerUpDamage : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)   
    {
        witch.Gun.Damage.AddModifier(_modifier);
    }
}
