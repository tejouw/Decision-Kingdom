using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Coklu dil destegi sistemi - Faz 8.3
    /// </summary>
    public class LocalizationSystem : MonoBehaviour
    {
        public static LocalizationSystem Instance { get; private set; }

        [Header("Dil Ayarlari")]
        [SerializeField] private SystemLanguage _currentLanguage = SystemLanguage.Turkish;
        [SerializeField] private SystemLanguage _defaultLanguage = SystemLanguage.Turkish;

        // Events
        public event Action<SystemLanguage> OnLanguageChanged;

        private Dictionary<string, Dictionary<SystemLanguage, string>> _localizedStrings;
        private const string LANGUAGE_KEY = "SelectedLanguage";

        #region Properties
        public SystemLanguage CurrentLanguage => _currentLanguage;
        public SystemLanguage[] SupportedLanguages => new[] { SystemLanguage.Turkish, SystemLanguage.English };
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

            InitializeLocalization();
            LoadLanguagePreference();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Dil degistir
        /// </summary>
        public void SetLanguage(SystemLanguage language)
        {
            if (_currentLanguage != language)
            {
                _currentLanguage = language;
                SaveLanguagePreference();
                OnLanguageChanged?.Invoke(language);
                Debug.Log($"[LocalizationSystem] Dil degistirildi: {language}");
            }
        }

        /// <summary>
        /// Lokalize edilmis metni al
        /// </summary>
        public string GetLocalizedString(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            if (_localizedStrings.TryGetValue(key, out var translations))
            {
                if (translations.TryGetValue(_currentLanguage, out string value))
                {
                    return value;
                }
                // Fallback to default language
                if (translations.TryGetValue(_defaultLanguage, out string fallback))
                {
                    return fallback;
                }
            }

            Debug.LogWarning($"[LocalizationSystem] Cevrisi bulunamadi: {key}");
            return key;
        }

        /// <summary>
        /// Kisa yol - L() fonksiyonu
        /// </summary>
        public string L(string key)
        {
            return GetLocalizedString(key);
        }

        /// <summary>
        /// Parametreli lokalizasyon
        /// </summary>
        public string GetLocalizedString(string key, params object[] args)
        {
            string template = GetLocalizedString(key);
            try
            {
                return string.Format(template, args);
            }
            catch (FormatException ex)
            {
                Debug.LogWarning($"[LocalizationSystem] Format hatası '{key}': {ex.Message}. Args count: {args?.Length ?? 0}");
                return template;
            }
            catch (ArgumentNullException ex)
            {
                Debug.LogWarning($"[LocalizationSystem] Null argument '{key}': {ex.Message}");
                return template;
            }
        }

        /// <summary>
        /// Dil kodunu al
        /// </summary>
        public string GetLanguageCode()
        {
            return _currentLanguage switch
            {
                SystemLanguage.Turkish => "tr",
                SystemLanguage.English => "en",
                _ => "tr"
            };
        }

        /// <summary>
        /// Dil adini al
        /// </summary>
        public string GetLanguageName(SystemLanguage language)
        {
            return language switch
            {
                SystemLanguage.Turkish => "Turkce",
                SystemLanguage.English => "English",
                _ => language.ToString()
            };
        }

        /// <summary>
        /// Sistem dilini kullan
        /// </summary>
        public void UseSystemLanguage()
        {
            SystemLanguage systemLang = Application.systemLanguage;

            // Desteklenen dillerden biriyse kullan
            foreach (var supported in SupportedLanguages)
            {
                if (systemLang == supported)
                {
                    SetLanguage(systemLang);
                    return;
                }
            }

            // Desteklenmiyorsa default kullan
            SetLanguage(_defaultLanguage);
        }
        #endregion

        #region Private Methods
        private void InitializeLocalization()
        {
            _localizedStrings = new Dictionary<string, Dictionary<SystemLanguage, string>>();

            // ===== UI STRINGS =====
            AddLocalization("UI_PLAY", "Oyna", "Play");
            AddLocalization("UI_SETTINGS", "Ayarlar", "Settings");
            AddLocalization("UI_QUIT", "Cikis", "Quit");
            AddLocalization("UI_CONTINUE", "Devam Et", "Continue");
            AddLocalization("UI_NEW_GAME", "Yeni Oyun", "New Game");
            AddLocalization("UI_PAUSE", "Duraklat", "Pause");
            AddLocalization("UI_RESUME", "Devam", "Resume");
            AddLocalization("UI_MAIN_MENU", "Ana Menu", "Main Menu");
            AddLocalization("UI_RESTART", "Yeniden Baslat", "Restart");
            AddLocalization("UI_YES", "Evet", "Yes");
            AddLocalization("UI_NO", "Hayir", "No");
            AddLocalization("UI_OK", "Tamam", "OK");
            AddLocalization("UI_CANCEL", "Iptal", "Cancel");
            AddLocalization("UI_BACK", "Geri", "Back");
            AddLocalization("UI_CLOSE", "Kapat", "Close");
            AddLocalization("UI_SAVE", "Kaydet", "Save");
            AddLocalization("UI_LOAD", "Yukle", "Load");
            AddLocalization("UI_SHARE", "Paylas", "Share");
            AddLocalization("UI_ACHIEVEMENTS", "Basarimlar", "Achievements");
            AddLocalization("UI_STATISTICS", "Istatistikler", "Statistics");
            AddLocalization("UI_PROFILE", "Profil", "Profile");
            AddLocalization("UI_LEADERBOARD", "Siralamalar", "Leaderboard");
            AddLocalization("UI_DAILY_CHALLENGE", "Gunluk Macera", "Daily Challenge");
            AddLocalization("UI_STORE", "Magaza", "Store");
            AddLocalization("UI_COSMETICS", "Kozmetikler", "Cosmetics");

            // ===== RESOURCES =====
            AddLocalization("RESOURCE_GOLD", "Hazine", "Treasury");
            AddLocalization("RESOURCE_HAPPINESS", "Mutluluk", "Happiness");
            AddLocalization("RESOURCE_MILITARY", "Ordu", "Military");
            AddLocalization("RESOURCE_FAITH", "Inanc", "Faith");
            AddLocalization("RESOURCE_APPROVAL", "Onay Orani", "Approval Rating");
            AddLocalization("RESOURCE_TECHNOLOGY", "Teknoloji", "Technology");
            AddLocalization("RESOURCE_ETHICS", "Etik", "Ethics");

            // ===== ERAS =====
            AddLocalization("ERA_MEDIEVAL", "Ortacag", "Medieval");
            AddLocalization("ERA_RENAISSANCE", "Ronesans", "Renaissance");
            AddLocalization("ERA_INDUSTRIAL", "Sanayi Devrimi", "Industrial Revolution");
            AddLocalization("ERA_MODERN", "Modern Cag", "Modern Era");
            AddLocalization("ERA_FUTURE", "Gelecek", "Future");

            // ===== GAME OVER REASONS =====
            AddLocalization("GAMEOVER_BANKRUPTCY", "Iflas! Hazine tamamen tukendi.", "Bankruptcy! The treasury is empty.");
            AddLocalization("GAMEOVER_REVOLUTION", "Devrim! Halk ayaklandi.", "Revolution! The people have risen.");
            AddLocalization("GAMEOVER_INVASION", "Istila! Dusmalar ulkeyi ele gecirdi.", "Invasion! Enemies have conquered.");
            AddLocalization("GAMEOVER_CHAOS", "Kaos! Inanc sistemi coktü.", "Chaos! The faith system collapsed.");
            AddLocalization("GAMEOVER_INFLATION", "Enflasyon Krizi! Asiri zenginlik sistemi cokertti.", "Inflation Crisis! Excessive wealth crashed the system.");
            AddLocalization("GAMEOVER_LAZINESS", "Tembellik! Halk cok mutlu, kimse calismak istemiyor.", "Laziness! People too happy to work.");
            AddLocalization("GAMEOVER_COUP", "Askeri Darbe! Ordu kontrolu ele aldi.", "Military Coup! The army seized control.");
            AddLocalization("GAMEOVER_THEOCRACY", "Teokrasi! Din adamlari yonetimi devraldi.", "Theocracy! Religious leaders took over.");

            // ===== TUTORIAL =====
            AddLocalization("TUTORIAL_WELCOME_TITLE", "Kralliga Hosgeldiniz!", "Welcome to the Kingdom!");
            AddLocalization("TUTORIAL_WELCOME_MESSAGE", "Kralliginizi yonetmeyi ogrenelim. Devam etmek icin dokunun.", "Let's learn to rule your kingdom. Tap to continue.");
            AddLocalization("TUTORIAL_RESOURCES_TITLE", "Kaynaklar", "Resources");
            AddLocalization("TUTORIAL_RESOURCES_MESSAGE", "Bu cubuklar kralliginizin durumunu gosterir. Hicbiri 0'a dusmemeli veya 100'e ulasmamalı!", "These bars show your kingdom's status. None should reach 0 or 100!");
            AddLocalization("TUTORIAL_CARD_TITLE", "Kart Sistemi", "Card System");
            AddLocalization("TUTORIAL_CARD_MESSAGE", "Her kart bir olay veya karar getirir. Iki secenek icin saga veya sola kaydiracaksiniz.", "Each card brings an event or decision. Swipe left or right for two choices.");
            AddLocalization("TUTORIAL_SWIPE_LEFT_TITLE", "Sola Kaydir", "Swipe Left");
            AddLocalization("TUTORIAL_SWIPE_LEFT_MESSAGE", "Karti sola kaydirarak sol secenegi secin.", "Swipe the card left to choose the left option.");
            AddLocalization("TUTORIAL_SWIPE_RIGHT_TITLE", "Saga Kaydir", "Swipe Right");
            AddLocalization("TUTORIAL_SWIPE_RIGHT_MESSAGE", "Karti saga kaydirarak sag secenegi secin.", "Swipe the card right to choose the right option.");
            AddLocalization("TUTORIAL_PREVIEW_TITLE", "Etki Onizleme", "Effect Preview");
            AddLocalization("TUTORIAL_PREVIEW_MESSAGE", "Kaydirirken kaynak cubuklarinda degisimi gorebilirsiniz.", "While swiping, you can see the effect on resource bars.");
            AddLocalization("TUTORIAL_BALANCE_TITLE", "Denge Sanatı", "The Art of Balance");
            AddLocalization("TUTORIAL_BALANCE_MESSAGE", "Basarinin sirri dengedir. Tum kaynaklari guvenli aralikta tutun!", "Success lies in balance. Keep all resources in safe range!");
            AddLocalization("TUTORIAL_GAMEOVER_TITLE", "Oyun Sonu", "Game Over");
            AddLocalization("TUTORIAL_GAMEOVER_MESSAGE", "Herhangi bir kaynak 0 veya 100'e ulasirsa oyun biter. Her son farklidir!", "When any resource hits 0 or 100, the game ends. Each ending is different!");
            AddLocalization("TUTORIAL_CHARACTERS_TITLE", "Karakterler", "Characters");
            AddLocalization("TUTORIAL_CHARACTERS_MESSAGE", "Karakterler sizi hatirlayacak. Iliskiniz gelecek olaylari etkileyecek.", "Characters will remember you. Your relationship affects future events.");
            AddLocalization("TUTORIAL_COMPLETE_TITLE", "Hazirsiniz!", "You're Ready!");
            AddLocalization("TUTORIAL_COMPLETE_MESSAGE", "Artik kralliginizi yonetebilirsiniz. Bol sans!", "Now you can rule your kingdom. Good luck!");
            AddLocalization("TUTORIAL_SKIP", "Atla", "Skip");

            // ===== SETTINGS =====
            AddLocalization("SETTINGS_AUDIO", "Ses", "Audio");
            AddLocalization("SETTINGS_MASTER_VOLUME", "Ana Ses", "Master Volume");
            AddLocalization("SETTINGS_MUSIC_VOLUME", "Muzik", "Music");
            AddLocalization("SETTINGS_SFX_VOLUME", "Efektler", "Effects");
            AddLocalization("SETTINGS_LANGUAGE", "Dil", "Language");
            AddLocalization("SETTINGS_NOTIFICATIONS", "Bildirimler", "Notifications");
            AddLocalization("SETTINGS_HAPTICS", "Titresim", "Haptics");
            AddLocalization("SETTINGS_PRIVACY", "Gizlilik", "Privacy");
            AddLocalization("SETTINGS_CREDITS", "Yapimcilar", "Credits");
            AddLocalization("SETTINGS_VERSION", "Surum", "Version");
            AddLocalization("SETTINGS_RESET_PROGRESS", "Ilerlemeyi Sifirla", "Reset Progress");
            AddLocalization("SETTINGS_RESET_CONFIRM", "Tum ilerlemeniz silinecek. Emin misiniz?", "All progress will be deleted. Are you sure?");

            // ===== ACHIEVEMENTS =====
            AddLocalization("ACH_FIRST_GAME", "Ilk Adim", "First Step");
            AddLocalization("ACH_FIRST_GAME_DESC", "Ilk oyununuzu tamamlayin", "Complete your first game");
            AddLocalization("ACH_SURVIVE_10", "Yeni Baslayanlar", "Beginner");
            AddLocalization("ACH_SURVIVE_10_DESC", "10 tur hayatta kalin", "Survive 10 turns");
            AddLocalization("ACH_SURVIVE_50", "Deneyimli Hukumdar", "Experienced Ruler");
            AddLocalization("ACH_SURVIVE_50_DESC", "50 tur hayatta kalin", "Survive 50 turns");
            AddLocalization("ACH_SURVIVE_100", "Efsanevi Kral", "Legendary King");
            AddLocalization("ACH_SURVIVE_100_DESC", "100 tur hayatta kalin", "Survive 100 turns");
            AddLocalization("ACH_BALANCED", "Dengeci", "Balancer");
            AddLocalization("ACH_BALANCED_DESC", "Tum kaynaklari 40-60 arasinda tutarak oyunu bitirin", "End game with all resources between 40-60");

            // ===== MONETIZATION =====
            AddLocalization("IAP_RESTORE", "Satin Alimlari Geri Yukle", "Restore Purchases");
            AddLocalization("IAP_BUY", "Satin Al", "Buy");
            AddLocalization("IAP_OWNED", "Sahip", "Owned");
            AddLocalization("AD_WATCH", "Reklam Izle", "Watch Ad");
            AddLocalization("AD_REWARD_REVIVE", "Yeniden Canlan", "Revive");
            AddLocalization("AD_REWARD_BOOST", "+10 Kaynak", "+10 Resource");
            AddLocalization("AD_REWARD_DOUBLE_PP", "2x Prestij Puani", "2x Prestige Points");
            AddLocalization("AD_NO_THANKS", "Hayir, Tesekkurler", "No, Thanks");

            // ===== MISC =====
            AddLocalization("TURN", "Tur", "Turn");
            AddLocalization("PRESTIGE_POINTS", "Prestij Puani", "Prestige Points");
            AddLocalization("LOADING", "Yukleniyor...", "Loading...");
            AddLocalization("ERROR", "Hata", "Error");
            AddLocalization("SUCCESS", "Basarili", "Success");
            AddLocalization("CONFIRM", "Onayla", "Confirm");
            AddLocalization("NETWORK_ERROR", "Baglanti hatasi. Lutfen internet baglantinizi kontrol edin.", "Connection error. Please check your internet connection.");

            Debug.Log($"[LocalizationSystem] {_localizedStrings.Count} ceviri yuklendi");
        }

        private void AddLocalization(string key, string turkish, string english)
        {
            _localizedStrings[key] = new Dictionary<SystemLanguage, string>
            {
                { SystemLanguage.Turkish, turkish },
                { SystemLanguage.English, english }
            };
        }

        private void SaveLanguagePreference()
        {
            PlayerPrefs.SetInt(LANGUAGE_KEY, (int)_currentLanguage);
            PlayerPrefs.Save();
        }

        private void LoadLanguagePreference()
        {
            if (PlayerPrefs.HasKey(LANGUAGE_KEY))
            {
                _currentLanguage = (SystemLanguage)PlayerPrefs.GetInt(LANGUAGE_KEY);
            }
            else
            {
                // İlk kez - sistem dilini kullan
                UseSystemLanguage();
            }
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Switch to Turkish")]
        private void DebugSwitchTurkish()
        {
            SetLanguage(SystemLanguage.Turkish);
        }

        [ContextMenu("Switch to English")]
        private void DebugSwitchEnglish()
        {
            SetLanguage(SystemLanguage.English);
        }

        [ContextMenu("Print Sample Strings")]
        private void DebugPrintSamples()
        {
            Debug.Log($"UI_PLAY: {GetLocalizedString("UI_PLAY")}");
            Debug.Log($"RESOURCE_GOLD: {GetLocalizedString("RESOURCE_GOLD")}");
            Debug.Log($"ERA_MEDIEVAL: {GetLocalizedString("ERA_MEDIEVAL")}");
        }
#endif
        #endregion
    }
}
