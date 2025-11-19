using System;

namespace DecisionKingdom.Core
{
    /// <summary>
    /// Oyundaki tarihsel dönemler
    /// </summary>
    public enum Era
    {
        Medieval,       // Ortaçağ
        Renaissance,    // Rönesans
        Industrial,     // Sanayi Devrimi
        Modern,         // Modern Dönem
        Future          // Gelecek
    }

    /// <summary>
    /// Event kategorileri
    /// </summary>
    public enum EventCategory
    {
        Story,      // Ana hikaye olayları
        Random,     // Rastgele krizler
        Character,  // Karakter spesifik
        Chain,      // Zincirleme olaylar
        Rare        // Nadir olaylar
    }

    /// <summary>
    /// Kaynak türleri
    /// </summary>
    public enum ResourceType
    {
        Gold,       // Para
        Happiness,  // Halk Memnuniyeti
        Military,   // Askeri Güç
        Faith       // İnanç/Etki
    }

    /// <summary>
    /// Oyun durumları
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    /// <summary>
    /// Game Over sebepleri
    /// </summary>
    public enum GameOverReason
    {
        None,
        // 0'a düşme sebepleri
        Bankruptcy,         // Gold = 0: İflas
        Revolution,         // Happiness = 0: Devrim
        Invasion,           // Military = 0: İstila
        Chaos,              // Faith = 0: Kaos
        // 100'e ulaşma sebepleri
        InflationCrisis,    // Gold = 100: Enflasyon krizi
        Laziness,           // Happiness = 100: Tembellik çöküşü
        MilitaryCoup,       // Military = 100: Askeri darbe
        Theocracy           // Faith = 100: Teokrasi
    }

    /// <summary>
    /// Koşul operatörleri
    /// </summary>
    public enum ConditionOperator
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }

    /// <summary>
    /// Koşul türleri
    /// </summary>
    public enum ConditionType
    {
        ResourceThreshold,      // Kaynak eşik kontrolü
        CharacterInteraction,   // Karakter etkileşim sayısı
        TurnCount,              // Tur sayısı kontrolü
        Flag,                   // Flag kontrolü
        Era                     // Dönem kontrolü
    }

    /// <summary>
    /// Swipe yönleri
    /// </summary>
    public enum SwipeDirection
    {
        None,
        Left,
        Right
    }
}
