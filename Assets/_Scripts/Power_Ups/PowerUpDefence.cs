using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Defence")]
public class PowerUpDefence : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.Health.Defence.AddModifier(_modifier);
    }
}
