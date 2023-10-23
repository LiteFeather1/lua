using UnityEngine;

[System.Serializable]
public class CompositeValueModifier
{
    [SerializeField] private float _value = 0f;
    [SerializeField] private CompositeValueModifierType _type = CompositeValueModifierType.None;
    //Maybe a GUID?
    private object _source;

    public float Value => _value;
    public void SetValue(float value) => _value = value;
    public CompositeValueModifierType Type => _type;
    public int Order => (int)_type;
    public object Source { get => _source;}
    public void SetSource(object source) => _source = source;

    public CompositeValueModifier()
    {
        _value = 0;
        _type = CompositeValueModifierType.Flat;
    }

    public CompositeValueModifier(float value, CompositeValueModifierType type, object source)
    {
        _value = value;
        _type = type;
        _source = source;
    }

    public CompositeValueModifier(float value, CompositeValueModifierType type) : this(value, type, null) { }

    public bool IsValidModifier()
    {
        return _value != 0 && _type != CompositeValueModifierType.None;
    }
}
