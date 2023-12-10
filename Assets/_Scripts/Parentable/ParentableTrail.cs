using UnityEngine;

public class ParentableTrail : Parentable
{
    [SerializeField] private TrailRenderer _trailRenderer;

    public override void Parent(Transform parent)
    {
        base.Parent(parent);
        _trailRenderer.Clear();
    }

    public override void UnParent()
    {
        base.UnParent();
        OnReturn?.Invoke(this);
    }
}
