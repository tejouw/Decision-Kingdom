import { Card, Character, ResourceType, EventCategory, ConditionType, Era } from '../models/types.js';

// Rönesans Karakterleri
const characters: Record<string, Character> = {
  sanatci: {
    id: 'sanatci',
    name: 'Usta Leonardo',
    title: 'Saray Ressamı',
    avatar: '🎨'
  },
  bilim_adami: {
    id: 'bilim_adami',
    name: 'Galileo',
    title: 'Astronom ve Bilim Adamı',
    avatar: '🔭'
  },
  kasif: {
    id: 'kasif',
    name: 'Kaptan Colombo',
    title: 'Denizci ve Kaşif',
    avatar: '🧭'
  },
  tuccar_ailesi: {
    id: 'tuccar_ailesi',
    name: 'Lorenzo Medici',
    title: 'Banka Ailesi Reisi',
    avatar: '🏛️'
  },
  kardinal: {
    id: 'kardinal',
    name: 'Kardinal Borgia',
    title: 'Kilise Temsilcisi',
    avatar: '⛪'
  },
  matbaaci: {
    id: 'matbaaci',
    name: 'Johannes Gutenberg',
    title: 'Matbaa Ustası',
    avatar: '📚'
  },
  mimar: {
    id: 'mimar',
    name: 'Mimar Brunelleschi',
    title: 'Baş Mimar',
    avatar: '🏰'
  },
  diplomat: {
    id: 'diplomat',
    name: 'Niccolò Machiavelli',
    title: 'Saray Diplomatı',
    avatar: '📜'
  },
  simyaci: {
    id: 'simyaci',
    name: 'Simyacı Paracelsus',
    title: 'Simya ve Tıp Uzmanı',
    avatar: '⚗️'
  },
  condottiere: {
    id: 'condottiere',
    name: 'Kondotyer Francesco',
    title: 'Paralı Asker Komutanı',
    avatar: '🗡️'
  }
};

