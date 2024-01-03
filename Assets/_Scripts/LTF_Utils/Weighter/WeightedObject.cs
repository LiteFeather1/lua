using UnityEngine;

namespace LTF.Weighter
{
    [System.Serializable]
    public class WeightedObject<T>
    {
        [SerializeField] private T _object;
        [SerializeField] private float _weight;

        public WeightedObject(float weight = 0f, T @object = default)
        {
            _object = @object;
            _weight = weight;
        }

        public WeightedObject() : this(0f) { }

        public T Object => _object;
        public float Weight => _weight;

        public void SetWeight(float weight) => _weight = weight;

        public static implicit operator T(WeightedObject<T> wo) => wo._object;

        public static implicit operator float (WeightedObject<T> wo) => wo._weight;
    }
}