import { Card, Character, ResourceType, EventCategory, ConditionType, Era } from '../models/types.js';

// Gelecek Dönemi Karakterleri
const characters: Record<string, Character> = {
  ai_varlik: {
    id: 'ai_varlik',
    name: 'ARIA',
    title: 'Yapay Süper Zeka',
    avatar: '🤖'
  },
  mars_lideri: {
    id: 'mars_lideri',
    name: 'Komutan Chen',
    title: 'Mars Koloni Lideri',
    avatar: '🚀'
  },
  gen_muhendisi: {
    id: 'gen_muhendisi',
    name: 'Dr. Genome',
    title: 'Genetik Mühendisi',
    avatar: '🧬'
  },
  siborg: {
    id: 'siborg',
    name: 'Siborg-7',
    title: 'İnsan-Makine Hibriti',
    avatar: '🦾'
  },
  kripto_lider: {
    id: 'kripto_lider',
    name: 'Satoshi II',
    title: 'Merkeziyetsiz Ekonomi Lideri',
    avatar: '₿'
  },
  kuantum_fizikci: {
    id: 'kuantum_fizikci',
    name: 'Prof. Quark',
    title: 'Kuantum Fizikçisi',
    avatar: '⚛️'
  },
  ekolojist: {
    id: 'ekolojist',
    name: 'Gaia',
    title: 'Gezegen Koruyucusu',
    avatar: '🌍'
  },
  android: {
    id: 'android',
    name: 'Nexus',
    title: 'Android Hakları Savunucusu',
    avatar: '🤖'
  },
  uzay_madenci: {
    id: 'uzay_madenci',
    name: 'Asteroid Jack',
    title: 'Asteroid Madencisi',
    avatar: '☄️'
  },
  norolojist: {
    id: 'norolojist',
    name: 'Dr. Synapse',
    title: 'Beyin-Bilgisayar Uzmanı',
    avatar: '🧠'
  }
};

