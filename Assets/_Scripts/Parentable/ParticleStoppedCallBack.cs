using System.Collections;
using UnityEngine;

public class ParticleStoppedCallBack : Parentable
{
    [SerializeField] private ParticleSystem _ps;

    public System.Action<ParticleStoppedCallBack> Disabled { get; set; }

    public override void Parent(Transform parent)
    {
        base.Parent(parent);
        var mainModule = _ps.main;
        mainModule.loop = true;
        _ps.Play();
    }

    public override void UnParent()
    {
        base.UnParent();
        var mainModule = _ps.main;
        mainModule.loop = false;
    }

    private IEnumerator OnParticleSystemStopped()
    {
        yield return null;
        Disabled?.Invoke(this);
    }
}
