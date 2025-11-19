using System.Collections.Generic;
using System.Linq;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Kararların gelecek nesillere etkisini yöneten sistem
    /// </summary>
    public static class LegacySystem
    {
        #region Legacy Data
        /// <summary>
        /// Miras veri yapısı
        /// </summary>
        [System.Serializable]
        public class LegacyData
        {
            public LegacyType type;
            public string title;
            public string description;
            public Dictionary<ResourceType, int> startingBonuses;
            public List<string> unlockedFlags;
            public int minimumTurns;

            public LegacyData()
            {
                startingBonuses = new Dictionary<ResourceType, int>();
                unlockedFlags = new List<string>();
            }
        }

        private static Dictionary<LegacyType, LegacyData> _legacies;

        static LegacySystem()
        {
            InitializeLegacies();
        }

        private static void InitializeLegacies()
        {
            _legacies = new Dictionary<LegacyType, LegacyData>
            {
                // Olumlu Miraslar
                [LegacyType.WisdomLegacy] = new LegacyData
                {
                    type = LegacyType.WisdomLegacy,
                    title = "Bilgelik Mirası",
                    description = "Önceki kralın bilgeliği nesiller boyu aktarıldı. Danışmanlar daha sadık.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Gold, 5 },
                        { ResourceType.Faith, 5 }
                    },
                    unlockedFlags = new List<string> { "wisdom_heir", "advisor_trust" },
                    minimumTurns = 30
                },
                [LegacyType.WealthLegacy] = new LegacyData
                {
                    type = LegacyType.WealthLegacy,
                    title = "Zenginlik Mirası",
                    description = "Önceki kralın hazinesi miras kaldı. Başlangıç altını yüksek.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Gold, 15 }
                    },
                    unlockedFlags = new List<string> { "wealthy_heir", "merchant_favor" },
                    minimumTurns = 25
                },
                [LegacyType.MilitaryLegacy] = new LegacyData
                {
                    type = LegacyType.MilitaryLegacy,
                    title = "Askeri Miras",
                    description = "Önceki kralın ordusu güçlü kaldı. Askeri güç yüksek başlıyor.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Military, 15 }
                    },
                    unlockedFlags = new List<string> { "military_heir", "general_loyalty" },
                    minimumTurns = 25
                },
                [LegacyType.FaithLegacy] = new LegacyData
                {
                    type = LegacyType.FaithLegacy,
                    title = "İnanç Mirası",
                    description = "Önceki kralın dindarlığı devam ediyor. Kilise desteği güçlü.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Faith, 15 }
                    },
                    unlockedFlags = new List<string> { "pious_heir", "church_favor" },
                    minimumTurns = 25
                },
                [LegacyType.DiplomacyLegacy] = new LegacyData
                {
                    type = LegacyType.DiplomacyLegacy,
                    title = "Diplomasi Mirası",
                    description = "Önceki kralın ittifakları güçlü. Halk mutlu başlıyor.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Happiness, 10 },
                        { ResourceType.Gold, 5 }
                    },
                    unlockedFlags = new List<string> { "diplomatic_heir", "alliance_ready" },
                    minimumTurns = 30
                },

                // Olumsuz Miraslar
                [LegacyType.TyrantLegacy] = new LegacyData
                {
                    type = LegacyType.TyrantLegacy,
                    title = "Tiran Mirası",
                    description = "Önceki kral zalimdi. Halk yeni krala güvenmiyor.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Happiness, -15 },
                        { ResourceType.Military, 10 }
                    },
                    unlockedFlags = new List<string> { "tyrant_heir", "feared_ruler" },
                    minimumTurns = 20
                },
                [LegacyType.WeakLegacy] = new LegacyData
                {
                    type = LegacyType.WeakLegacy,
                    title = "Zayıf Kral Mirası",
                    description = "Önceki kral zayıftı. Düşmanlar fırsat kolluyor.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Military, -10 },
                        { ResourceType.Gold, -5 }
                    },
                    unlockedFlags = new List<string> { "weak_heir", "vulnerable_kingdom" },
                    minimumTurns = 15
                },
                [LegacyType.HereticalLegacy] = new LegacyData
                {
                    type = LegacyType.HereticalLegacy,
                    title = "Sapkın Miras",
                    description = "Önceki kral sapkındı. Kilise yeni krala şüpheyle bakıyor.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Faith, -15 }
                    },
                    unlockedFlags = new List<string> { "heretical_heir", "church_suspicion" },
                    minimumTurns = 20
                },
                [LegacyType.CruelLegacy] = new LegacyData
                {
                    type = LegacyType.CruelLegacy,
                    title = "Zalim Miras",
                    description = "Önceki kral zalimdi. Halk intikam arıyor.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Happiness, -10 },
                        { ResourceType.Faith, -5 }
                    },
                    unlockedFlags = new List<string> { "cruel_heir", "revenge_seekers" },
                    minimumTurns = 20
                },
                [LegacyType.GreedyLegacy] = new LegacyData
                {
                    type = LegacyType.GreedyLegacy,
                    title = "Açgözlü Miras",
                    description = "Önceki kral hazineyi boşalttı. Ekonomi çökmüş durumda.",
                    startingBonuses = new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Gold, -15 },
                        { ResourceType.Happiness, -5 }
                    },
                    unlockedFlags = new List<string> { "greedy_heir", "empty_treasury" },
                    minimumTurns = 15
                }
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Miras tipine göre legacy verisini al
        /// </summary>
        public static LegacyData GetLegacy(LegacyType type)
        {
            return _legacies.TryGetValue(type, out var legacy) ? legacy : null;
        }

        /// <summary>
        /// Oyun durumuna göre miras tipini belirle
        /// </summary>
        public static LegacyType DetermineLegacy(GameStateData gameState)
        {
            // Minimum tur kontrolü
            if (gameState.turn < 15)
                return LegacyType.None;

            // Flag bazlı miraslar
            var flagLegacy = CheckFlagBasedLegacy(gameState);
            if (flagLegacy != LegacyType.None)
                return flagLegacy;

            // Kaynak bazlı miraslar
            return DetermineResourceBasedLegacy(gameState);
        }

        /// <summary>
        /// Mirası uygula (yeni oyun için)
        /// </summary>
        public static Resources ApplyLegacyBonuses(LegacyType legacyType, Resources baseResources)
        {
            var legacy = GetLegacy(legacyType);
            if (legacy == null)
                return baseResources;

            var newResources = new Resources(baseResources);

            foreach (var bonus in legacy.startingBonuses)
            {
                switch (bonus.Key)
                {
                    case ResourceType.Gold:
                        newResources.Gold = UnityEngine.Mathf.Clamp(newResources.Gold + bonus.Value, 0, 100);
                        break;
                    case ResourceType.Happiness:
                        newResources.Happiness = UnityEngine.Mathf.Clamp(newResources.Happiness + bonus.Value, 0, 100);
                        break;
                    case ResourceType.Military:
                        newResources.Military = UnityEngine.Mathf.Clamp(newResources.Military + bonus.Value, 0, 100);
                        break;
                    case ResourceType.Faith:
                        newResources.Faith = UnityEngine.Mathf.Clamp(newResources.Faith + bonus.Value, 0, 100);
                        break;
                }
            }

            return newResources;
        }

        /// <summary>
        /// Miras flag'lerini gamestate'e ekle
        /// </summary>
        public static void ApplyLegacyFlags(LegacyType legacyType, GameStateData gameState)
        {
            var legacy = GetLegacy(legacyType);
            if (legacy == null)
                return;

            foreach (var flag in legacy.unlockedFlags)
            {
                gameState.AddFlag(flag);
            }
        }

        /// <summary>
        /// Tüm mirasları listele
        /// </summary>
        public static List<LegacyData> GetAllLegacies()
        {
            return _legacies.Values.ToList();
        }

        /// <summary>
        /// Olumlu miras mı kontrol et
        /// </summary>
        public static bool IsPositiveLegacy(LegacyType type)
        {
            return type == LegacyType.WisdomLegacy ||
                   type == LegacyType.WealthLegacy ||
                   type == LegacyType.MilitaryLegacy ||
                   type == LegacyType.FaithLegacy ||
                   type == LegacyType.DiplomacyLegacy;
        }
        #endregion

        #region Private Methods
        private static LegacyType CheckFlagBasedLegacy(GameStateData gameState)
        {
            // Özel flag kombinasyonları
            if (gameState.HasFlag("tyrant_ruler") || gameState.HasFlag("mass_executions"))
                return LegacyType.TyrantLegacy;

            if (gameState.HasFlag("heretic_king") || gameState.HasFlag("church_enemy"))
                return LegacyType.HereticalLegacy;

            if (gameState.HasFlag("cruel_punishments") || gameState.HasFlag("torture_used"))
                return LegacyType.CruelLegacy;

            if (gameState.HasFlag("excessive_taxes") || gameState.HasFlag("treasury_emptied"))
                return LegacyType.GreedyLegacy;

            // Olumlu flag'ler
            if (gameState.HasFlag("wise_decisions") && gameState.turn >= 30)
                return LegacyType.WisdomLegacy;

            if (gameState.HasFlag("diplomatic_success") && gameState.turn >= 30)
                return LegacyType.DiplomacyLegacy;

            return LegacyType.None;
        }

        private static LegacyType DetermineResourceBasedLegacy(GameStateData gameState)
        {
            var res = gameState.resources;

            // Dominant kaynak analizi
            float maxResource = UnityEngine.Mathf.Max(res.Gold, res.Happiness, res.Military, res.Faith);
            float minResource = UnityEngine.Mathf.Min(res.Gold, res.Happiness, res.Military, res.Faith);

            // Çok düşük kaynaklar = Olumsuz miras
            if (minResource < 20)
            {
                if (res.Gold < 20)
                    return LegacyType.GreedyLegacy;
                if (res.Happiness < 20)
                    return LegacyType.CruelLegacy;
                if (res.Military < 20)
                    return LegacyType.WeakLegacy;
                if (res.Faith < 20)
                    return LegacyType.HereticalLegacy;
            }

            // Yüksek kaynaklar = Olumlu miras
            if (maxResource >= 70)
            {
                if (res.Gold >= 70)
                    return LegacyType.WealthLegacy;
                if (res.Military >= 70)
                    return LegacyType.MilitaryLegacy;
                if (res.Faith >= 70)
                    return LegacyType.FaithLegacy;
                if (res.Happiness >= 70)
                    return LegacyType.DiplomacyLegacy;
            }

            // Dengeli = Bilgelik
            if (res.Gold >= 40 && res.Happiness >= 40 && res.Military >= 40 && res.Faith >= 40)
                return LegacyType.WisdomLegacy;

            return LegacyType.None;
        }
        #endregion
    }

    /// <summary>
    /// Oyun sonunda hesaplanan miras özeti
    /// </summary>
    [System.Serializable]
    public class LegacySummary
    {
        public LegacyType legacyType;
        public EndingType endingType;
        public int finalTurn;
        public int prestigeEarned;
        public List<string> achievements;
        public Dictionary<string, int> characterRelationships;

        public LegacySummary()
        {
            achievements = new List<string>();
            characterRelationships = new Dictionary<string, int>();
        }

        /// <summary>
        /// Oyun durumundan miras özeti oluştur
        /// </summary>
        public static LegacySummary CreateFromGameState(GameStateData gameState)
        {
            var summary = new LegacySummary
            {
                legacyType = LegacySystem.DetermineLegacy(gameState),
                endingType = EndingSystem.DetermineEnding(gameState),
                finalTurn = gameState.turn,
                prestigeEarned = gameState.CalculatePrestigePoints()
            };

            // Karakter ilişkilerini kaydet
            foreach (var charState in gameState.characterStates)
            {
                summary.characterRelationships[charState.Key] = charState.Value.relationshipLevel;
            }

            // Başarıları belirle
            DetermineAchievements(summary, gameState);

            return summary;
        }

        private static void DetermineAchievements(LegacySummary summary, GameStateData gameState)
        {
            // Tur bazlı başarılar
            if (gameState.turn >= 25)
                summary.achievements.Add("Hayatta Kalan");
            if (gameState.turn >= 50)
                summary.achievements.Add("Deneyimli Hükümdar");
            if (gameState.turn >= 100)
                summary.achievements.Add("Efsanevi Kral");

            // Kaynak bazlı başarılar
            if (gameState.resources.Gold >= 80)
                summary.achievements.Add("Zengin Kral");
            if (gameState.resources.Military >= 80)
                summary.achievements.Add("Askeri Deha");
            if (gameState.resources.Faith >= 80)
                summary.achievements.Add("Dindar Hükümdar");
            if (gameState.resources.Happiness >= 80)
                summary.achievements.Add("Sevilen Kral");

            // Özel başarılar
            if (gameState.HasFlag("dragon_slayer"))
                summary.achievements.Add("Ejderha Avcısı");
            if (gameState.HasFlag("grail_found"))
                summary.achievements.Add("Kase Koruyucusu");
            if (gameState.HasFlag("crusade_joined"))
                summary.achievements.Add("Haçlı Şövalye");

            // Karakter bazlı başarılar
            if (gameState.characterStates.Count >= 6)
                summary.achievements.Add("Sosyal Kral");
        }
    }
}
