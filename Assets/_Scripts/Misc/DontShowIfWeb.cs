using UnityEngine;

namespace Lua.Misc
{
    public class DontShowIfWeb : MonoBehaviour
    {
#if UNITY_WEBGL
        private void Awake()
        {
             gameObject.SetActive(false);
        }
#endif
    }
}
