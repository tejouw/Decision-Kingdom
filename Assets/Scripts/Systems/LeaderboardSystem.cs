using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Skor siralama sistemi
    /// Faz 6: Sosyal Ozellikler
    /// </summary>
    public class LeaderboardSystem : MonoBehaviour
    {
        public static LeaderboardSystem Instance { get; private set; }

        [Header("Leaderboard Data")]
        [SerializeField] private LeaderboardData _data;

        // Events
        public event Action<LeaderboardEntry> OnNewHighScore;
        public event Action<int> OnRankChanged;
        public event Action OnLeaderboardUpdated;

        #region Properties
        public int PersonalBestScore => _data.personalBest;
        public int DailyBestScore => GetDailyBest(GetTodaySeed());
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
            _data = new LeaderboardData();
            LoadLeaderboardData();
        }
        #endregion

        #region Public Methods - Score Submission
        /// <summary>
        /// Gunluk skor gonder
        /// </summary>
        public void SubmitDailyScore(DailyChallengeResult result)
        {
            var entry = new LeaderboardEntry
            {
                playerName = GetPlayerName(),
                score = result.score,
                turns = result.turnsPlayed,
                endingType = result.endingType,
                timestamp = DateTime.Now,
                seed = result.seed
            };

            // Local leaderboard'a ekle
            AddToLocalLeaderboard(entry);

            // Kisisel en iyi kontrol
            if (result.score > _data.personalBest)
            {
                _data.personalBest = result.score;
                _data.personalBestDate = DateTime.Now;
                OnNewHighScore?.Invoke(entry);
            }

            SaveLeaderboardData();
            OnLeaderboardUpdated?.Invoke();

            Debug.Log($"[LeaderboardSystem] Skor gonderildi: {result.score}");

            // Online leaderboard'a gonder (async)
            SubmitToOnlineLeaderboard(entry);
        }

        /// <summary>
        /// Normal oyun skoru gonder
        /// </summary>
        public void SubmitScore(GameStateData gameState, EndingType endingType, int score)
        {
            var entry = new LeaderboardEntry
            {
                playerName = GetPlayerName(),
                score = score,
                turns = gameState.turn,
                endingType = endingType,
                timestamp = DateTime.Now,
                seed = 0 // Normal oyun
            };

            // All-time leaderboard
            AddToAllTimeLeaderboard(entry);

            // Kisisel en iyi kontrol
            if (score > _data.personalBest)
            {
                _data.personalBest = score;
                _data.personalBestDate = DateTime.Now;
                OnNewHighScore?.Invoke(entry);
            }

            SaveLeaderboardData();
            OnLeaderboardUpdated?.Invoke();
        }
        #endregion

        #region Public Methods - Leaderboard Queries
        /// <summary>
        /// Gunluk leaderboard'u al
        /// </summary>
        public List<LeaderboardEntry> GetDailyLeaderboard(int count = 100)
        {
            int todaySeed = GetTodaySeed();

            return _data.dailyEntries
                .Where(e => e.seed == todaySeed)
                .OrderByDescending(e => e.score)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Haftalik leaderboard'u al
        /// </summary>
        public List<LeaderboardEntry> GetWeeklyLeaderboard(int count = 100)
        {
            DateTime weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

            // Optimized: Use MaxBy pattern instead of OrderByDescending().First()
            return _data.dailyEntries
                .Where(e => e.timestamp >= weekStart)
                .GroupBy(e => e.playerName)
                .Select(g => g.Aggregate((max, e) => e.score > max.score ? e : max))
                .OrderByDescending(e => e.score)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// All-time leaderboard'u al
        /// </summary>
        public List<LeaderboardEntry> GetAllTimeLeaderboard(int count = 100)
        {
            return _data.allTimeEntries
                .OrderByDescending(e => e.score)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Kisisel en iyi skorlari al
        /// </summary>
        public List<LeaderboardEntry> GetPersonalBests(int count = 10)
        {
            return _data.personalBests
                .OrderByDescending(e => e.score)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Gunluk siralama al
        /// </summary>
        public int GetDailyRank(int seed = 0)
        {
            if (seed == 0) seed = GetTodaySeed();

            var dailyScores = _data.dailyEntries
                .Where(e => e.seed == seed)
                .OrderByDescending(e => e.score)
                .ToList();

            string playerName = GetPlayerName();
            int rank = dailyScores.FindIndex(e => e.playerName == playerName);

            return rank >= 0 ? rank + 1 : 0;
        }

        /// <summary>
        /// Belirli bir gun icin en iyi skoru al
        /// </summary>
        public int GetDailyBest(int seed)
        {
            var entry = _data.dailyEntries
                .Where(e => e.seed == seed && e.playerName == GetPlayerName())
                .OrderByDescending(e => e.score)
                .FirstOrDefault();

            return entry?.score ?? 0;
        }

        /// <summary>
        /// Oyuncunun cevresindeki siralama
        /// </summary>
        public List<LeaderboardEntry> GetNearbyRanks(int seed, int range = 5)
        {
            var allEntries = _data.dailyEntries
                .Where(e => e.seed == seed)
                .OrderByDescending(e => e.score)
                .ToList();

            string playerName = GetPlayerName();
            int playerIndex = allEntries.FindIndex(e => e.playerName == playerName);

            if (playerIndex < 0) return new List<LeaderboardEntry>();

            int start = Mathf.Max(0, playerIndex - range);
            int end = Mathf.Min(allEntries.Count, playerIndex + range + 1);

            return allEntries.GetRange(start, end - start);
        }

        /// <summary>
        /// Arkadas listesi siralama (placeholder)
        /// </summary>
        public List<LeaderboardEntry> GetFriendsLeaderboard()
        {
            // Online entegrasyon gerektirir
            // Simdilik bos liste
            return new List<LeaderboardEntry>();
        }
        #endregion

        #region Public Methods - Statistics
        /// <summary>
        /// Leaderboard istatistikleri
        /// </summary>
        public LeaderboardStats GetStats()
        {
            int todaySeed = GetTodaySeed();

            return new LeaderboardStats
            {
                personalBest = _data.personalBest,
                personalBestDate = _data.personalBestDate,
                totalSubmissions = _data.totalSubmissions,
                dailyRank = GetDailyRank(todaySeed),
                dailyParticipants = _data.dailyEntries.Count(e => e.seed == todaySeed),
                weeklyRank = GetWeeklyRank(),
                allTimeRank = GetAllTimeRank()
            };
        }

        /// <summary>
        /// Haftalik siralama
        /// </summary>
        public int GetWeeklyRank()
        {
            DateTime weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

            var weeklyScores = _data.dailyEntries
                .Where(e => e.timestamp >= weekStart)
                .GroupBy(e => e.playerName)
                .Select(g => new { Name = g.Key, Score = g.Max(e => e.score) })
                .OrderByDescending(x => x.Score)
                .ToList();

            string playerName = GetPlayerName();
            int rank = weeklyScores.FindIndex(x => x.Name == playerName);

            return rank >= 0 ? rank + 1 : 0;
        }

        /// <summary>
        /// All-time siralama
        /// </summary>
        public int GetAllTimeRank()
        {
            var allTimeScores = _data.allTimeEntries
                .OrderByDescending(e => e.score)
                .ToList();

            string playerName = GetPlayerName();
            int rank = allTimeScores.FindIndex(e => e.playerName == playerName);

            return rank >= 0 ? rank + 1 : 0;
        }
        #endregion

        #region Private Methods
        private void AddToLocalLeaderboard(LeaderboardEntry entry)
        {
            // Gunluk listeye ekle
            _data.dailyEntries.Add(entry);
            _data.totalSubmissions++;

            // Kisisel en iyilere ekle
            var existing = _data.personalBests.Find(e => e.seed == entry.seed);
            if (existing == null || entry.score > existing.score)
            {
                if (existing != null)
                    _data.personalBests.Remove(existing);
                _data.personalBests.Add(entry);
            }

            // Eski kayitlari temizle (30 gunluk)
            DateTime cutoff = DateTime.Now.AddDays(-30);
            _data.dailyEntries.RemoveAll(e => e.timestamp < cutoff);

            // Kisisel en iyi listesini sinirla
            if (_data.personalBests.Count > 100)
            {
                _data.personalBests = _data.personalBests
                    .OrderByDescending(e => e.score)
                    .Take(100)
                    .ToList();
            }
        }

        private void AddToAllTimeLeaderboard(LeaderboardEntry entry)
        {
            // All-time listeye ekle
            _data.allTimeEntries.Add(entry);

            // Listeyi sinirla (top 1000)
            if (_data.allTimeEntries.Count > 1000)
            {
                _data.allTimeEntries = _data.allTimeEntries
                    .OrderByDescending(e => e.score)
                    .Take(1000)
                    .ToList();
            }
        }

        private async void SubmitToOnlineLeaderboard(LeaderboardEntry entry)
        {
            // Online backend entegrasyonu gerektirir
            // Firebase, PlayFab, veya custom backend
            // Simdilik placeholder

            await System.Threading.Tasks.Task.Delay(100);

            Debug.Log("[LeaderboardSystem] Online submit - Backend entegrasyonu gerekli");
        }

        private int GetTodaySeed()
        {
            DateTime today = DateTime.Today;
            return today.Year * 10000 + today.Month * 100 + today.Day;
        }

        private string GetPlayerName()
        {
            if (ProfileSystem.Instance != null)
            {
                return ProfileSystem.Instance.PlayerName;
            }
            return "Oyuncu";
        }

        private void SaveLeaderboardData()
        {
            // Ana veriyi kaydet
            var saveData = new LeaderboardSaveData
            {
                personalBest = _data.personalBest,
                personalBestDate = _data.personalBestDate.ToString("o"),
                totalSubmissions = _data.totalSubmissions,
                dailyEntries = _data.dailyEntries,
                personalBests = _data.personalBests,
                allTimeEntries = _data.allTimeEntries
            };

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(Constants.SAVE_KEY_LEADERBOARD, json);
            PlayerPrefs.Save();
        }

        private void LoadLeaderboardData()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_LEADERBOARD, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var saveData = JsonUtility.FromJson<LeaderboardSaveData>(json);

                    if (saveData != null)
                    {
                        _data.personalBest = saveData.personalBest;
                        _data.totalSubmissions = saveData.totalSubmissions;
                        _data.dailyEntries = saveData.dailyEntries ?? new List<LeaderboardEntry>();
                        _data.personalBests = saveData.personalBests ?? new List<LeaderboardEntry>();
                        _data.allTimeEntries = saveData.allTimeEntries ?? new List<LeaderboardEntry>();

                        // Safe date parsing
                        if (!string.IsNullOrEmpty(saveData.personalBestDate))
                        {
                            if (DateTime.TryParse(saveData.personalBestDate, out DateTime parsedDate))
                            {
                                _data.personalBestDate = parsedDate;
                            }
                            else
                            {
                                Debug.LogWarning($"[LeaderboardSystem] Invalid date format: {saveData.personalBestDate}");
                                _data.personalBestDate = DateTime.MinValue;
                            }
                        }
                    }
                    else
                    {
                        _data = new LeaderboardData();
                    }
                }
                catch (ArgumentException ex)
                {
                    Debug.LogWarning($"[LeaderboardSystem] JSON parse hatası: {ex.Message}");
                    _data = new LeaderboardData();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[LeaderboardSystem] Yükleme hatası: {ex.Message}");
                    _data = new LeaderboardData();
                }
            }
            else
            {
                _data = new LeaderboardData();
            }

            Debug.Log($"[LeaderboardSystem] Yuklendi. En iyi: {_data.personalBest}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Print Leaderboard Stats")]
        private void DebugPrintStats()
        {
            var stats = GetStats();
            Debug.Log($"Personal Best: {stats.personalBest}");
            Debug.Log($"Daily Rank: #{stats.dailyRank}");
            Debug.Log($"Weekly Rank: #{stats.weeklyRank}");
            Debug.Log($"All-Time Rank: #{stats.allTimeRank}");
            Debug.Log($"Total Submissions: {stats.totalSubmissions}");
        }

        [ContextMenu("Print Daily Leaderboard")]
        private void DebugPrintDailyLeaderboard()
        {
            var entries = GetDailyLeaderboard(10);
            Debug.Log("=== Gunluk Siralama ===");
            for (int i = 0; i < entries.Count; i++)
            {
                Debug.Log($"#{i + 1} {entries[i].playerName}: {entries[i].score}");
            }
        }

        [ContextMenu("Add Test Entry")]
        private void DebugAddTestEntry()
        {
            var entry = new LeaderboardEntry
            {
                playerName = "TestPlayer",
                score = UnityEngine.Random.Range(1000, 5000),
                turns = UnityEngine.Random.Range(20, 100),
                endingType = EndingType.GoldenAge,
                timestamp = DateTime.Now,
                seed = GetTodaySeed()
            };

            AddToLocalLeaderboard(entry);
            SaveLeaderboardData();
            Debug.Log($"[LeaderboardSystem] Test entry eklendi: {entry.score}");
        }

        [ContextMenu("Reset Leaderboard")]
        private void DebugResetLeaderboard()
        {
            _data = new LeaderboardData();
            SaveLeaderboardData();
            Debug.Log("[LeaderboardSystem] Leaderboard sifirlandi!");
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Leaderboard ana veri yapisi
    /// </summary>
    [System.Serializable]
    public class LeaderboardData
    {
        public int personalBest;
        public DateTime personalBestDate;
        public int totalSubmissions;

        public List<LeaderboardEntry> dailyEntries;
        public List<LeaderboardEntry> personalBests;
        public List<LeaderboardEntry> allTimeEntries;

        public LeaderboardData()
        {
            dailyEntries = new List<LeaderboardEntry>();
            personalBests = new List<LeaderboardEntry>();
            allTimeEntries = new List<LeaderboardEntry>();
        }
    }

    /// <summary>
    /// Save data wrapper (DateTime serialization icin)
    /// </summary>
    [System.Serializable]
    public class LeaderboardSaveData
    {
        public int personalBest;
        public string personalBestDate;
        public int totalSubmissions;

        public List<LeaderboardEntry> dailyEntries;
        public List<LeaderboardEntry> personalBests;
        public List<LeaderboardEntry> allTimeEntries;
    }

    /// <summary>
    /// Leaderboard giris
    /// </summary>
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;
        public int turns;
        public EndingType endingType;
        public DateTime timestamp;
        public int seed;

        // Online icin
        public string odaId;
        public bool isVerified;
    }

    /// <summary>
    /// Leaderboard istatistikleri
    /// </summary>
    public class LeaderboardStats
    {
        public int personalBest;
        public DateTime personalBestDate;
        public int totalSubmissions;
        public int dailyRank;
        public int dailyParticipants;
        public int weeklyRank;
        public int allTimeRank;
    }
    #endregion
}
