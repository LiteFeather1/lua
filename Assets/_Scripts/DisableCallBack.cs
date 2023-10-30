using UnityEngine;

public class DisableCallBack : MonoBehaviour
{
    public System.Action<DisableCallBack> Disabled { get; set; }

    private void OnDisable()
    {
        Disabled?.Invoke(this);
    }
}
