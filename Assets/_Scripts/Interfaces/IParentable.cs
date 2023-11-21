
public interface IParentable : IReturnToPool<IParentable>
{
    public void Parent(UnityEngine.Transform parent);
    public void UnParent();
}
