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

    #region Operator
    public static implicit operator float(CompositeValueModifier a) => a._value;

    public static bool operator >(CompositeValueModifier lhs, CompositeValueModifier rhs) => lhs._value > rhs._value;
    public static bool operator >(CompositeValueModifier lhs, float rhs) => lhs._value > rhs;
    public static bool operator >(float lhs, CompositeValueModifier rhs) => lhs > rhs._value;

    public static bool operator <(CompositeValueModifier lhs, CompositeValueModifier rhs) => lhs._value < rhs._value;
    public static bool operator <(CompositeValueModifier lhs, float rhs) => lhs._value < rhs;
    public static bool operator <(float lhs, CompositeValueModifier rhs) => lhs < rhs._value;

    public static bool operator >=(CompositeValueModifier lhs, CompositeValueModifier rhs) => lhs._value >= rhs._value;
    public static bool operator >=(CompositeValueModifier lhs, float rhs) => lhs._value >= rhs;
    public static bool operator >=(float lhs, CompositeValueModifier rhs) => lhs >= rhs._value;

    public static bool operator <=(CompositeValueModifier lhs, CompositeValueModifier rhs) => lhs._value <= rhs._value;
    public static bool operator <=(CompositeValueModifier lhs, float rhs) => lhs._value <= rhs;
    public static bool operator <=(float lhs, CompositeValueModifier rhs) => lhs <= rhs._value;

    #endregion
}
