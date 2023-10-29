using UnityEngine;
using System.Collections.Generic;

namespace LTFUtils
{
    [System.Serializable]
    public class Weighter<T> 
    {
        [SerializeField] private List<WeightedObject<T>> _objects = new();
        public List<WeightedObject<T>> Objects => _objects;
        private bool _isDirty = true;
        private float _sumOfWeights;
        public int Count => _objects.Count;

        public float SumOfWeights
        {
            get
            {
                if (_isDirty)
                {
                    _isDirty = false;
                    RecalculateSumOfWeights();
                }

                return _sumOfWeights;
            }
        }

        public void RecalculateSumOfWeights()
        {
            _sumOfWeights = 0f;

            foreach (var object_ in _objects)
                _sumOfWeights += object_.Weight;
        }

        public Weighter(IList<WeightedObject<T>> objects)
        {
            _objects = new(objects);
            _isDirty = true;
            _sumOfWeights = SumOfWeights;
        }

        public Weighter()
        {
            _objects = new();
            _isDirty = true;
            _sumOfWeights = SumOfWeights;
        }

        public WeightedObject<T> GetWeightedObject()
        {
            if (_sumOfWeights < 0.01f)
                _isDirty = true;

            float f = Random.value * SumOfWeights;
            WeightedObject<T> objectToReturn = _objects[0];
            foreach (var object_ in _objects)
            {
                f -= object_.Weight;
                if (!(f <= 0))
                    continue;

                objectToReturn = object_;
                break;
            }
            return objectToReturn;
        }

        public void RemoveObject(WeightedObject<T> objectToRemove)
        {
            _objects.Remove(objectToRemove);
            _isDirty = true;
        }

        public void AddObject(WeightedObject<T> objectToAdd)
        {
            _objects.Add(objectToAdd);
            _isDirty = true;
        }
    }
}