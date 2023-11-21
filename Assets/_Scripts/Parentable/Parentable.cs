using System;
using UnityEngine;

public class Parentable : MonoBehaviour, IParentable
{
    public Action<IParentable> OnReturn { get; set; }

    public virtual void Parent(Transform parent)
    {
        transform.SetParent(parent, false);
        transform.localScale = parent.localScale;
        transform.localPosition = Vector3.zero;
    }

    public virtual void UnParent()
    {
        var pos = transform.position;
        transform.SetParent(null, false);
        transform.position = pos;
    }
}
