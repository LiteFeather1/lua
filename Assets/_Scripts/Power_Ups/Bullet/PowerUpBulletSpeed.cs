using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bullet Speed")]
public class PowerUpBulletSpeed : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.BulletSpeed.AddModifier(_modifier);
    }
}
