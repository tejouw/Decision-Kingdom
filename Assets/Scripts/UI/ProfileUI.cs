using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Oyuncu profili UI komponenti
    /// </summary>
    public class ProfileUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _profilePanel;

        [Header("Player Info")]
        [SerializeField] private InputField _playerNameInput;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private Text _playerIdText;
        [SerializeField] private Text _memberSinceText;

        [Header("Avatar Selection")]
        [SerializeField] private Transform _avatarContainer;
        [SerializeField] private GameObject _avatarButtonPrefab;
        [SerializeField] private Sprite[] _availableAvatars;

        [Header("Stats Summary")]
        [SerializeField] private Text _totalGamesText;
        [SerializeField] private Text _winRateText;
        [SerializeField] private Text _totalPrestigeText;
        [SerializeField] private Text _achievementCountText;
        [SerializeField] private Text _bestStreakText;

        [Header("Title/Badge")]
        [SerializeField] private Text _currentTitleText;
        [SerializeField] private Transform _badgeContainer;
        [SerializeField] private GameObject _badgePrefab;

        [Header("Buttons")]
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _linkAccountButton;
        [SerializeField] private Button _exportDataButton;

        // Events
        public event System.Action OnBackClicked;
        public event System.Action OnProfileSaved;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            LoadProfile();
            PopulateAvatars();
            UpdateStats();
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_saveButton != null)
                _saveButton.onClick.AddListener(SaveProfile);

            if (_backButton != null)
                _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

            if (_linkAccountButton != null)
                _linkAccountButton.onClick.AddListener(LinkAccount);

            if (_exportDataButton != null)
                _exportDataButton.onClick.AddListener(ExportData);

            if (_playerNameInput != null)
                _playerNameInput.onEndEdit.AddListener(OnNameChanged);
        }

        private void PopulateAvatars()
        {
            if (_avatarContainer == null || _avatarButtonPrefab == null || _availableAvatars == null) return;

            // Temizle
            foreach (Transform child in _avatarContainer)
            {
                Destroy(child.gameObject);
            }

            // Avatar butonlarini olustur
            for (int i = 0; i < _availableAvatars.Length; i++)
            {
                GameObject buttonObj = Instantiate(_avatarButtonPrefab, _avatarContainer);
                Button button = buttonObj.GetComponent<Button>();
                Image image = buttonObj.GetComponent<Image>();

                if (image != null && _availableAvatars[i] != null)
                {
                    image.sprite = _availableAvatars[i];
                }

                int avatarIndex = i;
                if (button != null)
                {
                    button.onClick.AddListener(() => SelectAvatar(avatarIndex));
                }
            }
        }
        #endregion

        #region Profile Management
        private void LoadProfile()
        {
            if (ProfileSystem.Instance == null) return;

            // Isim
            if (_playerNameInput != null)
            {
                _playerNameInput.text = ProfileSystem.Instance.PlayerName;
            }

            // Avatar
            int avatarIndex = ProfileSystem.Instance.AvatarIndex;
            if (_avatarImage != null && _availableAvatars != null && avatarIndex < _availableAvatars.Length)
            {
                _avatarImage.sprite = _availableAvatars[avatarIndex];
            }

            // Player ID
            if (_playerIdText != null)
            {
                _playerIdText.text = $"ID: {ProfileSystem.Instance.PlayerId}";
            }

            // Uyelik tarihi
            if (_memberSinceText != null)
            {
                var date = ProfileSystem.Instance.CreatedDate;
                _memberSinceText.text = $"Uye: {date:dd MMM yyyy}";
            }

            // Baslik
            if (_currentTitleText != null)
            {
                _currentTitleText.text = ProfileSystem.Instance.CurrentTitle;
            }
        }

        private void SaveProfile()
        {
            if (ProfileSystem.Instance == null) return;

            // Ismi kaydet
            if (_playerNameInput != null)
            {
                ProfileSystem.Instance.SetPlayerName(_playerNameInput.text);
            }

            OnProfileSaved?.Invoke();
            Debug.Log("[ProfileUI] Profil kaydedildi");

            // Kullaniciya bildir
            if (_saveButton != null)
            {
                var buttonText = _saveButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    StartCoroutine(ShowSaveConfirmation(buttonText));
                }
            }
        }

        private void OnNameChanged(string newName)
        {
            // Otomatik kaydetme opsiyonel
        }

        private void SelectAvatar(int index)
        {
            if (ProfileSystem.Instance == null) return;
            if (_availableAvatars == null || index >= _availableAvatars.Length) return;

            ProfileSystem.Instance.SetAvatar(index);

            if (_avatarImage != null)
            {
                _avatarImage.sprite = _availableAvatars[index];
            }

            Debug.Log($"[ProfileUI] Avatar secildi: {index}");
        }
        #endregion

        #region Stats
        private void UpdateStats()
        {
            if (StatisticsManager.Instance == null) return;

            var stats = StatisticsManager.Instance.LifetimeStats;
            if (stats == null) return;

            if (_totalGamesText != null)
                _totalGamesText.text = $"Oyunlar: {stats.totalGamesCompleted}";

            if (_winRateText != null)
            {
                float winRate = stats.totalGamesCompleted > 0
                    ? (float)stats.victories / stats.totalGamesCompleted * 100f
                    : 0f;
                _winRateText.text = $"Kazanma: %{winRate:F0}";
            }

            if (_totalPrestigeText != null)
            {
                int currentPP = PrestigeManager.Instance?.TotalPrestigePoints ?? 0;
                _totalPrestigeText.text = $"PP: {currentPP}";
            }

            if (_achievementCountText != null)
            {
                int unlocked = AchievementSystem.Instance?.UnlockedCount ?? 0;
                int total = AchievementSystem.Instance?.TotalCount ?? 0;
                _achievementCountText.text = $"Basari: {unlocked}/{total}";
            }

            if (_bestStreakText != null)
            {
                int bestStreak = DailyChallengeSystem.Instance?.BestStreak ?? 0;
                _bestStreakText.text = $"En Iyi Seri: {bestStreak}";
            }
        }
        #endregion

        #region Account
        private void LinkAccount()
        {
            // Hesap baglama - Social login, Google Play, Game Center vb.
            Debug.Log("[ProfileUI] Hesap baglama - Henuz implement edilmedi");
        }

        private void ExportData()
        {
            // GDPR uyumlulugu icin veri disa aktarma
            if (ProfileSystem.Instance != null)
            {
                string data = ProfileSystem.Instance.ExportAllData();
                Debug.Log($"[ProfileUI] Veri disari aktarildi:\n{data}");

                // Clipboard'a kopyala veya dosyaya kaydet
                GUIUtility.systemCopyBuffer = data;
            }
        }
        #endregion

        #region Public Methods
        public void Show()
        {
            if (_profilePanel != null)
                _profilePanel.SetActive(true);

            LoadProfile();
            UpdateStats();
        }

        public void Hide()
        {
            if (_profilePanel != null)
                _profilePanel.SetActive(false);
        }
        #endregion

        #region Coroutines
        private System.Collections.IEnumerator ShowSaveConfirmation(Text buttonText)
        {
            string originalText = buttonText.text;
            buttonText.text = "Kaydedildi!";

            yield return new WaitForSeconds(1.5f);

            buttonText.text = originalText;
        }
        #endregion
    }
}
