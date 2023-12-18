using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UICursor : MonoBehaviour
{
    [SerializeField] private Image _cursorImage;
    [SerializeField] private Sprite _normalCursor, _pressedCursor;

    private void Awake()
    {
        InputManager.Inputs.Player.Left_Click.performed += OnClickPerformed;
        InputManager.Inputs.Player.Left_Click.canceled += OnClickCanceled;
    }

    private void Update()
    {
        _cursorImage.transform.position = Input.mousePosition;
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = true;
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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void OnBeforeSplashScreen()
    {
        Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
