using MoonPhaseConsole;
using UnityEngine;

public class MoonSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite[] _moonPhases;

    public void Start()
    {
        var result = Moon.UtcNow();
        _sr.sprite = _moonPhases[result.Index];
    }
}
