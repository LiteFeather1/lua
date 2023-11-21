using System;
using UnityEngine;

public abstract class Parentable<T> : MonoBehaviour, IParentable<T>
{
    public Action<T> OnReturn { get; set; }

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
