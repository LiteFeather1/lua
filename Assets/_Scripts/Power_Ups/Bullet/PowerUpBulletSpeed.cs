using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Speed")]
public class PowerUpBulletSpeed : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.Gun.BulletSpeed.AddModifier(_modifier);
    }
}
