using LTF.ObjectPool;

namespace Lua.Parentables
{
    public interface IParentable<T> : IPoolable<T>
    {
        public void Parent(UnityEngine.Transform parent);
        public void UnParent();
    }
}
