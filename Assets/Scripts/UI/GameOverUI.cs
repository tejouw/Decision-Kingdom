using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Core;
using DecisionKingdom.Managers;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Game Over ekranı UI komponenti
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _reasonText;
        [SerializeField] private Text _turnCountText;
        [SerializeField] private Text _prestigePointsText;
        [SerializeField] private Text _resourceSummaryText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _shareButton;

        [Header("Animasyon")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration = 0.5f;

        #region Unity Lifecycle
        private void Awake()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);
        }

        private void Start()
        {
            // GameManager event'lerine abone ol
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver += HandleGameOver;
            }

            // Buton event'lerini bağla
            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartClicked);

            if (_mainMenuButton != null)
                _mainMenuButton.onClick.AddListener(OnMainMenuClicked);

            if (_shareButton != null)
                _shareButton.onClick.AddListener(OnShareClicked);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver -= HandleGameOver;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleGameOver(GameOverReason reason)
        {
            ShowGameOver(reason);
        }

        private void OnRestartClicked()
        {
            HideGameOver();
            GameManager.Instance?.StartNewGame();
        }

        private void OnMainMenuClicked()
        {
            HideGameOver();
            GameManager.Instance?.ReturnToMainMenu();
        }

        private void OnShareClicked()
        {
            // Share fonksiyonalitesi (Faz 6'da implement edilecek)
            Debug.Log("[GameOverUI] Share tıklandı");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Game Over ekranını göster
        /// </summary>
        public void ShowGameOver(GameOverReason reason)
        {
            if (_gameOverPanel == null) return;

            // UI'ı güncelle
            UpdateUI(reason);

            // Paneli göster
            _gameOverPanel.SetActive(true);

            // Fade in animasyonu
            if (_canvasGroup != null)
            {
                StartCoroutine(FadeIn());
            }
        }

        /// <summary>
        /// Game Over ekranını gizle
        /// </summary>
        public void HideGameOver()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);
        }
        #endregion

        #region Private Methods
        private void UpdateUI(GameOverReason reason)
        {
            // Başlık
            if (_titleText != null)
            {
                _titleText.text = reason == GameOverReason.None
                    ? "ZAFERİNİ İLAN ETTİN!"
                    : "KRALIKLIK ÇÖKTÜ!";
            }

            // Sebep
            if (_reasonText != null)
            {
                _reasonText.text = GetReasonText(reason);
            }

            // Tur sayısı
            if (_turnCountText != null && GameManager.Instance != null)
            {
                int turns = GameManager.Instance.CurrentTurn;
                _turnCountText.text = $"Hayatta Kalınan Tur: {turns}";
            }

            // Prestige puanı
            if (_prestigePointsText != null && GameManager.Instance != null)
            {
                int pp = GameManager.Instance.CalculatePrestigePoints();
                _prestigePointsText.text = $"Kazanılan PP: {pp}";
            }

            // Kaynak özeti
            if (_resourceSummaryText != null && ResourceManager.Instance != null)
            {
                var res = ResourceManager.Instance.CurrentResources;
                _resourceSummaryText.text = $"Para: {res.Gold} | Mutluluk: {res.Happiness}\n" +
                                           $"Askeri: {res.Military} | İnanç: {res.Faith}";
            }
        }

        private string GetReasonText(GameOverReason reason)
        {
            return reason switch
            {
                GameOverReason.None => "Tebrikler! Krallığını başarıyla yönettin!",
                GameOverReason.Bankruptcy => "Hazine tamamen boşaldı!\nKrallık iflas etti ve isyan başladı.",
                GameOverReason.Revolution => "Halk ayaklandı!\nSeni tahtından indirdiler.",
                GameOverReason.Invasion => "Ordu çok zayıf kaldı!\nDüşman krallığı istila etti.",
                GameOverReason.Chaos => "Toplumsal düzen çöktü!\nKaos ve anarşi hakim oldu.",
                GameOverReason.InflationCrisis => "Aşırı zenginlik enflasyona yol açtı!\nEkonomi çöktü.",
                GameOverReason.Laziness => "Halk fazla mutlu ve tembel oldu!\nKrallık çalışmayı bıraktı.",
                GameOverReason.MilitaryCoup => "Ordu çok güçlendi!\nGeneraller darbe yaptı.",
                GameOverReason.Theocracy => "Din adamları çok güçlendi!\nTeokrasi ilan edildi.",
                _ => "Bilinmeyen bir nedenle oyun sona erdi."
            };
        }

        private System.Collections.IEnumerator FadeIn()
        {
            _canvasGroup.alpha = 0f;

            float elapsed = 0f;
            while (elapsed < _fadeInDuration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = elapsed / _fadeInDuration;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }
        #endregion
    }
}
