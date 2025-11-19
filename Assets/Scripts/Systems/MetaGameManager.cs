using System;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;
using DecisionKingdom.Managers;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Meta oyun sistemlerini koordine eden yonetici
    /// Prestige, Achievement ve Statistics sistemlerini baglar
    /// Faz 4: Meta Sistemler
    /// </summary>
    public class MetaGameManager : MonoBehaviour
    {
        public static MetaGameManager Instance { get; private set; }

        [Header("System References")]
        [SerializeField] private PrestigeManager _prestigeManager;
        [SerializeField] private AchievementSystem _achievementSystem;
        [SerializeField] private StatisticsManager _statisticsManager;

        // Events
        public event Action<int> OnPrestigeEarned;
        public event Action<Achievement> OnAchievementUnlocked;
        public event Action<EndingType, int> OnGameEnded;

        #region Properties
        public PrestigeManager Prestige => _prestigeManager;
        public AchievementSystem Achievements => _achievementSystem;
        public StatisticsManager Statistics => _statisticsManager;
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

            InitializeSystems();
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region Initialization
        private void InitializeSystems()
        {
            // Sistemleri bul veya olustur
            if (_prestigeManager == null)
                _prestigeManager = GetComponentInChildren<PrestigeManager>() ?? gameObject.AddComponent<PrestigeManager>();

            if (_achievementSystem == null)
                _achievementSystem = GetComponentInChildren<AchievementSystem>() ?? gameObject.AddComponent<AchievementSystem>();

            if (_statisticsManager == null)
                _statisticsManager = GetComponentInChildren<StatisticsManager>() ?? gameObject.AddComponent<StatisticsManager>();

            Debug.Log("[MetaGameManager] Tum sistemler baslatildi.");
        }

        private void SubscribeToEvents()
        {
            // GameManager eventleri
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnNewGameStarted += HandleNewGameStarted;
                GameManager.Instance.OnTurnAdvanced += HandleTurnAdvanced;
                GameManager.Instance.OnGameOver += HandleGameOver;
                GameManager.Instance.OnEventProcessed += HandleEventProcessed;
            }

            // Achievement eventi
            if (_achievementSystem != null)
            {
                _achievementSystem.OnAchievementUnlocked += HandleAchievementUnlocked;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnNewGameStarted -= HandleNewGameStarted;
                GameManager.Instance.OnTurnAdvanced -= HandleTurnAdvanced;
                GameManager.Instance.OnGameOver -= HandleGameOver;
                GameManager.Instance.OnEventProcessed -= HandleEventProcessed;
            }

            if (_achievementSystem != null)
            {
                _achievementSystem.OnAchievementUnlocked -= HandleAchievementUnlocked;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleNewGameStarted()
        {
            if (GameManager.Instance == null) return;

            var gameState = GameManager.Instance.CurrentGameState;

            // Oturum istatistiklerini baslat
            _statisticsManager?.StartNewSession(gameState.era, "default");

            // Ilk oyun basarisi
            _achievementSystem?.UnlockAchievement("first_game");

            Debug.Log("[MetaGameManager] Yeni oyun baslatildi - Meta sistemler hazir.");
        }

        private void HandleTurnAdvanced(int turn)
        {
            if (GameManager.Instance == null) return;

            var gameState = GameManager.Instance.CurrentGameState;

            // Tur istatistiklerini kaydet
            _statisticsManager?.RecordTurn(turn, gameState.resources);

            // Her turda basarilari kontrol et
            _achievementSystem?.CheckAchievements(gameState);

            // Her 10 turda kaydet
            if (turn % 10 == 0)
            {
                Debug.Log($"[MetaGameManager] Tur {turn} - Kontrol noktasi");
            }
        }

        private void HandleGameOver(GameOverReason reason)
        {
            if (GameManager.Instance == null) return;

            var gameState = GameManager.Instance.CurrentGameState;

            // Ending tipini belirle
            EndingType endingType;
            if (reason != GameOverReason.None)
            {
                endingType = EndingSystem.GetEndingFromGameOver(reason);
            }
            else
            {
                endingType = EndingSystem.DetermineEnding(gameState);
            }

            // Miras tipini belirle
            LegacyType legacyType = LegacySystem.DetermineLegacy(gameState);

            // Prestige puani hesapla
            int prestigeEarned = _prestigeManager?.CalculateGameEndPrestige(gameState, endingType) ?? 0;

            // Prestige ekle
            _prestigeManager?.AddPrestigePoints(prestigeEarned);

            // Oyun sonu istatistiklerini kaydet
            _statisticsManager?.RecordGameEnd(gameState, endingType, prestigeEarned);

            // Oyun sonu basarilarini kontrol et
            _achievementSystem?.CheckEndGameAchievements(gameState, endingType);

            // Son basarilari kontrol et
            _achievementSystem?.CheckAchievements(gameState);

            // Eventleri tetikle
            OnPrestigeEarned?.Invoke(prestigeEarned);
            OnGameEnded?.Invoke(endingType, prestigeEarned);

            Debug.Log($"[MetaGameManager] Oyun bitti: {endingType}, +{prestigeEarned} PP, Miras: {legacyType}");
        }

        private void HandleEventProcessed(GameEvent gameEvent)
        {
            if (gameEvent == null) return;

            // Nadir event kontrolu
            if (gameEvent.isRare)
            {
                _statisticsManager?.RecordRareEvent(gameEvent.id);
            }
        }

        private void HandleAchievementUnlocked(Achievement achievement)
        {
            OnAchievementUnlocked?.Invoke(achievement);
            Debug.Log($"[MetaGameManager] Basari acildi: {achievement.name}");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Kart secimi kaydet
        /// </summary>
        public void RecordCardChoice(bool choseRight, GameEvent gameEvent)
        {
            _statisticsManager?.RecordCardChoice(choseRight, gameEvent);

            // Swipe bazli gizli basarilar icin (ilerde implement edilebilir)
        }

        /// <summary>
        /// Kaynak degisimi kaydet
        /// </summary>
        public void RecordResourceChange(ResourceType type, int oldValue, int newValue)
        {
            _statisticsManager?.RecordResourceChange(type, oldValue, newValue);
        }

        /// <summary>
        /// Era unlock kontrolu
        /// </summary>
        public bool CanPlayEra(Era era)
        {
            return _prestigeManager?.IsEraUnlocked(era) ?? false;
        }

        /// <summary>
        /// Senaryo unlock kontrolu
        /// </summary>
        public bool CanPlayScenario(string scenarioId)
        {
            return _prestigeManager?.IsScenarioUnlocked(scenarioId) ?? false;
        }

        /// <summary>
        /// PP ile era ac
        /// </summary>
        public bool TryUnlockEra(Era era)
        {
            if (_prestigeManager == null) return false;

            bool success = _prestigeManager.UnlockEra(era);
            if (success)
            {
                _achievementSystem?.UnlockAchievement("unlock_era");

                // Tum eralar acildi mi?
                bool allUnlocked = true;
                foreach (Era e in Enum.GetValues(typeof(Era)))
                {
                    if (!_prestigeManager.IsEraUnlocked(e))
                    {
                        allUnlocked = false;
                        break;
                    }
                }

                if (allUnlocked)
                {
                    _achievementSystem?.UnlockAchievement("unlock_all_eras");
                }
            }

            return success;
        }

        /// <summary>
        /// PP ile senaryo ac
        /// </summary>
        public bool TryUnlockScenario(string scenarioId)
        {
            if (_prestigeManager == null) return false;

            bool success = _prestigeManager.UnlockScenario(scenarioId);
            if (success)
            {
                _achievementSystem?.UnlockAchievement("unlock_scenario");
            }

            return success;
        }

        /// <summary>
        /// Oyun ozeti olustur
        /// </summary>
        public GameSummary CreateGameSummary(GameStateData gameState, EndingType endingType)
        {
            var summary = new GameSummary
            {
                endingType = endingType,
                endingData = EndingSystem.GetEnding(endingType),
                legacyType = LegacySystem.DetermineLegacy(gameState),
                legacyData = LegacySystem.GetLegacy(LegacySystem.DetermineLegacy(gameState)),
                finalTurn = gameState.turn,
                finalResources = new Resources(gameState.resources),
                prestigeEarned = _prestigeManager?.CalculateGameEndPrestige(gameState, endingType) ?? 0,
                totalPrestige = _prestigeManager?.TotalPrestigePoints ?? 0,
                achievementsUnlocked = _achievementSystem?.UnlockedCount ?? 0,
                totalAchievements = _achievementSystem?.TotalCount ?? 0
            };

            return summary;
        }

        /// <summary>
        /// PP bazli basarilari kontrol et
        /// </summary>
        public void CheckPrestigeAchievements(int totalPP)
        {
            if (totalPP >= 100) _achievementSystem?.UnlockAchievement("pp_100");
            if (totalPP >= 500) _achievementSystem?.UnlockAchievement("pp_500");
            if (totalPP >= 1000) _achievementSystem?.UnlockAchievement("pp_1000");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Simulate Game End")]
        private void DebugSimulateGameEnd()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogWarning("GameManager not found");
                return;
            }

            HandleGameOver(GameOverReason.None);
        }

        [ContextMenu("Print Meta Status")]
        private void DebugPrintMetaStatus()
        {
            Debug.Log($"=== Meta Game Status ===");
            Debug.Log($"Total PP: {_prestigeManager?.TotalPrestigePoints ?? 0}");
            Debug.Log($"Achievements: {_achievementSystem?.UnlockedCount ?? 0}/{_achievementSystem?.TotalCount ?? 0}");
            Debug.Log($"Games Played: {_statisticsManager?.LifetimeStats?.totalGamesCompleted ?? 0}");
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Oyun sonu ozeti (UI icin)
    /// </summary>
    [System.Serializable]
    public class GameSummary
    {
        public EndingType endingType;
        public EndingSystem.EndingData endingData;
        public LegacyType legacyType;
        public LegacySystem.LegacyData legacyData;
        public int finalTurn;
        public Resources finalResources;
        public int prestigeEarned;
        public int totalPrestige;
        public int achievementsUnlocked;
        public int totalAchievements;
    }
    #endregion
}
