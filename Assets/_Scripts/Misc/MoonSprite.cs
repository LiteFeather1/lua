using MoonPhaseConsole;
using UnityEngine;

namespace Lua.Misc
{
    public class MoonSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private LTF.ValueGeneric.ValueSprite _seasonalSprite;
        [SerializeField] private Sprite[] _moonPhases;

        public void Start()
        {
            if (_seasonalSprite.Value != null)
            {
                _sr.sprite = _seasonalSprite.Value;
                return;
            }

            var result = Moon.UtcNow();
            _sr.sprite = _moonPhases[result.Index];
        }
    }
}
