import { Card, Character, ResourceType, EventCategory, ConditionType, Era } from '../models/types.js';

// Modern Dönem Karakterleri
const characters: Record<string, Character> = {
  medya_patronu: {
    id: 'medya_patronu',
    name: 'Medya Baronu Murdoch',
    title: 'Medya İmparatorluğu Sahibi',
    avatar: '📺'
  },
  aktivist: {
    id: 'aktivist',
    name: 'Aktivist Maya',
    title: 'İnsan Hakları Savunucusu',
    avatar: '✊'
  },
  tech_ceo: {
    id: 'tech_ceo',
    name: 'CEO Steve',
    title: 'Teknoloji Şirketi Kurucusu',
    avatar: '💻'
  },
  diplomat: {
    id: 'diplomat_modern',
    name: 'Büyükelçi Chen',
    title: 'Dışişleri Danışmanı',
    avatar: '🌐'
  },
  ekonomist: {
    id: 'ekonomist',
    name: 'Prof. Keynes',
    title: 'Ekonomi Danışmanı',
    avatar: '📊'
  },
  general_modern: {
    id: 'general_modern',
    name: 'General Powell',
    title: 'Genelkurmay Başkanı',
    avatar: '🎖️'
  },
  cevreci: {
    id: 'cevreci',
    name: 'Çevreci Greta',
    title: 'İklim Aktivisti',
    avatar: '🌱'
  },
  sosyal_medya: {
    id: 'sosyal_medya',
    name: 'Influencer Kim',
    title: 'Sosyal Medya Fenomeni',
    avatar: '📱'
  },
  bilim_insani: {
    id: 'bilim_insani',
    name: 'Dr. Curie',
    title: 'Araştırma Enstitüsü Müdürü',
    avatar: '🔬'
  },
  sivil_toplum: {
    id: 'sivil_toplum',
    name: 'STK Başkanı Ahmet',
    title: 'Sivil Toplum Temsilcisi',
    avatar: '🤝'
  }
};