// Rönesans Kartları
export const renaissanceCards: Card[] = [
  // ============= SANATÇI KARTLARI =============
  {
    id: 'ren_sanat_siparis',
    character: characters.sanatci,
    text: 'Efendim, büyük bir fresk projesi için patronaj istiyorum. Bu eser sizin ihtişamınızı yüzyıllarca yaşatacak.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.FAITH, min: -3, max: -1 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Finanse et',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['sanat_patronaji'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 2,
    isRepeatable: true,
    cooldown: 8
  },
  {
    id: 'ren_sanat_portre',
    character: characters.sanatci,
    text: 'Portrelerinizi yapmak istiyorum. Tarihe nasıl geçmek istersiniz?',
    leftChoice: {
      text: 'Savaşçı olarak',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      setFlags: ['savasci_portre']
    },
    rightChoice: {
      text: 'Bilge olarak',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      setFlags: ['bilge_portre']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'ren_sanat_heykel',
    character: characters.sanatci,
    text: 'Mermer bir heykel projesi öneriyorum. Antik Yunan tarzında, insan formunun mükemmelliğini yansıtacak.',
    leftChoice: {
      text: 'Çok pahalı',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Harika fikir',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      setFlags: ['ronesans_heykeli']
    },
    category: EventCategory.RANDOM,
    weight: 1,
    isRepeatable: true,
    cooldown: 10
  },
  {
    id: 'ren_sanat_kilise',
    character: characters.sanatci,
    text: 'Kilise benden bir sunak resmi istiyor. Ama ben insan anatomisini gerçekçi çizmek istiyorum. Kilise buna karşı çıkabilir.',
    leftChoice: {
      text: 'Geleneksel yap',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      relationshipChange: -5
    },
    rightChoice: {
      text: 'Vizyonunu takip et',
      effects: [
        { resource: ResourceType.FAITH, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ],
      setFlags: ['sanat_ozgurlugu'],
      relationshipChange: 10
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= BİLİM ADAMI KARTLARI =============
  {
    id: 'ren_bilim_teleskop',
    character: characters.bilim_adami,
    text: 'Yeni bir teleskop icat ettim. Gökyüzündeki keşiflerim kiliseyi rahatsız edebilir. Araştırmalarımı yayınlayayım mı?',
    leftChoice: {
      text: 'Gizli tut',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Yayınla',
      effects: [
        { resource: ResourceType.FAITH, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      setFlags: ['bilimsel_devrim'],
      triggeredEvents: ['ren_kilise_tepki'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ren_kilise_tepki',
    character: characters.kardinal,
    text: 'Bu sapkın bilim adamı kiliseye meydan okuyor! Engizisyon mahkemesine verilmeli!',
    leftChoice: {
      text: 'Bilim adamını koru',
      effects: [
        { resource: ResourceType.FAITH, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['bilim_korumasi']
    },
    rightChoice: {
      text: 'Kiliseye teslim et',
      effects: [
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      setFlags: ['bilim_baskisi']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'bilimsel_devrim' }
    ]
  },
  {
    id: 'ren_bilim_anatomi',
    character: characters.bilim_adami,
    text: 'İnsan anatomisini incelemek için cesetlere ihtiyacım var. Yasal olmasa da bilim için şart.',
    leftChoice: {
      text: 'İzin verme',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Göz yum',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ],
      setFlags: ['anatomi_calismalari']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'ren_bilim_universite',
    character: characters.bilim_adami,
    text: 'Bir üniversite kurulmasını öneriyorum. Bilgi merkezi olur ama masraflı ve kilise karşı çıkabilir.',
    leftChoice: {
      text: 'Şimdi değil',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Üniversite kur',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['universite_kuruldu']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= KAŞİF KARTLARI =============
  {
    id: 'ren_kesif_yeni_dunya',
    character: characters.kasif,
    text: 'Batıya giderek yeni topraklar keşfetmek istiyorum. Riskli ama büyük kazanç getirebilir.',
    leftChoice: {
      text: 'Çok riskli',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Seferi finanse et',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -20 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      triggeredEvents: ['ren_kesif_sonuc'],
      setFlags: ['kesif_seferi'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ren_kesif_sonuc',
    character: characters.kasif,
    text: 'Harika haberler! Yeni topraklar keşfettik! Altın ve baharat dolu bir kıta bulduk!',
    leftChoice: {
      text: 'Kolonileştir',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['koloni_kuruldu']
    },
    rightChoice: {
      text: 'Ticaret yap',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 25 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['ticaret_yolu']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'kesif_seferi' }
    ]
  },
  {
    id: 'ren_kesif_harita',
    character: characters.kasif,
    text: 'Deniz haritalarımızı güncellemeliyiz. Yeni rotalar bulmamız gerekiyor.',
    leftChoice: {
      text: 'Eski haritalar yeter',
      effects: [
        { resource: ResourceType.GOLD, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Haritacılığa yatırım yap',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      setFlags: ['gelismis_haritacilik']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ren_kesif_baharat',
    character: characters.kasif,
    text: 'Doğu\'ya baharat yolu açabiliriz. Osmanlı topraklarından geçmeden yeni bir rota.',
    leftChoice: {
      text: 'Çok tehlikeli',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Rotayı aç',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      triggeredEvents: ['ren_baharat_sonuc'],
      setFlags: ['baharat_seferi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_baharat_sonuc',
    character: characters.kasif,
    text: 'Baharat yolunu bulduk! Karabiber, tarçın ve muskat cevizi getirdik!',
    leftChoice: {
      text: 'Tekel kur',
      effects: [
        { resource: ResourceType.GOLD, min: 25, max: 35 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Serbest ticaret',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ]
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'baharat_seferi' }
    ]
  },

  // ============= TÜCCAR AİLESİ KARTLARI =============
  {
    id: 'ren_banka_kredi',
    character: characters.tuccar_ailesi,
    text: 'Size büyük bir kredi teklif ediyorum. Düşük faiz, uzun vade. Karşılığında ticaret ayrıcalıkları istiyorum.',
    leftChoice: {
      text: 'Bağımsız kal',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Krediyi al',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['banka_borcu'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 2
  },
  {
    id: 'ren_banka_borc',
    character: characters.tuccar_ailesi,
    text: 'Borcunuzun vadesi geldi. Ödeme yapacak mısınız yoksa yeniden yapılandıralım mı?',
    leftChoice: {
      text: 'Öde',
      effects: [
        { resource: ResourceType.GOLD, min: -35, max: -25 }
      ],
      removeFlags: ['banka_borcu'],
      relationshipChange: 10
    },
    rightChoice: {
      text: 'Yeniden yapılandır',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['borc_artisi'],
      relationshipChange: -5
    },
    priority: 8,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'banka_borcu' },
      { type: ConditionType.TURN_ABOVE, value: 5 }
    ]
  },
  {
    id: 'ren_banka_ortaklik',
    character: characters.tuccar_ailesi,
    text: 'Ticaret filosuna ortak olmamı ister misiniz? Kar paylaşırız.',
    leftChoice: {
      text: 'Tek başıma',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Ortaklık kur',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.MILITARY, min: 3, max: 5 }
      ],
      setFlags: ['ticaret_ortakligi']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ren_banka_saray',
    character: characters.tuccar_ailesi,
    text: 'Yeni bir saray inşa etmenizi finanse edebilirim. İhtişamınız tüm Avrupa\'da konuşulur.',
    leftChoice: {
      text: 'Mütevazı kal',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Sarayı yap',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      setFlags: ['yeni_saray']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= KARDİNAL KARTLARI =============
  {
    id: 'ren_kilise_endulians',
    character: characters.kardinal,
    text: 'Endüljans satışı yapmamızı öneriyorum. Kilise için büyük gelir, halk günahlarından arınır.',
    leftChoice: {
      text: 'Bu yanlış',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Satışa izin ver',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.FAITH, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['endulians_satisi'],
      relationshipChange: 10
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ren_kilise_reform',
    character: characters.kardinal,
    text: 'Martin Luther adında bir rahip kiliseyi eleştiriyor. 95 tez yayınlamış! Bunu durdurmamız gerek.',
    leftChoice: {
      text: 'Diyalog kur',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['protestan_diyalog']
    },
    rightChoice: {
      text: 'Bastır',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['protestan_baskisi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_kilise_katedral',
    character: characters.kardinal,
    text: 'Aziz Petrus Bazilikası için bağış topluyoruz. Katkıda bulunur musunuz?',
    leftChoice: {
      text: 'Hayır',
      effects: [
        { resource: ResourceType.FAITH, min: -8, max: -5 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Cömertçe bağışla',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.FAITH, min: 15, max: 20 }
      ],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 1,
    isRepeatable: true,
    cooldown: 8
  },

  // ============= MATBAACI KARTLARI =============
  {
    id: 'ren_matbaa_kurulis',
    character: characters.matbaaci,
    text: 'Hareketli harflerle çalışan bir matbaa icat ettim. Kitapları hızlıca çoğaltabilirim. Yatırım yapar mısınız?',
    leftChoice: {
      text: 'El yazmaları yeter',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Matbaaya yatırım yap',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['matbaa_kuruldu'],
      relationshipChange: 20
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ren_matbaa_incil',
    character: characters.matbaaci,
    text: 'İncil\'i halk diliyle basabilirim. Herkes okuyabilir ama kilise karşı çıkacak.',
    leftChoice: {
      text: 'Latince kalsın',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Halk dilinde bas',
      effects: [
        { resource: ResourceType.FAITH, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['halk_incili']
    },
    category: EventCategory.STORY,
    weight: 1,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'matbaa_kuruldu' }
    ]
  },
  {
    id: 'ren_matbaa_gazete',
    character: characters.matbaaci,
    text: 'Haftalık haber bülteni basabilirim. Halk bilgilensin.',
    leftChoice: {
      text: 'Haber kontrolü lazım',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Gazete çıkar',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['ilk_gazete']
    },
    category: EventCategory.RANDOM,
    weight: 1,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'matbaa_kuruldu' }
    ]
  },

  // ============= MİMAR KARTLARI =============
  {
    id: 'ren_mimar_kubbe',
    character: characters.mimar,
    text: 'Devasa bir kubbe inşa etmek istiyorum. Roma\'dan beri görülmemiş bir mühendislik harikası olacak.',
    leftChoice: {
      text: 'İmkansız',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'İnşaata başla',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['buyuk_kubbe'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_mimar_sehir',
    character: characters.mimar,
    text: 'Şehir planlaması öneriyorum. Geniş caddeler, meydanlar, çeşmeler. Rönesans şehri olalım.',
    leftChoice: {
      text: 'Çok masraflı',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Şehri yenile',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      setFlags: ['sehir_plani']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_mimar_kale',
    character: characters.mimar,
    text: 'Yeni top teknolojisine karşı yıldız kale tasarladım. Savunmamızı güçlendirir.',
    leftChoice: {
      text: 'Surlar yeterli',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Kaleyi yap',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 15, max: 20 }
      ],
      setFlags: ['yildiz_kale']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },

  // ============= DİPLOMAT KARTLARI =============
  {
    id: 'ren_diplomat_ittifak',
    character: characters.diplomat,
    text: 'Fransa ile ittifak kurabiliriz. Güçlü bir müttefik ama İspanya\'yı kızdırır.',
    leftChoice: {
      text: 'Tarafsız kal',
      effects: [
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'İttifak kur',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      setFlags: ['fransiz_ittifaki']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_diplomat_evlilik',
    character: characters.diplomat,
    text: 'İspanyol prensesiyle evlilik ittifakı öneriyorum. Akdeniz\'de güç kazanırız.',
    leftChoice: {
      text: 'Bağımsız kal',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Evliliği kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['ispanyol_evliligi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_diplomat_casus',
    character: characters.diplomat,
    text: 'Venedik\'e casus göndermeliyiz. Ticaret sırlarını öğrenelim.',
    leftChoice: {
      text: 'Etik değil',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Casusları gönder',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      triggeredEvents: ['ren_casus_sonuc'],
      setFlags: ['venedik_casusluk']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ren_casus_sonuc',
    character: characters.diplomat,
    text: 'Casuslarımız yakalandı! Venedik savaş ilan etmekle tehdit ediyor.',
    leftChoice: {
      text: 'Özür dile',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Meydan oku',
      effects: [
        { resource: ResourceType.MILITARY, min: -15, max: -10 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      setFlags: ['venedik_dusmanligi']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'venedik_casusluk' }
    ]
  },

  // ============= SİMYACI KARTLARI =============
  {
    id: 'ren_simya_altin',
    character: characters.simyaci,
    text: 'Kurşunu altına çevirmeye çok yakınım! Biraz daha finansman gerekiyor.',
    leftChoice: {
      text: 'Saçmalık',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Devam et',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 }
      ],
      triggeredEvents: ['ren_simya_sonuc'],
      setFlags: ['simya_deney'],
      relationshipChange: 10
    },
    category: EventCategory.CHARACTER,
    weight: 1,
    isRepeatable: true,
    cooldown: 6
  },
  {
    id: 'ren_simya_sonuc',
    character: characters.simyaci,
    text: 'Altın yapamadım ama yeni bir ilaç keşfettim! Vebaya karşı etkili olabilir.',
    leftChoice: {
      text: 'İşe yaramaz',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'İlacı üret',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['yeni_ilac']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'simya_deney' }
    ]
  },
  {
    id: 'ren_simya_zehir',
    character: characters.simyaci,
    text: 'Düşmanlarınız için... özel bir iksir hazırlayabilirim. Kimse anlamaz.',
    leftChoice: {
      text: 'Asla!',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Hazırla',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      setFlags: ['zehir_stogu']
    },
    category: EventCategory.RARE,
    weight: 1
  },

  // ============= KONDOTİYER KARTLARI =============
  {
    id: 'ren_kondotiyer_kiralama',
    character: characters.condottiere,
    text: 'Deneyimli bir paralı asker birliğiyim. Hizmetlerim pahalı ama etkili.',
    leftChoice: {
      text: 'Kendi ordumuz var',
      effects: [
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Kirala',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 20, max: 25 }
      ],
      setFlags: ['parali_asker'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 2
  },
  {
    id: 'ren_kondotiyer_sadakat',
    character: characters.condottiere,
    text: 'Düşman daha yüksek ödeme teklif ediyor. Beni tutmak için zam yapmalısınız.',
    leftChoice: {
      text: 'Git o zaman',
      effects: [
        { resource: ResourceType.MILITARY, min: -15, max: -10 }
      ],
      removeFlags: ['parali_asker'],
      relationshipChange: -20
    },
    rightChoice: {
      text: 'Zam yap',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      relationshipChange: 10
    },
    priority: 8,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'parali_asker' }
    ]
  },
  {
    id: 'ren_kondotiyer_eğitim',
    character: characters.condottiere,
    text: 'Ordunuzu eğitmemi ister misiniz? Modern savaş taktikleri öğretirim.',
    leftChoice: {
      text: 'Gerek yok',
      effects: [
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Eğit',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 15, max: 20 }
      ],
      setFlags: ['egitimli_ordu']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },

  // ============= GENEL OLAYLAR =============
  {
    id: 'ren_veba_salgini',
    character: characters.simyaci,
    text: 'Şehirde veba salgını başladı! Acil önlem almalıyız.',
    leftChoice: {
      text: 'Karantina uygula',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['karantina']
    },
    rightChoice: {
      text: 'Normal yaşam devam etsin',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1,
    isRepeatable: true,
    cooldown: 15
  },
  {
    id: 'ren_italya_savasi',
    character: characters.condottiere,
    text: 'Fransa İtalya\'ya saldırıyor! Müdahale etmeli miyiz?',
    leftChoice: {
      text: 'Tarafsız kal',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 },
        { resource: ResourceType.GOLD, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Savaşa katıl',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['italya_savasi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_humanizm',
    character: characters.bilim_adami,
    text: 'Hümanist düşünce yayılıyor. İnsanı merkeze alan felsefe. Kilise endişeli.',
    leftChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Destekle',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['humanizm_destegi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ren_opera',
    character: characters.sanatci,
    text: 'Yeni bir sanat formu: Opera! Müzik ve drama birleşimi. İlk operayı sahneleyeyim mi?',
    leftChoice: {
      text: 'Gereksiz',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Sahne kur',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['opera_evi']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ren_ticaret_fuari',
    character: characters.tuccar_ailesi,
    text: 'Uluslararası ticaret fuarı düzenleyelim. Tüm Avrupa\'dan tüccarlar gelsin.',
    leftChoice: {
      text: 'Riskli',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Fuarı düzenle',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['ticaret_fuari']
    },
    category: EventCategory.RANDOM,
    weight: 1,
    isRepeatable: true,
    cooldown: 10
  },
  {
    id: 'ren_antik_eser',
    character: characters.mimar,
    text: 'Kazılarda antik Roma heykelleri bulduk. Bunları nereye koyalım?',
    leftChoice: {
      text: 'Kiliseye ver',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Müze kur',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['ilk_muze']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ren_perspektif',
    character: characters.sanatci,
    text: 'Perspektif tekniğini mükemmelleştirdim. Resimler artık üç boyutlu görünecek!',
    leftChoice: {
      text: 'Geleneksel tarz daha iyi',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Tüm sanatçılara öğret',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['perspektif_sanati']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'ren_lonca_isyani',
    character: characters.tuccar_ailesi,
    text: 'Zanaatkar loncaları daha fazla hak istiyor. Grev yapabilirler.',
    leftChoice: {
      text: 'Hakları ver',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ]
    },
    rightChoice: {
      text: 'Grevi bastır',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.GOLD, min: 5, max: 10 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ren_barut',
    character: characters.condottiere,
    text: 'Barut silahları yaygınlaşıyor. Topçu birliği kuralım mı?',
    leftChoice: {
      text: 'Geleneksel silahlar yeter',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Topçu birliği kur',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 20, max: 25 }
      ],
      setFlags: ['topcu_birligi']
    },
    category: EventCategory.STORY,
    weight: 1
  }
];
