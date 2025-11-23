using System;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Managers
{
    /// <summary>
    /// Save/Load yöneticisi
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        [Header("Otomatik Kayıt")]
        [SerializeField] private bool _autoSaveEnabled = true;
        [SerializeField] private float _autoSaveInterval = 60f;

        private float _lastAutoSaveTime;

        // Save versioning for forward compatibility
        private const int SAVE_VERSION = 1;

        // Events
        public event Action OnSaveCompleted;
        public event Action OnLoadCompleted;
        public event Action<string> OnSaveError;
        public event Action<string> OnLoadError;

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

            // Initialize auto-save timer
            _lastAutoSaveTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            if (_autoSaveEnabled && GameManager.Instance != null && GameManager.Instance.IsPlaying)
            {
                // Use realtimeSinceStartup instead of Time.time to avoid issues with timeScale
                if (Time.realtimeSinceStartup - _lastAutoSaveTime >= _autoSaveInterval)
                {
                    AutoSave();
                    _lastAutoSaveTime = Time.realtimeSinceStartup;
                }
            }
        }
        #endregion

        #region Public Methods - Save
        /// <summary>
        /// Oyun durumunu kaydet
        /// </summary>
        public bool SaveGame()
        {
            try
            {
                if (GameManager.Instance == null)
                {
                    OnSaveError?.Invoke("GameManager bulunamadı");
                    return false;
                }

                GameStateData gameState = GameManager.Instance.GetGameStateForSave();

                // Create versioned save wrapper
                var saveWrapper = new SaveDataWrapper
                {
                    version = SAVE_VERSION,
                    timestamp = DateTime.Now.ToBinary(),
                    gameState = gameState
                };

                string json = JsonUtility.ToJson(saveWrapper, true);
                PlayerPrefs.SetString(Constants.SAVE_KEY_GAME_STATE, json);
                PlayerPrefs.Save();

                OnSaveCompleted?.Invoke();
                Debug.Log("[SaveManager] Oyun kaydedildi");
                return true;
            }
            catch (ArgumentException ex)
            {
                OnSaveError?.Invoke($"Serileştirme hatası: {ex.Message}");
                Debug.LogError($"[SaveManager] Kayıt serileştirme hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                OnSaveError?.Invoke(ex.Message);
                Debug.LogError($"[SaveManager] Kayıt hatası: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// İstatistikleri kaydet
        /// </summary>
        public bool SaveStatistics(PlayerStatistics statistics)
        {
            if (statistics == null)
            {
                Debug.LogWarning("[SaveManager] SaveStatistics: statistics is null");
                return false;
            }

            try
            {
                string json = JsonUtility.ToJson(statistics, true);
                PlayerPrefs.SetString(Constants.SAVE_KEY_STATISTICS, json);
                PlayerPrefs.Save();

                Debug.Log("[SaveManager] İstatistikler kaydedildi");
                return true;
            }
            catch (ArgumentException ex)
            {
                Debug.LogError($"[SaveManager] İstatistik serileştirme hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] İstatistik kayıt hatası: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Prestige puanını kaydet
        /// </summary>
        public bool SavePrestigePoints(int points)
        {
            try
            {
                PlayerPrefs.SetInt(Constants.SAVE_KEY_PRESTIGE, points);
                PlayerPrefs.Save();

                Debug.Log($"[SaveManager] Prestige puanı kaydedildi: {points}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Prestige kayıt hatası: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unlock verilerini kaydet
        /// </summary>
        public bool SaveUnlocks(UnlockData unlocks)
        {
            if (unlocks == null)
            {
                Debug.LogWarning("[SaveManager] SaveUnlocks: unlocks is null");
                return false;
            }

            try
            {
                string json = JsonUtility.ToJson(unlocks, true);
                PlayerPrefs.SetString(Constants.SAVE_KEY_UNLOCKS, json);
                PlayerPrefs.Save();

                Debug.Log("[SaveManager] Unlock verileri kaydedildi");
                return true;
            }
            catch (ArgumentException ex)
            {
                Debug.LogError($"[SaveManager] Unlock serileştirme hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Unlock kayıt hatası: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }
        #endregion

        #region Public Methods - Load
        /// <summary>
        /// Oyun durumunu yükle
        /// </summary>
        public bool LoadGame()
        {
            try
            {
                if (!HasSavedGame())
                {
                    OnLoadError?.Invoke("Kayıtlı oyun bulunamadı");
                    return false;
                }

                string json = PlayerPrefs.GetString(Constants.SAVE_KEY_GAME_STATE);

                // Validate JSON before parsing
                if (string.IsNullOrEmpty(json))
                {
                    OnLoadError?.Invoke("Kayıt verisi boş");
                    return false;
                }

                GameStateData gameState;

                // Try to load with version wrapper first (new format)
                try
                {
                    var saveWrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
                    if (saveWrapper != null && saveWrapper.gameState != null)
                    {
                        // Handle version migration if needed
                        if (saveWrapper.version < SAVE_VERSION)
                        {
                            Debug.Log($"[SaveManager] Migrating save from version {saveWrapper.version} to {SAVE_VERSION}");
                            // Add migration logic here for future versions
                        }
                        gameState = saveWrapper.gameState;
                    }
                    else
                    {
                        // Fallback to old format (direct GameStateData)
                        gameState = JsonUtility.FromJson<GameStateData>(json);
                    }
                }
                catch
                {
                    // Fallback to old format for backwards compatibility
                    gameState = JsonUtility.FromJson<GameStateData>(json);
                }

                if (gameState == null)
                {
                    OnLoadError?.Invoke("Kayıt verisi okunamadı");
                    Debug.LogError("[SaveManager] GameState is null after deserialization");
                    return false;
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LoadGameState(gameState);
                }

                OnLoadCompleted?.Invoke();
                Debug.Log("[SaveManager] Oyun yüklendi");
                return true;
            }
            catch (ArgumentException ex)
            {
                OnLoadError?.Invoke($"JSON format hatası: {ex.Message}");
                Debug.LogError($"[SaveManager] JSON parse hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                OnLoadError?.Invoke(ex.Message);
                Debug.LogError($"[SaveManager] Yükleme hatası: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// İstatistikleri yükle
        /// </summary>
        public PlayerStatistics LoadStatistics()
        {
            try
            {
                if (!PlayerPrefs.HasKey(Constants.SAVE_KEY_STATISTICS))
                {
                    return new PlayerStatistics();
                }

                string json = PlayerPrefs.GetString(Constants.SAVE_KEY_STATISTICS);
                if (string.IsNullOrEmpty(json))
                {
                    return new PlayerStatistics();
                }

                var result = JsonUtility.FromJson<PlayerStatistics>(json);
                return result ?? new PlayerStatistics();
            }
            catch (ArgumentException ex)
            {
                Debug.LogError($"[SaveManager] İstatistik JSON parse hatası: {ex.Message}");
                return new PlayerStatistics();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] İstatistik yükleme hatası: {ex.Message}\n{ex.StackTrace}");
                return new PlayerStatistics();
            }
        }

        /// <summary>
        /// Prestige puanını yükle
        /// </summary>
        public int LoadPrestigePoints()
        {
            return PlayerPrefs.GetInt(Constants.SAVE_KEY_PRESTIGE, 0);
        }

        /// <summary>
        /// Unlock verilerini yükle
        /// </summary>
        public UnlockData LoadUnlocks()
        {
            try
            {
                if (!PlayerPrefs.HasKey(Constants.SAVE_KEY_UNLOCKS))
                {
                    return new UnlockData();
                }

                string json = PlayerPrefs.GetString(Constants.SAVE_KEY_UNLOCKS);
                if (string.IsNullOrEmpty(json))
                {
                    return new UnlockData();
                }

                var result = JsonUtility.FromJson<UnlockData>(json);
                return result ?? new UnlockData();
            }
            catch (ArgumentException ex)
            {
                Debug.LogError($"[SaveManager] Unlock JSON parse hatası: {ex.Message}");
                return new UnlockData();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Unlock yükleme hatası: {ex.Message}\n{ex.StackTrace}");
                return new UnlockData();
            }
        }
        #endregion

        #region Public Methods - Utility
        /// <summary>
        /// Kayıtlı oyun var mı?
        /// </summary>
        public bool HasSavedGame()
        {
            return PlayerPrefs.HasKey(Constants.SAVE_KEY_GAME_STATE);
        }

        /// <summary>
        /// Oyun kaydını sil
        /// </summary>
        public void DeleteSavedGame()
        {
            PlayerPrefs.DeleteKey(Constants.SAVE_KEY_GAME_STATE);
            PlayerPrefs.Save();
            Debug.Log("[SaveManager] Oyun kaydı silindi");
        }

        /// <summary>
        /// Tüm verileri sil
        /// </summary>
        public void DeleteAllData()
        {
            PlayerPrefs.DeleteKey(Constants.SAVE_KEY_GAME_STATE);
            PlayerPrefs.DeleteKey(Constants.SAVE_KEY_STATISTICS);
            PlayerPrefs.DeleteKey(Constants.SAVE_KEY_PRESTIGE);
            PlayerPrefs.DeleteKey(Constants.SAVE_KEY_UNLOCKS);
            PlayerPrefs.DeleteKey(Constants.SAVE_KEY_ACHIEVEMENTS);
            PlayerPrefs.Save();
            Debug.Log("[SaveManager] Tüm veriler silindi");
        }

        /// <summary>
        /// Otomatik kayıt aralığını ayarla
        /// </summary>
        public void SetAutoSaveInterval(float seconds)
        {
            _autoSaveInterval = Mathf.Max(10f, seconds);
        }

        /// <summary>
        /// Otomatik kaydı aç/kapat
        /// </summary>
        public void SetAutoSaveEnabled(bool enabled)
        {
            _autoSaveEnabled = enabled;
        }
        #endregion

        #region Private Methods
        private void AutoSave()
        {
            if (SaveGame())
            {
                Debug.Log("[SaveManager] Otomatik kayıt tamamlandı");
            }
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Save Game")]
        private void DebugSaveGame()
        {
            SaveGame();
        }

        [ContextMenu("Load Game")]
        private void DebugLoadGame()
        {
            LoadGame();
        }

        [ContextMenu("Delete All Data")]
        private void DebugDeleteAllData()
        {
            DeleteAllData();
        }

        [ContextMenu("Print Save Info")]
        private void DebugPrintSaveInfo()
        {
            Debug.Log($"Has Saved Game: {HasSavedGame()}");
            Debug.Log($"Prestige Points: {LoadPrestigePoints()}");

            if (HasSavedGame())
            {
                string json = PlayerPrefs.GetString(Constants.SAVE_KEY_GAME_STATE);
                Debug.Log($"Save Data Length: {json.Length} characters");
            }
        }
#endif
        #endregion
    }

    /// <summary>
    /// Save data wrapper with versioning support
    /// </summary>
    [System.Serializable]
    public class SaveDataWrapper
    {
        public int version;
        public long timestamp;
        public GameStateData gameState;
    }
}
