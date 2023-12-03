using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Orbital/Amount")]
public class PowerUpOrbitalBulletAmount : PowerUpFlat
{
    [Header("Orbital Bullet Amount")]
    [SerializeField] private string _unlockText = "Unlock Orbital Bullet";
    private static bool _unlocked = false;

    public override string Effect => _unlocked ? base.Effect : _unlockText;

    public override void Reset()
    {
        base.Reset();
        _unlocked = false;
    }

    protected override Func<int, int> ModifyValue(GameManager gm)
    {
        _unlocked = true;
        return gm.Witch.OrbitalGun.AddOrbitalAmount;
    }
}
