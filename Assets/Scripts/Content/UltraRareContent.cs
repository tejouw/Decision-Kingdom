using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Ultra nadir eventler (1/1000 şans)
    /// GAME_DESIGN.md'de belirtilen özel eventler
    /// </summary>
    public static class UltraRareContent
    {
        #region Event IDs
        // Ultra Rare Event IDs
        public const string EVENT_TIME_TRAVELER = "ultra_time_traveler";
        public const string EVENT_PEOPLES_HERO = "ultra_peoples_hero";
        public const string EVENT_PARALLEL_UNIVERSE = "ultra_parallel_universe";
        public const string EVENT_DIVINE_INTERVENTION = "ultra_divine_intervention";

        // Chain Event IDs
        public const string EVENT_TIME_TRAVELER_WARNING = "ultra_time_traveler_warning";
        public const string EVENT_TIME_TRAVELER_CRISIS = "ultra_time_traveler_crisis";
        public const string EVENT_PEOPLES_HERO_RESULT = "ultra_peoples_hero_result";
        public const string EVENT_PARALLEL_UNIVERSE_RETURN = "ultra_parallel_universe_return";
        public const string EVENT_DIVINE_AFTERMATH = "ultra_divine_aftermath";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character("time_traveler", "Gizemli Yabancı", "Zaman Yolcusu")
                {
                    eras = new List<Era> { Era.Medieval, Era.Renaissance, Era.Industrial, Era.Modern, Era.Future },
                    description = "Garip kıyafetler içinde, geleceği bildiğini iddia eden bir yabancı."
                },
                new Character("peoples_hero", "Halk Kahramanı", "Köylü")
                {
                    eras = new List<Era> { Era.Medieval, Era.Renaissance, Era.Industrial, Era.Modern, Era.Future },
                    description = "Sıradan bir köylüyken büyük bir kahramanlık yapan biri."
                },
                new Character("parallel_self", "Paralel Ben", "Ayna")
                {
                    eras = new List<Era> { Era.Medieval, Era.Renaissance, Era.Industrial, Era.Modern, Era.Future },
                    description = "Başka bir gerçeklikten gelen senin bir versiyonun."
                },
                new Character("divine_messenger", "İlahi Elçi", "Melek")
                {
                    eras = new List<Era> { Era.Medieval, Era.Renaissance },
                    description = "Tanrıların gönderdiği kutsal bir varlık."
                }
            };
        }
        #endregion

        #region Events
        public static List<GameEvent> GetEvents()
        {
            var events = new List<GameEvent>();
            var characters = GetCharacters();

            var timeTraveler = characters.Find(c => c.id == "time_traveler");
            var peoplesHero = characters.Find(c => c.id == "peoples_hero");
            var parallelSelf = characters.Find(c => c.id == "parallel_self");
            var divineMessenger = characters.Find(c => c.id == "divine_messenger");

            // ============ ZAMAN YOLCUSU (Time Traveler) ============
            // Tüm dönemlerde görülebilir

            // Medieval versiyon
            events.Add(CreateUltraRareEvent(EVENT_TIME_TRAVELER + "_medieval", Era.Medieval, timeTraveler,
                "Garip kıyafetler içinde bir yabancı sarayda belirdi. 'Gelecekten geliyorum! Üç gün sonra büyük bir kriz yaşanacak. Sizi uyarmaya geldim.'",
                new Choice("Bu adam deli, kovun!")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("Dinle, ne tür bir kriz?")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("time_traveler_believed")
                    .AddTriggeredEvent(EVENT_TIME_TRAVELER_WARNING + "_medieval"),
                "Zaman yolcusu - Ortaçağ"));

            // Zaman yolcusu uyarısı chain eventi
            events.Add(CreateEvent(EVENT_TIME_TRAVELER_WARNING + "_medieval", Era.Medieval, EventCategory.Chain, timeTraveler,
                "Zaman yolcusu açıkladı: 'Komşu krallık gizlice ordu topluyor. Üç gün sonra saldıracaklar. Hazırlık yapmalısınız!'",
                new Choice("Savunmayı güçlendir")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 20)
                    .AddFlag("prepared_for_invasion"),
                new Choice("Diplomasi dene")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("diplomatic_approach"),
                1f, "Zaman yolcusu uyarısı"));

            // Kriz gerçekleşiyor - 3 tur sonra tetiklenir
            var crisisEvent = CreateEvent(EVENT_TIME_TRAVELER_CRISIS + "_medieval", Era.Medieval, EventCategory.Chain, null,
                "Zaman yolcusunun dediği gerçek oldu! Düşman ordusu sınırda!",
                new Choice("Savaş!")
                    .AddEffect(ResourceType.Military, -10, 20)
                    .AddEffect(ResourceType.Gold, -10, 15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("time_crisis_survived"),
                new Choice("Teslim ol")
                    .AddEffect(ResourceType.Military, -20)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Gold, -20),
                2f, "Kriz gerçekleşti", priority: 3);
            crisisEvent.conditions.Add(new Condition
            {
                type = ConditionType.Flag,
                flag = "time_traveler_believed",
                conditionOperator = ConditionOperator.Equal
            });
            events.Add(crisisEvent);

            // Renaissance versiyon
            events.Add(CreateUltraRareEvent(EVENT_TIME_TRAVELER + "_renaissance", Era.Renaissance, timeTraveler,
                "Tuhaf bir makine ile biri belirdi! 'Ben gelecekten geliyorum. Büyük bir keşif çalınacak, onu korumalısınız!'",
                new Choice("Saçmalık, tutukla")
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Military, 5),
                new Choice("Hangi keşif?")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("protected_invention")
                    .AddTriggeredEvent(EVENT_TIME_TRAVELER_WARNING + "_renaissance"),
                "Zaman yolcusu - Rönesans"));

            events.Add(CreateEvent(EVENT_TIME_TRAVELER_WARNING + "_renaissance", Era.Renaissance, EventCategory.Chain, timeTraveler,
                "'Matbaanın planları çalınacak. Casuslar aranızda. Koruyun!'",
                new Choice("Güvenliği artır")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("invention_protected"),
                new Choice("Gizlice araştır")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("spy_hunt"),
                1f, "Keşif uyarısı"));

            // Industrial versiyon
            events.Add(CreateUltraRareEvent(EVENT_TIME_TRAVELER + "_industrial", Era.Industrial, timeTraveler,
                "Parlak metal bir araçtan biri indi. 'Gelecekten geliyorum! Fabrikalarınız patlayacak, işçiler ölecek!'",
                new Choice("İmkansız, güvenlik mükemmel")
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Fabrikaları kontrol et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("factory_disaster_prevented"),
                "Zaman yolcusu - Sanayi"));

            // Modern versiyon
            events.Add(CreateUltraRareEvent(EVENT_TIME_TRAVELER + "_modern", Era.Modern, timeTraveler,
                "Bir hologram belirdi! 'Gelecekten mesaj: Ekonomik kriz yaklaşıyor, piyasalarınızı koruyun!'",
                new Choice("Fake news, yoksay")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Gold, -15),
                new Choice("Önlem al")
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("economic_crisis_prevented"),
                "Zaman yolcusu - Modern"));

            // Future versiyon
            events.Add(CreateUltraRareEvent(EVENT_TIME_TRAVELER + "_future", Era.Future, timeTraveler,
                "Kuantum portaldan biri çıktı. 'Daha da gelecekten geliyorum. AI isyanı başlayacak, protokolleri güncelleyin!'",
                new Choice("AI güvenli, paranoya")
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Sistemleri güncelle")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("ai_rebellion_prevented"),
                "Zaman yolcusu - Gelecek"));

            // ============ HALK KAHRAMANI (People's Hero) ============

            // Medieval versiyon
            events.Add(CreateUltraRareEvent(EVENT_PEOPLES_HERO + "_medieval", Era.Medieval, peoplesHero,
                "Sıradan bir köylü tek başına eşkıya çetesini yendi ve tüm köyü kurtardı! Halk onu kral yapmak istiyor!",
                new Choice("Tehdit! Tutukla")
                    .AddEffect(ResourceType.Happiness, -25)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -15),
                new Choice("Şövalye ilan et")
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("peoples_champion")
                    .AddTriggeredEvent(EVENT_PEOPLES_HERO_RESULT + "_medieval"),
                "Halk kahramanı - Ortaçağ"));

            events.Add(CreateEvent(EVENT_PEOPLES_HERO_RESULT + "_medieval", Era.Medieval, EventCategory.Chain, peoplesHero,
                "Yeni şövalyeniz halkın sevgilisi oldu. Onun tavsiyeleriyle yönetmek ister misiniz?",
                new Choice("Sadece savaşsın")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Danışman yap")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("hero_advisor"),
                1f, "Kahraman kaderi"));

            // Renaissance versiyon
            events.Add(CreateUltraRareEvent(EVENT_PEOPLES_HERO + "_renaissance", Era.Renaissance, peoplesHero,
                "Bir ressam yangından çocukları kurtardı ve şehri koruyan bir sistem icat etti! Halk onu belediye başkanı istiyor!",
                new Choice("Sanatına dönsün")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Mühendis yap")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Gold, 15)
                    .AddFlag("hero_engineer"),
                "Halk kahramanı - Rönesans"));

            // Industrial versiyon
            events.Add(CreateUltraRareEvent(EVENT_PEOPLES_HERO + "_industrial", Era.Industrial, peoplesHero,
                "Bir işçi maden çöküşünde 50 kişiyi kurtardı! Sendikalar onu lider yapmak istiyor.",
                new Choice("Tehlikeli, sürgün et")
                    .AddEffect(ResourceType.Happiness, -30)
                    .AddEffect(ResourceType.Military, 5),
                new Choice("Güvenlik müdürü yap")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("hero_safety_chief"),
                "Halk kahramanı - Sanayi"));

            // Modern versiyon
            events.Add(CreateUltraRareEvent(EVENT_PEOPLES_HERO + "_modern", Era.Modern, peoplesHero,
                "Bir öğretmen terör saldırısını önledi ve viral oldu! Milyonlar onu başkan adayı istiyor!",
                new Choice("Siyasete karışmasın")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Kabineye al")
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("hero_minister"),
                "Halk kahramanı - Modern"));

            // Future versiyon
            events.Add(CreateUltraRareEvent(EVENT_PEOPLES_HERO + "_future", Era.Future, peoplesHero,
                "Bir teknisyen AI arızasını düzeltip şehri kurtardı! İnsanlar onu 'Son Kahraman' ilan ediyor!",
                new Choice("Ödül ver, gönder")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Baş Teknisyen yap")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddFlag("hero_tech_chief"),
                "Halk kahramanı - Gelecek"));

            // ============ PARALEL EVREN (Parallel Universe) ============

            // Medieval versiyon
            events.Add(CreateUltraRareEvent(EVENT_PARALLEL_UNIVERSE + "_medieval", Era.Medieval, parallelSelf,
                "Bir ayna parladı ve içinden... SEN çıktın! Farklı kararlar almış bir versiyonun. 'Ben tiranlığı seçtim, krallığım yıkıldı. Sen doğru yoldasın.'",
                new Choice("Bu büyücülük!")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Deneyimlerinden öğren")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("parallel_wisdom"),
                "Paralel evren - Ortaçağ"));

            // Renaissance versiyon
            events.Add(CreateUltraRareEvent(EVENT_PARALLEL_UNIVERSE + "_renaissance", Era.Renaissance, parallelSelf,
                "Bir resimden çıkan sen! 'Benim dünyamda sanatı yasakladım, karanlık çağ başladı. Sanatı koru!'",
                new Choice("İmkansız, hayal görüyorum")
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Tavsiyesini dinle")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, 15)
                    .AddFlag("art_patron"),
                "Paralel evren - Rönesans"));

            // Industrial versiyon
            events.Add(CreateUltraRareEvent(EVENT_PARALLEL_UNIVERSE + "_industrial", Era.Industrial, parallelSelf,
                "Bir makineden çıkan sen! 'Ben işçileri ezdim, devrim oldu. Onları koru!'",
                new Choice("İşçiler araç")
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("İşçi haklarını koru")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("worker_rights"),
                "Paralel evren - Sanayi"));

            // Modern versiyon
            events.Add(CreateUltraRareEvent(EVENT_PARALLEL_UNIVERSE + "_modern", Era.Modern, parallelSelf,
                "Ekrandan çıkan sen! 'Ben medyayı kontrol ettim, gerçek kayboldu. Basın özgürlüğünü koru!'",
                new Choice("Medya tehlikeli")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Basın özgürlüğünü koru")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("press_freedom"),
                "Paralel evren - Modern"));

            // Future versiyon
            events.Add(CreateUltraRareEvent(EVENT_PARALLEL_UNIVERSE + "_future", Era.Future, parallelSelf,
                "Hologramdan çıkan sen! 'Ben AI'yı tanrı yaptım, insanlık köle oldu. Teknolojiyi kontrol et!'",
                new Choice("AI geleceğimiz")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Military, 10),
                new Choice("İnsan kontrolünü koru")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("human_control"),
                "Paralel evren - Gelecek"));

            // ============ TANRILAR MÜDAHALESİ (Divine Intervention) ============

            // Medieval versiyon
            events.Add(CreateUltraRareEvent(EVENT_DIVINE_INTERVENTION + "_medieval", Era.Medieval, divineMessenger,
                "Gökyüzü yarıldı! Işıktan bir varlık belirdi: 'Tanrılar seni izliyor. Adaletli yönetirsen kutsal güç bahşedeceğiz.'",
                new Choice("Şeytani aldatmaca!")
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Military, 10),
                new Choice("Diz çök ve şükret")
                    .AddEffect(ResourceType.Faith, 40)
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("divine_blessing")
                    .AddTriggeredEvent(EVENT_DIVINE_AFTERMATH + "_medieval"),
                "İlahi müdahale - Ortaçağ"));

            events.Add(CreateEvent(EVENT_DIVINE_AFTERMATH + "_medieval", Era.Medieval, EventCategory.Chain, divineMessenger,
                "İlahi güç krallığa yayıldı! Hastalıklar iyileşiyor, ekinler büyüyor. Ama kilise bu güçü kontrol etmek istiyor.",
                new Choice("Gücü halka dağıt")
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("divine_for_people"),
                new Choice("Kiliseye emanet et")
                    .AddEffect(ResourceType.Faith, 30)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("divine_for_church"),
                1f, "İlahi güç yönetimi"));

            // Renaissance versiyon
            events.Add(CreateUltraRareEvent(EVENT_DIVINE_INTERVENTION + "_renaissance", Era.Renaissance, divineMessenger,
                "Bir heykelden ışık saçıldı ve bir ses konuştu: 'Sanat ve bilim tanrıların armağanı. Koruyucuları ödüllendir.'",
                new Choice("Paganizm, yak!")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -20),
                new Choice("Sanatçılara fon ayır")
                    .AddEffect(ResourceType.Faith, 25)
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Gold, -15)
                    .AddFlag("renaissance_patron"),
                "İlahi müdahale - Rönesans"));

            return events;
        }
        #endregion

        #region Helper Methods
        private static GameEvent CreateUltraRareEvent(string id, Era era, Character character,
            string text, Choice leftChoice, Choice rightChoice, string description)
        {
            return new GameEvent
            {
                id = id,
                era = era,
                category = EventCategory.Rare,
                character = character,
                text = text,
                leftChoice = leftChoice,
                rightChoice = rightChoice,
                weight = 0.001f, // 1/1000 şans
                description = description,
                priority = 10, // Ultra rare eventler yüksek öncelikli
                isRare = true,
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
