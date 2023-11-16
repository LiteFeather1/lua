using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Fire Rate")]
public class PowerUpFireRate : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.ShootTime;
    }
}
