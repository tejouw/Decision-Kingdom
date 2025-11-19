import { Card, Character, ResourceType, EventCategory, ConditionType, Era } from '../models/types.js';

// Karakterler
const characters: Record<string, Character> = {
  vezir: {
    id: 'vezir',
    name: 'Sadrazam Ahmet',
    title: 'Baş Vezir',
    avatar: '👳'
  },
  general: {
    id: 'general',
    name: 'Paşa Mahmut',
    title: 'Ordu Komutanı',
    avatar: '⚔️'
  },
  hazinedar: {
    id: 'hazinedar',
    name: 'Hazinedar Mustafa',
    title: 'Hazine Bakanı',
    avatar: '💰'
  },
  imam: {
    id: 'imam',
    name: 'Şeyhülislam Ömer',
    title: 'Din İşleri Başkanı',
    avatar: '🕌'
  },
  koy_muhtari: {
    id: 'koy_muhtari',
    name: 'Muhtar Ali',
    title: 'Köy Muhtarı',
    avatar: '👨‍🌾'
  },
  tuccar: {
    id: 'tuccar',
    name: 'Tüccar Hasan',
    title: 'Ticaret Loncası Başkanı',
    avatar: '🏪'
  },
  casus: {
    id: 'casus',
    name: 'Gölge',
    title: 'Saray Casusu',
    avatar: '🕵️'
  },
  hekim: {
    id: 'hekim',
    name: 'Hekim İbrahim',
    title: 'Saray Hekimi',
    avatar: '⚕️'
  },
  sair: {
    id: 'sair',
    name: 'Şair Kemal',
    title: 'Saray Şairi',
    avatar: '📜'
  },
  prenses: {
    id: 'prenses',
    name: 'Prenses Fatma',
    title: 'Sultanın Kızı',
    avatar: '👸'
  }
};

