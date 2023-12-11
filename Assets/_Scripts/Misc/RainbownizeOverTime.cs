using UnityEngine;

public class RainbownizeOverTime : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private ValueColourArray _colourArray;
    [SerializeField] private ValueFloat _timeToChange;
    private float _elaspsedTime = 100f;

    private void Update()
    {
        _elaspsedTime += Time.deltaTime;
        if (_elaspsedTime > _timeToChange )
        {
            _elaspsedTime = 0f;
            _sr.color = _colourArray.PickRandom();
        }
    }
}
