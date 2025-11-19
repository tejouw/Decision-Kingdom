using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Sanayi Devrimi dönemi karakter ve event tanımları
    /// Tema: Fabrikalar, işçi hakları, sömürgecilik, buhar teknolojisi
    /// </summary>
    public static class IndustrialContent
    {
        #region Character IDs
        public const string CHAR_FACTORY_OWNER = "factory_owner";
        public const string CHAR_WORKER_LEADER = "worker_leader";
        public const string CHAR_COLONIAL_GOV = "colonial_gov";
        public const string CHAR_INVENTOR = "inventor";
        public const string CHAR_JOURNALIST = "journalist";
        public const string CHAR_ACTIVIST = "activist";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character(CHAR_FACTORY_OWNER, "Edward", "Fabrikatör")
                {
                    eras = new List<Era> { Era.Industrial },
                    description = "Zengin fabrika sahibi. Kâr her şeyin önünde gelir."
                },
                new Character(CHAR_WORKER_LEADER, "Thomas", "İşçi Lideri")
                {
                    eras = new List<Era> { Era.Industrial },
                    description = "Sendika önderi. İşçi hakları için mücadele ediyor."
                },
                new Character(CHAR_COLONIAL_GOV, "Victoria", "Sömürge Valisi")
                {
                    eras = new List<Era> { Era.Industrial },
                    description = "Uzak toprakların yöneticisi. İmparatorluğu genişletmek istiyor."
                },
                new Character(CHAR_INVENTOR, "James", "Mucit")
                {
                    eras = new List<Era> { Era.Industrial },
                    description = "Buhar ve makine dehası. Yeni icatlarla dünyayı değiştiriyor."
                },
                new Character(CHAR_JOURNALIST, "Charles", "Gazeteci")
                {
                    eras = new List<Era> { Era.Industrial },
                    description = "Araştırmacı gazeteci. Toplumsal sorunları gün yüzüne çıkarıyor."
                },
                new Character(CHAR_ACTIVIST, "Emmeline", "Aktivist")
                {
                    eras = new List<Era> { Era.Industrial },
                    description = "Sosyal reformcu. Eşitlik ve adalet için savaşıyor."
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
            var factoryOwner = characters.Find(c => c.id == CHAR_FACTORY_OWNER);
            var workerLeader = characters.Find(c => c.id == CHAR_WORKER_LEADER);
            var colonialGov = characters.Find(c => c.id == CHAR_COLONIAL_GOV);
            var inventor = characters.Find(c => c.id == CHAR_INVENTOR);
            var journalist = characters.Find(c => c.id == CHAR_JOURNALIST);
            var activist = characters.Find(c => c.id == CHAR_ACTIVIST);

            // ============ TEMEL RANDOM EVENTLER ============

            // Fabrika ve Üretim Eventleri
            events.Add(CreateEvent("ind_new_factory", Era.Industrial, EventCategory.Random, null,
                "Yeni bir fabrika inşa edilmek isteniyor. 'Binlerce iş yaratacak!'",
                new Choice("Çevre kirlenir")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Fabrikayı onayla")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -5)
                    .AddFlag("factory_built"),
                1f, "Fabrika inşaatı"));

            events.Add(CreateEvent("ind_coal_mine", Era.Industrial, EventCategory.Random, null,
                "Yeni kömür madeni açılacak. Tehlikeli ama çok kârlı.",
                new Choice("İşçi güvenliği önce")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Madeni aç")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Military, 5),
                1f, "Kömür madeni"));

            events.Add(CreateEvent("ind_textile_boom", Era.Industrial, EventCategory.Random, null,
                "Tekstil sektörü patlıyor. Pamuk talebi çok yüksek.",
                new Choice("Mevcut üretim yeterli")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Üretimi artır")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -5),
                1f, "Tekstil patlaması"));

            events.Add(CreateEvent("ind_factory_fire", Era.Industrial, EventCategory.Random, null,
                "Fabrikada yangın çıktı! Onlarca işçi öldü.",
                new Choice("Talihsizlik")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Güvenlik reformları yap")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("safety_reforms"),
                1.5f, "Fabrika yangını", priority: 1));

            // Demiryolu Eventleri
            events.Add(CreateEvent("ind_railway_project", Era.Industrial, EventCategory.Random, null,
                "Demiryolu inşaatı için izin isteniyor. 'Ulaşımda devrim!'",
                new Choice("Atlar yeterli")
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Demiryolunu yap")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("railway_built"),
                1.5f, "Demiryolu projesi"));

            events.Add(CreateEvent("ind_railway_strike", Era.Industrial, EventCategory.Random, null,
                "Demiryolu işçileri greve gitti. Ulaşım durdu!",
                new Choice("Polisi gönder")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("Taleplerini dinle")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10),
                1.5f, "Demiryolu grevi"));

            // İşçi Hakları Eventleri
            events.Add(CreateEvent("ind_child_labor", Era.Industrial, EventCategory.Random, null,
                "Fabrikalarda çocuk işçiler çalıştırılıyor. Verim yüksek ama...",
                new Choice("Yasakla")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("child_labor_banned"),
                new Choice("Görmezden gel")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -15),
                1.5f, "Çocuk işçiler"));

            events.Add(CreateEvent("ind_work_hours", Era.Industrial, EventCategory.Random, null,
                "İşçiler günde 16 saat çalışıyor. Sağlık sorunları artıyor.",
                new Choice("Çalışma saatlerini sınırla")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15),
                new Choice("Fabrikatörlerin kararı")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                1f, "Çalışma saatleri"));

            events.Add(CreateEvent("ind_union_formation", Era.Industrial, EventCategory.Random, null,
                "İşçiler sendika kurmak istiyor. Fabrikatörler karşı.",
                new Choice("Sendikaları yasakla")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Military, 5),
                new Choice("Sendikalara izin ver")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("unions_legal"),
                1.5f, "Sendika kurulumu"));

            // Sömürgecilik Eventleri
            events.Add(CreateEvent("ind_colonial_expansion", Era.Industrial, EventCategory.Random, null,
                "Afrika'da yeni topraklar keşfedildi. Sömürge fırsatı!",
                new Choice("Sömürgecilik yanlış")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Sömürge kur")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("colonial_power"),
                1f, "Sömürge genişlemesi"));

            events.Add(CreateEvent("ind_colonial_revolt", Era.Industrial, EventCategory.Random, null,
                "Sömürgede isyan çıktı! Yerli halk bağımsızlık istiyor.",
                new Choice("Bağımsızlık ver")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("İsyanı bastır")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, -10),
                1.5f, "Sömürge isyanı"));

            events.Add(CreateEvent("ind_opium_trade", Era.Industrial, EventCategory.Random, null,
                "Uzak Doğu'ya afyon ticareti çok kârlı. Ama etik mi?",
                new Choice("Ticareti yasakla")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Faith, 15),
                new Choice("Ticarete devam")
                    .AddEffect(ResourceType.Gold, 25)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("opium_trade"),
                1f, "Afyon ticareti"));

            // Teknoloji Eventleri
            events.Add(CreateEvent("ind_steam_engine", Era.Industrial, EventCategory.Random, null,
                "Yeni nesil buhar makinesi geliştirildi. Daha verimli!",
                new Choice("Mevcut makineler yeterli")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Yeni makineye geç")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                1f, "Buhar makinesi"));

            events.Add(CreateEvent("ind_telegraph", Era.Industrial, EventCategory.Random, null,
                "Telgraf sistemi kurulmak isteniyor. 'Anında iletişim!'",
                new Choice("Lüks, gereksiz")
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Telgrafı kur")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("telegraph"),
                1.5f, "Telgraf sistemi"));

            events.Add(CreateEvent("ind_photography", Era.Industrial, EventCategory.Random, null,
                "Fotoğrafçılık yayılıyor. Ressamlar işsiz kalacak!",
                new Choice("Sanatı koru")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Yeniliği benimse")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, 10),
                1f, "Fotoğrafçılık"));

            // Sosyal Reform Eventleri
            events.Add(CreateEvent("ind_public_education", Era.Industrial, EventCategory.Random, null,
                "Halk için zorunlu eğitim öneriliyor. 'Her çocuk okula!'",
                new Choice("Pahalı")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Zorunlu eğitimi başlat")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("public_education"),
                1.5f, "Zorunlu eğitim"));

            events.Add(CreateEvent("ind_public_health", Era.Industrial, EventCategory.Random, null,
                "Şehirlerde hastalık salgınları artıyor. Kanalizasyon lazım.",
                new Choice("Bireysel sorumluluk")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("Altyapıyı geliştir")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10),
                1.5f, "Halk sağlığı"));

            events.Add(CreateEvent("ind_voting_rights", Era.Industrial, EventCategory.Random, null,
                "Halk oy hakkı istiyor. 'Demokrasi şimdi!'",
                new Choice("Tehlikeli")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Military, 5),
                new Choice("Oy hakkını genişlet")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, -10)
                    .AddFlag("voting_rights"),
                1.5f, "Oy hakları"));

            // ============ KARAKTER EVENTLERİ ============

            // EDWARD - Fabrikatör
            events.Add(CreateEvent("ind_factory_owner_intro", Era.Industrial, EventCategory.Character, factoryOwner,
                "Fabrikatör Edward huzurunuzda. 'Efendim, fabrikalarım binlerce kişiye iş veriyor. Desteğinize ihtiyacım var.'",
                new Choice("Fabrikatörlere güvenmem")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("factory_owner_rejected"),
                new Choice("İş birliği yapalım")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("factory_owner_ally"),
                2f, "Edward tanışma", priority: 3));

            events.Add(CreateEvent("ind_factory_expansion", Era.Industrial, EventCategory.Character, factoryOwner,
                "Edward: 'Efendim, yeni fabrika için arazi lazım. Köylülerin tarlalarını almalıyız.'",
                new Choice("Köylüleri koru")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Gold, -10),
                new Choice("Araziyi al")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddFlag("land_seized"),
                1.5f, "Fabrika genişlemesi"));

            events.Add(CreateEvent("ind_factory_monopoly", Era.Industrial, EventCategory.Character, factoryOwner,
                "Edward: 'Efendim, rakiplerimi satın aldım. Artık tek hakimim!'",
                new Choice("Tekel yanlış")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Tekeli onayla")
                    .AddEffect(ResourceType.Gold, 25)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("monopoly_approved"),
                1f, "Tekel oluşumu"));

            events.Add(CreateEvent("ind_factory_lockout", Era.Industrial, EventCategory.Character, factoryOwner,
                "Edward: 'İşçiler fazla talep ediyor. Fabrikayı kapatıp açlığa mahkum edeceğim!'",
                new Choice("İşçileri koru")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("workers_protected"),
                new Choice("Edward'ı destekle")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Military, -5),
                1.5f, "Lokavt"));

            // THOMAS - İşçi Lideri
            events.Add(CreateEvent("ind_worker_leader_intro", Era.Industrial, EventCategory.Character, workerLeader,
                "İşçi lideri Thomas huzurunuzda. 'Efendim, işçiler sefalet içinde. Adalet istiyoruz!'",
                new Choice("İşçi haklarını görmezden gel")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("worker_leader_rejected"),
                new Choice("Taleplerini dinle")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("worker_leader_supported"),
                2f, "Thomas tanışma", priority: 3));

            events.Add(CreateEvent("ind_general_strike", Era.Industrial, EventCategory.Character, workerLeader,
                "Thomas: 'Efendim, taleplerimiz karşılanmazsa genel grev ilan edeceğiz!'",
                new Choice("Grevi yasakla")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddTriggeredEvent("ind_strike_violence"),
                new Choice("Pazarlık yap")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 20),
                2f, "Genel grev tehdidi", priority: 2));

            events.Add(CreateEvent("ind_strike_violence", Era.Industrial, EventCategory.Chain, workerLeader,
                "Grev şiddete dönüştü! Fabrikalar yakılıyor, askerler ateş açtı!",
                new Choice("Düzeni koru")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -25)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Ateşkes ilan et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("strike_peace"),
                1.5f, "Grev şiddeti"));

            events.Add(CreateEvent("ind_worker_cooperative", Era.Industrial, EventCategory.Character, workerLeader,
                "Thomas: 'Efendim, işçi kooperatifi kurmak istiyoruz. Kendi fabrikamız olacak!'",
                new Choice("Kapitalizmi tehdit eder")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Kooperatife izin ver")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddFlag("worker_coop"),
                1f, "İşçi kooperatifi"));

            // VICTORIA - Sömürge Valisi
            events.Add(CreateEvent("ind_colonial_gov_intro", Era.Industrial, EventCategory.Character, colonialGov,
                "Sömürge Valisi Victoria huzurunuzda. 'Efendim, uzak topraklar zenginlik ve şan vaat ediyor!'",
                new Choice("Sömürgecilik barbarlık")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("colonial_gov_rejected"),
                new Choice("İmparatorluğu genişlet")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("colonial_gov_supported"),
                2f, "Victoria tanışma", priority: 2));

            events.Add(CreateEvent("ind_colonial_resource", Era.Industrial, EventCategory.Character, colonialGov,
                "Victoria: 'Efendim, sömürgede elmas madeni bulduk! Ama yerlilerin kutsal toprağı.'",
                new Choice("Kutsal toprağa saygı")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Madeni işlet")
                    .AddEffect(ResourceType.Gold, 30)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("sacred_land_violated"),
                1.5f, "Sömürge kaynakları"));

            events.Add(CreateEvent("ind_colonial_education", Era.Industrial, EventCategory.Character, colonialGov,
                "Victoria: 'Yerlilere medeniyet götürmeliyiz. Okullar açalım!'",
                new Choice("Kendi kültürlerine saygı")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Misyoner okulları aç")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("colonial_education"),
                1f, "Sömürge eğitimi"));

            // JAMES - Mucit
            events.Add(CreateEvent("ind_inventor_intro", Era.Industrial, EventCategory.Character, inventor,
                "Mucit James huzurunuzda. 'Efendim, yeni icatlarım dünyayı değiştirecek!'",
                new Choice("Mucitlere ihtiyacım yok")
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("inventor_rejected"),
                new Choice("İcatlarını göster")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("inventor_supported"),
                2f, "James tanışma", priority: 2));

            events.Add(CreateEvent("ind_inventor_engine", Era.Industrial, EventCategory.Character, inventor,
                "James: 'Efendim, daha güçlü bir motor yaptım! Fabrikalar iki kat üretir!'",
                new Choice("Pahalı görünüyor")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Motoru üret")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("new_engine"),
                1.5f, "Yeni motor"));

            events.Add(CreateEvent("ind_inventor_weapon", Era.Industrial, EventCategory.Character, inventor,
                "James: 'Efendim, yeni silah tasarladım. Bir asker yüz kişiyi öldürebilir!'",
                new Choice("İnsanlık dışı")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Silahı üret")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("advanced_weapons"),
                1f, "Yeni silah"));

            events.Add(CreateEvent("ind_inventor_medicine", Era.Industrial, EventCategory.Character, inventor,
                "James: 'Efendim, hastalıkları önleyen bir ilaç geliştirdim. Aşı!'",
                new Choice("Doğaya aykırı")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Aşıyı uygula")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("vaccination"),
                1.5f, "Aşı keşfi"));

            // CHARLES - Gazeteci
            events.Add(CreateEvent("ind_journalist_intro", Era.Industrial, EventCategory.Character, journalist,
                "Gazeteci Charles huzurunuzda. 'Efendim, halkın gerçeği bilme hakkı var!'",
                new Choice("Basın tehlikeli")
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("journalist_rejected"),
                new Choice("Basın özgürlüğü")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("journalist_supported"),
                1.5f, "Charles tanışma"));

            events.Add(CreateEvent("ind_journalist_expose", Era.Industrial, EventCategory.Character, journalist,
                "Charles: 'Efendim, fabrika koşullarını yazdım. Halk şokta!'",
                new Choice("Haberi sansürle")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("Yayınla")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("expose_published"),
                1.5f, "Araştırmacı gazetecilik"));

            events.Add(CreateEvent("ind_journalist_scandal", Era.Industrial, EventCategory.Character, journalist,
                "Charles: 'Efendim, yolsuzluk dosyası hazırladım. Bazı isimler sizi şaşırtacak.'",
                new Choice("Yayınlama")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Gerçeği yayınla")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("corruption_exposed"),
                1.5f, "Yolsuzluk skandalı"));

            // EMMELINE - Aktivist
            events.Add(CreateEvent("ind_activist_intro", Era.Industrial, EventCategory.Character, activist,
                "Aktivist Emmeline huzurunuzda. 'Efendim, kadınlar da oy hakkı istiyor!'",
                new Choice("Kadınlar siyasete karışmasın")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("activist_rejected"),
                new Choice("Taleplerini değerlendir")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("activist_supported"),
                1.5f, "Emmeline tanışma"));

            events.Add(CreateEvent("ind_suffrage_march", Era.Industrial, EventCategory.Character, activist,
                "Emmeline: 'Efendim, büyük bir yürüyüş düzenliyoruz. Kadınlara oy hakkı!'",
                new Choice("Yürüyüşü yasakla")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, -15),
                new Choice("Yürüyüşe izin ver")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("suffrage_movement"),
                1.5f, "Oy hakkı yürüyüşü"));

            events.Add(CreateEvent("ind_activist_hunger", Era.Industrial, EventCategory.Character, activist,
                "Emmeline hapse atıldı ve açlık grevi başlattı. Kamuoyu bölünmüş.",
                new Choice("Zorla besle")
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Serbest bırak")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("activist_freed"),
                2f, "Açlık grevi", priority: 2));

            // ============ NADİR EVENTLER ============

            events.Add(CreateEvent("ind_great_exhibition", Era.Industrial, EventCategory.Rare, null,
                "Büyük Sergi! Tüm dünyanın icatlarını gösterme fırsatı.",
                new Choice("Çok pahalı")
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("Sergiyi düzenle")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("great_exhibition"),
                1f, "Büyük Sergi", isRare: true));

            events.Add(CreateEvent("ind_cholera_epidemic", Era.Industrial, EventCategory.Rare, null,
                "Kolera salgını! Binlerce kişi ölüyor, şehirler panik içinde.",
                new Choice("Tanrı'nın cezası")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -30),
                new Choice("Tıbbi müdahale")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, 5),
                1f, "Kolera salgını", isRare: true));

            events.Add(CreateEvent("ind_dynamite_invention", Era.Industrial, EventCategory.Rare, null,
                "Dinamit icat edildi! İnşaat için harika ama silah olarak da kullanılabilir.",
                new Choice("Sivil kullanım")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Askeri kullanım")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("dynamite_military"),
                1f, "Dinamit", isRare: true));

            // ============ KOŞULLU EVENTLER ============

            // Düşük mutluluk eventi
            var lowHappinessEvent = CreateEvent("ind_revolution_threat", Era.Industrial, EventCategory.Story, workerLeader,
                "Thomas: 'Efendim, halk çok öfkeli. Devrim kapıda!'",
                new Choice("Baskıyı artır")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Acil reformlar yap")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 20),
                2f, "Devrim tehdidi", priority: 3);
            lowHappinessEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Happiness,
                conditionOperator = ConditionOperator.LessThan,
                value = 25
            });
            events.Add(lowHappinessEvent);

            // Yüksek askeri güç eventi
            var highMilitaryEvent = CreateEvent("ind_imperialist_war", Era.Industrial, EventCategory.Random, null,
                "Ordumuz çok güçlü. Komşu ülkeleri fethetme fırsatı!",
                new Choice("Barışı koru")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Savaş ilan et")
                    .AddEffect(ResourceType.Gold, -20, 30)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("imperialist_war"),
                1f, "Emperyalist savaş");
            highMilitaryEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Military,
                conditionOperator = ConditionOperator.GreaterThan,
                value = 70
            });
            events.Add(highMilitaryEvent);

            // ============ GEÇİŞ EVENTLERİ ============

            // Tur 10
            var turn10Event = CreateEvent("ind_turn_10", Era.Industrial, EventCategory.Story, null,
                "Sanayi devriminde ilk on gününüz. Nasıl bir lider olacaksınız?",
                new Choice("İşçi dostu")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("worker_friendly"),
                new Choice("Sanayici dostu")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("industrialist_friendly"),
                3f, "Sanayi vizyonu", priority: 5);
            turn10Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 10
            });
            events.Add(turn10Event);

            // Tur 25
            var turn25Event = CreateEvent("ind_turn_25", Era.Industrial, EventCategory.Story, null,
                "Yirmi beş gün geçti. Fabrikalar dumanıyla gökyüzünü karartıyor.",
                new Choice("Çevre koruma")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Üretim önce")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -10),
                3f, "Sanayi dengesi", priority: 5);
            turn25Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 25
            });
            events.Add(turn25Event);

            // Tur 50
            var turn50Event = CreateEvent("ind_turn_50", Era.Industrial, EventCategory.Story, null,
                "Elli gün! Sanayi devrimi zirveye ulaştı. Mirasınız ne olacak?",
                new Choice("Reform mirası")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("reform_legacy"),
                new Choice("Zenginlik mirası")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("wealth_legacy"),
                3f, "Sanayi mirası", priority: 5);
            turn50Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 50
            });
            events.Add(turn50Event);

            // ============ KARAKTER SON EVENTLERİ ============

            // Factory Owner final
            events.Add(CreateEvent("ind_factory_owner_final", Era.Industrial, EventCategory.Character, factoryOwner,
                "Edward: 'Efendim, birlikte imparatorluk kurduk. Sanayi sizin eseriniz!'",
                new Choice("İş ortağı kal")
                    .AddEffect(ResourceType.Gold, 15),
                new Choice("Hisse al")
                    .AddEffect(ResourceType.Gold, 25)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("industrial_empire"),
                1f, "Sanayi imparatorluğu"));

            // Worker Leader final
            events.Add(CreateEvent("ind_worker_leader_final", Era.Industrial, EventCategory.Character, workerLeader,
                "Thomas: 'Efendim, işçilerin hayatı değişti. Adalet için teşekkürler!'",
                new Choice("Görevimdi")
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Birlikte başardık")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("worker_champion"),
                1f, "İşçi şampiyonu"));

            // Inventor final
            events.Add(CreateEvent("ind_inventor_final", Era.Industrial, EventCategory.Character, inventor,
                "James: 'Efendim, icatlarım nesillere ilham verecek. Desteğiniz için minnettarım!'",
                new Choice("Başarılar")
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Akademi kur")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("inventors_academy"),
                1f, "Mucitler akademisi"));

            // Activist final
            events.Add(CreateEvent("ind_activist_final", Era.Industrial, EventCategory.Character, activist,
                "Emmeline: 'Efendim, kadınlar oy hakkı kazandı! Tarih yazıldı!'",
                new Choice("Başarını kutla")
                    .AddEffect(ResourceType.Happiness, 15),
                new Choice("Yeni haklar ver")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("women_rights"),
                1f, "Kadın hakları zaferi"));

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
