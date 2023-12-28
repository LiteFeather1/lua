using UnityEngine;

public abstract class ValueGeneric<T> : ScriptableObject, Seasonal.ISeasonalSetable<T>
{
    [field: SerializeField] public T Value { get; set; }

    public void SetSeasonal(T value) => Value = value;

    public static implicit operator T(ValueGeneric<T> genericValue) => genericValue.Value;
}
