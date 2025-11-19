using UnityEngine;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Haptic (titresim) geri bildirim sistemi - Faz 8
    /// </summary>
    public class HapticFeedback : MonoBehaviour
    {
        public static HapticFeedback Instance { get; private set; }

        [Header("Ayarlar")]
        [SerializeField] private bool _enabled = true;

        private const string HAPTIC_ENABLED_KEY = "HapticEnabled";

        #region Properties
        public bool IsEnabled => _enabled;
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

            LoadPreferences();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Haptic'i etkinlestir/devre disi birak
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
            SavePreferences();
        }

        /// <summary>
        /// Hafif titresim - UI etkilesimleri
        /// </summary>
        public void Light()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Light);
        }

        /// <summary>
        /// Orta titresim - Kart kaydirma
        /// </summary>
        public void Medium()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Medium);
        }

        /// <summary>
        /// Agir titresim - Onemli olaylar
        /// </summary>
        public void Heavy()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Heavy);
        }

        /// <summary>
        /// Basari titresimi
        /// </summary>
        public void Success()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Success);
        }

        /// <summary>
        /// Uyari titresimi
        /// </summary>
        public void Warning()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Warning);
        }

        /// <summary>
        /// Hata titresimi
        /// </summary>
        public void Error()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Error);
        }

        /// <summary>
        /// Secim titresimi - Kart karari
        /// </summary>
        public void Selection()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.Selection);
        }

        /// <summary>
        /// Impact titresimi - Kaynak degisimi
        /// </summary>
        public void ImpactLight()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.ImpactLight);
        }

        /// <summary>
        /// Rigid impact - Kritik kaynak
        /// </summary>
        public void ImpactRigid()
        {
            if (!_enabled)
                return;

            TriggerHaptic(HapticType.ImpactRigid);
        }
        #endregion

        #region Private Methods
        private void TriggerHaptic(HapticType type)
        {
#if UNITY_IOS
            TriggerIOSHaptic(type);
#elif UNITY_ANDROID
            TriggerAndroidHaptic(type);
#endif
        }

#if UNITY_IOS
        private void TriggerIOSHaptic(HapticType type)
        {
            // iOS Haptic Engine kullanarak daha ince kontrol
            // UIImpactFeedbackGenerator, UINotificationFeedbackGenerator vs.

            switch (type)
            {
                case HapticType.Light:
                case HapticType.Selection:
                case HapticType.ImpactLight:
                    // Light impact
                    Handheld.Vibrate();
                    break;

                case HapticType.Medium:
                    // Medium impact
                    Handheld.Vibrate();
                    break;

                case HapticType.Heavy:
                case HapticType.ImpactRigid:
                    // Heavy impact
                    Handheld.Vibrate();
                    break;

                case HapticType.Success:
                    // Success notification
                    Handheld.Vibrate();
                    break;

                case HapticType.Warning:
                    // Warning notification
                    Handheld.Vibrate();
                    break;

                case HapticType.Error:
                    // Error notification
                    Handheld.Vibrate();
                    break;
            }

            // Not: Gercek iOS haptic icin native plugin kullanilmali
            // Ornek: iOS.HapticFeedback.ImpactOccurred(style);
        }
#endif

#if UNITY_ANDROID
        private void TriggerAndroidHaptic(HapticType type)
        {
            // Android VibrationEffect kullanarak
            // VibrationEffect.createOneShot veya createWaveform

            long[] pattern;

            switch (type)
            {
                case HapticType.Light:
                case HapticType.Selection:
                case HapticType.ImpactLight:
                    pattern = new long[] { 0, 10 };
                    break;

                case HapticType.Medium:
                    pattern = new long[] { 0, 20 };
                    break;

                case HapticType.Heavy:
                case HapticType.ImpactRigid:
                    pattern = new long[] { 0, 40 };
                    break;

                case HapticType.Success:
                    pattern = new long[] { 0, 10, 50, 10 };
                    break;

                case HapticType.Warning:
                    pattern = new long[] { 0, 20, 50, 20 };
                    break;

                case HapticType.Error:
                    pattern = new long[] { 0, 50, 50, 50 };
                    break;

                default:
                    pattern = new long[] { 0, 20 };
                    break;
            }

            // Basit fallback - gercek implementasyon icin native plugin
            Handheld.Vibrate();

            // Not: Gercek Android haptic icin:
            // AndroidJavaClass vibrator = ...
            // vibrator.Call("vibrate", VibrationEffect.createWaveform(pattern, -1));
        }
#endif

        private void SavePreferences()
        {
            PlayerPrefs.SetInt(HAPTIC_ENABLED_KEY, _enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadPreferences()
        {
            _enabled = PlayerPrefs.GetInt(HAPTIC_ENABLED_KEY, 1) == 1;
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Test Light Haptic")]
        private void DebugTestLight()
        {
            Light();
            Debug.Log("[HapticFeedback] Light haptic triggered");
        }

        [ContextMenu("Test Heavy Haptic")]
        private void DebugTestHeavy()
        {
            Heavy();
            Debug.Log("[HapticFeedback] Heavy haptic triggered");
        }

        [ContextMenu("Test Success Haptic")]
        private void DebugTestSuccess()
        {
            Success();
            Debug.Log("[HapticFeedback] Success haptic triggered");
        }
#endif
        #endregion
    }

    /// <summary>
    /// Haptic turleri
    /// </summary>
    public enum HapticType
    {
        Light,
        Medium,
        Heavy,
        Success,
        Warning,
        Error,
        Selection,
        ImpactLight,
        ImpactRigid
    }
}
