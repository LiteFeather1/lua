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
    private float _elapsedTimeToDrawCard;

    [Header("Power Ups")]
    [SerializeField] private PowerUp[] _startingPowerUps;
    private Weighter<PowerUp> _weightedPowerUps;

    [Header("Cards")]
    [SerializeField] private float _cardSize = 32f;
    [SerializeField] private CardUIPowerUp[] _cards;
    private List<CardUIPowerUp> _cardsToDraw;
    private List<CardUIPowerUp> _drawnCards;
    [SerializeField] private RectTransform _cardArea;

    [Header("Drawer")]
    [SerializeField] private Image i_drawer;
    [SerializeField] private Sprite[] _drawerAnimation;
    
    [Header("Seeker")]
    [SerializeField] private CardUIDropContainerSeek _seek;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI t_cardName;
    [SerializeField] private TextMeshProUGUI t_cardEffect;

    [Header("Card Moviment")]
    [SerializeField] private float _cardMoveSpeed = 5f;
    [SerializeField] private float _spacingBetweenCards;
    private Vector2[] _cardVelocity;

    public CardUIPowerUp[] Cards => _cards;

    public Action OnCardHovered { get; set; }
    public Action OnCardUnHovered { get; set; }

    private void Awake()
    {
        _drawnCards = new();
        _cardVelocity = new Vector2[_cards.Length];
        for (int i = 0; i < _cards.Length; i++)
        {
            var card = _cards[i];
            card.OnShowDescription += CardHovered;
            card.OnCardUnHovered += CardUnHovered;
            card.OnPickedUp += CardPickedUp;
            card.OnDropped += CardDropped;
            card.OnUsed += OnUsed;
        }
        _cardsToDraw = new(_cards);

        _seek.OnPowerUpDropped += SeekDropped;
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
        for (int i = 0; i < _cards.Length; i++)
        {
            var card = _cards[i];
            card.OnShowDescription -= CardHovered;
            card.OnCardUnHovered -= CardUnHovered;
            card.OnPickedUp -= CardPickedUp;
            card.OnDropped -= CardDropped;
            card.OnUsed -= OnUsed;
        }

        _seek.OnPowerUpDropped -= SeekDropped;
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

    private void CardHovered(PowerUp power)
    {
        t_cardName.text = power.Name;
        t_cardEffect.color = power.RarityColour;
        t_cardEffect.text = power.Efffect;
        t_cardEffect.enabled = true;
        t_cardName.enabled = true;
        OnCardHovered?.Invoke();
    }

    private void CardUnHovered()
    {
        t_cardName.enabled = false;
        t_cardEffect.enabled = false;
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
