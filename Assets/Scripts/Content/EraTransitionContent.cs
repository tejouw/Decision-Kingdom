using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Dönem geçiş eventleri - "Sinematik" geçişler
    /// GAME_DESIGN.md: Özel transition eventleri
    /// </summary>
    public static class EraTransitionContent
    {
        #region Event IDs
        public const string TRANSITION_MEDIEVAL_TO_RENAISSANCE = "transition_medieval_renaissance";
        public const string TRANSITION_RENAISSANCE_TO_INDUSTRIAL = "transition_renaissance_industrial";
        public const string TRANSITION_INDUSTRIAL_TO_MODERN = "transition_industrial_modern";
        public const string TRANSITION_MODERN_TO_FUTURE = "transition_modern_future";

        // Geçiş sonrası ilk eventler
        public const string FIRST_RENAISSANCE = "first_event_renaissance";
        public const string FIRST_INDUSTRIAL = "first_event_industrial";
        public const string FIRST_MODERN = "first_event_modern";
        public const string FIRST_FUTURE = "first_event_future";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character("historian", "Tarihçi", "Kayıt Tutucu")
                {
                    eras = new List<Era> { Era.Medieval, Era.Renaissance, Era.Industrial, Era.Modern, Era.Future },
                    description = "Zamanın geçişini anlatan bilge bir tarihçi."
                },
                new Character("timekeeper", "Zaman Bekçisi", "Çağların Tanığı")
                {
                    eras = new List<Era> { Era.Medieval, Era.Renaissance, Era.Industrial, Era.Modern, Era.Future },
                    description = "Dönemler arası geçişi yöneten mistik bir varlık."
                }
            };
        }
        #endregion

        #region Transition Events
        public static List<GameEvent> GetEvents()
        {
            var events = new List<GameEvent>();
            var characters = GetCharacters();
            var historian = characters.Find(c => c.id == "historian");
            var timekeeper = characters.Find(c => c.id == "timekeeper");

            // ============ ORTAÇAĞ → RÖNESANS ============
            events.Add(CreateTransitionEvent(
                TRANSITION_MEDIEVAL_TO_RENAISSANCE,
                Era.Medieval,
                timekeeper,
                @"⏳ YÜZYILLAR GEÇTİ... ⏳

Torununun torunu şimdi tahtında. Dünya değişti.

Karanlık çağlar geride kaldı. Sanat ve bilim yükseliyor. Şehirler büyüyor, tüccarlar zenginleşiyor, yeni fikirler yayılıyor.

Artık kılıç değil, kalem daha güçlü. Artık inanç değil, bilgi daha değerli.

RÖNESANS BAŞLIYOR.

Bu yeni çağda nasıl yöneteceksin?",
                new Choice("Geleneklere sadık kal")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("traditional_ruler")
                    .AddTriggeredEvent(FIRST_RENAISSANCE + "_traditional"),
                new Choice("Yeniliklere açık ol")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, -5)
                    .AddFlag("progressive_ruler")
                    .AddTriggeredEvent(FIRST_RENAISSANCE + "_progressive"),
                "Ortaçağ'dan Rönesans'a geçiş"));

            // Rönesans ilk event - Geleneksel
            events.Add(CreateFirstEraEvent(
                FIRST_RENAISSANCE + "_traditional",
                Era.Renaissance,
                historian,
                "Yeni çağ başladı ama siz eski değerlere bağlısınız. Kilise sizi destekliyor, ama genç nesil huzursuz.",
                new Choice("Otoriteyi koru")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Uzlaşma ara")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Faith, -5),
                "Rönesans başlangıcı - Geleneksel"));

            // Rönesans ilk event - Yenilikçi
            events.Add(CreateFirstEraEvent(
                FIRST_RENAISSANCE + "_progressive",
                Era.Renaissance,
                historian,
                "Yeniliklere açık bir hükümdar olarak tanınıyorsunuz. Sanatçılar ve bilim adamları sarayınıza akın ediyor.",
                new Choice("Sanatı destekle")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Bilimi destekle")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Faith, -10),
                "Rönesans başlangıcı - Yenilikçi"));

            // ============ RÖNESANS → SANAYİ DEVRİMİ ============
            events.Add(CreateTransitionEvent(
                TRANSITION_RENAISSANCE_TO_INDUSTRIAL,
                Era.Renaissance,
                timekeeper,
                @"⚙️ ÇARKLAR DÖNMEYE BAŞLADI... ⚙️

Buhar makineleri uğulduyor. Fabrika bacaları gökyüzünü kaplıyor.

Sanat ve felsefe yerini makinelere ve üretime bıraktı. Köyler boşalıyor, şehirler tıka basa doluyor. İşçi sınıfı doğuyor.

Artık toprağın bereketi değil, fabrikanın verimliliği önemli. Artık tanrının lütfu değil, bilimin gücü belirleyici.

SANAYİ DEVRİMİ BAŞLIYOR.

Bu duman kaplı çağda nasıl yöneteceksin?",
                new Choice("Sanayileşmeyi hızlandır")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("industrialist")
                    .AddTriggeredEvent(FIRST_INDUSTRIAL + "_rapid"),
                new Choice("İşçileri koru")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("worker_protector")
                    .AddTriggeredEvent(FIRST_INDUSTRIAL + "_balanced"),
                "Rönesans'tan Sanayi Devrimi'ne geçiş"));

            // Sanayi ilk event - Hızlı
            events.Add(CreateFirstEraEvent(
                FIRST_INDUSTRIAL + "_rapid",
                Era.Industrial,
                historian,
                "Fabrikalarınız gece gündüz çalışıyor. Zenginlik akıyor ama işçiler tükenmiş durumda. Grevler başlıyor.",
                new Choice("Grevleri bastır")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -20),
                new Choice("Müzakere et")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 5),
                "Sanayi başlangıcı - Hızlı"));

            // Sanayi ilk event - Dengeli
            events.Add(CreateFirstEraEvent(
                FIRST_INDUSTRIAL + "_balanced",
                Era.Industrial,
                historian,
                "İşçi hakları için mücadele ettiniz. Halk sizi seviyor ama rakipleriniz daha hızlı sanayileşiyor.",
                new Choice("Rekabete katıl")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Kaliteye odaklan")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5),
                "Sanayi başlangıcı - Dengeli"));

            // ============ SANAYİ DEVRİMİ → MODERN DÖNEM ============
            events.Add(CreateTransitionEvent(
                TRANSITION_INDUSTRIAL_TO_MODERN,
                Era.Industrial,
                timekeeper,
                @"📺 DÜNYA KÜÇÜLDÜ... 📺

İki dünya savaşı geride kaldı. Milyonlar öldü, imparatorluklar yıkıldı.

Artık diktatörler değil, demokratlar yönetiyor. Artık fabrikalar değil, medya güç demek. Televizyonlar her eve girdi, bilgi anında yayılıyor.

Nükleer güç hem umut hem korku. Soğuk savaş her yerde. Uzay yarışı başladı.

MODERN DÖNEM BAŞLIYOR.

Bu medya çağında nasıl yöneteceksin?",
                new Choice("Güçlü liderlik")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("strong_leader")
                    .AddTriggeredEvent(FIRST_MODERN + "_authoritarian"),
                new Choice("Demokratik değerler")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("democratic_leader")
                    .AddTriggeredEvent(FIRST_MODERN + "_democratic"),
                "Sanayi Devrimi'nden Modern Dönem'e geçiş"));

            // Modern ilk event - Otoriter
            events.Add(CreateFirstEraEvent(
                FIRST_MODERN + "_authoritarian",
                Era.Modern,
                historian,
                "Güçlü bir lider olarak tanınıyorsunuz. Düzen var ama özgürlükler kısıtlı. Muhalefet büyüyor.",
                new Choice("Baskıyı artır")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Reformlar başlat")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -10),
                "Modern başlangıcı - Otoriter"));

            // Modern ilk event - Demokratik
            events.Add(CreateFirstEraEvent(
                FIRST_MODERN + "_democratic",
                Era.Modern,
                historian,
                "Demokratik değerleriniz takdir ediliyor. Seçimler yaklaşıyor ve her partinin farklı vaatleri var.",
                new Choice("Ekonomiye odaklan")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("Sosyal adalete odaklan")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -10),
                "Modern başlangıcı - Demokratik"));

            // ============ MODERN DÖNEM → GELECEK ============
            events.Add(CreateTransitionEvent(
                TRANSITION_MODERN_TO_FUTURE,
                Era.Modern,
                timekeeper,
                @"🚀 İNSANLIK YILDIZLARA UZANIYOR... 🚀

Yapay zeka, genetik mühendislik, kuantum bilgisayarlar... Bilim kurgu gerçek oldu.

Mars'ta koloniler kuruluyor. Robotlar her yerde. İnsanlar makinelerle birleşiyor. Ölümsüzlük artık bir hayal değil.

Ama sorular da büyük: İnsan nedir? Bilinç nedir? Makine ile insan arasındaki sınır nerede?

GELECEK BAŞLIYOR.

Bu yeni dünyada nasıl yöneteceksin?",
                new Choice("Teknolojiye güven")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("technocrat")
                    .AddTriggeredEvent(FIRST_FUTURE + "_tech"),
                new Choice("İnsanlığı koru")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("humanist")
                    .AddTriggeredEvent(FIRST_FUTURE + "_human"),
                "Modern Dönem'den Gelecek'e geçiş"));

            // Gelecek ilk event - Teknokrat
            events.Add(CreateFirstEraEvent(
                FIRST_FUTURE + "_tech",
                Era.Future,
                historian,
                "Teknolojiye olan güveniniz meyvelerini veriyor. AI sistemleri her şeyi yönetiyor ama bazıları 'insan değerleri' için endişeli.",
                new Choice("AI'ya tam yetki")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("İnsan gözetimi koru")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5),
                "Gelecek başlangıcı - Teknokrat"));

            // Gelecek ilk event - Hümanist
            events.Add(CreateFirstEraEvent(
                FIRST_FUTURE + "_human",
                Era.Future,
                historian,
                "İnsanlığı koruma kararınız saygı görüyor. Ama teknoloji devleri sizden hoşnut değil, yatırımlar azalıyor.",
                new Choice("Bağımsız teknoloji geliştir")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Uzlaşma ara")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, -5),
                "Gelecek başlangıcı - Hümanist"));

            // ============ ZAFER FİNALLERİ ============

            // Final event - Tüm çağları tamamladın
            events.Add(CreateEvent(
                "final_all_eras_complete",
                Era.Future,
                EventCategory.Story,
                timekeeper,
                @"🏆 TARİHİN SONUNA ULAŞTIN 🏆

Beş çağ boyunca hüküm sürdün. Ortaçağ'ın karanlığından, Geleceğin ışığına.

Krallığından imparatorluğa, imparatorluktan ulus devlete, ulus devletten küresel birliğe. Her çağda farklı zorluklar, farklı kararlar.

Ve şimdi, tarihin ötesinde, yeni bir sayfa açılıyor...

ZAMAN EFSANESİ OLDUN!",
                new Choice("Mirası koru")
                    .AddEffect(ResourceType.Faith, 30)
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddFlag("time_legend_keeper"),
                new Choice("Yeni maceraya başla")
                    .AddEffect(ResourceType.Gold, 30)
                    .AddEffect(ResourceType.Military, 30)
                    .AddFlag("time_legend_explorer"),
                1f, "Final - Tüm çağlar tamamlandı", priority: 10));

            return events;
        }
        #endregion

        #region Helper Methods
        private static GameEvent CreateTransitionEvent(string id, Era era, Character character,
            string text, Choice leftChoice, Choice rightChoice, string description)
        {
            return new GameEvent
            {
                id = id,
                era = era,
                category = EventCategory.Story,
                character = character,
                text = text,
                leftChoice = leftChoice,
                rightChoice = rightChoice,
                weight = 100f, // Çok yüksek öncelik - mutlaka gösterilmeli
                description = description,
                priority = 100, // En yüksek öncelik
                isRare = false,
                conditions = new List<Condition>()
            };
        }

        private static GameEvent CreateFirstEraEvent(string id, Era era, Character character,
            string text, Choice leftChoice, Choice rightChoice, string description)
        {
            return new GameEvent
            {
                id = id,
                era = era,
                category = EventCategory.Chain,
                character = character,
                text = text,
                leftChoice = leftChoice,
                rightChoice = rightChoice,
                weight = 50f,
                description = description,
                priority = 50,
                isRare = false,
                conditions = new List<Condition>()
            };
        }

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
