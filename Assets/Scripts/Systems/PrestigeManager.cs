using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Prestige Points ve Unlock sistemini yoneten sinif
    /// Faz 4: Meta Sistemler
    /// </summary>
    public class PrestigeManager : MonoBehaviour
    {
        public static PrestigeManager Instance { get; private set; }

        [Header("Prestige Data")]
        [SerializeField] private int _totalPrestigePoints;
        [SerializeField] private UnlockData _unlockData;

        // Events
        public event Action<int> OnPrestigePointsChanged;
        public event Action<Era> OnEraUnlocked;
        public event Action<string> OnScenarioUnlocked;
        public event Action<string> OnAchievementUnlocked;

        #region Properties
        public int TotalPrestigePoints => _totalPrestigePoints;
        public UnlockData Unlocks => _unlockData;
        #endregion

        #region Era Unlock Costs
        public static readonly Dictionary<Era, int> EraUnlockCosts = new Dictionary<Era, int>
        {
            { Era.Medieval, 0 },        // Baslangicta acik
            { Era.Renaissance, 100 },   // 100 PP
            { Era.Industrial, 250 },    // 250 PP
            { Era.Modern, 500 },        // 500 PP
            { Era.Future, 1000 }        // 1000 PP
        };

        public static readonly Dictionary<Era, string> EraNames = new Dictionary<Era, string>
        {
            { Era.Medieval, "Ortacag" },
            { Era.Renaissance, "Ronesans" },
            { Era.Industrial, "Sanayi Devrimi" },
            { Era.Modern, "Modern Donem" },
            { Era.Future, "Gelecek" }
        };
        #endregion

        #region Scenario Data
        public static readonly Dictionary<string, ScenarioInfo> Scenarios = new Dictionary<string, ScenarioInfo>
        {
            ["good_king"] = new ScenarioInfo
            {
                id = "good_king",
                name = "Iyi Kral",
                description = "Dengeli baslangic. Tum kaynaklar 50.",
                unlockCost = 0,
                startingResources = new Resources(50, 50, 50, 50)
            },
            ["young_heir"] = new ScenarioInfo
            {
                id = "young_heir",
                name = "Genc Varis",
                description = "Genc ve tecrubesiz. Mutluluk yuksek ama askeri guc dusuk.",
                unlockCost = 50,
                startingResources = new Resources(40, 70, 30, 50)
            },
            ["coup_leader"] = new ScenarioInfo
            {
                id = "coup_leader",
                name = "Darbe Lideri",
                description = "Askeri darbe ile tahta gectin. Guc yuksek ama halk mutsuz.",
                unlockCost = 75,
                startingResources = new Resources(50, 30, 80, 40)
            },
            ["rich_merchant"] = new ScenarioInfo
            {
                id = "rich_merchant",
                name = "Zengin Tuccar",
                description = "Tuccarlarin destegi ile kral oldun. Hazine dolu ama ordu zayif.",
                unlockCost = 100,
                startingResources = new Resources(80, 50, 30, 40)
            },
            ["peoples_choice"] = new ScenarioInfo
            {
                id = "peoples_choice",
                name = "Halkin Sevgilisi",
                description = "Halk seni secti. Cok sevilen ama fakir bir kral.",
                unlockCost = 100,
                startingResources = new Resources(30, 80, 40, 50)
            },
            ["holy_ruler"] = new ScenarioInfo
            {
                id = "holy_ruler",
                name = "Kutsal Hukumdar",
                description = "Kilise tarafindan kutsandin. Inanc cok yuksek.",
                unlockCost = 125,
                startingResources = new Resources(40, 50, 40, 80)
            },
            ["usurper"] = new ScenarioInfo
            {
                id = "usurper",
                name = "Gasipci",
                description = "Tahti ele gecirdin. Herkes senden supheleniyor.",
                unlockCost = 150,
                startingResources = new Resources(60, 25, 60, 25)
            },
            ["mad_king"] = new ScenarioInfo
            {
                id = "mad_king",
                name = "Deli Kral",
                description = "Deli oldugunu dusunuyorlar. Dengeler bozuk.",
                unlockCost = 200,
                startingResources = new Resources(70, 30, 70, 30)
            },
            ["legendary_start"] = new ScenarioInfo
            {
                id = "legendary_start",
                name = "Efsanevi Baslangic",
                description = "Tum kaynaklar yuksek! Ama ne kadar surdurebilirsin?",
                unlockCost = 500,
                startingResources = new Resources(70, 70, 70, 70)
            },
            ["nightmare_mode"] = new ScenarioInfo
            {
                id = "nightmare_mode",
                name = "Kabus Modu",
                description = "Tum kaynaklar dusuk. Gercek bir zorluk ariyorsan!",
                unlockCost = 300,
                startingResources = new Resources(30, 30, 30, 30)
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
            if (_unlockData == null)
            {
                _unlockData = new UnlockData();
            }

            LoadPrestigeData();
        }
        #endregion

        #region Public Methods - Prestige Points
        /// <summary>
        /// Prestige puani ekle
        /// </summary>
        public void AddPrestigePoints(int points)
        {
            if (points <= 0) return;

            _totalPrestigePoints += points;
            OnPrestigePointsChanged?.Invoke(_totalPrestigePoints);

            Debug.Log($"[PrestigeManager] +{points} PP. Toplam: {_totalPrestigePoints}");

            // Otomatik unlock kontrolu
            CheckAutoUnlocks();

            SavePrestigeData();
        }

        /// <summary>
        /// Prestige puani harca
        /// </summary>
        public bool SpendPrestigePoints(int points)
        {
            if (points <= 0 || _totalPrestigePoints < points)
                return false;

            _totalPrestigePoints -= points;
            OnPrestigePointsChanged?.Invoke(_totalPrestigePoints);

            Debug.Log($"[PrestigeManager] -{points} PP. Kalan: {_totalPrestigePoints}");

            SavePrestigeData();
            return true;
        }

        /// <summary>
        /// Oyun sonu prestige hesapla
        /// </summary>
        public int CalculateGameEndPrestige(GameStateData gameState, EndingType endingType)
        {
            int points = 0;

            // Temel puan: Hayatta kalinan tur
            points += gameState.turn * 2;

            // Denge bonusu (40-60 arasi)
            float balanceScore = gameState.resources.GetBalanceScore();
            points += Mathf.RoundToInt(balanceScore * 30);

            // Ending bonusu
            int endingBonus = EndingSystem.GetPrestigeBonus(endingType);
            points += endingBonus;

            // Donem bonusu
            points += (int)gameState.era * 10;

            // Karakter etkilesim bonusu
            foreach (var charState in gameState.characterStates.Values)
            {
                if (charState.interactionCount >= 5)
                    points += 5;
                if (charState.relationshipLevel >= 50)
                    points += 10;
                else if (charState.relationshipLevel <= -50)
                    points += 5; // Dusmanlar da puan verir
            }

            // Ozel flag bonuslari
            if (gameState.HasFlag("dragon_slayer")) points += 50;
            if (gameState.HasFlag("grail_found")) points += 50;
            if (gameState.HasFlag("time_saved")) points += 40;
            if (gameState.HasFlag("crusade_joined")) points += 30;
            if (gameState.HasFlag("wise_decisions")) points += 20;
            if (gameState.HasFlag("diplomatic_success")) points += 25;

            // Minimum 10 puan
            return Mathf.Max(points, 10);
        }
        #endregion

        #region Public Methods - Unlocks
        /// <summary>
        /// Era unlock kontrolu
        /// </summary>
        public bool IsEraUnlocked(Era era)
        {
            return _unlockData.IsEraUnlocked(era);
        }

        /// <summary>
        /// Era unlock et
        /// </summary>
        public bool UnlockEra(Era era)
        {
            if (IsEraUnlocked(era))
                return true;

            int cost = EraUnlockCosts[era];

            if (_totalPrestigePoints < cost)
            {
                Debug.Log($"[PrestigeManager] Yetersiz PP. Gerekli: {cost}, Mevcut: {_totalPrestigePoints}");
                return false;
            }

            if (SpendPrestigePoints(cost))
            {
                _unlockData.UnlockEra(era);
                OnEraUnlocked?.Invoke(era);

                Debug.Log($"[PrestigeManager] {EraNames[era]} donemi acildi!");

                SavePrestigeData();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Senaryo unlock kontrolu
        /// </summary>
        public bool IsScenarioUnlocked(string scenarioId)
        {
            return _unlockData.unlockedScenarios.Contains(scenarioId);
        }

        /// <summary>
        /// Senaryo unlock et
        /// </summary>
        public bool UnlockScenario(string scenarioId)
        {
            if (IsScenarioUnlocked(scenarioId))
                return true;

            if (!Scenarios.TryGetValue(scenarioId, out var scenario))
            {
                Debug.LogWarning($"[PrestigeManager] Senaryo bulunamadi: {scenarioId}");
                return false;
            }

            if (_totalPrestigePoints < scenario.unlockCost)
            {
                Debug.Log($"[PrestigeManager] Yetersiz PP. Gerekli: {scenario.unlockCost}");
                return false;
            }

            if (SpendPrestigePoints(scenario.unlockCost))
            {
                _unlockData.unlockedScenarios.Add(scenarioId);
                OnScenarioUnlocked?.Invoke(scenarioId);

                Debug.Log($"[PrestigeManager] {scenario.name} senaryosu acildi!");

                SavePrestigeData();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Senaryo bilgisini al
        /// </summary>
        public ScenarioInfo GetScenarioInfo(string scenarioId)
        {
            return Scenarios.TryGetValue(scenarioId, out var scenario) ? scenario : null;
        }

        /// <summary>
        /// Tum acilabilir eralari listele
        /// </summary>
        public List<EraUnlockInfo> GetEraUnlockInfos()
        {
            var result = new List<EraUnlockInfo>();

            foreach (var kvp in EraUnlockCosts)
            {
                result.Add(new EraUnlockInfo
                {
                    era = kvp.Key,
                    name = EraNames[kvp.Key],
                    cost = kvp.Value,
                    isUnlocked = IsEraUnlocked(kvp.Key),
                    canAfford = _totalPrestigePoints >= kvp.Value
                });
            }

            return result;
        }

        /// <summary>
        /// Tum acilabilir senaryolari listele
        /// </summary>
        public List<ScenarioUnlockInfo> GetScenarioUnlockInfos()
        {
            var result = new List<ScenarioUnlockInfo>();

            foreach (var scenario in Scenarios.Values)
            {
                result.Add(new ScenarioUnlockInfo
                {
                    scenario = scenario,
                    isUnlocked = IsScenarioUnlocked(scenario.id),
                    canAfford = _totalPrestigePoints >= scenario.unlockCost
                });
            }

            return result;
        }
        #endregion

        #region Private Methods
        private void CheckAutoUnlocks()
        {
            // PP esigine gore otomatik unlock kontrolu (opsiyonel)
            // Su an kullanilmiyor, manuel unlock tercih ediliyor
        }

        private void SavePrestigeData()
        {
            PlayerPrefs.SetInt(Constants.SAVE_KEY_PRESTIGE, _totalPrestigePoints);

            // UnlockData'yi JSON olarak kaydet
            string unlockJson = JsonUtility.ToJson(_unlockData);
            PlayerPrefs.SetString(Constants.SAVE_KEY_UNLOCKS, unlockJson);

            PlayerPrefs.Save();
        }

        private void LoadPrestigeData()
        {
            _totalPrestigePoints = PlayerPrefs.GetInt(Constants.SAVE_KEY_PRESTIGE, 0);

            string unlockJson = PlayerPrefs.GetString(Constants.SAVE_KEY_UNLOCKS, "");
            if (!string.IsNullOrEmpty(unlockJson))
            {
                try
                {
                    _unlockData = JsonUtility.FromJson<UnlockData>(unlockJson);
                }
                catch
                {
                    _unlockData = new UnlockData();
                }
            }
            else
            {
                _unlockData = new UnlockData();
            }

            Debug.Log($"[PrestigeManager] Yuklendi. PP: {_totalPrestigePoints}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Add 100 PP")]
        private void DebugAdd100PP()
        {
            AddPrestigePoints(100);
        }

        [ContextMenu("Add 1000 PP")]
        private void DebugAdd1000PP()
        {
            AddPrestigePoints(1000);
        }

        [ContextMenu("Reset All Prestige")]
        private void DebugResetPrestige()
        {
            _totalPrestigePoints = 0;
            _unlockData = new UnlockData();
            SavePrestigeData();
            Debug.Log("[PrestigeManager] Tum prestige verileri sifirlandi!");
        }

        [ContextMenu("Print Unlock Status")]
        private void DebugPrintUnlockStatus()
        {
            Debug.Log($"Total PP: {_totalPrestigePoints}");
            Debug.Log($"Unlocked Eras: {string.Join(", ", _unlockData.unlockedEras)}");
            Debug.Log($"Unlocked Scenarios: {string.Join(", ", _unlockData.unlockedScenarios)}");
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Senaryo bilgisi
    /// </summary>
    [System.Serializable]
    public class ScenarioInfo
    {
        public string id;
        public string name;
        public string description;
        public int unlockCost;
        public Resources startingResources;
    }

    /// <summary>
    /// Era unlock bilgisi (UI icin)
    /// </summary>
    public class EraUnlockInfo
    {
        public Era era;
        public string name;
        public int cost;
        public bool isUnlocked;
        public bool canAfford;
    }

    /// <summary>
    /// Senaryo unlock bilgisi (UI icin)
    /// </summary>
    public class ScenarioUnlockInfo
    {
        public ScenarioInfo scenario;
        public bool isUnlocked;
        public bool canAfford;
    }
    #endregion
}
