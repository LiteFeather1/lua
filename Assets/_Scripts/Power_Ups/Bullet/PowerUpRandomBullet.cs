using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Random Bullet")]
public class PowerUpRandomBullet : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.AddRandomBullet(_amount);
    }
}
