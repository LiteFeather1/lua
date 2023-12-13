using UnityEngine;

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
    }


}
