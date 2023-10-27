using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _effect;
    [SerializeField] private float _weight;
    [SerializeField] private int _limit;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Color _rarityColour = Color.white;
    private int _pickedAmount;

    public string Name => _name;
    public string Efffect => _effect.Replace("%", Num);
    public float Weight => _weight;
    public Sprite Icon => _icon;   
    public Color RarityColour => _rarityColour;

    protected abstract string Num { get; }

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
