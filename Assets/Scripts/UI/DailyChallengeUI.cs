using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Gunluk challenge UI komponenti
    /// </summary>
    public class DailyChallengeUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _challengePanel;

        [Header("Today's Challenge")]
        [SerializeField] private Text _dateText;
        [SerializeField] private Text _eraText;
        [SerializeField] private Text _scenarioText;
        [SerializeField] private Text _modifiersText;
        [SerializeField] private Text _statusText;
        [SerializeField] private Button _playButton;

        [Header("Streak Info")]
        [SerializeField] private Text _currentStreakText;
        [SerializeField] private Text _bestStreakText;
        [SerializeField] private Image[] _streakDayIcons;

        [Header("History")]
        [SerializeField] private Transform _historyContainer;
        [SerializeField] private GameObject _historyItemPrefab;
        [SerializeField] private ScrollRect _historyScrollRect;

        [Header("Leaderboard Preview")]
        [SerializeField] private Text _bestScoreText;
        [SerializeField] private Text _globalRankText;
        [SerializeField] private Button _viewLeaderboardButton;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;

        [Header("Colors")]
        [SerializeField] private Color _completedColor = new Color(0.5f, 1f, 0.5f);
        [SerializeField] private Color _pendingColor = Color.white;
        [SerializeField] private Color _missedColor = new Color(1f, 0.5f, 0.5f);

        // Events
        public event System.Action OnPlayClicked;
        public event System.Action OnLeaderboardClicked;
        public event System.Action OnBackClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            UpdateUI();
            PopulateHistory();
        }

        private void OnEnable()
        {
            if (DailyChallengeSystem.Instance != null)
            {
                DailyChallengeSystem.Instance.OnDailyChallengeCompleted += HandleChallengeCompleted;
                DailyChallengeSystem.Instance.OnNewDayDetected += HandleNewDay;
            }
        }

        private void OnDisable()
        {
            if (DailyChallengeSystem.Instance != null)
            {
                DailyChallengeSystem.Instance.OnDailyChallengeCompleted -= HandleChallengeCompleted;
                DailyChallengeSystem.Instance.OnNewDayDetected -= HandleNewDay;
            }
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_playButton != null)
                _playButton.onClick.AddListener(OnPlay);

            if (_viewLeaderboardButton != null)
                _viewLeaderboardButton.onClick.AddListener(() => OnLeaderboardClicked?.Invoke());

            if (_backButton != null)
                _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        }
        #endregion

        #region UI Updates
        private void UpdateUI()
        {
            if (DailyChallengeSystem.Instance == null) return;

            var info = DailyChallengeSystem.Instance.GetTodayInfo();

            // Tarih
            if (_dateText != null)
            {
                _dateText.text = info.date.ToString("dd MMMM yyyy");
            }

            // Era
            if (_eraText != null)
            {
                string eraName = PrestigeManager.EraNames.ContainsKey(info.era)
                    ? PrestigeManager.EraNames[info.era]
                    : info.era.ToString();
                _eraText.text = $"Donem: {eraName}";
            }

            // Senaryo
            if (_scenarioText != null)
            {
                var scenarioInfo = PrestigeManager.Scenarios.ContainsKey(info.scenario)
                    ? PrestigeManager.Scenarios[info.scenario]
                    : null;
                _scenarioText.text = $"Senaryo: {scenarioInfo?.name ?? info.scenario}";
            }

            // Modifier'lar
            if (_modifiersText != null)
            {
                if (info.modifiers != null && info.modifiers.Count > 0)
                {
                    var modifierNames = new List<string>();
                    foreach (var mod in info.modifiers)
                    {
                        modifierNames.Add(GetModifierName(mod));
                    }
                    _modifiersText.text = $"Modifiers: {string.Join(", ", modifierNames)}";
                }
                else
                {
                    _modifiersText.text = "Modifiers: Yok";
                }
            }

            // Durum
            if (_statusText != null)
            {
                if (info.hasCompleted)
                {
                    _statusText.text = $"Tamamlandi! Skor: {info.bestScore}";
                    _statusText.color = _completedColor;
                }
                else
                {
                    _statusText.text = "Henuz tamamlanmadi";
                    _statusText.color = _pendingColor;
                }
            }

            // Buton
            if (_playButton != null)
            {
                var buttonText = _playButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = info.hasCompleted ? "Tekrar Oyna" : "Baslat";
                }
            }

            // Streak
            UpdateStreakDisplay();

            // Leaderboard
            if (_bestScoreText != null)
            {
                _bestScoreText.text = $"En Iyi: {info.bestScore}";
            }

            if (_globalRankText != null)
            {
                _globalRankText.text = info.globalRank > 0 ? $"Siralama: #{info.globalRank}" : "Siralama: -";
            }
        }

        private void UpdateStreakDisplay()
        {
            if (DailyChallengeSystem.Instance == null) return;

            int currentStreak = DailyChallengeSystem.Instance.CurrentStreak;
            int bestStreak = DailyChallengeSystem.Instance.BestStreak;

            if (_currentStreakText != null)
            {
                _currentStreakText.text = $"Mevcut Seri: {currentStreak}";
            }

            if (_bestStreakText != null)
            {
                _bestStreakText.text = $"En Iyi Seri: {bestStreak}";
            }

            // Streak ikonlari (son 7 gun)
            if (_streakDayIcons != null)
            {
                var history = DailyChallengeSystem.Instance.GetHistory(7);
                for (int i = 0; i < _streakDayIcons.Length; i++)
                {
                    if (i < history.Count)
                    {
                        _streakDayIcons[i].color = history[i].isVictory ? _completedColor : _missedColor;
                    }
                    else
                    {
                        _streakDayIcons[i].color = _pendingColor;
                    }
                }
            }
        }

        private void PopulateHistory()
        {
            if (_historyContainer == null || _historyItemPrefab == null) return;

            // Temizle
            foreach (Transform child in _historyContainer)
            {
                Destroy(child.gameObject);
            }

            var history = DailyChallengeSystem.Instance?.GetHistory(7);
            if (history == null) return;

            foreach (var result in history)
            {
                GameObject itemObj = Instantiate(_historyItemPrefab, _historyContainer);
                var texts = itemObj.GetComponentsInChildren<Text>();

                if (texts.Length >= 2)
                {
                    texts[0].text = result.date.ToString("MM/dd");
                    texts[1].text = $"{result.score} puan";
                }

                var bg = itemObj.GetComponent<Image>();
                if (bg != null)
                {
                    bg.color = result.isVictory ? _completedColor : _missedColor;
                }
            }
        }
        #endregion

        #region Event Handlers
        private void OnPlay()
        {
            if (DailyChallengeSystem.Instance != null)
            {
                DailyChallengeSystem.Instance.StartDailyChallenge();
            }

            OnPlayClicked?.Invoke();
        }

        private void HandleChallengeCompleted(DailyChallengeResult result)
        {
            UpdateUI();
            PopulateHistory();
        }

        private void HandleNewDay()
        {
            UpdateUI();
            PopulateHistory();
        }
        #endregion

        #region Helper Methods
        private string GetModifierName(ChallengeModifier modifier)
        {
            return modifier switch
            {
                ChallengeModifier.HighStakes => "Yuksek Risk",
                ChallengeModifier.PovertyStart => "Fakir Baslangic",
                ChallengeModifier.WarTorn => "Savas Yorgunlugu",
                ChallengeModifier.ReligiousCrisis => "Dini Kriz",
                ChallengeModifier.GoldenAge => "Altin Cag",
                ChallengeModifier.Balanced => "Dengeli",
                ChallengeModifier.NoSecondChance => "Ikinci Sans Yok",
                ChallengeModifier.CharacterFocus => "Karakter Odakli",
                ChallengeModifier.ResourceRace => "Kaynak Yarisi",
                ChallengeModifier.Speedrun => "Hizli Kosus",
                _ => modifier.ToString()
            };
        }
        #endregion

        #region Public Methods
        public void Show()
        {
            if (_challengePanel != null)
                _challengePanel.SetActive(true);

            UpdateUI();
            PopulateHistory();
        }

        public void Hide()
        {
            if (_challengePanel != null)
                _challengePanel.SetActive(false);
        }
        #endregion
    }
}
