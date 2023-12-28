using UnityEngine;

namespace Lua.Misc
{
    public class RainbownizeOnEnable : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private ValueColourArray _colourArray;

        private void OnEnable() => _sr.color = _colourArray.PickRandom();
    }
}
