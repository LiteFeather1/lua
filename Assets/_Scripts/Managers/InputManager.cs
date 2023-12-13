using UnityEngine;
#if !UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine.InputSystem;
#endif
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

#if !UNITY_WEBGL && !UNITY_EDITOR
        Inputs.Player.Fullscreen_Toggle.performed += ToggleFullScreen;
#endif
    }

#if  !UNITY_WEBGL && !UNITY_EDITOR
    private void OnDestroy()
    {
        Inputs.Player.Fullscreen_Toggle.performed -= ToggleFullScreen;
    }

    private void ToggleFullScreen(InputAction.CallbackContext ctx)
    {
        var fullScreen = Screen.fullScreen = !Screen.fullScreen;
        if (!fullScreen && Screen.width != 1280) 
        {
            Screen.SetResolution(1280, 720, false);
        }
    }
#endif
}
