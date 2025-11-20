using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Achievement galeri UI komponenti
    /// </summary>
    public class AchievementUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _achievementPanel;

        [Header("Category Tabs")]
        [SerializeField] private Button _allTabButton;
        [SerializeField] private Button _survivalTabButton;
        [SerializeField] private Button _resourceTabButton;
        [SerializeField] private Button _characterTabButton;
        [SerializeField] private Button _storyTabButton;
        [SerializeField] private Button _specialTabButton;
        [SerializeField] private Button _secretTabButton;

        [Header("Achievement List")]
        [SerializeField] private Transform _achievementContainer;
        [SerializeField] private GameObject _achievementItemPrefab;
        [SerializeField] private ScrollRect _scrollRect;

        [Header("Info Display")]
        [SerializeField] private Text _progressText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private Text _selectedNameText;
        [SerializeField] private Text _selectedDescriptionText;
        [SerializeField] private Text _selectedRewardText;
        [SerializeField] private Image _selectedIcon;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;

        [Header("Colors")]
        [SerializeField] private Color _unlockedColor = new Color(0.5f, 1f, 0.5f);
        [SerializeField] private Color _lockedColor = new Color(0.5f, 0.5f, 0.5f);
        [SerializeField] private Color _secretColor = new Color(0.3f, 0.3f, 0.5f);

        // State
        private AchievementCategory _currentCategory = AchievementCategory.Survival;
        private Achievement _selectedAchievement;
        private List<AchievementItemUI> _items = new List<AchievementItemUI>();

        // Events
        public event System.Action OnBackClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            ShowCategory(AchievementCategory.Survival);
            UpdateProgress();
        }

        private void OnEnable()
        {
            if (AchievementSystem.Instance != null)
            {
                AchievementSystem.Instance.OnAchievementUnlocked += HandleAchievementUnlocked;
            }
        }

        private void OnDisable()
        {
            if (AchievementSystem.Instance != null)
            {
                AchievementSystem.Instance.OnAchievementUnlocked -= HandleAchievementUnlocked;
            }
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_allTabButton != null)
                _allTabButton.onClick.AddListener(() => ShowAll());

            if (_survivalTabButton != null)
                _survivalTabButton.onClick.AddListener(() => ShowCategory(AchievementCategory.Survival));

            if (_resourceTabButton != null)
                _resourceTabButton.onClick.AddListener(() => ShowCategory(AchievementCategory.Resource));

            if (_characterTabButton != null)
                _characterTabButton.onClick.AddListener(() => ShowCategory(AchievementCategory.Character));

            if (_storyTabButton != null)
                _storyTabButton.onClick.AddListener(() => ShowCategory(AchievementCategory.Story));

            if (_specialTabButton != null)
                _specialTabButton.onClick.AddListener(() => ShowCategory(AchievementCategory.Special));

            if (_secretTabButton != null)
                _secretTabButton.onClick.AddListener(() => ShowCategory(AchievementCategory.Secret));

            if (_backButton != null)
                _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        }
        #endregion

        #region Category Display
        private void ShowAll()
        {
            PopulateAchievements(null);
        }

        private void ShowCategory(AchievementCategory category)
        {
            _currentCategory = category;
            PopulateAchievements(category);
        }

        private void PopulateAchievements(AchievementCategory? category)
        {
            if (_achievementContainer == null || _achievementItemPrefab == null) return;

            // Temizle
            foreach (Transform child in _achievementContainer)
            {
                Destroy(child.gameObject);
            }
            _items.Clear();

            var achievements = category.HasValue
                ? AchievementSystem.Instance?.GetAchievementsByCategory(category.Value)
                : AchievementSystem.Instance?.GetAllAchievements();

            if (achievements == null) return;

            foreach (var achievement in achievements)
            {
                GameObject itemObj = Instantiate(_achievementItemPrefab, _achievementContainer);
                var itemUI = itemObj.GetComponent<AchievementItemUI>();

                if (itemUI != null)
                {
                    bool isUnlocked = AchievementSystem.Instance?.IsUnlocked(achievement.id) ?? false;
                    itemUI.Setup(achievement, isUnlocked, this);
                    _items.Add(itemUI);
                }
            }

            // Scroll'u en uste al
            if (_scrollRect != null)
            {
                _scrollRect.normalizedPosition = new Vector2(0, 1);
            }

            ClearSelection();
        }
        #endregion

        #region Selection
        public void SelectAchievement(Achievement achievement)
        {
            _selectedAchievement = achievement;
            UpdateSelectionDisplay();
        }

        private void ClearSelection()
        {
            _selectedAchievement = null;
            UpdateSelectionDisplay();
        }

        private void UpdateSelectionDisplay()
        {
            if (_selectedAchievement == null)
            {
                if (_selectedNameText != null) _selectedNameText.text = "";
                if (_selectedDescriptionText != null) _selectedDescriptionText.text = "Bir basari secin";
                if (_selectedRewardText != null) _selectedRewardText.text = "";
                if (_selectedIcon != null) _selectedIcon.gameObject.SetActive(false);
                return;
            }

            bool isUnlocked = AchievementSystem.Instance?.IsUnlocked(_selectedAchievement.id) ?? false;
            bool isSecret = _selectedAchievement.isSecret && !isUnlocked;

            if (_selectedNameText != null)
            {
                _selectedNameText.text = isSecret ? "???" : _selectedAchievement.name;
            }

            if (_selectedDescriptionText != null)
            {
                _selectedDescriptionText.text = isSecret
                    ? "Bu gizli bir basari. Acmak icin oyna!"
                    : _selectedAchievement.description;
            }

            if (_selectedRewardText != null)
            {
                _selectedRewardText.text = $"Odul: {_selectedAchievement.prestigeReward} PP";
            }

            if (_selectedIcon != null)
            {
                _selectedIcon.gameObject.SetActive(true);
                // Icon sprite burada set edilebilir
            }
        }
        #endregion

        #region Progress
        private void UpdateProgress()
        {
            if (AchievementSystem.Instance == null) return;

            int unlocked = AchievementSystem.Instance.UnlockedCount;
            int total = AchievementSystem.Instance.TotalCount;
            float progress = total > 0 ? (float)unlocked / total : 0f;

            if (_progressText != null)
            {
                _progressText.text = $"{unlocked} / {total} Basari";
            }

            if (_progressSlider != null)
            {
                _progressSlider.value = progress;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleAchievementUnlocked(Achievement achievement)
        {
            // Listeyi yenile
            if (_currentCategory == achievement.category || _items.Count > 0)
            {
                ShowCategory(_currentCategory);
            }

            UpdateProgress();
        }
        #endregion

        #region Public Methods
        public void Show()
        {
            if (_achievementPanel != null)
                _achievementPanel.SetActive(true);

            ShowCategory(AchievementCategory.Survival);
            UpdateProgress();
        }

        public void Hide()
        {
            if (_achievementPanel != null)
                _achievementPanel.SetActive(false);
        }

        public Color GetAchievementColor(Achievement achievement, bool isUnlocked)
        {
            if (isUnlocked)
                return _unlockedColor;

            if (achievement.isSecret)
                return _secretColor;

            return _lockedColor;
        }
        #endregion
    }

    /// <summary>
    /// Achievement item UI helper (prefab uzerinde olmali)
    /// </summary>
    public class AchievementItemUI : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _rewardText;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _lockIcon;
        [SerializeField] private Button _button;

        private Achievement _achievement;
        private bool _isUnlocked;
        private AchievementUI _ui;

        public void Setup(Achievement achievement, bool isUnlocked, AchievementUI ui)
        {
            _achievement = achievement;
            _isUnlocked = isUnlocked;
            _ui = ui;

            bool isSecret = achievement.isSecret && !isUnlocked;

            if (_nameText != null)
                _nameText.text = isSecret ? "???" : achievement.name;

            if (_rewardText != null)
                _rewardText.text = $"+{achievement.prestigeReward} PP";

            if (_lockIcon != null)
                _lockIcon.SetActive(!isUnlocked);

            if (_background != null)
                _background.color = ui.GetAchievementColor(achievement, isUnlocked);

            if (_button != null)
                _button.onClick.AddListener(() => ui.SelectAchievement(achievement));
        }
    }
}
