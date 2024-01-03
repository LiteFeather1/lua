
namespace LTF.ValueGeneric
{
    public class ValueGenericArray<T> : ValueGeneric<T[]>
    {
        public T this[int index] { get => Value[index]; set => Value[index] = value; }

        public int Length => Value.Length;

        public T PickRandom() => Value[UnityEngine.Random.Range(0, Value.Length)];
    }
}
