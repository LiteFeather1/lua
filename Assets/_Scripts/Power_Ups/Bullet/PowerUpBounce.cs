using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Burst")]
public class PowerUpBounce : PowerUp
{
    [SerializeField] private int _amountToAdd = 1;

    public override void ApplyEffect(Witch witch)
    {
        witch.Gun.AddBounce(_amountToAdd);
    }
}
