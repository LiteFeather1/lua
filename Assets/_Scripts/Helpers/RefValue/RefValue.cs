using UnityEngine;

namespace LTF.RefValue
{
    [System.Serializable]
    public class RefValue<T> : IRefValue<T>
    {
        [SerializeField] private T _value;

        public T Value { get => _value; set => _value = value; }

        public RefValue() { }

        public RefValue(T value) => _value = value;

        public static implicit operator T(RefValue<T> v) => v._value; 
    }
}
