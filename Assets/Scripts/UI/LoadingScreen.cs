using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Yukleme ekrani yonetimi - Faz 8
    /// </summary>
    public class LoadingScreen : MonoBehaviour
    {
        public static LoadingScreen Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Text _loadingText;
        [SerializeField] private Text _tipText;
        [SerializeField] private Image _backgroundImage;

        [Header("Settings")]
        [SerializeField] private float _minimumLoadTime = 0.5f;
        [SerializeField] private float _fadeSpeed = 2f;

        [Header("Tips")]
        [SerializeField] private string[] _loadingTips;

        private CanvasGroup _canvasGroup;
        private bool _isLoading;

        // Events
        public event Action OnLoadingStarted;
        public event Action OnLoadingCompleted;

        #region Properties
        public bool IsLoading => _isLoading;
        public float Progress { get; private set; }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Start()
        {
            HideImmediate();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Yukleme ekranini goster
        /// </summary>
        public void Show(string message = null)
        {
            if (_loadingPanel != null)
            {
                _loadingPanel.SetActive(true);
            }

            SetLoadingText(message ?? GetLocalizedText("LOADING"));
            ShowRandomTip();
            Progress = 0f;
            UpdateProgressBar(0f);

            StartCoroutine(FadeIn());
            _isLoading = true;
            OnLoadingStarted?.Invoke();
        }

        /// <summary>
        /// Yukleme ekranini gizle
        /// </summary>
        public void Hide()
        {
            StartCoroutine(HideWithMinimumTime());
        }

        /// <summary>
        /// Aninda gizle
        /// </summary>
        public void HideImmediate()
        {
            if (_loadingPanel != null)
            {
                _loadingPanel.SetActive(false);
            }

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
            }

            _isLoading = false;
        }

        /// <summary>
        /// Ilerleme guncelle
        /// </summary>
        public void SetProgress(float progress)
        {
            Progress = Mathf.Clamp01(progress);
            UpdateProgressBar(Progress);
        }

        /// <summary>
        /// Yukleme metnini guncelle
        /// </summary>
        public void SetLoadingText(string text)
        {
            if (_loadingText != null)
            {
                _loadingText.text = text;
            }
        }

        /// <summary>
        /// Ipucu metnini guncelle
        /// </summary>
        public void SetTipText(string tip)
        {
            if (_tipText != null)
            {
                _tipText.text = tip;
            }
        }

        /// <summary>
        /// Async islem ile yukleme goster
        /// </summary>
        public Coroutine ShowWithOperation(AsyncOperation operation, string message = null)
        {
            return StartCoroutine(LoadWithOperation(operation, message));
        }

        /// <summary>
        /// Action ile yukleme goster
        /// </summary>
        public Coroutine ShowWithAction(Action action, string message = null)
        {
            return StartCoroutine(LoadWithAction(action, message));
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null && _loadingPanel != null)
            {
                _canvasGroup = _loadingPanel.GetComponent<CanvasGroup>();
            }

            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            InitializeTips();
        }

        private void InitializeTips()
        {
            if (_loadingTips == null || _loadingTips.Length == 0)
            {
                _loadingTips = new string[]
                {
                    "Dengede kalmak basarinin anahtaridir.",
                    "Karakterler kararlarinizi hatirlayacak.",
                    "Her kaynak 0'a veya 100'e ulasirsa oyun biter.",
                    "Kaydirirken kaynak degisimlerini onizleyebilirsiniz.",
                    "Nadir olaylar ozel odullere sahiptir.",
                    "Prestij puanlari yeni donemleri acar.",
                    "Gunluk maceralarda tum oyuncular ayni kartlari gorur.",
                    "Miras sistemi gelecek oyunlarinizi etkiler."
                };
            }
        }

        private void ShowRandomTip()
        {
            if (_tipText != null && _loadingTips != null && _loadingTips.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, _loadingTips.Length);
                _tipText.text = _loadingTips[index];
            }
        }

        private void UpdateProgressBar(float progress)
        {
            if (_progressBar != null)
            {
                _progressBar.value = progress;
            }
        }

        private string GetLocalizedText(string key)
        {
            if (Systems.LocalizationSystem.Instance != null)
            {
                return Systems.LocalizationSystem.Instance.GetLocalizedString(key);
            }
            return key == "LOADING" ? "Yukleniyor..." : key;
        }
        #endregion

        #region Coroutines
        private IEnumerator FadeIn()
        {
            if (_canvasGroup == null)
                yield break;

            float alpha = 0f;
            while (alpha < 1f)
            {
                alpha += Time.deltaTime * _fadeSpeed;
                _canvasGroup.alpha = alpha;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOut()
        {
            if (_canvasGroup == null)
                yield break;

            float alpha = 1f;
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * _fadeSpeed;
                _canvasGroup.alpha = alpha;
                yield return null;
            }
            _canvasGroup.alpha = 0f;

            if (_loadingPanel != null)
            {
                _loadingPanel.SetActive(false);
            }
        }

        private IEnumerator HideWithMinimumTime()
        {
            float elapsed = 0f;
            while (elapsed < _minimumLoadTime)
            {
                elapsed += Time.deltaTime;
                SetProgress(elapsed / _minimumLoadTime);
                yield return null;
            }

            yield return StartCoroutine(FadeOut());
            _isLoading = false;
            OnLoadingCompleted?.Invoke();
        }

        private IEnumerator LoadWithOperation(AsyncOperation operation, string message)
        {
            Show(message);

            while (!operation.isDone)
            {
                SetProgress(operation.progress);
                yield return null;
            }

            Hide();
        }

        private IEnumerator LoadWithAction(Action action, string message)
        {
            Show(message);

            yield return null;
            action?.Invoke();
            yield return null;

            Hide();
        }
        #endregion
    }
}
