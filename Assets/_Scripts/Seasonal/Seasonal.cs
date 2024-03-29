using UnityEngine;
using LTF;
using LTF.ValueGeneric;

namespace Seasonal
{
    public abstract class Seasonal<T, G> : ScriptableObject, ISeasonal where G : ISetable<T>
    {
        [SerializeField] protected T _default;
#if UNITY_EDITOR
        public void SetDefault() => _toSet.Set(_default);
#endif

        [SerializeField] protected SerializedDictionary<string, T> _dictionary = new()
        {
            { "Christmas", default },
        };
        [SerializeField] protected G _toSet;

        public T Default => _default;
        public System.Collections.Generic.IDictionary<string, T> Dictionary => _dictionary;
        public G ToSet => _toSet;

        public virtual void Set(string season)
        {
            if (!_dictionary.ContainsKey(season))
                return;

            _toSet.Set(_dictionary[season]);
        }

        public void Add(string name) => _dictionary.TryAdd(name, default);

        public void AddChristmas() => _dictionary.TryAdd(SeasonNames.CHRISTMAS, default);

        public void AddHalloween() => _dictionary.TryAdd(SeasonNames.HALLOWEEN, default);
    }
}