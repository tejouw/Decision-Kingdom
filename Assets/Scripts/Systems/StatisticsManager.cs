using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Istatistik yonetim sistemi
    /// Faz 4: Meta Sistemler
    /// </summary>
    public class StatisticsManager : MonoBehaviour
    {
        public static StatisticsManager Instance { get; private set; }

        [Header("Lifetime Statistics")]
        [SerializeField] private LifetimeStatistics _lifetimeStats;

        [Header("Current Session")]
        [SerializeField] private SessionStatistics _currentSession;

        // Events
        public event Action<string, int> OnStatUpdated;
        public event Action OnStatsReset;

        #region Properties
        public LifetimeStatistics LifetimeStats => _lifetimeStats;
        public SessionStatistics CurrentSession => _currentSession;
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

            Initialize();
        }

        private void Initialize()
        {
            _lifetimeStats = new LifetimeStatistics();
            _currentSession = new SessionStatistics();

            LoadStatistics();
        }
        #endregion

        #region Public Methods - Session Tracking
        /// <summary>
        /// Yeni oturum baslat
        /// </summary>
        public void StartNewSession(Era era, string scenarioId)
        {
            _currentSession = new SessionStatistics
            {
                startTime = DateTime.Now,
                era = era,
                scenarioId = scenarioId
            };

            _lifetimeStats.totalGamesStarted++;
            SaveStatistics();

            Debug.Log($"[StatisticsManager] Yeni oturum: {era}, {scenarioId}");
        }

        /// <summary>
        /// Kart secimi kaydet
        /// </summary>
        public void RecordCardChoice(bool choseRight, GameEvent gameEvent)
        {
            _currentSession.cardsPlayed++;
            _lifetimeStats.totalCardsPlayed++;

            if (choseRight)
            {
                _currentSession.rightSwipes++;
                _lifetimeStats.totalRightSwipes++;
            }
            else
            {
                _currentSession.leftSwipes++;
                _lifetimeStats.totalLeftSwipes++;
            }

            // Karakter etkilesimi
            if (gameEvent != null && !string.IsNullOrEmpty(gameEvent.CharacterId))
            {
                RecordCharacterInteraction(gameEvent.CharacterId);
            }

            OnStatUpdated?.Invoke("cardsPlayed", _lifetimeStats.totalCardsPlayed);
        }

        /// <summary>
        /// Kaynak degisimini kaydet
        /// </summary>
        public void RecordResourceChange(ResourceType type, int oldValue, int newValue)
        {
            int change = newValue - oldValue;

            switch (type)
            {
                case ResourceType.Gold:
                    _currentSession.goldGained += Mathf.Max(0, change);
                    _currentSession.goldLost += Mathf.Max(0, -change);
                    _lifetimeStats.totalGoldGained += Mathf.Max(0, change);
                    _lifetimeStats.totalGoldLost += Mathf.Max(0, -change);
                    break;
                case ResourceType.Happiness:
                    _currentSession.happinessGained += Mathf.Max(0, change);
                    _currentSession.happinessLost += Mathf.Max(0, -change);
                    _lifetimeStats.totalHappinessGained += Mathf.Max(0, change);
                    _lifetimeStats.totalHappinessLost += Mathf.Max(0, -change);
                    break;
                case ResourceType.Military:
                    _currentSession.militaryGained += Mathf.Max(0, change);
                    _currentSession.militaryLost += Mathf.Max(0, -change);
                    _lifetimeStats.totalMilitaryGained += Mathf.Max(0, change);
                    _lifetimeStats.totalMilitaryLost += Mathf.Max(0, -change);
                    break;
                case ResourceType.Faith:
                    _currentSession.faithGained += Mathf.Max(0, change);
                    _currentSession.faithLost += Mathf.Max(0, -change);
                    _lifetimeStats.totalFaithGained += Mathf.Max(0, change);
                    _lifetimeStats.totalFaithLost += Mathf.Max(0, -change);
                    break;
            }
        }

        /// <summary>
        /// Tur kaydet
        /// </summary>
        public void RecordTurn(int turnNumber, Resources resources)
        {
            _currentSession.turnsPlayed = turnNumber;

            // Ortalama kaynak guncellemesi
            _currentSession.avgGold = (_currentSession.avgGold * (turnNumber - 1) + resources.Gold) / turnNumber;
            _currentSession.avgHappiness = (_currentSession.avgHappiness * (turnNumber - 1) + resources.Happiness) / turnNumber;
            _currentSession.avgMilitary = (_currentSession.avgMilitary * (turnNumber - 1) + resources.Military) / turnNumber;
            _currentSession.avgFaith = (_currentSession.avgFaith * (turnNumber - 1) + resources.Faith) / turnNumber;

            // Max/Min takibi
            if (resources.Gold > _currentSession.maxGold) _currentSession.maxGold = resources.Gold;
            if (resources.Gold < _currentSession.minGold) _currentSession.minGold = resources.Gold;
            if (resources.Happiness > _currentSession.maxHappiness) _currentSession.maxHappiness = resources.Happiness;
            if (resources.Happiness < _currentSession.minHappiness) _currentSession.minHappiness = resources.Happiness;
            if (resources.Military > _currentSession.maxMilitary) _currentSession.maxMilitary = resources.Military;
            if (resources.Military < _currentSession.minMilitary) _currentSession.minMilitary = resources.Military;
            if (resources.Faith > _currentSession.maxFaith) _currentSession.maxFaith = resources.Faith;
            if (resources.Faith < _currentSession.minFaith) _currentSession.minFaith = resources.Faith;
        }

        /// <summary>
        /// Karakter etkilesimi kaydet
        /// </summary>
        public void RecordCharacterInteraction(string characterId)
        {
            if (!_currentSession.characterInteractions.ContainsKey(characterId))
                _currentSession.characterInteractions[characterId] = 0;
            _currentSession.characterInteractions[characterId]++;

            if (!_lifetimeStats.characterInteractions.ContainsKey(characterId))
                _lifetimeStats.characterInteractions[characterId] = 0;
            _lifetimeStats.characterInteractions[characterId]++;
        }

        /// <summary>
        /// Oyun sonu kaydet
        /// </summary>
        public void RecordGameEnd(GameStateData gameState, EndingType endingType, int prestigeEarned)
        {
            _currentSession.endTime = DateTime.Now;
            _currentSession.endingType = endingType;
            _currentSession.prestigeEarned = prestigeEarned;
            _currentSession.finalResources = new Resources(gameState.resources);

            // Lifetime guncellemeleri
            _lifetimeStats.totalGamesCompleted++;
            _lifetimeStats.totalTurnsPlayed += gameState.turn;
            _lifetimeStats.totalPrestigeEarned += prestigeEarned;
            _lifetimeStats.totalPlayTime += _currentSession.Duration;

            // En uzun hayatta kalma
            if (gameState.turn > _lifetimeStats.longestSurvival)
            {
                _lifetimeStats.longestSurvival = gameState.turn;
                _lifetimeStats.longestSurvivalDate = DateTime.Now;
            }

            // En kisa hayatta kalma
            if (_lifetimeStats.shortestSurvival == 0 || gameState.turn < _lifetimeStats.shortestSurvival)
            {
                _lifetimeStats.shortestSurvival = gameState.turn;
            }

            // Ending sayaci
            if (!_lifetimeStats.endingCounts.ContainsKey(endingType))
                _lifetimeStats.endingCounts[endingType] = 0;
            _lifetimeStats.endingCounts[endingType]++;

            // Era sayaci
            if (!_lifetimeStats.gamesPerEra.ContainsKey(gameState.era))
                _lifetimeStats.gamesPerEra[gameState.era] = 0;
            _lifetimeStats.gamesPerEra[gameState.era]++;

            // GameOver sebep sayaci
            if (gameState.gameOverReason != GameOverReason.None)
            {
                if (!_lifetimeStats.deathCauses.ContainsKey(gameState.gameOverReason))
                    _lifetimeStats.deathCauses[gameState.gameOverReason] = 0;
                _lifetimeStats.deathCauses[gameState.gameOverReason]++;
            }

            // Zafer/Maglup sayaci
            if (EndingSystem.IsVictory(endingType))
            {
                _lifetimeStats.totalVictories++;
            }
            else
            {
                _lifetimeStats.totalDefeats++;
            }

            // Ortalama kaynak hesapla
            UpdateAverageResources(gameState.resources);

            SaveStatistics();

            Debug.Log($"[StatisticsManager] Oyun kaydedildi: {endingType}, {gameState.turn} tur, +{prestigeEarned} PP");
        }

        /// <summary>
        /// Nadir event gorme kaydet
        /// </summary>
        public void RecordRareEvent(string eventId)
        {
            if (!_lifetimeStats.rareEventsSeen.Contains(eventId))
            {
                _lifetimeStats.rareEventsSeen.Add(eventId);
            }

            _currentSession.rareEventsSeen++;
        }
        #endregion

        #region Public Methods - Statistics Queries
        /// <summary>
        /// Favori donem
        /// </summary>
        public Era GetFavoriteEra()
        {
            if (_lifetimeStats.gamesPerEra.Count == 0)
                return Era.Medieval;

            return _lifetimeStats.gamesPerEra
                .OrderByDescending(kvp => kvp.Value)
                .First().Key;
        }

        /// <summary>
        /// En sik olum sebebi
        /// </summary>
        public GameOverReason GetMostCommonDeath()
        {
            if (_lifetimeStats.deathCauses.Count == 0)
                return GameOverReason.None;

            return _lifetimeStats.deathCauses
                .OrderByDescending(kvp => kvp.Value)
                .First().Key;
        }

        /// <summary>
        /// Oyun stili analizi
        /// </summary>
        public PlayStyleAnalysis GetPlayStyleAnalysis()
        {
            var analysis = new PlayStyleAnalysis();

            // Swipe tercihi
            int totalSwipes = _lifetimeStats.totalLeftSwipes + _lifetimeStats.totalRightSwipes;
            if (totalSwipes > 0)
            {
                analysis.rightSwipeRatio = (float)_lifetimeStats.totalRightSwipes / totalSwipes;
                analysis.leftSwipeRatio = (float)_lifetimeStats.totalLeftSwipes / totalSwipes;
            }

            // Dominant kaynak
            float maxAvg = Mathf.Max(
                _lifetimeStats.avgGold,
                _lifetimeStats.avgHappiness,
                _lifetimeStats.avgMilitary,
                _lifetimeStats.avgFaith
            );

            if (maxAvg == _lifetimeStats.avgGold) analysis.dominantResource = ResourceType.Gold;
            else if (maxAvg == _lifetimeStats.avgHappiness) analysis.dominantResource = ResourceType.Happiness;
            else if (maxAvg == _lifetimeStats.avgMilitary) analysis.dominantResource = ResourceType.Military;
            else analysis.dominantResource = ResourceType.Faith;

            // Zayif kaynak
            float minAvg = Mathf.Min(
                _lifetimeStats.avgGold,
                _lifetimeStats.avgHappiness,
                _lifetimeStats.avgMilitary,
                _lifetimeStats.avgFaith
            );

            if (minAvg == _lifetimeStats.avgGold) analysis.weakestResource = ResourceType.Gold;
            else if (minAvg == _lifetimeStats.avgHappiness) analysis.weakestResource = ResourceType.Happiness;
            else if (minAvg == _lifetimeStats.avgMilitary) analysis.weakestResource = ResourceType.Military;
            else analysis.weakestResource = ResourceType.Faith;

            // Kazanma orani
            int totalGames = _lifetimeStats.totalVictories + _lifetimeStats.totalDefeats;
            if (totalGames > 0)
            {
                analysis.winRate = (float)_lifetimeStats.totalVictories / totalGames;
            }

            // Oyun stili tipi
            analysis.playStyle = DeterminePlayStyle(analysis);

            return analysis;
        }

        /// <summary>
        /// Karakter etkilesim istatistikleri
        /// </summary>
        public Dictionary<string, int> GetCharacterInteractionStats()
        {
            return new Dictionary<string, int>(_lifetimeStats.characterInteractions);
        }

        /// <summary>
        /// En cok etkilesilen karakter
        /// </summary>
        public string GetFavoriteCharacter()
        {
            if (_lifetimeStats.characterInteractions.Count == 0)
                return null;

            return _lifetimeStats.characterInteractions
                .OrderByDescending(kvp => kvp.Value)
                .First().Key;
        }

        /// <summary>
        /// Ortalama oyun suresi
        /// </summary>
        public TimeSpan GetAverageGameDuration()
        {
            if (_lifetimeStats.totalGamesCompleted == 0)
                return TimeSpan.Zero;

            double avgTicks = _lifetimeStats.totalPlayTime.Ticks / _lifetimeStats.totalGamesCompleted;
            return TimeSpan.FromTicks((long)avgTicks);
        }

        /// <summary>
        /// Ortalama tur sayisi
        /// </summary>
        public float GetAverageTurns()
        {
            if (_lifetimeStats.totalGamesCompleted == 0)
                return 0;

            return (float)_lifetimeStats.totalTurnsPlayed / _lifetimeStats.totalGamesCompleted;
        }
        #endregion

        #region Private Methods
        private void UpdateAverageResources(Resources resources)
        {
            int games = _lifetimeStats.totalGamesCompleted;
            if (games == 0) return;

            // Yuvarlanmis ortalama
            _lifetimeStats.avgGold = (_lifetimeStats.avgGold * (games - 1) + resources.Gold) / games;
            _lifetimeStats.avgHappiness = (_lifetimeStats.avgHappiness * (games - 1) + resources.Happiness) / games;
            _lifetimeStats.avgMilitary = (_lifetimeStats.avgMilitary * (games - 1) + resources.Military) / games;
            _lifetimeStats.avgFaith = (_lifetimeStats.avgFaith * (games - 1) + resources.Faith) / games;
        }

        private PlayStyle DeterminePlayStyle(PlayStyleAnalysis analysis)
        {
            // Denge odakli
            float resourceVariance = Mathf.Abs(_lifetimeStats.avgGold - 50) +
                                     Mathf.Abs(_lifetimeStats.avgHappiness - 50) +
                                     Mathf.Abs(_lifetimeStats.avgMilitary - 50) +
                                     Mathf.Abs(_lifetimeStats.avgFaith - 50);

            if (resourceVariance < 40) return PlayStyle.Balanced;

            // Dominant kaynak bazli
            switch (analysis.dominantResource)
            {
                case ResourceType.Gold:
                    return PlayStyle.Merchant;
                case ResourceType.Happiness:
                    return PlayStyle.Populist;
                case ResourceType.Military:
                    return PlayStyle.Warmonger;
                case ResourceType.Faith:
                    return PlayStyle.Pious;
            }

            return PlayStyle.Balanced;
        }

        private void SaveStatistics()
        {
            string json = JsonUtility.ToJson(_lifetimeStats);
            PlayerPrefs.SetString(Constants.SAVE_KEY_STATISTICS, json);
            PlayerPrefs.Save();
        }

        private void LoadStatistics()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_STATISTICS, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    _lifetimeStats = JsonUtility.FromJson<LifetimeStatistics>(json);
                }
                catch
                {
                    _lifetimeStats = new LifetimeStatistics();
                }
            }
            else
            {
                _lifetimeStats = new LifetimeStatistics();
            }

            Debug.Log($"[StatisticsManager] Yuklendi. Toplam oyun: {_lifetimeStats.totalGamesCompleted}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Print Lifetime Stats")]
        private void DebugPrintLifetimeStats()
        {
            Debug.Log($"=== Lifetime Statistics ===");
            Debug.Log($"Games: {_lifetimeStats.totalGamesCompleted}/{_lifetimeStats.totalGamesStarted}");
            Debug.Log($"Cards: {_lifetimeStats.totalCardsPlayed}");
            Debug.Log($"Turns: {_lifetimeStats.totalTurnsPlayed}");
            Debug.Log($"Victories: {_lifetimeStats.totalVictories}");
            Debug.Log($"Defeats: {_lifetimeStats.totalDefeats}");
            Debug.Log($"Longest: {_lifetimeStats.longestSurvival} turns");
            Debug.Log($"Shortest: {_lifetimeStats.shortestSurvival} turns");
            Debug.Log($"Prestige: {_lifetimeStats.totalPrestigeEarned}");
            Debug.Log($"Play Time: {_lifetimeStats.totalPlayTime}");
        }

        [ContextMenu("Print Play Style")]
        private void DebugPrintPlayStyle()
        {
            var analysis = GetPlayStyleAnalysis();
            Debug.Log($"=== Play Style Analysis ===");
            Debug.Log($"Style: {analysis.playStyle}");
            Debug.Log($"Win Rate: {analysis.winRate:P1}");
            Debug.Log($"Dominant: {analysis.dominantResource}");
            Debug.Log($"Weakest: {analysis.weakestResource}");
            Debug.Log($"Right Swipes: {analysis.rightSwipeRatio:P1}");
            Debug.Log($"Left Swipes: {analysis.leftSwipeRatio:P1}");
        }

        [ContextMenu("Reset All Statistics")]
        private void DebugResetStats()
        {
            _lifetimeStats = new LifetimeStatistics();
            SaveStatistics();
            OnStatsReset?.Invoke();
            Debug.Log("[StatisticsManager] Tum istatistikler sifirlandi!");
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Omur boyu istatistikler
    /// </summary>
    [System.Serializable]
    public class LifetimeStatistics
    {
        // Genel
        public int totalGamesStarted;
        public int totalGamesCompleted;
        public int totalCardsPlayed;
        public int totalTurnsPlayed;
        public int totalVictories;
        public int totalDefeats;
        public int totalPrestigeEarned;
        public TimeSpan totalPlayTime;

        // Hayatta kalma
        public int longestSurvival;
        public int shortestSurvival;
        public DateTime longestSurvivalDate;

        // Swipe istatistikleri
        public int totalLeftSwipes;
        public int totalRightSwipes;

        // Kaynak istatistikleri
        public int totalGoldGained;
        public int totalGoldLost;
        public int totalHappinessGained;
        public int totalHappinessLost;
        public int totalMilitaryGained;
        public int totalMilitaryLost;
        public int totalFaithGained;
        public int totalFaithLost;

        // Ortalamalar
        public float avgGold;
        public float avgHappiness;
        public float avgMilitary;
        public float avgFaith;

        // Sayac dictionary'leri
        public Dictionary<EndingType, int> endingCounts;
        public Dictionary<Era, int> gamesPerEra;
        public Dictionary<GameOverReason, int> deathCauses;
        public Dictionary<string, int> characterInteractions;
        public List<string> rareEventsSeen;

        public LifetimeStatistics()
        {
            endingCounts = new Dictionary<EndingType, int>();
            gamesPerEra = new Dictionary<Era, int>();
            deathCauses = new Dictionary<GameOverReason, int>();
            characterInteractions = new Dictionary<string, int>();
            rareEventsSeen = new List<string>();
            avgGold = 50f;
            avgHappiness = 50f;
            avgMilitary = 50f;
            avgFaith = 50f;
        }
    }

    /// <summary>
    /// Oturum istatistikleri
    /// </summary>
    [System.Serializable]
    public class SessionStatistics
    {
        // Oturum bilgisi
        public DateTime startTime;
        public DateTime endTime;
        public Era era;
        public string scenarioId;
        public EndingType endingType;

        // Oyun istatistikleri
        public int cardsPlayed;
        public int turnsPlayed;
        public int leftSwipes;
        public int rightSwipes;
        public int prestigeEarned;
        public int rareEventsSeen;

        // Kaynak kazanc/kayip
        public int goldGained;
        public int goldLost;
        public int happinessGained;
        public int happinessLost;
        public int militaryGained;
        public int militaryLost;
        public int faithGained;
        public int faithLost;

        // Ortalamalar
        public float avgGold;
        public float avgHappiness;
        public float avgMilitary;
        public float avgFaith;

        // Max/Min
        public int maxGold;
        public int minGold;
        public int maxHappiness;
        public int minHappiness;
        public int maxMilitary;
        public int minMilitary;
        public int maxFaith;
        public int minFaith;

        // Karakter etkilesimleri
        public Dictionary<string, int> characterInteractions;
        public Resources finalResources;

        public TimeSpan Duration => endTime - startTime;

        public SessionStatistics()
        {
            startTime = DateTime.Now;
            characterInteractions = new Dictionary<string, int>();
            minGold = 100;
            minHappiness = 100;
            minMilitary = 100;
            minFaith = 100;
            avgGold = 50f;
            avgHappiness = 50f;
            avgMilitary = 50f;
            avgFaith = 50f;
        }
    }

    /// <summary>
    /// Oyun stili analizi
    /// </summary>
    public class PlayStyleAnalysis
    {
        public PlayStyle playStyle;
        public float winRate;
        public float rightSwipeRatio;
        public float leftSwipeRatio;
        public ResourceType dominantResource;
        public ResourceType weakestResource;
    }

    /// <summary>
    /// Oyun stilleri
    /// </summary>
    public enum PlayStyle
    {
        Balanced,   // Dengeli
        Merchant,   // Altin odakli
        Populist,   // Mutluluk odakli
        Warmonger,  // Askeri odakli
        Pious       // Inanc odakli
    }
    #endregion
}
