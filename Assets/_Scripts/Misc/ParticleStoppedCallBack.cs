using UnityEngine;

public class ParticleStoppedCallBack : MonoBehaviour
{
    public System.Action<ParticleStoppedCallBack> Disabled { get; set; }

    private void OnParticleSystemStopped()
    {
        Disabled?.Invoke(this);
    }
}
