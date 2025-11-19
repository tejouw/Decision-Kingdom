using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Başlangıç senaryosu verisi
    /// </summary>
    [CreateAssetMenu(fileName = "NewScenario", menuName = "Decision Kingdom/Scenario")]
    public class ScenarioData : ScriptableObject
    {
        [Header("Senaryo Bilgileri")]
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;

        [Header("Başlangıç Kaynakları")]
        [SerializeField] private int _startingGold = 50;
        [SerializeField] private int _startingHappiness = 50;
        [SerializeField] private int _startingMilitary = 50;
        [SerializeField] private int _startingFaith = 50;

        [Header("Dönem")]
        [SerializeField] private Era _startingEra = Era.Medieval;

        [Header("Unlock")]
        [SerializeField] private int _requiredPrestigePoints;
        [SerializeField] private bool _isDefault;

        #region Properties
        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public Era StartingEra => _startingEra;
        public int RequiredPrestigePoints => _requiredPrestigePoints;
        public bool IsDefault => _isDefault;

        public Resources StartingResources => new Resources(
            _startingGold,
            _startingHappiness,
            _startingMilitary,
            _startingFaith
        );
        #endregion

        #region Public Methods
        /// <summary>
        /// Unlock edilmiş mi kontrol et
        /// </summary>
        public bool IsUnlocked(int currentPrestigePoints)
        {
            return _isDefault || currentPrestigePoints >= _requiredPrestigePoints;
        }

        /// <summary>
        /// Başlangıç kaynaklarının özet metni
        /// </summary>
        public string GetResourcesSummary()
        {
            return $"Para: {_startingGold} | Mutluluk: {_startingHappiness}\n" +
                   $"Askeri: {_startingMilitary} | İnanç: {_startingFaith}";
        }
        #endregion
    }
}
