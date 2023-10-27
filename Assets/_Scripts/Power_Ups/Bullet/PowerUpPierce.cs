using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Pierce")]
public class PowerUpPierce : PowerUp
{
    [SerializeField] private int _amountToAdd = 1;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Gun.AddPierce(_amountToAdd);
    }
}
