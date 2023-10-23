﻿using UnityEngine;

namespace LTFUtils
{
    [System.Serializable]
    public class WeightedObject<T>
    {
        [SerializeField] private T _object;
        [SerializeField] private float _weight;

        public WeightedObject(T @object, float weight)
        {
            _object = @object;
            _weight = weight;
        }

        public WeightedObject() { }

        public T Object => _object;
        public float Weight => _weight;
    }
}