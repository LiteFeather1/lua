using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Lua.Managers;

namespace Lua.UI
{
    public class UICursor : MonoBehaviour
    {
        private static bool s_initialized;

        [SerializeField] private Image _cursorImage;
        [SerializeField] private Sprite _normalCursor, _pressedCursor;

        private void Awake()
        {
            if (s_initialized)
            {
                Destroy(gameObject);
                return;
            }

            s_initialized = true;
            InputManager.Inputs.Player.Left_Click.performed += OnClickPerformed;
            InputManager.Inputs.Player.Left_Click.canceled += OnClickCanceled;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _cursorImage.transform.position = Input.mousePosition;
        }

        private void OnApplicationFocus(bool focus)
        {
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            InputManager.Inputs.Player.Left_Click.performed -= OnClickPerformed;
            InputManager.Inputs.Player.Left_Click.canceled -= OnClickCanceled;
        }

        private void OnClickPerformed(InputAction.CallbackContext ctx)
        {
            _cursorImage.sprite = _pressedCursor;
        }

        private void OnClickCanceled(InputAction.CallbackContext ctx)
        {
            _cursorImage.sprite = _normalCursor;
        }

#if UNITY_WEBGL
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScreen()
        {
            Cursor.visible = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }
#endif
    }
}
