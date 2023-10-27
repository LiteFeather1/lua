using LTFUtils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Canvas _root;

    [Header("Time")]
    [SerializeField] private CompositeValue _timeToDrawCard = new(5f);
    private float _elapsedTimeToDrawCard;

    [Header("Power Ups")]
    [SerializeField] private Weighter<PowerUp> _powerUps;

    [Header("Cards")]
    [SerializeField] private float _cardSize = 32f;
    [SerializeField] private List<CardUIPowerUp> _cardsToDrawn;
    private List<CardUIPowerUp> _drawnCards;
    [SerializeField] private RectTransform _cardArea;
    [SerializeField] private Image i_drawer;

    [Header("Animations")]
    [SerializeField] private float _cardMoveSpeed = 5f;
    [SerializeField] private float _spacingBetweenCards;
    private Vector2[] _cardVelocity;

    public System.Action<CardUIPowerUp> OnCardPlayed { get; set; }    

    private void Awake()
    {
        _drawnCards = new();
        _cardVelocity = new Vector2[_cardsToDrawn.Count];
    }

    private void OnEnable()
    {
        for (int i = 0; i < _cardsToDrawn.Count; i++)
        {
            var card = _cardsToDrawn[i];
            card.OnPickedUp += CardPickedUp;
            card.OnDropped += CardDropped;
            card.OnPlayed += CardPlayed;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _powerUps.Count; i++)
        {
            _powerUps.Objects[i].Object.Reset();
        }
    }

    private void Update()
    {
        DrawCards();
        MoveCards();
    }

    private void OnDisable()
    {
        for (int i = 0; i < _cardsToDrawn.Count; i++)
        {
            var card = _cardsToDrawn[i];
            card.OnPickedUp -= CardPickedUp;
            card.OnDropped -= CardDropped;
            card.OnPlayed -= CardPlayed;
        }
    }

    private void DrawCards()
    {
        if (_cardsToDrawn.Count == 0)
            return;

        _elapsedTimeToDrawCard += Time.deltaTime;

        if (_elapsedTimeToDrawCard > _timeToDrawCard.Value)
        {
            _elapsedTimeToDrawCard = 0f;
            var card = _cardsToDrawn[0];
            _cardsToDrawn.RemoveAt(0);
            var weightedObject = _powerUps.GetWeightedObject();
            var powerUp = weightedObject.Object;
            if (powerUp.IncreasePickedAmount())
                _powerUps.RemoveObject(weightedObject);
            card.SetPowerUp(powerUp);
            card.transform.position = i_drawer.transform.position;
            _drawnCards.Add(card);
            card.gameObject.SetActive(true);
        }
    }

    private void MoveCards()
    {
        var count = _drawnCards.Count;
        var addX = (_cardSize + _spacingBetweenCards) * _root.transform.localScale.x;
        var minX = -(count - 1) * (addX / 2f);
        var deltaTime = Time.deltaTime;
        for (int i = 0; i < count; i++)
        {
            var card = _drawnCards[i];
            Vector2 to = new Vector2(minX + (addX * i), 0f) + (Vector2)_cardArea.position;
            card.transform.position = Vector2.SmoothDamp(card.transform.position, to, 
                                                         ref _cardVelocity[i], _cardMoveSpeed * deltaTime);
        }
    }

    private void CardDropped(CardUIPowerUp card)
    {
        _drawnCards.Add(card);
    }

    private void CardPickedUp(CardUIPowerUp card)
    {
        _drawnCards.Remove(card);
    }

    private void CardPlayed(CardUIPowerUp card)
    {
        card.gameObject.SetActive(false);
    }
}
