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
  }
];

export default medievalCards;
