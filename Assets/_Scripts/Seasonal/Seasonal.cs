using UnityEngine;

public abstract class Seasonal<T, G> : ScriptableObject, ISeasonal where G : ISeasonalSetable<T>
{
    [SerializeField] private SerializedDictionary<string, T> _dictionary = new()
    {
        { "Christmas", default },
    };
    [SerializeField] protected G _toSet;

    public virtual void Set(string season)
    {
        if (!_dictionary.ContainsKey(season))
            return;

        _toSet.SetSeasonal(_dictionary[season]);
    }

    public void Add(string name) => _dictionary.TryAdd(name, default);

    public void AddChristmas() => _dictionary.TryAdd(SeasonNames.CHRISTMAS, default);

    public void AddHalloween() => _dictionary.TryAdd(SeasonNames.HALLOWEEN, default);
}