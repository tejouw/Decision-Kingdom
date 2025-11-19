using System.Collections.Generic;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Content
{
    /// <summary>
    /// Medieval dönemi karakter ve event tanımları
    /// </summary>
    public static class MedievalContent
    {
        #region Character IDs
        public const string CHAR_MARCUS = "marcus";
        public const string CHAR_MIRIAM = "miriam";
        public const string CHAR_VALERIUS = "valerius";
        public const string CHAR_HIGH_PRIEST = "high_priest";
        public const string CHAR_QUEEN = "queen";
        public const string CHAR_HEIR = "heir";
        #endregion

        #region Characters
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
                new Character(CHAR_MARCUS, "Marcus", "Danışman")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Sadık danışmanınız. Her zaman yanınızda olan ama niyetleri belirsiz bir figür."
                },
                new Character(CHAR_MIRIAM, "Miriam", "Tüccar")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Hırslı bir tüccar. Riskli ama kazançlı fırsatlar sunar."
                },
                new Character(CHAR_VALERIUS, "Valerius", "General")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Güçlü ordunuzun komutanı. Askeri güce önem verir."
                },
                new Character(CHAR_HIGH_PRIEST, "Benedictus", "Rahip Başı")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Kilisenin en yüksek temsilcisi. İnanç ve ahlak konularında etki sahibi."
                },
                new Character(CHAR_QUEEN, "Eleanor", "Kraliçe")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Kraliyet eşiniz. Siyasi ittifaklar ve saray entrikaları."
                },
                new Character(CHAR_HEIR, "Edmund", "Veliaht")
                {
                    eras = new List<Era> { Era.Medieval },
                    description = "Genç veliaht. Geleceğin kralı olarak yetişiyor."
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
            var marcus = characters.Find(c => c.id == CHAR_MARCUS);
            var miriam = characters.Find(c => c.id == CHAR_MIRIAM);
            var valerius = characters.Find(c => c.id == CHAR_VALERIUS);
            var highPriest = characters.Find(c => c.id == CHAR_HIGH_PRIEST);
            var queen = characters.Find(c => c.id == CHAR_QUEEN);
            var heir = characters.Find(c => c.id == CHAR_HEIR);

            // ============ TEMEL RANDOM EVENTLER ============

            // Ekonomi eventleri
            events.Add(CreateEvent("med_tax_peasants", Era.Medieval, EventCategory.Random, null,
                "Hazine danışmanınız geldi. 'Majeste, köylülerden daha fazla vergi toplayabilir miyiz?'",
                new Choice("Hayır, bu adaletsiz olur")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Evet, hazine dolmalı")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -5),
                1f, "Vergi toplama kararı"));

            events.Add(CreateEvent("med_merchant_guild", Era.Medieval, EventCategory.Random, null,
                "Tüccar loncası izin istiyor. 'Yeni bir pazar yeri kurmak istiyoruz.'",
                new Choice("Reddet, rekabet artar")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("İzin ver")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                1f, "Pazar yeri kurma"));

            events.Add(CreateEvent("med_gold_mine", Era.Medieval, EventCategory.Random, null,
                "Bir altın madeni bulundu! Ancak orman köylülerinin topraklarında.",
                new Choice("Köylülere bırak")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Madeni işlet")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -10),
                1f, "Altın madeni"));

            // Askeri eventler
            events.Add(CreateEvent("med_border_threat", Era.Medieval, EventCategory.Random, null,
                "Sınırda düşman kuvvetleri görüldü. Ordunuzu güçlendirmeli misiniz?",
                new Choice("Diplomasi dene")
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Orduyu güçlendir")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Gold, -10),
                1f, "Sınır tehdidi"));

            events.Add(CreateEvent("med_knight_tournament", Era.Medieval, EventCategory.Random, null,
                "Şövalyeler turnuva düzenlemek istiyor. Büyük bir gösteri olacak.",
                new Choice("Pahalı, iptal et")
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Turnuvayı onayla")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 5),
                1f, "Şövalye turnuvası"));

            events.Add(CreateEvent("med_deserters", Era.Medieval, EventCategory.Random, null,
                "Bazı askerler firar etti. Yakalandılar. Ne yapmalı?",
                new Choice("Affet, geri al")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, -5),
                new Choice("İdam et, ibret olsun")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -5),
                1f, "Firariler"));

            // Dini eventler
            events.Add(CreateEvent("med_church_donation", Era.Medieval, EventCategory.Random, null,
                "Kilise yeni bir katedral inşa etmek için bağış istiyor.",
                new Choice("Reddet")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Bağış yap")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 5),
                1f, "Katedral bağışı"));

            events.Add(CreateEvent("med_heresy_report", Era.Medieval, EventCategory.Random, null,
                "Bir köyde sapkınlık iddiası var. Araştırma yapmalı mıyız?",
                new Choice("Görmezden gel")
                    .AddEffect(ResourceType.Faith, -5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Araştır")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("heresy_investigation"),
                1f, "Sapkınlık iddiası"));

            events.Add(CreateEvent("med_miracle_claim", Era.Medieval, EventCategory.Random, null,
                "Bir köylü mucize gerçekleştirdiğini iddia ediyor. Kalabalık toplanmış.",
                new Choice("Dolandırıcı, tutukla")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Kutsa ve onayla")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, -5),
                1f, "Mucize iddiası"));

            // Halk eventleri
            events.Add(CreateEvent("med_famine", Era.Medieval, EventCategory.Random, null,
                "Kuzey bölgelerinde kıtlık başladı. Halk yardım bekliyor.",
                new Choice("Yardım gönderme")
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Yiyecek gönder")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                2f, "Kıtlık krizi", priority: 1));

            events.Add(CreateEvent("med_plague_outbreak", Era.Medieval, EventCategory.Random, null,
                "Veba salgını başladı! Acil önlem alınmalı.",
                new Choice("Karantina uygula")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, -5),
                new Choice("Görmezden gel")
                    .AddEffect(ResourceType.Happiness, -20)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Military, -10),
                2f, "Veba salgını", priority: 2));

            events.Add(CreateEvent("med_festival_request", Era.Medieval, EventCategory.Random, null,
                "Halk hasat festivali düzenlemek istiyor.",
                new Choice("Gereksiz masraf")
                    .AddEffect(ResourceType.Happiness, -10),
                new Choice("Festivali onayla")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 5),
                1f, "Hasat festivali"));

            events.Add(CreateEvent("med_peasant_complaint", Era.Medieval, EventCategory.Random, null,
                "Köylüler lordlarının zulmünden şikayet ediyor.",
                new Choice("Lordu destekle")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Köylüleri dinle")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Military, -5),
                1f, "Köylü şikayeti"));

            // ============ KARAKTER EVENTLERİ ============

            // MARCUS - Danışman
            events.Add(CreateEvent("med_marcus_intro", Era.Medieval, EventCategory.Character, marcus,
                "Danışman Marcus huzurunuza çıktı. 'Majeste, size sadık bir danışman olarak hizmet etmek istiyorum.'",
                new Choice("Güvenmiyorum")
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("marcus_rejected"),
                new Choice("Kabul ediyorum")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("marcus_accepted"),
                2f, "Marcus tanışma", priority: 3));

            events.Add(CreateEvent("med_marcus_advice", Era.Medieval, EventCategory.Character, marcus,
                "Marcus geldi. 'Majeste, hazinenizi artırmak için bir yolum var. Gizli ticaret.'",
                new Choice("Reddediyorum")
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Anlat")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Faith, -5)
                    .AddTriggeredEvent("med_marcus_trade"),
                1.5f, "Marcus gizli ticaret önerisi"));

            events.Add(CreateEvent("med_marcus_trade", Era.Medieval, EventCategory.Chain, marcus,
                "Marcus planını anlattı. Yasadışı ama kazançlı bir ticaret ağı.",
                new Choice("Çok riskli")
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Devam et")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("illegal_trade"),
                1f, "Yasadışı ticaret"));

            events.Add(CreateEvent("med_marcus_loyalty", Era.Medieval, EventCategory.Character, marcus,
                "Marcus: 'Majeste, bir komploya karşı sizi uyarmalıyım. Bazı lordlar isyan planlıyor.'",
                new Choice("Kanıt göster")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddTriggeredEvent("med_marcus_conspiracy"),
                new Choice("Hemen harekete geç")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("marcus_trusted"),
                1.5f, "Marcus sadakat testi"));

            events.Add(CreateEvent("med_marcus_conspiracy", Era.Medieval, EventCategory.Chain, marcus,
                "Marcus kanıtları sundu. Gerçekten de bir komplo var.",
                new Choice("Affet, barış sağla")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Military, -10),
                new Choice("Komploculari tutukla")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Gold, 10),
                1f, "Komplo çözümü"));

            // MIRIAM - Tüccar
            events.Add(CreateEvent("med_miriam_intro", Era.Medieval, EventCategory.Character, miriam,
                "Tüccar Miriam sarayda. 'Majeste, size harika bir teklif getirdim. Ucuza mal satacağım.'",
                new Choice("Güvenmiyorum")
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("Ne satıyorsun?")
                    .AddEffect(ResourceType.Gold, 5)
                    .AddTriggeredEvent("med_miriam_deal"),
                2f, "Miriam tanışma", priority: 2));

            events.Add(CreateEvent("med_miriam_deal", Era.Medieval, EventCategory.Chain, miriam,
                "Miriam mallarını gösterdi. Kaliteli ama kaynağı belirsiz.",
                new Choice("Alma")
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Al")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Military, 5)
                    .AddFlag("miriam_goods_bought")
                    .AddTriggeredEvent("med_miriam_problem"),
                1f, "Miriam alışverişi"));

            events.Add(CreateEvent("med_miriam_problem", Era.Medieval, EventCategory.Chain, miriam,
                "Miriam geri geldi. 'Sorun var majeste. O mallar... çalıntıymış.'",
                new Choice("Miriam'ı teslim et")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("miriam_betrayed"),
                new Choice("Koru")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("miriam_protected"),
                1.5f, "Çalıntı mal krizi"));

            events.Add(CreateEvent("med_miriam_opportunity", Era.Medieval, EventCategory.Character, miriam,
                "Miriam: 'Majeste, büyük bir fırsat! Ama tüm hazineyi riske atmalısınız.'",
                new Choice("Çok riskli")
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Risk al")
                    .AddEffect(ResourceType.Gold, -20, 30)
                    .AddFlag("miriam_big_risk"),
                1f, "Büyük fırsat"));

            // VALERIUS - General
            events.Add(CreateEvent("med_valerius_intro", Era.Medieval, EventCategory.Character, valerius,
                "General Valerius huzurunuzda. 'Majeste, ordumuz güçlendirilmeli. Düşmanlar kapıda.'",
                new Choice("Barış istiyoruz")
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Orduyu güçlendir")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Gold, -10)
                    .AddFlag("valerius_supported"),
                2f, "Valerius tanışma", priority: 2));

            events.Add(CreateEvent("med_valerius_war", Era.Medieval, EventCategory.Character, valerius,
                "Valerius: 'Komşu krallık zayıf. Saldırı zamanı.'",
                new Choice("Saldırma")
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Saldır")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, -15, 20)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("aggressive_war"),
                1.5f, "Savaş kararı"));

            events.Add(CreateEvent("med_valerius_mutiny", Era.Medieval, EventCategory.Character, valerius,
                "Valerius: 'Askerler maaşlarını istiyor. Ödemezsen isyan çıkar.'",
                new Choice("Beklesinler")
                    .AddEffect(ResourceType.Military, -15)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddTriggeredEvent("med_army_mutiny"),
                new Choice("Öde")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Military, 10),
                2f, "Asker maaşları", priority: 1));

            events.Add(CreateEvent("med_army_mutiny", Era.Medieval, EventCategory.Chain, valerius,
                "Askerler isyan etti! Kışla kaosa sürükleniyor.",
                new Choice("Müzakere et")
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Bastır")
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -5),
                1f, "Asker isyanı"));

            events.Add(CreateEvent("med_valerius_coup", Era.Medieval, EventCategory.Character, valerius,
                "Valerius: 'Majeste... ordunun desteği benimle. Ya bana daha fazla yetki verirsiniz, ya da...'",
                new Choice("Reddet")
                    .AddEffect(ResourceType.Military, -20)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("valerius_enemy"),
                new Choice("Yetki ver")
                    .AddEffect(ResourceType.Military, 20)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Faith, -10)
                    .AddFlag("military_power"),
                1f, "Darbe tehdidi", priority: 3));

            // HIGH PRIEST - Rahip Başı
            events.Add(CreateEvent("med_priest_intro", Era.Medieval, EventCategory.Character, highPriest,
                "Rahip Başı Benedictus huzurunuzda. 'Majeste, kilise sizin desteğinizi bekliyor.'",
                new Choice("Kilise kendi işine baksın")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Gold, 5),
                new Choice("Kiliseyi destekliyorum")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, -5)
                    .AddFlag("church_supported"),
                2f, "Rahip Başı tanışma", priority: 2));

            events.Add(CreateEvent("med_priest_heretic", Era.Medieval, EventCategory.Character, highPriest,
                "Benedictus: 'Bir sapkın yakalandı. Yakılmalı!'",
                new Choice("Affet")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, 10),
                new Choice("Yak")
                    .AddEffect(ResourceType.Faith, 15)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddFlag("burned_heretic"),
                1.5f, "Sapkın yakma"));

            events.Add(CreateEvent("med_priest_crusade", Era.Medieval, EventCategory.Character, highPriest,
                "Benedictus: 'Kutsal topraklar tehdit altında. Haçlı seferi çağrısı yapmalısınız!'",
                new Choice("Reddet")
                    .AddEffect(ResourceType.Faith, -20)
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, 5),
                new Choice("Sefere katıl")
                    .AddEffect(ResourceType.Faith, 25)
                    .AddEffect(ResourceType.Gold, -20)
                    .AddEffect(ResourceType.Military, -15)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddFlag("crusade_joined"),
                1f, "Haçlı seferi", priority: 1));

            events.Add(CreateEvent("med_priest_power", Era.Medieval, EventCategory.Character, highPriest,
                "Benedictus: 'Majeste, kilise yasalarını krallık yasalarının üstüne çıkarmalıyız.'",
                new Choice("Asla")
                    .AddEffect(ResourceType.Faith, -15)
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddFlag("secular_state"),
                new Choice("Kabul")
                    .AddEffect(ResourceType.Faith, 20)
                    .AddEffect(ResourceType.Happiness, -15)
                    .AddEffect(ResourceType.Military, -10)
                    .AddFlag("theocratic_power"),
                1f, "Teokratik güç"));

            // QUEEN - Kraliçe
            events.Add(CreateEvent("med_queen_intro", Era.Medieval, EventCategory.Character, queen,
                "Kraliçe Eleanor geldi. 'Sevgili eşim, bazı konularda fikrimi almak ister misiniz?'",
                new Choice("Saray işlerine karışma")
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("queen_ignored"),
                new Choice("Dinliyorum")
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddEffect(ResourceType.Faith, 5)
                    .AddFlag("queen_listened"),
                1.5f, "Kraliçe tanışma"));

            events.Add(CreateEvent("med_queen_alliance", Era.Medieval, EventCategory.Character, queen,
                "Eleanor: 'Ailem güçlü bir krallık. İttifak kurmalıyız.'",
                new Choice("Bağımsız kalmalıyız")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Gold, -5),
                new Choice("İttifak kur")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("queen_alliance"),
                1.5f, "Kraliçe ittifakı"));

            events.Add(CreateEvent("med_queen_affair", Era.Medieval, EventCategory.Character, queen,
                "Dedikodular yayılıyor. Kraliçe'nin bir aşığı olduğu söyleniyor.",
                new Choice("Görmezden gel")
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -10),
                new Choice("Araştır")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddTriggeredEvent("med_queen_affair_result"),
                1f, "Kraliçe dedikodu"));

            events.Add(CreateEvent("med_queen_affair_result", Era.Medieval, EventCategory.Chain, queen,
                "Araştırma sonuçlandı. Dedikodular asılsızmış, rakipleriniz yaymış.",
                new Choice("Kraliçe'den özür dile")
                    .AddEffect(ResourceType.Happiness, 10)
                    .AddEffect(ResourceType.Faith, 5),
                new Choice("Dedikoducuları cezalandır")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Faith, 5),
                1f, "Dedikodu sonucu"));

            events.Add(CreateEvent("med_queen_heir", Era.Medieval, EventCategory.Character, queen,
                "Eleanor: 'Bir veliahdımız oldu! Krallığın geleceği güvende.'",
                new Choice("Kutlama yapma")
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddEffect(ResourceType.Gold, 10),
                new Choice("Büyük kutlama")
                    .AddEffect(ResourceType.Gold, -15)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Faith, 10)
                    .AddFlag("heir_born"),
                2f, "Veliaht doğumu", priority: 3));

            // HEIR - Veliaht
            events.Add(CreateEvent("med_heir_education", Era.Medieval, EventCategory.Character, heir,
                "Veliaht Edmund'un eğitimi için karar vermelisiniz. Nasıl yetişsin?",
                new Choice("Savaşçı olarak")
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Faith, -5)
                    .AddFlag("heir_warrior"),
                new Choice("Alim olarak")
                    .AddEffect(ResourceType.Faith, 10)
                    .AddEffect(ResourceType.Gold, 5)
                    .AddEffect(ResourceType.Military, -5)
                    .AddFlag("heir_scholar"),
                1.5f, "Veliaht eğitimi"));

            events.Add(CreateEvent("med_heir_rebellion", Era.Medieval, EventCategory.Character, heir,
                "Veliaht Edmund sabırsız. 'Baba, ne zaman taht benim olacak?'",
                new Choice("Bekle")
                    .AddEffect(ResourceType.Happiness, -5)
                    .AddFlag("heir_impatient"),
                new Choice("Ona görev ver")
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("heir_responsibility"),
                1f, "Veliaht sabırsızlığı"));

            // ============ NADİR EVENTLER ============

            events.Add(CreateEvent("med_dragon", Era.Medieval, EventCategory.Rare, null,
                "Bir ejderha görüldü! Köyleri yakıyor, hazineler biriktiriyor.",
                new Choice("Şövalye gönder")
                    .AddEffect(ResourceType.Military, -15)
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Gold, 20)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Haraç ver")
                    .AddEffect(ResourceType.Gold, -25)
                    .AddEffect(ResourceType.Happiness, -10)
                    .AddEffect(ResourceType.Faith, -5),
                1f, "Ejderha", isRare: true));

            events.Add(CreateEvent("med_holy_grail", Era.Medieval, EventCategory.Rare, null,
                "Bir şövalye Kutsal Kase'yi bulduğunu iddia ediyor!",
                new Choice("Yalan, cezalandır")
                    .AddEffect(ResourceType.Faith, -10)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Kutsa ve sergile")
                    .AddEffect(ResourceType.Faith, 30)
                    .AddEffect(ResourceType.Happiness, 20)
                    .AddEffect(ResourceType.Gold, 15),
                1f, "Kutsal Kase", isRare: true));

            events.Add(CreateEvent("med_time_traveler", Era.Medieval, EventCategory.Rare, null,
                "Garip kıyafetli biri geldi. 'Gelecekten geliyorum, büyük bir kriz yaklaşıyor!'",
                new Choice("Deli, kov")
                    .AddEffect(ResourceType.Happiness, 5),
                new Choice("Dinle")
                    .AddEffect(ResourceType.Gold, -5)
                    .AddEffect(ResourceType.Military, 10)
                    .AddEffect(ResourceType.Happiness, 5)
                    .AddFlag("time_traveler_warning"),
                1f, "Zaman yolcusu", isRare: true));

            // ============ KOŞULLU EVENTLER ============

            // Düşük para eventleri
            var lowGoldEvent = CreateEvent("med_bankruptcy_warning", Era.Medieval, EventCategory.Story, marcus,
                "Marcus: 'Majeste, hazine tehlikeli derecede boş. Acil önlem almalıyız!'",
                new Choice("Vergileri artır")
                    .AddEffect(ResourceType.Gold, 15)
                    .AddEffect(ResourceType.Happiness, -15),
                new Choice("Harcamaları kıs")
                    .AddEffect(ResourceType.Gold, 10)
                    .AddEffect(ResourceType.Military, -10)
                    .AddEffect(ResourceType.Faith, -5),
                2f, "İflas uyarısı", priority: 2);
            lowGoldEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Gold,
                conditionOperator = ConditionOperator.LessThan,
                value = 25
            });
            events.Add(lowGoldEvent);

            // Yüksek askeri güç eventi
            var highMilitaryEvent = CreateEvent("med_military_parade", Era.Medieval, EventCategory.Random, valerius,
                "Valerius: 'Ordumuz çok güçlü! Askeri geçit töreni yapalım.'",
                new Choice("Gereksiz")
                    .AddEffect(ResourceType.Military, -5)
                    .AddEffect(ResourceType.Happiness, -5),
                new Choice("Tören yap")
                    .AddEffect(ResourceType.Gold, -10)
                    .AddEffect(ResourceType.Military, 5)
                    .AddEffect(ResourceType.Happiness, 10),
                1f, "Askeri geçit");
            highMilitaryEvent.conditions.Add(new Condition
            {
                type = ConditionType.ResourceThreshold,
                resource = ResourceType.Military,
                conditionOperator = ConditionOperator.GreaterThan,
                value = 70
            });
            events.Add(highMilitaryEvent);

            // Geç oyun eventleri
            var lateGameEvent = CreateEvent("med_legacy", Era.Medieval, EventCategory.Story, null,
                "Halk sizi 'Büyük Kral' olarak anmaya başladı. Nasıl hatırlanmak istersiniz?",
                new Choice("Adil Kral")
                    .AddEffect(ResourceType.Happiness, 15)
                    .AddEffect(ResourceType.Faith, 10),
                new Choice("Güçlü Kral")
                    .AddEffect(ResourceType.Military, 15)
                    .AddEffect(ResourceType.Gold, 10),
                1f, "Miras", priority: 1);
            lateGameEvent.conditions.Add(new Condition
            {
                type = ConditionType.TurnCount,
                conditionOperator = ConditionOperator.GreaterThan,
                value = 40
            });
            events.Add(lateGameEvent);

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
