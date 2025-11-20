using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Managers;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Tutorial overlay UI komponenti
    /// </summary>
    public class TutorialUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _tutorialPanel;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Content")]
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Image _highlightImage;
        [SerializeField] private Image _arrowImage;

        [Header("Buttons")]
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _previousButton;

        [Header("Progress")]
        [SerializeField] private Text _progressText;
        [SerializeField] private Transform _dotContainer;
        [SerializeField] private GameObject _dotPrefab;

        [Header("Animation")]
        [SerializeField] private float _fadeInDuration = 0.3f;
        [SerializeField] private float _fadeOutDuration = 0.2f;

        [Header("Hand Animation")]
        [SerializeField] private RectTransform _handIcon;
        [SerializeField] private float _handSwipeDistance = 100f;
        [SerializeField] private float _handSwipeDuration = 1f;

        // Tutorial steps
        private List<TutorialStep> _steps;
        private int _currentStepIndex;
        private Coroutine _handAnimationCoroutine;

        // Events
        public event System.Action OnTutorialCompleted;
        public event System.Action OnTutorialSkipped;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
            InitializeTutorialSteps();
        }

        private void Start()
        {
            // Tutorial'i basta gizle
            if (_tutorialPanel != null)
                _tutorialPanel.SetActive(false);
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_nextButton != null)
                _nextButton.onClick.AddListener(NextStep);

            if (_skipButton != null)
                _skipButton.onClick.AddListener(SkipTutorial);

            if (_previousButton != null)
                _previousButton.onClick.AddListener(PreviousStep);
        }

        private void InitializeTutorialSteps()
        {
            _steps = new List<TutorialStep>
            {
                new TutorialStep
                {
                    title = "Decision Kingdom'a Hosgeldin!",
                    description = "Bu oyunda bir krali yonetecek ve onemli kararlar vereceksin. Her karar kralliginin kaderini belirleyecek.",
                    highlightArea = TutorialHighlight.None,
                    showHand = false
                },
                new TutorialStep
                {
                    title = "Kaynaklar",
                    description = "Dort temel kaynagi dengede tutmalisin:\n- Altin (Para)\n- Mutluluk (Halk)\n- Askeri Guc\n- Inanc (Din)",
                    highlightArea = TutorialHighlight.Resources,
                    showHand = false
                },
                new TutorialStep
                {
                    title = "Dengeyi Koru!",
                    description = "Herhangi bir kaynak 0'a duserse veya 100'e ulasirsa oyun biter! Asiri zenginlik de tehlikelidir.",
                    highlightArea = TutorialHighlight.Resources,
                    showHand = false
                },
                new TutorialStep
                {
                    title = "Kart Mekanigi",
                    description = "Karakterler sana sorular soracak. Onlara cevap vermek icin karti saga veya sola kaydir.",
                    highlightArea = TutorialHighlight.Card,
                    showHand = true
                },
                new TutorialStep
                {
                    title = "Etkileri Gor",
                    description = "Karti kaydirirken kaynak cubuklerindaki oklar seciminin etkisini gosterir. Dikkatli ol!",
                    highlightArea = TutorialHighlight.Resources,
                    showHand = true
                },
                new TutorialStep
                {
                    title = "Karakter Iliskileri",
                    description = "Ayni karakterlerle tekrar karsilasacaksin. Iliskiler onemli - dostlar ve dusmanlar olusabilir.",
                    highlightArea = TutorialHighlight.Card,
                    showHand = false
                },
                new TutorialStep
                {
                    title = "Hazir misin?",
                    description = "Simdi kralligini yonetmeye basla! Basarilar kazanarak Prestige Puanlari topla ve yeni donemler ac.",
                    highlightArea = TutorialHighlight.None,
                    showHand = false
                }
            };
        }
        #endregion

        #region Navigation
        private void NextStep()
        {
            if (_currentStepIndex < _steps.Count - 1)
            {
                _currentStepIndex++;
                ShowStep(_currentStepIndex);
            }
            else
            {
                CompleteTutorial();
            }
        }

        private void PreviousStep()
        {
            if (_currentStepIndex > 0)
            {
                _currentStepIndex--;
                ShowStep(_currentStepIndex);
            }
        }

        private void SkipTutorial()
        {
            HideTutorial();
            OnTutorialSkipped?.Invoke();

            // Tutorial'i bir daha gosterme
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            PlayerPrefs.Save();
        }

        private void CompleteTutorial()
        {
            HideTutorial();
            OnTutorialCompleted?.Invoke();

            // Tutorial tamamlandi
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            PlayerPrefs.Save();

            Debug.Log("[TutorialUI] Tutorial tamamlandi");
        }
        #endregion

        #region Display
        private void ShowStep(int index)
        {
            if (index < 0 || index >= _steps.Count) return;

            var step = _steps[index];

            // Icerik
            if (_titleText != null)
                _titleText.text = step.title;

            if (_descriptionText != null)
                _descriptionText.text = step.description;

            // Progress
            if (_progressText != null)
                _progressText.text = $"{index + 1} / {_steps.Count}";

            // Butonlar
            if (_previousButton != null)
                _previousButton.gameObject.SetActive(index > 0);

            if (_nextButton != null)
            {
                var buttonText = _nextButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = index == _steps.Count - 1 ? "Basla!" : "Ileri";
                }
            }

            // Highlight
            UpdateHighlight(step.highlightArea);

            // El animasyonu
            if (step.showHand)
            {
                StartHandAnimation();
            }
            else
            {
                StopHandAnimation();
            }

            // Progress dots
            UpdateProgressDots(index);
        }

        private void UpdateHighlight(TutorialHighlight area)
        {
            if (_highlightImage == null) return;

            // Basit highlight - gercek implementasyonda RectTransform pozisyonlari ayarlanmali
            _highlightImage.gameObject.SetActive(area != TutorialHighlight.None);

            if (_arrowImage != null)
            {
                _arrowImage.gameObject.SetActive(area != TutorialHighlight.None);
            }
        }

        private void UpdateProgressDots(int currentIndex)
        {
            if (_dotContainer == null || _dotPrefab == null) return;

            // Temizle
            foreach (Transform child in _dotContainer)
            {
                Destroy(child.gameObject);
            }

            // Dot'lari olustur
            for (int i = 0; i < _steps.Count; i++)
            {
                GameObject dot = Instantiate(_dotPrefab, _dotContainer);
                var image = dot.GetComponent<Image>();
                if (image != null)
                {
                    image.color = i == currentIndex ? Color.white : new Color(1, 1, 1, 0.3f);
                }
            }
        }
        #endregion

        #region Hand Animation
        private void StartHandAnimation()
        {
            if (_handIcon == null) return;

            _handIcon.gameObject.SetActive(true);

            if (_handAnimationCoroutine != null)
                StopCoroutine(_handAnimationCoroutine);

            _handAnimationCoroutine = StartCoroutine(AnimateHand());
        }

        private void StopHandAnimation()
        {
            if (_handAnimationCoroutine != null)
            {
                StopCoroutine(_handAnimationCoroutine);
                _handAnimationCoroutine = null;
            }

            if (_handIcon != null)
                _handIcon.gameObject.SetActive(false);
        }

        private IEnumerator AnimateHand()
        {
            Vector2 startPos = _handIcon.anchoredPosition;
            Vector2 endPos = startPos + Vector2.right * _handSwipeDistance;

            while (true)
            {
                // Sola don
                _handIcon.anchoredPosition = startPos - Vector2.right * _handSwipeDistance;

                // Saga kaydir
                float elapsed = 0f;
                while (elapsed < _handSwipeDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / _handSwipeDuration;
                    _handIcon.anchoredPosition = Vector2.Lerp(
                        startPos - Vector2.right * _handSwipeDistance,
                        endPos,
                        t
                    );
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Tutorial'i baslat
        /// </summary>
        public void StartTutorial()
        {
            // Daha once tamamlandi mi?
            if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
            {
                Debug.Log("[TutorialUI] Tutorial zaten tamamlanmis");
                return;
            }

            _currentStepIndex = 0;

            if (_tutorialPanel != null)
                _tutorialPanel.SetActive(true);

            ShowStep(0);

            if (_canvasGroup != null)
            {
                StartCoroutine(FadeIn());
            }

            Debug.Log("[TutorialUI] Tutorial basladi");
        }

        /// <summary>
        /// Tutorial'i gizle
        /// </summary>
        public void HideTutorial()
        {
            StopHandAnimation();

            if (_canvasGroup != null)
            {
                StartCoroutine(FadeOut());
            }
            else if (_tutorialPanel != null)
            {
                _tutorialPanel.SetActive(false);
            }
        }

        /// <summary>
        /// Tutorial'i sifirla
        /// </summary>
        public void ResetTutorial()
        {
            PlayerPrefs.SetInt("TutorialCompleted", 0);
            PlayerPrefs.Save();
            Debug.Log("[TutorialUI] Tutorial sifirlandi");
        }

        /// <summary>
        /// Tutorial tamamlandi mi?
        /// </summary>
        public static bool IsTutorialCompleted()
        {
            return PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
        }
        #endregion

        #region Coroutines
        private IEnumerator FadeIn()
        {
            _canvasGroup.alpha = 0f;

            float elapsed = 0f;
            while (elapsed < _fadeInDuration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = elapsed / _fadeInDuration;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOut()
        {
            float elapsed = 0f;
            while (elapsed < _fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = 1f - (elapsed / _fadeOutDuration);
                yield return null;
            }

            _canvasGroup.alpha = 0f;

            if (_tutorialPanel != null)
                _tutorialPanel.SetActive(false);
        }
        #endregion
    }

    #region Data Classes
    public enum TutorialHighlight
    {
        None,
        Card,
        Resources,
        Character
    }

    [System.Serializable]
    public class TutorialStep
    {
        public string title;
        public string description;
        public TutorialHighlight highlightArea;
        public bool showHand;
    }
    #endregion
}
