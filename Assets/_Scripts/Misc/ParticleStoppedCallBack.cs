using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleStoppedCallBack : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;

    public System.Action<ParticleStoppedCallBack> Disabled { get; set; }

    public void Parent(Transform parent)
    {
        transform.SetParent(parent, false);
        transform.localScale = parent.localScale;
        transform.localPosition = Vector3.zero;
        var mainModule = _ps.main;
        mainModule.loop = true;
        _ps.Play();
    }

    public void Unparent()
    {
        var pos  = transform.position;
        transform.SetParent(null, false);
        transform.position = pos;
        var mainModule = _ps.main;
        mainModule.loop = false;
    }

    private IEnumerator OnParticleSystemStopped()
    {
        yield return null;
        Disabled?.Invoke(this);
    }
}
