using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Analitik ve telemetri sistemi - Faz 8.4
    /// </summary>
    public class AnalyticsSystem : MonoBehaviour
    {
        public static AnalyticsSystem Instance { get; private set; }

        [Header("Ayarlar")]
        [SerializeField] private bool _analyticsEnabled = true;
        [SerializeField] private bool _debugMode = false;

        // Events
        public event Action<AnalyticsEvent> OnEventTracked;

        private Queue<AnalyticsEvent> _eventQueue;
        private bool _isInitialized;
        private string _sessionId;
        private DateTime _sessionStartTime;

        private const string ANALYTICS_ENABLED_KEY = "AnalyticsEnabled";
        private const int MAX_QUEUE_SIZE = 100;

        #region Properties
        public bool IsEnabled => _analyticsEnabled;
        public bool IsInitialized => _isInitialized;
        public string SessionId => _sessionId;
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

            _eventQueue = new Queue<AnalyticsEvent>();
            LoadPreferences();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                TrackEvent(AnalyticsEventType.AppBackground);
            }
            else
            {
                TrackEvent(AnalyticsEventType.AppForeground);
            }
        }

        private void OnApplicationQuit()
        {
            TrackSessionEnd();
            FlushEvents();
        }
        #endregion

        #region Public Methods - Initialization
        /// <summary>
        /// Analytics sistemini baslatir
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
                return;

            _sessionId = GenerateSessionId();
            _sessionStartTime = DateTime.UtcNow;

            // Firebase veya Amplitude SDK'sini burada baslatacaksiniz
            // Firebase.FirebaseApp.CheckAndFixDependenciesAsync()...

            _isInitialized = true;
            TrackSessionStart();

            Debug.Log($"[AnalyticsSystem] Baslatildi - Session: {_sessionId}");
        }

        /// <summary>
        /// Analytics'i etkinlestir/devre disi birak
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            _analyticsEnabled = enabled;
            SavePreferences();

            if (enabled)
            {
                Debug.Log("[AnalyticsSystem] Analytics etkinlestirildi");
            }
            else
            {
                Debug.Log("[AnalyticsSystem] Analytics devre disi birakildi");
            }
        }
        #endregion

        #region Public Methods - Event Tracking
        /// <summary>
        /// Basit event takibi
        /// </summary>
        public void TrackEvent(AnalyticsEventType eventType)
        {
            TrackEvent(eventType, null);
        }

        /// <summary>
        /// Parametreli event takibi
        /// </summary>
        public void TrackEvent(AnalyticsEventType eventType, Dictionary<string, object> parameters)
        {
            if (!_analyticsEnabled)
                return;

            var analyticsEvent = new AnalyticsEvent
            {
                eventType = eventType,
                timestamp = DateTime.UtcNow,
                sessionId = _sessionId,
                parameters = parameters ?? new Dictionary<string, object>()
            };

            // Queue'ya ekle
            if (_eventQueue.Count >= MAX_QUEUE_SIZE)
            {
                _eventQueue.Dequeue();
            }
            _eventQueue.Enqueue(analyticsEvent);

            // Event'i gonder
            SendEvent(analyticsEvent);

            OnEventTracked?.Invoke(analyticsEvent);

            if (_debugMode)
            {
                Debug.Log($"[Analytics] {eventType}: {SerializeParameters(parameters)}");
            }
        }

        /// <summary>
        /// Oturum baslangici
        /// </summary>
        public void TrackSessionStart()
        {
            var parameters = new Dictionary<string, object>
            {
                { "platform", Application.platform.ToString() },
                { "version", Application.version },
                { "device_model", SystemInfo.deviceModel },
                { "os_version", SystemInfo.operatingSystem },
                { "language", Application.systemLanguage.ToString() }
            };

            TrackEvent(AnalyticsEventType.SessionStart, parameters);
        }

        /// <summary>
        /// Oturum sonu
        /// </summary>
        public void TrackSessionEnd()
        {
            var duration = (DateTime.UtcNow - _sessionStartTime).TotalSeconds;

            var parameters = new Dictionary<string, object>
            {
                { "duration_seconds", duration }
            };

            TrackEvent(AnalyticsEventType.SessionEnd, parameters);
        }

        /// <summary>
        /// Oyun baslangici
        /// </summary>
        public void TrackGameStart(Era era, string scenario = "default")
        {
            var parameters = new Dictionary<string, object>
            {
                { "era", era.ToString() },
                { "scenario", scenario }
            };

            TrackEvent(AnalyticsEventType.GameStart, parameters);
        }

        /// <summary>
        /// Oyun sonu
        /// </summary>
        public void TrackGameEnd(GameOverReason reason, int turns, int gold, int happiness, int military, int faith)
        {
            var parameters = new Dictionary<string, object>
            {
                { "reason", reason.ToString() },
                { "turns", turns },
                { "gold", gold },
                { "happiness", happiness },
                { "military", military },
                { "faith", faith }
            };

            TrackEvent(AnalyticsEventType.GameEnd, parameters);
        }

        /// <summary>
        /// Kart karari
        /// </summary>
        public void TrackCardDecision(string eventId, bool choseRight, int turn)
        {
            var parameters = new Dictionary<string, object>
            {
                { "event_id", eventId },
                { "choice", choseRight ? "right" : "left" },
                { "turn", turn }
            };

            TrackEvent(AnalyticsEventType.CardDecision, parameters);
        }

        /// <summary>
        /// Basarim acma
        /// </summary>
        public void TrackAchievementUnlock(string achievementId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "achievement_id", achievementId }
            };

            TrackEvent(AnalyticsEventType.AchievementUnlock, parameters);
        }

        /// <summary>
        /// Era degisimi
        /// </summary>
        public void TrackEraChange(Era fromEra, Era toEra)
        {
            var parameters = new Dictionary<string, object>
            {
                { "from_era", fromEra.ToString() },
                { "to_era", toEra.ToString() }
            };

            TrackEvent(AnalyticsEventType.EraChange, parameters);
        }

        /// <summary>
        /// Satin alma
        /// </summary>
        public void TrackPurchase(string productId, float price, string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                { "product_id", productId },
                { "price", price },
                { "currency", currency }
            };

            TrackEvent(AnalyticsEventType.Purchase, parameters);
        }

        /// <summary>
        /// Reklam izleme
        /// </summary>
        public void TrackAdWatched(string adType, bool completed)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ad_type", adType },
                { "completed", completed }
            };

            TrackEvent(AnalyticsEventType.AdWatched, parameters);
        }

        /// <summary>
        /// Tutorial ilerleme
        /// </summary>
        public void TrackTutorialStep(int step, string stepId, bool skipped = false)
        {
            var parameters = new Dictionary<string, object>
            {
                { "step", step },
                { "step_id", stepId },
                { "skipped", skipped }
            };

            TrackEvent(AnalyticsEventType.TutorialStep, parameters);
        }

        /// <summary>
        /// Tutorial tamamlama
        /// </summary>
        public void TrackTutorialComplete(bool skipped)
        {
            var parameters = new Dictionary<string, object>
            {
                { "skipped", skipped }
            };

            TrackEvent(AnalyticsEventType.TutorialComplete, parameters);
        }

        /// <summary>
        /// Gunluk macera katilimi
        /// </summary>
        public void TrackDailyChallengeParticipation(int score, int rank)
        {
            var parameters = new Dictionary<string, object>
            {
                { "score", score },
                { "rank", rank }
            };

            TrackEvent(AnalyticsEventType.DailyChallenge, parameters);
        }

        /// <summary>
        /// Paylasim
        /// </summary>
        public void TrackShare(string platform)
        {
            var parameters = new Dictionary<string, object>
            {
                { "platform", platform }
            };

            TrackEvent(AnalyticsEventType.Share, parameters);
        }

        /// <summary>
        /// Hata takibi
        /// </summary>
        public void TrackError(string errorType, string message, string stackTrace = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "error_type", errorType },
                { "message", message }
            };

            if (!string.IsNullOrEmpty(stackTrace))
            {
                parameters["stack_trace"] = stackTrace.Substring(0, Math.Min(stackTrace.Length, 500));
            }

            TrackEvent(AnalyticsEventType.Error, parameters);
        }

        /// <summary>
        /// Kullanici ozelligi ayarla
        /// </summary>
        public void SetUserProperty(string name, string value)
        {
            if (!_analyticsEnabled)
                return;

            // Firebase veya Amplitude'da user property ayarlama
            // Firebase.Analytics.FirebaseAnalytics.SetUserProperty(name, value);

            if (_debugMode)
            {
                Debug.Log($"[Analytics] User Property: {name} = {value}");
            }
        }
        #endregion

        #region Private Methods
        private void SendEvent(AnalyticsEvent analyticsEvent)
        {
            // Gercek implementasyonda burasi Firebase/Amplitude'a gonderir
            // Firebase.Analytics.FirebaseAnalytics.LogEvent(analyticsEvent.eventType.ToString(), ...);

            // Simdilik sadece debug log
            if (_debugMode)
            {
                string eventName = analyticsEvent.eventType.ToString();
                // Event gonderildi varsayalim
            }
        }

        private void FlushEvents()
        {
            // Bekleyen eventleri gonder
            while (_eventQueue.Count > 0)
            {
                var evt = _eventQueue.Dequeue();
                SendEvent(evt);
            }
        }

        private string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

        private string SerializeParameters(Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return "{}";

            var parts = new List<string>();
            foreach (var kvp in parameters)
            {
                parts.Add($"{kvp.Key}={kvp.Value}");
            }
            return "{" + string.Join(", ", parts) + "}";
        }

        private void SavePreferences()
        {
            PlayerPrefs.SetInt(ANALYTICS_ENABLED_KEY, _analyticsEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadPreferences()
        {
            _analyticsEnabled = PlayerPrefs.GetInt(ANALYTICS_ENABLED_KEY, 1) == 1;
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Track Test Event")]
        private void DebugTrackTestEvent()
        {
            TrackEvent(AnalyticsEventType.GameStart, new Dictionary<string, object>
            {
                { "test", true },
                { "era", "Medieval" }
            });
        }

        [ContextMenu("Print Session Info")]
        private void DebugPrintSessionInfo()
        {
            Debug.Log($"Session ID: {_sessionId}");
            Debug.Log($"Session Start: {_sessionStartTime}");
            Debug.Log($"Events in Queue: {_eventQueue.Count}");
            Debug.Log($"Analytics Enabled: {_analyticsEnabled}");
        }
#endif
        #endregion
    }

    /// <summary>
    /// Analytics event veri yapisi
    /// </summary>
    [Serializable]
    public class AnalyticsEvent
    {
        public AnalyticsEventType eventType;
        public DateTime timestamp;
        public string sessionId;
        public Dictionary<string, object> parameters;
    }

    /// <summary>
    /// Analytics event turleri
    /// </summary>
    public enum AnalyticsEventType
    {
        // Session Events
        SessionStart,
        SessionEnd,
        AppBackground,
        AppForeground,

        // Game Events
        GameStart,
        GameEnd,
        CardDecision,
        EraChange,

        // Progression Events
        AchievementUnlock,
        TutorialStep,
        TutorialComplete,
        LevelUp,

        // Monetization Events
        Purchase,
        AdWatched,
        AdClicked,
        StoreOpened,

        // Social Events
        DailyChallenge,
        Share,
        LeaderboardView,
        ProfileView,

        // UI Events
        ButtonClick,
        ScreenView,
        MenuOpen,
        SettingsChange,

        // Error Events
        Error,
        Crash,

        // Custom Events
        Custom
    }
}
