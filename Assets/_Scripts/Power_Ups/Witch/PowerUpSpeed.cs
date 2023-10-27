using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Speed")]
public class PowerUpSpeed : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.AddAccelerationMofifier(_modifier);
    }
}
