using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Heal")]
public class PowerUpHeal : PowerUp
{
    [SerializeField] protected float _amount;

    public override void ApplyEffect(Witch witch)
    {
        witch.Health.Heal(_amount);
    }
}
