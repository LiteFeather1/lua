using UnityEngine;

namespace Lua.Misc
{
    public class DontShowIfDesktop : MonoBehaviour
    {
#if !UNITY_WEBGL
        private void Awake()
        {
             gameObject.SetActive(false);
        }
#endif
    }
}
