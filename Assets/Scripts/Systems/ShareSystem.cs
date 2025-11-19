using System;
using System.Text;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Oyun sonucu paylasim sistemi
    /// Faz 6: Sosyal Ozellikler
    /// </summary>
    public class ShareSystem : MonoBehaviour
    {
        public static ShareSystem Instance { get; private set; }

        // Events
        public event Action<string> OnShareGenerated;
        public event Action<bool> OnShareCompleted;

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
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Oyun sonucu icin paylasilabilir metin olustur
        /// </summary>
        public string GenerateShareText(GameStateData gameState, EndingType endingType, int startGold = 50, int startHappiness = 50, int startMilitary = 50, int startFaith = 50)
        {
            var sb = new StringBuilder();

            // Baslik
            sb.AppendLine("Decision Kingdom");
            sb.AppendLine($"{DateTime.Today:yyyy-MM-dd}");
            sb.AppendLine();

            // Kaynak degisimleri
            sb.AppendLine(FormatResourceChange("Para", startGold, gameState.resources.Gold));
            sb.AppendLine(FormatResourceChange("Mutluluk", startHappiness, gameState.resources.Happiness));
            sb.AppendLine(FormatResourceChange("Askeri", startMilitary, gameState.resources.Military));
            sb.AppendLine(FormatResourceChange("Inanc", startFaith, gameState.resources.Faith));
            sb.AppendLine();

            // Sonuc
            sb.AppendLine($"Tur: {gameState.turn}");

            // Son tipi (spoiler-free)
            string endingCategory = GetEndingCategory(endingType);
            sb.AppendLine($"Son: {endingCategory}");

            // Donem
            sb.AppendLine($"Donem: {GetEraName(gameState.era)}");

            // Daily challenge ise
            if (DailyChallengeSystem.Instance != null && DailyChallengeSystem.Instance.CurrentChallenge != null)
            {
                sb.AppendLine();
                sb.AppendLine($"Gunluk Challenge #{DailyChallengeSystem.Instance.GetTodaySeed() % 10000}");
            }

            string shareText = sb.ToString();
            OnShareGenerated?.Invoke(shareText);

            return shareText;
        }

        /// <summary>
        /// Detayli sonuc karti olustur
        /// </summary>
        public string GenerateDetailedCard(GameStateData gameState, EndingType endingType, int score = 0)
        {
            var sb = new StringBuilder();

            // Baslik
            sb.AppendLine("=== DECISION KINGDOM ===");
            sb.AppendLine();

            // Ending bilgisi
            var ending = EndingSystem.GetEnding(endingType);
            if (ending != null)
            {
                sb.AppendLine(ending.title);
                sb.AppendLine(ending.description);
                sb.AppendLine();
            }

            // Istatistikler
            sb.AppendLine("--- ISTATISTIKLER ---");
            sb.AppendLine($"Tur: {gameState.turn}");
            sb.AppendLine($"Donem: {GetEraName(gameState.era)}");
            sb.AppendLine();

            // Final kaynaklar
            sb.AppendLine("--- KAYNAKLAR ---");
            sb.AppendLine($"Para: {gameState.resources.Gold}");
            sb.AppendLine($"Mutluluk: {gameState.resources.Happiness}");
            sb.AppendLine($"Askeri: {gameState.resources.Military}");
            sb.AppendLine($"Inanc: {gameState.resources.Faith}");
            sb.AppendLine();

            // Skor
            if (score > 0)
            {
                sb.AppendLine($"SKOR: {score}");
                sb.AppendLine();
            }

            // Prestige
            int pp = gameState.CalculatePrestigePoints();
            sb.AppendLine($"Kazanilan PP: +{pp}");

            // Tarih
            sb.AppendLine();
            sb.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm}");

            return sb.ToString();
        }

        /// <summary>
        /// Emoji formatinda sonuc olustur
        /// </summary>
        public string GenerateEmojiCard(GameStateData gameState, EndingType endingType)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Decision Kingdom");
            sb.AppendLine($"{DateTime.Today:yyyy-MM-dd}");
            sb.AppendLine();

            // Emoji kaynak gosterimi
            sb.AppendLine($"Para: {GetResourceEmoji(gameState.resources.Gold)}");
            sb.AppendLine($"Mutluluk: {GetResourceEmoji(gameState.resources.Happiness)}");
            sb.AppendLine($"Askeri: {GetResourceEmoji(gameState.resources.Military)}");
            sb.AppendLine($"Inanc: {GetResourceEmoji(gameState.resources.Faith)}");
            sb.AppendLine();

            sb.AppendLine($"Tur: {gameState.turn}");

            // Zafer/Yenilgi
            bool isVictory = EndingSystem.IsVictory(endingType);
            sb.AppendLine(isVictory ? "ZAFER!" : "YENILGI");

            return sb.ToString();
        }

        /// <summary>
        /// Basarim paylasimi olustur
        /// </summary>
        public string GenerateAchievementShare(Achievement achievement)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Decision Kingdom");
            sb.AppendLine();
            sb.AppendLine("BASARIM ACILDI!");
            sb.AppendLine();
            sb.AppendLine(achievement.name);
            sb.AppendLine(achievement.description);
            sb.AppendLine();
            sb.AppendLine($"+{achievement.prestigeReward} PP");

            return sb.ToString();
        }

        /// <summary>
        /// Leaderboard siralamasini paylas
        /// </summary>
        public string GenerateLeaderboardShare(int rank, int score, int seed)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Decision Kingdom");
            sb.AppendLine($"Gunluk Challenge #{seed % 10000}");
            sb.AppendLine();
            sb.AppendLine($"#{rank}");
            sb.AppendLine($"Skor: {score}");
            sb.AppendLine();
            sb.AppendLine($"{DateTime.Today:yyyy-MM-dd}");

            return sb.ToString();
        }

        /// <summary>
        /// Metni panoya kopyala
        /// </summary>
        public void CopyToClipboard(string text)
        {
            GUIUtility.systemCopyBuffer = text;
            Debug.Log("[ShareSystem] Metin panoya kopyalandi.");
        }

        /// <summary>
        /// Native share dialog ac (mobil icin)
        /// </summary>
        public void ShareNative(string text, string subject = "Decision Kingdom")
        {
            // Unity'de native share icin NativeShare plugin veya
            // platform-specific kod gerekir
            // Simdilik clipboard kullaniyoruz

            CopyToClipboard(text);
            OnShareCompleted?.Invoke(true);

#if UNITY_ANDROID || UNITY_IOS
            // Native share implementation would go here
            // Using a plugin like NativeShare
            Debug.Log("[ShareSystem] Native share - clipboard kullanildi.");
#else
            Debug.Log("[ShareSystem] Desktop - clipboard kullanildi.");
#endif
        }

        /// <summary>
        /// Screenshot al ve paylas
        /// </summary>
        public void CaptureAndShare(string filename = "decision_kingdom_result.png")
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, filename);
            ScreenCapture.CaptureScreenshot(path);

            Debug.Log($"[ShareSystem] Screenshot kaydedildi: {path}");

            // Bir frame bekleyip paylasim yap
            StartCoroutine(ShareScreenshotDelayed(path));
        }

        private System.Collections.IEnumerator ShareScreenshotDelayed(string path)
        {
            yield return new WaitForEndOfFrame();

#if UNITY_ANDROID || UNITY_IOS
            // Native share with image
            Debug.Log($"[ShareSystem] Screenshot paylasiliyor: {path}");
#else
            Debug.Log($"[ShareSystem] Screenshot kaydedildi: {path}");
#endif

            OnShareCompleted?.Invoke(true);
        }
        #endregion

        #region Private Methods
        private string FormatResourceChange(string name, int start, int end)
        {
            string arrow = end > start ? "^" : (end < start ? "v" : "=");
            int change = end - start;
            string changeStr = change >= 0 ? $"+{change}" : change.ToString();

            return $"{name}: {start} -> {end} ({changeStr}) {arrow}";
        }

        private string GetEndingCategory(EndingType endingType)
        {
            // Spoiler-free kategori
            if (EndingSystem.IsVictory(endingType))
            {
                return "Zafer";
            }

            // Game Over kategorileri
            return endingType switch
            {
                EndingType.BankruptKingdom or EndingType.EconomicCollapse or EndingType.TaxRevolt => "Ekonomik Cokus",
                EndingType.PeasantRevolution or EndingType.NobleConspiracy or EndingType.CivilWar or EndingType.LazyDecline => "Sosyal Cokus",
                EndingType.ForeignInvasion or EndingType.MilitaryCoup or EndingType.BetrayalByGeneral or EndingType.DefeatInWar => "Askeri Cokus",
                EndingType.ReligiousChaos or EndingType.TheocraticTakeover or EndingType.Excommunication or EndingType.HereticalKing => "Dini Cokus",
                EndingType.PoisonedByAdvisor or EndingType.AssassinatedByHeir or EndingType.BetrayelByQueen or EndingType.MerchantScam => "Ihanet",
                _ => "Yenilgi"
            };
        }

        private string GetEraName(Era era)
        {
            return era switch
            {
                Era.Medieval => "Ortacag",
                Era.Renaissance => "Ronesans",
                Era.Industrial => "Sanayi Devrimi",
                Era.Modern => "Modern Donem",
                Era.Future => "Gelecek",
                _ => "Bilinmeyen"
            };
        }

        private string GetResourceEmoji(int value)
        {
            // Metin bazli temsil (emoji yerine)
            if (value >= 80) return "[#####]";
            if (value >= 60) return "[####-]";
            if (value >= 40) return "[###--]";
            if (value >= 20) return "[##---]";
            return "[#----]";
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Test Share Text")]
        private void DebugTestShareText()
        {
            var testState = new GameStateData();
            testState.turn = 42;
            testState.era = Era.Medieval;
            testState.resources = new Resources(65, 45, 30, 70);

            string text = GenerateShareText(testState, EndingType.GoldenAge);
            Debug.Log(text);
            CopyToClipboard(text);
        }

        [ContextMenu("Test Detailed Card")]
        private void DebugTestDetailedCard()
        {
            var testState = new GameStateData();
            testState.turn = 75;
            testState.era = Era.Renaissance;
            testState.resources = new Resources(80, 60, 55, 70);

            string text = GenerateDetailedCard(testState, EndingType.ArtisticLegacy, 2500);
            Debug.Log(text);
            CopyToClipboard(text);
        }

        [ContextMenu("Test Emoji Card")]
        private void DebugTestEmojiCard()
        {
            var testState = new GameStateData();
            testState.turn = 30;
            testState.era = Era.Medieval;
            testState.resources = new Resources(20, 80, 50, 40);

            string text = GenerateEmojiCard(testState, EndingType.BankruptKingdom);
            Debug.Log(text);
            CopyToClipboard(text);
        }
#endif
        #endregion
    }
}
