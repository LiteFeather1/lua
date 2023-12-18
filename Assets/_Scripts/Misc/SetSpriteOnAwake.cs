using UnityEngine;

public class SetSpriteOnAwake : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private ValueSprite _valueSprite;

    private void Awake() => _sr.sprite = _valueSprite;
}
