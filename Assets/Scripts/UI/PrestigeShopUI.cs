using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Prestige magaza UI komponenti
    /// Era ve senaryo unlock isleri
    /// </summary>
    public class PrestigeShopUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _erasTab;
        [SerializeField] private GameObject _scenariosTab;

        [Header("Tab Buttons")]
        [SerializeField] private Button _erasTabButton;
        [SerializeField] private Button _scenariosTabButton;

        [Header("Item Containers")]
        [SerializeField] private Transform _erasContainer;
        [SerializeField] private Transform _scenariosContainer;

        [Header("Prefabs")]
        [SerializeField] private GameObject _shopItemPrefab;

        [Header("Info Display")]
        [SerializeField] private Text _prestigePointsText;
        [SerializeField] private Text _selectedItemName;
        [SerializeField] private Text _selectedItemDescription;
        [SerializeField] private Text _selectedItemCost;
        [SerializeField] private Button _purchaseButton;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;

        [Header("Colors")]
        [SerializeField] private Color _unlockedColor = new Color(0.5f, 1f, 0.5f);
        [SerializeField] private Color _affordableColor = Color.white;
        [SerializeField] private Color _unaffordableColor = new Color(1f, 0.5f, 0.5f);

        // State
        private ShopItem _selectedItem;
        private List<ShopItemUI> _eraItems = new List<ShopItemUI>();
        private List<ShopItemUI> _scenarioItems = new List<ShopItemUI>();

        // Events
        public event System.Action OnBackClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            PopulateShop();
            ShowErasTab();
            UpdatePrestigeDisplay();
        }

        private void OnEnable()
        {
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.OnPrestigePointsChanged += HandlePrestigeChanged;
                PrestigeManager.Instance.OnEraUnlocked += HandleEraUnlocked;
                PrestigeManager.Instance.OnScenarioUnlocked += HandleScenarioUnlocked;
            }
        }

        private void OnDisable()
        {
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.OnPrestigePointsChanged -= HandlePrestigeChanged;
                PrestigeManager.Instance.OnEraUnlocked -= HandleEraUnlocked;
                PrestigeManager.Instance.OnScenarioUnlocked -= HandleScenarioUnlocked;
            }
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_erasTabButton != null)
                _erasTabButton.onClick.AddListener(ShowErasTab);

            if (_scenariosTabButton != null)
                _scenariosTabButton.onClick.AddListener(ShowScenariosTab);

            if (_purchaseButton != null)
                _purchaseButton.onClick.AddListener(PurchaseSelected);

            if (_backButton != null)
                _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        }

        private void PopulateShop()
        {
            PopulateEras();
            PopulateScenarios();
        }

        private void PopulateEras()
        {
            if (_erasContainer == null || _shopItemPrefab == null) return;

            // Temizle
            foreach (Transform child in _erasContainer)
            {
                Destroy(child.gameObject);
            }
            _eraItems.Clear();

            var eraInfos = PrestigeManager.Instance?.GetEraUnlockInfos();
            if (eraInfos == null) return;

            foreach (var info in eraInfos)
            {
                var itemUI = CreateShopItem(_erasContainer);
                if (itemUI == null) continue;

                var item = new ShopItem
                {
                    type = ShopItemType.Era,
                    id = info.era.ToString(),
                    name = info.name,
                    description = GetEraDescription(info.era),
                    cost = info.cost,
                    isUnlocked = info.isUnlocked
                };

                itemUI.Setup(item, this);
                _eraItems.Add(itemUI);
            }
        }

        private void PopulateScenarios()
        {
            if (_scenariosContainer == null || _shopItemPrefab == null) return;

            // Temizle
            foreach (Transform child in _scenariosContainer)
            {
                Destroy(child.gameObject);
            }
            _scenarioItems.Clear();

            var scenarioInfos = PrestigeManager.Instance?.GetScenarioUnlockInfos();
            if (scenarioInfos == null) return;

            foreach (var info in scenarioInfos)
            {
                var itemUI = CreateShopItem(_scenariosContainer);
                if (itemUI == null) continue;

                var item = new ShopItem
                {
                    type = ShopItemType.Scenario,
                    id = info.scenario.id,
                    name = info.scenario.name,
                    description = info.scenario.description,
                    cost = info.scenario.unlockCost,
                    isUnlocked = info.isUnlocked
                };

                itemUI.Setup(item, this);
                _scenarioItems.Add(itemUI);
            }
        }

        private ShopItemUI CreateShopItem(Transform parent)
        {
            GameObject obj = Instantiate(_shopItemPrefab, parent);
            return obj.GetComponent<ShopItemUI>();
        }
        #endregion

        #region Tab Management
        private void ShowErasTab()
        {
            if (_erasTab != null) _erasTab.SetActive(true);
            if (_scenariosTab != null) _scenariosTab.SetActive(false);

            ClearSelection();
        }

        private void ShowScenariosTab()
        {
            if (_erasTab != null) _erasTab.SetActive(false);
            if (_scenariosTab != null) _scenariosTab.SetActive(true);

            ClearSelection();
        }
        #endregion

        #region Selection
        public void SelectItem(ShopItem item)
        {
            _selectedItem = item;
            UpdateSelectionDisplay();
        }

        private void ClearSelection()
        {
            _selectedItem = null;
            UpdateSelectionDisplay();
        }

        private void UpdateSelectionDisplay()
        {
            if (_selectedItem == null)
            {
                if (_selectedItemName != null) _selectedItemName.text = "";
                if (_selectedItemDescription != null) _selectedItemDescription.text = "";
                if (_selectedItemCost != null) _selectedItemCost.text = "";
                if (_purchaseButton != null) _purchaseButton.gameObject.SetActive(false);
                return;
            }

            if (_selectedItemName != null)
                _selectedItemName.text = _selectedItem.name;

            if (_selectedItemDescription != null)
                _selectedItemDescription.text = _selectedItem.description;

            if (_selectedItemCost != null)
            {
                if (_selectedItem.isUnlocked)
                {
                    _selectedItemCost.text = "Acildi";
                    _selectedItemCost.color = _unlockedColor;
                }
                else
                {
                    _selectedItemCost.text = $"{_selectedItem.cost} PP";
                    bool canAfford = PrestigeManager.Instance != null &&
                                    PrestigeManager.Instance.TotalPrestigePoints >= _selectedItem.cost;
                    _selectedItemCost.color = canAfford ? _affordableColor : _unaffordableColor;
                }
            }

            if (_purchaseButton != null)
            {
                _purchaseButton.gameObject.SetActive(!_selectedItem.isUnlocked);
                bool canAfford = PrestigeManager.Instance != null &&
                                PrestigeManager.Instance.TotalPrestigePoints >= _selectedItem.cost;
                _purchaseButton.interactable = canAfford;
            }
        }
        #endregion

        #region Purchase
        private void PurchaseSelected()
        {
            if (_selectedItem == null || _selectedItem.isUnlocked) return;

            bool success = false;

            if (_selectedItem.type == ShopItemType.Era)
            {
                if (System.Enum.TryParse(_selectedItem.id, out Era era))
                {
                    success = PrestigeManager.Instance?.UnlockEra(era) ?? false;
                }
            }
            else if (_selectedItem.type == ShopItemType.Scenario)
            {
                success = PrestigeManager.Instance?.UnlockScenario(_selectedItem.id) ?? false;
            }

            if (success)
            {
                Debug.Log($"[PrestigeShopUI] Satin alindi: {_selectedItem.name}");
            }
        }
        #endregion

        #region Event Handlers
        private void HandlePrestigeChanged(int newValue)
        {
            UpdatePrestigeDisplay();
            UpdateSelectionDisplay();
            RefreshItems();
        }

        private void HandleEraUnlocked(Era era)
        {
            PopulateEras();
            ClearSelection();
        }

        private void HandleScenarioUnlocked(string scenarioId)
        {
            PopulateScenarios();
            ClearSelection();
        }

        private void RefreshItems()
        {
            foreach (var item in _eraItems)
            {
                item.Refresh();
            }
            foreach (var item in _scenarioItems)
            {
                item.Refresh();
            }
        }
        #endregion

        #region UI Updates
        private void UpdatePrestigeDisplay()
        {
            if (_prestigePointsText != null && PrestigeManager.Instance != null)
            {
                _prestigePointsText.text = $"PP: {PrestigeManager.Instance.TotalPrestigePoints}";
            }
        }

        private string GetEraDescription(Era era)
        {
            return era switch
            {
                Era.Medieval => "Ortacag donemi. Krallar, soylular ve kilise.",
                Era.Renaissance => "Aydinlanma cagi. Sanat ve bilim.",
                Era.Industrial => "Sanayi devrimi. Fabrikalar ve teknoloji.",
                Era.Modern => "Modern donem. Demokrasi ve medya.",
                Era.Future => "Gelecek. AI ve uzay kolonileri.",
                _ => ""
            };
        }
        #endregion

        #region Public Methods
        public void Show()
        {
            if (_shopPanel != null)
                _shopPanel.SetActive(true);

            PopulateShop();
            UpdatePrestigeDisplay();
        }

        public void Hide()
        {
            if (_shopPanel != null)
                _shopPanel.SetActive(false);
        }

        public Color GetItemColor(ShopItem item)
        {
            if (item.isUnlocked)
                return _unlockedColor;

            bool canAfford = PrestigeManager.Instance != null &&
                            PrestigeManager.Instance.TotalPrestigePoints >= item.cost;
            return canAfford ? _affordableColor : _unaffordableColor;
        }
        #endregion
    }

    #region Data Classes
    public enum ShopItemType
    {
        Era,
        Scenario
    }

    public class ShopItem
    {
        public ShopItemType type;
        public string id;
        public string name;
        public string description;
        public int cost;
        public bool isUnlocked;
    }

    /// <summary>
    /// Shop item UI helper (prefab uzerinde olmali)
    /// </summary>
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _costText;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;

        private ShopItem _item;
        private PrestigeShopUI _shop;

        public void Setup(ShopItem item, PrestigeShopUI shop)
        {
            _item = item;
            _shop = shop;

            if (_nameText != null)
                _nameText.text = item.name;

            if (_costText != null)
                _costText.text = item.isUnlocked ? "Acik" : $"{item.cost} PP";

            if (_background != null)
                _background.color = shop.GetItemColor(item);

            if (_button != null)
                _button.onClick.AddListener(() => shop.SelectItem(item));
        }

        public void Refresh()
        {
            if (_item == null || _shop == null) return;

            if (_background != null)
                _background.color = _shop.GetItemColor(_item);
        }
    }
    #endregion
}
