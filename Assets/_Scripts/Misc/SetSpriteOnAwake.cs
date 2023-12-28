using UnityEngine;

namespace Lua.Misc
{
    public class SetSpriteOnAwake : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private LTF.ValueGeneric.ValueSprite _valueSprite;

        private void Awake() => _sr.sprite = _valueSprite;
    }
}
