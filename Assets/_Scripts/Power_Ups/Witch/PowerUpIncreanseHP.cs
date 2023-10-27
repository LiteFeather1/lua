using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Increase HP")]
public class PowerUpIncreanseHP : PowerUp
{
    [SerializeField] protected float _amount;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Health.IncreaseMaxHP(_amount);
    }
}
