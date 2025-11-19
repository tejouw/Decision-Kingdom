using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Tam oyun durumu verisi (save/load için)
    /// </summary>
    [System.Serializable]
    public class GameStateData
    {
        [Tooltip("Oyuncu kaynakları")]
        public Resources resources;

        [Tooltip("Mevcut tur")]
        public int turn;

        [Tooltip("Mevcut dönem")]
        public Era era;

        [Tooltip("Karakter durumları")]
        public Dictionary<string, CharacterState> characterStates;

        [Tooltip("Aktif flag'ler")]
        public HashSet<string> flags;

        [Tooltip("Oynanmış event ID'leri")]
        public List<string> eventHistory;

        [Tooltip("Tetiklenmiş event kuyruğu")]
        public Queue<string> triggeredEventQueue;

        [Tooltip("Oyun durumu")]
        public GameState gameState;

        [Tooltip("Game Over sebebi")]
        public GameOverReason gameOverReason;

        [Tooltip("Son tipi")]
        public EndingType endingType;

        [Tooltip("Miras tipi")]
        public LegacyType legacyType;

        [Tooltip("Başlangıç kaynakları (istatistik için)")]
        public Resources startingResources;

        [Tooltip("Oturum başlangıç zamanı")]
        public System.DateTime sessionStartTime;

        public GameStateData()
        {
            resources = new Resources();
            turn = 1;
            era = Era.Medieval;
            characterStates = new Dictionary<string, CharacterState>();
            flags = new HashSet<string>();
            eventHistory = new List<string>();
            triggeredEventQueue = new Queue<string>();
            gameState = GameState.MainMenu;
            gameOverReason = GameOverReason.None;
            endingType = EndingType.None;
            legacyType = LegacyType.None;
            startingResources = new Resources();
            sessionStartTime = System.DateTime.Now;
        }

        /// <summary>
        /// Yeni oyun başlat
        /// </summary>
        public void StartNewGame(Era startingEra = Era.Medieval, Resources customResources = null)
        {
            if (customResources != null)
            {
                resources = new Resources(customResources);
                startingResources = new Resources(customResources);
            }
            else
            {
                resources = new Resources();
                startingResources = new Resources();
            }

            turn = 1;
            era = startingEra;
            characterStates.Clear();
            flags.Clear();
            eventHistory.Clear();
            triggeredEventQueue.Clear();
            gameState = GameState.Playing;
            gameOverReason = GameOverReason.None;
            endingType = EndingType.None;
            legacyType = LegacyType.None;
            sessionStartTime = System.DateTime.Now;
        }

        /// <summary>
        /// Tur ilerlet
        /// </summary>
        public void AdvanceTurn()
        {
            turn++;
        }

        /// <summary>
        /// Event'i geçmişe ekle
        /// </summary>
        public void RecordEvent(string eventId)
        {
            eventHistory.Add(eventId);
        }

        /// <summary>
        /// Karakter durumu al veya oluştur
        /// </summary>
        public CharacterState GetOrCreateCharacterState(string characterId)
        {
            if (!characterStates.TryGetValue(characterId, out CharacterState state))
            {
                state = new CharacterState(characterId);
                characterStates[characterId] = state;
            }
            return state;
        }

        /// <summary>
        /// Flag ekle
        /// </summary>
        public void AddFlag(string flag)
        {
            flags.Add(flag);
        }

        /// <summary>
        /// Flag kaldır
        /// </summary>
        public void RemoveFlag(string flag)
        {
            flags.Remove(flag);
        }

        /// <summary>
        /// Flag kontrol et
        /// </summary>
        public bool HasFlag(string flag)
        {
            return flags.Contains(flag);
        }

        /// <summary>
        /// Tetiklenen event ekle
        /// </summary>
        public void QueueTriggeredEvent(string eventId)
        {
            triggeredEventQueue.Enqueue(eventId);
        }

        /// <summary>
        /// Tetiklenen event al
        /// </summary>
        public string DequeueTriggeredEvent()
        {
            return triggeredEventQueue.Count > 0 ? triggeredEventQueue.Dequeue() : null;
        }

        /// <summary>
        /// Tetiklenen event var mı?
        /// </summary>
        public bool HasTriggeredEvents => triggeredEventQueue.Count > 0;

        /// <summary>
        /// Event daha önce oynandı mı?
        /// </summary>
        public bool HasPlayedEvent(string eventId)
        {
            return eventHistory.Contains(eventId);
        }

        /// <summary>
        /// Oyun süresini hesapla
        /// </summary>
        public System.TimeSpan GetSessionDuration()
        {
            return System.DateTime.Now - sessionStartTime;
        }

        /// <summary>
        /// Zorluk çarpanını al (tur bazlı)
        /// </summary>
        public float GetDifficultyMultiplier()
        {
            if (turn <= Constants.EARLY_GAME_TURNS)
                return 1f;
            if (turn <= Constants.MID_GAME_TURNS)
                return 1.5f;
            if (turn <= Constants.LATE_GAME_TURNS)
                return 2f;
            return 3f;
        }

        /// <summary>
        /// Prestige puanı hesapla
        /// </summary>
        public int CalculatePrestigePoints()
        {
            // Temel puan: Hayatta kalınan tur
            int points = turn * 10;

            // Denge bonusu
            float balance = resources.GetBalanceScore();
            points += Mathf.RoundToInt(balance * 50);

            // Dönem bonusu
            points += (int)era * 25;

            // Karakter etkileşim bonusu
            points += characterStates.Count * 5;

            return points;
        }

        /// <summary>
        /// JSON'a çevir (save için)
        /// </summary>
        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }

        /// <summary>
        /// JSON'dan yükle (load için)
        /// </summary>
        public static GameStateData FromJson(string json)
        {
            return JsonUtility.FromJson<GameStateData>(json);
        }
    }

    /// <summary>
    /// Save edilebilir oyun verisi wrapper
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public GameStateData gameState;
        public PlayerStatistics statistics;
        public UnlockData unlocks;
        public int prestigePoints;
        public System.DateTime lastSaveTime;

        public SaveData()
        {
            gameState = new GameStateData();
            statistics = new PlayerStatistics();
            unlocks = new UnlockData();
            prestigePoints = 0;
            lastSaveTime = System.DateTime.Now;
        }
    }

    /// <summary>
    /// Oyuncu istatistikleri
    /// </summary>
    [System.Serializable]
    public class PlayerStatistics
    {
        public int totalCardsPlayed;
        public int totalGamesPlayed;
        public int totalDeaths;
        public int longestSurvival;
        public Dictionary<GameOverReason, int> deathCauses;
        public Dictionary<Era, int> gamesPerEra;
        public float averageGold;
        public float averageHappiness;
        public float averageMilitary;
        public float averageFaith;

        public PlayerStatistics()
        {
            totalCardsPlayed = 0;
            totalGamesPlayed = 0;
            totalDeaths = 0;
            longestSurvival = 0;
            deathCauses = new Dictionary<GameOverReason, int>();
            gamesPerEra = new Dictionary<Era, int>();
            averageGold = 50f;
            averageHappiness = 50f;
            averageMilitary = 50f;
            averageFaith = 50f;
        }

        public void RecordGameEnd(GameStateData gameState)
        {
            totalGamesPlayed++;
            totalCardsPlayed += gameState.turn;

            if (gameState.turn > longestSurvival)
                longestSurvival = gameState.turn;

            if (gameState.gameOverReason != GameOverReason.None)
            {
                totalDeaths++;
                if (!deathCauses.ContainsKey(gameState.gameOverReason))
                    deathCauses[gameState.gameOverReason] = 0;
                deathCauses[gameState.gameOverReason]++;
            }

            if (!gamesPerEra.ContainsKey(gameState.era))
                gamesPerEra[gameState.era] = 0;
            gamesPerEra[gameState.era]++;
        }
    }

    /// <summary>
    /// Unlock verileri
    /// </summary>
    [System.Serializable]
    public class UnlockData
    {
        public HashSet<Era> unlockedEras;
        public HashSet<string> unlockedScenarios;
        public HashSet<string> unlockedCharacters;
        public HashSet<string> unlockedAchievements;
        public HashSet<string> unlockedCosmetics;

        public UnlockData()
        {
            unlockedEras = new HashSet<Era> { Era.Medieval };
            unlockedScenarios = new HashSet<string> { "good_king" };
            unlockedCharacters = new HashSet<string>();
            unlockedAchievements = new HashSet<string>();
            unlockedCosmetics = new HashSet<string>();
        }

        public bool IsEraUnlocked(Era era)
        {
            return unlockedEras.Contains(era);
        }

        public void UnlockEra(Era era)
        {
            unlockedEras.Add(era);
        }
    }
}
