using UnityEngine;

public abstract class ValueGeneric<T> : ScriptableObject
{
    [field: SerializeField] public float Value { get; private set; }
}
