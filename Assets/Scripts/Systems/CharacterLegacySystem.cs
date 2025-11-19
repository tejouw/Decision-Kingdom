using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Cross-Era karakter hafızası sistemi
    /// Karakterlerin torunları sonraki dönemlerde ortaya çıkar ve atalarını hatırlar
    /// GAME_DESIGN.md: "Rönesans'ta bilim adamını koruduysan, Sanayi Devriminde onun torunu mühendis olarak geliyor ve seni hatırlıyor."
    /// </summary>
    public class CharacterLegacySystem : MonoBehaviour
    {
        public static CharacterLegacySystem Instance { get; private set; }

        [Header("Legacy Data")]
        [SerializeField] private CharacterLegacyData _legacyData;

        // Events
        public event Action<string, string> OnDescendantSpawned; // ancestorId, descendantId
        public event Action<CharacterLegacy> OnLegacyCreated;

        #region Character Lineages
        /// <summary>
        /// Karakter soy ağaçları - hangi karakter hangi dönemde kimin torunu
        /// </summary>
        public static readonly Dictionary<string, CharacterLineage> CharacterLineages = new Dictionary<string, CharacterLineage>
        {
            // ============ ORTAÇAĞ → RÖNESANS ============

            // Marcus (Danışman) -> Lorenzo (Bankacı)
            ["marcus_lineage"] = new CharacterLineage
            {
                ancestorId = "marcus",
                ancestorEra = Era.Medieval,
                descendantId = "marcus_grandson_banker",
                descendantName = "Alessandro di Marco",
                descendantTitle = "Bankacı",
                descendantEra = Era.Renaissance,
                relationDescription = "Danışman Marcus'un torunu",
                positiveMemory = "Büyükbabam sizin sadık hizmetkarınızdı. Aile olarak size minnetarız.",
                negativeMemory = "Büyükbabamı reddettiniz. Ama ben size hizmet etmeye hazırım... bir bedelle.",
                neutralMemory = "Marcus ailesinden geliyorum. Büyükbabam sizden çok bahsederdi.",
                requiredRelationship = 30,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Gold, 10 },
                    { ResourceType.Happiness, 5 }
                }
            },

            // Valerius (General) -> Condottiero
            ["valerius_lineage"] = new CharacterLineage
            {
                ancestorId = "valerius",
                ancestorEra = Era.Medieval,
                descendantId = "valerius_grandson_condottiero",
                descendantName = "Giovanni Valerio",
                descendantTitle = "Kondotyer",
                descendantEra = Era.Renaissance,
                relationDescription = "General Valerius'un torunu",
                positiveMemory = "Büyükbabam sizin en büyük generalinizdi. Ailemiz savaş sanatını nesilden nesile aktardı.",
                negativeMemory = "Büyükbabamı ihmal ettiniz. Ama ben size gücümü kanıtlayacağım.",
                neutralMemory = "Valerius soyundan geliyorum. Askeri geleneklerimiz güçlü.",
                requiredRelationship = 20,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Military, 10 },
                    { ResourceType.Gold, -5 }
                }
            },

            // Miriam (Tüccar) -> Medici tipi tüccar ailesi
            ["miriam_lineage"] = new CharacterLineage
            {
                ancestorId = "miriam",
                ancestorEra = Era.Medieval,
                descendantId = "miriam_granddaughter_merchant",
                descendantName = "Isabella Miriami",
                descendantTitle = "Ticaret Prensesi",
                descendantEra = Era.Renaissance,
                relationDescription = "Tüccar Miriam'ın torunu",
                positiveMemory = "Büyükannem sizinle çalışarak servetini kurdu. Şimdi en zengin aile biziz!",
                negativeMemory = "Büyükannemi hapse attırdınız. Ama ailemiz yine de yükseldi. Şimdi intikam zamanı mı?",
                neutralMemory = "Miriam ailesinin varisi olarak size bir teklif getirdim.",
                requiredRelationship = 40,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Gold, 15 },
                    { ResourceType.Faith, -5 }
                }
            },

            // ============ RÖNESANS → SANAYİ DEVRİMİ ============

            // Galileo (Bilim Adamı) -> Mucit
            ["galileo_lineage"] = new CharacterLineage
            {
                ancestorId = "galileo",
                ancestorEra = Era.Renaissance,
                descendantId = "galileo_grandson_inventor",
                descendantName = "Thomas Galilei",
                descendantTitle = "Mucit",
                descendantEra = Era.Industrial,
                relationDescription = "Galileo'nun torunu",
                positiveMemory = "Büyükbabam sizin sayenizde çalışmalarını sürdürdü. Ben de onun icatlarını geliştirdim!",
                negativeMemory = "Büyükbabamı engizisyona teslim ettiniz. Ama fikirleri yaşıyor ve ben onları gerçekleştireceğim.",
                neutralMemory = "Galileo ailesinin mirasçısıyım. Bilim için ne yaparsınız?",
                requiredRelationship = 35,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Gold, 10 },
                    { ResourceType.Military, 5 }
                }
            },

            // Leonardo (Sanatçı) -> Mühendis
            ["leonardo_lineage"] = new CharacterLineage
            {
                ancestorId = "leonardo",
                ancestorEra = Era.Renaissance,
                descendantId = "leonardo_grandson_engineer",
                descendantName = "Marco da Vinci",
                descendantTitle = "Mühendis",
                descendantEra = Era.Industrial,
                relationDescription = "Leonardo'nun torunu",
                positiveMemory = "Büyükbabamın çizimleri gerçek oldu! Sizin desteğiniz olmasaydı bunlar hayal kalırdı.",
                negativeMemory = "Büyükbabamı deli diye kovdunuz. Ama ben onun vizyonunu gerçekleştirdim.",
                neutralMemory = "Da Vinci ailesinden geliyorum. Büyükbabamın projelerini tamamlamaya geldim.",
                requiredRelationship = 30,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Gold, 5 },
                    { ResourceType.Military, 10 }
                }
            },

            // ============ SANAYİ DEVRİMİ → MODERN ============

            // İşçi Lideri -> Sendika Başkanı
            ["worker_leader_lineage"] = new CharacterLineage
            {
                ancestorId = "worker_leader",
                ancestorEra = Era.Industrial,
                descendantId = "worker_grandson_union",
                descendantName = "James Morrison",
                descendantTitle = "Sendika Başkanı",
                descendantEra = Era.Modern,
                relationDescription = "İşçi lideri Morrison'un torunu",
                positiveMemory = "Büyükbabam işçi hakları için savaştı ve siz onu desteklediniz. Ailemiz size minnettar.",
                negativeMemory = "Büyükbabamı hapse attırdınız ama hareketi durduramadınız. Şimdi biz daha güçlüyüz.",
                neutralMemory = "Morrison ailesinin lideri olarak işçilerin sesini duyurmaya geldim.",
                requiredRelationship = 25,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Happiness, 10 },
                    { ResourceType.Gold, -5 }
                }
            },

            // Fabrikatör -> CEO
            ["industrialist_lineage"] = new CharacterLineage
            {
                ancestorId = "industrialist",
                ancestorEra = Era.Industrial,
                descendantId = "industrialist_grandson_ceo",
                descendantName = "Robert Carnegie",
                descendantTitle = "Şirket CEO'su",
                descendantEra = Era.Modern,
                relationDescription = "Büyük sanayicinin torunu",
                positiveMemory = "Büyükbabam sizin sayenizde imparatorluğunu kurdu. Şimdi küresel bir deviz!",
                negativeMemory = "Büyükbabamı çökertmeye çalıştınız ama başaramadınız. Şimdi biz daha güçlüyüz.",
                neutralMemory = "Carnegie ailesinin varisi olarak bir ortaklık teklif ediyorum.",
                requiredRelationship = 40,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Gold, 20 },
                    { ResourceType.Happiness, -10 }
                }
            },

            // ============ MODERN → GELECEK ============

            // Bilim Adamı -> AI Araştırmacısı
            ["scientist_lineage"] = new CharacterLineage
            {
                ancestorId = "scientist",
                ancestorEra = Era.Modern,
                descendantId = "scientist_grandson_ai",
                descendantName = "Dr. Elena Turing",
                descendantTitle = "AI Araştırmacısı",
                descendantEra = Era.Future,
                relationDescription = "Dr. Oppenheimer'ın torunu",
                positiveMemory = "Büyükbabam atom çağını başlattı, şimdi ben AI çağını başlatıyorum. Sizin desteğinizle tabii.",
                negativeMemory = "Büyükbabamın vicdanı onu yedi. Ama ben daha kararlıyım. AI'yı durdurmayacağım.",
                neutralMemory = "Turing ailesinden Dr. Elena. Yapay zeka hakkında konuşmalıyız.",
                requiredRelationship = 30,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Military, 15 },
                    { ResourceType.Faith, -10 }
                }
            },

            // Diplomat -> Dünya Lideri
            ["diplomat_lineage"] = new CharacterLineage
            {
                ancestorId = "diplomat",
                ancestorEra = Era.Modern,
                descendantId = "diplomat_grandson_world_leader",
                descendantName = "Li Wei Chen",
                descendantTitle = "Dünya Konseyi Üyesi",
                descendantEra = Era.Future,
                relationDescription = "Ambassador Chen'in torunu",
                positiveMemory = "Büyükbabam barışı korudu, ben de onun mirasını sürdürüyorum. Birlikte çalışalım.",
                negativeMemory = "Büyükbabamı dinlemediniz ve savaş çıktı. Şimdi beni dinleyecek misiniz?",
                neutralMemory = "Chen ailesinin temsilcisi olarak küresel sorunları konuşmaya geldim.",
                requiredRelationship = 25,
                bonusResources = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Happiness, 10 },
                    { ResourceType.Faith, 10 }
                }
            }
        };
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
            if (_legacyData == null)
            {
                _legacyData = new CharacterLegacyData();
            }

            LoadLegacyData();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Ata ile etkileşimi kaydet (dönem bitiminde çağrılır)
        /// </summary>
        public void RecordAncestorInteraction(string characterId, int relationshipLevel, Era era, List<string> flags)
        {
            var legacy = new CharacterLegacy
            {
                ancestorId = characterId,
                era = era,
                relationshipLevel = relationshipLevel,
                interactionFlags = new List<string>(flags),
                timestamp = DateTime.Now.ToString()
            };

            // Mevcut kaydı güncelle veya yeni ekle
            int existingIndex = _legacyData.legacies.FindIndex(l => l.ancestorId == characterId && l.era == era);
            if (existingIndex >= 0)
            {
                _legacyData.legacies[existingIndex] = legacy;
            }
            else
            {
                _legacyData.legacies.Add(legacy);
            }

            OnLegacyCreated?.Invoke(legacy);
            SaveLegacyData();

            Debug.Log($"[CharacterLegacySystem] Legacy recorded: {characterId} in {era} with relationship {relationshipLevel}");
        }

        /// <summary>
        /// Sonraki dönem için torun karakterlerini al
        /// </summary>
        public List<DescendantCharacter> GetDescendantsForEra(Era era)
        {
            var descendants = new List<DescendantCharacter>();

            foreach (var lineage in CharacterLineages.Values)
            {
                if (lineage.descendantEra != era)
                    continue;

                // Ata ile etkileşim olmuş mu kontrol et
                var ancestorLegacy = _legacyData.legacies.Find(l => l.ancestorId == lineage.ancestorId);
                if (ancestorLegacy == null)
                    continue;

                // Torun karakteri oluştur
                var descendant = new DescendantCharacter
                {
                    lineage = lineage,
                    ancestorRelationship = ancestorLegacy.relationshipLevel,
                    ancestorFlags = ancestorLegacy.interactionFlags,
                    memoryType = GetMemoryType(ancestorLegacy.relationshipLevel, lineage.requiredRelationship)
                };

                descendants.Add(descendant);
                OnDescendantSpawned?.Invoke(lineage.ancestorId, lineage.descendantId);

                Debug.Log($"[CharacterLegacySystem] Descendant spawned: {lineage.descendantName} ({descendant.memoryType})");
            }

            return descendants;
        }

        /// <summary>
        /// Torun için event oluştur
        /// </summary>
        public GameEvent CreateDescendantEvent(DescendantCharacter descendant)
        {
            var lineage = descendant.lineage;
            string memory = GetMemoryText(descendant);

            var character = new Character(lineage.descendantId, lineage.descendantName, lineage.descendantTitle)
            {
                eras = new List<Era> { lineage.descendantEra },
                description = lineage.relationDescription
            };

            var eventText = $"{lineage.descendantName} huzurunuzda. '{memory}'";

            // Efektler ilişkiye göre değişir
            var leftEffects = new List<ResourceEffect>();
            var rightEffects = new List<ResourceEffect>();

            switch (descendant.memoryType)
            {
                case MemoryType.Positive:
                    // Olumlu hatıra - kabul etmek bonus verir
                    foreach (var bonus in lineage.bonusResources)
                    {
                        rightEffects.Add(new ResourceEffect { resource = bonus.Key, min = bonus.Value, max = bonus.Value });
                    }
                    leftEffects.Add(new ResourceEffect { resource = ResourceType.Happiness, min = -5, max = -5 });
                    break;

                case MemoryType.Negative:
                    // Olumsuz hatıra - dikkatli olmak lazım
                    rightEffects.Add(new ResourceEffect { resource = ResourceType.Gold, min = -10, max = 15 });
                    rightEffects.Add(new ResourceEffect { resource = ResourceType.Happiness, min = -5, max = 10 });
                    leftEffects.Add(new ResourceEffect { resource = ResourceType.Happiness, min = 5, max = 5 });
                    break;

                case MemoryType.Neutral:
                    // Nötr hatıra - dengeli efektler
                    rightEffects.Add(new ResourceEffect { resource = ResourceType.Gold, min = 5, max = 5 });
                    rightEffects.Add(new ResourceEffect { resource = ResourceType.Happiness, min = 5, max = 5 });
                    leftEffects.Add(new ResourceEffect { resource = ResourceType.Gold, min = -5, max = -5 });
                    break;
            }

            var leftChoice = new Choice("Reddediyorum");
            foreach (var effect in leftEffects)
            {
                leftChoice.AddEffect(effect.resource, effect.min, effect.max);
            }

            var rightChoice = new Choice("Kabul ediyorum");
            foreach (var effect in rightEffects)
            {
                rightChoice.AddEffect(effect.resource, effect.min, effect.max);
            }
            rightChoice.AddFlag($"accepted_{lineage.descendantId}");

            return new GameEvent
            {
                id = $"descendant_{lineage.descendantId}",
                era = lineage.descendantEra,
                category = EventCategory.Character,
                character = character,
                text = eventText,
                leftChoice = leftChoice,
                rightChoice = rightChoice,
                weight = 3f, // Torun eventleri önemli
                description = $"Torun event: {lineage.descendantName}",
                priority = 5,
                isRare = false,
                conditions = new List<Condition>()
            };
        }

        /// <summary>
        /// Belirli bir ata için torun var mı kontrol et
        /// </summary>
        public bool HasDescendant(string ancestorId, Era inEra)
        {
            foreach (var lineage in CharacterLineages.Values)
            {
                if (lineage.ancestorId == ancestorId && lineage.descendantEra == inEra)
                {
                    return _legacyData.legacies.Exists(l => l.ancestorId == ancestorId);
                }
            }
            return false;
        }

        /// <summary>
        /// Tüm legacy kayıtlarını temizle
        /// </summary>
        public void ClearAllLegacies()
        {
            _legacyData.legacies.Clear();
            SaveLegacyData();
            Debug.Log("[CharacterLegacySystem] All legacies cleared");
        }
        #endregion

        #region Private Methods
        private MemoryType GetMemoryType(int relationshipLevel, int requiredRelationship)
        {
            if (relationshipLevel >= requiredRelationship)
                return MemoryType.Positive;
            else if (relationshipLevel <= -requiredRelationship)
                return MemoryType.Negative;
            else
                return MemoryType.Neutral;
        }

        private string GetMemoryText(DescendantCharacter descendant)
        {
            return descendant.memoryType switch
            {
                MemoryType.Positive => descendant.lineage.positiveMemory,
                MemoryType.Negative => descendant.lineage.negativeMemory,
                _ => descendant.lineage.neutralMemory
            };
        }

        private void SaveLegacyData()
        {
            string json = JsonUtility.ToJson(_legacyData);
            PlayerPrefs.SetString("CharacterLegacyData", json);
            PlayerPrefs.Save();
        }

        private void LoadLegacyData()
        {
            string json = PlayerPrefs.GetString("CharacterLegacyData", "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    _legacyData = JsonUtility.FromJson<CharacterLegacyData>(json);
                }
                catch
                {
                    _legacyData = new CharacterLegacyData();
                }
            }
            else
            {
                _legacyData = new CharacterLegacyData();
            }

            Debug.Log($"[CharacterLegacySystem] Loaded {_legacyData.legacies.Count} legacies");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Print All Legacies")]
        private void DebugPrintLegacies()
        {
            foreach (var legacy in _legacyData.legacies)
            {
                Debug.Log($"Legacy: {legacy.ancestorId} in {legacy.era}, relationship: {legacy.relationshipLevel}");
            }
        }

        [ContextMenu("Add Test Legacy - Marcus Positive")]
        private void DebugAddMarcusLegacy()
        {
            RecordAncestorInteraction("marcus", 50, Era.Medieval, new List<string> { "marcus_trusted" });
        }

        [ContextMenu("Add Test Legacy - Galileo Negative")]
        private void DebugAddGalileoLegacy()
        {
            RecordAncestorInteraction("galileo", -40, Era.Renaissance, new List<string> { "galileo_arrested" });
        }

        [ContextMenu("Clear All Legacies")]
        private void DebugClearLegacies()
        {
            ClearAllLegacies();
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Karakter soy ağacı bilgisi
    /// </summary>
    [System.Serializable]
    public class CharacterLineage
    {
        public string ancestorId;
        public Era ancestorEra;
        public string descendantId;
        public string descendantName;
        public string descendantTitle;
        public Era descendantEra;
        public string relationDescription;
        public string positiveMemory;
        public string negativeMemory;
        public string neutralMemory;
        public int requiredRelationship;
        public Dictionary<ResourceType, int> bonusResources;
    }

    /// <summary>
    /// Karakter mirası kaydı
    /// </summary>
    [System.Serializable]
    public class CharacterLegacy
    {
        public string ancestorId;
        public Era era;
        public int relationshipLevel;
        public List<string> interactionFlags;
        public string timestamp;
    }

    /// <summary>
    /// Torun karakter bilgisi
    /// </summary>
    public class DescendantCharacter
    {
        public CharacterLineage lineage;
        public int ancestorRelationship;
        public List<string> ancestorFlags;
        public MemoryType memoryType;
    }

    /// <summary>
    /// Hatıra türü
    /// </summary>
    public enum MemoryType
    {
        Positive,   // Ata ile iyi ilişki
        Negative,   // Ata ile kötü ilişki
        Neutral     // Nötr ilişki
    }

    /// <summary>
    /// Legacy kayıt verileri
    /// </summary>
    [System.Serializable]
    public class CharacterLegacyData
    {
        public List<CharacterLegacy> legacies = new List<CharacterLegacy>();
    }
    #endregion
}
