using System;
using UnityEngine;

namespace LTF
{
    [Serializable]
    public struct OptionalValue<T> : IEquatable<OptionalValue<T>>
    {
        [SerializeField] private T _value;
        [SerializeField] private bool _enabled;

        public readonly bool Enabled => _enabled;
        public readonly T Value => _value;

        public OptionalValue(T initialValue)
        {
            _value = initialValue;
            _enabled = true;
        }

        public OptionalValue(T inivialValue, bool enabled)
        {
            _value = inivialValue;
            _enabled = enabled;
        }

        public OptionalValue(bool enabled)
        {
            _value = default;
            _enabled = enabled;
        }

        public readonly T GetValue() => _enabled ? _value : default;

        #region Operators
        public static implicit operator OptionalValue<T>(T v) => new(v);

        public static implicit operator T(OptionalValue<T> o) => o._value;

        public static implicit operator bool(OptionalValue<T> o) => o.Enabled;

        public static bool operator ==(OptionalValue<T> left, OptionalValue<T> right)
        {
            if (left._value is null)
            {
                if (right._value is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles the case of null on right side.
            return left._value.Equals(right._value);
        }

        public static bool operator !=(OptionalValue<T> lhs, OptionalValue<T> rhs) => !(lhs == rhs);

        public override readonly bool Equals(object obj)
        {
            // return base.Equals(obj);
            return _value.Equals(obj);
        }

        public readonly bool Equals(OptionalValue<T> other) => _value.Equals(other);

        public override readonly int GetHashCode() => _value.GetHashCode();

        public override readonly string ToString() => _value.ToString();

        #endregion
    }
}
