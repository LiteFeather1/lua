using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Lua.PowerUps;
using Lua.PowerUps.Cards;

namespace Lua.Managers
{
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

        [Header("Drag")]
        [SerializeField] private float _dragReduceSpeed = 100f;
        [SerializeField] private float _maxMouseDelta;
        private float _dragVelocity;
        private bool _dragging;
        private float _lastMouseX;
        private float _mouseDelta;

        private const float CARD_SIZE = 34f;

        public int CardsPlayed => _cards.Count;

        private void Update()
        {
            var minX = -(rt_carousel.sizeDelta.x + CARD_SIZE) * .5f;
            var maxCards = rt_carousel.sizeDelta.x / CARD_SIZE;
            float mul = _cards.Count < maxCards ? 1f : _cards.Count;
            var maxX = (rt_carousel.sizeDelta.x * .5f) + (CARD_SIZE * mul);
            maxX += _cards.Count > maxCards ? minX * 2f + 17f : 0f;
            float x;
            if (_mouseDelta == 0f && !_dragging)
                x = _carouselSpeed * _root.localScale.x * Time.deltaTime;
            else
            {
                var mousePosX = Input.mousePosition.x;
                if (_dragging)
                    _mouseDelta = mousePosX - _lastMouseX;
                else
                {
                    _mouseDelta = Mathf.SmoothDamp(_mouseDelta, 0f, ref _dragVelocity, Time.deltaTime * _dragReduceSpeed);
                    if (_mouseDelta < 2f && _mouseDelta > -2f)
                        _mouseDelta = 0f;
                }

                x = _mouseDelta;
                _lastMouseX = mousePosX;
            }

            var translation = new Vector3(x, 0f);
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

            // Limit Speed
            if (_mouseDelta > _maxMouseDelta)
                _mouseDelta = _maxMouseDelta;
            else if (_mouseDelta < -_maxMouseDelta)
                _mouseDelta = -_maxMouseDelta;
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
            var maxX = CARD_SIZE * _cards.Count;
            var card = Instantiate(_cardPrefab, rt_carousel);
            card.transform.localPosition = new(maxX, 0f);
            card.SetPower(power);
            _cards.Add(card);
        }

        public void Replay()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
