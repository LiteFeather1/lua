using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _transformToMove;
    [SerializeField] private float _pixels = 8f;
    [SerializeField] private float _time;
    [SerializeField] private AnimationCurve _curve;
    private Vector2 _startPos;
    private IEnumerator _movement;

    private void Start()
    {
        _startPos = _transformToMove.localPosition;
        _movement = Movement(Vector2.zero, Vector2.zero);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(MoveUp());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(MoveDown());
    }

    public IEnumerator MoveUp()
    {
        StopCoroutine(_movement);
        _movement = Movement(_transformToMove.localPosition, _startPos + new Vector2(0f, _pixels));
        return _movement;
    }
    
    public IEnumerator MoveDown()
    {
        StopCoroutine(_movement);
        _movement = Movement(_transformToMove.localPosition, _startPos);
        return _movement;
    }

    public void Stop() => StopCoroutine(_movement);

    private IEnumerator Movement(Vector2 from, Vector2 to)
    {
        float eTime = 0f;
        while (eTime < _time) 
        {
            float t = _curve.Evaluate(eTime / _time); 
            _transformToMove.localPosition = Vector2.Lerp(from, to, t);
            eTime += Time.deltaTime;
            yield return null;
        }
        _transformToMove.localPosition = to;
    }
}
