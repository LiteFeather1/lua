using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Increase HP")]
public class PowerUpIncreanseHP : PowerUp
{
    [SerializeField] protected float _amount;

    public override void ApplyEffect(Witch witch)
    {
        witch.Health.IncreaseMaxHP(_amount);
    }
}
