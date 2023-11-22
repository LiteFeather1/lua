using UnityEngine;

public abstract class Seasonal<T, G> : ScriptableObject, ISeasonal where G : ISeasonalSetable<T>
{
    [SerializeField] private SerializedDictionary<string, T> _dictionary = new()
    {
        { "Christmas", default },
    };
    [SerializeField] private G _toSet;

    public void Set(string key)
    {
        if (!_dictionary.ContainsKey(key))
            return;

        _toSet.SetSeasonal(_dictionary[key]);
    }

    public void Add(string name) => _dictionary.TryAdd(name, default);

    public void AddChristmas() => _dictionary.TryAdd(SeasonNames.CHRISTMAS, default);

    public void AddHalloween() => _dictionary.TryAdd(SeasonNames.HALLOWEEN, default);
}
