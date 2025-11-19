namespace DecisionKingdom.Core
{
    /// <summary>
    /// Oyun sabitleri
    /// </summary>
    public static class Constants
    {
        // Kaynak limitleri
        public const int RESOURCE_MIN = 0;
        public const int RESOURCE_MAX = 100;
        public const int RESOURCE_DEFAULT = 50;

        // Swipe ayarları
        public const float SWIPE_THRESHOLD = 50f;
        public const float SWIPE_PREVIEW_THRESHOLD = 20f;
        public const float CARD_SWIPE_DURATION = 0.3f;

        // Zorluk kademeleri (tur bazlı)
        public const int EARLY_GAME_TURNS = 10;
        public const int MID_GAME_TURNS = 30;
        public const int LATE_GAME_TURNS = 50;

        // Efekt büyüklükleri
        public const int EARLY_GAME_EFFECT = 5;
        public const int MID_GAME_EFFECT = 10;
        public const int LATE_GAME_EFFECT = 15;
        public const int EXTREME_EFFECT = 25;

        // Animasyon süreleri
        public const float RESOURCE_ANIMATION_DURATION = 0.5f;
        public const float CARD_ENTER_DURATION = 0.4f;
        public const float CARD_EXIT_DURATION = 0.3f;

        // Nadir event şansı
        public const float RARE_EVENT_CHANCE = 0.001f; // %0.1

        // Save keys
        public const string SAVE_KEY_GAME_STATE = "DecisionKingdom_GameState";
        public const string SAVE_KEY_STATISTICS = "DecisionKingdom_Statistics";
        public const string SAVE_KEY_ACHIEVEMENTS = "DecisionKingdom_Achievements";
        public const string SAVE_KEY_UNLOCKS = "DecisionKingdom_Unlocks";
        public const string SAVE_KEY_PRESTIGE = "DecisionKingdom_Prestige";
        public const string SAVE_KEY_DAILY_CHALLENGE = "DecisionKingdom_DailyChallenge";
        public const string SAVE_KEY_PROFILE = "DecisionKingdom_Profile";
        public const string SAVE_KEY_LEADERBOARD = "DecisionKingdom_Leaderboard";

        // Türkçe kaynak isimleri
        public static readonly string[] RESOURCE_NAMES_TR = {
            "Para",
            "Halk Memnuniyeti",
            "Askeri Güç",
            "İnanç"
        };

        // İngilizce kaynak isimleri
        public static readonly string[] RESOURCE_NAMES_EN = {
            "Gold",
            "Happiness",
            "Military",
            "Faith"
        };

        // Game Over mesajları (Türkçe)
        public static readonly string[] GAME_OVER_MESSAGES_TR = {
            "",
            "Hazine boşaldı! Krallık iflas etti.",
            "Halk ayaklandı! Devrildın.",
            "Ordu çok zayıf! Düşman istila etti.",
            "Toplum çöktü! Kaos hakim.",
            "Enflasyon patladı! Ekonomi çöktü.",
            "Halk tembel oldu! Krallık çöktü.",
            "Generaller darbe yaptı!",
            "Din adamları kontrolü ele geçirdi!"
        };

        // ============================================
        // MONETIZATION CONSTANTS (Phase 7)
        // ============================================

        // Save keys for monetization
        public const string SAVE_KEY_PURCHASES = "DecisionKingdom_Purchases";
        public const string SAVE_KEY_COSMETICS = "DecisionKingdom_Cosmetics";
        public const string SAVE_KEY_AD_STATE = "DecisionKingdom_AdState";
        public const string SAVE_KEY_PREMIUM = "DecisionKingdom_Premium";

        // IAP Product IDs (for store integration)
        public const string IAP_ERA_RENAISSANCE = "com.decisionkingdom.era.renaissance";
        public const string IAP_ERA_INDUSTRIAL = "com.decisionkingdom.era.industrial";
        public const string IAP_ERA_MODERN = "com.decisionkingdom.era.modern";
        public const string IAP_ERA_FUTURE = "com.decisionkingdom.era.future";
        public const string IAP_ERA_BUNDLE = "com.decisionkingdom.era.bundle";
        public const string IAP_SCENARIO_YOUNG_HEIR = "com.decisionkingdom.scenario.youngheir";
        public const string IAP_SCENARIO_COUP_LEADER = "com.decisionkingdom.scenario.coupleader";
        public const string IAP_SCENARIO_RICH_MERCHANT = "com.decisionkingdom.scenario.richmerchant";
        public const string IAP_SCENARIO_BUNDLE = "com.decisionkingdom.scenario.bundle";
        public const string IAP_COSMETIC_CARDBACK1 = "com.decisionkingdom.cosmetic.cardback1";
        public const string IAP_COSMETIC_CARDBACK2 = "com.decisionkingdom.cosmetic.cardback2";
        public const string IAP_COSMETIC_THEMES = "com.decisionkingdom.cosmetic.themes";
        public const string IAP_COSMETIC_BUNDLE = "com.decisionkingdom.cosmetic.bundle";
        public const string IAP_AD_REMOVAL = "com.decisionkingdom.premium.adfree";
        public const string IAP_COMPLETE_BUNDLE = "com.decisionkingdom.premium.complete";

        // Product prices (USD)
        public const float PRICE_ERA_RENAISSANCE = 0.99f;
        public const float PRICE_ERA_INDUSTRIAL = 1.49f;
        public const float PRICE_ERA_MODERN = 1.99f;
        public const float PRICE_ERA_FUTURE = 2.99f;
        public const float PRICE_ERA_BUNDLE = 4.99f;
        public const float PRICE_SCENARIO = 0.99f;
        public const float PRICE_SCENARIO_BUNDLE = 1.99f;
        public const float PRICE_COSMETIC = 0.99f;
        public const float PRICE_COSMETIC_BUNDLE = 1.99f;
        public const float PRICE_AD_REMOVAL = 2.99f;
        public const float PRICE_COMPLETE_BUNDLE = 6.99f;

        // Ad Configuration
        public const int AD_INTERSTITIAL_FREQUENCY = 3;      // Her 3 game over'da bir
        public const int AD_COOLDOWN_SECONDS = 60;           // Reklamlar arası minimum süre
        public const int REWARDED_AD_DAILY_LIMIT = 5;        // Günlük ödüllü reklam limiti
        public const int RESOURCE_BOOST_AMOUNT = 10;         // Reklam ile kaynak artışı
        public const float PP_MULTIPLIER_AD = 2.0f;          // Reklam ile PP çarpanı

        // Ad Unit IDs (placeholder - replace with actual AdMob/Unity Ads IDs)
        public const string AD_UNIT_INTERSTITIAL_ANDROID = "ca-app-pub-xxxxx/interstitial-android";
        public const string AD_UNIT_INTERSTITIAL_IOS = "ca-app-pub-xxxxx/interstitial-ios";
        public const string AD_UNIT_REWARDED_ANDROID = "ca-app-pub-xxxxx/rewarded-android";
        public const string AD_UNIT_REWARDED_IOS = "ca-app-pub-xxxxx/rewarded-ios";

        // Cosmetic defaults
        public const int DEFAULT_CARD_BACK = 0;              // Default CardBackDesign
        public const int DEFAULT_UI_THEME = 0;               // Default UITheme

        // Revive settings
        public const int REVIVE_PER_SESSION_LIMIT = 1;       // Oturum başına yeniden canlanma limiti
        public const int REVIVE_RESOURCE_SET = 25;           // Yeniden canlanma sonrası kaynak değeri

        // Türkçe ürün isimleri
        public static readonly string[] IAP_PRODUCT_NAMES_TR = {
            "Rönesans Dönemi",
            "Sanayi Dönemi",
            "Modern Dönem",
            "Gelecek Dönemi",
            "Tüm Dönemler Paketi",
            "Genç Varis Senaryosu",
            "Darbe Lideri Senaryosu",
            "Zengin Tüccar Senaryosu",
            "Tüm Senaryolar Paketi",
            "Kart Arkası Paketi 1",
            "Kart Arkası Paketi 2",
            "UI Tema Paketi",
            "Tüm Kozmetik Paketi",
            "Reklamsız Oyna",
            "Komple Paket"
        };

        // İngilizce ürün isimleri
        public static readonly string[] IAP_PRODUCT_NAMES_EN = {
            "Renaissance Era",
            "Industrial Era",
            "Modern Era",
            "Future Era",
            "All Eras Bundle",
            "Young Heir Scenario",
            "Coup Leader Scenario",
            "Rich Merchant Scenario",
            "All Scenarios Bundle",
            "Card Back Pack 1",
            "Card Back Pack 2",
            "UI Theme Pack",
            "All Cosmetics Bundle",
            "Remove Ads",
            "Complete Bundle"
        };

        // Kart arkası tasarım isimleri (Türkçe)
        public static readonly string[] CARD_BACK_NAMES_TR = {
            "Varsayılan",
            "Kraliyet",
            "Ejderha",
            "Kelt",
            "Gotik",
            "Minimalist",
            "Altın",
            "Karanlık",
            "Çiçek",
            "Geometrik"
        };

        // UI tema isimleri (Türkçe)
        public static readonly string[] UI_THEME_NAMES_TR = {
            "Varsayılan",
            "Karanlık",
            "Minimalist",
            "Kraliyet",
            "Doğa",
            "Steampunk",
            "Gelecek"
        };
    }
}