// Modern Dönem Kartları
export const modernCards: Card[] = [
  // ============= MEDYA PATRONU KARTLARI =============
  {
    id: 'mod_medya_kanal',
    character: characters.medya_patronu,
    text: 'Yeni bir haber kanalı açmak istiyorum. 7/24 yayın. Kamuoyunu şekillendirmek için güçlü bir araç.',
    leftChoice: {
      text: 'Bağımsız medya korunsun',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      relationshipChange: -15
    },
    rightChoice: {
      text: 'Lisans ver',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['medya_kontrolu'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'mod_medya_propaganda',
    character: characters.medya_patronu,
    text: 'Muhalefeti zayıflatacak bir kampanya düzenleyebilirim. Etkili ama etik değil.',
    leftChoice: {
      text: 'Medya tarafsız kalmalı',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 10 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Kampanyayı yap',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['medya_manipulasyonu']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'mod_medya_sahte_haber',
    character: characters.medya_patronu,
    text: 'Sahte haberler yayılıyor. Doğrulama mekanizması kurmalı mıyız?',
    leftChoice: {
      text: 'Sansür tehlikesi',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Fact-check sistemi kur',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['dogrulama_sistemi']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= AKTİVİST KARTLARI =============
  {
    id: 'mod_aktivist_protesto',
    character: characters.aktivist,
    text: 'Büyük bir barışçıl gösteri düzenliyoruz. Halkın sesini duyurmak istiyoruz.',
    leftChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 }
      ],
      setFlags: ['protesto_yasagi'],
      relationshipChange: -20
    },
    rightChoice: {
      text: 'İzin ver',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ],
      setFlags: ['ifade_ozgurlugu'],
      relationshipChange: 15
    },
    category: EventCategory.CHARACTER,
    weight: 2
  },
  {
    id: 'mod_aktivist_insan_haklari',
    character: characters.aktivist,
    text: 'İnsan hakları ihlalleri belgelendi. Uluslararası baskı artıyor.',
    leftChoice: {
      text: 'İç işlerimiz',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ]
    },
    rightChoice: {
      text: 'Reform yap',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['insan_haklari_reformu']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_aktivist_esitlik',
    character: characters.aktivist,
    text: 'Azınlık hakları için mücadele ediyoruz. Ayrımcılık karşıtı yasa çıkarın.',
    leftChoice: {
      text: 'Toplum hazır değil',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Eşitlik yasası',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['esitlik_yasasi']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= TEKNOLOJİ CEO KARTLARI =============
  {
    id: 'mod_tech_startup',
    character: characters.tech_ceo,
    text: 'Devlet destekli teknoloji merkezi kurmak istiyoruz. Silikon Vadisi gibi.',
    leftChoice: {
      text: 'Özel sektör kendi başına',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ],
      relationshipChange: -10
    },
    rightChoice: {
      text: 'Teknopark kur',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      triggeredEvents: ['mod_tech_boom'],
      setFlags: ['teknopark'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'mod_tech_boom',
    character: characters.tech_ceo,
    text: 'Teknopark patladı! Startup\'lar büyüyor, yatırımlar akıyor!',
    leftChoice: {
      text: 'Regülasyonla kontrol et',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Serbest bırak',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['tech_balonu']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'teknopark' }
    ]
  },
  {
    id: 'mod_tech_gizlilik',
    character: characters.tech_ceo,
    text: 'Kullanıcı verilerini topluyoruz. Çok değerli ama gizlilik endişesi var.',
    leftChoice: {
      text: 'Veri koruma yasası',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['veri_koruma']
    },
    rightChoice: {
      text: 'Serbest veri ekonomisi',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['veri_somurgesi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_tech_yapay_zeka',
    character: characters.tech_ceo,
    text: 'Yapay zeka araştırmalarına yatırım yapalım. Geleceğin teknolojisi.',
    leftChoice: {
      text: 'Riskli',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'AI\'a yatırım yap',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 }
      ],
      setFlags: ['ai_arastirmasi']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= DİPLOMAT KARTLARI =============
  {
    id: 'mod_diplomasi_bm',
    character: characters.diplomat,
    text: 'Birleşmiş Milletler kararına uymalı mıyız? Egemenlik vs uluslararası hukuk.',
    leftChoice: {
      text: 'Ulusal çıkarlar önce',
      effects: [
        { resource: ResourceType.MILITARY, min: 5, max: 10 },
        { resource: ResourceType.GOLD, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Uluslararası hukuka uy',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['bm_uyum']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_diplomasi_ticaret',
    character: characters.diplomat,
    text: 'Serbest ticaret anlaşması teklifi aldık. Ekonomiye iyi ama yerli üretim zarar görebilir.',
    leftChoice: {
      text: 'Korumacılık',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Serbest ticaret',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 25 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['serbest_ticaret']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_diplomasi_multeci',
    character: characters.diplomat,
    text: 'Mülteci krizi kapıda. Binlerce insan sığınma hakkı istiyor.',
    leftChoice: {
      text: 'Sınırları kapat',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'İnsani yardım',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ],
      setFlags: ['multeci_kabul']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= EKONOMİST KARTLARI =============
  {
    id: 'mod_ekonomi_kriz',
    character: characters.ekonomist,
    text: 'Finansal kriz yaklaşıyor. Bütçe açığı büyük, borçlar artıyor.',
    leftChoice: {
      text: 'Kemer sıkma',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 }
      ],
      setFlags: ['kemer_sikma']
    },
    rightChoice: {
      text: 'Teşvik paketi',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['ekonomik_tesvik']
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'mod_ekonomi_enflasyon',
    character: characters.ekonomist,
    text: 'Enflasyon kontrolden çıkıyor. Faiz artıralım mı?',
    leftChoice: {
      text: 'Faizi düşük tut',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Faizi artır',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      setFlags: ['enflasyon_kontrolu']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'mod_ekonomi_vergi',
    character: characters.ekonomist,
    text: 'Zenginlerden daha fazla vergi almalı mıyız? Sosyal adalet vs yatırım ortamı.',
    leftChoice: {
      text: 'Düz vergi',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Artan oranlı vergi',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['progresif_vergi']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= GENERAL KARTLARI =============
  {
    id: 'mod_askeri_nukleer',
    character: characters.general_modern,
    text: 'Nükleer silah programı başlatmalı mıyız? Caydırıcılık için.',
    leftChoice: {
      text: 'Silahsızlanma',
      effects: [
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['nukleer_red']
    },
    rightChoice: {
      text: 'Nükleer program',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.MILITARY, min: 25, max: 30 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['nukleer_guc']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_askeri_teror',
    character: characters.general_modern,
    text: 'Terör saldırısı gerçekleşti! Halk intikam istiyor.',
    leftChoice: {
      text: 'Diplomatik çözüm',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Askeri operasyon',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['teror_savasi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_askeri_drone',
    character: characters.general_modern,
    text: 'Drone teknolojisine yatırım yapalım. İnsansız hava araçları.',
    leftChoice: {
      text: 'Etik sorunlar var',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Drone filosu kur',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.MILITARY, min: 20, max: 25 }
      ],
      setFlags: ['drone_filosu']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= ÇEVRECİ KARTLARI =============
  {
    id: 'mod_cevre_iklim',
    character: characters.cevreci,
    text: 'İklim krizi acil! Karbon emisyonlarını azaltmalıyız yoksa felaket kaçınılmaz.',
    leftChoice: {
      text: 'Ekonomi önce gelir',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      relationshipChange: -20
    },
    rightChoice: {
      text: 'Yeşil dönüşüm',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['yesil_donusum'],
      relationshipChange: 15
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'mod_cevre_yenilenebilir',
    character: characters.cevreci,
    text: 'Güneş ve rüzgar enerjisine geçelim. Fosil yakıtlara son.',
    leftChoice: {
      text: 'Çok pahalı',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Yenilenebilir enerji',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['yenilenebilir_enerji']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },
  {
    id: 'mod_cevre_plastik',
    character: characters.cevreci,
    text: 'Tek kullanımlık plastikleri yasaklayalım. Okyanuslar ölüyor.',
    leftChoice: {
      text: 'Sanayi zarar görür',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: -8, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Plastik yasağı',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['plastik_yasagi']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },

  // ============= SOSYAL MEDYA KARTLARI =============
  {
    id: 'mod_sosyal_viral',
    character: characters.sosyal_medya,
    text: 'Hükümet aleyhine bir video viral oldu. Milyonlarca izlenme. Kriz yönetimi şart.',
    leftChoice: {
      text: 'Sansürle',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['internet_sansuru']
    },
    rightChoice: {
      text: 'Şeffaflıkla yanıtla',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['seffaf_iletisim']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'mod_sosyal_bot',
    character: characters.sosyal_medya,
    text: 'Sosyal medya botları kamuoyunu manipüle ediyor. Yabancı müdahale şüphesi.',
    leftChoice: {
      text: 'Görmezden gel',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Siber güvenlik önlemi',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.MILITARY, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ],
      setFlags: ['siber_guvenlik']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_sosyal_influencer',
    character: characters.sosyal_medya,
    text: 'Hükümet kampanyasında bizi kullanın! Gençlere ulaşırız.',
    leftChoice: {
      text: 'Geleneksel medya yeter',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'İşbirliği yap',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['influencer_kampanya']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= BİLİM İNSANI KARTLARI =============
  {
    id: 'mod_bilim_gen',
    character: characters.bilim_insani,
    text: 'Gen düzenleme teknolojisi hazır. Hastalıkları önleyebiliriz ama etik sorular var.',
    leftChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Kontrollü izin',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['gen_teknolojisi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_bilim_uzay',
    character: characters.bilim_insani,
    text: 'Uzay programı başlatalım. Mars\'a giden ilk ülke olalım.',
    leftChoice: {
      text: 'Dünya sorunları önce',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Uzay programı',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: 10, max: 15 }
      ],
      setFlags: ['uzay_programi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_bilim_pandemi',
    character: characters.bilim_insani,
    text: 'Pandemi riski yüksek. Aşı araştırmalarına acil yatırım gerekiyor.',
    leftChoice: {
      text: 'Risk abartılıyor',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ],
      setFlags: ['pandemi_ihmali']
    },
    rightChoice: {
      text: 'Aşı araştırması',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ],
      setFlags: ['asi_hazirlik']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= SİVİL TOPLUM KARTLARI =============
  {
    id: 'mod_sivil_demokrasi',
    character: characters.sivil_toplum,
    text: 'Demokratik kurumlar zayıflıyor. Yargı bağımsızlığı tehlikede.',
    leftChoice: {
      text: 'Yürütme güçlü olmalı',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ]
    },
    rightChoice: {
      text: 'Kuvvetler ayrılığı',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 },
        { resource: ResourceType.FAITH, min: 15, max: 20 }
      ],
      setFlags: ['gucler_ayriligi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_sivil_yolsuzluk',
    character: characters.sivil_toplum,
    text: 'Yolsuzluk iddiaları var. Bağımsız soruşturma başlatalım mı?',
    leftChoice: {
      text: 'İç soruşturma yeter',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ]
    },
    rightChoice: {
      text: 'Bağımsız soruşturma',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 15, max: 20 }
      ],
      setFlags: ['yolsuzluk_sorusturma']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= GENEL OLAYLAR =============
  {
    id: 'mod_secim',
    character: characters.sivil_toplum,
    text: 'Seçimler yaklaşıyor. Adil ve şeffaf seçim için bağımsız gözlemci istiyoruz.',
    leftChoice: {
      text: 'Gerek yok',
      effects: [
        { resource: ResourceType.FAITH, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Gözlemcileri davet et',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['seffaf_secim']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_egitim_reform',
    character: characters.aktivist,
    text: 'Eğitim sistemi çağın gerisinde. Dijital okuryazarlık şart.',
    leftChoice: {
      text: 'Geleneksel sistem iyi',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Eğitim reformu',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['egitim_reformu']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_saglik_universal',
    character: characters.bilim_insani,
    text: 'Evrensel sağlık sigortası tartışılıyor. Herkes sağlık hizmetine erişebilmeli mi?',
    leftChoice: {
      text: 'Özel sektör daha iyi',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Evrensel sağlık',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: 5, max: 10 }
      ],
      setFlags: ['evrensel_saglik']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'mod_kuresellesme',
    character: characters.diplomat,
    text: 'Küreselleşme hızlanıyor. Ulusal kimlik mi yoksa dünya vatandaşlığı mı?',
    leftChoice: {
      text: 'Milli değerler',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Kozmopolit yaklaşım',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ],
      setFlags: ['kuresel_entegrasyon']
    },
    category: EventCategory.RANDOM,
    weight: 1
  },
  {
    id: 'mod_otomasyon',
    character: characters.tech_ceo,
    text: 'Otomasyon işleri yok ediyor. Robotlar insan işçilerin yerini alıyor.',
    leftChoice: {
      text: 'Robot vergisi',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Yeniden eğitim programı',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ],
      setFlags: ['yeniden_egitim']
    },
    category: EventCategory.STORY,
    weight: 1
  }
];
