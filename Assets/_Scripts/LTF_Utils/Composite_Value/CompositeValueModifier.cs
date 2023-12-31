using UnityEngine;

namespace LTF.CompositeValue
{
    [System.Serializable]
    public class CompositeValueModifier
    {
        [SerializeField] private float _value = 0f;
        [SerializeField] private CompositeValueModifierType _type = CompositeValueModifierType.Flat;
        private object _source;

        public float Value => _value;
        public CompositeValueModifierType Type => _type;
        public int Order => (int)_type;
        public object Source => _source;

        public void SetValue(float value) => _value = value;
        public void SetSource(object source) => _source = source;

        public CompositeValueModifier(
            float value = 0f,
            CompositeValueModifierType type = CompositeValueModifierType.Flat,
            object source = null)
        {
            _value = value;
            _type = type;
            _source = source;
        }

        public CompositeValueModifier() : this(0f) { }

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
}
