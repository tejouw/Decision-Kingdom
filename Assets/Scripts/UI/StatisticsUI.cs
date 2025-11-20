using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Istatistik UI komponenti
    /// </summary>
    public class StatisticsUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _statisticsPanel;

        [Header("Lifetime Stats")]
        [SerializeField] private Text _totalGamesText;
        [SerializeField] private Text _totalTurnsText;
        [SerializeField] private Text _totalPlayTimeText;
        [SerializeField] private Text _totalPrestigeText;
        [SerializeField] private Text _bestTurnText;
        [SerializeField] private Text _averageTurnText;

        [Header("Win/Loss Stats")]
        [SerializeField] private Text _victoriesText;
        [SerializeField] private Text _defeatsText;
        [SerializeField] private Text _winRateText;
        [SerializeField] private Slider _winRateSlider;

        [Header("Death Causes")]
        [SerializeField] private Text _bankruptcyDeathsText;
        [SerializeField] private Text _revolutionDeathsText;
        [SerializeField] private Text _invasionDeathsText;
        [SerializeField] private Text _chaosDeathsText;
        [SerializeField] private Text _otherDeathsText;

        [Header("Era Stats")]
        [SerializeField] private Text _medievalGamesText;
        [SerializeField] private Text _renaissanceGamesText;
        [SerializeField] private Text _industrialGamesText;
        [SerializeField] private Text _modernGamesText;
        [SerializeField] private Text _futureGamesText;

        [Header("Card Stats")]
        [SerializeField] private Text _totalCardsText;
        [SerializeField] private Text _leftSwipesText;
        [SerializeField] private Text _rightSwipesText;
        [SerializeField] private Text _rareEventsText;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetButton;

        [Header("Reset Confirmation")]
        [SerializeField] private GameObject _confirmResetPanel;
        [SerializeField] private Button _confirmResetYesButton;
        [SerializeField] private Button _confirmResetNoButton;

        // Events
        public event System.Action OnBackClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            UpdateUI();
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_backButton != null)
                _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

            if (_resetButton != null)
                _resetButton.onClick.AddListener(ShowResetConfirmation);

            if (_confirmResetYesButton != null)
                _confirmResetYesButton.onClick.AddListener(ResetStatistics);

            if (_confirmResetNoButton != null)
                _confirmResetNoButton.onClick.AddListener(HideResetConfirmation);
        }
        #endregion

        #region UI Updates
        private void UpdateUI()
        {
            if (StatisticsManager.Instance == null) return;

            var stats = StatisticsManager.Instance.LifetimeStats;
            if (stats == null) return;

            // Lifetime Stats
            if (_totalGamesText != null)
                _totalGamesText.text = $"Toplam Oyun: {stats.totalGamesCompleted}";

            if (_totalTurnsText != null)
                _totalTurnsText.text = $"Toplam Tur: {stats.totalTurnsPlayed}";

            if (_totalPlayTimeText != null)
            {
                int hours = stats.totalPlayTimeMinutes / 60;
                int minutes = stats.totalPlayTimeMinutes % 60;
                _totalPlayTimeText.text = $"Oynama Suresi: {hours}s {minutes}dk";
            }

            if (_totalPrestigeText != null)
                _totalPrestigeText.text = $"Kazanilan PP: {stats.totalPrestigeEarned}";

            if (_bestTurnText != null)
                _bestTurnText.text = $"En Uzun Oyun: {stats.longestGame} tur";

            if (_averageTurnText != null)
            {
                float avg = stats.totalGamesCompleted > 0
                    ? (float)stats.totalTurnsPlayed / stats.totalGamesCompleted
                    : 0f;
                _averageTurnText.text = $"Ortalama: {avg:F1} tur";
            }

            // Win/Loss
            int victories = stats.victories;
            int defeats = stats.totalGamesCompleted - victories;
            float winRate = stats.totalGamesCompleted > 0
                ? (float)victories / stats.totalGamesCompleted * 100f
                : 0f;

            if (_victoriesText != null)
                _victoriesText.text = $"Zaferler: {victories}";

            if (_defeatsText != null)
                _defeatsText.text = $"Yenilgiler: {defeats}";

            if (_winRateText != null)
                _winRateText.text = $"Kazanma Orani: %{winRate:F1}";

            if (_winRateSlider != null)
                _winRateSlider.value = winRate / 100f;

            // Death Causes
            if (_bankruptcyDeathsText != null)
                _bankruptcyDeathsText.text = $"Iflas: {stats.deathsByBankruptcy}";

            if (_revolutionDeathsText != null)
                _revolutionDeathsText.text = $"Isyan: {stats.deathsByRevolution}";

            if (_invasionDeathsText != null)
                _invasionDeathsText.text = $"Istila: {stats.deathsByInvasion}";

            if (_chaosDeathsText != null)
                _chaosDeathsText.text = $"Kaos: {stats.deathsByChaos}";

            if (_otherDeathsText != null)
            {
                int other = defeats - stats.deathsByBankruptcy - stats.deathsByRevolution
                          - stats.deathsByInvasion - stats.deathsByChaos;
                _otherDeathsText.text = $"Diger: {Mathf.Max(0, other)}";
            }

            // Era Stats
            if (_medievalGamesText != null)
                _medievalGamesText.text = $"Ortacag: {stats.gamesPlayedMedieval}";

            if (_renaissanceGamesText != null)
                _renaissanceGamesText.text = $"Ronesans: {stats.gamesPlayedRenaissance}";

            if (_industrialGamesText != null)
                _industrialGamesText.text = $"Sanayi: {stats.gamesPlayedIndustrial}";

            if (_modernGamesText != null)
                _modernGamesText.text = $"Modern: {stats.gamesPlayedModern}";

            if (_futureGamesText != null)
                _futureGamesText.text = $"Gelecek: {stats.gamesPlayedFuture}";

            // Card Stats
            int totalCards = stats.totalLeftSwipes + stats.totalRightSwipes;

            if (_totalCardsText != null)
                _totalCardsText.text = $"Toplam Kart: {totalCards}";

            if (_leftSwipesText != null)
                _leftSwipesText.text = $"Sol Swipe: {stats.totalLeftSwipes}";

            if (_rightSwipesText != null)
                _rightSwipesText.text = $"Sag Swipe: {stats.totalRightSwipes}";

            if (_rareEventsText != null)
                _rareEventsText.text = $"Nadir Event: {stats.rareEventsEncountered}";
        }
        #endregion

        #region Reset
        private void ShowResetConfirmation()
        {
            if (_confirmResetPanel != null)
                _confirmResetPanel.SetActive(true);
        }

        private void HideResetConfirmation()
        {
            if (_confirmResetPanel != null)
                _confirmResetPanel.SetActive(false);
        }

        private void ResetStatistics()
        {
            if (StatisticsManager.Instance != null)
            {
                StatisticsManager.Instance.ResetAllStats();
                UpdateUI();
            }

            HideResetConfirmation();
            Debug.Log("[StatisticsUI] Istatistikler sifirlandi");
        }
        #endregion

        #region Public Methods
        public void Show()
        {
            if (_statisticsPanel != null)
                _statisticsPanel.SetActive(true);

            UpdateUI();
        }

        public void Hide()
        {
            if (_statisticsPanel != null)
                _statisticsPanel.SetActive(false);

            HideResetConfirmation();
        }
        #endregion
    }
}
