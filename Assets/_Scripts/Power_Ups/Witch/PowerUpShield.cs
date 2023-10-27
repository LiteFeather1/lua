using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Shield")]
public class PowerUpShield : PowerUp
{
    [SerializeField] protected int _amount;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Health.AddShield(_amount);
    }
}
