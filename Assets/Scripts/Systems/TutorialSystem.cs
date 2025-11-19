using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Tutorial ve onboarding sistemi - Faz 8.1
    /// </summary>
    public class TutorialSystem : MonoBehaviour
    {
        public static TutorialSystem Instance { get; private set; }

        [Header("Tutorial Durumu")]
        [SerializeField] private bool _tutorialCompleted;
        [SerializeField] private int _currentStep;
        [SerializeField] private List<TutorialStep> _steps;

        // Events
        public event Action<TutorialStep> OnTutorialStepStarted;
        public event Action<TutorialStep> OnTutorialStepCompleted;
        public event Action OnTutorialCompleted;
        public event Action OnTutorialSkipped;

        private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";
        private const string TUTORIAL_STEP_KEY = "TutorialStep";

        #region Properties
        public bool IsTutorialCompleted => _tutorialCompleted;
        public bool IsTutorialActive => !_tutorialCompleted && _currentStep < _steps.Count;
        public TutorialStep CurrentStep => _currentStep < _steps.Count ? _steps[_currentStep] : null;
        public int CurrentStepIndex => _currentStep;
        public int TotalSteps => _steps.Count;
        public float Progress => _steps.Count > 0 ? (float)_currentStep / _steps.Count : 0f;
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

            InitializeTutorialSteps();
            LoadTutorialProgress();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Tutorial'i baslat
        /// </summary>
        public void StartTutorial()
        {
            if (_tutorialCompleted)
            {
                Debug.Log("[TutorialSystem] Tutorial zaten tamamlanmis");
                return;
            }

            _currentStep = 0;
            SaveTutorialProgress();

            if (_steps.Count > 0)
            {
                OnTutorialStepStarted?.Invoke(_steps[0]);
                Debug.Log($"[TutorialSystem] Tutorial basladi - Adim: {_steps[0].id}");
            }
        }

        /// <summary>
        /// Mevcut adimi tamamla ve sonrakine gec
        /// </summary>
        public void CompleteCurrentStep()
        {
            if (_currentStep >= _steps.Count)
                return;

            var completedStep = _steps[_currentStep];
            OnTutorialStepCompleted?.Invoke(completedStep);

            _currentStep++;
            SaveTutorialProgress();

            if (_currentStep >= _steps.Count)
            {
                CompleteTutorial();
            }
            else
            {
                OnTutorialStepStarted?.Invoke(_steps[_currentStep]);
                Debug.Log($"[TutorialSystem] Sonraki adim: {_steps[_currentStep].id}");
            }
        }

        /// <summary>
        /// Tutorial'i atla
        /// </summary>
        public void SkipTutorial()
        {
            _tutorialCompleted = true;
            _currentStep = _steps.Count;
            SaveTutorialProgress();

            OnTutorialSkipped?.Invoke();
            Debug.Log("[TutorialSystem] Tutorial atlandı");
        }

        /// <summary>
        /// Tutorial'i sifirla
        /// </summary>
        public void ResetTutorial()
        {
            _tutorialCompleted = false;
            _currentStep = 0;
            SaveTutorialProgress();

            Debug.Log("[TutorialSystem] Tutorial sifirlandı");
        }

        /// <summary>
        /// Belirli bir adima git
        /// </summary>
        public void GoToStep(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= _steps.Count)
                return;

            _currentStep = stepIndex;
            SaveTutorialProgress();
            OnTutorialStepStarted?.Invoke(_steps[_currentStep]);
        }

        /// <summary>
        /// Belirli bir adimi ID ile bul
        /// </summary>
        public TutorialStep GetStepById(string stepId)
        {
            return _steps.Find(s => s.id == stepId);
        }

        /// <summary>
        /// Oyuncu ilk kez mi oynuyor kontrolu
        /// </summary>
        public bool IsFirstTimePlayer()
        {
            return !PlayerPrefs.HasKey(TUTORIAL_COMPLETED_KEY);
        }
        #endregion

        #region Private Methods
        private void InitializeTutorialSteps()
        {
            _steps = new List<TutorialStep>
            {
                // Adim 1: Karsilama
                new TutorialStep
                {
                    id = "welcome",
                    titleKey = "TUTORIAL_WELCOME_TITLE",
                    messageKey = "TUTORIAL_WELCOME_MESSAGE",
                    highlightTarget = TutorialHighlight.None,
                    requiresAction = TutorialAction.Tap,
                    canSkip = true
                },

                // Adim 2: Kaynak Cubukları
                new TutorialStep
                {
                    id = "resources",
                    titleKey = "TUTORIAL_RESOURCES_TITLE",
                    messageKey = "TUTORIAL_RESOURCES_MESSAGE",
                    highlightTarget = TutorialHighlight.ResourceBars,
                    requiresAction = TutorialAction.Tap,
                    canSkip = false
                },

                // Adim 3: Kart Sistemi
                new TutorialStep
                {
                    id = "card_intro",
                    titleKey = "TUTORIAL_CARD_TITLE",
                    messageKey = "TUTORIAL_CARD_MESSAGE",
                    highlightTarget = TutorialHighlight.Card,
                    requiresAction = TutorialAction.Tap,
                    canSkip = false
                },

                // Adim 4: Sola Kaydir
                new TutorialStep
                {
                    id = "swipe_left",
                    titleKey = "TUTORIAL_SWIPE_LEFT_TITLE",
                    messageKey = "TUTORIAL_SWIPE_LEFT_MESSAGE",
                    highlightTarget = TutorialHighlight.SwipeLeft,
                    requiresAction = TutorialAction.SwipeLeft,
                    canSkip = false
                },

                // Adim 5: Saga Kaydir
                new TutorialStep
                {
                    id = "swipe_right",
                    titleKey = "TUTORIAL_SWIPE_RIGHT_TITLE",
                    messageKey = "TUTORIAL_SWIPE_RIGHT_MESSAGE",
                    highlightTarget = TutorialHighlight.SwipeRight,
                    requiresAction = TutorialAction.SwipeRight,
                    canSkip = false
                },

                // Adim 6: Onizleme
                new TutorialStep
                {
                    id = "preview",
                    titleKey = "TUTORIAL_PREVIEW_TITLE",
                    messageKey = "TUTORIAL_PREVIEW_MESSAGE",
                    highlightTarget = TutorialHighlight.ResourceBars,
                    requiresAction = TutorialAction.SwipeHold,
                    canSkip = false
                },

                // Adim 7: Dengede Kalma
                new TutorialStep
                {
                    id = "balance",
                    titleKey = "TUTORIAL_BALANCE_TITLE",
                    messageKey = "TUTORIAL_BALANCE_MESSAGE",
                    highlightTarget = TutorialHighlight.ResourceBars,
                    requiresAction = TutorialAction.Tap,
                    canSkip = false
                },

                // Adim 8: Game Over
                new TutorialStep
                {
                    id = "game_over",
                    titleKey = "TUTORIAL_GAMEOVER_TITLE",
                    messageKey = "TUTORIAL_GAMEOVER_MESSAGE",
                    highlightTarget = TutorialHighlight.None,
                    requiresAction = TutorialAction.Tap,
                    canSkip = false
                },

                // Adim 9: Karakterler
                new TutorialStep
                {
                    id = "characters",
                    titleKey = "TUTORIAL_CHARACTERS_TITLE",
                    messageKey = "TUTORIAL_CHARACTERS_MESSAGE",
                    highlightTarget = TutorialHighlight.CharacterPortrait,
                    requiresAction = TutorialAction.Tap,
                    canSkip = false
                },

                // Adim 10: Tamamlama
                new TutorialStep
                {
                    id = "complete",
                    titleKey = "TUTORIAL_COMPLETE_TITLE",
                    messageKey = "TUTORIAL_COMPLETE_MESSAGE",
                    highlightTarget = TutorialHighlight.None,
                    requiresAction = TutorialAction.Tap,
                    canSkip = false
                }
            };
        }

        private void CompleteTutorial()
        {
            _tutorialCompleted = true;
            SaveTutorialProgress();

            OnTutorialCompleted?.Invoke();
            Debug.Log("[TutorialSystem] Tutorial tamamlandı!");
        }

        private void SaveTutorialProgress()
        {
            PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, _tutorialCompleted ? 1 : 0);
            PlayerPrefs.SetInt(TUTORIAL_STEP_KEY, _currentStep);
            PlayerPrefs.Save();
        }

        private void LoadTutorialProgress()
        {
            _tutorialCompleted = PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 1;
            _currentStep = PlayerPrefs.GetInt(TUTORIAL_STEP_KEY, 0);
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Reset Tutorial")]
        private void DebugResetTutorial()
        {
            ResetTutorial();
        }

        [ContextMenu("Complete Tutorial")]
        private void DebugCompleteTutorial()
        {
            CompleteTutorial();
        }

        [ContextMenu("Next Step")]
        private void DebugNextStep()
        {
            CompleteCurrentStep();
        }
#endif
        #endregion
    }

    /// <summary>
    /// Tutorial adimi veri yapisi
    /// </summary>
    [Serializable]
    public class TutorialStep
    {
        public string id;
        public string titleKey;
        public string messageKey;
        public TutorialHighlight highlightTarget;
        public TutorialAction requiresAction;
        public bool canSkip;
        public float delayBeforeShow;
        public string customData;
    }

    /// <summary>
    /// Tutorial vurgulama hedefleri
    /// </summary>
    public enum TutorialHighlight
    {
        None,
        ResourceBars,
        Card,
        SwipeLeft,
        SwipeRight,
        CharacterPortrait,
        TurnCounter,
        PauseButton,
        SettingsButton
    }

    /// <summary>
    /// Tutorial icin gereken aksiyonlar
    /// </summary>
    public enum TutorialAction
    {
        Tap,
        SwipeLeft,
        SwipeRight,
        SwipeHold,
        Wait
    }
}
