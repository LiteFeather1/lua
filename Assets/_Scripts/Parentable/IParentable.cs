
namespace Lua.Parentables
{
    public interface IParentable<T>
    {
        public void Parent(UnityEngine.Transform parent);
        public void UnParent();
    }
}
