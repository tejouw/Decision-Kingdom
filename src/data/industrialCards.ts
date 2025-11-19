import { Card, Character, ResourceType, EventCategory, ConditionType, Era } from '../models/types.js';

// Sanayi Devrimi Karakterleri
const characters: Record<string, Character> = {
  fabrikator: {
    id: 'fabrikator',
    name: 'Sanayici Edward',
    title: 'Fabrika Sahibi',
    avatar: '🏭'
  },
  isci_lideri: {
    id: 'isci_lideri',
    name: 'İşçi Lideri Thomas',
    title: 'Sendika Başkanı',
    avatar: '👷'
  },
  somurge_valisi: {
    id: 'somurge_valisi',
    name: 'Vali Wellington',
    title: 'Sömürge Valisi',
    avatar: '🎖️'
  },
  mucit: {
    id: 'mucit',
    name: 'Mucit James',
    title: 'Mühendis ve Mucit',
    avatar: '⚙️'
  },
  bankaci: {
    id: 'bankaci',
    name: 'Banker Rothschild',
    title: 'Finans Baronu',
    avatar: '🏦'
  },
  gazeteci: {
    id: 'gazeteci',
    name: 'Gazeteci Victoria',
    title: 'Araştırmacı Gazeteci',
    avatar: '📰'
  },
  madenci: {
    id: 'madenci',
    name: 'Madenci William',
    title: 'Maden İşçisi Temsilcisi',
    avatar: '⛏️'
  },
  doktor: {
    id: 'doktor',
    name: 'Dr. Florence',
    title: 'Hastane Müdürü',
    avatar: '🏥'
  },
  demiryolu: {
    id: 'demiryolu',
    name: 'Demiryolcu George',
    title: 'Demiryolu Şirketi Başkanı',
    avatar: '🚂'
  },
  reformcu: {
    id: 'reformcu',
    name: 'Reformcu Charles',
    title: 'Sosyal Reformcu',
    avatar: '📋'
  }
};

