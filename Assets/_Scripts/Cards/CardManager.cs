using LTFUtils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Canvas _root;

    [Header("Time")]
    [SerializeField] private CompositeValue _timeToDrawCard = new(5f);
    [SerializeField] private CompositeValue _refundOnDiscard = new(0f);
    private float _elapsedTimeToDrawCard;

    [Header("Power Ups")]
    [SerializeField] private PowerUp[] _startingPowerUps;
    private Weighter<PowerUp> _weightedPowerUps;

    [Header("Cards")]
    [SerializeField] private float _cardSize = 30f;
    [SerializeField] private int _maxCards = 6;
    [SerializeField] private int _startingCards = 3;
    [SerializeField] private CardUIPowerUp _cardPrefab;
    private List<CardUIPowerUp> _cardsToDraw;
    private List<CardUIPowerUp> _drawnCards;
    [SerializeField] private RectTransform _cardArea;

    [Header("Drawer")]
    [SerializeField] private Image i_drawer;
    [SerializeField] private Sprite[] _drawerAnimation;

    [Header("Player")]
    [SerializeField] private CardUIContainerPlay _player;

    [Header("Seeker")]
    [SerializeField] private CardUIDropContainerSeek _seek;

    [Header("Recycle")]
    [SerializeField] private CardUIDropContainerRecycle _recycler;

    [Header("Text")]
    [SerializeField] private GameObject _textBG;
    [SerializeField] private TextMeshProUGUI t_cardName;
    [SerializeField] private TextMeshProUGUI t_cardEffect;

    [Header("Card Moviment")]
    [SerializeField] private float _cardMoveSpeed = 5f;
    [SerializeField] private Vector2 _spacingBetweenCardsRange = new(16f, 12f);

    public CompositeValue TimeToDrawCard => _timeToDrawCard;
    public CompositeValue RefundOnDiscard => _refundOnDiscard;

    public CardUIContainerPlay Player => _player;
    public CardUIDropContainerRecycle Recycler => _recycler;

    public Action OnCardHovered { get; set; }
    public Action OnCardUnHovered { get; set; }

    private void Awake()
    {
        _drawnCards = new();
        _cardsToDraw = new(_startingCards);
        for (int i = 0; i < _startingCards; i++)
            AddCard();

        _seek.OnPowerUpDropped += SeekDropped;

        _recycler.OnCardUsed += RefundCooldown;
    }

    private void Start()
    {
        var weightedObjects = new WeightedObject<PowerUp>[_startingPowerUps.Length];
        for (int i = 0; i < _startingPowerUps.Length; i++)
        {
            var powerUp = _startingPowerUps[i];
            weightedObjects[i] = new(powerUp, powerUp.Weight);  
            powerUp.Reset();
        }

        _weightedPowerUps = new(weightedObjects);
    }

    private void Update()
    {
        DrawCards();
        MoveCards();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _cardsToDraw.Count; i++)
            UnSubToCard(_cardsToDraw[i]);

        for (int i = 0; i < _drawnCards.Count; i++)
            UnSubToCard(_drawnCards[i]);

        _seek.OnPowerUpDropped -= SeekDropped;

        _recycler.OnCardUsed -= RefundCooldown;
    }

    public void RefundCooldown()
    {
        var refund = (_timeToDrawCard.Value - _elapsedTimeToDrawCard) * _refundOnDiscard.Value;
        _elapsedTimeToDrawCard += refund;
    }

    [ContextMenu("Add Card")]
    public void AddCard()
    {
        var card = Instantiate(_cardPrefab, _cardArea);
        card.gameObject.SetActive(false);
        _cardsToDraw.Add(card);
        SubToCard(card);
    }

    private void SubToCard(CardUIPowerUp card)
    {
        card.OnShowDescription += CardHovered;
        card.OnCardUnHovered += CardUnHovered;
        card.OnPickedUp += CardPickedUp;
        card.OnDropped += CardDropped;
        card.OnUsed += OnUsed;
    }

    private void UnSubToCard(CardUIPowerUp card)
    {
        card.OnShowDescription -= CardHovered;
        card.OnCardUnHovered -= CardUnHovered;
        card.OnPickedUp -= CardPickedUp;
        card.OnDropped -= CardDropped;
        card.OnUsed -= OnUsed;
    }

    private void DrawCards()
    {
        if (_cardsToDraw.Count == 0)
            return;

        _elapsedTimeToDrawCard += Time.deltaTime;

        if (_elapsedTimeToDrawCard > _timeToDrawCard.Value)
        {
            _elapsedTimeToDrawCard = 0f;
            var card = _cardsToDraw[0];
            _cardsToDraw.RemoveAt(0);
            var weightedObject = _weightedPowerUps.GetWeightedObject();
            var powerUp = weightedObject.Object;
            if (powerUp.IncreasePickedAmount())
                _weightedPowerUps.RemoveObject(weightedObject);
            card.SetPowerUp(powerUp);
            card.transform.position = i_drawer.transform.position;
            _drawnCards.Add(card);
            card.gameObject.SetActive(true);
        }

        float t = _elapsedTimeToDrawCard / _timeToDrawCard.Value;
        var index = Mathf.FloorToInt(t * (_drawerAnimation.Length - 1));
        i_drawer.sprite = _drawerAnimation[index];
    }

    private void MoveCards()
    {
        var count = _drawnCards.Count;
        var spacing = _spacingBetweenCardsRange.Evaluate((float)count / _maxCards);
        var addX = (_cardSize + spacing) * _root.transform.localScale.x;
        var minX = -(count - 1) * (addX / 2f);
        var deltaTime = Time.deltaTime;
        for (int i = 0; i < count; i++)
        {
            var card = _drawnCards[i];
            Vector2 to = new Vector2(minX + (addX * i), 0f) + (Vector2)_cardArea.position;
            card.transform.position = Vector2.SmoothDamp(card.transform.position, to, 
                                                         ref card.Velocity, _cardMoveSpeed * deltaTime);
        }
    }

    private void CardHovered(PowerUp power)
    {
        t_cardName.text = power.Name;
        t_cardEffect.color = power.RarityColour;
        t_cardEffect.text = power.Efffect;
        _textBG.SetActive(true);
        OnCardHovered?.Invoke();
    }

    private void CardUnHovered()
    {
        _textBG.SetActive(false);
        OnCardUnHovered?.Invoke();
    }

    private void CardDropped(CardUIPowerUp card)
    {
        _drawnCards.Add(card);
    }

    private void CardPickedUp(CardUIPowerUp card)
    {
        _drawnCards.Remove(card);
    }

    private void OnUsed(CardUIPowerUp card)
    {
        card.gameObject.SetActive(false);
        _drawnCards.Remove(card);
        _cardsToDraw.Add(card);
    }

    private void SeekDropped(WeightedObject<PowerUp> old, WeightedObject<PowerUp> new_)
    {
        if (old != null)
            _weightedPowerUps.RemoveObject(old);

        _weightedPowerUps.AddObject(new_);
    }
}
