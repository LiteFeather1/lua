using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Heal")]
public class PowerUpHeal : PowerUp
{
    [SerializeField] protected float _amount;

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Health.Heal(_amount);
    }
}
