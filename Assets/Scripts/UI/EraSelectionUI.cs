using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Systems;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Era ve Senaryo secim UI komponenti
    /// </summary>
    public class EraSelectionUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _eraSelectionPanel;
        [SerializeField] private GameObject _scenarioSelectionPanel;

        [Header("Era Selection")]
        [SerializeField] private Transform _eraButtonContainer;
        [SerializeField] private GameObject _eraButtonPrefab;

        [Header("Scenario Selection")]
        [SerializeField] private Transform _scenarioButtonContainer;
        [SerializeField] private GameObject _scenarioButtonPrefab;

        [Header("Info Display")]
        [SerializeField] private Text _selectedEraText;
        [SerializeField] private Text _selectedEraDescription;
        [SerializeField] private Text _selectedScenarioText;
        [SerializeField] private Text _selectedScenarioDescription;
        [SerializeField] private Text _startingResourcesText;
        [SerializeField] private Text _prestigePointsText;

        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _startButton;

        [Header("Colors")]
        [SerializeField] private Color _unlockedColor = Color.white;
        [SerializeField] private Color _lockedColor = Color.gray;
        [SerializeField] private Color _selectedColor = Color.yellow;

        // State
        private Era _selectedEra = Era.Medieval;
        private string _selectedScenarioId = "good_king";
        private List<Button> _eraButtons = new List<Button>();
        private List<Button> _scenarioButtons = new List<Button>();

        // Events
        public event System.Action<Era, string> OnStartGame;
        public event System.Action OnBackClicked;

        #region Unity Lifecycle
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            PopulateEras();
            PopulateScenarios();
            ShowEraSelection();
            UpdatePrestigeDisplay();
        }

        private void OnEnable()
        {
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.OnPrestigePointsChanged += HandlePrestigeChanged;
            }
        }

        private void OnDisable()
        {
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.OnPrestigePointsChanged -= HandlePrestigeChanged;
            }
        }
        #endregion

        #region Setup
        private void SetupButtons()
        {
            if (_backButton != null)
                _backButton.onClick.AddListener(OnBack);

            if (_nextButton != null)
                _nextButton.onClick.AddListener(OnNext);

            if (_startButton != null)
                _startButton.onClick.AddListener(OnStart);
        }

        private void PopulateEras()
        {
            if (_eraButtonContainer == null || _eraButtonPrefab == null) return;

            // Mevcut butonlari temizle
            foreach (Transform child in _eraButtonContainer)
            {
                Destroy(child.gameObject);
            }
            _eraButtons.Clear();

            // Era butonlarini olustur
            var eraInfos = PrestigeManager.Instance?.GetEraUnlockInfos();
            if (eraInfos == null) return;

            foreach (var info in eraInfos)
            {
                GameObject buttonObj = Instantiate(_eraButtonPrefab, _eraButtonContainer);
                Button button = buttonObj.GetComponent<Button>();
                Text buttonText = buttonObj.GetComponentInChildren<Text>();

                if (buttonText != null)
                {
                    string lockStatus = info.isUnlocked ? "" : $" ({info.cost} PP)";
                    buttonText.text = $"{info.name}{lockStatus}";
                }

                // Renk ayarla
                var colors = button.colors;
                if (!info.isUnlocked)
                {
                    colors.normalColor = _lockedColor;
                    button.interactable = info.canAfford;
                }
                button.colors = colors;

                // Click handler
                Era era = info.era;
                button.onClick.AddListener(() => SelectEra(era, info.isUnlocked, info.cost));

                _eraButtons.Add(button);
            }
        }

        private void PopulateScenarios()
        {
            if (_scenarioButtonContainer == null || _scenarioButtonPrefab == null) return;

            // Mevcut butonlari temizle
            foreach (Transform child in _scenarioButtonContainer)
            {
                Destroy(child.gameObject);
            }
            _scenarioButtons.Clear();

            // Senaryo butonlarini olustur
            var scenarioInfos = PrestigeManager.Instance?.GetScenarioUnlockInfos();
            if (scenarioInfos == null) return;

            foreach (var info in scenarioInfos)
            {
                GameObject buttonObj = Instantiate(_scenarioButtonPrefab, _scenarioButtonContainer);
                Button button = buttonObj.GetComponent<Button>();
                Text buttonText = buttonObj.GetComponentInChildren<Text>();

                if (buttonText != null)
                {
                    string lockStatus = info.isUnlocked ? "" : $" ({info.scenario.unlockCost} PP)";
                    buttonText.text = $"{info.scenario.name}{lockStatus}";
                }

                // Renk ayarla
                var colors = button.colors;
                if (!info.isUnlocked)
                {
                    colors.normalColor = _lockedColor;
                    button.interactable = info.canAfford;
                }
                button.colors = colors;

                // Click handler
                string scenarioId = info.scenario.id;
                button.onClick.AddListener(() => SelectScenario(scenarioId, info.isUnlocked, info.scenario.unlockCost));

                _scenarioButtons.Add(button);
            }
        }
        #endregion

        #region Selection
        private void SelectEra(Era era, bool isUnlocked, int cost)
        {
            if (!isUnlocked)
            {
                // Unlock dene
                if (PrestigeManager.Instance != null && PrestigeManager.Instance.UnlockEra(era))
                {
                    PopulateEras(); // Refresh
                    isUnlocked = true;
                }
                else
                {
                    Debug.Log($"[EraSelectionUI] Yetersiz PP: {cost} gerekli");
                    return;
                }
            }

            _selectedEra = era;
            UpdateEraInfo();

            Debug.Log($"[EraSelectionUI] Era secildi: {era}");
        }

        private void SelectScenario(string scenarioId, bool isUnlocked, int cost)
        {
            if (!isUnlocked)
            {
                // Unlock dene
                if (PrestigeManager.Instance != null && PrestigeManager.Instance.UnlockScenario(scenarioId))
                {
                    PopulateScenarios(); // Refresh
                    isUnlocked = true;
                }
                else
                {
                    Debug.Log($"[EraSelectionUI] Yetersiz PP: {cost} gerekli");
                    return;
                }
            }

            _selectedScenarioId = scenarioId;
            UpdateScenarioInfo();

            Debug.Log($"[EraSelectionUI] Senaryo secildi: {scenarioId}");
        }
        #endregion

        #region UI Updates
        private void UpdateEraInfo()
        {
            if (_selectedEraText != null)
            {
                _selectedEraText.text = PrestigeManager.EraNames.ContainsKey(_selectedEra)
                    ? PrestigeManager.EraNames[_selectedEra]
                    : _selectedEra.ToString();
            }

            if (_selectedEraDescription != null)
            {
                _selectedEraDescription.text = GetEraDescription(_selectedEra);
            }
        }

        private void UpdateScenarioInfo()
        {
            var scenario = PrestigeManager.Instance?.GetScenarioInfo(_selectedScenarioId);
            if (scenario == null) return;

            if (_selectedScenarioText != null)
            {
                _selectedScenarioText.text = scenario.name;
            }

            if (_selectedScenarioDescription != null)
            {
                _selectedScenarioDescription.text = scenario.description;
            }

            if (_startingResourcesText != null)
            {
                var res = scenario.startingResources;
                _startingResourcesText.text = $"Para: {res.Gold} | Mutluluk: {res.Happiness}\n" +
                                              $"Askeri: {res.Military} | Inanc: {res.Faith}";
            }
        }

        private void UpdatePrestigeDisplay()
        {
            if (_prestigePointsText != null && PrestigeManager.Instance != null)
            {
                _prestigePointsText.text = $"PP: {PrestigeManager.Instance.TotalPrestigePoints}";
            }
        }

        private void HandlePrestigeChanged(int newValue)
        {
            UpdatePrestigeDisplay();
            PopulateEras();
            PopulateScenarios();
        }

        private string GetEraDescription(Era era)
        {
            return era switch
            {
                Era.Medieval => "Ortacag donemi. Krallar, soylular ve kilise guc icin yarisir.",
                Era.Renaissance => "Aydinlanma cagi. Sanat, bilim ve ticaret yukseliste.",
                Era.Industrial => "Sanayi devrimi. Fabrikalar, isci haklari ve teknoloji.",
                Era.Modern => "Modern donem. Demokrasi, medya ve kuresellesme.",
                Era.Future => "Gelecek. Yapay zeka, uzay kolonileri ve bilinmeyen.",
                _ => ""
            };
        }
        #endregion

        #region Panel Navigation
        private void ShowEraSelection()
        {
            if (_eraSelectionPanel != null) _eraSelectionPanel.SetActive(true);
            if (_scenarioSelectionPanel != null) _scenarioSelectionPanel.SetActive(false);

            if (_nextButton != null) _nextButton.gameObject.SetActive(true);
            if (_startButton != null) _startButton.gameObject.SetActive(false);

            UpdateEraInfo();
        }

        private void ShowScenarioSelection()
        {
            if (_eraSelectionPanel != null) _eraSelectionPanel.SetActive(false);
            if (_scenarioSelectionPanel != null) _scenarioSelectionPanel.SetActive(true);

            if (_nextButton != null) _nextButton.gameObject.SetActive(false);
            if (_startButton != null) _startButton.gameObject.SetActive(true);

            UpdateScenarioInfo();
        }
        #endregion

        #region Button Handlers
        private void OnBack()
        {
            if (_scenarioSelectionPanel != null && _scenarioSelectionPanel.activeSelf)
            {
                ShowEraSelection();
            }
            else
            {
                OnBackClicked?.Invoke();
            }
        }

        private void OnNext()
        {
            ShowScenarioSelection();
        }

        private void OnStart()
        {
            Debug.Log($"[EraSelectionUI] Oyun baslatiliyor: {_selectedEra}, {_selectedScenarioId}");
            OnStartGame?.Invoke(_selectedEra, _selectedScenarioId);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// UI'yi goster
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            ShowEraSelection();
            UpdatePrestigeDisplay();
        }

        /// <summary>
        /// UI'yi gizle
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Secili era ve senaryoyu al
        /// </summary>
        public (Era era, string scenarioId) GetSelection()
        {
            return (_selectedEra, _selectedScenarioId);
        }
        #endregion
    }
}
