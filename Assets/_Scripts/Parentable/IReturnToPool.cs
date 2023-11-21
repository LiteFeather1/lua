public interface IReturnToPool<T>
{
    public System.Action<T> OnReturn { get; set; }
}
