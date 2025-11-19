using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Hata yonetim ve kullanici bildirimi sistemi - Faz 8
    /// </summary>
    public class ErrorHandler : MonoBehaviour
    {
        public static ErrorHandler Instance { get; private set; }

        [Header("Ayarlar")]
        [SerializeField] private bool _logErrors = true;
        [SerializeField] private bool _showPopups = true;
        [SerializeField] private int _maxStoredErrors = 50;

        // Events
        public event Action<GameError> OnErrorOccurred;
        public event Action<string, string> OnShowErrorPopup;
        public event Action<string> OnShowToast;

        private List<GameError> _errorHistory;

        #region Properties
        public List<GameError> ErrorHistory => _errorHistory;
        public int ErrorCount => _errorHistory.Count;
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

            _errorHistory = new List<GameError>();

            // Global exception handler
            Application.logMessageReceived += HandleUnityLog;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleUnityLog;
        }
        #endregion

        #region Public Methods - Error Handling
        /// <summary>
        /// Hata kaydet ve isle
        /// </summary>
        public void HandleError(ErrorType type, string message, string details = null, bool showPopup = false)
        {
            var error = new GameError
            {
                type = type,
                message = message,
                details = details,
                timestamp = DateTime.UtcNow,
                handled = false
            };

            StoreError(error);

            if (_logErrors)
            {
                LogError(error);
            }

            // Analytics'e gonder
            if (AnalyticsSystem.Instance != null)
            {
                AnalyticsSystem.Instance.TrackError(type.ToString(), message, details);
            }

            OnErrorOccurred?.Invoke(error);

            if (showPopup && _showPopups)
            {
                ShowErrorPopup(type, message);
            }
        }

        /// <summary>
        /// Exception isle
        /// </summary>
        public void HandleException(Exception ex, bool showPopup = false)
        {
            HandleError(
                ErrorType.Exception,
                ex.Message,
                ex.StackTrace,
                showPopup
            );
        }

        /// <summary>
        /// Network hatasi
        /// </summary>
        public void HandleNetworkError(string message, bool showPopup = true)
        {
            HandleError(ErrorType.Network, message, null, showPopup);
        }

        /// <summary>
        /// Save/Load hatasi
        /// </summary>
        public void HandleSaveError(string message, bool showPopup = true)
        {
            HandleError(ErrorType.SaveLoad, message, null, showPopup);
        }

        /// <summary>
        /// IAP hatasi
        /// </summary>
        public void HandlePurchaseError(string message, bool showPopup = true)
        {
            HandleError(ErrorType.Purchase, message, null, showPopup);
        }

        /// <summary>
        /// Reklam hatasi
        /// </summary>
        public void HandleAdError(string message, bool showPopup = false)
        {
            HandleError(ErrorType.Ad, message, null, showPopup);
        }
        #endregion

        #region Public Methods - User Feedback
        /// <summary>
        /// Hata popup'i goster
        /// </summary>
        public void ShowErrorPopup(ErrorType type, string message)
        {
            string title = GetErrorTitle(type);
            string localizedMessage = GetLocalizedErrorMessage(type, message);

            OnShowErrorPopup?.Invoke(title, localizedMessage);
        }

        /// <summary>
        /// Toast mesaji goster
        /// </summary>
        public void ShowToast(string message)
        {
            OnShowToast?.Invoke(message);
        }

        /// <summary>
        /// Basari mesaji goster
        /// </summary>
        public void ShowSuccess(string message)
        {
            ShowToast(message);
        }

        /// <summary>
        /// Uyari mesaji goster
        /// </summary>
        public void ShowWarning(string message)
        {
            ShowToast(message);
        }
        #endregion

        #region Public Methods - Error History
        /// <summary>
        /// Hata gecmisini temizle
        /// </summary>
        public void ClearErrorHistory()
        {
            _errorHistory.Clear();
        }

        /// <summary>
        /// Son hatayi al
        /// </summary>
        public GameError GetLastError()
        {
            return _errorHistory.Count > 0 ? _errorHistory[_errorHistory.Count - 1] : null;
        }

        /// <summary>
        /// Belirli tipteki hatalari al
        /// </summary>
        public List<GameError> GetErrorsByType(ErrorType type)
        {
            return _errorHistory.FindAll(e => e.type == type);
        }
        #endregion

        #region Private Methods
        private void StoreError(GameError error)
        {
            if (_errorHistory.Count >= _maxStoredErrors)
            {
                _errorHistory.RemoveAt(0);
            }
            _errorHistory.Add(error);
        }

        private void LogError(GameError error)
        {
            string logMessage = $"[ErrorHandler] {error.type}: {error.message}";

            switch (error.type)
            {
                case ErrorType.Critical:
                    Debug.LogError(logMessage);
                    break;
                case ErrorType.Exception:
                    Debug.LogError(logMessage);
                    if (!string.IsNullOrEmpty(error.details))
                    {
                        Debug.LogError(error.details);
                    }
                    break;
                default:
                    Debug.LogWarning(logMessage);
                    break;
            }
        }

        private void HandleUnityLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                HandleError(
                    ErrorType.Exception,
                    logString,
                    stackTrace,
                    false
                );
            }
        }

        private string GetErrorTitle(ErrorType type)
        {
            string key = type switch
            {
                ErrorType.Network => "ERROR_NETWORK_TITLE",
                ErrorType.SaveLoad => "ERROR_SAVE_TITLE",
                ErrorType.Purchase => "ERROR_PURCHASE_TITLE",
                ErrorType.Ad => "ERROR_AD_TITLE",
                ErrorType.Critical => "ERROR_CRITICAL_TITLE",
                _ => "ERROR"
            };

            if (LocalizationSystem.Instance != null)
            {
                return LocalizationSystem.Instance.GetLocalizedString(key);
            }

            return type switch
            {
                ErrorType.Network => "Baglanti Hatasi",
                ErrorType.SaveLoad => "Kayit Hatasi",
                ErrorType.Purchase => "Satin Alma Hatasi",
                ErrorType.Ad => "Reklam Hatasi",
                ErrorType.Critical => "Kritik Hata",
                _ => "Hata"
            };
        }

        private string GetLocalizedErrorMessage(ErrorType type, string message)
        {
            // Eger spesifik bir lokalizasyon anahtari varsa onu kullan
            if (LocalizationSystem.Instance != null)
            {
                string key = type switch
                {
                    ErrorType.Network => "NETWORK_ERROR",
                    _ => null
                };

                if (!string.IsNullOrEmpty(key))
                {
                    return LocalizationSystem.Instance.GetLocalizedString(key);
                }
            }

            return message;
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Trigger Test Error")]
        private void DebugTriggerTestError()
        {
            HandleError(ErrorType.Network, "Test error message", null, true);
        }

        [ContextMenu("Print Error History")]
        private void DebugPrintErrorHistory()
        {
            Debug.Log($"Error count: {_errorHistory.Count}");
            foreach (var error in _errorHistory)
            {
                Debug.Log($"[{error.timestamp}] {error.type}: {error.message}");
            }
        }
#endif
        #endregion
    }

    /// <summary>
    /// Oyun hatasi veri yapisi
    /// </summary>
    [Serializable]
    public class GameError
    {
        public ErrorType type;
        public string message;
        public string details;
        public DateTime timestamp;
        public bool handled;
    }

    /// <summary>
    /// Hata turleri
    /// </summary>
    public enum ErrorType
    {
        General,
        Network,
        SaveLoad,
        Purchase,
        Ad,
        Exception,
        Critical,
        Validation,
        Timeout
    }
}
