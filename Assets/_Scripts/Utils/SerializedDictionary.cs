using System.Collections.Generic;
using UnityEngine;

namespace LTF
{
    [System.Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Pair> _entries = new();

        public SerializedDictionary() { }

        public SerializedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public void OnBeforeSerialize()
        {
            _entries.Clear();
            foreach (var pair in this)
                _entries.Add(pair);
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var entry in _entries)
                this[entry.Key] = entry.Value;
        }

        [System.Serializable]
        public struct Pair
        {
            public TKey Key;
            public TValue Value;

            public Pair(TKey key, TValue value)
            {
                Key = key; 
                Value = value;
            }

            public static implicit operator KeyValuePair<TKey, TValue>(Pair Pair)
            {
                return new(Pair.Key, Pair.Value);
            }

            public static implicit operator Pair(KeyValuePair<TKey, TValue> pair) 
            {
                return new(pair.Key, pair.Value);
            }
        }
    }
}
