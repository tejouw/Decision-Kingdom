using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Rönesans dönemi karakter ve event tanımları
    /// Tema: Sanat, keşif, bilim, matbaa, tüccar aileleri
    /// </summary>
    public static class RenaissanceContent
    {
        #region Character IDs
        public const string CHAR_LEONARDO = "leonardo";
        public const string CHAR_EXPLORER = "explorer";
        public const string CHAR_SCIENTIST = "scientist";
        public const string CHAR_MEDICI = "medici";
        public const string CHAR_PRINTER = "printer";
        public const string CHAR_AMBASSADOR = "ambassador";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character(CHAR_LEONARDO, "Leonardo", "Sanatçı")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Dahi sanatçı ve mucit. Sanat ve bilim arasında köprü kuran vizyon sahibi."
                },
                new Character(CHAR_EXPLORER, "Colombo", "Kaşif")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Cesur denizci ve kaşif. Yeni dünyalar keşfetmek için seferlere çıkıyor."
                },
                new Character(CHAR_SCIENTIST, "Galileo", "Bilim Adamı")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Gözlemci ve düşünür. Kiliseyle çatışan yeni fikirler ortaya koyuyor."
                },
                new Character(CHAR_MEDICI, "Lorenzo", "Bankacı")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Güçlü Medici ailesinin temsilcisi. Sanat ve siyaseti finanse ediyor."
                },
                new Character(CHAR_PRINTER, "Gutenberg", "Matbaacı")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Matbaa ustası. Bilgiyi halka yaymak için çalışıyor."
                },
                new Character(CHAR_AMBASSADOR, "Isabella", "Elçi")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Zeki diplomat. Avrupa saraylarında ittifaklar kuruyor."
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
            var leonardo = characters.Find(c => c.id == CHAR_LEONARDO);
            var explorer = characters.Find(c => c.id == CHAR_EXPLORER);
            var scientist = characters.Find(c => c.id == CHAR_SCIENTIST);
            var medici = characters.Find(c => c.id == CHAR_MEDICI);
            var printer = characters.Find(c => c.id == CHAR_PRINTER);
            var ambassador = characters.Find(c => c.id == CHAR_AMBASSADOR);

            // ============ TEMEL RANDOM EVENTLER ============

            // Sanat ve Kültür Eventleri
            events.Add(CreateEvent("ren_art_commission", Era.Renaissance, EventCategory.Random, null,
                "Bir ressam büyük bir tablo için sipariş istiyor. 'Majeste, şanınızı ölümsüzleştireceğim!'",
                new Choice("Gereksiz masraf")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Sipariş ver")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                1f, "Sanat siparişi"));

            events.Add(CreateEvent("ren_theater_troupe", Era.Renaissance, EventCategory.Random, null,
                "Bir tiyatro topluluğu sarayda oyun sergilemek istiyor.",
                new Choice("Reddet, ciddi işler var")
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Oyunu izle")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -5),
                1f, "Tiyatro gösterisi"));

            events.Add(CreateEvent("ren_music_academy", Era.Renaissance, EventCategory.Random, null,
                "Müzisyenler bir akademi kurmak için izin istiyor.",
                new Choice("Müzik lüks")
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Akademiyi destekle")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                1f, "Müzik akademisi"));

            events.Add(CreateEvent("ren_sculpture_garden", Era.Renaissance, EventCategory.Random, null,
                "Heykeltraş saray bahçesi için eserler yapmak istiyor.",
                new Choice("Bahçe yeterli güzel")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Heykelleri sipariş et")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10),
                1f, "Heykel bahçesi"));

            // Ticaret ve Ekonomi Eventleri
            events.Add(CreateEvent("ren_spice_trade", Era.Renaissance, EventCategory.Random, null,
                "Doğu'dan baharat kervanı geldi. Pahalı ama kârlı bir ticaret.",
                new Choice("Çok riskli")
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Ticaret yap")
                    .AddEffect(ResourceType.Gold, -10, 20)
                    .AddFlag("spice_trade"),
                1.5f, "Baharat ticareti"));

            events.Add(CreateEvent("ren_banking_system", Era.Renaissance, EventCategory.Random, null,
                "Bankerler yeni bir kredi sistemi öneriyor. 'Faizle para kazanabiliriz.'",
                new Choice("Faiz günahtır")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Sistemi kur")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("banking_established"),
                1f, "Bankacılık sistemi"));

            events.Add(CreateEvent("ren_guild_expansion", Era.Renaissance, EventCategory.Random, null,
                "Zanaatkar loncaları genişlemek istiyor. Daha fazla üretim, daha fazla vergi.",
                new Choice("Mevcut durum yeterli")
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Genişlemeyi onayla")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                1f, "Lonca genişlemesi"));

            events.Add(CreateEvent("ren_silk_road", Era.Renaissance, EventCategory.Random, null,
                "İpek Yolu tüccarları yeni bir rota öneriyor. Tehlikeli ama kazançlı.",
                new Choice("Mevcut rotalar yeterli")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Yeni rotayı dene")
                    .AddEffect(ResourceType.Gold, -5, 15)
                    .AddEffect(ResourceType.Military, -5),
                1f, "İpek Yolu"));

            // Bilim ve Keşif Eventleri
            events.Add(CreateEvent("ren_university", Era.Renaissance, EventCategory.Random, null,
                "Alimler bir üniversite kurmak için destek istiyor.",
                new Choice("Kilise yeterli eğitim veriyor")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Üniversiteyi destekle")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("university_founded"),
                1.5f, "Üniversite kurulumu"));

            events.Add(CreateEvent("ren_anatomy_study", Era.Renaissance, EventCategory.Random, null,
                "Doktorlar insan anatomisi çalışmak istiyor. Kilise karşı çıkıyor.",
                new Choice("Kiliseyi dinle")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Araştırmaya izin ver")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("anatomy_allowed"),
                1f, "Anatomi çalışması"));

            events.Add(CreateEvent("ren_map_making", Era.Renaissance, EventCategory.Random, null,
                "Haritacılar daha doğru haritalar yapmak için kaynak istiyor.",
                new Choice("Mevcut haritalar yeterli")
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Harita projesini destekle")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 10),
                1f, "Harita yapımı"));

            // Dini Eventler
            events.Add(CreateEvent("ren_reform_movement", Era.Renaissance, EventCategory.Random, null,
                "Din adamları reform hareketinden bahsediyor. 'Kilise değişmeli!'",
                new Choice("Kilise mükemmel")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Reformları değerlendir")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("reform_supporter"),
                1.5f, "Reform hareketi"));

            events.Add(CreateEvent("ren_indulgence_sale", Era.Renaissance, EventCategory.Random, null,
                "Kilise endüljans satışı yapıyor. 'Günahlarınızı altınla affettirin!'",
                new Choice("Bu yanlış")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Endüljans al")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -5),
                1f, "Endüljans satışı"));

            events.Add(CreateEvent("ren_heresy_trial", Era.Renaissance, EventCategory.Random, null,
                "Bir düşünür sapkınlıkla suçlanıyor. Engizisyon yargılama istiyor.",
                new Choice("Düşünceye özgürlük")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Yargılamaya izin ver")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -15),
                1.5f, "Sapkınlık yargılaması"));

            // Siyasi Eventler
            events.Add(CreateEvent("ren_city_state_alliance", Era.Renaissance, EventCategory.Random, null,
                "Komşu şehir devleti ittifak öneriyor. Güçlü ama bağımsızlık azalır.",
                new Choice("Bağımsız kal")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("İttifak kur")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("city_alliance"),
                1f, "Şehir devleti ittifakı"));

            events.Add(CreateEvent("ren_mercenary_offer", Era.Renaissance, EventCategory.Random, null,
                "Paralı askerler (condottieri) hizmet teklif ediyor.",
                new Choice("Kendi ordumuz yeterli")
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Paralı askerleri kirala")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 15),
                1f, "Paralı askerler"));

            events.Add(CreateEvent("ren_papal_relations", Era.Renaissance, EventCategory.Random, null,
                "Papa elçi gönderdi. 'Kutsal Sandalye'nin desteğini ister misiniz?'",
                new Choice("Bağımsız kalmalıyız")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Military, 5),
                new Choice("Papa'nın desteğini al")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("papal_support"),
                1.5f, "Papalık ilişkileri"));

            // ============ KARAKTER EVENTLERİ ============

            // LEONARDO - Sanatçı
            events.Add(CreateEvent("ren_leonardo_intro", Era.Renaissance, EventCategory.Character, leonardo,
                "Ünlü sanatçı Leonardo huzurunuza geldi. 'Efendim, sanat ve bilimi birleştireceğim eserler yaratmak istiyorum.'",
                new Choice("Sanata ihtiyacım yok")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("leonardo_rejected"),
                new Choice("Himayemize al")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("leonardo_patron"),
                2f, "Leonardo tanışma", priority: 3));

            events.Add(CreateEvent("ren_leonardo_invention", Era.Renaissance, EventCategory.Character, leonardo,
                "Leonardo: 'Efendim, uçan bir makine tasarladım! Ama malzeme pahalı.'",
                new Choice("Boş hayal")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Projeyi finanse et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddTriggeredEvent("ren_leonardo_flying"),
                1.5f, "Uçan makine"));

            events.Add(CreateEvent("ren_leonardo_flying", Era.Renaissance, EventCategory.Chain, leonardo,
                "Leonardo'nun uçan makinesi test edildi. Tam başarı değil ama ilginç sonuçlar!",
                new Choice("Projeyi durdur")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Araştırmaya devam")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("leonardo_research"),
                1f, "Uçuş deneyi"));

            events.Add(CreateEvent("ren_leonardo_mural", Era.Renaissance, EventCategory.Character, leonardo,
                "Leonardo: 'Efendim, saray duvarlarına büyük bir fresk yapmak istiyorum. Son Akşam Yemeği gibi!'",
                new Choice("Duvarlar iyi")
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Fresk yaptır")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddFlag("leonardo_mural"),
                1.5f, "Büyük fresk"));

            events.Add(CreateEvent("ren_leonardo_war_machine", Era.Renaissance, EventCategory.Character, leonardo,
                "Leonardo: 'Efendim, düşmanları yok edecek savaş makineleri tasarlayabilirim.'",
                new Choice("Barışı tercih et")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Savaş makinelerini yap")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("war_machines"),
                1f, "Savaş makineleri"));

            // COLOMBO - Kaşif
            events.Add(CreateEvent("ren_explorer_intro", Era.Renaissance, EventCategory.Character, explorer,
                "Kaşif Colombo huzurunuzda. 'Efendim, batıya giderek Hindistan'a ulaşabiliriz. Sadece gemi lazım!'",
                new Choice("Çılgınlık")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddFlag("explorer_rejected"),
                new Choice("Seferi finanse et")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("explorer_supported")
                    .AddTriggeredEvent("ren_explorer_voyage"),
                2f, "Colombo tanışma", priority: 3));

            events.Add(CreateEvent("ren_explorer_voyage", Era.Renaissance, EventCategory.Chain, explorer,
                "Colombo denize açıldı. Haftalar sonra haber geldi: 'Yeni topraklar bulduk!'",
                new Choice("Geri dön")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Kolonileş")
                    .AddEffect(ResourceType.Gold, -15, 30)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("new_world")
                    .AddTriggeredEvent("ren_explorer_colony"),
                1.5f, "Yeni dünya"));

            events.Add(CreateEvent("ren_explorer_colony", Era.Renaissance, EventCategory.Chain, explorer,
                "Koloni kuruldu ama yerlilerle sorun çıkıyor. Colombo: 'Ne yapmalıyız?'",
                new Choice("Barışçıl ilişki kur")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, 5)
                    .AddFlag("peaceful_colony"),
                new Choice("Zorla itaat ettir")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("conquest_colony"),
                1f, "Koloni yönetimi"));

            events.Add(CreateEvent("ren_explorer_treasure", Era.Renaissance, EventCategory.Character, explorer,
                "Colombo: 'Efendim, yeni dünyada altın dolu bir şehir efsanesi var. El Dorado!'",
                new Choice("Efsane sadece efsane")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("El Dorado'yu ara")
                    .AddEffect(ResourceType.Gold, -20, 40)
                    .AddEffect(ResourceType.Military, -10)
                    .AddFlag("eldorado_search"),
                1f, "El Dorado arayışı"));

            // GALILEO - Bilim Adamı
            events.Add(CreateEvent("ren_scientist_intro", Era.Renaissance, EventCategory.Character, scientist,
                "Bilim adamı Galileo huzurunuzda. 'Efendim, gökyüzünü inceledim. Dünya güneşin etrafında dönüyor!'",
                new Choice("Sapkınlık! Kov")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("galileo_rejected"),
                new Choice("Çalışmalarını destekle")
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("galileo_supported"),
                2f, "Galileo tanışma", priority: 3));

            events.Add(CreateEvent("ren_scientist_telescope", Era.Renaissance, EventCategory.Character, scientist,
                "Galileo: 'Efendim, teleskop icat ettim! Gezegenlerin uydularını görebiliyorum!'",
                new Choice("Gereksiz oyuncak")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Teleskopu finanse et")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("telescope"),
                1.5f, "Teleskop"));

            events.Add(CreateEvent("ren_scientist_trial", Era.Renaissance, EventCategory.Character, scientist,
                "Kilise Galileo'yu yargılamak istiyor. 'Sapkın fikirler yayıyor!'",
                new Choice("Kiliseye teslim et")
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("galileo_condemned"),
                new Choice("Galileo'yu koru")
                    .AddEffect(ResourceType.Faith, -25)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("galileo_protected"),
                2f, "Galileo yargılaması", priority: 2));

            events.Add(CreateEvent("ren_scientist_book", Era.Renaissance, EventCategory.Character, scientist,
                "Galileo: 'Efendim, bulgularımı kitap haline getirdim. Yayınlansın mı?'",
                new Choice("Tehlikeli, yayınlama")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Kitabı yayınla")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddFlag("science_book"),
                1f, "Bilim kitabı"));

            // LORENZO - Bankacı (Medici)
            events.Add(CreateEvent("ren_medici_intro", Era.Renaissance, EventCategory.Character, medici,
                "Banker Lorenzo Medici huzurunuzda. 'Efendim, ailemi siyasi çalkantılardan kaçırdım. Size hizmet etmek istiyorum.'",
                new Choice("Bankerlere güvenmem")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("medici_rejected"),
                new Choice("Kabul et")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddFlag("medici_accepted"),
                2f, "Medici tanışma", priority: 2));

            events.Add(CreateEvent("ren_medici_loan", Era.Renaissance, EventCategory.Character, medici,
                "Lorenzo: 'Efendim, size büyük bir kredi teklif edebilirim. Faizi düşük tutarım.'",
                new Choice("Borca girmem")
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Krediyi al")
                    .AddEffect(ResourceType.Gold, 30)
                    .AddFlag("medici_loan")
                    .AddTriggeredEvent("ren_medici_debt"),
                1.5f, "Medici kredisi"));

            events.Add(CreateEvent("ren_medici_debt", Era.Renaissance, EventCategory.Chain, medici,
                "Lorenzo: 'Efendim, kredi vadesi geldi. Ödeme zamanı.'",
                new Choice("Süre iste")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Borcunu öde")
                    .AddEffect(ResourceType.Gold, -35)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("medici_trusted"),
                1.5f, "Borç ödeme"));

            events.Add(CreateEvent("ren_medici_patron", Era.Renaissance, EventCategory.Character, medici,
                "Lorenzo: 'Efendim, birlikte sanat hamiliği yapalım. Şanımız sonsuza dek sürecek!'",
                new Choice("Para israfı")
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("Ortak hamilik")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("medici_patron"),
                1f, "Sanat hamiliği"));

            events.Add(CreateEvent("ren_medici_conspiracy", Era.Renaissance, EventCategory.Character, medici,
                "Dedikodular yayılıyor. Lorenzo rakip bir hanedan kurmaya çalışıyor.",
                new Choice("Görmezden gel")
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("medici_power"),
                new Choice("Lorenzo'yu sorgula")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddTriggeredEvent("ren_medici_truth"),
                1f, "Medici komplosu"));

            events.Add(CreateEvent("ren_medici_truth", Era.Renaissance, EventCategory.Chain, medici,
                "Lorenzo: 'Efendim, sadece ailem için güvenlik istiyorum. İhanet etmem!'",
                new Choice("Sürgün et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("medici_exiled"),
                new Choice("Güven ver")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("medici_loyal"),
                1f, "Lorenzo'nun gerçeği"));

            // GUTENBERG - Matbaacı
            events.Add(CreateEvent("ren_printer_intro", Era.Renaissance, EventCategory.Character, printer,
                "Matbaacı Gutenberg huzurunuzda. 'Efendim, hareketli harflerle kitap basabilirim. Bilgi herkese ulaşacak!'",
                new Choice("Kilise kızmaz mı?")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("printer_rejected"),
                new Choice("Matbaayı destekle")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("printing_supported"),
                2f, "Gutenberg tanışma", priority: 2));

            events.Add(CreateEvent("ren_printer_bible", Era.Renaissance, EventCategory.Character, printer,
                "Gutenberg: 'Efendim, İncil'i basıp halka dağıtmak istiyorum.'",
                new Choice("Kilise izin vermez")
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("İncil'i bas")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("printed_bible"),
                1.5f, "Basılı İncil"));

            events.Add(CreateEvent("ren_printer_pamphlet", Era.Renaissance, EventCategory.Character, printer,
                "Gutenberg: 'Efendim, politik broşürler basılıyor. Bazıları sizin aleyhine.'",
                new Choice("Matbaayı sansürle")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("censorship"),
                new Choice("Basın özgürlüğü")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("free_press"),
                1.5f, "Basın özgürlüğü"));

            // ISABELLA - Elçi
            events.Add(CreateEvent("ren_ambassador_intro", Era.Renaissance, EventCategory.Character, ambassador,
                "Elçi Isabella huzurunuzda. 'Efendim, Avrupa saraylarında ittifaklar kurabilirim.'",
                new Choice("Kendi başımıza yetiyoruz")
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("ambassador_rejected"),
                new Choice("Diplomatik görev ver")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("ambassador_active"),
                1.5f, "Isabella tanışma"));

            events.Add(CreateEvent("ren_ambassador_marriage", Era.Renaissance, EventCategory.Character, ambassador,
                "Isabella: 'Efendim, Fransa kralı kızını size teklif ediyor. Güçlü ittifak!'",
                new Choice("Bağımsızlık önemli")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Evliliği kabul et")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("french_alliance"),
                1.5f, "Kraliyet evliliği"));

            events.Add(CreateEvent("ren_ambassador_spy", Era.Renaissance, EventCategory.Character, ambassador,
                "Isabella: 'Efendim, rakip ülkelere casus gönderebilirim. Bilgi güçtür.'",
                new Choice("Casusluk onursuz")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Casusları gönder")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -5)
                    .AddFlag("spy_network"),
                1f, "Casus ağı"));

            // ============ NADİR EVENTLER ============

            events.Add(CreateEvent("ren_plague_return", Era.Renaissance, EventCategory.Rare, null,
                "Kara Veba geri döndü! Şehirler boşalıyor, ölüm her yerde.",
                new Choice("Dua et ve bekle")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -25)
                    .AddEffect(ResourceType.Gold, -15),
                new Choice("Karantina uygula")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Military, 10),
                1f, "Veba dönüşü", isRare: true));

            events.Add(CreateEvent("ren_ottoman_threat", Era.Renaissance, EventCategory.Rare, null,
                "Osmanlı ordusu sınırlara dayandı! Konstantinopolis'in kaderini hatırlayın.",
                new Choice("Barış iste")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Savaşa hazırlan")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("ottoman_war"),
                1f, "Osmanlı tehdidi", isRare: true));

            events.Add(CreateEvent("ren_lost_manuscript", Era.Renaissance, EventCategory.Rare, null,
                "Antik bir el yazması bulundu! Aristoteles'in kayıp eseri olabilir.",
                new Choice("Kiliseye ver")
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Üniversiteye ver")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddFlag("ancient_knowledge"),
                1f, "Kayıp el yazması", isRare: true));

            // ============ KOŞULLU EVENTLER ============

            // Düşük para eventi
            var lowGoldEvent = CreateEvent("ren_bankruptcy_warning", Era.Renaissance, EventCategory.Story, medici,
                "Lorenzo: 'Efendim, hazine kritik seviyede. Medici bankası yardım edebilir... bir bedeli var.'",
                new Choice("Kendi başımıza çözeriz")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Medici'den yardım al")
                    .AddEffect(ResourceType.Gold, 25)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("medici_debt"),
                2f, "Mali kriz", priority: 2);
            lowGoldEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Gold,
                conditionOperator = ConditionOperator.LessThan,
                value = 25
            });
            events.Add(lowGoldEvent);

            // Yüksek inanç eventi
            var highFaithEvent = CreateEvent("ren_inquisition", Era.Renaissance, EventCategory.Random, null,
                "Engizisyon güçleniyor. 'Sapkınları temizlemeliyiz!'",
                new Choice("Engizisyonu durdu")
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, 15),
                new Choice("Engizisyona izin ver")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddFlag("inquisition_active"),
                1f, "Engizisyon");
            highFaithEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Faith,
                conditionOperator = ConditionOperator.GreaterThan,
                value = 70
            });
            events.Add(highFaithEvent);

            // ============ GEÇİŞ EVENTLERİ ============

            // Tur 10 - İlk dönüm noktası
            var turn10Event = CreateEvent("ren_turn_10", Era.Renaissance, EventCategory.Story, null,
                "İlk on gününüz Rönesans'ta geçti. Sanat mı yoksa güç mü önceliğiniz?",
                new Choice("Sanat ve kültür")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("culture_focused"),
                new Choice("Güç ve zenginlik")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("power_focused"),
                3f, "Rönesans vizyonu", priority: 5);
            turn10Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 10
            });
            events.Add(turn10Event);

            // Tur 25 - Orta oyun
            var turn25Event = CreateEvent("ren_turn_25", Era.Renaissance, EventCategory.Story, null,
                "Rönesans altın çağında. Sanatçılar ve alimler sarayınızı dolduruyor.",
                new Choice("Mütevazi kal")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Şanını duyur")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -5),
                3f, "Altın çağ", priority: 5);
            turn25Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 25
            });
            events.Add(turn25Event);

            // Tur 50 - Büyük dönüm noktası
            var turn50Event = CreateEvent("ren_turn_50", Era.Renaissance, EventCategory.Story, null,
                "Elli gün Rönesans'ın zirvesinde! Eseriniz nesillere ilham verecek.",
                new Choice("Mirası koru")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Yeni ufuklar aç")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("new_horizons"),
                3f, "Rönesans mirası", priority: 5);
            turn50Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 50
            });
            events.Add(turn50Event);

            // ============ KARAKTER SON EVENTLERİ ============

            // Leonardo final
            events.Add(CreateEvent("ren_leonardo_final", Era.Renaissance, EventCategory.Character, leonardo,
                "Leonardo: 'Efendim, tüm hayatımı size adadım. Eserlerim sizin miraszıdır.'",
                new Choice("Teşekkürler, git")
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Sonsuza dek hatırlanacaksın")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("leonardo_legacy"),
                1f, "Leonardo mirası"));

            // Colombo final
            events.Add(CreateEvent("ren_explorer_final", Era.Renaissance, EventCategory.Character, explorer,
                "Colombo: 'Efendim, yeni dünya sizin. Altın, toprak ve şan... Hepsi sizin!'",
                new Choice("Yeterli keşif")
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("İmparatorluk kur")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("explorer_empire"),
                1f, "Keşif imparatorluğu"));

            // Galileo final
            events.Add(CreateEvent("ren_scientist_final", Era.Renaissance, EventCategory.Character, scientist,
                "Galileo: 'Efendim, gerçek kazandı. Dünya dönüyor ve herkes biliyor!'",
                new Choice("Sessiz ol")
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Bilimi kutla")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("science_triumph"),
                1f, "Bilim zaferi"));

            // Medici final
            events.Add(CreateEvent("ren_medici_final", Era.Renaissance, EventCategory.Character, medici,
                "Lorenzo: 'Efendim, Medici ailesi sonsuza dek sizin hizmetinizde. Birlikte tarihi değiştirdik!'",
                new Choice("İş ortağı kal")
                    .AddEffect(ResourceType.Gold, 15),
                new Choice("Dost ol")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("medici_friendship"),
                1f, "Medici dostluğu"));

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
