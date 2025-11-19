using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Data;
using DecisionKingdom.Managers;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Kart görünüm UI komponenti
    /// </summary>
    public class CardView : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private Text _characterNameText;
        [SerializeField] private Text _eventText;
        [SerializeField] private Text _leftChoiceText;
        [SerializeField] private Text _rightChoiceText;
        [SerializeField] private Image _characterPortrait;
        [SerializeField] private Image _cardBackground;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Animasyon")]
        [SerializeField] private float _enterDuration = Constants.CARD_ENTER_DURATION;
        [SerializeField] private float _exitDuration = Constants.CARD_EXIT_DURATION;
        [SerializeField] private AnimationCurve _enterCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _exitCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Swipe Görsel")]
        [SerializeField] private float _maxRotation = 15f;
        [SerializeField] private float _maxOffset = 100f;
        [SerializeField] private Image _leftOverlay;
        [SerializeField] private Image _rightOverlay;

        // State
        private RectTransform _rectTransform;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private GameEvent _currentEvent;
        private Coroutine _animationCoroutine;

        #region Unity Lifecycle
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
            _originalRotation = _rectTransform.rotation;

            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            // Overlay'leri başlangıçta gizle
            if (_leftOverlay != null) _leftOverlay.gameObject.SetActive(false);
            if (_rightOverlay != null) _rightOverlay.gameObject.SetActive(false);
        }

        private void Start()
        {
            // CardManager event'lerine abone ol
            if (CardManager.Instance != null)
            {
                CardManager.Instance.OnCardDisplayed += HandleCardDisplayed;
                CardManager.Instance.OnCardSwiped += HandleCardSwiped;
                CardManager.Instance.OnSwipePreview += HandleSwipePreview;
                CardManager.Instance.OnSwipeCancelled += HandleSwipeCancelled;
            }
        }

        private void OnDestroy()
        {
            if (CardManager.Instance != null)
            {
                CardManager.Instance.OnCardDisplayed -= HandleCardDisplayed;
                CardManager.Instance.OnCardSwiped -= HandleCardSwiped;
                CardManager.Instance.OnSwipePreview -= HandleSwipePreview;
                CardManager.Instance.OnSwipeCancelled -= HandleSwipeCancelled;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Kartı göster
        /// </summary>
        public void DisplayCard(GameEvent gameEvent)
        {
            _currentEvent = gameEvent;

            // UI'ı güncelle
            if (_characterNameText != null)
                _characterNameText.text = gameEvent.CharacterName;

            if (_eventText != null)
                _eventText.text = gameEvent.text;

            if (_leftChoiceText != null)
                _leftChoiceText.text = gameEvent.leftChoice?.text ?? "";

            if (_rightChoiceText != null)
                _rightChoiceText.text = gameEvent.rightChoice?.text ?? "";

            if (_characterPortrait != null && gameEvent.character?.portrait != null)
                _characterPortrait.sprite = gameEvent.character.portrait;

            // Animasyonla gir
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimateEnter());
        }

        /// <summary>
        /// Kartı kaldır
        /// </summary>
        public void RemoveCard(bool swipedRight)
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimateExit(swipedRight));
        }

        /// <summary>
        /// Swipe önizlemesi güncelle
        /// </summary>
        public void UpdateSwipePreview(SwipeDirection direction, float progress)
        {
            if (_rectTransform == null) return;

            float directionMultiplier = direction == SwipeDirection.Right ? 1f : -1f;

            // Pozisyon offset
            Vector3 offset = Vector3.right * _maxOffset * progress * directionMultiplier;
            _rectTransform.anchoredPosition = _originalPosition + (Vector2)offset;

            // Rotasyon
            float rotation = -_maxRotation * progress * directionMultiplier;
            _rectTransform.rotation = _originalRotation * Quaternion.Euler(0, 0, rotation);

            // Overlay'leri güncelle
            UpdateOverlays(direction, progress);
        }

        /// <summary>
        /// Pozisyonu sıfırla
        /// </summary>
        public void ResetPosition()
        {
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.rotation = _originalRotation;

            if (_leftOverlay != null) _leftOverlay.gameObject.SetActive(false);
            if (_rightOverlay != null) _rightOverlay.gameObject.SetActive(false);
        }
        #endregion

        #region Event Handlers
        private void HandleCardDisplayed(GameEvent gameEvent)
        {
            DisplayCard(gameEvent);
        }

        private void HandleCardSwiped(GameEvent gameEvent, bool swipedRight)
        {
            RemoveCard(swipedRight);
        }

        private void HandleSwipePreview(SwipeDirection direction, float progress)
        {
            UpdateSwipePreview(direction, progress);
        }

        private void HandleSwipeCancelled()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimateReturn());
        }
        #endregion

        #region Animations
        private IEnumerator AnimateEnter()
        {
            // Başlangıç durumu
            _rectTransform.anchoredPosition = _originalPosition + Vector2.up * 500f;
            _rectTransform.localScale = Vector3.one * 0.8f;

            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;

            float elapsed = 0f;

            while (elapsed < _enterDuration)
            {
                elapsed += Time.deltaTime;
                float t = _enterCurve.Evaluate(elapsed / _enterDuration);

                _rectTransform.anchoredPosition = Vector2.Lerp(
                    _originalPosition + Vector2.up * 500f,
                    _originalPosition,
                    t
                );

                _rectTransform.localScale = Vector3.Lerp(
                    Vector3.one * 0.8f,
                    Vector3.one,
                    t
                );

                if (_canvasGroup != null)
                    _canvasGroup.alpha = t;

                yield return null;
            }

            // Final durum
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.localScale = Vector3.one;

            if (_canvasGroup != null)
                _canvasGroup.alpha = 1f;

            _animationCoroutine = null;
        }

        private IEnumerator AnimateExit(bool swipedRight)
        {
            Vector2 startPos = _rectTransform.anchoredPosition;
            Vector2 targetPos = startPos + (swipedRight ? Vector2.right : Vector2.left) * 1000f;
            Quaternion startRot = _rectTransform.rotation;
            Quaternion targetRot = startRot * Quaternion.Euler(0, 0, swipedRight ? -30f : 30f);

            float elapsed = 0f;

            while (elapsed < _exitDuration)
            {
                elapsed += Time.deltaTime;
                float t = _exitCurve.Evaluate(elapsed / _exitDuration);

                _rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                _rectTransform.rotation = Quaternion.Lerp(startRot, targetRot, t);

                if (_canvasGroup != null)
                    _canvasGroup.alpha = 1f - t;

                yield return null;
            }

            // Kartı gizle ve sıfırla
            gameObject.SetActive(false);
            ResetPosition();

            if (_canvasGroup != null)
                _canvasGroup.alpha = 1f;

            gameObject.SetActive(true);
            _animationCoroutine = null;
        }

        private IEnumerator AnimateReturn()
        {
            Vector2 startPos = _rectTransform.anchoredPosition;
            Quaternion startRot = _rectTransform.rotation;

            float elapsed = 0f;
            float duration = 0.2f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                _rectTransform.anchoredPosition = Vector2.Lerp(startPos, _originalPosition, t);
                _rectTransform.rotation = Quaternion.Lerp(startRot, _originalRotation, t);

                yield return null;
            }

            ResetPosition();
            _animationCoroutine = null;
        }
        #endregion

        #region Helper Methods
        private void UpdateOverlays(SwipeDirection direction, float progress)
        {
            if (_leftOverlay != null)
            {
                _leftOverlay.gameObject.SetActive(direction == SwipeDirection.Left);
                if (direction == SwipeDirection.Left)
                {
                    Color color = _leftOverlay.color;
                    color.a = progress;
                    _leftOverlay.color = color;
                }
            }

            if (_rightOverlay != null)
            {
                _rightOverlay.gameObject.SetActive(direction == SwipeDirection.Right);
                if (direction == SwipeDirection.Right)
                {
                    Color color = _rightOverlay.color;
                    color.a = progress;
                    _rightOverlay.color = color;
                }
            }
        }
        #endregion
    }
}
