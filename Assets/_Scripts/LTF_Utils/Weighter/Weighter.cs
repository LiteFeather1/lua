using System;
using UnityEngine;
using System.Collections.Generic;

namespace LTF.Weighter
{
    [Serializable]
    public class Weighter<T> : IDisposable
    {
        [SerializeField] private List<WeightedObject<T>> _objects = new();

        private bool _isDirty = true;
        private float _sumOfWeights;

        public List<WeightedObject<T>> Objects => _objects;
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

        public Weighter(IEnumerable<WeightedObject<T>> objects)
        {
            _objects = new(objects);
            _isDirty = true;
        }

        public Weighter() : this(new WeightedObject<T>[0]) { }

        ~Weighter() 
        {
            Dispose();
        }

        public void RecalculateSumOfWeights()
        {
            _sumOfWeights = 0f;
            for (int i = 0; i < _objects.Count; i++)
                _sumOfWeights += _objects[i];
        }

        public WeightedObject<T> GetWeightedObject()
        {
            var f = UnityEngine.Random.value * SumOfWeights;
            for (int i = 0; i < _objects.Count; i++)
            {
                if ((f -= _objects[i]) <= 0f)
                    return _objects[i];
            }

            return _objects[0];
        }

        public T GetObject() => GetWeightedObject();

        public void AddObject(WeightedObject<T> objectToAdd)
        {
            _objects.Add(objectToAdd);
            _isDirty = true;
        }

        public void AddRange(IEnumerable<WeightedObject<T>> objectsToAdd)
        {
            _objects.AddRange(objectsToAdd); 
            _isDirty = true;
        }

        public void RemoveObject(WeightedObject<T> objectToRemove)
        {
            _objects.Remove(objectToRemove);
            _isDirty = true;
        }

        public void Dispose()
        {
            _objects.Clear();
        }
    }
}