using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Burst")]
public class PowerUpBurst : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddBurst(_amount);
    }
}
