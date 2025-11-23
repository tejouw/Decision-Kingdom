using System;
using System.Collections;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Managers
{
    /// <summary>
    /// Kart görüntüleme ve swipe yönetimi
    /// </summary>
    public class CardManager : MonoBehaviour
    {
        public static CardManager Instance { get; private set; }

        [Header("Swipe Ayarları")]
        [SerializeField] private float _swipeThreshold = Constants.SWIPE_THRESHOLD;
        [SerializeField] private float _previewThreshold = Constants.SWIPE_PREVIEW_THRESHOLD;
        [SerializeField] private float _swipeDuration = Constants.CARD_SWIPE_DURATION;

        [Header("Mevcut Kart")]
        [SerializeField] private GameEvent _currentEvent;

        // Swipe tracking
        private Vector2 _swipeStartPosition;
        private bool _isSwiping;
        private SwipeDirection _previewDirection;

        // Events
        public event Action<GameEvent> OnCardDisplayed;
        public event Action<GameEvent, bool> OnCardSwiped; // bool: true = sağ, false = sol
        public event Action<SwipeDirection, float> OnSwipePreview;
        public event Action OnSwipeStarted;
        public event Action OnSwipeCancelled;

        #region Properties
        public GameEvent CurrentEvent => _currentEvent;
        public bool HasCard => _currentEvent != null;
        public bool IsSwiping => _isSwiping;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsPlaying || _currentEvent == null)
                return;

            HandleInput();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Yeni kart göster
        /// </summary>
        public void DisplayCard(GameEvent gameEvent)
        {
            _currentEvent = gameEvent;
            _isSwiping = false;
            _previewDirection = SwipeDirection.None;

            OnCardDisplayed?.Invoke(gameEvent);

            Debug.Log($"[CardManager] Kart gösteriliyor: {gameEvent.id}");
        }

        /// <summary>
        /// Kartı temizle
        /// </summary>
        public void ClearCard()
        {
            _currentEvent = null;
            _isSwiping = false;
            _previewDirection = SwipeDirection.None;
        }

        /// <summary>
        /// Programatik olarak kart kaydır
        /// </summary>
        public void SwipeCard(bool swipeRight)
        {
            if (_currentEvent == null) return;

            GameEvent eventToProcess = _currentEvent;
            _currentEvent = null;

            OnCardSwiped?.Invoke(eventToProcess, swipeRight);

            // GameManager'a bildir
            GameManager.Instance?.ProcessEventChoice(eventToProcess, swipeRight);

            Debug.Log($"[CardManager] Kart kaydırıldı: {(swipeRight ? "Sağ" : "Sol")}");
        }

        /// <summary>
        /// Sol seçeneği seç
        /// </summary>
        public void SelectLeft()
        {
            SwipeCard(false);
        }

        /// <summary>
        /// Sağ seçeneği seç
        /// </summary>
        public void SelectRight()
        {
            SwipeCard(true);
        }
        #endregion

        #region Input Handling
        private void HandleInput()
        {
            // Dokunmatik giriş
            if (Input.touchCount > 0)
            {
                HandleTouchInput();
            }
            // Mouse giriş
            else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                HandleMouseInput();
            }
            // Klavye giriş
            else
            {
                HandleKeyboardInput();
            }
        }

        private void HandleTouchInput()
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartSwipe(touch.position);
                    break;

                case TouchPhase.Moved:
                    UpdateSwipe(touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndSwipe(touch.position);
                    break;
            }
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSwipe(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && _isSwiping)
            {
                UpdateSwipe(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && _isSwiping)
            {
                EndSwipe(Input.mousePosition);
            }
        }

        private void HandleKeyboardInput()
        {
            // Sol ok / A tuşu = Sol seçenek
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SelectLeft();
            }
            // Sağ ok / D tuşu = Sağ seçenek
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SelectRight();
            }
        }
        #endregion

        #region Swipe Logic
        private void StartSwipe(Vector2 position)
        {
            _swipeStartPosition = position;
            _isSwiping = true;
            _previewDirection = SwipeDirection.None;

            OnSwipeStarted?.Invoke();
        }

        private void UpdateSwipe(Vector2 position)
        {
            if (!_isSwiping) return;

            float deltaX = position.x - _swipeStartPosition.x;
            float progress = Mathf.Clamp01(Mathf.Abs(deltaX) / _swipeThreshold);

            // Önizleme yönünü belirle
            SwipeDirection direction = SwipeDirection.None;
            if (Mathf.Abs(deltaX) >= _previewThreshold)
            {
                direction = deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }

            // Önizleme değiştiyse event tetikle
            if (direction != _previewDirection)
            {
                _previewDirection = direction;

                // ResourceManager'a önizleme bildir
                if (_currentEvent != null && ResourceManager.Instance != null)
                {
                    if (direction == SwipeDirection.Left && _currentEvent.leftChoice?.effects != null)
                    {
                        ResourceManager.Instance.PreviewEffects(_currentEvent.leftChoice.effects.ToArray());
                    }
                    else if (direction == SwipeDirection.Right && _currentEvent.rightChoice?.effects != null)
                    {
                        ResourceManager.Instance.PreviewEffects(_currentEvent.rightChoice.effects.ToArray());
                    }
                    else
                    {
                        ResourceManager.Instance.ClearPreview();
                    }
                }
            }

            OnSwipePreview?.Invoke(direction, progress);
        }

        private void EndSwipe(Vector2 position)
        {
            if (!_isSwiping) return;

            _isSwiping = false;
            float deltaX = position.x - _swipeStartPosition.x;

            // Önizlemeyi temizle
            ResourceManager.Instance?.ClearPreview();

            // Swipe eşiğini aştı mı?
            if (Mathf.Abs(deltaX) >= _swipeThreshold)
            {
                SwipeCard(deltaX > 0);
            }
            else
            {
                // İptal edildi
                _previewDirection = SwipeDirection.None;
                OnSwipeCancelled?.Invoke();
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Sol seçenek metnini al
        /// </summary>
        public string GetLeftChoiceText()
        {
            return _currentEvent?.leftChoice?.text ?? "";
        }

        /// <summary>
        /// Sağ seçenek metnini al
        /// </summary>
        public string GetRightChoiceText()
        {
            return _currentEvent?.rightChoice?.text ?? "";
        }

        /// <summary>
        /// Event metnini al
        /// </summary>
        public string GetEventText()
        {
            return _currentEvent?.text ?? "";
        }

        /// <summary>
        /// Karakter adını al
        /// </summary>
        public string GetCharacterName()
        {
            return _currentEvent?.CharacterName ?? "";
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Test Swipe Left")]
        private void DebugSwipeLeft()
        {
            SelectLeft();
        }

        [ContextMenu("Test Swipe Right")]
        private void DebugSwipeRight()
        {
            SelectRight();
        }

        [ContextMenu("Create Test Card")]
        private void DebugCreateTestCard()
        {
            var testEvent = new GameEvent
            {
                id = "test_event",
                era = Era.Medieval,
                text = "Test event metni. Bu bir deneme kartıdır.",
                leftChoice = new Choice("Reddet")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 5),
                rightChoice = new Choice("Kabul Et")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -5)
            };

            DisplayCard(testEvent);
        }
#endif
        #endregion
    }
}
