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
        SuccessionCrisis,       // Veraset krizi: Veliaht sorunu

        // === RÖNESANS SONLARI ===
        // Zaferler
        ArtisticLegacy,         // Sanat mirası: Leonardo ile efsaneler yarattı
        ExplorerTriumph,        // Kaşif zaferi: Yeni dünya keşfedildi
        ScientificRevolution,   // Bilim devrimi: Galileo ile gerçek kazandı
        BankingEmpire,          // Bankacılık imparatorluğu: Medici ortaklığı
        // Yenilgiler
        InquisitionTyranny,     // Engizisyon tiranlığı: Düşünce özgürlüğü yok edildi
        PlagueDestruction,      // Veba yıkımı: Salgın her şeyi yok etti
        OttomanConquest,        // Osmanlı fethi: İstila edildi

        // === SANAYİ DEVRİMİ SONLARI ===
        // Zaferler
        IndustrialEmpire,       // Sanayi imparatorluğu: Fabrika imparatorluğu kuruldu
        WorkerChampion,         // İşçi şampiyonu: İşçi hakları savunuldu
        ColonialPower,          // Sömürge gücü: İmparatorluk genişledi
        InventorLegacy,         // Mucit mirası: Teknolojik yenilikler
        // Yenilgiler
        WorkerRevolution,       // İşçi devrimi: Proleterya ayaklandı
        ChildLaborShame,        // Çocuk işçi utancı: İnsanlık dışı uygulamalar
        EnvironmentCollapse,    // Çevre çöküşü: Endüstriyel kirlilik

        // === MODERN DÖNEM SONLARI ===
        // Zaferler
        MediaMaster,            // Medya ustası: Kamuoyunu yönetti
        TechUtopia,             // Teknoloji ütopyası: Dijital cennet
        GreenLeader,            // Yeşil lider: Çevre şampiyonu
        PandemicHero,           // Salgın kahramanı: Krizi yönetti
        DiplomaticGenius,       // Diplomatik dahi: Barış mimarı
        // Yenilgiler
        ImpeachmentShame,       // Azil utancı: Görevden alındı
        CyberWarDefeat,         // Siber savaş yenilgisi: Sistemler çöktü
        PopulistCollapse,       // Popülist çöküş: Vaatler tutulmadı

        // === GELECEK SONLARI ===
        // Zaferler
        SingularityAscension,   // Tekillik yükselişi: AI ile birleşim
        MarsFounder,            // Mars kurucusu: Çok gezegenli tür
        GeneticPerfection,      // Genetik mükemmellik: Evrim yönlendirildi
        DigitalImmortality,     // Dijital ölümsüzlük: Zihin yüklendi
        AlienAlliance,          // Uzaylı ittifakı: İlk temas başarılı
        HumanistChampion,       // Hümanist şampiyon: İnsanlık korundu
        // Yenilgiler
        AITakeover,             // AI ele geçirme: Makineler kazandı
        RobotUprising,          // Robot isyanı: Makineler ayaklandı
        ClimateExtinction,      // İklim yok oluşu: Gezegen yaşanmaz oldu
        TranshumanDystopia      // Transhüman distopyası: İnsanlık kaybedildi
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

    // ============================================
    // MONETIZATION ENUMS (Phase 7)
    // ============================================

    /// <summary>
    /// Satın alınabilir ürün türleri
    /// </summary>
    public enum IAPProductType
    {
        // Era Unlocks
        EraRenaissance,         // Rönesans dönemi açma ($0.99)
        EraIndustrial,          // Sanayi dönemi açma ($1.49)
        EraModern,              // Modern dönem açma ($1.99)
        EraFuture,              // Gelecek dönemi açma ($2.99)
        EraBundle,              // Tüm dönemler paketi ($4.99)

        // Scenarios
        ScenarioYoungHeir,      // Genç Varis senaryosu ($0.99)
        ScenarioCoupLeader,     // Darbe Lideri senaryosu ($0.99)
        ScenarioRichMerchant,   // Zengin Tüccar senaryosu ($0.99)
        ScenarioBundle,         // Tüm senaryolar paketi ($1.99)

        // Cosmetics
        CosmeticCardBackPack1,  // Kart arkası paketi 1 ($0.99)
        CosmeticCardBackPack2,  // Kart arkası paketi 2 ($0.99)
        CosmeticThemePack,      // UI tema paketi ($0.99)
        CosmeticBundle,         // Tüm kozmetik paketi ($1.99)

        // Premium
        AdRemoval,              // Reklam kaldırma ($2.99)
        CompleteBundle          // Komple paket ($6.99)
    }

    /// <summary>
    /// Reklam türleri
    /// </summary>
    public enum AdType
    {
        Interstitial,           // Geçiş reklamı (Game Over sonrası)
        RewardedRevive,         // Ödüllü: Yeniden canlanma
        RewardedResourceBoost,  // Ödüllü: Kaynak artışı
        RewardedDoublePP        // Ödüllü: 2x Prestige Point
    }

    /// <summary>
    /// Kart arkası tasarımları
    /// </summary>
    public enum CardBackDesign
    {
        Default,                // Varsayılan (ücretsiz)
        Royal,                  // Kraliyet motifi
        Dragon,                 // Ejderha motifi
        Celtic,                 // Kelt desenleri
        Gothic,                 // Gotik tarzı
        Minimalist,             // Minimalist tasarım
        Golden,                 // Altın tasarım
        Dark,                   // Karanlık tema
        Floral,                 // Çiçek desenleri
        Geometric               // Geometrik desenler
    }

    /// <summary>
    /// UI tema seçenekleri
    /// </summary>
    public enum UITheme
    {
        Default,                // Varsayılan (ücretsiz)
        Dark,                   // Karanlık tema
        Minimalist,             // Minimalist tema
        Royal,                  // Kraliyet teması
        Nature,                 // Doğa teması
        Steampunk,              // Steampunk tema
        Futuristic              // Gelecek teması
    }

    /// <summary>
    /// Karakter portre stilleri
    /// </summary>
    public enum PortraitStyle
    {
        Default,                // Varsayılan stil
        Classic,                // Klasik yağlı boya
        Medieval,               // Ortaçağ el yazması
        Renaissance,            // Rönesans dönemi
        Anime,                  // Anime çizim
        Realistic,              // Gerçekçi dijital
        Cartoon,                // Karikatür stili
        Pixel,                  // Pixel art
        Watercolor,             // Suluboya
        Noir                    // Film noir siyah-beyaz
    }

    /// <summary>
    /// Satın alma durumu
    /// </summary>
    public enum PurchaseState
    {
        NotPurchased,           // Satın alınmadı
        Pending,                // Beklemede
        Purchased,              // Satın alındı
        Failed,                 // Başarısız
        Restored                // Geri yüklendi
    }

    /// <summary>
    /// Reklam izleme durumu
    /// </summary>
    public enum AdWatchState
    {
        NotAvailable,           // Mevcut değil
        Available,              // İzlenebilir
        Loading,                // Yükleniyor
        Showing,                // Gösteriliyor
        Completed,              // Tamamlandı
        Skipped,                // Atlandı
        Failed                  // Başarısız
    }

    /// <summary>
    /// Premium üyelik durumu
    /// </summary>
    public enum PremiumStatus
    {
        Free,                   // Ücretsiz kullanıcı
        AdFree,                 // Reklamsız
        Premium                 // Tam premium
    }
}
