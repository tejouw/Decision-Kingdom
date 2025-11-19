using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Managers;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Kaynak bar UI komponenti
    /// </summary>
    public class ResourceBar : MonoBehaviour
    {
        [Header("Kaynak Türü")]
        [SerializeField] private ResourceType _resourceType;

        [Header("UI Referansları")]
        [SerializeField] private Image _fillImage;
        [SerializeField] private Text _valueText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _backgroundImage;

        [Header("Renk Ayarları")]
        [SerializeField] private Color _normalColor = Color.green;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _dangerColor = Color.red;
        [SerializeField] private Color _previewPositiveColor = new Color(0, 1, 0, 0.5f);
        [SerializeField] private Color _previewNegativeColor = new Color(1, 0, 0, 0.5f);

        [Header("Animasyon")]
        [SerializeField] private float _animationDuration = Constants.RESOURCE_ANIMATION_DURATION;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Uyarı Eşikleri")]
        [SerializeField] private int _warningThreshold = 25;
        [SerializeField] private int _dangerThreshold = 10;

        // State
        private float _currentDisplayValue;
        private float _targetValue;
        private Coroutine _animationCoroutine;
        private Coroutine _pulseCoroutine;
        private int _previewAmount;
        private bool _isShowingPreview;
        private bool _isPulsing;

        #region Properties
        public ResourceType ResourceType => _resourceType;
        public float CurrentValue => _currentDisplayValue;
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            // ResourceManager event'lerine abone ol
            if (ResourceManager.Instance != null)
            {
                ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
                ResourceManager.Instance.OnResourcePreview += HandleResourcePreview;
                ResourceManager.Instance.OnPreviewCleared += HandlePreviewCleared;

                // Başlangıç değerini al
                int initialValue = ResourceManager.Instance.GetResource(_resourceType);
                SetValueImmediate(initialValue);
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

        #region Public Methods
        /// <summary>
        /// Değeri animasyonlu olarak ayarla
        /// </summary>
        public void SetValue(int newValue)
        {
            _targetValue = newValue;

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animationCoroutine = StartCoroutine(AnimateValue(newValue));
        }

        /// <summary>
        /// Değeri hemen ayarla (animasyonsuz)
        /// </summary>
        public void SetValueImmediate(int newValue)
        {
            _currentDisplayValue = newValue;
            _targetValue = newValue;
            UpdateUI();
        }

        /// <summary>
        /// Önizleme göster
        /// </summary>
        public void ShowPreview(int amount)
        {
            _previewAmount = amount;
            _isShowingPreview = true;
            UpdateUI();
        }

        /// <summary>
        /// Önizlemeyi temizle
        /// </summary>
        public void ClearPreview()
        {
            _previewAmount = 0;
            _isShowingPreview = false;
            UpdateUI();
        }
        #endregion

        #region Private Methods
        private void HandleResourceChanged(ResourceType type, int oldValue, int newValue)
        {
            if (type == _resourceType)
            {
                SetValue(newValue);
            }
        }

        private void HandleResourcePreview(ResourceType type, int amount)
        {
            if (type == _resourceType)
            {
                ShowPreview(amount);
            }
        }

        private void HandlePreviewCleared()
        {
            ClearPreview();
        }

        private IEnumerator AnimateValue(float targetValue)
        {
            float startValue = _currentDisplayValue;
            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = _animationCurve.Evaluate(elapsed / _animationDuration);
                _currentDisplayValue = Mathf.Lerp(startValue, targetValue, t);
                UpdateUI();
                yield return null;
            }

            _currentDisplayValue = targetValue;
            UpdateUI();
            _animationCoroutine = null;
        }

        private void UpdateUI()
        {
            float normalizedValue = _currentDisplayValue / Constants.RESOURCE_MAX;

            // Fill bar güncelle
            if (_fillImage != null)
            {
                _fillImage.fillAmount = normalizedValue;
                _fillImage.color = GetColorForValue(_currentDisplayValue);
            }

            // Değer metni güncelle
            if (_valueText != null)
            {
                if (_isShowingPreview && _previewAmount != 0)
                {
                    string sign = _previewAmount > 0 ? "+" : "";
                    _valueText.text = $"{Mathf.RoundToInt(_currentDisplayValue)} ({sign}{_previewAmount})";
                    _valueText.color = _previewAmount > 0 ? _previewPositiveColor : _previewNegativeColor;
                }
                else
                {
                    _valueText.text = Mathf.RoundToInt(_currentDisplayValue).ToString();
                    _valueText.color = Color.white;
                }
            }

            // Arka plan efekti (tehlike durumunda)
            if (_backgroundImage != null)
            {
                bool isDanger = _currentDisplayValue <= _dangerThreshold ||
                               _currentDisplayValue >= (Constants.RESOURCE_MAX - _dangerThreshold);

                // Tehlike durumunda yanıp sönme efekti
                if (isDanger && !_isPulsing)
                {
                    StartPulse();
                }
                else if (!isDanger && _isPulsing)
                {
                    StopPulse();
                }
            }
        }

        private void StartPulse()
        {
            if (_pulseCoroutine != null)
                StopCoroutine(_pulseCoroutine);

            _isPulsing = true;
            _pulseCoroutine = StartCoroutine(PulseAnimation());
        }

        private void StopPulse()
        {
            if (_pulseCoroutine != null)
            {
                StopCoroutine(_pulseCoroutine);
                _pulseCoroutine = null;
            }

            _isPulsing = false;

            if (_backgroundImage != null)
            {
                Color color = _backgroundImage.color;
                color.a = 1f;
                _backgroundImage.color = color;
            }
        }

        private IEnumerator PulseAnimation()
        {
            float pulseSpeed = 3f;
            float minAlpha = 0.3f;
            float maxAlpha = 1f;

            while (_isPulsing)
            {
                float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

                if (_backgroundImage != null)
                {
                    Color color = _backgroundImage.color;
                    color.a = alpha;
                    _backgroundImage.color = color;
                }

                yield return null;
            }
        }

        private Color GetColorForValue(float value)
        {
            // Hem düşük hem yüksek değerler tehlikeli
            float distanceFromEdge = Mathf.Min(value, Constants.RESOURCE_MAX - value);

            if (distanceFromEdge <= _dangerThreshold)
            {
                return _dangerColor;
            }
            else if (distanceFromEdge <= _warningThreshold)
            {
                // Uyarı ve tehlike arasında geçiş
                float t = (distanceFromEdge - _dangerThreshold) / (_warningThreshold - _dangerThreshold);
                return Color.Lerp(_dangerColor, _warningColor, t);
            }
            else
            {
                // Uyarı ve normal arasında geçiş
                float t = (distanceFromEdge - _warningThreshold) / (50f - _warningThreshold);
                return Color.Lerp(_warningColor, _normalColor, t);
            }
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_fillImage != null && Application.isPlaying)
            {
                UpdateUI();
            }
        }
#endif
        #endregion
    }
}
