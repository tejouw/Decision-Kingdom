import { Card, Character, ResourceType } from '../models/types.js';

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
  tüccar: {
    id: 'tuccar',
    name: 'Tüccar Hasan',
    title: 'Ticaret Loncası Başkanı',
    avatar: '🏪'
  }
};

// Ortaçağ kartları
export const medievalCards: Card[] = [
  // Vezir kartları
  {
    id: 'vezir_vergi',
    character: characters.vezir,
    text: 'Sultanım, hazine tükenmek üzere. Halktan ek vergi toplamamızı öneriyorum. Ne dersiniz?',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 10 }
      ]
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 }
      ]
    }
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
      ]
    }
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
      ]
    },
    rightChoice: {
      text: 'Kabul et',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 10 },
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    }
  },

  // General kartları
  {
    id: 'general_savunma',
    character: characters.general,
    text: 'Sultanım, sınırlarda hareketlilik var. Orduyu güçlendirmemiz gerekiyor.',
    leftChoice: {
      text: 'Şimdi değil',
      effects: [
        { resource: ResourceType.MILITARY, min: -8, max: -5 },
        { resource: ResourceType.GOLD, min: 5, max: 8 }
      ]
    },
    rightChoice: {
      text: 'Orduyu güçlendir',
      effects: [
        { resource: ResourceType.MILITARY, min: 10, max: 15 },
        { resource: ResourceType.GOLD, min: -12, max: -8 }
      ]
    }
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
      ]
    },
    rightChoice: {
      text: 'Sefere çık',
      effects: [
        { resource: ResourceType.GOLD, min: 15, max: 25 },
        { resource: ResourceType.MILITARY, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: -5, max: -3 }
      ]
    }
  },
  {
    id: 'general_egitim',
    character: characters.general,
    text: 'Askerlere yeni eğitim programı başlatmak istiyorum. İzin verir misiniz?',
    leftChoice: {
      text: 'Reddet',
      effects: [
        { resource: ResourceType.MILITARY, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.MILITARY, min: 8, max: 12 },
        { resource: ResourceType.GOLD, min: -8, max: -5 }
      ]
    }
  },

  // Hazinedar kartları
  {
    id: 'hazinedar_ticaret',
    character: characters.hazinedar,
    text: 'Yeni ticaret yolları açabiliriz. Başlangıç yatırımı gerekiyor.',
    leftChoice: {
      text: 'Çok riskli',
      effects: [
        { resource: ResourceType.GOLD, min: -3, max: -1 }
      ]
    },
    rightChoice: {
      text: 'Yatırım yap',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 5, max: 8 }
      ],
      triggeredEvents: ['hazinedar_ticaret_sonuc']
    }
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
    priority: 10
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
      ]
    }
  },

  // İmam kartları
  {
    id: 'imam_cami',
    character: characters.imam,
    text: 'Sultanım, yeni bir cami inşa etmemiz halkın maneviyatını yükseltir.',
    leftChoice: {
      text: 'Bütçe yok',
      effects: [
        { resource: ResourceType.FAITH, min: -8, max: -5 }
      ]
    },
    rightChoice: {
      text: 'İnşa et',
      effects: [
        { resource: ResourceType.GOLD, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: 12, max: 18 },
        { resource: ResourceType.HAPPINESS, min: 3, max: 5 }
      ]
    }
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
      ]
    },
    rightChoice: {
      text: 'Onayla',
      effects: [
        { resource: ResourceType.GOLD, min: -8, max: -5 },
        { resource: ResourceType.FAITH, min: 8, max: 12 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ]
    }
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
      ]
    },
    rightChoice: {
      text: 'Yasakla',
      effects: [
        { resource: ResourceType.FAITH, min: 10, max: 15 },
        { resource: ResourceType.HAPPINESS, min: -10, max: -5 }
      ]
    }
  },

  // Köy muhtarı kartları
  {
    id: 'muhtar_kuraklik',
    character: characters.koy_muhtari,
    text: 'Sultanım, kuraklık var. Köylüler yardım bekliyor.',
    leftChoice: {
      text: 'Beklesinler',
      effects: [
        { resource: ResourceType.HAPPINESS, min: -15, max: -10 },
        { resource: ResourceType.FAITH, min: -5, max: -3 }
      ]
    },
    rightChoice: {
      text: 'Yardım gönder',
      effects: [
        { resource: ResourceType.GOLD, min: -10, max: -5 },
        { resource: ResourceType.HAPPINESS, min: 10, max: 15 },
        { resource: ResourceType.FAITH, min: 5, max: 8 }
      ]
    }
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
    }
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
    }
  },

  // Tüccar kartları
  {
    id: 'tuccar_kervan',
    character: characters.tüccar,
    text: 'Uzak diyarlardan kervan geldi. Pahalı mallar getirmişler.',
    leftChoice: {
      text: 'Geri çevir',
      effects: [
        { resource: ResourceType.GOLD, min: 3, max: 5 }
      ]
    },
    rightChoice: {
      text: 'Satın al',
      effects: [
        { resource: ResourceType.GOLD, min: -12, max: -8 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ]
    }
  },
  {
    id: 'tuccar_lonca',
    character: characters.tüccar,
    text: 'Lonca vergileri çok yüksek diyor tüccarlar. İndirim yapalım mı?',
    leftChoice: {
      text: 'Hayır',
      effects: [
        { resource: ResourceType.GOLD, min: 5, max: 8 },
        { resource: ResourceType.HAPPINESS, min: -8, max: -5 }
      ]
    },
    rightChoice: {
      text: 'İndir',
      effects: [
        { resource: ResourceType.GOLD, min: -5, max: -3 },
        { resource: ResourceType.HAPPINESS, min: 8, max: 12 }
      ]
    }
  },
  {
    id: 'tuccar_karaborsaci',
    character: characters.tüccar,
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
    }
  },

  // Ek çeşitli kartlar
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
    priority: 5
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
    priority: 8
  }
];

export default medievalCards;
