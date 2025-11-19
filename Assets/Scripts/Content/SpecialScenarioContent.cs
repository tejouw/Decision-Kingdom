using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Özel senaryolar için benzersiz event chain'leri
    /// GAME_DESIGN.md: Cadı Avı, Fransız Devrimi, Nükleer Çağ
    /// </summary>
    public static class SpecialScenarioContent
    {
        #region Scenario IDs
        // Cadı Avı Senaryosu
        public const string SCENARIO_WITCH_HUNT = "witch_hunt";
        public const string EVENT_WITCH_HUNT_START = "witch_hunt_start";
        public const string EVENT_WITCH_HUNT_ACCUSATION = "witch_hunt_accusation";
        public const string EVENT_WITCH_HUNT_TRIAL = "witch_hunt_trial";
        public const string EVENT_WITCH_HUNT_SPREAD = "witch_hunt_spread";
        public const string EVENT_WITCH_HUNT_CLIMAX = "witch_hunt_climax";
        public const string EVENT_WITCH_HUNT_END = "witch_hunt_end";

        // Fransız Devrimi Senaryosu
        public const string SCENARIO_FRENCH_REVOLUTION = "french_revolution";
        public const string EVENT_REVOLUTION_START = "revolution_start";
        public const string EVENT_REVOLUTION_ASSEMBLY = "revolution_assembly";
        public const string EVENT_REVOLUTION_BASTILLE = "revolution_bastille";
        public const string EVENT_REVOLUTION_TERROR = "revolution_terror";
        public const string EVENT_REVOLUTION_CHOICE = "revolution_choice";
        public const string EVENT_REVOLUTION_END = "revolution_end";

        // Nükleer Çağ Senaryosu
        public const string SCENARIO_NUCLEAR_AGE = "nuclear_age";
        public const string EVENT_NUCLEAR_START = "nuclear_start";
        public const string EVENT_NUCLEAR_TEST = "nuclear_test";
        public const string EVENT_NUCLEAR_CRISIS = "nuclear_crisis";
        public const string EVENT_NUCLEAR_ESCALATION = "nuclear_escalation";
        public const string EVENT_NUCLEAR_CHOICE = "nuclear_choice";
        public const string EVENT_NUCLEAR_END = "nuclear_end";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                // Cadı Avı karakterleri
                new Character("witch_hunter", "Matthias", "Cadı Avcısı")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Fanatik bir cadı avcısı. Her yerde şeytanın izini arıyor."
                },
                new Character("accused_witch", "Agnes", "Suçlanan Kadın")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Cadılıkla suçlanan masum bir şifacı."
                },
                new Character("village_elder", "Wilhelm", "Köy Büyüğü")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Akıllı ve temkinli bir köy lideri."
                },

                // Fransız Devrimi karakterleri
                new Character("revolutionary_leader", "Jean-Pierre", "Devrimci Lider")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Halkın haklarını savunan karizmatik bir lider."
                },
                new Character("royal_advisor", "Comte de Valois", "Kraliyet Danışmanı")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Eski düzenin savunucusu, aristokrat danışman."
                },
                new Character("common_citizen", "Marie", "Sıradan Vatandaş")
                {
                    eras = new List<Era> { Era.Renaissance },
                    description = "Ekmek için mücadele eden bir anne."
                },

                // Nükleer Çağ karakterleri
                new Character("military_general", "General Morrison", "Askeri Komutan")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Soğuk savaş döneminin sert askeri lideri."
                },
                new Character("scientist", "Dr. Oppenheimer", "Nükleer Fizikçi")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Atom bombasının yaratıcısı, vicdan azabı çeken bir dahi."
                },
                new Character("diplomat", "Ambassador Chen", "Diplomat")
                {
                    eras = new List<Era> { Era.Modern },
                    description = "Barış için çalışan deneyimli bir diplomat."
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
            var witchHunter = characters.Find(c => c.id == "witch_hunter");
            var accusedWitch = characters.Find(c => c.id == "accused_witch");
            var villageElder = characters.Find(c => c.id == "village_elder");
            var revolutionaryLeader = characters.Find(c => c.id == "revolutionary_leader");
            var royalAdvisor = characters.Find(c => c.id == "royal_advisor");
            var commonCitizen = characters.Find(c => c.id == "common_citizen");
            var militaryGeneral = characters.Find(c => c.id == "military_general");
            var scientist = characters.Find(c => c.id == "scientist");
            var diplomat = characters.Find(c => c.id == "diplomat");

            // ============ CADI AVI SENARYOSU ============

            // Senaryo başlangıç eventi
            events.Add(CreateScenarioStartEvent(
                EVENT_WITCH_HUNT_START,
                Era.Medieval,
                witchHunter,
                @"🔥 CADI AVI BAŞLIYOR 🔥

Karanlık günler... Köylerde garip olaylar yaşanıyor. Hayvanlar ölüyor, ekinler kuruyor.

Kilise emriyle cadı avcısı Matthias krallığınıza geldi. Yanında kara bir liste ve yakacak odunlar var.

'Majeste, şeytan bu topraklarda. İzin verin, temizleyeyim.'",
                new Choice("Reddet, halk sakin")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("witch_hunt_rejected"),
                new Choice("İzin ver, araştırsın")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("witch_hunt_allowed")
                    .AddTriggeredEvent(EVENT_WITCH_HUNT_ACCUSATION),
                "Cadı avı başlangıcı"));

            // İlk suçlama
            var accusationEvent = CreateEvent(
                EVENT_WITCH_HUNT_ACCUSATION,
                Era.Medieval,
                EventCategory.Chain,
                witchHunter,
                "Matthias ilk suçlamasını yaptı! Köyün şifacısı Agnes cadılıkla suçlanıyor. 'Bu kadın şeytanla konuşuyor!'",
                new Choice("Kanıt iste")
                    .AddEffect(ResourceType.Faith, -5)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddTriggeredEvent(EVENT_WITCH_HUNT_TRIAL),
                new Choice("Hemen yargıla")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddTriggeredEvent(EVENT_WITCH_HUNT_SPREAD),
                1f, "İlk suçlama", priority: 5);
            accusationEvent.conditions.Add(new Condition
            {
                type = ConditionType.Flag,
                flag = "witch_hunt_allowed",
                conditionOperator = ConditionOperator.Equal
            });
            events.Add(accusationEvent);

            // Mahkeme
            events.Add(CreateEvent(
                EVENT_WITCH_HUNT_TRIAL,
                Era.Medieval,
                EventCategory.Chain,
                villageElder,
                "Köy büyüğü Wilhelm şahitlik ediyor: 'Agnes yıllardır bizi iyileştiriyor. Cadı değil, bilge kadın!'",
                new Choice("Agnes'i affet")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("agnes_saved"),
                new Choice("Yine de yak")
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -25)
                    .AddFlag("agnes_burned")
                    .AddTriggeredEvent(EVENT_WITCH_HUNT_SPREAD),
                1f, "Cadı mahkemesi", priority: 4));

            // Yayılma
            events.Add(CreateEvent(
                EVENT_WITCH_HUNT_SPREAD,
                Era.Medieval,
                EventCategory.Chain,
                witchHunter,
                "Matthias durmuyor! 'Daha çok cadı var! Her köyde aramam lazım!' Halk korku içinde, herkes birbirini suçluyor.",
                new Choice("Durdur!")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("hunt_stopped"),
                new Choice("Devam et")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -30)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddTriggeredEvent(EVENT_WITCH_HUNT_CLIMAX),
                2f, "Av yayılıyor", priority: 3));

            // Doruk nokta
            events.Add(CreateEvent(
                EVENT_WITCH_HUNT_CLIMAX,
                Era.Medieval,
                EventCategory.Chain,
                witchHunter,
                "Dehşet! Matthias şimdi de soyluları suçluyor! 'Danışmanınız da cadı! Yakmalıyız!' Saray panik içinde.",
                new Choice("Matthias'ı tutukla!")
                    .AddEffect(ResourceType.Faith, -25)
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("hunter_arrested")
                    .AddTriggeredEvent(EVENT_WITCH_HUNT_END),
                new Choice("Araştırsın...")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -30)
                    .AddEffect(ResourceType.Military, -20)
                    .AddFlag("total_paranoia"),
                3f, "Doruk nokta", priority: 2));

            // Final
            events.Add(CreateEvent(
                EVENT_WITCH_HUNT_END,
                Era.Medieval,
                EventCategory.Chain,
                villageElder,
                "Cadı avı sona erdi. Köyler yavaş yavaş normale dönüyor. Ama yaralar derin, güven sarsıldı.",
                new Choice("Geçmişi unut")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Anma töreni yap")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("witch_hunt_memorial"),
                1f, "Cadı avı sonu", priority: 1));

            // ============ FRANSIZ DEVRİMİ SENARYOSU ============

            // Senaryo başlangıç eventi
            events.Add(CreateScenarioStartEvent(
                EVENT_REVOLUTION_START,
                Era.Renaissance,
                commonCitizen,
                @"⚔️ DEVRİM KAPIDA ⚔️

Halk aç, soylular şampanya içiyor. Eşitsizlik dayanılmaz hale geldi.

Sokaklar kaynıyor. 'Ekmek istiyoruz!' sesleri sarayın duvarlarından duyuluyor.

Danışmanınız Comte de Valois: 'Bastırın efendim, yoksa krallık düşer!'
Halktan Marie: 'Çocuklarımız aç ölüyor, bizi duyun!'",
                new Choice("Reformlar başlat")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("reforms_started")
                    .AddTriggeredEvent(EVENT_REVOLUTION_ASSEMBLY),
                new Choice("Sert müdahale")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -25)
                    .AddFlag("revolution_suppressed")
                    .AddTriggeredEvent(EVENT_REVOLUTION_BASTILLE),
                "Devrim başlangıcı"));

            // Meclis
            events.Add(CreateEvent(
                EVENT_REVOLUTION_ASSEMBLY,
                Era.Renaissance,
                EventCategory.Chain,
                revolutionaryLeader,
                "Jean-Pierre mecliste konuşuyor: 'Reform yetmez! Anayasa istiyoruz! Eşit haklar istiyoruz!'",
                new Choice("Anayasayı kabul et")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Military, -15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("constitution_accepted"),
                new Choice("Meclisi dağıt")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -30)
                    .AddTriggeredEvent(EVENT_REVOLUTION_BASTILLE),
                1f, "Meclis talebi", priority: 5));

            // Bastille
            var bastilleEvent = CreateEvent(
                EVENT_REVOLUTION_BASTILLE,
                Era.Renaissance,
                EventCategory.Chain,
                revolutionaryLeader,
                "BASTILLE DÜŞTÜ! Halk hapisaneyi bastı! Devrim artık durdurulamaz. Soylular kaçıyor!",
                new Choice("Halka katıl")
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddEffect(ResourceType.Military, -20)
                    .AddEffect(ResourceType.Gold, -15)
                    .AddFlag("joined_revolution")
                    .AddTriggeredEvent(EVENT_REVOLUTION_CHOICE),
                new Choice("Sarayı koru")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -35)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddTriggeredEvent(EVENT_REVOLUTION_TERROR),
                2f, "Bastille baskını", priority: 4);
            events.Add(bastilleEvent);

            // Terör
            events.Add(CreateEvent(
                EVENT_REVOLUTION_TERROR,
                Era.Renaissance,
                EventCategory.Chain,
                revolutionaryLeader,
                "Terör dönemi başladı! Giyotin meydanlarda işliyor. 'Düşmanlar yok edilmeli!' diye haykırıyorlar.",
                new Choice("Kaç!")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Military, -25)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddFlag("king_fled"),
                new Choice("Direnişi örgütle")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Gold, -20)
                    .AddTriggeredEvent(EVENT_REVOLUTION_END),
                2f, "Terör dönemi", priority: 3));

            // Seçim
            events.Add(CreateEvent(
                EVENT_REVOLUTION_CHOICE,
                Era.Renaissance,
                EventCategory.Chain,
                revolutionaryLeader,
                "Jean-Pierre: 'Artık krallar yok! Cumhuriyet ilan ediyoruz! Siz de bizimle misiniz, yoksa...'",
                new Choice("Cumhuriyeti destekle")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("republic_supporter")
                    .AddTriggeredEvent(EVENT_REVOLUTION_END),
                new Choice("Anayasal monarşi")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -10)
                    .AddFlag("constitutional_monarch")
                    .AddTriggeredEvent(EVENT_REVOLUTION_END),
                1f, "Rejim seçimi", priority: 2));

            // Final
            events.Add(CreateEvent(
                EVENT_REVOLUTION_END,
                Era.Renaissance,
                EventCategory.Chain,
                commonCitizen,
                "Devrim sona erdi. Dünya asla eskisi gibi olmayacak. Yeni bir çağ başlıyor.",
                new Choice("Yeniden inşa et")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Güçlü devlet kur")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddFlag("revolution_complete"),
                1f, "Devrim sonu", priority: 1));

            // ============ NÜKLEER ÇAĞ SENARYOSU ============

            // Senaryo başlangıç eventi
            events.Add(CreateScenarioStartEvent(
                EVENT_NUCLEAR_START,
                Era.Modern,
                scientist,
                @"☢️ NÜKLEER ÇAĞ BAŞLIYOR ☢️

Atom parçalandı. İnsanlık tanrıların gücünü elde etti.

Dr. Oppenheimer: 'Ben ölümün kendisi oldum, dünyaların yok edicisi.'

İki süper güç karşı karşıya. Nükleer silahlar çoğalıyor. Bir hata, insanlığın sonu olabilir.

General Morrison: 'Güçlü olan kazanır!'
Ambassador Chen: 'Barış tek yol!'",
                new Choice("Silahlanmayı artır")
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("arms_race")
                    .AddTriggeredEvent(EVENT_NUCLEAR_TEST),
                new Choice("Barış görüşmeleri")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -10)
                    .AddFlag("peace_talks")
                    .AddTriggeredEvent(EVENT_NUCLEAR_CRISIS),
                "Nükleer çağ başlangıcı"));

            // Test
            var testEvent = CreateEvent(
                EVENT_NUCLEAR_TEST,
                Era.Modern,
                EventCategory.Chain,
                militaryGeneral,
                "General Morrison: 'Yeni bomba hazır! Test edersek düşmanlarımız korkacak. Ama dünya bizi kınayabilir.'",
                new Choice("Test et")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("nuke_tested")
                    .AddTriggeredEvent(EVENT_NUCLEAR_ESCALATION),
                new Choice("Gizli tut")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddTriggeredEvent(EVENT_NUCLEAR_CRISIS),
                1f, "Nükleer test", priority: 5);
            testEvent.conditions.Add(new Condition
            {
                type = ConditionType.Flag,
                flag = "arms_race",
                conditionOperator = ConditionOperator.Equal
            });
            events.Add(testEvent);

            // Kriz
            events.Add(CreateEvent(
                EVENT_NUCLEAR_CRISIS,
                Era.Modern,
                EventCategory.Chain,
                diplomat,
                "KRİZ! Düşman gemileri sularımıza girdi! Füzeler hazır! Dünya nükleer savaşın eşiğinde!",
                new Choice("Karşı saldırı emri")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -30)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddTriggeredEvent(EVENT_NUCLEAR_ESCALATION),
                new Choice("Diplomasi dene")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Military, -10)
                    .AddTriggeredEvent(EVENT_NUCLEAR_CHOICE),
                3f, "Nükleer kriz", priority: 4));

            // Tırmanma
            events.Add(CreateEvent(
                EVENT_NUCLEAR_ESCALATION,
                Era.Modern,
                EventCategory.Chain,
                militaryGeneral,
                "ALARM! Radarlar düşman füzelerini tespit etti! 15 dakikanız var. Karşı saldırı mı?",
                new Choice("Ateş!")
                    .AddEffect(ResourceType.Military, -50)
                    .AddEffect(ResourceType.Happiness, -50)
                    .AddEffect(ResourceType.Gold, -50)
                    .AddEffect(ResourceType.Faith, -50)
                    .AddFlag("nuclear_war"),
                new Choice("Bekle ve doğrula")
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddTriggeredEvent(EVENT_NUCLEAR_CHOICE),
                5f, "Nükleer tırmanma", priority: 3));

            // Seçim
            events.Add(CreateEvent(
                EVENT_NUCLEAR_CHOICE,
                Era.Modern,
                EventCategory.Chain,
                scientist,
                "Dr. Oppenheimer: 'İnsanlığın kaderi elinizde. Silahsızlanma mı, caydırıcılık mı?'",
                new Choice("Silahsızlanma")
                    .AddEffect(ResourceType.Faith, 25)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, -25)
                    .AddFlag("disarmament")
                    .AddTriggeredEvent(EVENT_NUCLEAR_END),
                new Choice("Güçlü ol, caydır")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("deterrence")
                    .AddTriggeredEvent(EVENT_NUCLEAR_END),
                1f, "Nükleer seçim", priority: 2));

            // Final
            events.Add(CreateEvent(
                EVENT_NUCLEAR_END,
                Era.Modern,
                EventCategory.Chain,
                diplomat,
                "Soğuk savaş bitmiyor ama dünya nükleer yıkımdan kurtuldu. Şimdilik...",
                new Choice("Uluslararası işbirliği")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("Ulusal güvenlik öncelik")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("nuclear_scenario_complete"),
                1f, "Nükleer çağ devam ediyor", priority: 1));

            return events;
        }
        #endregion

        #region Scenario Definitions
        /// <summary>
        /// Özel senaryo tanımları (PrestigeManager ile entegrasyon için)
        /// </summary>
        public static readonly Dictionary<string, SpecialScenarioInfo> SpecialScenarios = new Dictionary<string, SpecialScenarioInfo>
        {
            [SCENARIO_WITCH_HUNT] = new SpecialScenarioInfo
            {
                id = SCENARIO_WITCH_HUNT,
                name = "Cadı Avı Başlangıcı",
                description = "Ortaçağ'da karanlık bir dönem. Cadı avcıları krallığınıza geldi. İnanç ve adalet arasında seçim yapmalısınız.",
                era = Era.Medieval,
                unlockCost = 150,
                startingResources = new Resources(45, 40, 45, 70), // Yüksek Faith
                startEventId = EVENT_WITCH_HUNT_START,
                startFlags = new List<string> { "witch_hunt_scenario" }
            },
            [SCENARIO_FRENCH_REVOLUTION] = new SpecialScenarioInfo
            {
                id = SCENARIO_FRENCH_REVOLUTION,
                name = "Fransız Devrimi",
                description = "Rönesans'ın sonunda büyük değişim. Halk ayaklanıyor. Devrime katılacak mısınız, yoksa bastıracak mısınız?",
                era = Era.Renaissance,
                unlockCost = 175,
                startingResources = new Resources(30, 25, 50, 40), // Düşük Gold ve Happiness
                startEventId = EVENT_REVOLUTION_START,
                startFlags = new List<string> { "revolution_scenario" }
            },
            [SCENARIO_NUCLEAR_AGE] = new SpecialScenarioInfo
            {
                id = SCENARIO_NUCLEAR_AGE,
                name = "Nükleer Çağ",
                description = "Modern dönemde soğuk savaş dorukta. Nükleer silahlar insanlığı tehdit ediyor. Barış mı, güç mü?",
                era = Era.Modern,
                unlockCost = 200,
                startingResources = new Resources(50, 40, 70, 35), // Yüksek Military
                startEventId = EVENT_NUCLEAR_START,
                startFlags = new List<string> { "nuclear_scenario" }
            }
        };
        #endregion

        #region Helper Methods
        private static GameEvent CreateScenarioStartEvent(string id, Era era, Character character,
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
                weight = 1000f, // Senaryo başlangıcı - en yüksek öncelik
                description = description,
                priority = 100,
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

    #region Data Classes
    /// <summary>
    /// Özel senaryo bilgisi
    /// </summary>
    [System.Serializable]
    public class SpecialScenarioInfo
    {
        public string id;
        public string name;
        public string description;
        public Era era;
        public int unlockCost;
        public Resources startingResources;
        public string startEventId;
        public List<string> startFlags;
    }
    #endregion
}
