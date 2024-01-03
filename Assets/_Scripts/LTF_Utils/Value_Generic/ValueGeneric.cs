using UnityEngine;

namespace LTF.ValueGeneric
{
    public abstract class ValueGeneric<T> : ScriptableObject, ISetable<T>
    {
        [field: SerializeField] public T Value { get; set; }

        public void Set(T value) => Value = value;

        public static implicit operator T(ValueGeneric<T> v) => v.Value;
    }
}
