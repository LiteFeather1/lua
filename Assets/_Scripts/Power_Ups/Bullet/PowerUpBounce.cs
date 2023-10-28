using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Bounce")]
public class PowerUpBounce : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddBounce(_amount);
    }
}
