using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Speed")]
public class PowerUpSpeed : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.AddAccelerationMofifier(_modifier);
    }
}
