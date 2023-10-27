using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected int _limit;
    [SerializeField] private Sprite _icon;
    private int _pickedAmount;

    public string Name => _name;
    public int Limit => _limit;

    public abstract void ApplyEffect(Witch witch);

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
