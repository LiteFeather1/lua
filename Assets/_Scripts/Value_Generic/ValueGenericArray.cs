public class ValueGenericArray<T> : ValueGeneric<T[]>
{
    public T this[int index]
    {
        get => Value[index];
        set => Value[index] = value;
    }

    public int Length => Value.Length;
}
