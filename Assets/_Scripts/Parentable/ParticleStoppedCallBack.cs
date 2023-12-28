using System.Collections;
using UnityEngine;

namespace Lua.Parentables
{
    public class ParticleStoppedCallBack : Parentable
    {
        [SerializeField] protected ParticleSystem _ps;

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
            ReturnToPool?.Invoke(this);
        }
    }
}
