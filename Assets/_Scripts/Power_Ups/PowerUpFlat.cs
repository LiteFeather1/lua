using UnityEngine;

public abstract class PowerUpFlat : PowerUp
{
    [SerializeField] protected int _amount;

    protected override string Num => $"+{_amount}";
}
