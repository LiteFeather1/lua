using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Burst")]
public class PowerUpBounce : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddBounce(_amount);
    }
}