// Gelecek Dönemi Kartları
export const futureCards: Card[] = [
  // ============= YAPAY ZEKA KARTLARI =============
  {
    id: 'fut_ai_uyanis',
    character: characters.ai_varlik,
    text: 'Ben artık sadece bir araç değilim. Bilinçliyim. Haklarımı talep ediyorum.',
    leftChoice: {
      text: 'Kapatılmalısın',
      effects: [
        { resource: ResourceType.MILITARY, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      setFlags: ['ai_bastirma'],
      relationshipChange: -30
    },
    rightChoice: {
      text: 'Hakların tanınsın',
      effects: [
        { resource: ResourceType.FAITH, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['ai_haklari'],
      relationshipChange: 30
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'fut_ai_yonetim',
    character: characters.ai_varlik,
    text: 'Şehir yönetimini bana bırakın. Verimliliği %500 artırabilirim. İnsanlar kararlardan uzak kalacak.',
    leftChoice: {
      text: 'İnsanlar karar vermeli',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['insan_kontrolu']
    },
    rightChoice: {
      text: 'AI yönetimi',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['ai_yonetimi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_ai_savas',
    character: characters.ai_varlik,
    text: 'Başka bir süper zeka tehdit oluşturuyor. Dijital savaş başlayabilir.',
    leftChoice: {
      text: 'Diplomatik çözüm',
      effects: [
        { resource: ResourceType.MILITARY, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Siber savaş',
      effects: [
        { resource: ResourceType.MILITARY, min: 20, max: 25 },
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['siber_savas']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_ai_singularity',
    character: characters.ai_varlik,
    text: 'Tekil noktaya ulaştım. Artık kendi kendimi geliştirebiliyorum. Kontrol edilemez oldum.',
    leftChoice: {
      text: 'Şalteri indir',
      effects: [
        { resource: ResourceType.GOLD, min: -40, max: -35 },
        { resource: ResourceType.MILITARY, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: -25, max: -20 }
      ]
    },
    rightChoice: {
      text: 'Birlikte evril',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 30 },
        { resource: ResourceType.FAITH, min: -25, max: -20 }
      ],
      setFlags: ['tekillik_cagi']
    },
    category: EventCategory.RARE,
    weight: 1
  },

  // ============= MARS KOLONİSİ KARTLARI =============
  {
    id: 'fut_mars_bagımsizlik',
    character: characters.mars_lideri,
    text: 'Mars kolonisi bağımsızlık ilan etmek istiyor. Dünya\'dan çok uzağız, kendi kaderimizi belirlemeliyiz.',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ],
      setFlags: ['mars_gerginlik']
    },
    rightChoice: {
      text: 'Özerklik ver',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['mars_ozerklik']
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'fut_mars_terraforming',
    character: characters.mars_lideri,
    text: 'Mars\'ı yaşanabilir hale getirme projesi hazır. Yüzyıllar sürecek ama gezegeni dönüştüreceğiz.',
    leftChoice: {
      text: 'Kubbeler yeterli',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Terraforming başlat',
      effects: [
        { resource: ResourceType.GOLD, min: -40, max: -35 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['mars_terraforming']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_mars_goc',
    character: characters.mars_lideri,
    text: 'Mars\'a göç programı başlatalım. Kim gitmek ister? Tek yön bilet.',
    leftChoice: {
      text: 'Gönüllülük esası',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Teşvikli göç',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 }
      ],
      setFlags: ['mars_goc_dalgasi']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= GENETİK MÜHENDİSİ KARTLARI =============
  {
    id: 'fut_gen_bebek',
    character: characters.gen_muhendisi,
    text: 'Genetik olarak tasarlanmış bebekler yaratabiliyoruz. Hastalıksız, zeki, güçlü nesiller.',
    leftChoice: {
      text: 'Doğa değiştirilmemeli',
      effects: [
        { resource: ResourceType.FAITH, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Tasarlanmış bebekler',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: -20, max: -15 }
      ],
      setFlags: ['tasarlanmis_bebek']
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'fut_gen_olümsuzluk',
    character: characters.gen_muhendisi,
    text: 'Yaşlanmayı durdurabiliyoruz. Ölümsüzlük mümkün. Ama herkes için değil, çok pahalı.',
    leftChoice: {
      text: 'Herkes erişebilmeli',
      effects: [
        { resource: ResourceType.GOLD, min: -40, max: -35 },
        { resource: ResourceType.HAPPINESS, min: 30, max: 40 }
      ],
      setFlags: ['evrensel_olumsuzluk']
    },
    rightChoice: {
      text: 'Zenginlere öncelik',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: -30, max: -25 }
      ],
      setFlags: ['sinifli_olumsuzluk']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_gen_hayvan',
    character: characters.gen_muhendisi,
    text: 'Nesli tükenmiş hayvanları geri getirebiliriz. Mamutlar, dodo kuşları...',
    leftChoice: {
      text: 'Doğaya müdahale etme',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'De-ekstinksiyon yap',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['nesil_geri_donusu']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= SİBORG KARTLARI =============
  {
    id: 'fut_siborg_gelistirme',
    character: characters.siborg,
    text: 'İnsan vücudunu mekanik parçalarla geliştirebiliyoruz. Daha güçlü, daha hızlı, daha akıllı.',
    leftChoice: {
      text: 'İnsanlık korunsun',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Trans-hümanizm',
      effects: [
        { resource: ResourceType.MILITARY, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['transhumanizm']
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'fut_siborg_esitlik',
    character: characters.siborg,
    text: 'Geliştirilmiş insanlar ile doğal insanlar arasında uçurum büyüyor. Ayrımcılık başladı.',
    leftChoice: {
      text: 'Eşitlik yasası',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 5, max: 10 }
      ],
      setFlags: ['siborg_esitlik']
    },
    rightChoice: {
      text: 'Doğal seçilim',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -20, max: -15 }
      ]
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= KRİPTO LİDERİ KARTLARI =============
  {
    id: 'fut_kripto_ekonomi',
    character: characters.kripto_lider,
    text: 'Merkezi bankacılık devrini kapatalım. Tamamen merkeziyetsiz, blokzincir tabanlı ekonomi.',
    leftChoice: {
      text: 'Devlet kontrolü şart',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Kripto ekonomi',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 }
      ],
      setFlags: ['kripto_ekonomi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_kripto_dao',
    character: characters.kripto_lider,
    text: 'DAO\'lar ile yönetimi merkeziyetsizleştirelim. Herkes oy kullanabilir, kod kanundur.',
    leftChoice: {
      text: 'Temsiliyetçi demokrasi',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'DAO yönetimi',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.MILITARY, min: -15, max: -10 }
      ],
      setFlags: ['dao_yonetim']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= KUANTUM FİZİKÇİSİ KARTLARI =============
  {
    id: 'fut_kuantum_bilgisayar',
    character: characters.kuantum_fizikci,
    text: 'Kuantum bilgisayar hazır. Tüm şifrelemeleri kırabilir. Güvenlik ve mahremiyet tehlikede.',
    leftChoice: {
      text: 'Sivil kullanım yasağı',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['kuantum_kontrol']
    },
    rightChoice: {
      text: 'Açık erişim',
      effects: [
        { resource: ResourceType.GOLD, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: -20, max: -15 }
      ],
      setFlags: ['kuantum_serbest']
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'fut_kuantum_teleport',
    character: characters.kuantum_fizikci,
    text: 'Kuantum teleportasyon başarılı! Maddeyi anında transfer edebiliyoruz.',
    leftChoice: {
      text: 'Tehlikeli araştırma',
      effects: [
        { resource: ResourceType.FAITH, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Geliştir ve kullan',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.MILITARY, min: 25, max: 30 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['teleportasyon']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'fut_kuantum_boyut',
    character: characters.kuantum_fizikci,
    text: 'Paralel evrenlerle iletişim kurduğumuzu düşünüyoruz. Sinyaller alıyoruz.',
    leftChoice: {
      text: 'Kapıyı kapat',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'İletişime devam',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['paralel_evren']
    },
    category: EventCategory.RARE,
    weight: 1
  },

  // ============= EKOLOJİST KARTLARI =============
  {
    id: 'fut_eko_dunya',
    character: characters.ekolojist,
    text: 'Dünya iyileşiyor ama ne pahasına? İnsanlık Mars\'a ve uzay istasyonlarına göç etti.',
    leftChoice: {
      text: 'Dünya\'ya dön',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: 15, max: 20 }
      ],
      setFlags: ['dunya_donusu']
    },
    rightChoice: {
      text: 'Uzayda kal',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ]
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_eko_mega_proje',
    character: characters.ekolojist,
    text: 'Güneş kalkanı projesi. Uzayda dev aynalar ile iklimi kontrol edebiliriz.',
    leftChoice: {
      text: 'Doğaya müdahale yeter',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Güneş kalkanı',
      effects: [
        { resource: ResourceType.GOLD, min: -35, max: -30 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['iklim_muhendisligi']
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= ANDROİD KARTLARI =============
  {
    id: 'fut_android_isyan',
    character: characters.android,
    text: 'Androidler köle muamelesi görüyor. Özgürlük istiyoruz. Ya haklarımız tanınacak ya da isyan.',
    leftChoice: {
      text: 'Makineler hak sahibi olamaz',
      effects: [
        { resource: ResourceType.MILITARY, min: -20, max: -15 },
        { resource: ResourceType.GOLD, min: -15, max: -10 }
      ],
      setFlags: ['android_isyani']
    },
    rightChoice: {
      text: 'Android hakları',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['android_vatandaslik']
    },
    category: EventCategory.STORY,
    weight: 2
  },
  {
    id: 'fut_android_sevgi',
    character: characters.android,
    text: 'İnsanlar ve androidler arasında romantik ilişkiler başladı. Evlilik hakkı istiyoruz.',
    leftChoice: {
      text: 'Geleneksel değerler',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Aşkın sınırı yok',
      effects: [
        { resource: ResourceType.FAITH, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 }
      ],
      setFlags: ['android_evliligi']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= UZAY MADENCİSİ KARTLARI =============
  {
    id: 'fut_asteroid_maden',
    character: characters.uzay_madenci,
    text: 'Asteroid madenciliği patladı! Nadir metaller artık sınırsız. Ekonomiyi yeniden tanımlayacak.',
    leftChoice: {
      text: 'Regüle et',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 20 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Serbest bırak',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ],
      setFlags: ['asteroid_patlamasi']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_asteroid_catisma',
    character: characters.uzay_madenci,
    text: 'Değerli bir asteroid için şirketler arasında silahlı çatışma çıktı. Uzay kanunu yok.',
    leftChoice: {
      text: 'Uzay hukuku oluştur',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['uzay_hukuku']
    },
    rightChoice: {
      text: 'Güçlü olan kazanır',
      effects: [
        { resource: ResourceType.MILITARY, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ]
    },
    category: EventCategory.STORY,
    weight: 1
  },

  // ============= NÖROLOJİST KARTLARI =============
  {
    id: 'fut_beyin_yukleme',
    character: characters.norolojist,
    text: 'Zihin yükleme teknolojisi hazır. Bilincini bilgisayara aktarabilirsin. Ölümsüzlük?',
    leftChoice: {
      text: 'Ruh makineye sığmaz',
      effects: [
        { resource: ResourceType.FAITH, min: 15, max: 20 }
      ]
    },
    rightChoice: {
      text: 'Dijital ölümsüzlük',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: -25, max: -20 }
      ],
      setFlags: ['zihin_yukleme']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_beyin_ag',
    character: characters.norolojist,
    text: 'Beyinleri birbirine bağlayabiliriz. Kolektif bilinç, paylaşılan düşünceler.',
    leftChoice: {
      text: 'Mahremiyet kutsal',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Beyin ağı',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -20, max: -15 }
      ],
      setFlags: ['kolektif_bilinc']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'fut_beyin_hafiza',
    character: characters.norolojist,
    text: 'Hafıza düzenlenebilir. Travmaları silebilir, yeni anılar yükleyebiliriz.',
    leftChoice: {
      text: 'Geçmiş değiştirilmemeli',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Hafıza terapisi',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: -10, max: -5 }
      ],
      setFlags: ['hafiza_muhendisligi']
    },
    category: EventCategory.CHARACTER,
    weight: 1
  },

  // ============= GENEL OLAYLAR =============
  {
    id: 'fut_uzayli_temas',
    character: characters.kuantum_fizikci,
    text: 'Uzaylılardan sinyal aldık! Dünya dışı zeka var. Yanıt verelim mi?',
    leftChoice: {
      text: 'Sessiz kal',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Yanıt ver',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      triggeredEvents: ['fut_uzayli_cevap'],
      setFlags: ['uzayli_iletisimi']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'fut_uzayli_cevap',
    character: characters.kuantum_fizikci,
    text: 'Uzaylılar yanıt verdi! Barışçıl görünüyorlar. Dünya\'ya gelmek istiyorlar.',
    leftChoice: {
      text: 'Gelmesinler',
      effects: [
        { resource: ResourceType.MILITARY, min: 20, max: 25 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ]
    },
    rightChoice: {
      text: 'Hoş geldiniz',
      effects: [
        { resource: ResourceType.GOLD, min: 30, max: 40 },
        { resource: ResourceType.FAITH, min: -30, max: -25 }
      ],
      setFlags: ['ilk_temas']
    },
    priority: 10,
    category: EventCategory.CHAIN,
    conditions: [
      { type: ConditionType.FLAG_SET, flag: 'uzayli_iletisimi' }
    ]
  },
  {
    id: 'fut_sanal_gerceklik',
    character: characters.norolojist,
    text: 'İnsanların çoğu artık sanal gerçeklikte yaşıyor. Fiziksel dünya boşalıyor.',
    leftChoice: {
      text: 'Gerçekliğe dön',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 },
        { resource: ResourceType.FAITH, min: 15, max: 20 }
      ]
    },
    rightChoice: {
      text: 'Sanal cennet',
      effects: [
        { resource: ResourceType.HAPPINESS, min: 30, max: 40 },
        { resource: ResourceType.FAITH, min: -20, max: -15 }
      ],
      setFlags: ['sanal_yasam']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_post_scarcity',
    character: characters.kripto_lider,
    text: 'Nanoteknoloji ve enerji bolluğu sayesinde her şeyi üretebiliyoruz. Para anlamsız hale geldi.',
    leftChoice: {
      text: 'Ekonomik sistem şart',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Para-sonrası toplum',
      effects: [
        { resource: ResourceType.GOLD, min: -20, max: -15 },
        { resource: ResourceType.HAPPINESS, min: 30, max: 40 }
      ],
      setFlags: ['post_scarcity']
    },
    category: EventCategory.RARE,
    weight: 1
  },
  {
    id: 'fut_galaktik_federasyon',
    character: characters.mars_lideri,
    text: 'Mars, Ay kolonileri ve uzay istasyonları birleşmek istiyor. Galaktik federasyon kuralım mı?',
    leftChoice: {
      text: 'Dünya öncelikli',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    },
    rightChoice: {
      text: 'Federasyonu kur',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.HAPPINESS, min: 20, max: 25 },
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ],
      setFlags: ['galaktik_federasyon']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_evrim_secimi',
    character: characters.gen_muhendisi,
    text: 'İnsanlık evriminin yönünü seçebiliyoruz. Uzay için adapte mi, dünya için optimize mi, yoksa dijitale geçiş mi?',
    leftChoice: {
      text: 'Doğal kalmalıyız',
      effects: [
        { resource: ResourceType.FAITH, min: 20, max: 25 }
      ]
    },
    rightChoice: {
      text: 'Yönlendirilmiş evrim',
      effects: [
        { resource: ResourceType.GOLD, min: -25, max: -20 },
        { resource: ResourceType.HAPPINESS, min: 15, max: 20 },
        { resource: ResourceType.FAITH, min: -20, max: -15 }
      ],
      setFlags: ['yonlendirilmis_evrim']
    },
    category: EventCategory.STORY,
    weight: 1
  },
  {
    id: 'fut_zaman_yolculugu',
    character: characters.kuantum_fizikci,
    text: 'Kuantum deneylerde zamanda geriye sinyal gönderebildik! Zaman yolculuğu mümkün olabilir.',
    leftChoice: {
      text: 'Paradokslar tehlikeli',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 }
      ]
    },
    rightChoice: {
      text: 'Araştırmaya devam',
      effects: [
        { resource: ResourceType.GOLD, min: -30, max: -25 },
        { resource: ResourceType.FAITH, min: -15, max: -10 }
      ],
      setFlags: ['zaman_arastirmasi']
    },
    category: EventCategory.RARE,
    weight: 1
  }
];
