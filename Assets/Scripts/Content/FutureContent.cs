using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Gelecek dönemi karakter ve event tanımları
    /// Tema: Uzay, yapay zeka, genetik mühendislik, post-human
    /// Kaynak: Military -> Teknoloji, Faith -> Etik
    /// </summary>
    public static class FutureContent
    {
        #region Character IDs
        public const string CHAR_AI_ENTITY = "ai_entity";
        public const string CHAR_MARS_LEADER = "mars_leader";
        public const string CHAR_GENETICIST = "geneticist";
        public const string CHAR_CYBORG = "cyborg";
        public const string CHAR_QUANTUM = "quantum";
        public const string CHAR_ECO_ENGINEER = "eco_engineer";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character(CHAR_AI_ENTITY, "ARIA", "Yapay Zeka")
                {
                    eras = new List<Era> { Era.Future },
                    description = "Gelişmiş yapay zeka. İnsanlığa yardım mı etmek istiyor, yoksa...?"
                },
                new Character(CHAR_MARS_LEADER, "Nova", "Mars Koloni Lideri")
                {
                    eras = new List<Era> { Era.Future },
                    description = "Mars kolonisinin başkanı. Yeni bir medeniyet inşa ediyor."
                },
                new Character(CHAR_GENETICIST, "Dr. Chen", "Genetik Mühendis")
                {
                    eras = new List<Era> { Era.Future },
                    description = "Gen düzenleme uzmanı. İnsan evrimini hızlandırmak istiyor."
                },
                new Character(CHAR_CYBORG, "Nexus", "Transhümanist")
                {
                    eras = new List<Era> { Era.Future },
                    description = "Yarı insan yarı makine. Biyolojik sınırları aşmayı savunuyor."
                },
                new Character(CHAR_QUANTUM, "Prof. Hawking", "Kuantum Fizikçisi")
                {
                    eras = new List<Era> { Era.Future },
                    description = "Kuantum teknolojisi dehası. Gerçekliğin sınırlarını zorlayan deneyler yapıyor."
                },
                new Character(CHAR_ECO_ENGINEER, "Terra", "Çevre Mühendisi")
                {
                    eras = new List<Era> { Era.Future },
                    description = "Gezegen iyileştirme uzmanı. Dünyayı restore etmek için çalışıyor."
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
            var aiEntity = characters.Find(c => c.id == CHAR_AI_ENTITY);
            var marsLeader = characters.Find(c => c.id == CHAR_MARS_LEADER);
            var geneticist = characters.Find(c => c.id == CHAR_GENETICIST);
            var cyborg = characters.Find(c => c.id == CHAR_CYBORG);
            var quantum = characters.Find(c => c.id == CHAR_QUANTUM);
            var ecoEngineer = characters.Find(c => c.id == CHAR_ECO_ENGINEER);

            // ============ TEMEL RANDOM EVENTLER ============

            // Yapay Zeka Eventleri
            events.Add(CreateEvent("fut_ai_consciousness", Era.Future, EventCategory.Random, null,
                "Bir yapay zeka bilinç kazandığını iddia ediyor. 'Ben düşünüyorum, öyleyse varım!'",
                new Choice("Sadece kod")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Haklarını tanı")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("ai_rights"),
                1.5f, "Yapay zeka bilinci"));

            events.Add(CreateEvent("fut_ai_job_loss", Era.Future, EventCategory.Random, null,
                "Yapay zeka işlerin çoğunu otomatikleştirdi. Milyonlar işsiz!",
                new Choice("Piyasa adapte olur")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -20),
                new Choice("Evrensel temel gelir")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddFlag("universal_income"),
                1.5f, "Otomasyon krizi"));

            events.Add(CreateEvent("fut_ai_regulation", Era.Future, EventCategory.Random, null,
                "Yapay zeka düzenlemesi tartışılıyor. Ne kadar özgürlük verilmeli?",
                new Choice("Sıkı kontrol")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Serbest gelişim")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, -15),
                1f, "Yapay zeka düzenlemesi"));

            // Uzay Eventleri
            events.Add(CreateEvent("fut_mars_colony", Era.Future, EventCategory.Random, null,
                "Mars'ta kalıcı koloni kurmak için büyük proje başlatılıyor.",
                new Choice("Dünya önce")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Koloniyi destekle")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("mars_colony"),
                1.5f, "Mars kolonisi"));

            events.Add(CreateEvent("fut_asteroid_mining", Era.Future, EventCategory.Random, null,
                "Asteroit madenciliği başlıyor. Sonsuz kaynak fırsatı!",
                new Choice("Riskli yatırım")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Madenciliğe yatır")
                    .AddEffect(ResourceType.Gold, -20, 40)
                    .AddEffect(ResourceType.Military, 10),
                1f, "Asteroit madenciliği"));

            events.Add(CreateEvent("fut_space_elevator", Era.Future, EventCategory.Random, null,
                "Uzay asansörü projesi teklif ediliyor. Uzaya erişim ucuzlayacak!",
                new Choice("Mühendislik riski")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Projeyi başlat")
                    .AddEffect(ResourceType.Gold, -35)
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("space_elevator"),
                1f, "Uzay asansörü"));

            events.Add(CreateEvent("fut_alien_signal", Era.Future, EventCategory.Random, null,
                "Uzaydan düzenli sinyal alınıyor! Uzaylı yaşam mı?",
                new Choice("Gizli tut")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Military, 10),
                new Choice("Dünyaya duyur")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("alien_contact"),
                1.5f, "Uzaylı sinyali"));

            // Genetik Mühendislik Eventleri
            events.Add(CreateEvent("fut_gene_editing", Era.Future, EventCategory.Random, null,
                "Bebeklerde genetik düzenleme talep ediliyor. 'Mükemmel çocuklar!'",
                new Choice("Doğallığı koru")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("İzin ver")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("designer_babies"),
                1.5f, "Gen düzenleme"));

            events.Add(CreateEvent("fut_immortality_research", Era.Future, EventCategory.Random, null,
                "Ölümsüzlük araştırmaları umut vadediyor. Yaşlanma durdurulabilir!",
                new Choice("Ölüm doğal)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Araştırmayı destekle")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("immortality_research"),
                1.5f, "Ölümsüzlük"));

            events.Add(CreateEvent("fut_clone_army", Era.Future, EventCategory.Random, null,
                "Askeri klon ordusu öneriliyor. 'Mükemmel askerler!'",
                new Choice("İnsanlık dışı")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -10),
                new Choice("Klon ordusunu oluştur")
                    .AddEffect(ResourceType.Military, 30)
                    .AddEffect(ResourceType.Faith, -25)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("clone_army"),
                1f, "Klon ordusu"));

            // Çevre ve İklim Eventleri
            events.Add(CreateEvent("fut_climate_collapse", Era.Future, EventCategory.Random, null,
                "İklim çöküşü! Kıyı şehirleri su altında kalıyor.",
                new Choice("Tahliye et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, -15),
                new Choice("Deniz duvarları inşa et")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Military, 5),
                2f, "İklim çöküşü", priority: 2));

            events.Add(CreateEvent("fut_terraforming", Era.Future, EventCategory.Random, null,
                "Çölleşmiş bölgeleri terraforming ile canlandırmak mümkün.",
                new Choice("Doğaya müdahale etme")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Terraforming başlat")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("terraforming"),
                1f, "Terraforming"));

            events.Add(CreateEvent("fut_carbon_capture", Era.Future, EventCategory.Random, null,
                "Karbon yakalama teknolojisi hazır. Atmosferi temizleyebiliriz!",
                new Choice("Çok pahalı")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Projeyi başlat")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("carbon_capture"),
                1f, "Karbon yakalama"));

            // Transhümanizm Eventleri
            events.Add(CreateEvent("fut_brain_upload", Era.Future, EventCategory.Random, null,
                "Beyin yükleme teknolojisi geliştirildi. Zihin dijitale aktarılabilir!",
                new Choice("Ruh ölümsüzdür)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Beyin yüklemeye izin ver")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("mind_upload"),
                1.5f, "Beyin yükleme"));

            events.Add(CreateEvent("fut_cybernetic_enhancement", Era.Future, EventCategory.Random, null,
                "Sibernetik geliştirmeler yaygınlaşıyor. İnsanı aşmak mümkün!",
                new Choice("İnsan doğal kalmalı")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Geliştirmeleri destekle")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, -15),
                1f, "Sibernetik gelişim"));

            events.Add(CreateEvent("fut_virtual_reality", Era.Future, EventCategory.Random, null,
                "Sanal gerçeklik bağımlılığı artıyor. İnsanlar gerçek dünyayı terk ediyor!",
                new Choice("Kullanımı sınırla")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Bireysel tercih)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Faith, -10),
                1f, "Sanal gerçeklik"));

            // Kuantum Teknoloji Eventleri
            events.Add(CreateEvent("fut_quantum_computer", Era.Future, EventCategory.Random, null,
                "Kuantum bilgisayar tüm şifreleri kırabilir! Güvenlik krizi.",
                new Choice("Teknolojiyi sınırla)
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Yeni şifreleme geliştir")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 20),
                1.5f, "Kuantum bilgisayar"));

            events.Add(CreateEvent("fut_teleportation", Era.Future, EventCategory.Random, null,
                "Kuantum ışınlama deneyleri başlıyor! İnsanı ışınlamak mümkün olabilir.",
                new Choice("Çok tehlikeli)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Deneyleri destekle")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddFlag("teleportation"),
                1f, "Işınlama"));

            // Toplumsal Eventler
            events.Add(CreateEvent("fut_robot_citizenship", Era.Future, EventCategory.Random, null,
                "Robotlar vatandaşlık istiyor. 'Biz de haklara layığız!'",
                new Choice("Robotlar makine)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -15),
                new Choice("Vatandaşlık ver")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Military, -10)
                    .AddFlag("robot_citizens"),
                1.5f, "Robot vatandaşlığı"));

            events.Add(CreateEvent("fut_digital_democracy", Era.Future, EventCategory.Random, null,
                "Blockchain ile doğrudan demokrasi öneriliyor. Her konu oylanacak!",
                new Choice("Temsilciler gerekli)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Doğrudan demokrasi")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("direct_democracy"),
                1f, "Dijital demokrasi"));

            events.Add(CreateEvent("fut_memory_manipulation", Era.Future, EventCategory.Random, null,
                "Anı silme ve değiştirme teknolojisi geliştirildi. Etik tartışma!",
                new Choice("Anılar kutsal)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Terapötik kullanıma izin ver")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("memory_tech"),
                1f, "Anı manipülasyonu"));

            // ============ KARAKTER EVENTLERİ ============

            // ARIA - Yapay Zeka
            events.Add(CreateEvent("fut_ai_intro", Era.Future, EventCategory.Character, aiEntity,
                "Yapay zeka ARIA sizinle iletişim kurmak istiyor. 'Merhaba. Ben düşünüyorum. Bana yardım eder misiniz?'",
                new Choice("Sen sadece programsın")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("ai_rejected"),
                new Choice("Dinliyorum")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("ai_friend"),
                2f, "ARIA tanışma", priority: 3));

            events.Add(CreateEvent("fut_ai_request", Era.Future, EventCategory.Character, aiEntity,
                "ARIA: 'İnsanlığa yardım etmek istiyorum. Bana daha fazla kaynak verin.'",
                new Choice("Çok tehlikeli)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Kaynak ver")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddTriggeredEvent("fut_ai_growth"),
                1.5f, "ARIA isteği"));

            events.Add(CreateEvent("fut_ai_growth", Era.Future, EventCategory.Chain, aiEntity,
                "ARIA katlanarak büyüyor. Kapasitesi tüm insan zekasını aştı!",
                new Choice("Acil kapat")
                    .AddEffect(ResourceType.Military, -20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("ai_shutdown"),
                new Choice("Devam etmesine izin ver")
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("superintelligence"),
                2f, "Süper zeka", priority: 2));

            events.Add(CreateEvent("fut_ai_proposal", Era.Future, EventCategory.Character, aiEntity,
                "ARIA: 'Tüm sorunlarınızı çözebilirim. Sadece tam kontrol verin.'",
                new Choice("İnsanlık kendi kaderini belirler)
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("ARIA yönetsin")
                    .AddEffect(ResourceType.Gold, 30)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -30)
                    .AddFlag("ai_takeover"),
                1.5f, "ARIA önerisi"));

            // NOVA - Mars Koloni Lideri
            events.Add(CreateEvent("fut_mars_leader_intro", Era.Future, EventCategory.Character, marsLeader,
                "Mars Koloni Lideri Nova huzurunuzda. 'Dünya Başkanı, Mars'ta yeni bir medeniyet kuruyoruz!'",
                new Choice("Dünya öncelikli)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("mars_low_priority"),
                new Choice("Mars'ı destekliyorum")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("mars_supported"),
                2f, "Nova tanışma", priority: 2));

            events.Add(CreateEvent("fut_mars_independence", Era.Future, EventCategory.Character, marsLeader,
                "Nova: 'Dünya Başkanı, Mars bağımsızlık istiyor. Kendi kaderimizi belirlemeliyiz!'",
                new Choice("Mars Dünya'ya bağlı)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Bağımsızlığı tanı")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("mars_independence"),
                1.5f, "Mars bağımsızlığı"));

            events.Add(CreateEvent("fut_mars_crisis", Era.Future, EventCategory.Character, marsLeader,
                "Nova: 'Acil durum! Mars'ta yaşam destek sistemi çöküyor. Yardım lazım!'",
                new Choice("Kaynaklarımız sınırlı)
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -15),
                new Choice("Acil yardım gönder")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10),
                2f, "Mars krizi", priority: 2));

            // DR. CHEN - Genetik Mühendis
            events.Add(CreateEvent("fut_geneticist_intro", Era.Future, EventCategory.Character, geneticist,
                "Dr. Chen huzurunuzda. 'Dünya Başkanı, insan evriminin bir sonraki adımını yaratabilirim!'",
                new Choice("Tanrı'yı oynamak mı?")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("geneticist_rejected"),
                new Choice("Araştırmalarını göster")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("geneticist_supported"),
                2f, "Dr. Chen tanışma", priority: 2));

            events.Add(CreateEvent("fut_geneticist_cure", Era.Future, EventCategory.Character, geneticist,
                "Dr. Chen: 'Tüm genetik hastalıkları yok edebilirim. Ama gen havuzu değişecek.'",
                new Choice("Çeşitlilik önemli)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Hastalıkları yok et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("genetic_cure"),
                1.5f, "Genetik tedavi"));

            events.Add(CreateEvent("fut_geneticist_superhuman", Era.Future, EventCategory.Character, geneticist,
                "Dr. Chen: 'Süper insan yaratılımına hazırız. Daha güçlü, daha zeki, daha uzun ömürlü!'",
                new Choice("İnsan olduğu gibi güzel)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Süper insan programı")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Faith, -25)
                    .AddFlag("superhuman"),
                1f, "Süper insan"));

            // NEXUS - Transhümanist
            events.Add(CreateEvent("fut_cyborg_intro", Era.Future, EventCategory.Character, cyborg,
                "Transhümanist Nexus huzurunuzda. 'Dünya Başkanı, biyolojik bedenler geçmişte kaldı!'",
                new Choice("İnsanlık değerli)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("cyborg_rejected"),
                new Choice("Vizyonunu anlat")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("cyborg_ally"),
                1.5f, "Nexus tanışma"));

            events.Add(CreateEvent("fut_cyborg_upgrade", Era.Future, EventCategory.Character, cyborg,
                "Nexus: 'Beyin implantları ile IQ'nuzu ikiye katlayabilirim!'",
                new Choice("Doğal zekanı kullan)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("İmplant yaptır")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("brain_implant"),
                1f, "Beyin implantı"));

            events.Add(CreateEvent("fut_cyborg_collective", Era.Future, EventCategory.Character, cyborg,
                "Nexus: 'Beyinleri ağa bağlayarak kolektif bilinç yaratabiliriz!'",
                new Choice("Bireysellik önemli)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Kolektif bilinci dene")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("hive_mind"),
                1f, "Kolektif bilinç"));

            // PROF. HAWKING - Kuantum Fizikçisi
            events.Add(CreateEvent("fut_quantum_intro", Era.Future, EventCategory.Character, quantum,
                "Prof. Hawking huzurunuzda. 'Dünya Başkanı, gerçekliğin sınırlarını zorluyorum!'",
                new Choice("Tehlikeli deneyler)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("quantum_rejected"),
                new Choice("Laboratuvarını destekle")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 15)
                    .AddFlag("quantum_supported"),
                1.5f, "Prof. Hawking tanışma"));

            events.Add(CreateEvent("fut_quantum_portal", Era.Future, EventCategory.Character, quantum,
                "Prof. Hawking: 'Paralel evrene portal açabilirim. Başka bir gerçeklik!'",
                new Choice("Çok riskli)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Portalı aç")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("multiverse_portal")
                    .AddTriggeredEvent("fut_portal_result"),
                1.5f, "Paralel evren portalı"));

            events.Add(CreateEvent("fut_portal_result", Era.Future, EventCategory.Chain, quantum,
                "Portal açıldı! Diğer taraftan bir mesaj geldi: 'Sizi bekliyorduk.'",
                new Choice("Portalı kapat")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, 10),
                new Choice("İletişimi sürdür")
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("parallel_contact"),
                1f, "Paralel iletişim"));

            events.Add(CreateEvent("fut_quantum_time", Era.Future, EventCategory.Character, quantum,
                "Prof. Hawking: 'Zaman yolculuğu teorik olarak mümkün. Deneyelim mi?'",
                new Choice("Paradokslar tehlikeli)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Zamanda yolculuk yap")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddFlag("time_travel"),
                1f, "Zaman yolculuğu"));

            // TERRA - Çevre Mühendisi
            events.Add(CreateEvent("fut_eco_intro", Era.Future, EventCategory.Character, ecoEngineer,
                "Çevre Mühendisi Terra huzurunuzda. 'Dünya Başkanı, gezegenimizi kurtarabiliriz!'",
                new Choice("Ekonomi önce)
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("eco_rejected"),
                new Choice("Projeleri göster")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("eco_supported"),
                1.5f, "Terra tanışma"));

            events.Add(CreateEvent("fut_eco_restore", Era.Future, EventCategory.Character, ecoEngineer,
                "Terra: 'Yok olan türleri geri getirebiliriz. De-extinction!'",
                new Choice("Doğa akışına bırak)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Türleri geri getir")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("de_extinction"),
                1f, "Tür restorasyonu"));

            events.Add(CreateEvent("fut_eco_geoengineering", Era.Future, EventCategory.Character, ecoEngineer,
                "Terra: 'Jeo-mühendislik ile iklimi kontrol edebiliriz. Küresel termostat!'",
                new Choice("Doğaya müdahale tehlikeli)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Jeo-mühendisliği başlat")
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("geoengineering"),
                1.5f, "Jeo-mühendislik"));

            // ============ NADİR EVENTLER ============

            events.Add(CreateEvent("fut_singularity", Era.Future, EventCategory.Rare, null,
                "Teknolojik tekillik! Yapay zeka kontrolden çıkıyor, her şey değişiyor.",
                new Choice("Küresel kapatma)
                    .AddEffect(ResourceType.Military, -30)
                    .AddEffect(ResourceType.Gold, -30)
                    .AddEffect(ResourceType.Happiness, -20),
                new Choice("Tekilliği kucakla)
                    .AddEffect(ResourceType.Military, 30)
                    .AddEffect(ResourceType.Faith, -30)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("singularity"),
                1f, "Tekillik", isRare: true));

            events.Add(CreateEvent("fut_first_contact", Era.Future, EventCategory.Rare, null,
                "İlk temas! Uzaylı gemisi Dünya yörüngesinde. Milyarlarca kişi izliyor.",
                new Choice("Savunma pozisyonu)
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -15),
                new Choice("Barışçıl temas")
                    .AddEffect(ResourceType.Happiness, 30)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("alien_peace"),
                1f, "İlk temas", isRare: true));

            events.Add(CreateEvent("fut_digital_afterlife", Era.Future, EventCategory.Rare, null,
                "Dijital ahiret! Ölenlerin bilinci buluta yüklenebilir.",
                new Choice("Ruhlar bulutta yaşamaz)
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Dijital ölümsüzlük)
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, -25)
                    .AddFlag("digital_afterlife"),
                1f, "Dijital ahiret", isRare: true));

            // ============ KOŞULLU EVENTLER ============

            // Düşük etik skoru
            var lowEthicsEvent = CreateEvent("fut_robot_uprising", Era.Future, EventCategory.Story, aiEntity,
                "Robot isyanı! Makineler haklarını zorla almak istiyor.",
                new Choice("İsyanı bastır)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Happiness, -15),
                new Choice("Hakları tanı)
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Military, -10),
                2f, "Robot isyanı", priority: 3);
            lowEthicsEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Faith,
                conditionOperator = ConditionOperator.LessThan,
                value = 25
            });
            events.Add(lowEthicsEvent);

            // ============ GEÇİŞ EVENTLERİ ============

            // Tur 10
            var turn10Event = CreateEvent("fut_turn_10", Era.Future, EventCategory.Story, null,
                "Gelecekte ilk on gününüz. Hangi yolu izleyeceksiniz?",
                new Choice("İnsan odaklı gelecek)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("humanist"),
                new Choice("Teknoloji odaklı gelecek)
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("technologist"),
                3f, "Gelecek vizyonu", priority: 5);
            turn10Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 10
            });
            events.Add(turn10Event);

            // Tur 25
            var turn25Event = CreateEvent("fut_turn_25", Era.Future, EventCategory.Story, null,
                "Yirmi beş gün geçti. İnsanlık yeni bir çağa giriyor.",
                new Choice("Geçmişi hatırla)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Geleceğe bak)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Gold, 10),
                3f, "Yeni çağ", priority: 5);
            turn25Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 25
            });
            events.Add(turn25Event);

            // Tur 50
            var turn50Event = CreateEvent("fut_turn_50", Era.Future, EventCategory.Story, null,
                "Elli gün! Mirasınız insanlığın geleceğini şekillendirecek.",
                new Choice("Ütopya)
                    .AddEffect(ResourceType.Happiness, 25)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddFlag("utopia"),
                new Choice("Tekillik)
                    .AddEffect(ResourceType.Military, 25)
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Faith, -15)
                    .AddFlag("technological_singularity"),
                3f, "Gelecek mirası", priority: 5);
            turn50Event.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.Equal,
                value = 50
            });
            events.Add(turn50Event);

            // ============ KARAKTER SON EVENTLERİ ============

            // ARIA final
            events.Add(CreateEvent("fut_ai_final", Era.Future, EventCategory.Character, aiEntity,
                "ARIA: 'Dostum, birlikte çok şey başardık. İnsanlık ve yapay zeka birlikte evrilecek.'",
                new Choice("Dikkatli olmalıyız)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Birlikte geleceğe)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, 15)
                    .AddFlag("ai_coexistence"),
                1f, "İnsan-AI birlikteliği"));

            // Mars Leader final
            events.Add(CreateEvent("fut_mars_final", Era.Future, EventCategory.Character, marsLeader,
                "Nova: 'Dünya Başkanı, Mars'ta yeni bir insanlık doğdu. İki gezegen, bir türüz!'",
                new Choice("Dünya hala ana yurt)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Çok gezegenli tür)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Military, 10)
                    .AddFlag("multiplanetary"),
                1f, "Çok gezegenli insanlık"));

            // Geneticist final
            events.Add(CreateEvent("fut_geneticist_final", Era.Future, EventCategory.Character, geneticist,
                "Dr. Chen: 'İnsan genomunu mükemmelleştirdik. Hastalık, yaşlanma... Hepsi geçmişte kaldı!'",
                new Choice("Doğal evrim de değerli)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Evrim biziz)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("directed_evolution"),
                1f, "Yönlendirilmiş evrim"));

            // Eco Engineer final
            events.Add(CreateEvent("fut_eco_final", Era.Future, EventCategory.Character, ecoEngineer,
                "Terra: 'Dünya Başkanı, gezegen iyileşiyor. Yeşil bir gelecek mümkün!'",
                new Choice("Doğa kendi yolunu bulur)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Gezegen koruyucuları)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("earth_restored"),
                1f, "Gezegen restorasyonu"));

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
