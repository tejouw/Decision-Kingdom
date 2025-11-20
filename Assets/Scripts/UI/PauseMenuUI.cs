using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DecisionKingdom.Core;
using DecisionKingdom.Managers;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Pause menu UI komponenti
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _confirmQuitPanel;

        [Header("Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _quitButton;

        [Header("Confirm Quit")]
        [SerializeField] private Button _confirmQuitYesButton;
        [SerializeField] private Button _confirmQuitNoButton;

        [Header("Info Display")]
        [SerializeField] private Text _turnText;
        [SerializeField] private Text _eraText;
        [SerializeField] private Text _playTimeText;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.2f;

        private bool _isPaused;

        #region Properties
        public bool IsPaused => _isPaused;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
            HideAllPanels();
        }

        private void Update()
        {
            // Escape tusu ile pause toggle
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                    Resume();
                else
                    Pause();
            }
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_resumeButton != null)
                _resumeButton.onClick.AddListener(Resume);

            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(ShowSettings);

            if (_saveButton != null)
                _saveButton.onClick.AddListener(SaveGame);

            if (_mainMenuButton != null)
                _mainMenuButton.onClick.AddListener(ShowConfirmQuit);

            if (_quitButton != null)
                _quitButton.onClick.AddListener(QuitGame);

            if (_confirmQuitYesButton != null)
                _confirmQuitYesButton.onClick.AddListener(ReturnToMainMenu);

            if (_confirmQuitNoButton != null)
                _confirmQuitNoButton.onClick.AddListener(HideConfirmQuit);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Oyunu duraklat
        /// </summary>
        public void Pause()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
                return;

            _isPaused = true;
            GameManager.Instance.PauseGame();
            Time.timeScale = 0f;

            UpdateUI();
            ShowPausePanel();

            Debug.Log("[PauseMenuUI] Oyun duraklatildi");
        }

        /// <summary>
        /// Oyuna devam et
        /// </summary>
        public void Resume()
        {
            _isPaused = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResumeGame();
            }

            Time.timeScale = 1f;
            HideAllPanels();

            Debug.Log("[PauseMenuUI] Oyun devam ediyor");
        }

        /// <summary>
        /// Oyunu kaydet
        /// </summary>
        public void SaveGame()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveGame();
                Debug.Log("[PauseMenuUI] Oyun kaydedildi");

                // Kullaniciya bildir (kisa sure buton metnini degistir)
                if (_saveButton != null)
                {
                    var buttonText = _saveButton.GetComponentInChildren<Text>();
                    if (buttonText != null)
                    {
                        StartCoroutine(ShowSaveConfirmation(buttonText));
                    }
                }
            }
        }

        /// <summary>
        /// Ana menuye don
        /// </summary>
        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            _isPaused = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToMainMenu();
            }

            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Oyundan cik
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region Panel Management
        private void ShowPausePanel()
        {
            HideAllPanels();
            if (_pausePanel != null)
            {
                _pausePanel.SetActive(true);
                if (_canvasGroup != null)
                {
                    StartCoroutine(FadeIn());
                }
            }
        }

        private void ShowSettings()
        {
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.SetActive(true);
        }

        private void ShowConfirmQuit()
        {
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_confirmQuitPanel != null) _confirmQuitPanel.SetActive(true);
        }

        private void HideConfirmQuit()
        {
            if (_confirmQuitPanel != null) _confirmQuitPanel.SetActive(false);
            if (_pausePanel != null) _pausePanel.SetActive(true);
        }

        private void HideAllPanels()
        {
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.SetActive(false);
            if (_confirmQuitPanel != null) _confirmQuitPanel.SetActive(false);
        }
        #endregion

        #region UI Updates
        private void UpdateUI()
        {
            if (GameManager.Instance == null) return;

            // Tur bilgisi
            if (_turnText != null)
            {
                _turnText.text = $"Tur: {GameManager.Instance.CurrentTurn}";
            }

            // Era bilgisi
            if (_eraText != null)
            {
                string eraName = PrestigeManager.EraNames.ContainsKey(GameManager.Instance.CurrentEra)
                    ? PrestigeManager.EraNames[GameManager.Instance.CurrentEra]
                    : GameManager.Instance.CurrentEra.ToString();
                _eraText.text = $"Donem: {eraName}";
            }

            // Oynama suresi
            if (_playTimeText != null)
            {
                var duration = GameManager.Instance.GetSessionDuration();
                _playTimeText.text = $"Sure: {duration.Minutes:D2}:{duration.Seconds:D2}";
            }
        }
        #endregion

        #region Coroutines
        private System.Collections.IEnumerator FadeIn()
        {
            _canvasGroup.alpha = 0f;

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime; // unscaled cunku timeScale = 0
                _canvasGroup.alpha = elapsed / _fadeDuration;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }

        private System.Collections.IEnumerator ShowSaveConfirmation(Text buttonText)
        {
            string originalText = buttonText.text;
            buttonText.text = "Kaydedildi!";

            yield return new WaitForSecondsRealtime(1.5f);

            buttonText.text = originalText;
        }
        #endregion
    }
}
