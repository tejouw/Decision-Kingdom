using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Gunluk challenge sistemi
    /// Faz 6: Sosyal Ozellikler
    /// </summary>
    public class DailyChallengeSystem : MonoBehaviour
    {
        public static DailyChallengeSystem Instance { get; private set; }

        [Header("Daily Challenge Data")]
        [SerializeField] private DailyChallengeData _currentChallenge;
        [SerializeField] private DailyChallengeHistory _history;

        // Events
        public event Action<DailyChallengeData> OnDailyChallengeStarted;
        public event Action<DailyChallengeResult> OnDailyChallengeCompleted;
        public event Action OnNewDayDetected;

        #region Properties
        public DailyChallengeData CurrentChallenge => _currentChallenge;
        public bool HasCompletedToday => _history.HasCompletedToday(GetTodaySeed());
        public int CurrentStreak => _history.currentStreak;
        public int BestStreak => _history.bestStreak;
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
            _history = new DailyChallengeHistory();
            LoadChallengeData();
            CheckForNewDay();
        }

        private void Update()
        {
            // Her dakika yeni gun kontrolu
            if (Time.frameCount % 3600 == 0)
            {
                CheckForNewDay();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gunluk challenge'i baslat
        /// </summary>
        public bool StartDailyChallenge()
        {
            if (HasCompletedToday)
            {
                Debug.Log("[DailyChallengeSystem] Bugunku challenge zaten tamamlandi!");
                return false;
            }

            int todaySeed = GetTodaySeed();
            _currentChallenge = GenerateDailyChallenge(todaySeed);

            OnDailyChallengeStarted?.Invoke(_currentChallenge);

            Debug.Log($"[DailyChallengeSystem] Gunluk challenge basladi. Seed: {todaySeed}");
            return true;
        }

        /// <summary>
        /// Gunluk challenge'i tamamla
        /// </summary>
        public void CompleteDailyChallenge(GameStateData gameState, EndingType endingType)
        {
            if (_currentChallenge == null) return;

            var result = new DailyChallengeResult
            {
                seed = _currentChallenge.seed,
                date = DateTime.Today,
                turnsPlayed = gameState.turn,
                endingType = endingType,
                finalResources = new Resources(gameState.resources),
                score = CalculateChallengeScore(gameState, endingType),
                isVictory = EndingSystem.IsVictory(endingType)
            };

            // Tarihe ekle
            _history.AddResult(result);

            // Streak guncelle
            UpdateStreak(result.isVictory);

            OnDailyChallengeCompleted?.Invoke(result);

            // Leaderboard'a gonder
            if (LeaderboardSystem.Instance != null)
            {
                LeaderboardSystem.Instance.SubmitDailyScore(result);
            }

            SaveChallengeData();

            Debug.Log($"[DailyChallengeSystem] Challenge tamamlandi. Skor: {result.score}");
        }

        /// <summary>
        /// Bugunun seed'ini al
        /// </summary>
        public int GetTodaySeed()
        {
            DateTime today = DateTime.Today;
            return today.Year * 10000 + today.Month * 100 + today.Day;
        }

        /// <summary>
        /// Belirli bir seed icin random generator olustur
        /// </summary>
        public System.Random GetSeededRandom()
        {
            return new System.Random(_currentChallenge?.seed ?? GetTodaySeed());
        }

        /// <summary>
        /// Bugunun challenge bilgisini al
        /// </summary>
        public DailyChallengeInfo GetTodayInfo()
        {
            int seed = GetTodaySeed();
            var challenge = GenerateDailyChallenge(seed);

            return new DailyChallengeInfo
            {
                seed = seed,
                date = DateTime.Today,
                era = challenge.era,
                scenario = challenge.scenario,
                modifiers = challenge.modifiers,
                hasCompleted = HasCompletedToday,
                bestScore = _history.GetBestScoreForSeed(seed),
                globalRank = LeaderboardSystem.Instance?.GetDailyRank(seed) ?? 0
            };
        }

        /// <summary>
        /// Gecmis sonuclari al
        /// </summary>
        public List<DailyChallengeResult> GetHistory(int count = 7)
        {
            return _history.GetRecentResults(count);
        }

        /// <summary>
        /// Belirli bir tarihin sonucunu al
        /// </summary>
        public DailyChallengeResult GetResultForDate(DateTime date)
        {
            int seed = date.Year * 10000 + date.Month * 100 + date.Day;
            return _history.GetResultForSeed(seed);
        }
        #endregion

        #region Private Methods
        private DailyChallengeData GenerateDailyChallenge(int seed)
        {
            var random = new System.Random(seed);

            var challenge = new DailyChallengeData
            {
                seed = seed,
                date = DateTime.Today
            };

            // Era secimi (weighted random)
            int eraRoll = random.Next(100);
            if (eraRoll < 40)
                challenge.era = Era.Medieval;
            else if (eraRoll < 60)
                challenge.era = Era.Renaissance;
            else if (eraRoll < 75)
                challenge.era = Era.Industrial;
            else if (eraRoll < 90)
                challenge.era = Era.Modern;
            else
                challenge.era = Era.Future;

            // Senaryo secimi
            string[] scenarios = { "good_king", "young_heir", "coup_leader", "rich_merchant", "peoples_choice", "holy_ruler" };
            challenge.scenario = scenarios[random.Next(scenarios.Length)];

            // Modifier'lar
            challenge.modifiers = new List<ChallengeModifier>();

            // Her gun 1-3 modifier
            int modifierCount = random.Next(1, 4);
            var allModifiers = GetAllModifiers();

            for (int i = 0; i < modifierCount && allModifiers.Count > 0; i++)
            {
                int index = random.Next(allModifiers.Count);
                challenge.modifiers.Add(allModifiers[index]);
                allModifiers.RemoveAt(index);
            }

            // Baslangic kaynaklari (senaryo bazli + modifier etkileri)
            challenge.startingResources = CalculateStartingResources(challenge);

            return challenge;
        }

        private Resources CalculateStartingResources(DailyChallengeData challenge)
        {
            // Senaryo bazli baslangic
            var scenarioInfo = PrestigeManager.Scenarios.ContainsKey(challenge.scenario)
                ? PrestigeManager.Scenarios[challenge.scenario]
                : PrestigeManager.Scenarios["good_king"];

            var resources = new Resources(scenarioInfo.startingResources);

            // Modifier etkileri
            foreach (var modifier in challenge.modifiers)
            {
                ApplyModifierToResources(modifier, resources);
            }

            return resources;
        }

        private void ApplyModifierToResources(ChallengeModifier modifier, Resources resources)
        {
            switch (modifier)
            {
                case ChallengeModifier.HighStakes:
                    // Kaynaklar daha hassas
                    break;
                case ChallengeModifier.PovertyStart:
                    resources.Gold = Mathf.Max(20, resources.Gold - 20);
                    break;
                case ChallengeModifier.WarTorn:
                    resources.Military = Mathf.Max(20, resources.Military - 20);
                    resources.Happiness = Mathf.Max(30, resources.Happiness - 10);
                    break;
                case ChallengeModifier.ReligiousCrisis:
                    resources.Faith = Mathf.Max(20, resources.Faith - 20);
                    break;
                case ChallengeModifier.GoldenAge:
                    resources.Gold = Mathf.Min(80, resources.Gold + 15);
                    resources.Happiness = Mathf.Min(80, resources.Happiness + 10);
                    break;
                case ChallengeModifier.Balanced:
                    resources.Gold = 50;
                    resources.Happiness = 50;
                    resources.Military = 50;
                    resources.Faith = 50;
                    break;
            }
        }

        private List<ChallengeModifier> GetAllModifiers()
        {
            return new List<ChallengeModifier>
            {
                ChallengeModifier.HighStakes,
                ChallengeModifier.PovertyStart,
                ChallengeModifier.WarTorn,
                ChallengeModifier.ReligiousCrisis,
                ChallengeModifier.GoldenAge,
                ChallengeModifier.Balanced,
                ChallengeModifier.NoSecondChance,
                ChallengeModifier.CharacterFocus,
                ChallengeModifier.ResourceRace,
                ChallengeModifier.Speedrun
            };
        }

        private int CalculateChallengeScore(GameStateData gameState, EndingType endingType)
        {
            int score = 0;

            // Temel puan: Hayatta kalinan tur
            score += gameState.turn * 10;

            // Kaynak dengesi bonusu
            float balance = gameState.resources.GetBalanceScore();
            score += Mathf.RoundToInt(balance * 200);

            // Ending bonusu
            int endingBonus = EndingSystem.GetPrestigeBonus(endingType);
            score += endingBonus * 2;

            // Zafer bonusu
            if (EndingSystem.IsVictory(endingType))
            {
                score += 500;
            }

            // Modifier bonuslari
            if (_currentChallenge != null)
            {
                foreach (var modifier in _currentChallenge.modifiers)
                {
                    score += GetModifierBonus(modifier);
                }
            }

            // Streak bonusu
            score += CurrentStreak * 50;

            return score;
        }

        private int GetModifierBonus(ChallengeModifier modifier)
        {
            return modifier switch
            {
                ChallengeModifier.HighStakes => 100,
                ChallengeModifier.PovertyStart => 150,
                ChallengeModifier.WarTorn => 150,
                ChallengeModifier.ReligiousCrisis => 150,
                ChallengeModifier.NoSecondChance => 200,
                ChallengeModifier.Speedrun => 100,
                _ => 50
            };
        }

        private void UpdateStreak(bool isVictory)
        {
            if (isVictory)
            {
                _history.currentStreak++;
                if (_history.currentStreak > _history.bestStreak)
                {
                    _history.bestStreak = _history.currentStreak;
                }
            }
            else
            {
                _history.currentStreak = 0;
            }
        }

        private void CheckForNewDay()
        {
            int todaySeed = GetTodaySeed();

            if (_history.lastPlayedSeed != todaySeed)
            {
                _currentChallenge = null;
                OnNewDayDetected?.Invoke();

                Debug.Log("[DailyChallengeSystem] Yeni gun tespit edildi!");
            }
        }

        private void SaveChallengeData()
        {
            string json = JsonUtility.ToJson(_history);
            PlayerPrefs.SetString(Constants.SAVE_KEY_DAILY_CHALLENGE, json);
            PlayerPrefs.Save();
        }

        private void LoadChallengeData()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_DAILY_CHALLENGE, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    _history = JsonUtility.FromJson<DailyChallengeHistory>(json);
                }
                catch
                {
                    _history = new DailyChallengeHistory();
                }
            }
            else
            {
                _history = new DailyChallengeHistory();
            }

            Debug.Log($"[DailyChallengeSystem] Yuklendi. Streak: {_history.currentStreak}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Start Today's Challenge")]
        private void DebugStartChallenge()
        {
            StartDailyChallenge();
        }

        [ContextMenu("Print Challenge Info")]
        private void DebugPrintChallengeInfo()
        {
            var info = GetTodayInfo();
            Debug.Log($"Date: {info.date:yyyy-MM-dd}");
            Debug.Log($"Seed: {info.seed}");
            Debug.Log($"Era: {info.era}");
            Debug.Log($"Scenario: {info.scenario}");
            Debug.Log($"Modifiers: {string.Join(", ", info.modifiers)}");
            Debug.Log($"Completed: {info.hasCompleted}");
            Debug.Log($"Best Score: {info.bestScore}");
        }

        [ContextMenu("Reset Challenge History")]
        private void DebugResetHistory()
        {
            _history = new DailyChallengeHistory();
            SaveChallengeData();
            Debug.Log("[DailyChallengeSystem] Tarihce sifirlandi!");
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Gunluk challenge verisi
    /// </summary>
    [System.Serializable]
    public class DailyChallengeData
    {
        public int seed;
        public DateTime date;
        public Era era;
        public string scenario;
        public List<ChallengeModifier> modifiers;
        public Resources startingResources;

        public DailyChallengeData()
        {
            modifiers = new List<ChallengeModifier>();
        }
    }

    /// <summary>
    /// Challenge sonucu
    /// </summary>
    [System.Serializable]
    public class DailyChallengeResult
    {
        public int seed;
        public DateTime date;
        public int turnsPlayed;
        public EndingType endingType;
        public Resources finalResources;
        public int score;
        public bool isVictory;
    }

    /// <summary>
    /// Challenge tarihcesi
    /// </summary>
    [System.Serializable]
    public class DailyChallengeHistory
    {
        public List<DailyChallengeResult> results;
        public int lastPlayedSeed;
        public int currentStreak;
        public int bestStreak;
        public int totalChallengesCompleted;
        public int totalVictories;

        public DailyChallengeHistory()
        {
            results = new List<DailyChallengeResult>();
        }

        public bool HasCompletedToday(int todaySeed)
        {
            return results.Exists(r => r.seed == todaySeed);
        }

        public void AddResult(DailyChallengeResult result)
        {
            results.Add(result);
            lastPlayedSeed = result.seed;
            totalChallengesCompleted++;

            if (result.isVictory)
                totalVictories++;

            // Son 30 gunu tut
            if (results.Count > 30)
            {
                results.RemoveAt(0);
            }
        }

        public int GetBestScoreForSeed(int seed)
        {
            var result = results.Find(r => r.seed == seed);
            return result?.score ?? 0;
        }

        public DailyChallengeResult GetResultForSeed(int seed)
        {
            return results.Find(r => r.seed == seed);
        }

        public List<DailyChallengeResult> GetRecentResults(int count)
        {
            int start = Mathf.Max(0, results.Count - count);
            return results.GetRange(start, Mathf.Min(count, results.Count));
        }
    }

    /// <summary>
    /// Challenge bilgisi (UI icin)
    /// </summary>
    public class DailyChallengeInfo
    {
        public int seed;
        public DateTime date;
        public Era era;
        public string scenario;
        public List<ChallengeModifier> modifiers;
        public bool hasCompleted;
        public int bestScore;
        public int globalRank;
    }

    /// <summary>
    /// Challenge modifier'lari
    /// </summary>
    public enum ChallengeModifier
    {
        HighStakes,         // Kaynak degisimleri %50 daha etkili
        PovertyStart,       // Dusuk altin ile basla
        WarTorn,            // Dusuk askeri guc ve mutluluk
        ReligiousCrisis,    // Dusuk inanc ile basla
        GoldenAge,          // Yuksek kaynaklarla basla
        Balanced,           // Tum kaynaklar 50
        NoSecondChance,     // Revive yok
        CharacterFocus,     // Daha fazla karakter eventi
        ResourceRace,       // Belirli kaynagi yuksek tutma hedefi
        Speedrun            // 30 tur icinde bitir
    }
    #endregion
}