// Sanayi Devrimi Kartları
export const industrialCards: Card[] = [
  // ============= FABRİKATÖR KARTLARI =============
  {
    id: 'ind_fabrika_kurulum',
    character: characters.fabrikator,
    text: 'Yeni bir tekstil fabrikası kurmak istiyorum. Büyük yatırım ama karlı olacak.',
    leftChoice: {
      text: 'El emeği yeterli',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Fabrikayı kur',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      triggeredEvents: ['ind_fabrika_acilis'],
      setFlags: ['fabrika_kuruldu'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ind_fabrika_acilis',
    character: characters.fabrikator,
    text: 'Fabrika açıldı! Üretim başladı. Her hafta binlerce parça üretiyoruz.',
    leftChoice: {
      text: 'İşçi haklarına dikkat et',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['adil_fabrika']
    },
    rightChoice: {
      text: 'Karı maksimize et',
      effects: [
        { resource: ResourceType.GOLD, min: 25, max: 35 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['somuru_fabrikasi']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'fabrika_kuruldu' }
    ]
  },
  {
    id: 'ind_fabrika_cocuk',
    character: characters.fabrikator,
    text: 'Çocuk işçiler daha ucuz ve küçük elleri makinelere giriyor. Kullanmamı ister misiniz?',
    leftChoice: {
      text: 'Kesinlikle hayır',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['cocuk_isci_yasagi']
    },
    rightChoice: {
      text: 'Ekonomik zorunluluk',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['cocuk_isci']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_fabrika_genisleme',
    character: characters.fabrikator,
    text: 'Talebi karşılayamıyoruz. İkinci fabrika açmalıyız.',
    leftChoice: {
      text: 'Mevcut yeterli',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Genişle',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 }
      ],
      triggeredEvents: ['ind_fabrika_empire'],
      setFlags: ['fabrika_zinciri']
    },
    category: EventCategory.RANDOM,
    weight: 1,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'fabrika_kuruldu' }
    ]
  },
  {
    id: 'ind_fabrika_empire',
    character: characters.fabrikator,
    text: 'Artık bir sanayi imparatorluğumuz var! Piyasayı domine ediyoruz.',
    leftChoice: {
      text: 'Rekabeti koru',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 25 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Tekel kur',
      effects: [
        { resource: ResourceType.GOLD, min: 35, max: 45 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      setFlags: ['sanayi_tekeli']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'fabrika_zinciri' }
    ]
  },

  // ============= İŞÇİ LİDERİ KARTLARI =============
  {
    id: 'ind_isci_sendika',
    character: characters.isci_lideri,
    text: 'İşçiler birleşiyor. Sendika kurmak istiyoruz. İzin verecek misiniz?',
    leftChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['sendika_yasagi'],
      relationshipChange: -20
    },
    rightChoice: {
      text: 'İzin ver',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['sendika_hakki'],
      relationshipChange: 20
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ind_isci_grev',
    character: characters.isci_lideri,
    text: 'Fabrikalar durdu! Genel grev başladı! İşçiler daha iyi koşullar istiyor.',
    leftChoice: {
      text: 'Müzakere et',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['isci_haklari']
    },
    rightChoice: {
      text: 'Polisi gönder',
      effects: [
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -25, max: -20 }
      ],
      setFlags: ['grev_baskisi']
    },
    category: EventCategory.STORY,
    weight: 1,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'sendika_hakki' }
    ]
  },
  {
    id: 'ind_isci_calisma_saati',
    character: characters.isci_lideri,
    text: 'Günde 16 saat çalışıyoruz. 10 saate indirilmesini istiyoruz.',
    leftChoice: {
      text: '10 saat çok az',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Makul talep',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['calisma_saati_siniri'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'ind_isci_ucret',
    character: characters.isci_lideri,
    text: 'Asgari ücret belirlenmeli. Ailelerimizi geçindiremiyoruz.',
    leftChoice: {
      text: 'Piyasa belirler',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Asgari ücret koy',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['asgari_ucret']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= SÖMÜRGE VALİSİ KARTLARI =============
  {
    id: 'ind_somurge_genisleme',
    character: characters.somurge_valisi,
    text: 'Afrika\'da yeni topraklar işgal edebiliriz. Hammadde kaynakları zengin.',
    leftChoice: {
      text: 'Emperyalizm yanlış',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 10 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Toprakları al',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['somurge_genislemesi'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ind_somurge_isyan',
    character: characters.somurge_valisi,
    text: 'Sömürgede isyan çıktı! Yerli halk bağımsızlık istiyor.',
    leftChoice: {
      text: 'Özerklik ver',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['somurge_ozerklik']
    },
    rightChoice: {
      text: 'İsyanı bastır',
      effects: [
        { resource: ResourceType.MILITARY, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['somurge_baskisi']
    },
    category: EventCategory.STORY,
    weight: 1,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'somurge_genislemesi' }
    ]
  },
  {
    id: 'ind_somurge_kaucuk',
    character: characters.somurge_valisi,
    text: 'Kongo\'da kauçuk plantasyonları kurabiliriz. Ama çalışma koşulları sert olacak.',
    leftChoice: {
      text: 'İnsanlık dışı',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Ekonomik gereklilik',
      effects: [
        { resource: ResourceType.GOLD, min: 25, max: 35 },
        { resource: ResourceType.FAITH, min: -20, max: -15 }
      ],
      setFlags: ['kaucuk_somurgesi']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'ind_somurge_hindistan',
    character: characters.somurge_valisi,
    text: 'Hindistan\'da çay üretimini artırmalıyız. Daha fazla işçi gerekiyor.',
    leftChoice: {
      text: 'Mevcut yeterli',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Üretimi artır',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },

  // ============= MUCİT KARTLARI =============
  {
    id: 'ind_mucit_buhar',
    character: characters.mucit,
    text: 'Yeni buhar makinesi tasarladım. Daha verimli ve güçlü. Patent alayım mı?',
    leftChoice: {
      text: 'Mevcut yeterli',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Patenti al',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 }
      ],
      triggeredEvents: ['ind_buhar_devrim'],
      setFlags: ['buhar_patenti'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ind_buhar_devrim',
    character: characters.mucit,
    text: 'Yeni buhar makinesi tüm sektörleri dönüştürüyor. Sanayi devrimi hızlanıyor!',
    leftChoice: {
      text: 'Kontrollü büyüme',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 25 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Tam gaz ilerle',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['hizli_sanayilesme']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'buhar_patenti' }
    ]
  },
  {
    id: 'ind_mucit_telgraf',
    character: characters.mucit,
    text: 'Elektrikli telgraf icat ettim! Anında iletişim mümkün olacak.',
    leftChoice: {
      text: 'Posta yeterli',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Telgraf ağı kur',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['telgraf_agi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_mucit_elektrik',
    character: characters.mucit,
    text: 'Elektrik ampulü üzerinde çalışıyorum. Geceleri aydınlık olacak!',
    leftChoice: {
      text: 'Gaz lambası yeter',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Araştırmayı finanse et',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 }
      ],
      triggeredEvents: ['ind_elektrik_cagi'],
      setFlags: ['elektrik_arastirmasi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_elektrik_cagi',
    character: characters.mucit,
    text: 'Ampul çalışıyor! Elektrik çağı başlıyor!',
    leftChoice: {
      text: 'Sadece zenginlere',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Herkese elektrik',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 }
      ],
      setFlags: ['elektrik_altyapisi']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'elektrik_arastirmasi' }
    ]
  },

  // ============= BANKACI KARTLARI =============
  {
    id: 'ind_banka_sanayi',
    character: characters.bankaci,
    text: 'Sanayi yatırımları için kredi paketi hazırladık. Sanayicilere destek olalım.',
    leftChoice: {
      text: 'Riskli',
      effects: [
        { resource: ResourceType.GOLD, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Kredi ver',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['sanayi_kredisi']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ind_banka_kriz',
    character: characters.bankaci,
    text: 'Bankalar zor durumda! Panik başladı. Devlet müdahalesi gerekebilir.',
    leftChoice: {
      text: 'Serbest piyasa',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 }
      ],
      setFlags: ['banka_iflasi']
    },
    rightChoice: {
      text: 'Bankaları kurtar',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['banka_kurtarma']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_banka_altin',
    character: characters.bankaci,
    text: 'Altın standardına geçelim. Para birimimiz daha güvenilir olur.',
    leftChoice: {
      text: 'Esnek para daha iyi',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Altın standardı',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['altin_standardi']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= GAZETECİ KARTLARI =============
  {
    id: 'ind_gazete_skandal',
    character: characters.gazeteci,
    text: 'Fabrikaların korkunç koşullarını belgeledim. Halkın bilmesi gerekiyor.',
    leftChoice: {
      text: 'Yayınlama',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Yayınla',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      triggeredEvents: ['ind_kamuoyu_baskisi'],
      setFlags: ['skandal_haberi'],
      relationshipChange: 10
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_kamuoyu_baskisi',
    character: characters.reformcu,
    text: 'Haber büyük yankı uyandırdı! Halk reform istiyor.',
    leftChoice: {
      text: 'Baskıyı görmezden gel',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ]
    },
    rightChoice: {
      text: 'Reform yap',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 }
      ],
      setFlags: ['fabrika_reformu']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'skandal_haberi' }
    ]
  },
  {
    id: 'ind_gazete_basin',
    character: characters.gazeteci,
    text: 'Basın özgürlüğü kısıtlanıyor. Gazeteciler hapse atılıyor.',
    leftChoice: {
      text: 'Düzen için gerekli',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Özgürlüğü koru',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['basin_ozgurlugu']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= MADENCİ KARTLARI =============
  {
    id: 'ind_maden_guvenlik',
    character: characters.madenci,
    text: 'Madenlerde her gün kazalar oluyor. Güvenlik önlemleri istiyoruz.',
    leftChoice: {
      text: 'Çok pahalı',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Güvenliği artır',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['maden_guvenligi'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'ind_maden_grizu',
    character: characters.madenci,
    text: 'Grizu patlaması! Onlarca madenci mahsur kaldı!',
    leftChoice: {
      text: 'Kurtarma operasyonu',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Çok tehlikeli',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -25, max: -20 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ind_maden_komur',
    character: characters.madenci,
    text: 'Yeni kömür damarı keşfettik. Ama orman alanında.',
    leftChoice: {
      text: 'Ormanı koru',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Madeni aç',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['cevre_tahribati']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= DOKTOR KARTLARI =============
  {
    id: 'ind_saglik_kolera',
    character: characters.doktor,
    text: 'Kolera salgını yayılıyor! Temiz su şart. Kanalizasyon sistemi kurmalıyız.',
    leftChoice: {
      text: 'Çok masraflı',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Kanalizasyonu yap',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['kanalizasyon_sistemi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_saglik_hastane',
    character: characters.doktor,
    text: 'Devlet hastanesi açmalıyız. Fakir halk doktora gidemiyor.',
    leftChoice: {
      text: 'Özel sektör halleder',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Hastane aç',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['devlet_hastanesi']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'ind_saglik_asi',
    character: characters.doktor,
    text: 'Çiçek hastalığına karşı aşı geliştirdim. Zorunlu aşılama yapmalı mıyız?',
    leftChoice: {
      text: 'Gönüllü olsun',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Zorunlu aşılama',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.FAITH, min: 5, max: 10 }
      ],
      setFlags: ['zorunlu_asi']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= DEMİRYOLU KARTLARI =============
  {
    id: 'ind_demiryolu_kurulum',
    character: characters.demiryolu,
    text: 'Kıtalararası demiryolu projesi öneriyorum. Devasa yatırım ama ulaşım değişecek.',
    leftChoice: {
      text: 'At arabaları yeter',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Demiryolunu yap',
      effects: [
        { resource: ResourceType.GOLD, min: -35, max: -30 }
      ],
      triggeredEvents: ['ind_demiryolu_acilis'],
      setFlags: ['demiryolu_projesi'],
      relationshipChange: 20
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'ind_demiryolu_acilis',
    character: characters.demiryolu,
    text: 'Demiryolu hattı açıldı! İlk tren törenle yola çıktı!',
    leftChoice: {
      text: 'Yük taşımacılığı öncelik',
      effects: [
        { resource: ResourceType.GOLD, min: 25, max: 30 }
      ],
      setFlags: ['yuk_demiryolu']
    },
    rightChoice: {
      text: 'Yolcu taşımacılığı öncelik',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['yolcu_demiryolu']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'demiryolu_projesi' }
    ]
  },
  {
    id: 'ind_demiryolu_kaza',
    character: characters.demiryolu,
    text: 'Tren kazası! Raylar arızalıymış. Güvenlik standartları sorgulanıyor.',
    leftChoice: {
      text: 'Hız sınırı koy',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Rayları yenile',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['demiryolu_modernizasyon']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },

  // ============= REFORMCU KARTLARI =============
  {
    id: 'ind_reform_egitim',
    character: characters.reformcu,
    text: 'Zorunlu ilköğretim kanunu çıkarmalıyız. Her çocuk okula gitmeli.',
    leftChoice: {
      text: 'Aileler karar versin',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Zorunlu eğitim',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['zorunlu_egitim']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_reform_oy',
    character: characters.reformcu,
    text: 'Seçim hakkı genişletilmeli. Sadece toprak sahipleri değil, herkes oy kullanmalı.',
    leftChoice: {
      text: 'Tehlikeli',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Genel oy hakkı',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['genel_oy_hakki']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_reform_kadin',
    character: characters.reformcu,
    text: 'Kadınlar da oy hakkı istiyor. Suffrajet hareketi büyüyor.',
    leftChoice: {
      text: 'Toplum hazır değil',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Eşit haklar',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['kadin_oy_hakki']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= GENEL OLAYLAR =============
  {
    id: 'ind_cevre_kirlilik',
    character: characters.doktor,
    text: 'Hava kirliliği dayanılmaz boyutlarda. Londra\'nın üstünde sis örtüsü.',
    leftChoice: {
      text: 'İlerlemenin bedeli',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Temiz hava yasası',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['cevre_yasasi']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'ind_sinif_catismasi',
    character: characters.isci_lideri,
    text: 'Zengin-fakir uçurumu büyüyor. Sosyalist fikirler yayılıyor.',
    leftChoice: {
      text: 'Düzeni koru',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ]
    },
    rightChoice: {
      text: 'Sosyal devlet',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 }
      ],
      setFlags: ['sosyal_devlet']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'ind_dunya_fuari',
    character: characters.fabrikator,
    text: 'Dünya fuarı düzenleyelim! Sanayimizi tüm dünyaya gösterelim.',
    leftChoice: {
      text: 'Çok gösterişli',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Fuarı düzenle',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.MILITARY, min: 5, max: 10 }
      ],
      setFlags: ['dunya_fuari']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'ind_buhran',
    character: characters.bankaci,
    text: 'Ekonomik buhran! Borsalar çöktü, fabrikalar kapanıyor.',
    leftChoice: {
      text: 'Serbest piyasa toparlar',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 }
      ]
    },
    rightChoice: {
      text: 'Devlet müdahalesi',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['ekonomik_mudahale']
    },
    category: EventCategory.STORY,
    weight: 1
  }
];
