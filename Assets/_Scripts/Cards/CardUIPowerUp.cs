using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lua.Managers;

namespace Lua.PowerUps.Cards
{
    public class CardUIPowerUp : CardUi, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler
    {
        private int _cost;

        [Header("Card UI Power Up")]
        [SerializeField] private float _defaultAlpha = .9f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _canvasGroupBack;
        [SerializeField] private Image i_powerUp;
        [SerializeField] private Image i_powerUpTierIcon;
        [SerializeField] private TextMeshProUGUI t_cardCost;
        private Transform _originalParent;
        private PowerUp _powerUp;
        private Quaternion _deriv;
        private bool _dragging;

        [Header("Audio")]
        [SerializeField] private AudioClip _grabClip, _ungrabClip;

        // icky
        [HideInInspector] public Vector2 Velocity;

        public Action<CardUIPowerUp> OnPickedUp { get; set; }
        public Action<CardUIPowerUp> OnReturnToPile { get; set; }
        public Action<CardUIPowerUp> OnDropped { get; set; }
        public Action<PowerUp> OnShowDescription { get; set; }

        public int Cost => _cost;
        public PowerUp PowerUp => _powerUp;
        public CanvasGroup CanvasGroupBack => _canvasGroupBack;

        public void Update()
        {
            if (!_dragging)
                return;

            var delta = Time.deltaTime;
            transform.position = Vector2.SmoothDamp(transform.position, Input.mousePosition, ref Velocity, delta * 6f);
            var zRotation = Mathf.Lerp(0f, 18f, Mathf.Abs(Velocity.x) / 1800f) * Mathf.Sign(Velocity.x);
            var to = Quaternion.Euler(0f, 0f, -zRotation);
            transform.localRotation = SmoothDamp(transform.localRotation, to, ref _deriv, delta * 128f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnShowDescription?.Invoke(_powerUp);
            _canvasGroup.alpha = 1f;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (_dragging && GameManager.Instance.Witch.Currency > _powerUp.Cost)
                return; 

            base.OnPointerExit(eventData);
            _canvasGroup.alpha = _defaultAlpha;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            OnPickedUp?.Invoke(this);
            _dragging = true;
            _originalParent = transform.parent;
            transform.SetParent(transform.parent.parent);
            _canvasGroup.blocksRaycasts = false;
            OnCardUnHovered?.Invoke();
            AudioManager.Instance.PlayOneShot(_grabClip);
        }

        // Only used to voit the event system to know what is dragging
        public void OnDrag(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _dragging = false;
            transform.SetParent(_originalParent);
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = _defaultAlpha;
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            OnDropped?.Invoke(this);
            AudioManager.Instance.PlayOneShot(_ungrabClip);
        }

        public void SetPowerUp(PowerUp powerUp, int cost)
        {
            _powerUp = powerUp;
            i_powerUp.sprite = powerUp.Icon;
            i_powerUpTierIcon.sprite = powerUp.TierIcon;
            i_card.color = powerUp.RarityColour;
            t_cardCost.text = (_cost = cost).ToString();
            AudioManager.Instance.PlayOneShot(_grabClip);
        }

        public void ReturnToPile()
        {
            Velocity = Vector2.zero;
            OnReturnToPile?.Invoke(this);
        }

        private static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
        {
            if (Time.deltaTime < Mathf.Epsilon) 
                return rot;

            // account for double-cover
            var dot = Quaternion.Dot(rot, target);
            var mult = dot > 0f ? 1f : -1f;
            target.x *= mult;
            target.y *= mult;
            target.z *= mult;
            target.w *= mult;
            // smooth damp (nlerp approx)
            var result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
            ).normalized;

            // ensure deriv is tangent
            var derivError = Vector4.Project(new(deriv.x, deriv.y, deriv.z, deriv.w), result);
            deriv.x -= derivError.x;
            deriv.y -= derivError.y;
            deriv.z -= derivError.z;
            deriv.w -= derivError.w;

            return new(result.x, result.y, result.z, result.w);
        }
    }
}
