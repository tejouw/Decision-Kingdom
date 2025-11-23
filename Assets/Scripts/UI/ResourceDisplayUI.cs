using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Managers;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Kaynak gosterge HUD komponenti
    /// </summary>
    public class ResourceDisplayUI : MonoBehaviour
    {
        [Header("Resource Bars")]
        [SerializeField] private Slider _goldSlider;
        [SerializeField] private Slider _happinessSlider;
        [SerializeField] private Slider _militarySlider;
        [SerializeField] private Slider _faithSlider;

        [Header("Resource Icons")]
        [SerializeField] private Image _goldIcon;
        [SerializeField] private Image _happinessIcon;
        [SerializeField] private Image _militaryIcon;
        [SerializeField] private Image _faithIcon;

        [Header("Resource Texts (Optional)")]
        [SerializeField] private Text _goldText;
        [SerializeField] private Text _happinessText;
        [SerializeField] private Text _militaryText;
        [SerializeField] private Text _faithText;

        [Header("Preview Indicators")]
        [SerializeField] private Image _goldPreviewArrow;
        [SerializeField] private Image _happinessPreviewArrow;
        [SerializeField] private Image _militaryPreviewArrow;
        [SerializeField] private Image _faithPreviewArrow;

        [Header("Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _lowColor = Color.red;
        [SerializeField] private Color _highColor = Color.yellow;
        [SerializeField] private Color _previewIncreaseColor = Color.green;
        [SerializeField] private Color _previewDecreaseColor = Color.red;

        [Header("Thresholds")]
        [SerializeField] private int _lowThreshold = 20;
        [SerializeField] private int _highThreshold = 80;

        [Header("Animation")]
        [SerializeField] private float _updateSpeed = 5f;
        [SerializeField] private bool _animateChanges = true;

        // Targets for smooth animation
        private float _targetGold;
        private float _targetHappiness;
        private float _targetMilitary;
        private float _targetFaith;

        #region Unity Lifecycle
        private void Start()
        {
            // ResourceManager event'lerine abone ol
            if (ResourceManager.Instance != null)
            {
                ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
                ResourceManager.Instance.OnResourcePreview += HandleResourcePreview;
                ResourceManager.Instance.OnPreviewCleared += HandlePreviewCleared;

                // Baslangic degerlerini ayarla
                InitializeValues();
            }

            // Preview ok'larini gizle
            HideAllPreviews();
        }

        private void Update()
        {
            if (_animateChanges)
            {
                AnimateSliders();
            }
        }

        private void OnDestroy()
        {
            if (ResourceManager.Instance != null)
            {
                ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
                ResourceManager.Instance.OnResourcePreview -= HandleResourcePreview;
                ResourceManager.Instance.OnPreviewCleared -= HandlePreviewCleared;
            }
        }
        #endregion

        #region Initialization
        private void InitializeValues()
        {
            if (ResourceManager.Instance == null) return;

            var resources = ResourceManager.Instance.CurrentResources;

            _targetGold = resources.Gold;
            _targetHappiness = resources.Happiness;
            _targetMilitary = resources.Military;
            _targetFaith = resources.Faith;

            // Immediate set
            if (_goldSlider != null) _goldSlider.value = _targetGold;
            if (_happinessSlider != null) _happinessSlider.value = _targetHappiness;
            if (_militarySlider != null) _militarySlider.value = _targetMilitary;
            if (_faithSlider != null) _faithSlider.value = _targetFaith;

            UpdateTexts();
            UpdateColors();
        }
        #endregion

        #region Event Handlers
        private void HandleResourceChanged(ResourceType type, int oldValue, int newValue)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    _targetGold = newValue;
                    break;
                case ResourceType.Happiness:
                    _targetHappiness = newValue;
                    break;
                case ResourceType.Military:
                    _targetMilitary = newValue;
                    break;
                case ResourceType.Faith:
                    _targetFaith = newValue;
                    break;
            }

            if (!_animateChanges)
            {
                UpdateSliderImmediate(type, newValue);
            }

            UpdateTexts();
            UpdateColors();
        }

        private void HandleResourcePreview(ResourceType type, int amount)
        {
            ShowPreviewForResource(type, amount);
        }

        private void HandlePreviewCleared()
        {
            HideAllPreviews();
        }
        #endregion

        #region Animation
        private void AnimateSliders()
        {
            float speed = _updateSpeed * Time.deltaTime;

            if (_goldSlider != null)
                _goldSlider.value = Mathf.Lerp(_goldSlider.value, _targetGold, speed);

            if (_happinessSlider != null)
                _happinessSlider.value = Mathf.Lerp(_happinessSlider.value, _targetHappiness, speed);

            if (_militarySlider != null)
                _militarySlider.value = Mathf.Lerp(_militarySlider.value, _targetMilitary, speed);

            if (_faithSlider != null)
                _faithSlider.value = Mathf.Lerp(_faithSlider.value, _targetFaith, speed);
        }

        private void UpdateSliderImmediate(ResourceType type, int value)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    if (_goldSlider != null) _goldSlider.value = value;
                    break;
                case ResourceType.Happiness:
                    if (_happinessSlider != null) _happinessSlider.value = value;
                    break;
                case ResourceType.Military:
                    if (_militarySlider != null) _militarySlider.value = value;
                    break;
                case ResourceType.Faith:
                    if (_faithSlider != null) _faithSlider.value = value;
                    break;
            }
        }
        #endregion

        #region UI Updates
        private void UpdateTexts()
        {
            if (_goldText != null) _goldText.text = _targetGold.ToString("F0");
            if (_happinessText != null) _happinessText.text = _targetHappiness.ToString("F0");
            if (_militaryText != null) _militaryText.text = _targetMilitary.ToString("F0");
            if (_faithText != null) _faithText.text = _targetFaith.ToString("F0");
        }

        private void UpdateColors()
        {
            UpdateIconColor(_goldIcon, _targetGold);
            UpdateIconColor(_happinessIcon, _targetHappiness);
            UpdateIconColor(_militaryIcon, _targetMilitary);
            UpdateIconColor(_faithIcon, _targetFaith);
        }

        private void UpdateIconColor(Image icon, float value)
        {
            if (icon == null) return;

            if (value <= _lowThreshold)
            {
                icon.color = _lowColor;
            }
            else if (value >= _highThreshold)
            {
                icon.color = _highColor;
            }
            else
            {
                icon.color = _normalColor;
            }
        }
        #endregion

        #region Preview
        private void ShowPreviewForResource(ResourceType type, int amount)
        {
            Image arrow = GetPreviewArrow(type);
            if (arrow == null) return;

            arrow.gameObject.SetActive(true);

            // Yonu ve rengi ayarla
            if (amount > 0)
            {
                arrow.color = _previewIncreaseColor;
                arrow.transform.localRotation = Quaternion.Euler(0, 0, 0); // Yukari
            }
            else if (amount < 0)
            {
                arrow.color = _previewDecreaseColor;
                arrow.transform.localRotation = Quaternion.Euler(0, 0, 180); // Asagi
            }
        }

        private void HideAllPreviews()
        {
            if (_goldPreviewArrow != null) _goldPreviewArrow.gameObject.SetActive(false);
            if (_happinessPreviewArrow != null) _happinessPreviewArrow.gameObject.SetActive(false);
            if (_militaryPreviewArrow != null) _militaryPreviewArrow.gameObject.SetActive(false);
            if (_faithPreviewArrow != null) _faithPreviewArrow.gameObject.SetActive(false);
        }

        private Image GetPreviewArrow(ResourceType type)
        {
            return type switch
            {
                ResourceType.Gold => _goldPreviewArrow,
                ResourceType.Happiness => _happinessPreviewArrow,
                ResourceType.Military => _militaryPreviewArrow,
                ResourceType.Faith => _faithPreviewArrow,
                _ => null
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Kaynaklari aninda guncelle
        /// </summary>
        public void RefreshImmediate()
        {
            if (ResourceManager.Instance == null) return;

            var resources = ResourceManager.Instance.CurrentResources;

            _targetGold = resources.Gold;
            _targetHappiness = resources.Happiness;
            _targetMilitary = resources.Military;
            _targetFaith = resources.Faith;

            if (_goldSlider != null) _goldSlider.value = _targetGold;
            if (_happinessSlider != null) _happinessSlider.value = _targetHappiness;
            if (_militarySlider != null) _militarySlider.value = _targetMilitary;
            if (_faithSlider != null) _faithSlider.value = _targetFaith;

            UpdateTexts();
            UpdateColors();
        }

        /// <summary>
        /// Animasyonu ac/kapat
        /// </summary>
        public void SetAnimated(bool animated)
        {
            _animateChanges = animated;
        }
        #endregion
    }
}
