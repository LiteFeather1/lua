using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Life Steal")]
public class PowerUpLifeSteal : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.LifeStealPercent.AddModifier(_modifier);
    }
}
