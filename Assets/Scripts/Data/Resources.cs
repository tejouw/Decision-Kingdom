using System;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Oyuncu kaynaklarını yöneten veri yapısı
    /// </summary>
    [System.Serializable]
    public class Resources
    {
        [Range(0, 100)]
        [SerializeField] private int _gold;

        [Range(0, 100)]
        [SerializeField] private int _happiness;

        [Range(0, 100)]
        [SerializeField] private int _military;

        [Range(0, 100)]
        [SerializeField] private int _faith;

        // Events
        public event Action<ResourceType, int, int> OnResourceChanged;

        #region Properties
        public int Gold
        {
            get => _gold;
            set => SetResource(ResourceType.Gold, ref _gold, value);
        }

        public int Happiness
        {
            get => _happiness;
            set => SetResource(ResourceType.Happiness, ref _happiness, value);
        }

        public int Military
        {
            get => _military;
            set => SetResource(ResourceType.Military, ref _military, value);
        }

        public int Faith
        {
            get => _faith;
            set => SetResource(ResourceType.Faith, ref _faith, value);
        }
        #endregion

        #region Constructors
        public Resources()
        {
            _gold = Constants.RESOURCE_DEFAULT;
            _happiness = Constants.RESOURCE_DEFAULT;
            _military = Constants.RESOURCE_DEFAULT;
            _faith = Constants.RESOURCE_DEFAULT;
        }

        public Resources(int gold, int happiness, int military, int faith)
        {
            _gold = Mathf.Clamp(gold, Constants.RESOURCE_MIN, Constants.RESOURCE_MAX);
            _happiness = Mathf.Clamp(happiness, Constants.RESOURCE_MIN, Constants.RESOURCE_MAX);
            _military = Mathf.Clamp(military, Constants.RESOURCE_MIN, Constants.RESOURCE_MAX);
            _faith = Mathf.Clamp(faith, Constants.RESOURCE_MIN, Constants.RESOURCE_MAX);
        }

        public Resources(Resources other)
        {
            _gold = other._gold;
            _happiness = other._happiness;
            _military = other._military;
            _faith = other._faith;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Belirli bir kaynağı index ile al
        /// </summary>
        public int GetResource(ResourceType type)
        {
            return type switch
            {
                ResourceType.Gold => _gold,
                ResourceType.Happiness => _happiness,
                ResourceType.Military => _military,
                ResourceType.Faith => _faith,
                _ => 0
            };
        }

        /// <summary>
        /// Belirli bir kaynağı ayarla
        /// </summary>
        public void SetResource(ResourceType type, int value)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    Gold = value;
                    break;
                case ResourceType.Happiness:
                    Happiness = value;
                    break;
                case ResourceType.Military:
                    Military = value;
                    break;
                case ResourceType.Faith:
                    Faith = value;
                    break;
            }
        }

        /// <summary>
        /// Kaynağa değer ekle (negatif olabilir)
        /// </summary>
        public void ModifyResource(ResourceType type, int amount)
        {
            int currentValue = GetResource(type);
            SetResource(type, currentValue + amount);
        }

        /// <summary>
        /// Birden fazla kaynağı aynı anda değiştir
        /// </summary>
        public void ApplyEffects(ResourceEffect[] effects)
        {
            if (effects == null) return;

            foreach (var effect in effects)
            {
                int amount = UnityEngine.Random.Range(effect.min, effect.max + 1);
                ModifyResource(effect.resource, amount);
            }
        }

        /// <summary>
        /// Game Over kontrolü
        /// </summary>
        public GameOverReason CheckGameOver()
        {
            // 0'a düşme kontrolleri
            if (_gold <= Constants.RESOURCE_MIN)
                return GameOverReason.Bankruptcy;
            if (_happiness <= Constants.RESOURCE_MIN)
                return GameOverReason.Revolution;
            if (_military <= Constants.RESOURCE_MIN)
                return GameOverReason.Invasion;
            if (_faith <= Constants.RESOURCE_MIN)
                return GameOverReason.Chaos;

            // 100'e ulaşma kontrolleri
            if (_gold >= Constants.RESOURCE_MAX)
                return GameOverReason.InflationCrisis;
            if (_happiness >= Constants.RESOURCE_MAX)
                return GameOverReason.Laziness;
            if (_military >= Constants.RESOURCE_MAX)
                return GameOverReason.MilitaryCoup;
            if (_faith >= Constants.RESOURCE_MAX)
                return GameOverReason.Theocracy;

            return GameOverReason.None;
        }

        /// <summary>
        /// Tüm kaynakları sıfırla
        /// </summary>
        public void Reset()
        {
            _gold = Constants.RESOURCE_DEFAULT;
            _happiness = Constants.RESOURCE_DEFAULT;
            _military = Constants.RESOURCE_DEFAULT;
            _faith = Constants.RESOURCE_DEFAULT;
        }

        /// <summary>
        /// Kaynak değerlerini kopyala
        /// </summary>
        public void CopyFrom(Resources other)
        {
            _gold = other._gold;
            _happiness = other._happiness;
            _military = other._military;
            _faith = other._faith;
        }

        /// <summary>
        /// Kaynakların toplam dengesi (50'ye yakınlık)
        /// </summary>
        public float GetBalanceScore()
        {
            float goldScore = 1f - Mathf.Abs(_gold - 50f) / 50f;
            float happinessScore = 1f - Mathf.Abs(_happiness - 50f) / 50f;
            float militaryScore = 1f - Mathf.Abs(_military - 50f) / 50f;
            float faithScore = 1f - Mathf.Abs(_faith - 50f) / 50f;

            return (goldScore + happinessScore + militaryScore + faithScore) / 4f;
        }

        public override string ToString()
        {
            return $"Gold: {_gold}, Happiness: {_happiness}, Military: {_military}, Faith: {_faith}";
        }
        #endregion

        #region Private Methods
        private void SetResource(ResourceType type, ref int field, int value)
        {
            int oldValue = field;
            field = Mathf.Clamp(value, Constants.RESOURCE_MIN, Constants.RESOURCE_MAX);

            if (oldValue != field)
            {
                OnResourceChanged?.Invoke(type, oldValue, field);
            }
        }
        #endregion
    }

    /// <summary>
    /// Kaynak değişim efekti
    /// </summary>
    [System.Serializable]
    public class ResourceEffect
    {
        public ResourceType resource;
        public int min;
        public int max;

        public ResourceEffect()
        {
            resource = ResourceType.Gold;
            min = 0;
            max = 0;
        }

        public ResourceEffect(ResourceType resource, int min, int max)
        {
            this.resource = resource;
            this.min = min;
            this.max = max;
        }

        public ResourceEffect(ResourceType resource, int fixedAmount)
        {
            this.resource = resource;
            this.min = fixedAmount;
            this.max = fixedAmount;
        }

        /// <summary>
        /// Rastgele değer hesapla
        /// </summary>
        public int GetRandomValue()
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}
