using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Shield")]
public class PowerUpShield : PowerUp
{
    [SerializeField] protected int _amount;

    public override void ApplyEffect(Witch witch)
    {
        witch.Health.AddShield(_amount);
    }
}
