using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Fire Rate")]
public class PowerUpFireRate : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.ShootTime.AddModifier(_modifier);
    }
}