// Ortaçağ kartları
export const medievalCards: Card[] = [
  // ============= VEZİR KARTLARI =============
  {
    id: 'vezir_vergi',
    character: characters.vezir,
    text: 'Sultanım, hazine tükenmek üzere. Halktan ek vergi toplamamızı öneriyorum. Ne dersiniz?',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ],
      relationshipChange: -5
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      relationshipChange: 5
    },
    category: EventCategory.RANDOM,
    weight: 2,
    isRepeatable: true,
    cooldown: 5
  },
  {
    id: 'vezir_festival',
    character: characters.vezir,
    text: 'Halk için büyük bir festival düzenleyelim mi? Moral yükseltir ama maliyetli olur.',
    leftChoice: {
      text: 'Hayır',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Evet',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['festival_duzenlendi']
    },
    category: EventCategory.RANDOM,
    weight: 1,
    isRepeatable: true,
    cooldown: 10
  },
  {
    id: 'vezir_elci',
    character: characters.vezir,
    text: 'Komşu krallıktan elçi geldi. Barış antlaşması imzalamak istiyorlar.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 10 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      setFlags: ['savas_yolu']
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['baris_antlasmasi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'vezir_komplo',
    character: characters.vezir,
    text: 'Sultanım, sarayda bir komplo olduğunu duydum. Soruşturma başlatalım mı?',
    leftChoice: {
      text: 'Görmezden gel',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Soruştur',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      triggeredEvents: ['vezir_komplo_sonuc'],
      setFlags: ['komplo_sorusturma']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'vezir_komplo_sonuc',
    character: characters.vezir,
    text: 'Soruşturma tamamlandı. Bazı saray mensupları suçlu bulundu. Ne yapacağız?',
    leftChoice: {
      text: 'Affet',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Cezalandır',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      relationshipChange: 10
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'komplo_sorusturma' }
    ]
  },
  {
    id: 'vezir_reform',
    character: characters.vezir,
    text: 'Yeni bir yönetim reformu öneriyorum. Daha verimli olacağız ama halk direniş gösterebilir.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.GOLD, min: 8, max: 12 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['reform_yapildi']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.TURN_ABOVE, value: 10 }
    ]
  },

  // ============= GENERAL KARTLARI =============
  {
    id: 'general_savunma',
    character: characters.general,
    text: 'Sultanım, sınırlarda hareketlilik var. Orduyu güçlendirmemiz gerekiyor.',
    leftChoice: {
      text: 'Şimdi değil',
      effects: [
        { resource: ResourceType.MILITARY, min: -8, max: -5 },
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Orduyu güçlendir',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -12, max: -8 }
      ],
      relationshipChange: 10
    },
    category: EventCategory.RANDOM,
    weight: 2,
    isRepeatable: true,
    cooldown: 5
  },
  {
    id: 'general_sefer',
    character: characters.general,
    text: 'Komşu topraklara sefer düzenleyebiliriz. Zafer kesin ama kayıplar olacak.',
    leftChoice: {
      text: 'Barışı koru',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 },
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Sefere çık',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 25 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      triggeredEvents: ['general_sefer_sonuc'],
      setFlags: ['sefer_basladi'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.MILITARY, value: 40 }
    ]
  },
  {
    id: 'general_sefer_sonuc',
    character: characters.general,
    text: 'Sefer başarıyla tamamlandı! Ganimetler paylaşılacak. Nasıl dağıtmalıyız?',
    leftChoice: {
      text: 'Askerlere ver',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Hazineye aktar',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'sefer_basladi' }
    ]
  },
  {
    id: 'general_egitim',
    character: characters.general,
    text: 'Askerlere yeni eğitim programı başlatmak istiyorum. İzin verir misiniz?',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      relationshipChange: -5
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.MILITARY, min: 8, max: 12 },
        { resource: ResourceType.GOLD, min: -8, max: -5 }
      ],
      relationshipChange: 5
    },
    category: EventCategory.CHARACTER,
    weight: 1,
    isRepeatable: true,
    cooldown: 8
  },
  {
    id: 'general_firar',
    character: characters.general,
    text: 'Bazı askerler firar etti. Onları yakalayıp örnek ceza mı verelim?',
    leftChoice: {
      text: 'Affet',
      effects: [
        { resource: ResourceType.MILITARY, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Cezalandır',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_BELOW, resource: ResourceType.MILITARY, value: 40 }
    ]
  },
  {
    id: 'general_silah',
    character: characters.general,
    text: 'Yeni silahlar geliştirdik. Test etmek için izninizi istiyorum.',
    leftChoice: {
      text: 'Tehlikeli',
      effects: [
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Test et',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      setFlags: ['yeni_silahlar']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'general', value: 3 }
    ],
    memoryText: 'Sizi tanıyorum Sultanım, biliyorsunuz askeri işlerden anlarım.'
  },

  // ============= HAZİNEDAR KARTLARI =============
  {
    id: 'hazinedar_ticaret',
    character: characters.hazinedar,
    text: 'Yeni ticaret yolları açabiliriz. Başlangıç yatırımı gerekiyor.',
    leftChoice: {
      text: 'Çok riskli',
      effects: [
        { resource: ResourceType.GOLD, min: -3, max: -1 }
      ],
      relationshipChange: -5
    },
    rightChoice: {
      text: 'Yatırım yap',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      triggeredEvents: ['hazinedar_ticaret_sonuc'],
      setFlags: ['ticaret_yolu_acildi'],
      relationshipChange: 5
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'hazinedar_ticaret_sonuc',
    character: characters.hazinedar,
    text: 'Ticaret yolları başarılı oldu! Gelirlerimiz artıyor.',
    leftChoice: {
      text: 'Güzel',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 }
      ]
    },
    rightChoice: {
      text: 'Daha fazla yatırım',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'ticaret_yolu_acildi' }
    ]
  },
  {
    id: 'hazinedar_maden',
    character: characters.hazinedar,
    text: 'Dağlarda altın madeni keşfedildi. İşletmeye açalım mı?',
    leftChoice: {
      text: 'Hayır',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Evet',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['maden_acildi']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'hazinedar_borc',
    character: characters.hazinedar,
    text: 'Komşu krallıktan borç isteyebiliriz. Faizi yüksek ama acil para lazım.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Borç al',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 }
      ],
      triggeredEvents: ['hazinedar_borc_odeme'],
      setFlags: ['borc_alindi']
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_BELOW, resource: ResourceType.GOLD, value: 30 }
    ]
  },
  {
    id: 'hazinedar_borc_odeme',
    character: characters.hazinedar,
    text: 'Borç ödeme zamanı geldi. Faizle birlikte ödememiz gerekiyor.',
    leftChoice: {
      text: 'Öde',
      effects: [
        { resource: ResourceType.GOLD, min: -35, max: -25 }
      ],
      removeFlags: ['borc_alindi']
    },
    rightChoice: {
      text: 'Ertele',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 }
      ]
    },
    priority: 15,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'borc_alindi' },
      { type: ConditionType.TURN_ABOVE, value: 5 }
    ]
  },
  {
    id: 'hazinedar_yolsuzluk',
    character: characters.hazinedar,
    text: 'Hazinede yolsuzluk tespit ettim. Suçluları yakaladık. Ne yapacağız?',
    leftChoice: {
      text: 'Ört bas',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'İfşa et',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'hazinedar', value: 2 }
    ]
  },

  // ============= İMAM KARTLARI =============
  {
    id: 'imam_cami',
    character: characters.imam,
    text: 'Sultanım, yeni bir cami inşa etmemiz halkın maneviyatını yükseltir.',
    leftChoice: {
      text: 'Bütçe yok',
      effects: [
        { resource: ResourceType.FAITH, min: -8, max: -5 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'İnşa et',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: 12, max: 18 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ],
      setFlags: ['cami_insa_edildi'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'imam_yardim',
    character: characters.imam,
    text: 'Fakirlere yardım dağıtımı yapmak istiyoruz. Onay verir misiniz?',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.FAITH, min: 8, max: 12 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      relationshipChange: 10
    },
    category: EventCategory.RANDOM,
    weight: 2,
    isRepeatable: true,
    cooldown: 6
  },
  {
    id: 'imam_sapkin',
    character: characters.imam,
    text: 'Bazı gruplar sapkın fikirler yayıyor. Müdahale edelim mi?',
    leftChoice: {
      text: 'Hoşgörü göster',
      effects: [
        { resource: ResourceType.FAITH, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['hosgoru_politikasi']
    },
    rightChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['sapkinlik_yasaklandi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'imam_mucize',
    character: characters.imam,
    text: 'Sultanım, halk arasında bir mucize söylentisi yayılıyor. Bu inanç güçlendiriyor.',
    leftChoice: {
      text: 'Araştır',
      effects: [
        { resource: ResourceType.FAITH, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Destekle',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'imam_medrese',
    character: characters.imam,
    text: 'Yeni bir medrese açmak istiyoruz. Eğitim ve inanç güçlenecek.',
    leftChoice: {
      text: 'Pahalı',
      effects: [
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.GOLD, min: -12, max: -8 },
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['medrese_acildi']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 40 }
    ]
  },

  // ============= KÖY MUHTARI KARTLARI =============
  {
    id: 'muhtar_kuraklik',
    character: characters.koy_muhtari,
    text: 'Sultanım, kuraklık var. Köylüler yardım bekliyor.',
    leftChoice: {
      text: 'Beklesinler',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Yardım gönder',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      relationshipChange: 15
    },
    category: EventCategory.RANDOM,
    weight: 2,
    isRepeatable: true,
    cooldown: 8
  },
  {
    id: 'muhtar_hasat',
    character: characters.koy_muhtari,
    text: 'Hasat çok bereketli oldu! Fazlasını nasıl değerlendirelim?',
    leftChoice: {
      text: 'Halka dağıt',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Hazineye aktar',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'muhtar_isyan',
    character: characters.koy_muhtari,
    text: 'Sultanım, bazı köylüler isyan ediyor! Ne yapalım?',
    leftChoice: {
      text: 'Dinle',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ]
    },
    rightChoice: {
      text: 'Bastır',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_BELOW, resource: ResourceType.HAPPINESS, value: 35 }
    ],
    priority: 3
  },
  {
    id: 'muhtar_goc',
    character: characters.koy_muhtari,
    text: 'Köylerden şehirlere göç artıyor. Önlem alalım mı?',
    leftChoice: {
      text: 'Doğal süreç',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Teşvik ver',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'muhtar_yol',
    character: characters.koy_muhtari,
    text: 'Köylere yeni yol yapılması gerekiyor. Ticaret ve ulaşım kolaylaşır.',
    leftChoice: {
      text: 'Bütçe yok',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Yap',
      effects: [
        { resource: ResourceType.GOLD, min: -12, max: -8 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 },
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ],
      setFlags: ['yol_yapildi']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'koy_muhtari', value: 2 }
    ]
  },

  // ============= TÜCCAR KARTLARI =============
  {
    id: 'tuccar_kervan',
    character: characters.tuccar,
    text: 'Uzak diyarlardan kervan geldi. Pahalı mallar getirmişler.',
    leftChoice: {
      text: 'Geri çevir',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ],
      relationshipChange: -5
    },
    rightChoice: {
      text: 'Satın al',
      effects: [
        { resource: ResourceType.GOLD, min: -12, max: -8 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      relationshipChange: 5
    },
    category: EventCategory.RANDOM,
    weight: 2,
    isRepeatable: true,
    cooldown: 5
  },
  {
    id: 'tuccar_lonca',
    character: characters.tuccar,
    text: 'Lonca vergileri çok yüksek diyor tüccarlar. İndirim yapalım mı?',
    leftChoice: {
      text: 'Hayır',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'İndir',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      relationshipChange: 10
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'tuccar_karaborsaci',
    character: characters.tuccar,
    text: 'Karaborsacılar yakalandı. Cezalandıralım mı yoksa bırakalım mı?',
    leftChoice: {
      text: 'Bırak',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Cezalandır',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: -3, max: -1 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'tuccar_ipek',
    character: characters.tuccar,
    text: 'İpek Yolu\'ndan özel bir teklif var. Yüksek risk, yüksek kazanç.',
    leftChoice: {
      text: 'Riskli',
      effects: [
        { resource: ResourceType.GOLD, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 }
      ],
      triggeredEvents: ['tuccar_ipek_sonuc']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_RELATIONSHIP_ABOVE, characterId: 'tuccar', value: 20 }
    ]
  },
  {
    id: 'tuccar_ipek_sonuc',
    character: characters.tuccar,
    text: 'İpek Yolu ticareti büyük başarı sağladı! Kazançlarımız muazzam.',
    leftChoice: {
      text: 'Harika',
      effects: [
        { resource: ResourceType.GOLD, min: 25, max: 35 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Devam et',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 }
      ],
      setFlags: ['ipek_yolu_aktif']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },

  // ============= CASUS KARTLARI =============
  {
    id: 'casus_bilgi',
    character: characters.casus,
    text: 'Sultanım, komşu krallık hakkında önemli bilgiler edindim. Dinlemek ister misiniz?',
    leftChoice: {
      text: 'İlgilenmem',
      effects: [
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Anlat',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.MILITARY, min: 8, max: 12 }
      ],
      setFlags: ['casus_bilgisi']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'casus_suikast',
    character: characters.casus,
    text: 'Size karşı bir suikast planı tespit ettik. Hemen müdahale edelim mi?',
    leftChoice: {
      text: 'İzle',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Yakala',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['suikast_onlendi']
    },
    category: EventCategory.STORY,
    priority: 5,
    conditions: [
      { type: ConditionType.TURN_ABOVE, value: 15 }
    ]
  },
  {
    id: 'casus_ihanetkontrol',
    character: characters.casus,
    text: 'Vezirin sadakatinden şüpheleniyorum. Onu takip etmemi ister misiniz?',
    leftChoice: {
      text: 'Güveniyorum',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Takip et',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      triggeredEvents: ['casus_vezir_rapor'],
      setFlags: ['vezir_takibi']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'casus', value: 2 }
    ]
  },
  {
    id: 'casus_vezir_rapor',
    character: characters.casus,
    text: 'Vezir tamamen sadık görünüyor. Ama şüpheli bağlantıları var.',
    leftChoice: {
      text: 'Kapat dosyayı',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Derinleştir',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    priority: 8,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'vezir_takibi' }
    ]
  },

  // ============= HEKİM KARTLARI =============
  {
    id: 'hekim_salgin',
    character: characters.hekim,
    text: 'Sultanım, şehirde hastalık yayılıyor. Karantina uygulamamız gerekiyor.',
    leftChoice: {
      text: 'Gereksiz',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: -8, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Uygula',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.MILITARY, min: 3, max: 5 }
      ],
      setFlags: ['karantina_uygulandi']
    },
    category: EventCategory.RANDOM,
    priority: 5,
    weight: 1
  },
  {
    id: 'hekim_ilac',
    character: characters.hekim,
    text: 'Yeni bir ilaç geliştirdim. Test etmek için izninizi istiyorum.',
    leftChoice: {
      text: 'Riskli',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'İzin ver',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ],
      setFlags: ['yeni_ilac']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'hekim_saglik',
    character: characters.hekim,
    text: 'Halk sağlığı için temiz su sistemi kurmamız lazım. Pahalı olacak.',
    leftChoice: {
      text: 'Sonra',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Kur',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['temiz_su']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 50 }
    ]
  },

  // ============= ŞAİR KARTLARI =============
  {
    id: 'sair_methiye',
    character: characters.sair,
    text: 'Sultanım, sizin için bir methiye yazdım. Halka okutmamı ister misiniz?',
    leftChoice: {
      text: 'Gerek yok',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Okut',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      relationshipChange: 10
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'sair_hiciv',
    character: characters.sair,
    text: 'Bir şair sizin hakkınızda hiciv yazdı. Halk arasında yayılıyor.',
    leftChoice: {
      text: 'Görmezden gel',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 },
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Cezalandır',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.FAITH, min: -8, max: -5 }
      ]
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'sair_tarih',
    character: characters.sair,
    text: 'Saltanatınızın tarihini yazmak istiyorum. Gelecek nesiller okusun.',
    leftChoice: {
      text: 'Başka zaman',
      effects: []
    },
    rightChoice: {
      text: 'Yaz',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.FAITH, min: 8, max: 12 }
      ],
      setFlags: ['tarih_yaziliyor']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.TURN_ABOVE, value: 20 }
    ]
  },

  // ============= PRENSES KARTLARI =============
  {
    id: 'prenses_evlilik',
    character: characters.prenses,
    text: 'Babacığım, komşu prens benimle evlenmek istiyor. Bu ittifak güçlü olur.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      relationshipChange: -20
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['prenses_evlendi'],
      relationshipChange: 20
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.TURN_ABOVE, value: 15 }
    ]
  },
  {
    id: 'prenses_hayir',
    character: characters.prenses,
    text: 'Fakirler için bir hayır kurumu kurmak istiyorum. Destek verir misiniz?',
    leftChoice: {
      text: 'Pahalı',
      effects: [
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Destekle',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      relationshipChange: 10
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= ÖZEL OLAYLAR =============
  {
    id: 'veba',
    character: characters.vezir,
    text: 'Sultanım, şehirde veba salgını başladı! Acil önlem almalıyız.',
    leftChoice: {
      text: 'Karantina',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 },
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Dua et',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 }
      ]
    },
    priority: 8,
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'deprem',
    character: characters.koy_muhtari,
    text: 'Büyük deprem oldu! Birçok ev yıkıldı, halk sokakta.',
    leftChoice: {
      text: 'Yardım gönder',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Kadere bırak',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ]
    },
    priority: 10,
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'hazine_bulundu',
    character: characters.hazinedar,
    text: 'Eski bir definenin haritasını bulduk! Kazı yapmalıyız.',
    leftChoice: {
      text: 'Boş ver',
      effects: []
    },
    rightChoice: {
      text: 'Kaz',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      triggeredEvents: ['hazine_sonuc']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'hazine_sonuc',
    character: characters.hazinedar,
    text: 'Define gerçekmiş! Büyük bir hazine bulduk!',
    leftChoice: {
      text: 'Muhteşem',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Dağıt',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    priority: 15,
    category: EventCategory.CHAIN
  },
  {
    id: 'yabanci_elci',
    character: characters.vezir,
    text: 'Uzak diyarlardan bir elçi geldi. Garip hediyeler ve teklifler getirmiş.',
    leftChoice: {
      text: 'Geri gönder',
      effects: [
        { resource: ResourceType.MILITARY, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['yabanci_iliski']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'ejderha_soylentisi',
    character: characters.general,
    text: 'Sultanım, dağlarda bir ejderha görüldüğü söyleniyor. Halk korkuyor.',
    leftChoice: {
      text: 'Hurafe',
      effects: [
        { resource: ResourceType.FAITH, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Keşif gönder',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      triggeredEvents: ['ejderha_sonuc']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'ejderha_sonuc',
    character: characters.general,
    text: 'Keşif tamamlandı! Ejderha değil, sadece büyük bir mağara ayısıymış.',
    leftChoice: {
      text: 'Öldür',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Barışıkla',
      effects: [
        { resource: ResourceType.FAITH, min: 8, max: 12 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    },
    priority: 8,
    category: EventCategory.CHAIN
  },
  {
    id: 'saray_yangin',
    character: characters.vezir,
    text: 'Sarayda yangın çıktı! Hızlı müdahale gerekiyor.',
    leftChoice: {
      text: 'Su getir',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Yık engelle',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    priority: 10,
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'festival_sonrasi',
    character: characters.vezir,
    text: 'Festival çok başarılıydı! Halk çok memnun. Geleneksel yapalım mı?',
    leftChoice: {
      text: 'Bir kerelik',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Her yıl',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      setFlags: ['yillik_festival']
    },
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'festival_duzenlendi' }
    ],
    priority: 5
  },

  // ============= VEZİR OLAY ZİNCİRİ =============
  {
    id: 'vezir_guc_toplama',
    character: characters.vezir,
    text: 'Sultanım, saraya daha fazla kontrol getirmemiz lazım. Valilerin yetkilerini kısıtlayalım mı?',
    leftChoice: {
      text: 'Hayır',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Evet',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 }
      ],
      setFlags: ['merkezi_yonetim'],
      triggeredEvents: ['vezir_valiler_tepki'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'vezir', value: 3 }
    ]
  },
  {
    id: 'vezir_valiler_tepki',
    character: characters.vezir,
    text: 'Valiler yeni düzene karşı çıkıyor. Bazıları isyan hazırlığında olabilir.',
    leftChoice: {
      text: 'Yumuşat',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      removeFlags: ['merkezi_yonetim']
    },
    rightChoice: {
      text: 'Bastır',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['valiler_bastirildi']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },
  {
    id: 'vezir_taht_plani',
    character: characters.vezir,
    text: 'Sultanım, geleceği düşünmeliyiz. Bir varis belirlememiz gerekiyor.',
    leftChoice: {
      text: 'Bekle',
      effects: [
        { resource: ResourceType.FAITH, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Belirle',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['varis_belirlendi']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.TURN_ABOVE, value: 25 },
      { type: ConditionType.CHARACTER_RELATIONSHIP_ABOVE, characterId: 'vezir', value: 30 }
    ]
  },

  // ============= GENERAL OLAY ZİNCİRİ =============
  {
    id: 'general_ordu_modernizasyon',
    character: characters.general,
    text: 'Ordunun modernizasyonu şart! Yeni taktikler ve silahlar için bütçe istiyorum.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.MILITARY, min: -10, max: -5 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 15, max: 20 }
      ],
      setFlags: ['ordu_modern'],
      triggeredEvents: ['general_modernizasyon_sonuc'],
      relationshipChange: 20
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 50 },
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'general', value: 4 }
    ]
  },
  {
    id: 'general_modernizasyon_sonuc',
    character: characters.general,
    text: 'Modernizasyon başarılı! Ordumuz artık çok daha güçlü. İmparatorluk kurabiliriz!',
    leftChoice: {
      text: 'Savunma odaklı',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Fetih planla',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      setFlags: ['fetih_plani']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },
  {
    id: 'general_darbe_teklifi',
    character: characters.general,
    text: 'Sultanım, bazı komutanlar... güç için size sadık olmayabilir. Önlem alalım mı?',
    leftChoice: {
      text: 'Güveniyorum',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Temizlik yap',
      effects: [
        { resource: ResourceType.MILITARY, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['ordu_temizligi']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.MILITARY, value: 70 },
      { type: ConditionType.TURN_ABOVE, value: 20 }
    ],
    priority: 8
  },
  {
    id: 'general_zafer_kutlamasi',
    character: characters.general,
    text: 'Büyük zaferimizi kutlamalıyız! Askerlere ödül ve halka şölen!',
    leftChoice: {
      text: 'Mütevazı ol',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Kutla',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 }
      ]
    },
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'sefer_basladi' }
    ],
    priority: 7
  },

  // ============= HAZİNEDAR OLAY ZİNCİRİ =============
  {
    id: 'hazinedar_ekonomi_reform',
    character: characters.hazinedar,
    text: 'Ekonomik reform zamanı! Para birimini değiştirelim ve ticaret vergileri yeniden düzenleyelim.',
    leftChoice: {
      text: 'Riskli',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Başlat',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 }
      ],
      setFlags: ['ekonomi_reform'],
      triggeredEvents: ['hazinedar_reform_sonuc'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'hazinedar', value: 4 },
      { type: ConditionType.TURN_ABOVE, value: 15 }
    ]
  },
  {
    id: 'hazinedar_reform_sonuc',
    character: characters.hazinedar,
    text: 'Reform meyvelerini veriyor! Ticaret canlanıyor, hazine doluyor.',
    leftChoice: {
      text: 'Yeterli',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Genişlet',
      effects: [
        { resource: ResourceType.GOLD, min: 25, max: 35 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['ticaret_imparatorlugu']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },
  {
    id: 'hazinedar_vergi_isyani',
    character: characters.hazinedar,
    text: 'Vergi toplayıcılarına saldırılar başladı! Halk yüksek vergilerden şikayetçi.',
    leftChoice: {
      text: 'Vergi indir',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ]
    },
    rightChoice: {
      text: 'Askeri gönder',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_BELOW, resource: ResourceType.HAPPINESS, value: 40 },
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 60 }
    ],
    priority: 7
  },
  {
    id: 'hazinedar_yatirim_firsat',
    character: characters.hazinedar,
    text: 'Denizaşırı ticaret için büyük bir fırsat! Riskli ama kazanç çok yüksek olabilir.',
    leftChoice: {
      text: 'Geç',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Yatır',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 }
      ],
      triggeredEvents: ['hazinedar_yatirim_sonuc']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_RELATIONSHIP_ABOVE, characterId: 'hazinedar', value: 25 }
    ]
  },
  {
    id: 'hazinedar_yatirim_sonuc',
    character: characters.hazinedar,
    text: 'Yatırım büyük başarı sağladı! Gemiler altınla döndü!',
    leftChoice: {
      text: 'Mükemmel',
      effects: [
        { resource: ResourceType.GOLD, min: 40, max: 50 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ]
    },
    rightChoice: {
      text: 'Tekrarla',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 }
      ],
      setFlags: ['deniz_ticareti']
    },
    priority: 12,
    category: EventCategory.CHAIN
  },

  // ============= İMAM OLAY ZİNCİRİ =============
  {
    id: 'imam_din_devlet',
    character: characters.imam,
    text: 'Sultanım, dinin devlet işlerinde daha fazla söz sahibi olması gerekiyor.',
    leftChoice: {
      text: 'Ayır',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['laik_yonetim']
    },
    rightChoice: {
      text: 'Birleştir',
      effects: [
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['dini_yonetim'],
      triggeredEvents: ['imam_dini_kanun']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'imam', value: 4 },
      { type: ConditionType.TURN_ABOVE, value: 20 }
    ]
  },
  {
    id: 'imam_dini_kanun',
    character: characters.imam,
    text: 'Yeni dini kanunlar hazırladık. Onayınızı bekliyoruz.',
    leftChoice: {
      text: 'Yumuşat',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Katı uygula',
      effects: [
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      setFlags: ['seri_hukuk']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },
  {
    id: 'imam_kutsal_savas',
    character: characters.imam,
    text: 'Kafirler kutsal topraklarımızı tehdit ediyor! Kutsal savaş ilan etmeliyiz!',
    leftChoice: {
      text: 'Diplomasi',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.GOLD, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Cihat!',
      effects: [
        { resource: ResourceType.FAITH, min: 20, max: 25 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -15, max: -10 }
      ],
      setFlags: ['kutsal_savas']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.FAITH, value: 60 },
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.MILITARY, value: 50 }
    ],
    priority: 8
  },
  {
    id: 'imam_alim_davet',
    character: characters.imam,
    text: 'Uzak diyarlardan ünlü alimler geldi. Onları ağırlayıp ilim meclisi kuralım mı?',
    leftChoice: {
      text: 'Gerek yok',
      effects: [
        { resource: ResourceType.FAITH, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Ağırla',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 12, max: 18 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['ilim_meclisi']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'medrese_acildi' }
    ]
  },

  // ============= TÜCCAR OLAY ZİNCİRİ =============
  {
    id: 'tuccar_lonca_birlik',
    character: characters.tuccar,
    text: 'Tüm loncaları tek çatı altında birleştirmek istiyoruz. Ticaret daha koordineli olacak.',
    leftChoice: {
      text: 'Bağımsız kalsın',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Birleştir',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['lonca_birligi'],
      triggeredEvents: ['tuccar_lonca_guc']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'tuccar', value: 4 }
    ]
  },
  {
    id: 'tuccar_lonca_guc',
    character: characters.tuccar,
    text: 'Lonca birliği çok güçlendi. Artık devlet işlerinde de söz istiyorlar.',
    leftChoice: {
      text: 'Sınırla',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['tuccar_oligarsi']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },
  {
    id: 'tuccar_pazar_tekel',
    character: characters.tuccar,
    text: 'Bazı tüccarlar tekel oluşturmuş. Fiyatları kontrol ediyorlar.',
    leftChoice: {
      text: 'Serbest bırak',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Kır',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 65 }
    ]
  },
  {
    id: 'tuccar_dis_ticaret',
    character: characters.tuccar,
    text: 'Yeni ticaret anlaşmaları imzalamak istiyoruz. Batı krallıklarıyla mı, Doğu hanlıklarıyla mı?',
    leftChoice: {
      text: 'Batı',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['bati_ticareti']
    },
    rightChoice: {
      text: 'Doğu',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ],
      setFlags: ['dogu_ticareti']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'ticaret_yolu_acildi' }
    ]
  },

  // ============= CASUS OLAY ZİNCİRİ =============
  {
    id: 'casus_ag_kurma',
    character: characters.casus,
    text: 'Sultanım, tüm krallıklarda casus ağı kurabiliriz. Hiçbir şey gizli kalmaz.',
    leftChoice: {
      text: 'Pahalı',
      effects: [
        { resource: ResourceType.MILITARY, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Kur',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 }
      ],
      setFlags: ['casus_agi'],
      triggeredEvents: ['casus_ag_rapor']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'casus', value: 3 }
    ]
  },
  {
    id: 'casus_ag_rapor',
    character: characters.casus,
    text: 'Casus ağımız mükemmel çalışıyor. Düşman planlarını önceden biliyoruz.',
    leftChoice: {
      text: 'Savunma',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Sabotaj',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['aktif_sabotaj']
    },
    priority: 10,
    category: EventCategory.CHAIN
  },
  {
    id: 'casus_ic_tehdit',
    character: characters.casus,
    text: 'Sarayda ciddi bir iç tehdit var. Birisi sizi zehirlemeye çalışıyor.',
    leftChoice: {
      text: 'Dikkat et',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Bul',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ],
      triggeredEvents: ['casus_zehir_sonuc']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'casus_agi' },
      { type: ConditionType.TURN_ABOVE, value: 25 }
    ],
    priority: 8
  },
  {
    id: 'casus_zehir_sonuc',
    character: characters.casus,
    text: 'Suçluyu yakaladık! Komşu kralın adamıymış. Ne yapalım?',
    leftChoice: {
      text: 'İade et',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'İdam et',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['dusman_ilan']
    },
    priority: 12,
    category: EventCategory.CHAIN
  },

  // ============= HEKİM OLAY ZİNCİRİ =============
  {
    id: 'hekim_hastane',
    character: characters.hekim,
    text: 'Büyük bir hastane kurmak istiyorum. Tüm halk tedavi görebilir.',
    leftChoice: {
      text: 'Çok pahalı',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Kur',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 8, max: 12 }
      ],
      setFlags: ['hastane_kuruldu']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'hekim', value: 3 },
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 50 }
    ]
  },
  {
    id: 'hekim_eczane',
    character: characters.hekim,
    text: 'Şifalı bitki bahçesi ve eczane kurmak istiyorum. Kendi ilaçlarımızı üretebiliriz.',
    leftChoice: {
      text: 'Gerek yok',
      effects: [
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Kur',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ],
      setFlags: ['eczane_kuruldu']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'hastane_kuruldu' }
    ]
  },
  {
    id: 'hekim_yasli_sultan',
    character: characters.hekim,
    text: 'Sultanım, sağlığınız için endişeleniyorum. Dinlenmeniz gerekiyor.',
    leftChoice: {
      text: 'Çalışmalıyım',
      effects: [
        { resource: ResourceType.MILITARY, min: 3, max: 5 },
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Dinlen',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: 3, max: 5 }
      ]
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.TURN_ABOVE, value: 40 }
    ]
  },

  // ============= ŞAİR OLAY ZİNCİRİ =============
  {
    id: 'sair_destan',
    character: characters.sair,
    text: 'Saltanatınız için bir destan yazmak istiyorum. Nesiller boyu anlatılsın.',
    leftChoice: {
      text: 'Mütevazı kal',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Yaz',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['destan_yazildi']
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'sair', value: 3 },
      { type: ConditionType.TURN_ABOVE, value: 30 }
    ]
  },
  {
    id: 'sair_propaganda',
    character: characters.sair,
    text: 'Düşmanlarınız hakkında aşağılayıcı şiirler yazabilirim. Moral bozar.',
    leftChoice: {
      text: 'Onurlu ol',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Yaz',
      effects: [
        { resource: ResourceType.MILITARY, min: 8, max: 12 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'sefer_basladi' }
    ]
  },
  {
    id: 'sair_kultur_festivali',
    character: characters.sair,
    text: 'Büyük bir kültür ve sanat festivali düzenleyelim! Tüm sanatçılar gelsin.',
    leftChoice: {
      text: 'Gereksiz',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Düzenle',
      effects: [
        { resource: ResourceType.GOLD, min: -12, max: -8 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['kultur_merkezi']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'destan_yazildi' }
    ]
  },

  // ============= PRENSES OLAY ZİNCİRİ =============
  {
    id: 'prenses_egitim',
    character: characters.prenses,
    text: 'Babacığım, daha fazla eğitim almak istiyorum. Yabancı hocalar getirtin.',
    leftChoice: {
      text: 'Yeter',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Getirt',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'prenses', value: 2 }
    ]
  },
  {
    id: 'prenses_asi',
    character: characters.prenses,
    text: 'Babacığım, bu evlilik istemiyorum. Aşık olduğum biri var - bir şövalye.',
    leftChoice: {
      text: 'Anlayış göster',
      effects: [
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      relationshipChange: 30,
      setFlags: ['prenses_ask']
    },
    rightChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      relationshipChange: -30
    },
    category: EventCategory.STORY,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'prenses_evlendi' }
    ],
    priority: 8
  },
  {
    id: 'prenses_yardim_dernegi',
    character: characters.prenses,
    text: 'Yoksul kadınlar için eğitim ve meslek kazandırma programı başlatmak istiyorum.',
    leftChoice: {
      text: 'Uygun değil',
      effects: [
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Destekle',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 12, max: 18 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      relationshipChange: 15,
      setFlags: ['kadin_egitimi']
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_RELATIONSHIP_ABOVE, characterId: 'prenses', value: 20 }
    ]
  },

  // ============= EK OLAYLAR =============
  {
    id: 'kuraklık_devam',
    character: characters.koy_muhtari,
    text: 'Kuraklık devam ediyor! Su kaynakları tükeniyor, halk göç etmeye başladı.',
    leftChoice: {
      text: 'Kanal aç',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['sulama_sistemi']
    },
    rightChoice: {
      text: 'Yağmur duası',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_BELOW, resource: ResourceType.HAPPINESS, value: 40 }
    ],
    priority: 6
  },
  {
    id: 'sinir_catismasi',
    character: characters.general,
    text: 'Sınırda komşu güçlerle çatışma çıktı! Tırmanma riski var.',
    leftChoice: {
      text: 'Geri çekil',
      effects: [
        { resource: ResourceType.MILITARY, min: -8, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Mukabele',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -8, max: -5 }
      ],
      setFlags: ['sinir_gerginlik']
    },
    category: EventCategory.RANDOM,
    priority: 7
  },
  {
    id: 'goçebe_saldiri',
    character: characters.general,
    text: 'Göçebe kabileler köylere saldırıyor! Acil müdahale gerekiyor.',
    leftChoice: {
      text: 'Haraç ver',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Püskürt',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    category: EventCategory.RANDOM,
    priority: 7,
    weight: 1
  },
  {
    id: 'halk_sikayet',
    character: characters.koy_muhtari,
    text: 'Halk adalet arıyor! Zenginler ve fakirler arasında uçurum büyüyor.',
    leftChoice: {
      text: 'Dinle',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Sustur',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.RESOURCE_ABOVE, resource: ResourceType.GOLD, value: 70 },
      { type: ConditionType.RESOURCE_BELOW, resource: ResourceType.HAPPINESS, value: 50 }
    ]
  },
  {
    id: 'yabanci_din',
    character: characters.imam,
    text: 'Yabancı misyonerler halka farklı din öğretiyor. Ne yapalım?',
    leftChoice: {
      text: 'Hoşgörü',
      effects: [
        { resource: ResourceType.FAITH, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'yabanci_iliski' }
    ]
  },
  {
    id: 'saray_entrika',
    character: characters.casus,
    text: 'Sarayda ciddi bir entrika var. Vezirin ve generalin arası bozuk.',
    leftChoice: {
      text: 'Arabulucu',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Kullan',
      effects: [
        { resource: ResourceType.MILITARY, min: 8, max: 12 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'vezir', value: 3 },
      { type: ConditionType.CHARACTER_INTERACTION, characterId: 'general', value: 3 }
    ]
  },
  {
    id: 'zengin_bagiş',
    character: characters.hazinedar,
    text: 'Zengin bir tüccar sarayı onurlandırmak istiyor. Büyük bağış teklif ediyor.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['tuccar_nufuz']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'astronomi_keşif',
    character: characters.hekim,
    text: 'Astronomlar ilginç bir keşif yaptı! Gökyüzünde yeni bir yıldız.',
    leftChoice: {
      text: 'Uğursuz',
      effects: [
        { resource: ResourceType.FAITH, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Kutla',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'korsan_tehdidi',
    character: characters.tuccar,
    text: 'Korsanlar ticaret gemilerimizi vuruyor! Deniz güvenliği sağlanmalı.',
    leftChoice: {
      text: 'Haraç öde',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Donanma kur',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 }
      ],
      setFlags: ['donanma_kuruldu']
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'deniz_ticareti' }
    ]
  },
  {
    id: 'salgın_yayılıyor',
    character: characters.hekim,
    text: 'Hastalık başka şehirlere yayılıyor! Karantina genişletilmeli.',
    leftChoice: {
      text: 'Kısıtlı tut',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Genişlet',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 },
        { resource: ResourceType.MILITARY, min: 5, max: 8 }
      ]
    },
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'karantina_uygulandi' }
    ],
    priority: 8
  },
  {
    id: 'hanedan_kavga',
    character: characters.vezir,
    text: 'Hanedan üyeleri arasında miras kavgası çıktı. Taraf tutmanız isteniyor.',
    leftChoice: {
      text: 'Tarafsız kal',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Hakem ol',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    category: EventCategory.RANDOM,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'varis_belirlendi' }
    ]
  },
  {
    id: 'kahramanlik_hikayesi',
    character: characters.sair,
    text: 'Bir askerimiz savaşta kahramanlık gösterdi. Hikayesini yazayım mı?',
    leftChoice: {
      text: 'Gerek yok',
      effects: []
    },
    rightChoice: {
      text: 'Yaz',
      effects: [
        { resource: ResourceType.MILITARY, min: 8, max: 12 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    category: EventCategory.CHARACTER,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'sefer_basladi' }
    ]
  }
];

export default medievalCards;
