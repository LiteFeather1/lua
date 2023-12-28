using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lua.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputMapping Inputs { get; private set; }

        private void Awake()
        {
            if (Inputs != null)
            {
                Destroy(gameObject);
                return;
            }

            Inputs = new();
            Inputs.Enable();

            DontDestroyOnLoad(gameObject);
#if !UNITY_WEBGL || UNITY_EDITOR
            Inputs.Player.Fullscreen_Toggle.performed += ToggleFullScreen;
#endif
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private void OnDestroy()
        {
            Inputs.Player.Fullscreen_Toggle.performed -= ToggleFullScreen;
        }
#endif

#if UNITY_EDITOR
        private void ToggleFullScreen(InputAction.CallbackContext ctx)
        {
            var window = EditorWindow.focusedWindow;
            window.maximized = !window.maximized;
        }
#elif !UNITY_WEBGL
        private void ToggleFullScreen(InputAction.CallbackContext ctx)
        {
            var fullScreen = Screen.fullScreen = !Screen.fullScreen;
            if (!fullScreen && Screen.width != 1280)
                Screen.SetResolution(1280, 720, false);
            else
                Screen.SetResolution(1920, 1080, true);
        }
#endif
    }
}
