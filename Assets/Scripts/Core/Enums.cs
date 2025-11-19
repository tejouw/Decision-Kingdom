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
    /// Oyun sonu türleri (20+ benzersiz son)
    /// </summary>
    public enum EndingType
    {
        None,
        // === BAŞARISIZ SONLAR (Game Over) ===
        // Ekonomik Çöküşler
        BankruptKingdom,        // İflas: Hazine tamamen tükendi
        EconomicCollapse,       // Enflasyon krizi: Aşırı zenginlik
        TaxRevolt,              // Vergi isyanı: Köylüler ayaklandı

        // Sosyal Çöküşler
        PeasantRevolution,      // Halk devrimi: Mutluluk sıfırlandı
        NobleConspiracy,        // Soylu komplosu: Lordlar tahtı ele geçirdi
        CivilWar,               // İç savaş: Krallık bölündü
        LazyDecline,            // Tembellik çöküşü: Aşırı mutluluk

        // Askeri Çöküşler
        ForeignInvasion,        // Yabancı istila: Ordu yetersiz
        MilitaryCoup,           // Askeri darbe: Ordu çok güçlü
        BetrayalByGeneral,      // General ihaneti: Valerius darbe yaptı
        DefeatInWar,            // Savaşta yenilgi

        // Dini Çöküşler
        ReligiousChaos,         // Dini kaos: İnanç sıfırlandı
        TheocraticTakeover,     // Teokratik devralma: İnanç çok yüksek
        Excommunication,        // Aforoz: Kilise tarafından lanetlendi
        HereticalKing,          // Sapkın kral: Dini otoriteye karşı geldi

        // Karakter Bazlı Kötü Sonlar
        PoisonedByAdvisor,      // Danışman tarafından zehirlendi
        AssassinatedByHeir,     // Veliaht tarafından suikast
        BetrayelByQueen,        // Kraliçe ihaneti
        MerchantScam,           // Tüccar dolandırıcılığı

        // === BAŞARILI SONLAR (Victory) ===
        // Klasik Zaferler
        GoldenAge,              // Altın çağ: Dengeli ve uzun hükümdarlık
        PeacefulReign,          // Barışçıl hükümdarlık: Diplomasi zaferi
        MightyConqueror,        // Güçlü fatih: Askeri zafer
        HolyKingdom,            // Kutsal krallık: Dini zafer

        // Karakter Bazlı İyi Sonlar
        TrustedAdvisor,         // Sadık danışman: Marcus'la güçlü bağ
        WealthyAlliance,        // Zengin ittifak: Miriam'la ortaklık
        MilitaryGlory,          // Askeri şan: Valerius'la zafer
        DivineBlessing,         // İlahi kutsama: Rahip desteği
        RoyalLegacy,            // Kraliyet mirası: Güçlü veliaht
        LovingMarriage,         // Mutlu evlilik: Kraliçe ile uyum

        // Özel Sonlar
        DragonSlayer,           // Ejderha avcısı: Ejderhayı yendi
        GrailKeeper,            // Kase koruyucusu: Kutsal Kase'yi buldu
        TimeSaver,              // Zaman kurtarıcı: Geleceği değiştirdi
        LegendaryKing,          // Efsanevi kral: 100+ tur hayatta kaldı
        BalancedRuler,          // Dengeli hükümdar: Tüm kaynaklar 40-60

        // Nötr Sonlar
        NaturalDeath,           // Doğal ölüm: Yaşlılıktan vefat
        Abdication,             // Tahttan feragat: Kendi isteğiyle çekildi
        SuccessionCrisis        // Veraset krizi: Veliaht sorunu
    }

    /// <summary>
    /// Miras türleri
    /// </summary>
    public enum LegacyType
    {
        None,
        // Olumlu Miraslar
        WisdomLegacy,           // Bilgelik mirası
        WealthLegacy,           // Zenginlik mirası
        MilitaryLegacy,         // Askeri miras
        FaithLegacy,            // İnanç mirası
        DiplomacyLegacy,        // Diplomasi mirası

        // Olumsuz Miraslar
        TyrantLegacy,           // Tiran mirası
        WeakLegacy,             // Zayıf kral mirası
        HereticalLegacy,        // Sapkın miras
        CruelLegacy,            // Zalim miras
        GreedyLegacy            // Açgözlü miras
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
