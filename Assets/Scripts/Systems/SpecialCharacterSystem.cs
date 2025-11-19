using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Özel unlock edilebilir karakterler ve passive yetenekleri
    /// GAME_DESIGN.md: Zeki Casus, Sadık General, Mucize İşçi
    /// </summary>
    public class SpecialCharacterSystem : MonoBehaviour
    {
        public static SpecialCharacterSystem Instance { get; private set; }

        [Header("Active Characters")]
        [SerializeField] private List<string> _activeCharacters = new List<string>();
        [SerializeField] private SpecialCharacterData _characterData;

        // Events
        public event Action<string> OnSpecialCharacterUnlocked;
        public event Action<string> OnSpecialCharacterActivated;
        public event Action<string> OnSpecialCharacterDeactivated;
        public event Action<string, string> OnAbilityTriggered; // characterId, message

        #region Special Character Definitions
        public static readonly Dictionary<string, SpecialCharacterInfo> SpecialCharacters = new Dictionary<string, SpecialCharacterInfo>
        {
            ["smart_spy"] = new SpecialCharacterInfo
            {
                id = "smart_spy",
                name = "Zeki Casus",
                title = "Gölge Danışman",
                description = "Gelecek olayları hafif spoiler eder. Her 5 turda bir sonraki 3 olayın ipucunu verir.",
                unlockCost = 200,
                abilityType = SpecialAbilityType.EventPreview,
                abilityDescription = "Her 5 turda bir sonraki olayları önceden görürsünüz.",
                portraitPath = "Portraits/smart_spy"
            },
            ["loyal_general"] = new SpecialCharacterInfo
            {
                id = "loyal_general",
                name = "Sadık General",
                title = "Koruyucu Komutan",
                description = "Military asla 0'a düşmez. Minimum 5'te kalır ve savaşlarda bonus sağlar.",
                unlockCost = 300,
                abilityType = SpecialAbilityType.ResourceProtection,
                abilityDescription = "Military asla 0'a düşmez (minimum 5).",
                portraitPath = "Portraits/loyal_general",
                protectedResource = ResourceType.Military,
                minimumValue = 5
            },
            ["miracle_worker"] = new SpecialCharacterInfo
            {
                id = "miracle_worker",
                name = "Mucize İşçi",
                title = "Kutsal Elçi",
                description = "Faith kaynaklarına +20% bonus verir. Dini olaylarda ekstra etki sağlar.",
                unlockCost = 250,
                abilityType = SpecialAbilityType.ResourceBonus,
                abilityDescription = "Faith değişimlerine +20% bonus.",
                portraitPath = "Portraits/miracle_worker",
                bonusResource = ResourceType.Faith,
                bonusPercentage = 0.2f
            },
            ["master_merchant"] = new SpecialCharacterInfo
            {
                id = "master_merchant",
                name = "Usta Tüccar",
                title = "Hazine Koruyucusu",
                description = "Gold kaynaklarına +15% bonus verir. Ticaret olaylarında ekstra kazanç.",
                unlockCost = 225,
                abilityType = SpecialAbilityType.ResourceBonus,
                abilityDescription = "Gold değişimlerine +15% bonus.",
                portraitPath = "Portraits/master_merchant",
                bonusResource = ResourceType.Gold,
                bonusPercentage = 0.15f
            },
            ["peoples_voice"] = new SpecialCharacterInfo
            {
                id = "peoples_voice",
                name = "Halkın Sesi",
                title = "Halk Temsilcisi",
                description = "Happiness asla 0'a düşmez. Minimum 5'te kalır ve isyanları önler.",
                unlockCost = 275,
                abilityType = SpecialAbilityType.ResourceProtection,
                abilityDescription = "Happiness asla 0'a düşmez (minimum 5).",
                portraitPath = "Portraits/peoples_voice",
                protectedResource = ResourceType.Happiness,
                minimumValue = 5
            },
            ["oracle"] = new SpecialCharacterInfo
            {
                id = "oracle",
                name = "Kahin",
                title = "Geleceği Gören",
                description = "Nadir olayların görülme şansını 2x artırır.",
                unlockCost = 400,
                abilityType = SpecialAbilityType.RareEventBonus,
                abilityDescription = "Nadir olay şansı 2 katına çıkar.",
                portraitPath = "Portraits/oracle"
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
            if (_characterData == null)
            {
                _characterData = new SpecialCharacterData();
            }

            LoadCharacterData();
        }
        #endregion

        #region Public Methods - Unlock & Activation
        /// <summary>
        /// Özel karakter unlock kontrolü
        /// </summary>
        public bool IsCharacterUnlocked(string characterId)
        {
            return _characterData.unlockedCharacters.Contains(characterId);
        }

        /// <summary>
        /// Özel karakteri unlock et
        /// </summary>
        public bool UnlockCharacter(string characterId)
        {
            if (IsCharacterUnlocked(characterId))
                return true;

            if (!SpecialCharacters.TryGetValue(characterId, out var charInfo))
            {
                Debug.LogWarning($"[SpecialCharacterSystem] Karakter bulunamadı: {characterId}");
                return false;
            }

            var prestigeManager = PrestigeManager.Instance;
            if (prestigeManager == null || prestigeManager.TotalPrestigePoints < charInfo.unlockCost)
            {
                Debug.Log($"[SpecialCharacterSystem] Yetersiz PP. Gerekli: {charInfo.unlockCost}");
                return false;
            }

            if (prestigeManager.SpendPrestigePoints(charInfo.unlockCost))
            {
                _characterData.unlockedCharacters.Add(characterId);
                OnSpecialCharacterUnlocked?.Invoke(characterId);

                Debug.Log($"[SpecialCharacterSystem] {charInfo.name} açıldı!");

                SaveCharacterData();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Özel karakteri aktif et (oyun başlangıcında)
        /// </summary>
        public bool ActivateCharacter(string characterId)
        {
            if (!IsCharacterUnlocked(characterId))
            {
                Debug.LogWarning($"[SpecialCharacterSystem] Karakter açık değil: {characterId}");
                return false;
            }

            if (_activeCharacters.Contains(characterId))
                return true;

            // Maksimum 2 aktif karakter
            if (_activeCharacters.Count >= 2)
            {
                Debug.Log("[SpecialCharacterSystem] Maksimum 2 karakter aktif olabilir.");
                return false;
            }

            _activeCharacters.Add(characterId);
            OnSpecialCharacterActivated?.Invoke(characterId);

            Debug.Log($"[SpecialCharacterSystem] {SpecialCharacters[characterId].name} aktif edildi.");
            return true;
        }

        /// <summary>
        /// Özel karakteri deaktif et
        /// </summary>
        public void DeactivateCharacter(string characterId)
        {
            if (_activeCharacters.Remove(characterId))
            {
                OnSpecialCharacterDeactivated?.Invoke(characterId);
                Debug.Log($"[SpecialCharacterSystem] {SpecialCharacters[characterId].name} deaktif edildi.");
            }
        }

        /// <summary>
        /// Tüm aktif karakterleri temizle
        /// </summary>
        public void ClearActiveCharacters()
        {
            _activeCharacters.Clear();
        }

        /// <summary>
        /// Aktif karakter listesini al
        /// </summary>
        public List<string> GetActiveCharacters()
        {
            return new List<string>(_activeCharacters);
        }
        #endregion

        #region Public Methods - Ability Effects
        /// <summary>
        /// Resource koruma kontrolü (Sadık General, Halkın Sesi)
        /// </summary>
        public int ApplyResourceProtection(ResourceType resourceType, int currentValue, int newValue)
        {
            foreach (var characterId in _activeCharacters)
            {
                if (!SpecialCharacters.TryGetValue(characterId, out var charInfo))
                    continue;

                if (charInfo.abilityType == SpecialAbilityType.ResourceProtection &&
                    charInfo.protectedResource == resourceType)
                {
                    if (newValue < charInfo.minimumValue)
                    {
                        OnAbilityTriggered?.Invoke(characterId,
                            $"{charInfo.name} {resourceType}'ı korudu! (Min: {charInfo.minimumValue})");
                        return charInfo.minimumValue;
                    }
                }
            }

            return newValue;
        }

        /// <summary>
        /// Resource bonus uygula (Mucize İşçi, Usta Tüccar)
        /// </summary>
        public int ApplyResourceBonus(ResourceType resourceType, int change)
        {
            if (change == 0) return 0;

            float totalBonus = 0f;

            foreach (var characterId in _activeCharacters)
            {
                if (!SpecialCharacters.TryGetValue(characterId, out var charInfo))
                    continue;

                if (charInfo.abilityType == SpecialAbilityType.ResourceBonus &&
                    charInfo.bonusResource == resourceType)
                {
                    // Sadece pozitif değişimlere bonus
                    if (change > 0)
                    {
                        totalBonus += charInfo.bonusPercentage;
                    }
                }
            }

            if (totalBonus > 0 && change > 0)
            {
                int bonusAmount = Mathf.RoundToInt(change * totalBonus);
                int newChange = change + bonusAmount;

                Debug.Log($"[SpecialCharacterSystem] {resourceType} bonus: {change} -> {newChange} (+{bonusAmount})");
                return newChange;
            }

            return change;
        }

        /// <summary>
        /// Nadir olay bonus kontrolü (Kahin)
        /// </summary>
        public float GetRareEventMultiplier()
        {
            float multiplier = 1f;

            foreach (var characterId in _activeCharacters)
            {
                if (!SpecialCharacters.TryGetValue(characterId, out var charInfo))
                    continue;

                if (charInfo.abilityType == SpecialAbilityType.RareEventBonus)
                {
                    multiplier *= 2f;
                }
            }

            return multiplier;
        }

        /// <summary>
        /// Olay önizleme aktif mi? (Zeki Casus)
        /// </summary>
        public bool IsEventPreviewActive()
        {
            foreach (var characterId in _activeCharacters)
            {
                if (!SpecialCharacters.TryGetValue(characterId, out var charInfo))
                    continue;

                if (charInfo.abilityType == SpecialAbilityType.EventPreview)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Önizleme gösterilmeli mi? (her 5 turda)
        /// </summary>
        public bool ShouldShowEventPreview(int turn)
        {
            return IsEventPreviewActive() && turn > 0 && turn % 5 == 0;
        }

        /// <summary>
        /// Karakterin aktif olup olmadığını kontrol et
        /// </summary>
        public bool IsCharacterActive(string characterId)
        {
            return _activeCharacters.Contains(characterId);
        }
        #endregion

        #region Public Methods - UI & Info
        /// <summary>
        /// Tüm özel karakterlerin bilgisini al
        /// </summary>
        public List<SpecialCharacterUnlockInfo> GetAllCharacterInfos()
        {
            var result = new List<SpecialCharacterUnlockInfo>();
            var prestigeManager = PrestigeManager.Instance;
            int currentPP = prestigeManager?.TotalPrestigePoints ?? 0;

            foreach (var charInfo in SpecialCharacters.Values)
            {
                result.Add(new SpecialCharacterUnlockInfo
                {
                    character = charInfo,
                    isUnlocked = IsCharacterUnlocked(charInfo.id),
                    isActive = IsCharacterActive(charInfo.id),
                    canAfford = currentPP >= charInfo.unlockCost
                });
            }

            return result;
        }

        /// <summary>
        /// Karakter bilgisini al
        /// </summary>
        public SpecialCharacterInfo GetCharacterInfo(string characterId)
        {
            return SpecialCharacters.TryGetValue(characterId, out var info) ? info : null;
        }

        /// <summary>
        /// Aktif karakter sayısı
        /// </summary>
        public int ActiveCharacterCount => _activeCharacters.Count;

        /// <summary>
        /// Maksimum aktif karakter sayısı
        /// </summary>
        public int MaxActiveCharacters => 2;
        #endregion

        #region Private Methods
        private void SaveCharacterData()
        {
            string json = JsonUtility.ToJson(_characterData);
            PlayerPrefs.SetString("SpecialCharacterData", json);
            PlayerPrefs.Save();
        }

        private void LoadCharacterData()
        {
            string json = PlayerPrefs.GetString("SpecialCharacterData", "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    _characterData = JsonUtility.FromJson<SpecialCharacterData>(json);
                }
                catch
                {
                    _characterData = new SpecialCharacterData();
                }
            }
            else
            {
                _characterData = new SpecialCharacterData();
            }

            Debug.Log($"[SpecialCharacterSystem] Yüklendi. Açık karakterler: {_characterData.unlockedCharacters.Count}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Unlock All Characters")]
        private void DebugUnlockAll()
        {
            foreach (var charId in SpecialCharacters.Keys)
            {
                if (!_characterData.unlockedCharacters.Contains(charId))
                {
                    _characterData.unlockedCharacters.Add(charId);
                }
            }
            SaveCharacterData();
            Debug.Log("[SpecialCharacterSystem] Tüm karakterler açıldı!");
        }

        [ContextMenu("Reset All Characters")]
        private void DebugResetAll()
        {
            _characterData = new SpecialCharacterData();
            _activeCharacters.Clear();
            SaveCharacterData();
            Debug.Log("[SpecialCharacterSystem] Tüm karakter verileri sıfırlandı!");
        }

        [ContextMenu("Activate Smart Spy")]
        private void DebugActivateSmartSpy()
        {
            if (!IsCharacterUnlocked("smart_spy"))
                _characterData.unlockedCharacters.Add("smart_spy");
            ActivateCharacter("smart_spy");
        }

        [ContextMenu("Activate Loyal General")]
        private void DebugActivateLoyalGeneral()
        {
            if (!IsCharacterUnlocked("loyal_general"))
                _characterData.unlockedCharacters.Add("loyal_general");
            ActivateCharacter("loyal_general");
        }

        [ContextMenu("Activate Miracle Worker")]
        private void DebugActivateMiracleWorker()
        {
            if (!IsCharacterUnlocked("miracle_worker"))
                _characterData.unlockedCharacters.Add("miracle_worker");
            ActivateCharacter("miracle_worker");
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Özel karakter bilgisi
    /// </summary>
    [System.Serializable]
    public class SpecialCharacterInfo
    {
        public string id;
        public string name;
        public string title;
        public string description;
        public int unlockCost;
        public SpecialAbilityType abilityType;
        public string abilityDescription;
        public string portraitPath;

        // Resource Protection için
        public ResourceType protectedResource;
        public int minimumValue;

        // Resource Bonus için
        public ResourceType bonusResource;
        public float bonusPercentage;
    }

    /// <summary>
    /// Özel yetenek türleri
    /// </summary>
    public enum SpecialAbilityType
    {
        EventPreview,       // Zeki Casus - Olayları önceden görme
        ResourceProtection, // Sadık General, Halkın Sesi - Resource 0'a düşmez
        ResourceBonus,      // Mucize İşçi, Usta Tüccar - Resource bonus
        RareEventBonus      // Kahin - Nadir olay şansı artışı
    }

    /// <summary>
    /// Özel karakter kayıt verileri
    /// </summary>
    [System.Serializable]
    public class SpecialCharacterData
    {
        public List<string> unlockedCharacters = new List<string>();
    }

    /// <summary>
    /// UI için unlock bilgisi
    /// </summary>
    public class SpecialCharacterUnlockInfo
    {
        public SpecialCharacterInfo character;
        public bool isUnlocked;
        public bool isActive;
        public bool canAfford;
    }
    #endregion
}
