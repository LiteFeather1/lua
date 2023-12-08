using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EndScreenManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform _root;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI t_timeText;
    [SerializeField] private TextMeshProUGUI t_enemies;
    [SerializeField] private TextMeshProUGUI t_cardsPlayed;
    [SerializeField] private TextMeshProUGUI t_cardsRecycled;
    [SerializeField] private TextMeshProUGUI t_candyEarned;

    [Header("Carousel")]
    [SerializeField] private float _carouselSpeed = 28f;
    [SerializeField] private EndCardUi _cardPrefab;
    [SerializeField] private RectTransform rt_carousel;
    private readonly List<EndCardUi> _cards = new();

    //[Header("Drag")]
    private bool _dragging;
    private float _lastMouseX;

    private void Start()
    {
        var powerUps = LTFUtils.LTFHelpers_Misc.GetScriptableObjects<PowerUp>();
        foreach (var powerUp in powerUps)
        {
            AddCard(powerUp);
        }
    }

    private void Update()
    {
        var minX = -(rt_carousel.sizeDelta.x + 34f) * .5f;
        var maxCards = rt_carousel.sizeDelta.x / 34f;
        float mul = _cards.Count < maxCards ? 1f : _cards.Count;
        var maxX = (rt_carousel.sizeDelta.x * .5f) + (34f * mul);
        maxX += _cards.Count > maxCards ? minX * 2f + 17f : 0f;
        Vector3 translation;
        if (!_dragging)
            translation = _carouselSpeed * _root.localScale.x * Time.deltaTime * Vector3.left;
        else
        {
            var mousePosX = Input.mousePosition.x;
            var delta = (mousePosX - _lastMouseX);
            translation = new(delta, 0f);
            _lastMouseX = mousePosX;
        }
        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];
            card.transform.Translate(translation);
            if (card.transform.localPosition.x < minX)
            {
                var outOff = card.transform.localPosition.x - minX;
                card.transform.localPosition = new (maxX + outOff, 0f);
            }
            else if (card.transform.localPosition.x > maxX)
            {
                var outOff = maxX - card.transform.localPosition.x;
                card.transform.localPosition = new(minX - outOff, 0f);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _lastMouseX = Input.mousePosition.x;
        _dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _dragging = false;
    }

    public void SetTexts(string time, int enemies, int cardsReciclyed, int candy)
    {
        t_timeText.text = time;
        t_enemies.text = enemies.ToString();
        t_cardsPlayed.text = _cards.Count.ToString();
        t_cardsRecycled.text = cardsReciclyed.ToString();
        t_candyEarned.text = candy.ToString();
    }

    public void AddCard(PowerUp power)
    {
        var maxX = 34f * _cards.Count;
        var card = Instantiate(_cardPrefab, rt_carousel);
        card.transform.localPosition = new Vector2(maxX, 0f);
        card.SetPower(power);
        _cards.Add(card);
    }

    public void Replay()
    {
        var scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }
}
