using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Life Steal Chance")]
public class PowerUpLifeStealChance : PowerUp
{
    [SerializeField] protected CompositeValueModifier _modifier;

    public override void ApplyEffect(Witch witch)
    {
        witch.ChanceToLifeSteal.AddModifier(_modifier);
    }
}
