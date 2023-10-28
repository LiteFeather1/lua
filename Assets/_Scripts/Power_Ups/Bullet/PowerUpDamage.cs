using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Bullet/Damage")]
public class PowerUpDamage : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)   
    {
        gm.Witch.Damage.AddModifier(_modifier);
    }
}
