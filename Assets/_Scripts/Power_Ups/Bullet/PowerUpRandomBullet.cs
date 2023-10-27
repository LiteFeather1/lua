using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Random Bullet")]
public class PowerUpRandomBullet : PowerUp
{
    [SerializeField] private int _amountToAdd = 1;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddRandomBullet(_amountToAdd);
    }
}
