using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Ayarlar UI komponenti
    /// </summary>
    public class SettingsUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _settingsPanel;

        [Header("Audio Settings")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Toggle _muteToggle;

        [Header("Gameplay Settings")]
        [SerializeField] private Toggle _vibrationToggle;
        [SerializeField] private Toggle _tutorialToggle;
        [SerializeField] private Toggle _autoSaveToggle;
        [SerializeField] private Dropdown _languageDropdown;

        [Header("Graphics Settings")]
        [SerializeField] private Toggle _particlesToggle;
        [SerializeField] private Toggle _screenShakeToggle;
        [SerializeField] private Dropdown _qualityDropdown;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _creditsButton;

        [Header("Info")]
        [SerializeField] private Text _versionText;

        // PlayerPrefs keys
        private const string KEY_MASTER_VOLUME = "MasterVolume";
        private const string KEY_MUSIC_VOLUME = "MusicVolume";
        private const string KEY_SFX_VOLUME = "SFXVolume";
        private const string KEY_MUTE = "Mute";
        private const string KEY_VIBRATION = "Vibration";
        private const string KEY_TUTORIAL = "Tutorial";
        private const string KEY_AUTO_SAVE = "AutoSave";
        private const string KEY_LANGUAGE = "Language";
        private const string KEY_PARTICLES = "Particles";
        private const string KEY_SCREEN_SHAKE = "ScreenShake";
        private const string KEY_QUALITY = "Quality";

        // Events
        public event System.Action OnBackClicked;
        public event System.Action OnCreditsClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupListeners();
        }

        private void Start()
        {
            LoadSettings();
            UpdateVersionText();
        }
        #endregion

        #region Setup
        private void SetupListeners()
        {
            // Audio
            if (_masterVolumeSlider != null)
                _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

            if (_musicVolumeSlider != null)
                _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

            if (_sfxVolumeSlider != null)
                _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            if (_muteToggle != null)
                _muteToggle.onValueChanged.AddListener(OnMuteChanged);

            // Gameplay
            if (_vibrationToggle != null)
                _vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);

            if (_tutorialToggle != null)
                _tutorialToggle.onValueChanged.AddListener(OnTutorialChanged);

            if (_autoSaveToggle != null)
                _autoSaveToggle.onValueChanged.AddListener(OnAutoSaveChanged);

            if (_languageDropdown != null)
                _languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            // Graphics
            if (_particlesToggle != null)
                _particlesToggle.onValueChanged.AddListener(OnParticlesChanged);

            if (_screenShakeToggle != null)
                _screenShakeToggle.onValueChanged.AddListener(OnScreenShakeChanged);

            if (_qualityDropdown != null)
                _qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

            // Buttons
            if (_backButton != null)
                _backButton.onClick.AddListener(OnBackButtonClicked);

            if (_resetButton != null)
                _resetButton.onClick.AddListener(ResetToDefaults);

            if (_creditsButton != null)
                _creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        }
        #endregion

        #region Load/Save Settings
        private void LoadSettings()
        {
            // Audio
            if (_masterVolumeSlider != null)
                _masterVolumeSlider.value = PlayerPrefs.GetFloat(KEY_MASTER_VOLUME, 1f);

            if (_musicVolumeSlider != null)
                _musicVolumeSlider.value = PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, 0.8f);

            if (_sfxVolumeSlider != null)
                _sfxVolumeSlider.value = PlayerPrefs.GetFloat(KEY_SFX_VOLUME, 1f);

            if (_muteToggle != null)
                _muteToggle.isOn = PlayerPrefs.GetInt(KEY_MUTE, 0) == 1;

            // Gameplay
            if (_vibrationToggle != null)
                _vibrationToggle.isOn = PlayerPrefs.GetInt(KEY_VIBRATION, 1) == 1;

            if (_tutorialToggle != null)
                _tutorialToggle.isOn = PlayerPrefs.GetInt(KEY_TUTORIAL, 1) == 1;

            if (_autoSaveToggle != null)
                _autoSaveToggle.isOn = PlayerPrefs.GetInt(KEY_AUTO_SAVE, 1) == 1;

            if (_languageDropdown != null)
                _languageDropdown.value = PlayerPrefs.GetInt(KEY_LANGUAGE, 0);

            // Graphics
            if (_particlesToggle != null)
                _particlesToggle.isOn = PlayerPrefs.GetInt(KEY_PARTICLES, 1) == 1;

            if (_screenShakeToggle != null)
                _screenShakeToggle.isOn = PlayerPrefs.GetInt(KEY_SCREEN_SHAKE, 1) == 1;

            if (_qualityDropdown != null)
                _qualityDropdown.value = PlayerPrefs.GetInt(KEY_QUALITY, QualitySettings.GetQualityLevel());
        }

        private void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Varsayilan ayarlara don
        /// </summary>
        public void ResetToDefaults()
        {
            // Audio
            if (_masterVolumeSlider != null) _masterVolumeSlider.value = 1f;
            if (_musicVolumeSlider != null) _musicVolumeSlider.value = 0.8f;
            if (_sfxVolumeSlider != null) _sfxVolumeSlider.value = 1f;
            if (_muteToggle != null) _muteToggle.isOn = false;

            // Gameplay
            if (_vibrationToggle != null) _vibrationToggle.isOn = true;
            if (_tutorialToggle != null) _tutorialToggle.isOn = true;
            if (_autoSaveToggle != null) _autoSaveToggle.isOn = true;
            if (_languageDropdown != null) _languageDropdown.value = 0;

            // Graphics
            if (_particlesToggle != null) _particlesToggle.isOn = true;
            if (_screenShakeToggle != null) _screenShakeToggle.isOn = true;
            if (_qualityDropdown != null) _qualityDropdown.value = 2; // Medium

            SaveSettings();
            Debug.Log("[SettingsUI] Ayarlar sifirlandi");
        }
        #endregion

        #region Setting Handlers
        private void OnMasterVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat(KEY_MASTER_VOLUME, value);
            AudioListener.volume = _muteToggle != null && _muteToggle.isOn ? 0f : value;
            SaveSettings();
        }

        private void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, value);
            // AudioManager.Instance?.SetMusicVolume(value);
            SaveSettings();
        }

        private void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat(KEY_SFX_VOLUME, value);
            // AudioManager.Instance?.SetSFXVolume(value);
            SaveSettings();
        }

        private void OnMuteChanged(bool isMuted)
        {
            PlayerPrefs.SetInt(KEY_MUTE, isMuted ? 1 : 0);
            AudioListener.volume = isMuted ? 0f : (_masterVolumeSlider?.value ?? 1f);
            SaveSettings();
        }

        private void OnVibrationChanged(bool enabled)
        {
            PlayerPrefs.SetInt(KEY_VIBRATION, enabled ? 1 : 0);
            SaveSettings();
        }

        private void OnTutorialChanged(bool enabled)
        {
            PlayerPrefs.SetInt(KEY_TUTORIAL, enabled ? 1 : 0);
            SaveSettings();
        }

        private void OnAutoSaveChanged(bool enabled)
        {
            PlayerPrefs.SetInt(KEY_AUTO_SAVE, enabled ? 1 : 0);
            SaveSettings();
        }

        private void OnLanguageChanged(int index)
        {
            PlayerPrefs.SetInt(KEY_LANGUAGE, index);
            // LocalizationManager.Instance?.SetLanguage(index);
            SaveSettings();
        }

        private void OnParticlesChanged(bool enabled)
        {
            PlayerPrefs.SetInt(KEY_PARTICLES, enabled ? 1 : 0);
            SaveSettings();
        }

        private void OnScreenShakeChanged(bool enabled)
        {
            PlayerPrefs.SetInt(KEY_SCREEN_SHAKE, enabled ? 1 : 0);
            SaveSettings();
        }

        private void OnQualityChanged(int index)
        {
            PlayerPrefs.SetInt(KEY_QUALITY, index);
            QualitySettings.SetQualityLevel(index);
            SaveSettings();
        }

        private void OnBackButtonClicked()
        {
            if (_settingsPanel != null)
                _settingsPanel.SetActive(false);

            OnBackClicked?.Invoke();
        }

        private void OnCreditsButtonClicked()
        {
            OnCreditsClicked?.Invoke();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Ayarlar panelini goster
        /// </summary>
        public void Show()
        {
            if (_settingsPanel != null)
                _settingsPanel.SetActive(true);

            LoadSettings();
        }

        /// <summary>
        /// Ayarlar panelini gizle
        /// </summary>
        public void Hide()
        {
            if (_settingsPanel != null)
                _settingsPanel.SetActive(false);
        }

        /// <summary>
        /// Belirli bir ayari al
        /// </summary>
        public static bool GetBoolSetting(string key, bool defaultValue = true)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static float GetFloatSetting(string key, float defaultValue = 1f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static int GetIntSetting(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
        #endregion

        #region Helper Methods
        private void UpdateVersionText()
        {
            if (_versionText != null)
            {
                _versionText.text = $"v{Application.version}";
            }
        }
        #endregion
    }
}
