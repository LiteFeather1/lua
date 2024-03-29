﻿using System;
using UnityEngine;

namespace Lua.Parentables
{
    public abstract class Parentable : MonoBehaviour, IParentable<Parentable>
    {
        public Action<Parentable> ReturnToPool { get; set; }

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
}
