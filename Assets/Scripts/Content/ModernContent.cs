using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Modern dönem karakter ve event tanımları
    /// Tema: Demokrasi, medya, küreselleşme, teknoloji
    /// Kaynak: Faith -> Onay Oranı (Approval)
    /// </summary>
    public static class ModernContent
    {
        #region Character IDs
        public const string CHAR_MEDIA_MOGUL = "media_mogul";
        public const string CHAR_TECH_CEO = "tech_ceo";
        public const string CHAR_DIPLOMAT = "diplomat";
        public const string CHAR_GENERAL = "general";
        public const string CHAR_ACTIVIST = "activist";
        public const string CHAR_OPPOSITION = "opposition";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character(CHAR_MEDIA_MOGUL, "Murdoch", "Medya Patronu")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Güçlü medya imparatorunun sahibi. Kamuoyunu şekillendirebilir."
                },
                new Character(CHAR_TECH_CEO, "Elon", "Teknoloji CEO'su")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Vizyoner teknoloji girişimcisi. İnovasyon ve risk bir arada."
                },
                new Character(CHAR_DIPLOMAT, "Kissinger", "Diplomat")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Usta diplomat. Uluslararası ilişkilerde uzman."
                },
                new Character(CHAR_GENERAL, "Marshall", "General")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Ordunun başkomutanı. Ulusal güvenlik önceliği."
                },
                new Character(CHAR_ACTIVIST, "Greta", "Çevre Aktivisti")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Genç ve kararlı aktivist. İklim değişikliği için mücadele ediyor."
                },
                new Character(CHAR_OPPOSITION, "Sanders", "Muhalefet Lideri")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Muhalefet partisi başkanı. Reform vaatleriyle seçim kazanmak istiyor."
                }
            };
        }
        #endregion

        #region Events
        public static List<GameEvent> GetEvents()
        {
            var events = new List<GameEvent>();
            var characters = GetCharacters();

            // Karakter referansları
            var mediaMogul = characters.Find(c => c.id == CHAR_MEDIA_MOGUL);
            var techCeo = characters.Find(c => c.id == CHAR_TECH_CEO);
            var diplomat = characters.Find(c => c.id == CHAR_DIPLOMAT);
            var general = characters.Find(c => c.id == CHAR_GENERAL);
            var activist = characters.Find(c => c.id == CHAR_ACTIVIST);
            var opposition = characters.Find(c => c.id == CHAR_OPPOSITION);

            // ============ TEMEL RANDOM EVENTLER ============

            // Siyaset ve Seçim Eventleri
            events.Add(CreateEvent("mod_election_campaign", Era.Modern, EventCategory.Random, null,
                "Seçim kampanyası başlıyor. Nasıl bir kampanya yürüteceksiniz?",
                new Choice("Dürüst kampanya")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 15),
                new Choice("Agresif kampanya")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                1.5f, "Seçim kampanyası"));

            events.Add(CreateEvent("mod_corruption_scandal", Era.Modern, EventCategory.Random, null,
                "Yolsuzluk skandalı patlak verdi! Bakanlarınız suçlanıyor.",
                new Choice("İstifalarını iste")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Koru ve inkar et")
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -10),
                1.5f, "Yolsuzluk skandalı", priority: 1));

            events.Add(CreateEvent("mod_referendum", Era.Modern, EventCategory.Random, null,
                "Tartışmalı bir konuda referandum düzenlenmeli mi?",
                new Choice("Meclis karar versin")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Referandum yap")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10),
                1f, "Referandum"));

            // Ekonomi Eventleri
            events.Add(CreateEvent("mod_economic_crisis", Era.Modern, EventCategory.Random, null,
                "Ekonomik kriz kapıda! Borsalar çöküyor, işsizlik artıyor.",
                new Choice("Serbest piyasa çözer")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, -20),
                new Choice("Devlet müdahalesi")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                2f, "Ekonomik kriz", priority: 2));

            events.Add(CreateEvent("mod_tax_reform", Era.Modern, EventCategory.Random, null,
                "Vergi reformu tartışılıyor. Zenginlerden daha fazla mı alınmalı?",
                new Choice("Düşük vergi herkes için")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Artan oranlı vergi")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 10),
                1f, "Vergi reformu"));

            events.Add(CreateEvent("mod_unemployment", Era.Modern, EventCategory.Random, null,
                "İşsizlik oranı yükseliyor. Halk iş istiyor!",
                new Choice("Piyasa düzeltir")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("İstihdam programı başlat")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15),
                1.5f, "İşsizlik krizi"));

            events.Add(CreateEvent("mod_trade_deal", Era.Modern, EventCategory.Random, null,
                "Yabancı ülkeyle ticaret anlaşması fırsatı. Ama yerli üretim zarar görebilir.",
                new Choice("Yerli üretimi koru")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Anlaşmayı imzala")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -10),
                1f, "Ticaret anlaşması"));

            // Medya ve İletişim Eventleri
            events.Add(CreateEvent("mod_fake_news", Era.Modern, EventCategory.Random, null,
                "Yanlış bilgi sosyal medyada yayılıyor. Halk panik içinde!",
                new Choice("Basın özgürlüğü")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Yanlış bilgiyi yasakla")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Military, 5),
                1.5f, "Yanlış bilgi"));

            events.Add(CreateEvent("mod_press_freedom", Era.Modern, EventCategory.Random, null,
                "Gazeteciler hükümeti eleştiriyor. Basın özgürlüğü mü, istikrar mı?",
                new Choice("Eleştiriye izin ver")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Basını sınırla")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -10),
                1.5f, "Basın özgürlüğü"));

            events.Add(CreateEvent("mod_social_media_ban", Era.Modern, EventCategory.Random, null,
                "Sosyal medya platformu zararlı içerik yayıyor. Yasaklanmalı mı?",
                new Choice("İfade özgürlüğü")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Platformu yasakla")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("social_media_ban"),
                1f, "Sosyal medya yasağı"));

            // Teknoloji Eventleri
            events.Add(CreateEvent("mod_ai_development", Era.Modern, EventCategory.Random, null,
                "Yapay zeka teknolojisi gelişiyor. Fırsatlar ve riskler...",
                new Choice("Düzenleme getir")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Serbest bırak")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -10),
                1.5f, "Yapay zeka"));

            events.Add(CreateEvent("mod_cyber_attack", Era.Modern, EventCategory.Random, null,
                "Siber saldırı! Devlet sistemleri hedef alındı.",
                new Choice("Diplomatik çözüm")
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("Karşı saldırı")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("cyber_warfare"),
                1.5f, "Siber saldırı", priority: 1));

            events.Add(CreateEvent("mod_privacy_debate", Era.Modern, EventCategory.Random, null,
                "Gözetleme programı ifşa edildi! Halk öfkeli.",
                new Choice("Güvenlik için gerekli")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -15),
                new Choice("Programı durdur")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Military, -10),
                1.5f, "Gizlilik tartışması"));

            // Çevre Eventleri
            events.Add(CreateEvent("mod_climate_change", Era.Modern, EventCategory.Random, null,
                "İklim değişikliği raporları endişe verici. Acil önlem lazım!",
                new Choice("Ekonomi önce")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -15),
                new Choice("Yeşil politikalar")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("green_policy"),
                1.5f, "İklim değişikliği"));

            events.Add(CreateEvent("mod_natural_disaster", Era.Modern, EventCategory.Random, null,
                "Doğal afet! Kasırga kıyı şehirlerini vurdu.",
                new Choice("Yerel yönetimler halletsin")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Acil yardım gönder")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10),
                2f, "Doğal afet", priority: 1));

            events.Add(CreateEvent("mod_renewable_energy", Era.Modern, EventCategory.Random, null,
                "Yenilenebilir enerji projesi teklif ediliyor. Pahalı ama temiz.",
                new Choice("Fosil yakıtlar ucuz")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Yenilenebilire geç")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("renewable_energy"),
                1f, "Yenilenebilir enerji"));

            // Uluslararası İlişkiler
            events.Add(CreateEvent("mod_international_summit", Era.Modern, EventCategory.Random, null,
                "Uluslararası zirve yaklaşıyor. Hangi pozisyonu alacaksınız?",
                new Choice("Ulusal çıkarlar")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("İş birliği")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -5),
                1f, "Uluslararası zirve"));

            events.Add(CreateEvent("mod_refugee_crisis", Era.Modern, EventCategory.Random, null,
                "Mülteci krizi! Binlerce insan sınırda bekliyor.",
                new Choice("Sınırları kapat")
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Military, 10),
                new Choice("Mültecileri kabul et")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 5),
                1.5f, "Mülteci krizi"));

            events.Add(CreateEvent("mod_trade_war", Era.Modern, EventCategory.Random, null,
                "Ticaret savaşı başladı! Gümrük tarifeleri artıyor.",
                new Choice("Anlaşma ara")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Karşılık ver")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, -5),
                1.5f, "Ticaret savaşı"));

            // Sosyal Konular
            events.Add(CreateEvent("mod_healthcare_reform", Era.Modern, EventCategory.Random, null,
                "Sağlık sistemi reformu tartışılıyor. Herkes için ücretsiz sağlık?",
                new Choice("Özel sektör verimli")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Ücretsiz sağlık")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15),
                1.5f, "Sağlık reformu"));

            events.Add(CreateEvent("mod_education_policy", Era.Modern, EventCategory.Random, null,
                "Eğitim politikası tartışılıyor. Üniversite ücretsiz olmalı mı?",
                new Choice("Öğrenci kredisi yeterli")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Ücretsiz üniversite")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10),
                1f, "Eğitim politikası"));

            events.Add(CreateEvent("mod_protest_movement", Era.Modern, EventCategory.Random, null,
                "Büyük protesto hareketi başladı! Sokaklar dolu.",
                new Choice("Polisi gönder")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -15),
                new Choice("Diyalog kur")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -5),
                1.5f, "Protesto hareketi"));

            // ============ KARAKTER EVENTLERİ ============

            // MURDOCH - Medya Patronu
            events.Add(CreateEvent("mod_media_mogul_intro", Era.Modern, EventCategory.Character, mediaMogul,
                "Medya patronu Murdoch huzurunuzda. 'Sayın Başkan, medyam sizin hizmetinizde... Karşılığında...'",
                new Choice("Medya bağımsız kalmalı")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("media_mogul_rejected"),
                new Choice("Anlaşalım")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("media_mogul_ally"),
                2f, "Murdoch tanışma", priority: 3));

            events.Add(CreateEvent("mod_media_propaganda", Era.Modern, EventCategory.Character, mediaMogul,
                "Murdoch: 'Sayın Başkan, rakiplerinizi yok edecek haberler yayınlayabilirim.'",
                new Choice("Kara propaganda istemem")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Yap")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("propaganda_used"),
                1.5f, "Medya propagandası"));

            events.Add(CreateEvent("mod_media_monopoly", Era.Modern, EventCategory.Character, mediaMogul,
                "Murdoch: 'Sayın Başkan, tüm medyayı kontrol etmek istiyorum. Onay verin.'",
                new Choice("Tekel olamaz")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("media_monopoly_blocked"),
                new Choice("Onayla")
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("media_monopoly"),
                1.5f, "Medya tekeli"));

            events.Add(CreateEvent("mod_media_leak", Era.Modern, EventCategory.Character, mediaMogul,
                "Murdoch: 'Sayın Başkan, biri sızıntı yapıyor. Gizli bilgiler basına düştü!'",
                new Choice("Soruşturma başlat")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 10),
                new Choice("Sızıntıyı Murdoch'a buldur")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Military, 5),
                1.5f, "Medya sızıntısı"));

            // ELON - Tech CEO
            events.Add(CreateEvent("mod_tech_ceo_intro", Era.Modern, EventCategory.Character, techCeo,
                "Tech CEO Elon huzurunuzda. 'Sayın Başkan, geleceği birlikte inşa edelim. Mars'a bile gidebiliriz!'",
                new Choice("Gerçekçi ol")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddFlag("tech_ceo_rejected"),
                new Choice("Projelerini destekle")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("tech_ceo_supported"),
                2f, "Elon tanışma", priority: 2));

            events.Add(CreateEvent("mod_tech_innovation", Era.Modern, EventCategory.Character, techCeo,
                "Elon: 'Sayın Başkan, elektrikli araç fabrikası kurmak istiyorum. Devlet desteği lazım.'",
                new Choice("Özel sektör kendi halletsin")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Teşvik ver")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("ev_factory"),
                1.5f, "Elektrikli araç"));

            events.Add(CreateEvent("mod_tech_satellite", Era.Modern, EventCategory.Character, techCeo,
                "Elon: 'Sayın Başkan, uydu internet ağı kuracağım. Dünya bağlanacak!'",
                new Choice("Gizlilik riski")
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Projeyi onayla")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("satellite_internet"),
                1.5f, "Uydu interneti"));

            events.Add(CreateEvent("mod_tech_mars", Era.Modern, EventCategory.Character, techCeo,
                "Elon: 'Sayın Başkan, Mars'a koloni kurmak istiyorum. Tarihte yeriniz olsun!'",
                new Choice("Dünyaya odaklan")
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Mars projesini destekle")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("mars_mission"),
                1f, "Mars kolonisi"));

            // KISSINGER - Diplomat
            events.Add(CreateEvent("mod_diplomat_intro", Era.Modern, EventCategory.Character, diplomat,
                "Diplomat Kissinger huzurunuzda. 'Sayın Başkan, uluslararası arenada size rehberlik edebilirim.'",
                new Choice("Kendi yolumuzu buluruz")
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("diplomat_rejected"),
                new Choice("Danışmanım ol")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("diplomat_advisor"),
                1.5f, "Kissinger tanışma"));

            events.Add(CreateEvent("mod_diplomat_peace", Era.Modern, EventCategory.Character, diplomat,
                "Kissinger: 'Sayın Başkan, düşmanla gizli barış görüşmeleri yapabilirim.'",
                new Choice("Düşmanla konuşmam")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Gizli görüşmelere izin ver")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("secret_talks"),
                1.5f, "Gizli barış"));

            events.Add(CreateEvent("mod_diplomat_alliance", Era.Modern, EventCategory.Character, diplomat,
                "Kissinger: 'Sayın Başkan, stratejik ittifak için büyük güçle anlaşabiliriz.'",
                new Choice("Bağımsız kal")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("İttifak kur")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Faith, -5)
                    .AddFlag("superpower_alliance"),
                1.5f, "Stratejik ittifak"));

            // MARSHALL - General
            events.Add(CreateEvent("mod_general_intro", Era.Modern, EventCategory.Character, general,
                "General Marshall huzurunuzda. 'Sayın Başkan, ulusal güvenlik her şeyin üstünde!'",
                new Choice("Ordu sivil kontrolde")
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("general_sidelined"),
                new Choice("Görüşlerini değerlendireceğim")
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("general_trusted"),
                1.5f, "Marshall tanışma"));

            events.Add(CreateEvent("mod_general_intervention", Era.Modern, EventCategory.Character, general,
                "Marshall: 'Sayın Başkan, küçük ülke istikrarsız. Askeri müdahale şart!'",
                new Choice("Diplomasiyi dene")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Müdahale et")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("military_intervention"),
                1.5f, "Askeri müdahale"));

            events.Add(CreateEvent("mod_general_drone", Era.Modern, EventCategory.Character, general,
                "Marshall: 'Sayın Başkan, drone saldırısıyla teröristi öldürebiliriz. Ama sivil kayıp riski var.'",
                new Choice("Sivil kayıp kabul edilemez")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -10),
                new Choice("Saldırıyı onayla")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("drone_strike"),
                2f, "Drone saldırısı", priority: 1));

            // GRETA - Çevre Aktivisti
            events.Add(CreateEvent("mod_activist_intro", Era.Modern, EventCategory.Character, activist,
                "Çevre aktivisti Greta huzurunuzda. 'Sayın Başkan, gezegenimiz yanıyor! Harekete geçmelisiniz!'",
                new Choice("Ekonomi önce gelir")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("activist_rejected"),
                new Choice("Haklısın")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("activist_supported"),
                1.5f, "Greta tanışma"));

            events.Add(CreateEvent("mod_activist_strike", Era.Modern, EventCategory.Character, activist,
                "Greta: 'Sayın Başkan, gençler iklim grevi yapıyor. Okulları boşaltıyorlar!'",
                new Choice("Okula dönsünler")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Seslerini duyun")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("youth_movement"),
                1.5f, "İklim grevi"));

            events.Add(CreateEvent("mod_activist_protest", Era.Modern, EventCategory.Character, activist,
                "Greta: 'Sayın Başkan, büyük çevre protestosu planlıyoruz. Birlikte yürüyecek misiniz?'",
                new Choice("Tarafsız kalmalıyım")
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("Protestoya katıl")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("green_leader"),
                1f, "Çevre protestosu"));

            // SANDERS - Muhalefet Lideri
            events.Add(CreateEvent("mod_opposition_intro", Era.Modern, EventCategory.Character, opposition,
                "Muhalefet lideri Sanders huzurunuzda. 'Sayın Başkan, halk değişim istiyor!'",
                new Choice("Muhalefet engelliyor")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("opposition_enemy"),
                new Choice("Önerilerini dinle")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("opposition_dialogue"),
                1.5f, "Sanders tanışma"));

            events.Add(CreateEvent("mod_opposition_bill", Era.Modern, EventCategory.Character, opposition,
                "Sanders: 'Sayın Başkan, bu yasa tasarısını birlikte geçirelim. İki partili destek!'",
                new Choice("Kendi tasarımız yeterli")
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Birlikte çalışalım")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("bipartisan"),
                1f, "İki partili yasa"));

            events.Add(CreateEvent("mod_opposition_debate", Era.Modern, EventCategory.Character, opposition,
                "Sanders: 'Sayın Başkan, televizyonda tartışalım. Halk karar versin!'",
                new Choice("Tartışmaya gerek yok")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Tartışmayı kabul et")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 15),
                1f, "TV tartışması"));

            // ============ NADİR EVENTLER ============

            events.Add(CreateEvent("mod_pandemic", Era.Modern, EventCategory.Rare, null,
                "Küresel salgın! Virüs hızla yayılıyor, hastaneler dolu.",
                new Choice("Ekonomi açık kalsın")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -30)
                    .AddEffect(ResourceType.Faith, -25),
                new Choice("Tam karantina")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("lockdown"),
                1f, "Küresel salgın", isRare: true));

            events.Add(CreateEvent("mod_nuclear_threat", Era.Modern, EventCategory.Rare, null,
                "Nükleer kriz! Düşman ülke nükleer silah test etti.",
                new Choice("Diplomasi)
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Caydırıcılık)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("nuclear_buildup"),
                1f, "Nükleer tehdit", isRare: true));

            events.Add(CreateEvent("mod_terrorist_attack", Era.Modern, EventCategory.Rare, null,
                "Terör saldırısı! Başkentte bomba patladı, onlarca ölü.",
                new Choice("Savaş ilan et")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("İstihbarat odaklı müdahale")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 10),
                1f, "Terör saldırısı", isRare: true));

            // ============ KOŞULLU EVENTLER ============

            // Düşük onay oranı
            var lowApprovalEvent = CreateEvent("mod_impeachment", Era.Modern, EventCategory.Story, opposition,
                "Sanders: 'Sayın Başkan, meclis azil sürecini başlattı!'",
                new Choice("Anayasal hak)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Karşı kampanya)
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 5),
                2f, "Azil süreci", priority: 3);
            lowApprovalEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Faith,
                conditionOperator = ConditionOperator.LessThan,
                value = 25
            });
            events.Add(lowApprovalEvent);

            // ============ GEÇİŞ EVENTLERİ ============

            // Tur 10
            var turn10Event = CreateEvent("mod_turn_10", Era.Modern, EventCategory.Story, null,
                "İlk on gününüz modern çağda. Nasıl bir lider olacaksınız?",
                new Choice("Halk odaklı")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("populist"),
                new Choice("Teknokrat")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("technocrat"),
                3f, "Modern vizyon", priority: 5);
            turn10Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 10
            });
            events.Add(turn10Event);

            // Tur 25
            var turn25Event = CreateEvent("mod_turn_25", Era.Modern, EventCategory.Story, null,
                "Yirmi beş gün geçti. Kamuoyu yoklamaları nasıl?",
                new Choice("Halka teşekkür et")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Başarıları vurgula")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Gold, 5),
                3f, "Orta dönem", priority: 5);
            turn25Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 25
            });
            events.Add(turn25Event);

            // Tur 50
            var turn50Event = CreateEvent("mod_turn_50", Era.Modern, EventCategory.Story, null,
                "Elli gün! Görev süreniz doluyor. Mirasınız ne olacak?",
                new Choice("İstikrar")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("stable_legacy"),
                new Choice("Dönüşüm")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("transformative_legacy"),
                3f, "Modern miras", priority: 5);
            turn50Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 50
            });
            events.Add(turn50Event);

            // ============ KARAKTER SON EVENTLERİ ============

            // Media Mogul final
            events.Add(CreateEvent("mod_media_final", Era.Modern, EventCategory.Character, mediaMogul,
                "Murdoch: 'Sayın Başkan, medyam mirasınızı koruyacak. Tarih sizi hatırlayacak!'",
                new Choice("Bağımsız medya önemli")
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Anlaşmayı sürdür")
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("media_legacy"),
                1f, "Medya mirası"));

            // Tech CEO final
            events.Add(CreateEvent("mod_tech_final", Era.Modern, EventCategory.Character, techCeo,
                "Elon: 'Sayın Başkan, geleceği birlikte inşa ettik. Mars'ta bir şehir sizin adınızı taşıyacak!'",
                new Choice("Yeryüzü yeterli")
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Geleceği kucakla")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("tech_legacy"),
                1f, "Teknoloji mirası"));

            // Activist final
            events.Add(CreateEvent("mod_activist_final", Era.Modern, EventCategory.Character, activist,
                "Greta: 'Sayın Başkan, birlikte gezegeni kurtardık. Gelecek nesiller size teşekkür edecek!'",
                new Choice("Görevimdi")
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Mücadele devam etmeli")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("green_legacy"),
                1f, "Yeşil miras"));

            return events;
        }
        #endregion

        #region Helper Methods
        private static GameEvent CreateEvent(string id, Era era, EventCategory category, Character character,
            string text, Choice leftChoice, Choice rightChoice, float weight, string description,
            int priority = 0, bool isRare = false)
        {
            return new GameEvent
            {
                id = id,
                era = era,
                category = category,
                character = character,
                text = text,
                leftChoice = leftChoice,
                rightChoice = rightChoice,
                weight = weight,
                description = description,
                priority = priority,
                isRare = isRare,
                conditions = new List<Condition>()
            };
        }
        #endregion
    }
}
