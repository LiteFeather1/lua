using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Speed")]
public class PowerUpSpeed : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.AddAccelerationMofifier(_modifier);
    }
}
