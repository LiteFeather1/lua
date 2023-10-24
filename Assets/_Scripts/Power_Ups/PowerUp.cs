using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField, Range(0f, 1f)] protected float _chance;
    [SerializeField] protected int _limit;

    public string Name => _name;
    public float Chance => _chance;
    public int Limit => _limit;

    public abstract void ApplyEffect(Witch witch);
}
