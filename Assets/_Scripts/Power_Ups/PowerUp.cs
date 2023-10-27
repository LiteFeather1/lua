using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _effect;
    [SerializeField] private float _chance;
    [SerializeField] protected int _limit;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Color _rarityColour = Color.white;
    private int _pickedAmount;

    public string Name => _name;
    public string Efffect => _effect.Replace("%", Num);
    public float Chance => _chance;
    public Sprite Icon => _icon;   
    public Color RarityColour => _rarityColour;

    protected virtual string Num { get; }

    public abstract void ApplyEffect(GameManager gm);

    public void Reset()
    {
        _pickedAmount = 0;
    }

    public bool IncreasePickedAmount()
    {
        _pickedAmount++;
        return _pickedAmount >= _limit && _limit != 0;
    }
}
