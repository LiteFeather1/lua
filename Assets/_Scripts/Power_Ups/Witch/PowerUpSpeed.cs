using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Speed")]
public class PowerUpSpeed : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Acceleration;
    }

    protected override void ApplyEffect(GameManager gm)
    {
        base.ApplyEffect(gm);
        gm.Witch.EvaluateDrag();
    }
}
