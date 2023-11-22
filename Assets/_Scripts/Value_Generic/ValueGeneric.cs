using UnityEngine;

public abstract class ValueGeneric<T> : ScriptableObject, ISeasonalSetable<T>
{
    [field: SerializeField] public T Value { get; set; }

    public void SetSeasonal(T value) => Value = value;
}
