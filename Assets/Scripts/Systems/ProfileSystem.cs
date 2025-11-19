using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Oyuncu profil sistemi
    /// Faz 6: Sosyal Ozellikler
    /// </summary>
    public class ProfileSystem : MonoBehaviour
    {
        public static ProfileSystem Instance { get; private set; }

        [Header("Profile Data")]
        [SerializeField] private PlayerProfile _profile;

        // Events
        public event Action<PlayerProfile> OnProfileUpdated;
        public event Action<string> OnTitleUnlocked;
        public event Action<string> OnBadgeEarned;

        #region Properties
        public PlayerProfile Profile => _profile;
        public string PlayerName => _profile.playerName;
        public string CurrentTitle => _profile.currentTitle;
        public int TotalPlayTime => _profile.totalPlayTimeMinutes;
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
            _profile = new PlayerProfile();
            LoadProfile();
        }
        #endregion

        #region Public Methods - Profile Management
        /// <summary>
        /// Oyuncu ismini ayarla
        /// </summary>
        public void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 20)
                return;

            _profile.playerName = name;
            SaveProfile();
            OnProfileUpdated?.Invoke(_profile);

            Debug.Log($"[ProfileSystem] Oyuncu ismi: {name}");
        }

        /// <summary>
        /// Aktif unvani ayarla
        /// </summary>
        public bool SetTitle(string titleId)
        {
            if (!_profile.unlockedTitles.Contains(titleId))
            {
                Debug.Log($"[ProfileSystem] Unvan acilmamis: {titleId}");
                return false;
            }

            _profile.currentTitle = titleId;
            SaveProfile();
            OnProfileUpdated?.Invoke(_profile);

            return true;
        }

        /// <summary>
        /// Yeni unvan ac
        /// </summary>
        public void UnlockTitle(string titleId)
        {
            if (_profile.unlockedTitles.Contains(titleId))
                return;

            _profile.unlockedTitles.Add(titleId);
            OnTitleUnlocked?.Invoke(titleId);

            SaveProfile();

            Debug.Log($"[ProfileSystem] Yeni unvan: {GetTitleName(titleId)}");
        }

        /// <summary>
        /// Rozet kazan
        /// </summary>
        public void EarnBadge(string badgeId)
        {
            if (_profile.earnedBadges.Contains(badgeId))
                return;

            _profile.earnedBadges.Add(badgeId);
            OnBadgeEarned?.Invoke(badgeId);

            SaveProfile();

            Debug.Log($"[ProfileSystem] Yeni rozet: {GetBadgeName(badgeId)}");
        }
        #endregion

        #region Public Methods - Statistics Update
        /// <summary>
        /// Oyun sonunda profili guncelle
        /// </summary>
        public void UpdateAfterGame(GameStateData gameState, EndingType endingType, int score)
        {
            _profile.gamesPlayed++;
            _profile.lastPlayDate = DateTime.Now;

            // Zafer/Yenilgi
            if (EndingSystem.IsVictory(endingType))
            {
                _profile.totalVictories++;
            }

            // En iyi skor
            if (score > _profile.highScore)
            {
                _profile.highScore = score;
                _profile.highScoreDate = DateTime.Now;
            }

            // En uzun hayatta kalma
            if (gameState.turn > _profile.longestRun)
            {
                _profile.longestRun = gameState.turn;
            }

            // Era mastery
            UpdateEraMastery(gameState.era, gameState.turn, EndingSystem.IsVictory(endingType));

            // Karakter koleksiyonu
            UpdateCharacterCollection(gameState);

            // Nadir event koleksiyonu
            UpdateRareEventCollection(gameState);

            // Unvan kontrolleri
            CheckTitleUnlocks();

            // Rozet kontrolleri
            CheckBadgeUnlocks(gameState, endingType);

            // Oyun stili guncelle
            UpdatePlayStyle(gameState);

            SaveProfile();
            OnProfileUpdated?.Invoke(_profile);
        }

        /// <summary>
        /// Oynama suresi ekle
        /// </summary>
        public void AddPlayTime(int minutes)
        {
            _profile.totalPlayTimeMinutes += minutes;
            SaveProfile();
        }
        #endregion

        #region Public Methods - Queries
        /// <summary>
        /// Profil ozetini al
        /// </summary>
        public ProfileSummary GetProfileSummary()
        {
            return new ProfileSummary
            {
                playerName = _profile.playerName,
                currentTitle = GetTitleName(_profile.currentTitle),
                gamesPlayed = _profile.gamesPlayed,
                totalVictories = _profile.totalVictories,
                winRate = _profile.gamesPlayed > 0 ? (float)_profile.totalVictories / _profile.gamesPlayed : 0,
                highScore = _profile.highScore,
                longestRun = _profile.longestRun,
                totalPlayTime = TimeSpan.FromMinutes(_profile.totalPlayTimeMinutes),
                favoriteEra = GetFavoriteEra(),
                playStyle = _profile.dominantPlayStyle,
                badgeCount = _profile.earnedBadges.Count,
                titleCount = _profile.unlockedTitles.Count,
                charactersMet = _profile.characterCollection.Count,
                rareEventsSeen = _profile.rareEventCollection.Count
            };
        }

        /// <summary>
        /// Era mastery bilgisini al
        /// </summary>
        public EraMasteryInfo GetEraMastery(Era era)
        {
            if (!_profile.eraMastery.TryGetValue(era, out var mastery))
            {
                mastery = new EraMasteryData();
            }

            return new EraMasteryInfo
            {
                era = era,
                gamesPlayed = mastery.gamesPlayed,
                victories = mastery.victories,
                bestTurn = mastery.bestTurn,
                masteryLevel = CalculateMasteryLevel(mastery),
                masteryProgress = CalculateMasteryProgress(mastery)
            };
        }

        /// <summary>
        /// Karakter koleksiyonunu al
        /// </summary>
        public List<CharacterCollectionEntry> GetCharacterCollection()
        {
            var result = new List<CharacterCollectionEntry>();

            foreach (var kvp in _profile.characterCollection)
            {
                result.Add(new CharacterCollectionEntry
                {
                    characterId = kvp.Key,
                    characterName = GetCharacterName(kvp.Key),
                    timesEncountered = kvp.Value.timesEncountered,
                    bestRelationship = kvp.Value.bestRelationship,
                    worstRelationship = kvp.Value.worstRelationship,
                    specialEndingsUnlocked = kvp.Value.specialEndingsUnlocked
                });
            }

            return result.OrderByDescending(c => c.timesEncountered).ToList();
        }

        /// <summary>
        /// Tum unvanlari al
        /// </summary>
        public List<TitleInfo> GetAllTitles()
        {
            var titles = new List<TitleInfo>();

            foreach (var titleId in GetAllTitleIds())
            {
                titles.Add(new TitleInfo
                {
                    id = titleId,
                    name = GetTitleName(titleId),
                    description = GetTitleDescription(titleId),
                    isUnlocked = _profile.unlockedTitles.Contains(titleId),
                    isEquipped = _profile.currentTitle == titleId
                });
            }

            return titles;
        }

        /// <summary>
        /// Tum rozetleri al
        /// </summary>
        public List<BadgeInfo> GetAllBadges()
        {
            var badges = new List<BadgeInfo>();

            foreach (var badgeId in GetAllBadgeIds())
            {
                badges.Add(new BadgeInfo
                {
                    id = badgeId,
                    name = GetBadgeName(badgeId),
                    description = GetBadgeDescription(badgeId),
                    isEarned = _profile.earnedBadges.Contains(badgeId)
                });
            }

            return badges;
        }

        /// <summary>
        /// Favori era
        /// </summary>
        public Era GetFavoriteEra()
        {
            if (_profile.eraMastery.Count == 0)
                return Era.Medieval;

            return _profile.eraMastery
                .OrderByDescending(kvp => kvp.Value.gamesPlayed)
                .First().Key;
        }
        #endregion

        #region Private Methods - Updates
        private void UpdateEraMastery(Era era, int turns, bool isVictory)
        {
            if (!_profile.eraMastery.ContainsKey(era))
            {
                _profile.eraMastery[era] = new EraMasteryData();
            }

            var mastery = _profile.eraMastery[era];
            mastery.gamesPlayed++;

            if (isVictory)
                mastery.victories++;

            if (turns > mastery.bestTurn)
                mastery.bestTurn = turns;

            mastery.totalTurns += turns;
        }

        private void UpdateCharacterCollection(GameStateData gameState)
        {
            foreach (var charState in gameState.characterStates)
            {
                string charId = charState.Key;

                if (!_profile.characterCollection.ContainsKey(charId))
                {
                    _profile.characterCollection[charId] = new CharacterCollectionData();
                }

                var data = _profile.characterCollection[charId];
                data.timesEncountered++;

                if (charState.Value.relationshipLevel > data.bestRelationship)
                    data.bestRelationship = charState.Value.relationshipLevel;

                if (charState.Value.relationshipLevel < data.worstRelationship)
                    data.worstRelationship = charState.Value.relationshipLevel;
            }
        }

        private void UpdateRareEventCollection(GameStateData gameState)
        {
            // Flag bazli nadir event kontrolu
            string[] rareEvents = {
                "dragon_slayer", "grail_found", "time_saved",
                "plague_survived", "crusade_joined", "tournament_won"
            };

            foreach (var eventId in rareEvents)
            {
                if (gameState.HasFlag(eventId) && !_profile.rareEventCollection.Contains(eventId))
                {
                    _profile.rareEventCollection.Add(eventId);
                }
            }
        }

        private void UpdatePlayStyle(GameStateData gameState)
        {
            // Basit oyun stili analizi
            var res = gameState.resources;
            float avgGold = res.Gold;
            float avgMilitary = res.Military;
            float avgFaith = res.Faith;
            float avgHappiness = res.Happiness;

            // En yuksek kaynak
            float max = Mathf.Max(avgGold, avgMilitary, avgFaith, avgHappiness);

            if (max == avgGold)
                _profile.dominantPlayStyle = "Tuccar";
            else if (max == avgMilitary)
                _profile.dominantPlayStyle = "Savasci";
            else if (max == avgFaith)
                _profile.dominantPlayStyle = "Dindar";
            else if (max == avgHappiness)
                _profile.dominantPlayStyle = "Populist";
            else
                _profile.dominantPlayStyle = "Dengeli";
        }

        private void CheckTitleUnlocks()
        {
            // Oyun sayisi bazli unvanlar
            if (_profile.gamesPlayed >= 1)
                UnlockTitle("newbie");
            if (_profile.gamesPlayed >= 10)
                UnlockTitle("experienced");
            if (_profile.gamesPlayed >= 50)
                UnlockTitle("veteran");
            if (_profile.gamesPlayed >= 100)
                UnlockTitle("master");

            // Zafer bazli unvanlar
            if (_profile.totalVictories >= 1)
                UnlockTitle("winner");
            if (_profile.totalVictories >= 10)
                UnlockTitle("champion");
            if (_profile.totalVictories >= 25)
                UnlockTitle("legend");

            // Skor bazli unvanlar
            if (_profile.highScore >= 1000)
                UnlockTitle("scorer");
            if (_profile.highScore >= 5000)
                UnlockTitle("high_scorer");
            if (_profile.highScore >= 10000)
                UnlockTitle("elite_scorer");

            // Hayatta kalma bazli unvanlar
            if (_profile.longestRun >= 50)
                UnlockTitle("survivor");
            if (_profile.longestRun >= 100)
                UnlockTitle("immortal");
        }

        private void CheckBadgeUnlocks(GameStateData gameState, EndingType endingType)
        {
            // Ending bazli rozetler
            if (EndingSystem.IsVictory(endingType))
            {
                EarnBadge("first_victory");

                switch (endingType)
                {
                    case EndingType.GoldenAge:
                        EarnBadge("golden_age");
                        break;
                    case EndingType.DragonSlayer:
                        EarnBadge("dragon_slayer");
                        break;
                    case EndingType.GrailKeeper:
                        EarnBadge("grail_keeper");
                        break;
                    case EndingType.LegendaryKing:
                        EarnBadge("legendary_king");
                        break;
                }
            }

            // Era bazli rozetler
            if (_profile.eraMastery.ContainsKey(Era.Medieval) && _profile.eraMastery[Era.Medieval].victories >= 5)
                EarnBadge("medieval_master");
            if (_profile.eraMastery.ContainsKey(Era.Future) && _profile.eraMastery[Era.Future].victories >= 3)
                EarnBadge("future_pioneer");

            // Karakter bazli rozetler
            if (_profile.characterCollection.Count >= 6)
                EarnBadge("social_butterfly");
        }
        #endregion

        #region Private Methods - Calculations
        private int CalculateMasteryLevel(EraMasteryData mastery)
        {
            int score = mastery.gamesPlayed * 10 + mastery.victories * 50 + mastery.bestTurn;

            if (score >= 1000) return 5;
            if (score >= 500) return 4;
            if (score >= 250) return 3;
            if (score >= 100) return 2;
            if (score >= 25) return 1;
            return 0;
        }

        private float CalculateMasteryProgress(EraMasteryData mastery)
        {
            int level = CalculateMasteryLevel(mastery);
            int score = mastery.gamesPlayed * 10 + mastery.victories * 50 + mastery.bestTurn;

            int[] thresholds = { 0, 25, 100, 250, 500, 1000 };

            if (level >= 5) return 1f;

            int current = score - thresholds[level];
            int needed = thresholds[level + 1] - thresholds[level];

            return (float)current / needed;
        }
        #endregion

        #region Private Methods - Data Access
        private List<string> GetAllTitleIds()
        {
            return new List<string>
            {
                "newbie", "experienced", "veteran", "master",
                "winner", "champion", "legend",
                "scorer", "high_scorer", "elite_scorer",
                "survivor", "immortal",
                "merchant", "warrior", "priest", "diplomat"
            };
        }

        private string GetTitleName(string titleId)
        {
            return titleId switch
            {
                "newbie" => "Acemi Kral",
                "experienced" => "Tecrubeli Hukumdar",
                "veteran" => "Kizil Kral",
                "master" => "Usta Hukumdar",
                "winner" => "Zafer Sahibi",
                "champion" => "Sampiyon",
                "legend" => "Efsane",
                "scorer" => "Puan Avcisi",
                "high_scorer" => "Yuksek Skorcu",
                "elite_scorer" => "Elit Skorcu",
                "survivor" => "Hayatta Kalan",
                "immortal" => "Olumsuz",
                "merchant" => "Tuccar Kral",
                "warrior" => "Savasci Kral",
                "priest" => "Kutsal Kral",
                "diplomat" => "Diplomat Kral",
                _ => titleId
            };
        }

        private string GetTitleDescription(string titleId)
        {
            return titleId switch
            {
                "newbie" => "Ilk oyununu tamamla",
                "experienced" => "10 oyun tamamla",
                "veteran" => "50 oyun tamamla",
                "master" => "100 oyun tamamla",
                "winner" => "Ilk zaferini kazan",
                "champion" => "10 zafer kazan",
                "legend" => "25 zafer kazan",
                "scorer" => "1000+ skor al",
                "high_scorer" => "5000+ skor al",
                "elite_scorer" => "10000+ skor al",
                "survivor" => "50 tur hayatta kal",
                "immortal" => "100 tur hayatta kal",
                _ => ""
            };
        }

        private List<string> GetAllBadgeIds()
        {
            return new List<string>
            {
                "first_victory", "golden_age", "dragon_slayer", "grail_keeper",
                "legendary_king", "medieval_master", "future_pioneer",
                "social_butterfly", "speed_runner", "perfectionist"
            };
        }

        private string GetBadgeName(string badgeId)
        {
            return badgeId switch
            {
                "first_victory" => "Ilk Zafer",
                "golden_age" => "Altin Cag",
                "dragon_slayer" => "Ejderha Avcisi",
                "grail_keeper" => "Kase Koruyucusu",
                "legendary_king" => "Efsanevi Kral",
                "medieval_master" => "Ortacag Ustasi",
                "future_pioneer" => "Gelecek Oncusu",
                "social_butterfly" => "Sosyal Kelebek",
                "speed_runner" => "Hiz Kosusu",
                "perfectionist" => "Mukemmeliyetci",
                _ => badgeId
            };
        }

        private string GetBadgeDescription(string badgeId)
        {
            return badgeId switch
            {
                "first_victory" => "Ilk zaferini kazan",
                "golden_age" => "Altin Cag sonuna ulas",
                "dragon_slayer" => "Ejderhayi yen",
                "grail_keeper" => "Kutsal Kase'yi bul",
                "legendary_king" => "100 tur yasa",
                "medieval_master" => "Ortacag'da 5 zafer",
                "future_pioneer" => "Gelecek'te 3 zafer",
                "social_butterfly" => "6 karakterle tani",
                "speed_runner" => "30 turda zafer",
                "perfectionist" => "Tum kaynaklar dengeli bitir",
                _ => ""
            };
        }

        private string GetCharacterName(string characterId)
        {
            return characterId switch
            {
                "marcus" => "Danisman Marcus",
                "miriam" => "Tuccar Miriam",
                "valerius" => "General Valerius",
                "benedictus" => "Rahip Benedictus",
                "eleanor" => "Kralice Eleanor",
                "edmund" => "Veliaht Edmund",
                _ => characterId
            };
        }
        #endregion

        #region Save/Load
        private void SaveProfile()
        {
            string json = JsonUtility.ToJson(_profile);
            PlayerPrefs.SetString(Constants.SAVE_KEY_PROFILE, json);
            PlayerPrefs.Save();
        }

        private void LoadProfile()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_PROFILE, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    _profile = JsonUtility.FromJson<PlayerProfile>(json);
                }
                catch
                {
                    _profile = new PlayerProfile();
                }
            }
            else
            {
                _profile = new PlayerProfile();
            }

            Debug.Log($"[ProfileSystem] Yuklendi. Oyuncu: {_profile.playerName}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Print Profile Summary")]
        private void DebugPrintProfile()
        {
            var summary = GetProfileSummary();
            Debug.Log($"Player: {summary.playerName}");
            Debug.Log($"Title: {summary.currentTitle}");
            Debug.Log($"Games: {summary.gamesPlayed}");
            Debug.Log($"Victories: {summary.totalVictories}");
            Debug.Log($"Win Rate: {summary.winRate:P1}");
            Debug.Log($"High Score: {summary.highScore}");
            Debug.Log($"Longest Run: {summary.longestRun}");
            Debug.Log($"Play Style: {summary.playStyle}");
        }

        [ContextMenu("Reset Profile")]
        private void DebugResetProfile()
        {
            _profile = new PlayerProfile();
            SaveProfile();
            Debug.Log("[ProfileSystem] Profil sifirlandi!");
        }

        [ContextMenu("Unlock All Titles")]
        private void DebugUnlockAllTitles()
        {
            foreach (var titleId in GetAllTitleIds())
            {
                UnlockTitle(titleId);
            }
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Oyuncu profili
    /// </summary>
    [System.Serializable]
    public class PlayerProfile
    {
        // Temel bilgiler
        public string playerName;
        public string currentTitle;
        public DateTime creationDate;
        public DateTime lastPlayDate;

        // Istatistikler
        public int gamesPlayed;
        public int totalVictories;
        public int highScore;
        public DateTime highScoreDate;
        public int longestRun;
        public int totalPlayTimeMinutes;

        // Koleksiyonlar
        public List<string> unlockedTitles;
        public List<string> earnedBadges;
        public List<string> rareEventCollection;
        public Dictionary<Era, EraMasteryData> eraMastery;
        public Dictionary<string, CharacterCollectionData> characterCollection;

        // Oyun stili
        public string dominantPlayStyle;

        public PlayerProfile()
        {
            playerName = "Kral";
            currentTitle = "newbie";
            creationDate = DateTime.Now;
            lastPlayDate = DateTime.Now;
            unlockedTitles = new List<string> { "newbie" };
            earnedBadges = new List<string>();
            rareEventCollection = new List<string>();
            eraMastery = new Dictionary<Era, EraMasteryData>();
            characterCollection = new Dictionary<string, CharacterCollectionData>();
            dominantPlayStyle = "Dengeli";
        }
    }

    /// <summary>
    /// Era mastery verisi
    /// </summary>
    [System.Serializable]
    public class EraMasteryData
    {
        public int gamesPlayed;
        public int victories;
        public int bestTurn;
        public int totalTurns;
    }

    /// <summary>
    /// Karakter koleksiyon verisi
    /// </summary>
    [System.Serializable]
    public class CharacterCollectionData
    {
        public int timesEncountered;
        public int bestRelationship;
        public int worstRelationship;
        public List<string> specialEndingsUnlocked;

        public CharacterCollectionData()
        {
            worstRelationship = 0;
            bestRelationship = 0;
            specialEndingsUnlocked = new List<string>();
        }
    }

    /// <summary>
    /// Profil ozeti (UI icin)
    /// </summary>
    public class ProfileSummary
    {
        public string playerName;
        public string currentTitle;
        public int gamesPlayed;
        public int totalVictories;
        public float winRate;
        public int highScore;
        public int longestRun;
        public TimeSpan totalPlayTime;
        public Era favoriteEra;
        public string playStyle;
        public int badgeCount;
        public int titleCount;
        public int charactersMet;
        public int rareEventsSeen;
    }

    /// <summary>
    /// Era mastery bilgisi (UI icin)
    /// </summary>
    public class EraMasteryInfo
    {
        public Era era;
        public int gamesPlayed;
        public int victories;
        public int bestTurn;
        public int masteryLevel;
        public float masteryProgress;
    }

    /// <summary>
    /// Karakter koleksiyon girisim (UI icin)
    /// </summary>
    public class CharacterCollectionEntry
    {
        public string characterId;
        public string characterName;
        public int timesEncountered;
        public int bestRelationship;
        public int worstRelationship;
        public List<string> specialEndingsUnlocked;
    }

    /// <summary>
    /// Unvan bilgisi (UI icin)
    /// </summary>
    public class TitleInfo
    {
        public string id;
        public string name;
        public string description;
        public bool isUnlocked;
        public bool isEquipped;
    }

    /// <summary>
    /// Rozet bilgisi (UI icin)
    /// </summary>
    public class BadgeInfo
    {
        public string id;
        public string name;
        public string description;
        public bool isEarned;
    }
    #endregion
}
