
namespace LTFUtils.ObjectPool
{
    public interface IPoolable<T>
    {
        public System.Action<T> ReturnToPool { get; set; }
    }
}
