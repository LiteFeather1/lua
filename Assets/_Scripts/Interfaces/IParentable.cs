
public interface IParentable<T> : IReturnToPool<T>
{
    public void Parent(UnityEngine.Transform parent);
    public void UnParent();
}
