using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DecisionKingdom.Core;
using DecisionKingdom.Managers;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Ana menu UI komponenti
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _eraSelectionPanel;
        [SerializeField] private GameObject _settingsPanel;

        [Header("Main Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _dailyChallengeButton;
        [SerializeField] private Button _achievementsButton;
        [SerializeField] private Button _statisticsButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _quitButton;

        [Header("Info Display")]
        [SerializeField] private Text _prestigePointsText;
        [SerializeField] private Text _playerNameText;
        [SerializeField] private Text _versionText;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration = 0.5f;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            UpdateUI();
            ShowMainPanel();

            // Fade in animasyonu
            if (_canvasGroup != null)
            {
                StartCoroutine(FadeIn());
            }
        }

        private void OnEnable()
        {
            // Event subscriptions
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.OnPrestigePointsChanged += HandlePrestigeChanged;
            }
        }

        private void OnDisable()
        {
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.OnPrestigePointsChanged -= HandlePrestigeChanged;
            }
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_newGameButton != null)
                _newGameButton.onClick.AddListener(OnNewGameClicked);

            if (_continueButton != null)
                _continueButton.onClick.AddListener(OnContinueClicked);

            if (_dailyChallengeButton != null)
                _dailyChallengeButton.onClick.AddListener(OnDailyChallengeClicked);

            if (_achievementsButton != null)
                _achievementsButton.onClick.AddListener(OnAchievementsClicked);

            if (_statisticsButton != null)
                _statisticsButton.onClick.AddListener(OnStatisticsClicked);

            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(OnSettingsClicked);

            if (_shopButton != null)
                _shopButton.onClick.AddListener(OnShopClicked);

            if (_quitButton != null)
                _quitButton.onClick.AddListener(OnQuitClicked);
        }
        #endregion

        #region UI Updates
        private void UpdateUI()
        {
            // Prestige puani
            if (_prestigePointsText != null && PrestigeManager.Instance != null)
            {
                _prestigePointsText.text = $"PP: {PrestigeManager.Instance.TotalPrestigePoints}";
            }

            // Oyuncu adi
            if (_playerNameText != null && ProfileSystem.Instance != null)
            {
                _playerNameText.text = ProfileSystem.Instance.PlayerName;
            }

            // Versiyon
            if (_versionText != null)
            {
                _versionText.text = $"v{Application.version}";
            }

            // Continue butonu - kayitli oyun var mi?
            if (_continueButton != null)
            {
                bool hasSave = SaveManager.Instance != null && SaveManager.Instance.HasSavedGame();
                _continueButton.interactable = hasSave;
            }

            // Daily challenge - bugun tamamlandi mi?
            if (_dailyChallengeButton != null && DailyChallengeSystem.Instance != null)
            {
                var buttonText = _dailyChallengeButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = DailyChallengeSystem.Instance.HasCompletedToday
                        ? "Gunluk (Tamamlandi)"
                        : "Gunluk Challenge";
                }
            }
        }

        private void HandlePrestigeChanged(int newValue)
        {
            if (_prestigePointsText != null)
            {
                _prestigePointsText.text = $"PP: {newValue}";
            }
        }
        #endregion

        #region Button Handlers
        private void OnNewGameClicked()
        {
            Debug.Log("[MainMenuUI] Yeni oyun tiklandi");

            // Era secim panelini goster
            if (_eraSelectionPanel != null)
            {
                HideAllPanels();
                _eraSelectionPanel.SetActive(true);
            }
            else
            {
                // Direkt oyuna basla
                StartGame(Era.Medieval, null);
            }
        }

        private void OnContinueClicked()
        {
            Debug.Log("[MainMenuUI] Devam et tiklandi");

            if (SaveManager.Instance != null && SaveManager.Instance.HasSavedGame())
            {
                SaveManager.Instance.LoadGame();
                LoadGameScene();
            }
        }

        private void OnDailyChallengeClicked()
        {
            Debug.Log("[MainMenuUI] Gunluk challenge tiklandi");

            if (DailyChallengeSystem.Instance != null)
            {
                if (DailyChallengeSystem.Instance.StartDailyChallenge())
                {
                    LoadGameScene();
                }
            }
        }

        private void OnAchievementsClicked()
        {
            Debug.Log("[MainMenuUI] Basarilar tiklandi");
            // AchievementUI'yi goster - baska bir panel veya scene
        }

        private void OnStatisticsClicked()
        {
            Debug.Log("[MainMenuUI] Istatistikler tiklandi");
            // StatisticsUI'yi goster
        }

        private void OnSettingsClicked()
        {
            Debug.Log("[MainMenuUI] Ayarlar tiklandi");

            if (_settingsPanel != null)
            {
                HideAllPanels();
                _settingsPanel.SetActive(true);
            }
        }

        private void OnShopClicked()
        {
            Debug.Log("[MainMenuUI] Magaza tiklandi");
            // PrestigeShopUI'yi goster
        }

        private void OnQuitClicked()
        {
            Debug.Log("[MainMenuUI] Cikis tiklandi");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Oyunu baslat
        /// </summary>
        public void StartGame(Era era, string scenarioId)
        {
            Resources startingResources = null;

            // Senaryo varsa kaynaklari al
            if (!string.IsNullOrEmpty(scenarioId))
            {
                var scenarioInfo = PrestigeManager.Instance?.GetScenarioInfo(scenarioId);
                if (scenarioInfo != null)
                {
                    startingResources = scenarioInfo.startingResources;
                }
            }

            // GameManager'da yeni oyun baslat
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame(era, startingResources);
            }

            LoadGameScene();
        }

        /// <summary>
        /// Ana paneli goster
        /// </summary>
        public void ShowMainPanel()
        {
            HideAllPanels();
            if (_mainPanel != null)
                _mainPanel.SetActive(true);

            UpdateUI();
        }

        /// <summary>
        /// Tum panelleri gizle
        /// </summary>
        public void HideAllPanels()
        {
            if (_mainPanel != null) _mainPanel.SetActive(false);
            if (_eraSelectionPanel != null) _eraSelectionPanel.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.SetActive(false);
        }
        #endregion

        #region Private Methods
        private void LoadGameScene()
        {
            // Game scene'ine gec
            SceneManager.LoadScene("Game");
        }

        private System.Collections.IEnumerator FadeIn()
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
        #endregion
    }
}
