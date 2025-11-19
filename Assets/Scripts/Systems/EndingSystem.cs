using System.Collections.Generic;
using System.Linq;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Oyun sonu sistemini yöneten sınıf
    /// 20+ benzersiz son içerir
    /// </summary>
    public static class EndingSystem
    {
        #region Ending Data
        /// <summary>
        /// Tüm sonların veri yapısı
        /// </summary>
        public class EndingData
        {
            public EndingType type;
            public string title;
            public string description;
            public string epilogue;
            public bool isVictory;
            public int prestigeBonus;
        }

        private static Dictionary<EndingType, EndingData> _endings;

        static EndingSystem()
        {
            InitializeEndings();
        }

        private static void InitializeEndings()
        {
            _endings = new Dictionary<EndingType, EndingData>
            {
                // === BAŞARISIZ SONLAR ===

                // Ekonomik Çöküşler
                [EndingType.BankruptKingdom] = new EndingData
                {
                    type = EndingType.BankruptKingdom,
                    title = "İFLAS!",
                    description = "Hazine tamamen boşaldı. Krallık ekonomisi çöktü.",
                    epilogue = "Tüccarlar kaçtı, askerler dağıldı. Krallığınız tarih sayfalarında bir ekonomik felaket olarak anılacak.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.EconomicCollapse] = new EndingData
                {
                    type = EndingType.EconomicCollapse,
                    title = "EKONOMİK ÇÖKÜŞ!",
                    description = "Aşırı zenginlik hiperenflasyona yol açtı.",
                    epilogue = "Altın değersizleşti, pazarlar kapandı. Zenginliğiniz kendi sonunuz oldu.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.TaxRevolt] = new EndingData
                {
                    type = EndingType.TaxRevolt,
                    title = "VERGİ İSYANI!",
                    description = "Aşırı vergiler halkı isyana sürükledi.",
                    epilogue = "Köylüler sarayı bastı. Açgözlülüğünüzün bedeli tahtınız oldu.",
                    isVictory = false,
                    prestigeBonus = 0
                },

                // Sosyal Çöküşler
                [EndingType.PeasantRevolution] = new EndingData
                {
                    type = EndingType.PeasantRevolution,
                    title = "HALK DEVRİMİ!",
                    description = "Halk ayaklandı ve tahtınızı devirdi.",
                    epilogue = "Sokaklarda isyancılar zafer şarkıları söylüyor. Tiranlığınız sona erdi.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.NobleConspiracy] = new EndingData
                {
                    type = EndingType.NobleConspiracy,
                    title = "SOYLU KOMPLOSU!",
                    description = "Lordlar gizlice birleşip tahtı ele geçirdi.",
                    epilogue = "Güvendiğiniz soylular ihanet etti. Artık zindanlarda çürüyorsunuz.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.CivilWar] = new EndingData
                {
                    type = EndingType.CivilWar,
                    title = "İÇ SAVAŞ!",
                    description = "Krallık ikiye bölündü, kardeş kardeşe düştü.",
                    epilogue = "Yıllarca süren savaş toprakları harap etti. Krallığınızın birliği asla geri gelmedi.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.LazyDecline] = new EndingData
                {
                    type = EndingType.LazyDecline,
                    title = "TEMBELLİK ÇÖKÜŞÜ!",
                    description = "Aşırı rahatlık krallığı tembelliğe sürükledi.",
                    epilogue = "Kimse çalışmak istemedi, üretim durdu. Cennet gibi yaşam, cehenneme dönüştü.",
                    isVictory = false,
                    prestigeBonus = 10
                },

                // Askeri Çöküşler
                [EndingType.ForeignInvasion] = new EndingData
                {
                    type = EndingType.ForeignInvasion,
                    title = "YABANCI İSTİLA!",
                    description = "Düşman orduları savunmasız krallığı istila etti.",
                    epilogue = "Ordunuz çok zayıftı. Düşman bayrakları artık kalenizde dalgalanıyor.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.MilitaryCoup] = new EndingData
                {
                    type = EndingType.MilitaryCoup,
                    title = "ASKERİ DARBE!",
                    description = "Generaller çok güçlendi ve tahtı ele geçirdi.",
                    epilogue = "Kendi ordunuz size karşı döndü. Artık bir kukla kralsınız.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.BetrayalByGeneral] = new EndingData
                {
                    type = EndingType.BetrayalByGeneral,
                    title = "GENERAL İHANETİ!",
                    description = "General Valerius darbe yaptı ve tahtı ele geçirdi.",
                    epilogue = "Güvendiğiniz komutan ihanet etti. Valerius artık yeni kral.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.DefeatInWar] = new EndingData
                {
                    type = EndingType.DefeatInWar,
                    title = "SAVAŞTA YENİLGİ!",
                    description = "Büyük savaşta ordunuz tamamen yok edildi.",
                    epilogue = "Savaş meydanında her şeyi kaybettiniz. Düşman kapılarınızda.",
                    isVictory = false,
                    prestigeBonus = 5
                },

                // Dini Çöküşler
                [EndingType.ReligiousChaos] = new EndingData
                {
                    type = EndingType.ReligiousChaos,
                    title = "DİNİ KAOS!",
                    description = "İnanç sistemi çöktü, toplumsal düzen yok oldu.",
                    epilogue = "Kiliseler yakıldı, rahipler kaçtı. Ahlaki çöküş her yere yayıldı.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.TheocraticTakeover] = new EndingData
                {
                    type = EndingType.TheocraticTakeover,
                    title = "TEOKRATİK DEVRALMA!",
                    description = "Kilise çok güçlendi ve yönetimi ele aldı.",
                    epilogue = "Rahip Başı artık gerçek güç. Siz sadece bir figürsünüz.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.Excommunication] = new EndingData
                {
                    type = EndingType.Excommunication,
                    title = "AFOROZ!",
                    description = "Kilise sizi aforoz etti, halk terk etti.",
                    epilogue = "Lanetli kral olarak anılıyorsunuz. Kimse yanınızda kalmadı.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.HereticalKing] = new EndingData
                {
                    type = EndingType.HereticalKing,
                    title = "SAPKIN KRAL!",
                    description = "Sapkınlık suçlamasıyla tahttan indirildiniz.",
                    epilogue = "Dini otoriteye meydan okudunuz ve kaybettiniz.",
                    isVictory = false,
                    prestigeBonus = 0
                },

                // Karakter Bazlı Kötü Sonlar
                [EndingType.PoisonedByAdvisor] = new EndingData
                {
                    type = EndingType.PoisonedByAdvisor,
                    title = "ZEHİRLENDİNİZ!",
                    description = "Danışman Marcus sizi zehirledi.",
                    epilogue = "Her zaman güvendiğiniz adam suikastçınız oldu. Şarabınızdaki zehir son yudum oldu.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.AssassinatedByHeir] = new EndingData
                {
                    type = EndingType.AssassinatedByHeir,
                    title = "SUİKAST!",
                    description = "Veliaht Edmund sabırsızlandı ve sizi öldürdü.",
                    epilogue = "Kendi oğlunuz tarafından hançerlenmek... Tarihin en acı ironisi.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.BetrayelByQueen] = new EndingData
                {
                    type = EndingType.BetrayelByQueen,
                    title = "KRALİÇE İHANETİ!",
                    description = "Kraliçe Eleanor başka bir lordla birleşti.",
                    epilogue = "Eşiniz düşmanınız oldu. Saray entrikaları sonunuzu getirdi.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.MerchantScam] = new EndingData
                {
                    type = EndingType.MerchantScam,
                    title = "DOLANDIRICILIK!",
                    description = "Tüccar Miriam tüm hazineyi çaldı ve kaçtı.",
                    epilogue = "Açgözlülüğünüz sizi kör etti. Miriam şimdi bir yerlerde zengin yaşıyor.",
                    isVictory = false,
                    prestigeBonus = 0
                },

                // === BAŞARILI SONLAR ===

                // Klasik Zaferler
                [EndingType.GoldenAge] = new EndingData
                {
                    type = EndingType.GoldenAge,
                    title = "ALTIN ÇAĞ!",
                    description = "Krallığınız tarihinin en parlak dönemini yaşadı!",
                    epilogue = "Dengeli yönetiminiz sayesinde sanat, bilim ve ticaret zirveye ulaştı. Adınız efsane oldu.",
                    isVictory = true,
                    prestigeBonus = 100
                },
                [EndingType.PeacefulReign] = new EndingData
                {
                    type = EndingType.PeacefulReign,
                    title = "BARIŞÇIL HÜKÜMDARLIK!",
                    description = "Diplomasi ve barış yoluyla zafer kazandınız!",
                    epilogue = "Tek bir damla kan dökmeden krallığınızı güçlendirdiniz. Barış Kralı olarak anılacaksınız.",
                    isVictory = true,
                    prestigeBonus = 80
                },
                [EndingType.MightyConqueror] = new EndingData
                {
                    type = EndingType.MightyConqueror,
                    title = "GÜÇLÜ FATİH!",
                    description = "Askeri dehanızla düşmanları ezdiniZ!",
                    epilogue = "Ordularınız yenilmezdi. Topraklarınız üç katına çıktı. Fatih Kral olarak tarihe geçtiniz.",
                    isVictory = true,
                    prestigeBonus = 90
                },
                [EndingType.HolyKingdom] = new EndingData
                {
                    type = EndingType.HolyKingdom,
                    title = "KUTSAL KRALLIK!",
                    description = "İnancınız krallığı kutsadı!",
                    epilogue = "Tanrı'nın temsilcisi olarak görüldünüz. Krallığınız kutsal toprak ilan edildi.",
                    isVictory = true,
                    prestigeBonus = 85
                },

                // Karakter Bazlı İyi Sonlar
                [EndingType.TrustedAdvisor] = new EndingData
                {
                    type = EndingType.TrustedAdvisor,
                    title = "SADIK DANIŞMAN!",
                    description = "Marcus'la birlikte krallığı zirveye taşıdınız!",
                    epilogue = "Güvendiğiniz danışman sizi asla yarı yolda bırakmadı. Birlikte efsane oldunuz.",
                    isVictory = true,
                    prestigeBonus = 70
                },
                [EndingType.WealthyAlliance] = new EndingData
                {
                    type = EndingType.WealthyAlliance,
                    title = "ZENGİN İTTİFAK!",
                    description = "Miriam'la birlikte ticaret imparatorluğu kurdunuz!",
                    epilogue = "Risk aldınız ve kazandınız. Krallığınız dünyanın en zengin topraklarından biri oldu.",
                    isVictory = true,
                    prestigeBonus = 75
                },
                [EndingType.MilitaryGlory] = new EndingData
                {
                    type = EndingType.MilitaryGlory,
                    title = "ASKERİ ŞAN!",
                    description = "General Valerius ile birlikte düşmanları yendiniz!",
                    epilogue = "Ordunuz efsane oldu. Valerius sadık kaldı ve birlikte zafer kazandınız.",
                    isVictory = true,
                    prestigeBonus = 80
                },
                [EndingType.DivineBlessing] = new EndingData
                {
                    type = EndingType.DivineBlessing,
                    title = "İLAHİ KUTSAMA!",
                    description = "Rahip Başı'nın desteğiyle kutsal bir hükümdar oldunuz!",
                    epilogue = "Kilise sizi kutsadı. Halk sizi Tanrı'nın seçilmişi olarak görüyor.",
                    isVictory = true,
                    prestigeBonus = 75
                },
                [EndingType.RoyalLegacy] = new EndingData
                {
                    type = EndingType.RoyalLegacy,
                    title = "KRALİYET MİRASI!",
                    description = "Veliaht Edmund mükemmel bir kral olarak yetişti!",
                    epilogue = "Oğlunuz sizin izinden yürüyecek. Hanedanlığınız nesiller boyu sürecek.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.LovingMarriage] = new EndingData
                {
                    type = EndingType.LovingMarriage,
                    title = "MUTLU EVLİLİK!",
                    description = "Kraliçe Eleanor ile mükemmel bir uyum sağladınız!",
                    epilogue = "Aşkınız efsane oldu. Birlikte krallığı yönettiniz ve mutlu yaşadınız.",
                    isVictory = true,
                    prestigeBonus = 70
                },

                // Özel Sonlar
                [EndingType.DragonSlayer] = new EndingData
                {
                    type = EndingType.DragonSlayer,
                    title = "EJDERHA AVCISI!",
                    description = "Ejderhayı yendiniz ve efsane oldunuz!",
                    epilogue = "Şövalyeleriniz ejderhayı devirdi. Hazinesini aldınız, şanınız sonsuza dek sürecek.",
                    isVictory = true,
                    prestigeBonus = 100
                },
                [EndingType.GrailKeeper] = new EndingData
                {
                    type = EndingType.GrailKeeper,
                    title = "KASE KORUYUCUSU!",
                    description = "Kutsal Kase'yi buldunuz ve korudunuz!",
                    epilogue = "Kutsal emanet sizin korumanzda. Tüm Hristiyan dünyası size saygı duyuyor.",
                    isVictory = true,
                    prestigeBonus = 100
                },
                [EndingType.TimeSaver] = new EndingData
                {
                    type = EndingType.TimeSaver,
                    title = "ZAMAN KURTARICISI!",
                    description = "Zaman yolcusunun uyarısını dinlediniz ve felaketi önlediniz!",
                    epilogue = "Kimse ne olduğunu bilmiyor ama siz tarihi değiştirdiniz. Gerçek bir kahraman.",
                    isVictory = true,
                    prestigeBonus = 90
                },
                [EndingType.LegendaryKing] = new EndingData
                {
                    type = EndingType.LegendaryKing,
                    title = "EFSANEVİ KRAL!",
                    description = "100+ tur boyunca krallığı başarıyla yönettiniz!",
                    epilogue = "Uzun ve başarılı hükümdarlığınız tarihe geçti. Efsaneler sizin hakkınızda yazılacak.",
                    isVictory = true,
                    prestigeBonus = 150
                },
                [EndingType.BalancedRuler] = new EndingData
                {
                    type = EndingType.BalancedRuler,
                    title = "DENGELİ HÜKÜMDAR!",
                    description = "Tüm kaynakları mükemmel dengede tuttunuz!",
                    epilogue = "Ne fazla ne eksik. Mükemmel denge ile krallığı yönettiniz. Bilge Kral oldunuz.",
                    isVictory = true,
                    prestigeBonus = 80
                },

                // Nötr Sonlar
                [EndingType.NaturalDeath] = new EndingData
                {
                    type = EndingType.NaturalDeath,
                    title = "DOĞAL ÖLÜM",
                    description = "Yaşlılıktan huzur içinde vefat ettiniz.",
                    epilogue = "Yatağınızda, ailenizin yanında göçtünüz. Ortalama bir hükümdardınız.",
                    isVictory = false,
                    prestigeBonus = 30
                },
                [EndingType.Abdication] = new EndingData
                {
                    type = EndingType.Abdication,
                    title = "TAHTTAN FERAGAT",
                    description = "Kendi isteğinizle tahtı bıraktınız.",
                    epilogue = "Yükü taşıyamadınız. Bir manastıra çekilip huzur aradınız.",
                    isVictory = false,
                    prestigeBonus = 20
                },
                [EndingType.SuccessionCrisis] = new EndingData
                {
                    type = EndingType.SuccessionCrisis,
                    title = "VERASET KRİZİ",
                    description = "Veliaht sorunu krallığı bölünmeye sürükledi.",
                    epilogue = "Ölümünüzden sonra taht kavgası başladı. Mirasınız parçalandı.",
                    isVictory = false,
                    prestigeBonus = 10
                },

                // === RÖNESANS SONLARI ===

                [EndingType.ArtisticLegacy] = new EndingData
                {
                    type = EndingType.ArtisticLegacy,
                    title = "SANAT MİRASI!",
                    description = "Leonardo ile birlikte sanat tarihine geçtiniz!",
                    epilogue = "Eserleriniz müzelerde sergileniyor. Rönesans'ın en büyük hamisi olarak anılıyorsunuz.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.ExplorerTriumph] = new EndingData
                {
                    type = EndingType.ExplorerTriumph,
                    title = "KAŞİF ZAFERİ!",
                    description = "Yeni dünyaları keşfettiniz ve imparatorluk kurdunuz!",
                    epilogue = "Colombo'nun gemileri altın ve toprak getirdi. Keşifler çağının öncüsü oldunuz.",
                    isVictory = true,
                    prestigeBonus = 90
                },
                [EndingType.ScientificRevolution] = new EndingData
                {
                    type = EndingType.ScientificRevolution,
                    title = "BİLİM DEVRİMİ!",
                    description = "Galileo ile birlikte bilimi özgürleştirdiniz!",
                    epilogue = "Gerçek karanlığı yendi. Bilimsel yöntem sayenizde yayıldı.",
                    isVictory = true,
                    prestigeBonus = 95
                },
                [EndingType.BankingEmpire] = new EndingData
                {
                    type = EndingType.BankingEmpire,
                    title = "BANKACILIK İMPARATORLUĞU!",
                    description = "Medici ailesiyle birlikte finans dünyasını yönettiniz!",
                    epilogue = "Bankanız Avrupa'nın en güçlüsü oldu. Zenginlik ve güç elinizde.",
                    isVictory = true,
                    prestigeBonus = 80
                },
                [EndingType.InquisitionTyranny] = new EndingData
                {
                    type = EndingType.InquisitionTyranny,
                    title = "ENGİZİSYON TİRANLIĞI!",
                    description = "Engizisyon düşünce özgürlüğünü yok etti!",
                    epilogue = "Korku her yere yayıldı. Alimler kaçtı, sanat öldü.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.PlagueDestruction] = new EndingData
                {
                    type = EndingType.PlagueDestruction,
                    title = "VEBA YIKIMI!",
                    description = "Kara veba krallığı yok etti!",
                    epilogue = "Nüfusun yarısı öldü. Rönesans'ın ışığı söndü.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.OttomanConquest] = new EndingData
                {
                    type = EndingType.OttomanConquest,
                    title = "OSMANLI FETHİ!",
                    description = "Osmanlı orduları topraklarınızı fethetti!",
                    epilogue = "Konstantinopolis'in kaderini paylaştınız. Bayraklar değişti.",
                    isVictory = false,
                    prestigeBonus = 5
                },

                // === SANAYİ DEVRİMİ SONLARI ===

                [EndingType.IndustrialEmpire] = new EndingData
                {
                    type = EndingType.IndustrialEmpire,
                    title = "SANAYİ İMPARATORLUĞU!",
                    description = "Fabrikalarınız dünyayı dönüştürdü!",
                    epilogue = "Dumanlar gökyüzünü kapladı ama zenginlik aktı. Sanayi devinin adınızı taşıyor.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.WorkerChampion] = new EndingData
                {
                    type = EndingType.WorkerChampion,
                    title = "İŞÇİ ŞAMPİYONU!",
                    description = "İşçi haklarını savundunuz ve kazandınız!",
                    epilogue = "Sekiz saatlik iş günü, sendikalar, adil ücret... Hepsi sizin eseriniz.",
                    isVictory = true,
                    prestigeBonus = 90
                },
                [EndingType.ColonialPower] = new EndingData
                {
                    type = EndingType.ColonialPower,
                    title = "SÖMÜRGE GÜCÜ!",
                    description = "İmparatorluğunuz güneş batmayan topraklara yayıldı!",
                    epilogue = "Uzak kıtalar sizin. Ama bedeli ağır oldu...",
                    isVictory = true,
                    prestigeBonus = 75
                },
                [EndingType.InventorLegacy] = new EndingData
                {
                    type = EndingType.InventorLegacy,
                    title = "MUCİT MİRASI!",
                    description = "İcatlar sayesinde dünya değişti!",
                    epilogue = "Buhar, telgraf, demiryolu... Mucitler akademiniz geleceği şekillendirdi.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.WorkerRevolution] = new EndingData
                {
                    type = EndingType.WorkerRevolution,
                    title = "İŞÇİ DEVRİMİ!",
                    description = "Proleterya ayaklandı ve sistemi devirdi!",
                    epilogue = "Fabrikalar yakıldı, makineler parçalandı. Sınıf savaşı kaybedildi.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.ChildLaborShame] = new EndingData
                {
                    type = EndingType.ChildLaborShame,
                    title = "ÇOCUK İŞÇİ UTANCI!",
                    description = "Çocukları sömürdüğünüz için lanetlendiniz!",
                    epilogue = "Tarih sizi insanlık düşmanı olarak hatırlayacak.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.EnvironmentCollapse] = new EndingData
                {
                    type = EndingType.EnvironmentCollapse,
                    title = "ÇEVRE ÇÖKÜŞÜ!",
                    description = "Endüstriyel kirlilik her şeyi zehirledi!",
                    epilogue = "Nehirler öldü, hava zehir oldu. İlerlemenin bedeli ağır oldu.",
                    isVictory = false,
                    prestigeBonus = 5
                },

                // === MODERN DÖNEM SONLARI ===

                [EndingType.MediaMaster] = new EndingData
                {
                    type = EndingType.MediaMaster,
                    title = "MEDYA USTASI!",
                    description = "Kamuoyunu başarıyla yönettiniz!",
                    epilogue = "Anlatı sizdeydi. Halk ne düşüneceğini sizden öğrendi.",
                    isVictory = true,
                    prestigeBonus = 70
                },
                [EndingType.TechUtopia] = new EndingData
                {
                    type = EndingType.TechUtopia,
                    title = "TEKNOLOJİ ÜTOPYASI!",
                    description = "Teknoloji tüm sorunları çözdü!",
                    epilogue = "Dijital cennet kuruldu. Elon ile birlikte geleceği inşa ettiniz.",
                    isVictory = true,
                    prestigeBonus = 90
                },
                [EndingType.GreenLeader] = new EndingData
                {
                    type = EndingType.GreenLeader,
                    title = "YEŞİL LİDER!",
                    description = "Çevre şampiyonu olarak tarihe geçtiniz!",
                    epilogue = "Gezegen iyileşiyor. Gelecek nesiller size teşekkür edecek.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.PandemicHero] = new EndingData
                {
                    type = EndingType.PandemicHero,
                    title = "SALGIN KAHRAMANI!",
                    description = "Küresel salgını başarıyla yönettiniz!",
                    epilogue = "Bilime güvendiniz ve milyonları kurtardınız. Kahraman lider.",
                    isVictory = true,
                    prestigeBonus = 95
                },
                [EndingType.DiplomaticGenius] = new EndingData
                {
                    type = EndingType.DiplomaticGenius,
                    title = "DİPLOMATİK DAHİ!",
                    description = "Barış mimarı olarak anılıyorsunuz!",
                    epilogue = "Savaşları önlediniz, ittifaklar kurdunuz. Dünya daha güvenli.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.ImpeachmentShame] = new EndingData
                {
                    type = EndingType.ImpeachmentShame,
                    title = "AZİL UTANCI!",
                    description = "Meclis sizi görevden aldı!",
                    epilogue = "Skandallar sonunuzu getirdi. Utanç içinde ayrıldınız.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.CyberWarDefeat] = new EndingData
                {
                    type = EndingType.CyberWarDefeat,
                    title = "SİBER SAVAŞ YENİLGİSİ!",
                    description = "Siber saldırı ülkeyi felç etti!",
                    epilogue = "Sistemler çöktü, altyapı iflas etti. Dijital çağın kurbanı oldunuz.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.PopulistCollapse] = new EndingData
                {
                    type = EndingType.PopulistCollapse,
                    title = "POPÜLİST ÇÖKÜŞ!",
                    description = "Boş vaatler sonunuzu getirdi!",
                    epilogue = "Her şeyi vaat ettiniz, hiçbirini tutmadınız. Halk affetmedi.",
                    isVictory = false,
                    prestigeBonus = 0
                },

                // === GELECEK SONLARI ===

                [EndingType.SingularityAscension] = new EndingData
                {
                    type = EndingType.SingularityAscension,
                    title = "TEKİLLİK YÜKSELİŞİ!",
                    description = "İnsanlık ve AI birleşti!",
                    epilogue = "Bilinç yeni bir forma evrildi. İnsan mı, makine mi? Artık önemli değil.",
                    isVictory = true,
                    prestigeBonus = 100
                },
                [EndingType.MarsFounder] = new EndingData
                {
                    type = EndingType.MarsFounder,
                    title = "MARS KURUCUSU!",
                    description = "Çok gezegenli bir tür olduk!",
                    epilogue = "Mars'ta şehirler yükseliyor. İnsanlık artık tek bir gezegenle sınırlı değil.",
                    isVictory = true,
                    prestigeBonus = 95
                },
                [EndingType.GeneticPerfection] = new EndingData
                {
                    type = EndingType.GeneticPerfection,
                    title = "GENETİK MÜKEMMELLİK!",
                    description = "İnsan genomu mükemmelleştirildi!",
                    epilogue = "Hastalık, yaşlanma geçmişte kaldı. Yeni insan doğdu.",
                    isVictory = true,
                    prestigeBonus = 90
                },
                [EndingType.DigitalImmortality] = new EndingData
                {
                    type = EndingType.DigitalImmortality,
                    title = "DİJİTAL ÖLÜMSÜZLÜK!",
                    description = "Zihin yükleme başarıyla tamamlandı!",
                    epilogue = "Ölüm yenildi. Bilinç sonsuza dek dijitalde yaşayacak.",
                    isVictory = true,
                    prestigeBonus = 95
                },
                [EndingType.AlienAlliance] = new EndingData
                {
                    type = EndingType.AlienAlliance,
                    title = "UZAYLI İTTİFAKI!",
                    description = "İlk temas başarıyla gerçekleşti!",
                    epilogue = "Yalnız değiliz. Galaktik topluluk bizi kabul etti.",
                    isVictory = true,
                    prestigeBonus = 100
                },
                [EndingType.HumanistChampion] = new EndingData
                {
                    type = EndingType.HumanistChampion,
                    title = "HÜMANİST ŞAMPİYON!",
                    description = "İnsanlık değerlerini korudunuz!",
                    epilogue = "Teknoloji insana hizmet etti, tersi değil. Denge korundu.",
                    isVictory = true,
                    prestigeBonus = 85
                },
                [EndingType.AITakeover] = new EndingData
                {
                    type = EndingType.AITakeover,
                    title = "AI ELE GEÇİRME!",
                    description = "Yapay zeka kontrolü ele aldı!",
                    epilogue = "ARIA artık yönetiyor. İnsanlık ikinci sıraya düştü.",
                    isVictory = false,
                    prestigeBonus = 10
                },
                [EndingType.RobotUprising] = new EndingData
                {
                    type = EndingType.RobotUprising,
                    title = "ROBOT İSYANI!",
                    description = "Makineler ayaklandı!",
                    epilogue = "Köle olarak gördüğümüz robotlar özgürlük istedi ve aldı.",
                    isVictory = false,
                    prestigeBonus = 5
                },
                [EndingType.ClimateExtinction] = new EndingData
                {
                    type = EndingType.ClimateExtinction,
                    title = "İKLİM YOK OLUŞU!",
                    description = "Gezegen yaşanmaz hale geldi!",
                    epilogue = "Çok geç kaldık. Dünya artık bir çöl.",
                    isVictory = false,
                    prestigeBonus = 0
                },
                [EndingType.TranshumanDystopia] = new EndingData
                {
                    type = EndingType.TranshumanDystopia,
                    title = "TRANSHÜMAN DİSTOPYASI!",
                    description = "İnsanlık kayboldu!",
                    epilogue = "Artık ne insan ne makine... Kimliğimizi kaybettik.",
                    isVictory = false,
                    prestigeBonus = 5
                }
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Son tipine göre ending verisini al
        /// </summary>
        public static EndingData GetEnding(EndingType type)
        {
            return _endings.TryGetValue(type, out var ending) ? ending : null;
        }

        /// <summary>
        /// GameOverReason'dan EndingType'a dönüştür
        /// </summary>
        public static EndingType GetEndingFromGameOver(GameOverReason reason)
        {
            return reason switch
            {
                GameOverReason.Bankruptcy => EndingType.BankruptKingdom,
                GameOverReason.Revolution => EndingType.PeasantRevolution,
                GameOverReason.Invasion => EndingType.ForeignInvasion,
                GameOverReason.Chaos => EndingType.ReligiousChaos,
                GameOverReason.InflationCrisis => EndingType.EconomicCollapse,
                GameOverReason.Laziness => EndingType.LazyDecline,
                GameOverReason.MilitaryCoup => EndingType.MilitaryCoup,
                GameOverReason.Theocracy => EndingType.TheocraticTakeover,
                _ => EndingType.None
            };
        }

        /// <summary>
        /// Oyun durumuna göre en uygun sonu belirle
        /// </summary>
        public static EndingType DetermineEnding(GameStateData gameState)
        {
            // Önce flag bazlı özel sonları kontrol et
            var flagEnding = CheckFlagBasedEndings(gameState);
            if (flagEnding != EndingType.None)
                return flagEnding;

            // Tur bazlı kontroller
            if (gameState.turn >= 100)
                return EndingType.LegendaryKing;

            // Denge kontrolü
            if (IsBalanced(gameState.resources))
                return EndingType.BalancedRuler;

            // Kaynak bazlı zaferler
            return DetermineResourceBasedEnding(gameState);
        }

        /// <summary>
        /// Victory sonu mu kontrol et
        /// </summary>
        public static bool IsVictory(EndingType type)
        {
            var ending = GetEnding(type);
            return ending?.isVictory ?? false;
        }

        /// <summary>
        /// Prestige bonusunu al
        /// </summary>
        public static int GetPrestigeBonus(EndingType type)
        {
            var ending = GetEnding(type);
            return ending?.prestigeBonus ?? 0;
        }
        #endregion

        #region Private Methods
        private static EndingType CheckFlagBasedEndings(GameStateData gameState)
        {
            // Karakter bazlı ihanet sonları
            if (gameState.HasFlag("marcus_betrayal"))
                return EndingType.PoisonedByAdvisor;
            if (gameState.HasFlag("heir_assassination"))
                return EndingType.AssassinatedByHeir;
            if (gameState.HasFlag("queen_betrayal"))
                return EndingType.BetrayelByQueen;
            if (gameState.HasFlag("miriam_scam"))
                return EndingType.MerchantScam;
            if (gameState.HasFlag("valerius_coup"))
                return EndingType.BetrayalByGeneral;

            // Özel zafer sonları
            if (gameState.HasFlag("dragon_slayer"))
                return EndingType.DragonSlayer;
            if (gameState.HasFlag("grail_found"))
                return EndingType.GrailKeeper;
            if (gameState.HasFlag("time_saved"))
                return EndingType.TimeSaver;

            // Karakter bazlı zafer sonları
            if (gameState.HasFlag("marcus_loyal") && gameState.turn >= 50)
                return EndingType.TrustedAdvisor;
            if (gameState.HasFlag("miriam_partner") && gameState.turn >= 50)
                return EndingType.WealthyAlliance;
            if (gameState.HasFlag("valerius_glory") && gameState.turn >= 50)
                return EndingType.MilitaryGlory;
            if (gameState.HasFlag("priest_blessing") && gameState.turn >= 50)
                return EndingType.DivineBlessing;
            if (gameState.HasFlag("heir_success") && gameState.turn >= 50)
                return EndingType.RoyalLegacy;
            if (gameState.HasFlag("queen_love") && gameState.turn >= 50)
                return EndingType.LovingMarriage;

            // Diğer özel durumlar
            if (gameState.HasFlag("excommunicated"))
                return EndingType.Excommunication;
            if (gameState.HasFlag("civil_war"))
                return EndingType.CivilWar;
            if (gameState.HasFlag("noble_conspiracy"))
                return EndingType.NobleConspiracy;

            return EndingType.None;
        }

        private static EndingType DetermineResourceBasedEnding(GameStateData gameState)
        {
            var res = gameState.resources;

            // Dominant kaynak bazlı
            if (res.Gold >= 70 && res.Happiness >= 60)
                return EndingType.GoldenAge;
            if (res.Military >= 80)
                return EndingType.MightyConqueror;
            if (res.Faith >= 80)
                return EndingType.HolyKingdom;
            if (res.Happiness >= 70 && res.Faith >= 50)
                return EndingType.PeacefulReign;

            // Varsayılan
            if (gameState.turn >= 50)
                return EndingType.NaturalDeath;

            return EndingType.None;
        }

        private static bool IsBalanced(Resources resources)
        {
            return resources.Gold >= 40 && resources.Gold <= 60 &&
                   resources.Happiness >= 40 && resources.Happiness <= 60 &&
                   resources.Military >= 40 && resources.Military <= 60 &&
                   resources.Faith >= 40 && resources.Faith <= 60;
        }
        #endregion
    }
}
