using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LTF;
using LTF.Utils;
using LTF.Weighter;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps.Cards
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private EndScreenManager _endScreenManager;
        [SerializeField] private Canvas _root;

        [Header("Recycle Effects")]
        [SerializeField] private CompositeValue _onRecycleDamageEnemies;
        [SerializeField] private CompositeValue _onRecycleHeal;
        [SerializeField] private CompositeValue _onRecycleAddCurrency;
        [SerializeField] private CompositeValue _onRecycleRefund;

        [Header("On Card Played Effect")]
        [SerializeField] private CompositeValue _onCardPlayedDamageEnemies;
        [SerializeField] private CompositeValue _onCardPlayedHeal;
        [SerializeField] private CompositeValue _onCardPlayedRefund;

        [Header("Time")]
        [SerializeField] private CompositeValue _timeToDrawCard = new(5f);
        private float _elapsedTimeToDrawCard;

        [Header("Inflation")]
        [SerializeField] private Rarity[] _rarities;
        [SerializeField, ReadOnly] private float _inflation = -.25f;
        private const float INFLATION_INCREASE = 0.015f;
        private readonly Dictionary<Rarity, float> _rarityInflation = new();

        [Header("Power Ups")]
        [SerializeField] private PowerUp[] _startingPowerUps;
        private Weighter<PowerUp> _weightedPowerUps;
        private readonly Dictionary<string, HashSet<WeightedObject<PowerUp>>> _powerUpToPowerUps = new();
        private readonly HashSet<PowerUp> _allPowerUps = new();

        [Header("Cards")]
        [SerializeField] private int _cardToDrawPerDraw = 1;
        [SerializeField] private int _maxCards = 5;
        [SerializeField] private int _startingCards = 3;
        private int _cards;
        [SerializeField] private CardUIPowerUp _cardPrefab;
        private List<CardUIPowerUp> _cardsToDraw;
        private List<CardUIPowerUp> _drawnCards;
        [SerializeField] private RectTransform _cardArea;
        private List<CanvasGroup> _cardGroupBack;

        [Header("Card Moviment")]
        [SerializeField] private float _cardSize = 30f;
        [SerializeField] private float _cardMoveSpeed = 5f;
        [SerializeField] private Vector2 _spacingBetweenCardsRange = new(16f, 12f);

        [Header("Drawer")]
        [SerializeField] private Image i_drawer;
        [SerializeField] private MoveOnPointerEnter _drawerMove;
        [SerializeField] private Color _drawerDisabledColour = Color.grey;
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
        [SerializeField] private TextMeshProUGUI t_cardNameUnderlay;
        [SerializeField] private TextMeshProUGUI t_cardEffect;
        [SerializeField] private TextMeshProUGUI t_cardEffectUnderlay;

        public GameManager GameManager => GameManager.Instance;

        public CompositeValue TimeToDrawCard => _timeToDrawCard;

        public CompositeValue OnRecycleDamageEnemies => _onRecycleDamageEnemies;
        public CompositeValue OnRecycleHeal => _onRecycleHeal;
        public CompositeValue OnRecycleAddCurrency => _onRecycleAddCurrency;
        public CompositeValue OnRecycleRefund => _onRecycleRefund;

        public CompositeValue OnCardPlayedDamageEnemies => _onCardPlayedDamageEnemies;
        public CompositeValue OnCardPlayedHeal => _onCardPlayedHeal;
        public CompositeValue OnCardPlayedRefund => _onCardPlayedRefund;

        private void Awake()
        {
            _drawnCards = new();
            _cardsToDraw = new(_startingCards);
            _cardGroupBack = new(_startingCards);
            _cards = _startingCards;
            for (int i = 0; i < _startingCards; i++)
                CreateCard();

            for (int i = 0; i < _rarities.Length; i++)
                _rarityInflation.Add(_rarities[i], -.1f);

            _seek.OnPowerUpDropped += SeekDropped;
            _player.OnPowerPlayed += PowerUpPlayed;
            _recycler.OnCardUsed += CardRecycled;
        }

        private void Start()
        {
            _weightedPowerUps = new(CreateRangeWeightedPowerUp(_startingPowerUps));
            StartCoroutine(_drawerMove.MoveUp());

            var gm = GameManager.Instance;
            gm.OnGameEnded += GameEnded;
            gm.OnGetCardsActedInfo = GetCardsActedInfo;
        }

        private void Update()
        {
            DrawCardsUpdate();
            MoveCards();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _cardsToDraw.Count; i++)
                UnSubToCard(_cardsToDraw[i]);

            for (int i = 0; i < _drawnCards.Count; i++)
                UnSubToCard(_drawnCards[i]);

            _seek.OnPowerUpDropped -= SeekDropped;
            _player.OnPowerPlayed -= PowerUpPlayed;
            _recycler.OnCardUsed -= CardRecycled;

            var gm = GameManager.Instance;
            gm.OnGameEnded -= GameEnded;
        }

        public void CardRefundDrawer(float unitInterval)
        {
            _elapsedTimeToDrawCard += (_timeToDrawCard - _elapsedTimeToDrawCard) * unitInterval;
        }

        public int AddCard(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateCard();

            _cards += amount;
            _cardToDrawPerDraw += amount;
            return _cards;
        }

        public void AddWeightedPowerUps(IEnumerable<PowerUp> powerUps)
        {
            _weightedPowerUps.AddRange(CreateRangeWeightedPowerUp(powerUps));
        }

        public void RemoveCardsOfType(string type)
        {
            if (!_powerUpToPowerUps.ContainsKey(type))
                return;

            foreach (var powerup in _powerUpToPowerUps[type])
                _weightedPowerUps.RemoveObject(powerup);

            _powerUpToPowerUps.Remove(type);
        }

        public IEnumerator RedrawHand()
        {
            yield return null;
            for (int i = _drawnCards.Count - 1; i >= 0; i--)
                _recycler.DropCard(_drawnCards[i]);

            for (int i = _cardsToDraw.Count - 1; i >= 0; i--)
                ActivateCard(_cardsToDraw[i]);

            _cardsToDraw.Clear();
            i_drawer.color = _drawerDisabledColour;
        }

        // Unity event
        public void SetBackCanvasGroupState(bool state)
        {
            for (int i = 0; i < _cardGroupBack.Count; i++)
                _cardGroupBack[i].enabled = state;
        }

        private IEnumerable<WeightedObject<PowerUp>> CreateRangeWeightedPowerUp(IEnumerable<PowerUp> powerUps)
        {
            foreach (var powerUp in powerUps)
            {
                // Don't add the same power up multiple times
                if (_allPowerUps.Contains(powerUp))
                    continue;

                _allPowerUps.Add(powerUp);
                powerUp.Reset();

                var weightedObject = new WeightedObject<PowerUp>(powerUp.Weight, powerUp);

                var powerUpType = powerUp.PowerUpType;
                // Power up dictionary type
                if (_powerUpToPowerUps.ContainsKey(powerUpType))
                    _powerUpToPowerUps[powerUpType].Add(weightedObject);
                else
                    _powerUpToPowerUps.Add(powerUpType, new(1) { weightedObject });

                yield return weightedObject;
            }
        }

        private void CreateCard()
        {
            var card = Instantiate(_cardPrefab, _cardArea);
            card.gameObject.SetActive(false);
            _cardsToDraw.Add(card);
            _cardGroupBack.Add(card.CanvasGroupBack);
            SubToCard(card);
        }

        private void SubToCard(CardUIPowerUp card)
        {
            card.OnShowDescription += CardHovered;
            card.OnCardUnHovered += CardUnHovered;
            card.OnPickedUp += CardPickedUp;
            card.OnDropped += CardDropped;
            card.OnReturnToPile += ReturnToDrawPile;
        }

        private void UnSubToCard(CardUIPowerUp card)
        {
            card.OnShowDescription -= CardHovered;
            card.OnCardUnHovered -= CardUnHovered;
            card.OnPickedUp -= CardPickedUp;
            card.OnDropped -= CardDropped;
            card.OnReturnToPile -= ReturnToDrawPile;
        }

        private void ActivateCard(CardUIPowerUp card)
        {
            var powerUp = _weightedPowerUps.GetObject();
            var cost = Mathf.CeilToInt(powerUp.Cost + 
                (powerUp.Cost * Mathf.Max(0f, _inflation + _rarityInflation[powerUp.Rarity])));
            card.SetPowerUp(powerUp, cost);
            card.transform.position = i_drawer.transform.position;
            _drawnCards.Add(card);
            card.gameObject.SetActive(true);
        }

        private void DrawCardsUpdate()
        {
            if (_cardsToDraw.Count == 0)
                return;

            _elapsedTimeToDrawCard += Time.deltaTime;

            if (_elapsedTimeToDrawCard > _timeToDrawCard.Value)
            {
                _elapsedTimeToDrawCard = 0f;
                for (int i = 0; i < _cardToDrawPerDraw; i++)
                {
                    var card = _cardsToDraw[0];
                    _cardsToDraw.RemoveAt(0);
                    ActivateCard(card);

                    if (_cardsToDraw.Count == 0)
                    {
                        i_drawer.color = _drawerDisabledColour;
                        StartCoroutine(_drawerMove.MoveDown());
                        break;
                    }
                }
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
            t_cardName.text = t_cardNameUnderlay.text = power.Name;
            t_cardName.color = power.RarityColour;
            t_cardEffect.text = t_cardEffectUnderlay.text = power.Effect;
            _textBG.SetActive(true);
            GameManager.Instance.SlowDown();
        }

        private void CardUnHovered()
        {
            _textBG.SetActive(false);
            GameManager.Instance.UnSlowDown();
        }

        private void CardDropped(CardUIPowerUp card)
        {
            _drawnCards.Add(card);
        }

        private void CardPickedUp(CardUIPowerUp card)
        {
            _drawnCards.Remove(card);
        }

        private void ReturnToDrawPile(CardUIPowerUp card)
        {
            card.gameObject.SetActive(false);
            _drawnCards.Remove(card);
            _cardsToDraw.Add(card);

            if (_cardsToDraw.Count == 1)
            {
                i_drawer.color = Color.white;
                StartCoroutine(_drawerMove.MoveUp());
            }
        }

        private void SeekDropped(WeightedObject<PowerUp> old, WeightedObject<PowerUp> @new)
        {
            if (old != null)
                _weightedPowerUps.RemoveObject(old);

            _weightedPowerUps.AddObject(@new);
        }

        private void PowerUpPlayed(PowerUp powerUp)
        {
            _inflation += INFLATION_INCREASE;
            _rarityInflation[powerUp.Rarity] += INFLATION_INCREASE;

            _endScreenManager.AddCard(powerUp);

            var gm = GameManager.Instance;
            if (_onCardPlayedDamageEnemies > 0.01f)
                gm.SpawnManager.DamageEveryEnemy(_onCardPlayedDamageEnemies);

            gm.Witch.Health.Heal(_onCardPlayedHeal);

            CardRefundDrawer(_onCardPlayedRefund);
        }

        private void CardRecycled()
        {
            var gm = GameManager.Instance;
            if (_onRecycleDamageEnemies > 0.01f)
                gm.SpawnManager.DamageEveryEnemy(_onRecycleDamageEnemies);

            gm.Witch.Health.Heal(_onRecycleHeal);
            gm.Witch.ModifyCurrency((int)_onRecycleAddCurrency);

            CardRefundDrawer(_onRecycleRefund);
        }

        private void GameEnded(string time, int enemies, int candy)
        {
            _endScreenManager.SetTexts(time, enemies, _recycler.CardsRecycled, candy);
        }

        private (int CardsPlayed, int CardsRecycled) GetCardsActedInfo()
        {
            return (_endScreenManager.CardsPlayed, _recycler.CardsRecycled);
        }

#if UNITY_EDITOR
        [ContextMenu("Find Power Ups")]
        private void FindPowerUps()
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Find Power Ups");
            var allPowers = LTFHelpersEditorOnly.GetScriptableObjects<PowerUp>();
            _startingPowerUps = (from powerUp in allPowers
                                 where !powerUp.name.StartsWith('X') &&
                                    !(from otherPowerUp in allPowers
                                      from unlockPowerUp in otherPowerUp.PowerUpsToUnlock
                                      where unlockPowerUp == powerUp
                                      select unlockPowerUp)
                                    .Any()
                                 select powerUp)
                                 .ToArray();

            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Find Rarities")]
        private void FindRarities()
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Find Rarities");
            _rarities = LTFHelpersEditorOnly.GetScriptableObjects<Rarity>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
