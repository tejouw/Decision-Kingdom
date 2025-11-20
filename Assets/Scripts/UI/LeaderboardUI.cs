using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Leaderboard UI komponenti
    /// </summary>
    public class LeaderboardUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _leaderboardPanel;

        [Header("Tab Buttons")]
        [SerializeField] private Button _dailyTabButton;
        [SerializeField] private Button _weeklyTabButton;
        [SerializeField] private Button _allTimeTabButton;

        [Header("Leaderboard List")]
        [SerializeField] private Transform _entryContainer;
        [SerializeField] private GameObject _entryPrefab;
        [SerializeField] private ScrollRect _scrollRect;

        [Header("Player Info")]
        [SerializeField] private Text _playerRankText;
        [SerializeField] private Text _playerScoreText;
        [SerializeField] private Text _playerNameText;

        [Header("Loading")]
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Text _statusText;

        [Header("Buttons")]
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _backButton;

        [Header("Colors")]
        [SerializeField] private Color _goldColor = new Color(1f, 0.84f, 0f);
        [SerializeField] private Color _silverColor = new Color(0.75f, 0.75f, 0.75f);
        [SerializeField] private Color _bronzeColor = new Color(0.8f, 0.5f, 0.2f);
        [SerializeField] private Color _playerHighlightColor = new Color(0.5f, 0.8f, 1f);
        [SerializeField] private Color _normalColor = Color.white;

        // State
        private LeaderboardType _currentType = LeaderboardType.Daily;
        private List<LeaderboardEntryUI> _entries = new List<LeaderboardEntryUI>();

        // Events
        public event System.Action OnBackClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            ShowLeaderboard(LeaderboardType.Daily);
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_dailyTabButton != null)
                _dailyTabButton.onClick.AddListener(() => ShowLeaderboard(LeaderboardType.Daily));

            if (_weeklyTabButton != null)
                _weeklyTabButton.onClick.AddListener(() => ShowLeaderboard(LeaderboardType.Weekly));

            if (_allTimeTabButton != null)
                _allTimeTabButton.onClick.AddListener(() => ShowLeaderboard(LeaderboardType.AllTime));

            if (_refreshButton != null)
                _refreshButton.onClick.AddListener(RefreshLeaderboard);

            if (_backButton != null)
                _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        }
        #endregion

        #region Leaderboard Display
        private void ShowLeaderboard(LeaderboardType type)
        {
            _currentType = type;
            RefreshLeaderboard();
        }

        private void RefreshLeaderboard()
        {
            if (LeaderboardSystem.Instance == null)
            {
                ShowStatus("Leaderboard sistemi bulunamadi");
                return;
            }

            ShowLoading(true);

            // Leaderboard verilerini al
            LeaderboardSystem.Instance.GetLeaderboard(_currentType, OnLeaderboardReceived);
        }

        private void OnLeaderboardReceived(List<LeaderboardEntry> entries)
        {
            ShowLoading(false);
            PopulateEntries(entries);
            UpdatePlayerInfo();
        }

        private void PopulateEntries(List<LeaderboardEntry> entries)
        {
            if (_entryContainer == null || _entryPrefab == null) return;

            // Temizle
            foreach (Transform child in _entryContainer)
            {
                Destroy(child.gameObject);
            }
            _entries.Clear();

            if (entries == null || entries.Count == 0)
            {
                ShowStatus("Henuz skor yok");
                return;
            }

            string currentPlayerId = ProfileSystem.Instance?.PlayerId ?? "";

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                GameObject entryObj = Instantiate(_entryPrefab, _entryContainer);
                var entryUI = entryObj.GetComponent<LeaderboardEntryUI>();

                if (entryUI != null)
                {
                    bool isPlayer = entry.playerId == currentPlayerId;
                    Color color = GetRankColor(i + 1, isPlayer);
                    entryUI.Setup(i + 1, entry, color);
                    _entries.Add(entryUI);
                }
            }

            // Scroll'u en uste al
            if (_scrollRect != null)
            {
                _scrollRect.normalizedPosition = new Vector2(0, 1);
            }

            if (_statusText != null)
                _statusText.gameObject.SetActive(false);
        }

        private void UpdatePlayerInfo()
        {
            if (LeaderboardSystem.Instance == null) return;

            int playerRank = LeaderboardSystem.Instance.GetPlayerRank(_currentType);
            int playerScore = LeaderboardSystem.Instance.GetPlayerScore(_currentType);
            string playerName = ProfileSystem.Instance?.PlayerName ?? "Oyuncu";

            if (_playerRankText != null)
            {
                _playerRankText.text = playerRank > 0 ? $"#{playerRank}" : "-";
            }

            if (_playerScoreText != null)
            {
                _playerScoreText.text = playerScore.ToString();
            }

            if (_playerNameText != null)
            {
                _playerNameText.text = playerName;
            }
        }
        #endregion

        #region Helper Methods
        private Color GetRankColor(int rank, bool isPlayer)
        {
            if (isPlayer)
                return _playerHighlightColor;

            return rank switch
            {
                1 => _goldColor,
                2 => _silverColor,
                3 => _bronzeColor,
                _ => _normalColor
            };
        }

        private void ShowLoading(bool show)
        {
            if (_loadingIndicator != null)
                _loadingIndicator.SetActive(show);

            if (_refreshButton != null)
                _refreshButton.interactable = !show;
        }

        private void ShowStatus(string message)
        {
            if (_statusText != null)
            {
                _statusText.text = message;
                _statusText.gameObject.SetActive(true);
            }
        }
        #endregion

        #region Public Methods
        public void Show()
        {
            if (_leaderboardPanel != null)
                _leaderboardPanel.SetActive(true);

            ShowLeaderboard(_currentType);
        }

        public void Hide()
        {
            if (_leaderboardPanel != null)
                _leaderboardPanel.SetActive(false);
        }
        #endregion
    }

    /// <summary>
    /// Leaderboard entry UI helper (prefab uzerinde olmali)
    /// </summary>
    public class LeaderboardEntryUI : MonoBehaviour
    {
        [SerializeField] private Text _rankText;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Image _background;

        public void Setup(int rank, LeaderboardEntry entry, Color color)
        {
            if (_rankText != null)
                _rankText.text = $"#{rank}";

            if (_nameText != null)
                _nameText.text = entry.playerName;

            if (_scoreText != null)
                _scoreText.text = entry.score.ToString();

            if (_background != null)
                _background.color = color;
        }
    }

    #region Data Classes
    public enum LeaderboardType
    {
        Daily,
        Weekly,
        AllTime
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerId;
        public string playerName;
        public int score;
        public System.DateTime date;
    }
    #endregion
}
