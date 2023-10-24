using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Pierce")]
public class PowerUpPierce : PowerUp
{
    [SerializeField] private int _amountToAdd = 1;

    public override void ApplyEffect(Witch witch)
    {
        witch.Gun.AddPierce(_amountToAdd);
    }
}
