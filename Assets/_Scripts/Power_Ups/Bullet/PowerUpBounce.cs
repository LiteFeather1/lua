using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Burst")]
public class PowerUpBounce : PowerUp
{
    [SerializeField] private int _amountToAdd = 1;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddBounce(_amountToAdd);
    }
}
