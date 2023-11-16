using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _effect;
    [SerializeField] private int _cost = 4;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private Sprite _icon;

    public string Name => _name;
    public string Efffect => _effect.Replace("$", Num);
    public int Cost => _cost;
    public float Weight => _rarity.Weight;
    public Color RarityColour => _rarity.RarityColour;
    public Sprite Icon => _icon;   

    protected abstract string Num { get; }

    public abstract void ApplyEffect(GameManager gm);
}
