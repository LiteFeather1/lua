using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static InputMapping Inputs { get; private set; }

    private void Awake()
    {
        Instance = this;
        Inputs = new();
        Inputs.Enable();
    }

}
