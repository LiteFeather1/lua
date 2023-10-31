using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private Transform _root;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI t_timeText;
    [SerializeField] private TextMeshProUGUI t_enemies;
    [SerializeField] private TextMeshProUGUI t_cardsPlayed;
    [SerializeField] private TextMeshProUGUI t_cardsRecycled;
    [SerializeField] private TextMeshProUGUI t_candyEarned;

    [Header("Carousel")]
    [SerializeField] private float _speed = 24f;
    [SerializeField] private EndCardUi _cardPrefab;
    [SerializeField] private RectTransform rt_carousel;
    private readonly List<EndCardUi> _cards = new();

    private void Update()
    {
        var size = _root.localScale.x;
        var minX = (-(rt_carousel.sizeDelta.x) - (34f)) * .5f;
        var maxCards = rt_carousel.sizeDelta.x / 34f;
        float mul = _cards.Count;
        if (_cards.Count < maxCards)
            mul = 1f;
        var maxX = (rt_carousel.sizeDelta.x * .5f) + (34f * mul);
        if (_cards.Count > maxCards)
            maxX += minX * 2f + 17f;
        float delta = Time.deltaTime;
        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];
            var speed = _speed * size;
            card.transform.Translate(delta * speed * Vector2.left);
            if (card.transform.localPosition.x < minX)
            {
                card.transform.localPosition = new Vector2(maxX, 0f);
            }
        }
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
