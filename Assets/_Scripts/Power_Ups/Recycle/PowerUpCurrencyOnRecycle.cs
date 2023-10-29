using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Gain Currency")]
public class PowerUpCurrencyOnRecycle : PowerUpModifier
{
    public override void ApplyEffect(GameManager gm)
    {
        gm.AddCurrencyOnRecycle.AddModifier(_modifier);
    }
}
