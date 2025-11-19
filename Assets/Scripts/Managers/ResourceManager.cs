using System;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Managers
{
    /// <summary>
    /// Kaynak yönetimi ve UI güncellemelerini koordine eden manager
    /// </summary>
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        [Header("Mevcut Kaynaklar")]
        [SerializeField] private Resources _resources;

        // Events
        public event Action<ResourceType, int, int> OnResourceChanged;
        public event Action<GameOverReason> OnGameOver;
        public event Action<ResourceType, int> OnResourcePreview;
        public event Action OnPreviewCleared;

        #region Properties
        public Resources CurrentResources => _resources;
        public int Gold => _resources.Gold;
        public int Happiness => _resources.Happiness;
        public int Military => _resources.Military;
        public int Faith => _resources.Faith;
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

            InitializeResources();
        }

        private void OnDestroy()
        {
            if (_resources != null)
            {
                _resources.OnResourceChanged -= HandleResourceChanged;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Kaynakları başlat
        /// </summary>
        public void InitializeResources()
        {
            _resources = new Resources();
            _resources.OnResourceChanged += HandleResourceChanged;
        }

        /// <summary>
        /// Kaynakları özel değerlerle başlat
        /// </summary>
        public void InitializeResources(int gold, int happiness, int military, int faith)
        {
            if (_resources != null)
            {
                _resources.OnResourceChanged -= HandleResourceChanged;
            }

            _resources = new Resources(gold, happiness, military, faith);
            _resources.OnResourceChanged += HandleResourceChanged;
        }

        /// <summary>
        /// Mevcut kaynakları yükle
        /// </summary>
        public void LoadResources(Resources resources)
        {
            if (_resources != null)
            {
                _resources.OnResourceChanged -= HandleResourceChanged;
            }

            _resources = new Resources(resources);
            _resources.OnResourceChanged += HandleResourceChanged;

            // Tüm kaynakları güncelle
            OnResourceChanged?.Invoke(ResourceType.Gold, 0, _resources.Gold);
            OnResourceChanged?.Invoke(ResourceType.Happiness, 0, _resources.Happiness);
            OnResourceChanged?.Invoke(ResourceType.Military, 0, _resources.Military);
            OnResourceChanged?.Invoke(ResourceType.Faith, 0, _resources.Faith);
        }

        /// <summary>
        /// Belirli bir kaynağı değiştir
        /// </summary>
        public void ModifyResource(ResourceType type, int amount)
        {
            _resources.ModifyResource(type, amount);
            CheckGameOver();
        }

        /// <summary>
        /// Birden fazla efekti uygula
        /// </summary>
        public void ApplyEffects(ResourceEffect[] effects)
        {
            _resources.ApplyEffects(effects);
            CheckGameOver();
        }

        /// <summary>
        /// Seçim efektlerini uygula
        /// </summary>
        public void ApplyChoice(Choice choice)
        {
            if (choice == null || choice.effects == null) return;
            _resources.ApplyEffects(choice.effects.ToArray());
            CheckGameOver();
        }

        /// <summary>
        /// Efektlerin önizlemesini göster (swipe sırasında)
        /// </summary>
        public void PreviewEffects(ResourceEffect[] effects)
        {
            if (effects == null) return;

            foreach (var effect in effects)
            {
                int previewAmount = (effect.min + effect.max) / 2;
                OnResourcePreview?.Invoke(effect.resource, previewAmount);
            }
        }

        /// <summary>
        /// Önizlemeyi temizle
        /// </summary>
        public void ClearPreview()
        {
            OnPreviewCleared?.Invoke();
        }

        /// <summary>
        /// Belirli bir kaynağın değerini al
        /// </summary>
        public int GetResource(ResourceType type)
        {
            return _resources.GetResource(type);
        }

        /// <summary>
        /// Kaynakları sıfırla
        /// </summary>
        public void ResetResources()
        {
            _resources.Reset();

            OnResourceChanged?.Invoke(ResourceType.Gold, 0, Constants.RESOURCE_DEFAULT);
            OnResourceChanged?.Invoke(ResourceType.Happiness, 0, Constants.RESOURCE_DEFAULT);
            OnResourceChanged?.Invoke(ResourceType.Military, 0, Constants.RESOURCE_DEFAULT);
            OnResourceChanged?.Invoke(ResourceType.Faith, 0, Constants.RESOURCE_DEFAULT);
        }

        /// <summary>
        /// Kaynakların denge skorunu al
        /// </summary>
        public float GetBalanceScore()
        {
            return _resources.GetBalanceScore();
        }

        /// <summary>
        /// Kritik kaynakları kontrol et (uyarı için)
        /// </summary>
        public bool HasCriticalResource(int threshold = 15)
        {
            return _resources.Gold <= threshold ||
                   _resources.Gold >= (100 - threshold) ||
                   _resources.Happiness <= threshold ||
                   _resources.Happiness >= (100 - threshold) ||
                   _resources.Military <= threshold ||
                   _resources.Military >= (100 - threshold) ||
                   _resources.Faith <= threshold ||
                   _resources.Faith >= (100 - threshold);
        }

        /// <summary>
        /// En kritik kaynağı bul
        /// </summary>
        public ResourceType GetMostCriticalResource()
        {
            int minDistance = int.MaxValue;
            ResourceType criticalType = ResourceType.Gold;

            CheckResourceCriticality(ResourceType.Gold, _resources.Gold, ref minDistance, ref criticalType);
            CheckResourceCriticality(ResourceType.Happiness, _resources.Happiness, ref minDistance, ref criticalType);
            CheckResourceCriticality(ResourceType.Military, _resources.Military, ref minDistance, ref criticalType);
            CheckResourceCriticality(ResourceType.Faith, _resources.Faith, ref minDistance, ref criticalType);

            return criticalType;
        }
        #endregion

        #region Private Methods
        private void HandleResourceChanged(ResourceType type, int oldValue, int newValue)
        {
            OnResourceChanged?.Invoke(type, oldValue, newValue);
        }

        private void CheckGameOver()
        {
            GameOverReason reason = _resources.CheckGameOver();
            if (reason != GameOverReason.None)
            {
                OnGameOver?.Invoke(reason);
            }
        }

        private void CheckResourceCriticality(ResourceType type, int value, ref int minDistance, ref ResourceType criticalType)
        {
            int distanceToMin = value;
            int distanceToMax = 100 - value;
            int distance = Mathf.Min(distanceToMin, distanceToMax);

            if (distance < minDistance)
            {
                minDistance = distance;
                criticalType = type;
            }
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Set All to 50")]
        private void DebugSetAllTo50()
        {
            InitializeResources(50, 50, 50, 50);
        }

        [ContextMenu("Set Random Values")]
        private void DebugSetRandom()
        {
            InitializeResources(
                UnityEngine.Random.Range(20, 80),
                UnityEngine.Random.Range(20, 80),
                UnityEngine.Random.Range(20, 80),
                UnityEngine.Random.Range(20, 80)
            );
        }

        [ContextMenu("Trigger Low Gold")]
        private void DebugTriggerLowGold()
        {
            _resources.Gold = 5;
        }
#endif
        #endregion
    }
}
