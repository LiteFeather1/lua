using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Size")]
public class PowerUpSBulletSize : PowerUp
{
    [SerializeField] private CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.Gun.Size.AddModifier(_modifier);
    }
}
