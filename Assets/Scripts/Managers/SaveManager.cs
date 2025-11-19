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
        }

        private void Update()
        {
            if (_autoSaveEnabled && GameManager.Instance != null && GameManager.Instance.IsPlaying)
            {
                if (Time.time - _lastAutoSaveTime >= _autoSaveInterval)
                {
                    AutoSave();
                    _lastAutoSaveTime = Time.time;
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
                string json = JsonUtility.ToJson(gameState, true);
                PlayerPrefs.SetString(Constants.SAVE_KEY_GAME_STATE, json);
                PlayerPrefs.Save();

                OnSaveCompleted?.Invoke();
                Debug.Log("[SaveManager] Oyun kaydedildi");
                return true;
            }
            catch (Exception ex)
            {
                OnSaveError?.Invoke(ex.Message);
                Debug.LogError($"[SaveManager] Kayıt hatası: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// İstatistikleri kaydet
        /// </summary>
        public bool SaveStatistics(PlayerStatistics statistics)
        {
            try
            {
                string json = JsonUtility.ToJson(statistics, true);
                PlayerPrefs.SetString(Constants.SAVE_KEY_STATISTICS, json);
                PlayerPrefs.Save();

                Debug.Log("[SaveManager] İstatistikler kaydedildi");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] İstatistik kayıt hatası: {ex.Message}");
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
            try
            {
                string json = JsonUtility.ToJson(unlocks, true);
                PlayerPrefs.SetString(Constants.SAVE_KEY_UNLOCKS, json);
                PlayerPrefs.Save();

                Debug.Log("[SaveManager] Unlock verileri kaydedildi");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Unlock kayıt hatası: {ex.Message}");
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
                GameStateData gameState = JsonUtility.FromJson<GameStateData>(json);

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LoadGameState(gameState);
                }

                OnLoadCompleted?.Invoke();
                Debug.Log("[SaveManager] Oyun yüklendi");
                return true;
            }
            catch (Exception ex)
            {
                OnLoadError?.Invoke(ex.Message);
                Debug.LogError($"[SaveManager] Yükleme hatası: {ex.Message}");
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
                return JsonUtility.FromJson<PlayerStatistics>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] İstatistik yükleme hatası: {ex.Message}");
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
                return JsonUtility.FromJson<UnlockData>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Unlock yükleme hatası: {ex.Message}");
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
}
