using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Recycle/Gain Currency")]
public class PowerUpCurrencyOnRecycle : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.AddCurrencyOnRecycle;
    }
}
