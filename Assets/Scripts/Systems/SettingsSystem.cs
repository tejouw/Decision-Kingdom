using System;
using UnityEngine;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Oyun ayarlari yonetim sistemi - Faz 8.5
    /// </summary>
    public class SettingsSystem : MonoBehaviour
    {
        public static SettingsSystem Instance { get; private set; }

        [Header("Varsayilan Ayarlar")]
        [SerializeField] private GameSettings _defaultSettings;

        private GameSettings _currentSettings;

        // Events
        public event Action<GameSettings> OnSettingsChanged;
        public event Action OnSettingsReset;

        private const string SETTINGS_KEY = "GameSettings";

        #region Properties
        public GameSettings CurrentSettings => _currentSettings;
        public bool HapticsEnabled => _currentSettings.hapticsEnabled;
        public bool NotificationsEnabled => _currentSettings.notificationsEnabled;
        public bool AutoSaveEnabled => _currentSettings.autoSaveEnabled;
        public int TargetFrameRate => _currentSettings.targetFrameRate;
        public bool ShowFPS => _currentSettings.showFPS;
        public bool ScreenShakeEnabled => _currentSettings.screenShakeEnabled;
        public float TextSize => _currentSettings.textSize;
        public bool HighContrastMode => _currentSettings.highContrastMode;
        public bool ReducedMotion => _currentSettings.reducedMotion;
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

            InitializeDefaultSettings();
            LoadSettings();
            ApplySettings();
        }
        #endregion

        #region Public Methods - Settings Management
        /// <summary>
        /// Ayarlari kaydet
        /// </summary>
        public void SaveSettings()
        {
            string json = JsonUtility.ToJson(_currentSettings);
            PlayerPrefs.SetString(SETTINGS_KEY, json);
            PlayerPrefs.Save();

            Debug.Log("[SettingsSystem] Ayarlar kaydedildi");
        }

        /// <summary>
        /// Ayarlari yukle
        /// </summary>
        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey(SETTINGS_KEY))
            {
                string json = PlayerPrefs.GetString(SETTINGS_KEY);
                _currentSettings = JsonUtility.FromJson<GameSettings>(json);
            }
            else
            {
                _currentSettings = new GameSettings(_defaultSettings);
            }

            Debug.Log("[SettingsSystem] Ayarlar yuklendi");
        }

        /// <summary>
        /// Ayarlari varsayilana sifirla
        /// </summary>
        public void ResetToDefaults()
        {
            _currentSettings = new GameSettings(_defaultSettings);
            SaveSettings();
            ApplySettings();
            OnSettingsReset?.Invoke();

            Debug.Log("[SettingsSystem] Ayarlar varsayilana sifirlandi");
        }

        /// <summary>
        /// Ayarlari uygula
        /// </summary>
        public void ApplySettings()
        {
            // Frame rate
            Application.targetFrameRate = _currentSettings.targetFrameRate;

            // VSync
            QualitySettings.vSyncCount = _currentSettings.vSyncEnabled ? 1 : 0;

            // Screen orientation
            Screen.orientation = _currentSettings.screenOrientation;

            // Audio sistemine bildir
            if (AudioSystem.Instance != null)
            {
                AudioSystem.Instance.SetMasterVolume(_currentSettings.masterVolume);
                AudioSystem.Instance.SetMusicVolume(_currentSettings.musicVolume);
                AudioSystem.Instance.SetSfxVolume(_currentSettings.sfxVolume);
            }

            // Localization sistemine bildir
            if (LocalizationSystem.Instance != null)
            {
                LocalizationSystem.Instance.SetLanguage(_currentSettings.language);
            }

            OnSettingsChanged?.Invoke(_currentSettings);
        }
        #endregion

        #region Public Methods - Individual Settings
        /// <summary>
        /// Haptic feedback ayarla
        /// </summary>
        public void SetHapticsEnabled(bool enabled)
        {
            _currentSettings.hapticsEnabled = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Bildirimler ayarla
        /// </summary>
        public void SetNotificationsEnabled(bool enabled)
        {
            _currentSettings.notificationsEnabled = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Otomatik kaydetme ayarla
        /// </summary>
        public void SetAutoSaveEnabled(bool enabled)
        {
            _currentSettings.autoSaveEnabled = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// FPS gosterimi ayarla
        /// </summary>
        public void SetShowFPS(bool show)
        {
            _currentSettings.showFPS = show;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Ekran sallanmasi ayarla
        /// </summary>
        public void SetScreenShakeEnabled(bool enabled)
        {
            _currentSettings.screenShakeEnabled = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Metin boyutu ayarla
        /// </summary>
        public void SetTextSize(float size)
        {
            _currentSettings.textSize = Mathf.Clamp(size, 0.75f, 1.5f);
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Yuksek kontrast modu ayarla
        /// </summary>
        public void SetHighContrastMode(bool enabled)
        {
            _currentSettings.highContrastMode = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Azaltilmis hareket ayarla
        /// </summary>
        public void SetReducedMotion(bool enabled)
        {
            _currentSettings.reducedMotion = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Target frame rate ayarla
        /// </summary>
        public void SetTargetFrameRate(int fps)
        {
            _currentSettings.targetFrameRate = Mathf.Clamp(fps, 30, 120);
            Application.targetFrameRate = _currentSettings.targetFrameRate;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// VSync ayarla
        /// </summary>
        public void SetVSyncEnabled(bool enabled)
        {
            _currentSettings.vSyncEnabled = enabled;
            QualitySettings.vSyncCount = enabled ? 1 : 0;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Dil ayarla
        /// </summary>
        public void SetLanguage(SystemLanguage language)
        {
            _currentSettings.language = language;
            if (LocalizationSystem.Instance != null)
            {
                LocalizationSystem.Instance.SetLanguage(language);
            }
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Ses ayarlarini toplu guncelle
        /// </summary>
        public void SetAudioSettings(float master, float music, float sfx)
        {
            _currentSettings.masterVolume = Mathf.Clamp01(master);
            _currentSettings.musicVolume = Mathf.Clamp01(music);
            _currentSettings.sfxVolume = Mathf.Clamp01(sfx);

            if (AudioSystem.Instance != null)
            {
                AudioSystem.Instance.SetMasterVolume(_currentSettings.masterVolume);
                AudioSystem.Instance.SetMusicVolume(_currentSettings.musicVolume);
                AudioSystem.Instance.SetSfxVolume(_currentSettings.sfxVolume);
            }

            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Analytics iznini ayarla
        /// </summary>
        public void SetAnalyticsEnabled(bool enabled)
        {
            _currentSettings.analyticsEnabled = enabled;
            if (AnalyticsSystem.Instance != null)
            {
                AnalyticsSystem.Instance.SetEnabled(enabled);
            }
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }

        /// <summary>
        /// Kisisellestirılmis reklamlari ayarla
        /// </summary>
        public void SetPersonalizedAdsEnabled(bool enabled)
        {
            _currentSettings.personalizedAdsEnabled = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke(_currentSettings);
        }
        #endregion

        #region Public Methods - Haptic Feedback
        /// <summary>
        /// Hafif haptic feedback
        /// </summary>
        public void TriggerLightHaptic()
        {
            if (!_currentSettings.hapticsEnabled)
                return;

#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        /// <summary>
        /// Orta haptic feedback
        /// </summary>
        public void TriggerMediumHaptic()
        {
            if (!_currentSettings.hapticsEnabled)
                return;

#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        /// <summary>
        /// Guclu haptic feedback
        /// </summary>
        public void TriggerHeavyHaptic()
        {
            if (!_currentSettings.hapticsEnabled)
                return;

#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }
        #endregion

        #region Private Methods
        private void InitializeDefaultSettings()
        {
            _defaultSettings = new GameSettings
            {
                // Audio
                masterVolume = 1f,
                musicVolume = 0.7f,
                sfxVolume = 1f,

                // General
                language = SystemLanguage.Turkish,
                hapticsEnabled = true,
                notificationsEnabled = true,
                autoSaveEnabled = true,

                // Graphics
                targetFrameRate = 60,
                vSyncEnabled = false,
                screenOrientation = ScreenOrientation.Portrait,
                showFPS = false,
                screenShakeEnabled = true,

                // Accessibility
                textSize = 1f,
                highContrastMode = false,
                reducedMotion = false,

                // Privacy
                analyticsEnabled = true,
                personalizedAdsEnabled = true
            };
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Reset Settings")]
        private void DebugResetSettings()
        {
            ResetToDefaults();
        }

        [ContextMenu("Print Current Settings")]
        private void DebugPrintSettings()
        {
            Debug.Log($"Master Volume: {_currentSettings.masterVolume}");
            Debug.Log($"Music Volume: {_currentSettings.musicVolume}");
            Debug.Log($"SFX Volume: {_currentSettings.sfxVolume}");
            Debug.Log($"Language: {_currentSettings.language}");
            Debug.Log($"Haptics: {_currentSettings.hapticsEnabled}");
            Debug.Log($"Target FPS: {_currentSettings.targetFrameRate}");
        }
#endif
        #endregion
    }

    /// <summary>
    /// Oyun ayarlari veri yapisi
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        // Audio Settings
        [Range(0f, 1f)] public float masterVolume;
        [Range(0f, 1f)] public float musicVolume;
        [Range(0f, 1f)] public float sfxVolume;

        // General Settings
        public SystemLanguage language;
        public bool hapticsEnabled;
        public bool notificationsEnabled;
        public bool autoSaveEnabled;

        // Graphics Settings
        public int targetFrameRate;
        public bool vSyncEnabled;
        public ScreenOrientation screenOrientation;
        public bool showFPS;
        public bool screenShakeEnabled;

        // Accessibility Settings
        [Range(0.75f, 1.5f)] public float textSize;
        public bool highContrastMode;
        public bool reducedMotion;

        // Privacy Settings
        public bool analyticsEnabled;
        public bool personalizedAdsEnabled;

        // Constructor for default values
        public GameSettings()
        {
            masterVolume = 1f;
            musicVolume = 0.7f;
            sfxVolume = 1f;
            language = SystemLanguage.Turkish;
            hapticsEnabled = true;
            notificationsEnabled = true;
            autoSaveEnabled = true;
            targetFrameRate = 60;
            vSyncEnabled = false;
            screenOrientation = ScreenOrientation.Portrait;
            showFPS = false;
            screenShakeEnabled = true;
            textSize = 1f;
            highContrastMode = false;
            reducedMotion = false;
            analyticsEnabled = true;
            personalizedAdsEnabled = true;
        }

        // Copy constructor
        public GameSettings(GameSettings other)
        {
            masterVolume = other.masterVolume;
            musicVolume = other.musicVolume;
            sfxVolume = other.sfxVolume;
            language = other.language;
            hapticsEnabled = other.hapticsEnabled;
            notificationsEnabled = other.notificationsEnabled;
            autoSaveEnabled = other.autoSaveEnabled;
            targetFrameRate = other.targetFrameRate;
            vSyncEnabled = other.vSyncEnabled;
            screenOrientation = other.screenOrientation;
            showFPS = other.showFPS;
            screenShakeEnabled = other.screenShakeEnabled;
            textSize = other.textSize;
            highContrastMode = other.highContrastMode;
            reducedMotion = other.reducedMotion;
            analyticsEnabled = other.analyticsEnabled;
            personalizedAdsEnabled = other.personalizedAdsEnabled;
        }
    }
}
