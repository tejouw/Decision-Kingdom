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
    }
}
