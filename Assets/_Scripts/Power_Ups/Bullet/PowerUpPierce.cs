using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Pierce")]
public class PowerUpPierce : PowerUpFlat
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddPierce(_amount);
    }
}
