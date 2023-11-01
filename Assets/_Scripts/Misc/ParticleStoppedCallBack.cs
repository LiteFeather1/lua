using System.Collections;
using UnityEngine;

public class ParticleStoppedCallBack : MonoBehaviour
{
    public System.Action<ParticleStoppedCallBack> Disabled { get; set; }

    private IEnumerator OnParticleSystemStopped()
    {
        yield return null;
        Disabled?.Invoke(this);
    }
}
