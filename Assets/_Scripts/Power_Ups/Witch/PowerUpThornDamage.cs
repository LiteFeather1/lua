using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Thorn Damage")]
public class PowerUpThornDamage : PowerUpModifier
{
    [Header(nameof(PowerUpThornDamage))]
    [SerializeField, TextArea] private string _unlockText = "When Taking Damage, Damage Enemies Around Lua \n Based on Lua's Defence and Shields";
    [SerializeField] private CompositeValueModifier _defenceDamageMultiplier;

    private static bool _picked = false;

    public override string Effect => _picked ? base.Effect.Replace("$2", $"+{_defenceDamageMultiplier.Value * 100f:0.00}%") : _unlockText;

    public override void Reset()
    {
        base.Reset();
        _picked = false;
    }

    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.ThornBaseDamage;
    }

    protected override void ApplyEffect(GameManager gm)
    {
        base.ApplyEffect(gm);
        _picked = true;
        gm.Witch.ThornDefenceDamageMultiplier.AddModifier(_defenceDamageMultiplier);
    }
}
