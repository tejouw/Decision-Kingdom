import {
  Achievement,
  AchievementCategory,
  AchievementConditionType,
  EndingType,
  ResourceType,
  Era
} from '../models/types.js';

export const achievements: Achievement[] = [
  // ============================================
  // HAYATTA KALMA BAŞARIMLARI
  // ============================================
  {
    id: 'survival_10',
    name: 'Acemi Sultan',
    description: '10 tur hayatta kal',
    category: AchievementCategory.SURVIVAL,
    icon: '👑',
    ppReward: 5,
    isSecret: false,
    condition: {
      type: AchievementConditionType.TURNS_SURVIVED,
      value: 10
    }
  },
  {
    id: 'survival_30',
    name: 'Deneyimli Yonetici',
    description: '30 tur hayatta kal',
    category: AchievementCategory.SURVIVAL,
    icon: '🏰',
    ppReward: 15,
    isSecret: false,
    condition: {
      type: AchievementConditionType.TURNS_SURVIVED,
      value: 30
    }
  },
  {
    id: 'survival_50',
    name: 'Usta Sultan',
    description: '50 tur hayatta kal',
    category: AchievementCategory.SURVIVAL,
    icon: '🌟',
    ppReward: 30,
    isSecret: false,
    condition: {
      type: AchievementConditionType.TURNS_SURVIVED,
      value: 50
    }
  },
  {
    id: 'survival_100',
    name: 'Efsanevi Hukumdar',
    description: '100 tur hayatta kal',
    category: AchievementCategory.SURVIVAL,
    icon: '💫',
    ppReward: 75,
    isSecret: false,
    condition: {
      type: AchievementConditionType.TURNS_SURVIVED,
      value: 100
    }
  },
  {
    id: 'games_5',
    name: 'Yenilmez Ruh',
    description: '5 oyun tamamla',
    category: AchievementCategory.SURVIVAL,
    icon: '🔄',
    ppReward: 10,
    isSecret: false,
    condition: {
      type: AchievementConditionType.GAMES_COMPLETED,
      value: 5
    }
  },
  {
    id: 'games_20',
    name: 'Kararli Hukumdar',
    description: '20 oyun tamamla',
    category: AchievementCategory.SURVIVAL,
    icon: '🎯',
    ppReward: 25,
    isSecret: false,
    condition: {
      type: AchievementConditionType.GAMES_COMPLETED,
      value: 20
    }
  },
  {
    id: 'games_50',
    name: 'Taht Bagimlisi',
    description: '50 oyun tamamla',
    category: AchievementCategory.SURVIVAL,
    icon: '🎮',
    ppReward: 50,
    isSecret: false,
    condition: {
      type: AchievementConditionType.GAMES_COMPLETED,
      value: 50
    }
  },

  // ============================================
  // SKOR BAŞARIMLARI
  // ============================================
  {
    id: 'score_100',
    name: 'Iyi Baslangiç',
    description: '100 puan kazan',
    category: AchievementCategory.SURVIVAL,
    icon: '📊',
    ppReward: 5,
    isSecret: false,
    condition: {
      type: AchievementConditionType.SPECIFIC_SCORE,
      value: 100
    }
  },
  {
    id: 'score_300',
    name: 'Basarili Yonetim',
    description: '300 puan kazan',
    category: AchievementCategory.SURVIVAL,
    icon: '📈',
    ppReward: 15,
    isSecret: false,
    condition: {
      type: AchievementConditionType.SPECIFIC_SCORE,
      value: 300
    }
  },
  {
    id: 'score_500',
    name: 'Muhteşem Performans',
    description: '500 puan kazan',
    category: AchievementCategory.SURVIVAL,
    icon: '🏆',
    ppReward: 30,
    isSecret: false,
    condition: {
      type: AchievementConditionType.SPECIFIC_SCORE,
      value: 500
    }
  },
  {
    id: 'score_1000',
    name: 'Efsane Skor',
    description: '1000 puan kazan',
    category: AchievementCategory.SURVIVAL,
    icon: '👑',
    ppReward: 75,
    isSecret: false,
    condition: {
      type: AchievementConditionType.SPECIFIC_SCORE,
      value: 1000
    }
  },

  // ============================================
  // HİKAYE / BİTİŞ BAŞARIMLARI
  // ============================================
  {
    id: 'ending_peaceful',
    name: 'Baris Elcisi',
    description: 'Barisçil Sultan sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '🕊️',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.PEACEFUL
    }
  },
  {
    id: 'ending_military',
    name: 'Demir Yumruk',
    description: 'Askeri Diktatorluk sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '⚔️',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.MILITARY
    }
  },
  {
    id: 'ending_theocratic',
    name: 'Ilahi Yonetici',
    description: 'Teokratik Yonetim sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '🙏',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.THEOCRATIC
    }
  },
  {
    id: 'ending_merchant',
    name: 'Altin Tacir',
    description: 'Tuccar Oligarşisi sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '💰',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.MERCHANT
    }
  },
  {
    id: 'ending_beloved',
    name: 'Halk Kahramani',
    description: 'Sevilen Sultan sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '❤️',
    ppReward: 25,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.BELOVED
    }
  },
  {
    id: 'ending_scholar',
    name: 'Bilgelik Yolu',
    description: 'Bilge Sultan sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '📚',
    ppReward: 25,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.SCHOLAR
    }
  },
  {
    id: 'ending_conqueror',
    name: 'Fatih',
    description: 'Fatih Sultan sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '🦅',
    ppReward: 30,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.CONQUEROR
    }
  },
  {
    id: 'ending_balanced',
    name: 'Denge Ustasi',
    description: 'Dengeli Yonetim sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '⚖️',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.BALANCED
    }
  },
  {
    id: 'ending_tyranny',
    name: 'Korkunc Sultan',
    description: 'Zorba Sultan sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '😈',
    ppReward: 15,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.TYRANNY
    }
  },
  {
    id: 'ending_revolutionary',
    name: 'Devrimci Ruh',
    description: 'Devrimci Sultan sonuna ulaş',
    category: AchievementCategory.STORY,
    icon: '✊',
    ppReward: 25,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ENDING_REACHED,
      endingType: EndingType.REVOLUTIONARY
    }
  },

  // ============================================
  // KARAKTER BAŞARIMLARI
  // ============================================
  {
    id: 'all_characters',
    name: 'Sosyal Kelebek',
    description: 'Tum 10 karakter ile tanış',
    category: AchievementCategory.CHARACTER,
    icon: '🤝',
    ppReward: 25,
    isSecret: false,
    condition: {
      type: AchievementConditionType.ALL_CHARACTERS_MET
    }
  },
  {
    id: 'vizier_5',
    name: 'Sadik Dost',
    description: 'Sadrazam Ahmet ile 5 kez etkileşime gir',
    category: AchievementCategory.CHARACTER,
    icon: '👔',
    ppReward: 10,
    isSecret: false,
    condition: {
      type: AchievementConditionType.CHARACTER_INTERACTION_COUNT,
      characterId: 'sadrazam_ahmet',
      value: 5
    }
  },
  {
    id: 'general_5',
    name: 'Savas Arkadaşi',
    description: 'Paşa Mahmut ile 5 kez etkileşime gir',
    category: AchievementCategory.CHARACTER,
    icon: '🎖️',
    ppReward: 10,
    isSecret: false,
    condition: {
      type: AchievementConditionType.CHARACTER_INTERACTION_COUNT,
      characterId: 'pasa_mahmut',
      value: 5
    }
  },
  {
    id: 'treasurer_5',
    name: 'Para Ustasi',
    description: 'Hazinedar Mustafa ile 5 kez etkileşime gir',
    category: AchievementCategory.CHARACTER,
    icon: '💵',
    ppReward: 10,
    isSecret: false,
    condition: {
      type: AchievementConditionType.CHARACTER_INTERACTION_COUNT,
      characterId: 'hazinedar_mustafa',
      value: 5
    }
  },
  {
    id: 'imam_5',
    name: 'Inanc Yolu',
    description: 'Şeyhulislam Omer ile 5 kez etkileşime gir',
    category: AchievementCategory.CHARACTER,
    icon: '🕌',
    ppReward: 10,
    isSecret: false,
    condition: {
      type: AchievementConditionType.CHARACTER_INTERACTION_COUNT,
      characterId: 'seyhulislam_omer',
      value: 5
    }
  },
  {
    id: 'spy_5',
    name: 'Golge Avcisi',
    description: 'Golge ile 5 kez etkileşime gir',
    category: AchievementCategory.CHARACTER,
    icon: '🕵️',
    ppReward: 15,
    isSecret: false,
    condition: {
      type: AchievementConditionType.CHARACTER_INTERACTION_COUNT,
      characterId: 'golge',
      value: 5
    }
  },
  {
    id: 'princess_5',
    name: 'Kraliyet Dostu',
    description: 'Prenses Fatma ile 5 kez etkileşime gir',
    category: AchievementCategory.CHARACTER,
    icon: '👸',
    ppReward: 15,
    isSecret: false,
    condition: {
      type: AchievementConditionType.CHARACTER_INTERACTION_COUNT,
      characterId: 'prenses_fatma',
      value: 5
    }
  },

  // ============================================
  // GİZLİ BAŞARIMLAR
  // ============================================
  {
    id: 'secret_quick_death',
    name: 'Hizli Son',
    description: 'Ilk 5 turda oyunu bitir',
    category: AchievementCategory.SECRET,
    icon: '💀',
    ppReward: 5,
    isSecret: true,
    condition: {
      type: AchievementConditionType.TURNS_SURVIVED,
      value: 5
    }
  },
  {
    id: 'secret_pp_100',
    name: 'Prestij Avcisi',
    description: 'Toplam 100 PP kazan',
    category: AchievementCategory.SECRET,
    icon: '🌟',
    ppReward: 20,
    isSecret: true,
    condition: {
      type: AchievementConditionType.TOTAL_PP_EARNED,
      value: 100
    }
  },
  {
    id: 'secret_pp_500',
    name: 'Prestij Ustasi',
    description: 'Toplam 500 PP kazan',
    category: AchievementCategory.SECRET,
    icon: '💎',
    ppReward: 50,
    isSecret: true,
    condition: {
      type: AchievementConditionType.TOTAL_PP_EARNED,
      value: 500
    }
  },
  {
    id: 'secret_war_declaration',
    name: 'Savas Ilani',
    description: 'Savaş ilan et',
    category: AchievementCategory.SECRET,
    icon: '🔥',
    ppReward: 15,
    isSecret: true,
    condition: {
      type: AchievementConditionType.FLAG_SET,
      flag: 'war_declared'
    }
  },
  {
    id: 'secret_rebellion',
    name: 'Isyan Bastiran',
    description: 'Bir isyani bastir',
    category: AchievementCategory.SECRET,
    icon: '🛡️',
    ppReward: 15,
    isSecret: true,
    condition: {
      type: AchievementConditionType.FLAG_SET,
      flag: 'rebellion_crushed'
    }
  },
  {
    id: 'secret_plague_survivor',
    name: 'Veba Kahramani',
    description: 'Veba salginini atlattirsertifika',
    category: AchievementCategory.SECRET,
    icon: '🏥',
    ppReward: 20,
    isSecret: true,
    condition: {
      type: AchievementConditionType.FLAG_SET,
      flag: 'plague_survived'
    }
  },

  // ============================================
  // EKSTREM BAŞARIMLAR
  // ============================================
  {
    id: 'extreme_gold_master',
    name: 'Altin Imparatoru',
    description: 'Hazine 90\'a ulaştir',
    category: AchievementCategory.EXTREME,
    icon: '💰',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.RESOURCE_REACHED,
      resource: ResourceType.GOLD,
      value: 90
    }
  },
  {
    id: 'extreme_happiness_master',
    name: 'Mutluluk Imparatoru',
    description: 'Mutluluk 90\'a ulaştir',
    category: AchievementCategory.EXTREME,
    icon: '😊',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.RESOURCE_REACHED,
      resource: ResourceType.HAPPINESS,
      value: 90
    }
  },
  {
    id: 'extreme_military_master',
    name: 'Savas Lordu',
    description: 'Ordu 90\'a ulaştir',
    category: AchievementCategory.EXTREME,
    icon: '⚔️',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.RESOURCE_REACHED,
      resource: ResourceType.MILITARY,
      value: 90
    }
  },
  {
    id: 'extreme_faith_master',
    name: 'Din Imparatoru',
    description: 'Inanç 90\'a ulaştir',
    category: AchievementCategory.EXTREME,
    icon: '🙏',
    ppReward: 20,
    isSecret: false,
    condition: {
      type: AchievementConditionType.RESOURCE_REACHED,
      resource: ResourceType.FAITH,
      value: 90
    }
  },
  {
    id: 'extreme_cards_100',
    name: 'Kart Koleksiyoncusu',
    description: 'Toplam 100 kart oyna',
    category: AchievementCategory.EXTREME,
    icon: '🃏',
    ppReward: 15,
    isSecret: false,
    condition: {
      type: AchievementConditionType.TOTAL_CARDS_PLAYED,
      value: 100
    }
  },
  {
    id: 'extreme_cards_500',
    name: 'Karar Makinesi',
    description: 'Toplam 500 kart oyna',
    category: AchievementCategory.EXTREME,
    icon: '🎴',
    ppReward: 40,
    isSecret: false,
    condition: {
      type: AchievementConditionType.TOTAL_CARDS_PLAYED,
      value: 500
    }
  }
];

// Başarım yardımcı fonksiyonları
export function getAchievementById(id: string): Achievement | undefined {
  return achievements.find(a => a.id === id);
}

export function getAchievementsByCategory(category: AchievementCategory): Achievement[] {
  return achievements.filter(a => a.category === category);
}

export function getPublicAchievements(): Achievement[] {
  return achievements.filter(a => !a.isSecret);
}

export function getSecretAchievements(): Achievement[] {
  return achievements.filter(a => a.isSecret);
}
