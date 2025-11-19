// Kaynak türleri
export enum ResourceType {
  GOLD = 'gold',
  HAPPINESS = 'happiness',
  MILITARY = 'military',
  FAITH = 'faith'
}

// Oyun durumu enum'u
export enum GameStatus {
  PLAYING = 'playing',
  PAUSED = 'paused',
  GAME_OVER = 'game_over',
  VICTORY = 'victory'
}

// Çağ enum'u
export enum Era {
  MEDIEVAL = 'medieval',
  RENAISSANCE = 'renaissance',
  INDUSTRIAL = 'industrial',
  MODERN = 'modern',
  FUTURE = 'future'
}

// Kaynak efekti
export interface ResourceEffect {
  resource: ResourceType;
  min: number;
  max: number;
}

// Seçim
export interface Choice {
  text: string;
  effects: ResourceEffect[];
  triggeredEvents?: string[];
  setFlags?: string[];
  removeFlags?: string[];
  relationshipChange?: number; // Karakter ilişkisine etki (-100 to 100)
}

// Koşul türleri
export enum ConditionType {
  RESOURCE_ABOVE = 'resource_above',
  RESOURCE_BELOW = 'resource_below',
  TURN_ABOVE = 'turn_above',
  TURN_BELOW = 'turn_below',
  FLAG_SET = 'flag_set',
  FLAG_NOT_SET = 'flag_not_set',
  CHARACTER_INTERACTION = 'character_interaction',
  CHARACTER_RELATIONSHIP_ABOVE = 'character_relationship_above',
  CHARACTER_RELATIONSHIP_BELOW = 'character_relationship_below',
  ERA = 'era'
}

// Event kategorileri
export enum EventCategory {
  STORY = 'story',           // Ana hikaye olayları
  RANDOM = 'random',         // Rastgele krizler
  CHARACTER = 'character',   // Karakter spesifik
  CHAIN = 'chain',           // Zincirleme olaylar
  RARE = 'rare'              // Nadir olaylar
}

// Flag türleri
export enum FlagType {
  PERSISTENT = 'persistent', // Oyun boyunca kalıcı
  TEMPORARY = 'temporary'    // Oturum boyunca
}

// Koşul
export interface Condition {
  type: ConditionType;
  resource?: ResourceType;
  value?: number;
  flag?: string;
  characterId?: string;
  era?: Era;
}

// Flag verisi
export interface GameFlag {
  name: string;
  type: FlagType;
  value: boolean;
  setAt?: number; // Tur numarası
}

// Karakter
export interface Character {
  id: string;
  name: string;
  title: string;
  avatar?: string;
}

// Kart
export interface Card {
  id: string;
  character: Character;
  text: string;
  leftChoice: Choice;
  rightChoice: Choice;
  conditions?: Condition[];
  priority?: number;
  weight?: number;           // Seçilme ağırlığı (default: 1)
  category?: EventCategory;  // Event kategorisi
  isRepeatable?: boolean;    // Tekrar oynanabilir mi?
  cooldown?: number;         // Kaç tur sonra tekrar çıkabilir
  memoryText?: string;       // Karakter hatırlama metni
  era?: Era;                 // Kartın ait olduğu dönem
}

// Kaynaklar
export interface Resources {
  [ResourceType.GOLD]: number;
  [ResourceType.HAPPINESS]: number;
  [ResourceType.MILITARY]: number;
  [ResourceType.FAITH]: number;
}

// Karakter durumu
export interface CharacterState {
  characterId: string;
  interactionCount: number;
  lastInteractionTurn: number;
  relationship: number; // -100 to 100
  flags: string[];      // Karakter spesifik flagler
  decisions: string[];  // Karakter ile ilgili alınan kararlar (kart ID'leri)
}

// Oyun durumu
export interface GameState {
  resources: Resources;
  turn: number;
  era: Era;
  status: GameStatus;
  characterStates: Map<string, CharacterState>;
  flags: Map<string, GameFlag>;
  eventHistory: string[];
  score: number;
  cardCooldowns: Map<string, number>; // cardId -> kullanılabilir olacağı tur
  totalCardsPlayed: number;
}

// Kaydetme verisi
export interface SaveData {
  resources: Resources;
  turn: number;
  era: Era;
  status: GameStatus;
  characterStates: [string, CharacterState][];
  flags: [string, GameFlag][];
  eventHistory: string[];
  score: number;
  savedAt: number;
  cardCooldowns: [string, number][];
  totalCardsPlayed: number;
}

// Kaynak sabitleri
export const RESOURCE_MIN = 0;
export const RESOURCE_MAX = 100;
export const RESOURCE_DEFAULT = 50;

// Kaynak isimleri (Türkçe)
export const RESOURCE_NAMES: Record<ResourceType, string> = {
  [ResourceType.GOLD]: 'Hazine',
  [ResourceType.HAPPINESS]: 'Mutluluk',
  [ResourceType.MILITARY]: 'Ordu',
  [ResourceType.FAITH]: 'İnanç'
};

// Kaynak ikonları
export const RESOURCE_ICONS: Record<ResourceType, string> = {
  [ResourceType.GOLD]: '💰',
  [ResourceType.HAPPINESS]: '😊',
  [ResourceType.MILITARY]: '⚔️',
  [ResourceType.FAITH]: '🙏'
};

// Bitiş türleri
export enum EndingType {
  PEACEFUL = 'peaceful',           // Barışçıl yönetim
  MILITARY = 'military',           // Askeri diktatörlük
  THEOCRATIC = 'theocratic',       // Teokratik yönetim
  MERCHANT = 'merchant',           // Tüccar oligarşisi
  BALANCED = 'balanced',           // Dengeli yönetim
  TYRANNY = 'tyranny',             // Zorba sultan
  BELOVED = 'beloved',             // Sevilen sultan
  REVOLUTIONARY = 'revolutionary', // Devrimci sultan
  SCHOLAR = 'scholar',             // Bilge sultan
  CONQUEROR = 'conqueror'          // Fatih sultan
}

// Bitiş verisi
export interface Ending {
  type: EndingType;
  title: string;
  description: string;
  conditions: EndingCondition[];
  priority: number; // Yüksek öncelik önce kontrol edilir
}

// Bitiş koşulu
export interface EndingCondition {
  type: 'resource_above' | 'resource_below' | 'flag_set' | 'flag_not_set' | 'turn_above' | 'character_relationship';
  resource?: ResourceType;
  value?: number;
  flag?: string;
  characterId?: string;
}

// Hikaye dalı
export interface StoryBranch {
  id: string;
  name: string;
  description: string;
  requiredFlags: string[];
  excludedFlags: string[];
  endingType: EndingType;
}

// ============================================
// FAZ 4: META SİSTEM TİPLERİ
// ============================================

// Başarım kategorileri
export enum AchievementCategory {
  SURVIVAL = 'survival',     // Hayatta kalma başarımları
  EXTREME = 'extreme',       // Ekstrem stratejiler
  STORY = 'story',           // Hikaye tamamlama
  SECRET = 'secret',         // Gizli başarımlar
  CHARACTER = 'character'    // Karakter etkileşimleri
}

// Başarım tanımı
export interface Achievement {
  id: string;
  name: string;
  description: string;
  category: AchievementCategory;
  icon: string;
  ppReward: number;           // Kazanılan Prestige Points
  isSecret: boolean;          // Gizli mi?
  condition: AchievementCondition;
}

// Başarım koşulu
export interface AchievementCondition {
  type: AchievementConditionType;
  value?: number;
  resource?: ResourceType;
  characterId?: string;
  flag?: string;
  endingType?: EndingType;
  era?: Era;
}

// Başarım koşul türleri
export enum AchievementConditionType {
  TURNS_SURVIVED = 'turns_survived',
  TOTAL_CARDS_PLAYED = 'total_cards_played',
  RESOURCE_REACHED = 'resource_reached',
  RESOURCE_NEVER_BELOW = 'resource_never_below',
  CHARACTER_INTERACTION_COUNT = 'character_interaction_count',
  FLAG_SET = 'flag_set',
  ENDING_REACHED = 'ending_reached',
  GAMES_COMPLETED = 'games_completed',
  TOTAL_PP_EARNED = 'total_pp_earned',
  ERA_UNLOCKED = 'era_unlocked',
  ALL_CHARACTERS_MET = 'all_characters_met',
  SPECIFIC_SCORE = 'specific_score'
}

// Kazanılmış başarım
export interface UnlockedAchievement {
  achievementId: string;
  unlockedAt: number;         // Timestamp
  gameNumber: number;         // Hangi oyunda kazanıldı
}

// Dönem kilidi bilgisi
export interface EraUnlock {
  era: Era;
  requiredPP: number;
  isUnlocked: boolean;
  unlockedAt?: number;
}

// Dönem PP gereksinimleri
export const ERA_UNLOCK_REQUIREMENTS: Record<Era, number> = {
  [Era.MEDIEVAL]: 0,       // Başlangıçta açık
  [Era.RENAISSANCE]: 100,
  [Era.INDUSTRIAL]: 250,
  [Era.MODERN]: 500,
  [Era.FUTURE]: 1000
};

// Oyun istatistikleri
export interface GameStatistics {
  // Genel istatistikler
  totalGamesPlayed: number;
  totalCardsPlayed: number;
  totalTurnsSurvived: number;
  longestRun: number;
  highestScore: number;

  // Kaynak istatistikleri
  averageGold: number;
  averageHappiness: number;
  averageMilitary: number;
  averageFaith: number;

  // Ölüm sebepleri
  deathsByResource: Record<ResourceType, number>;
  deathsByExcess: Record<ResourceType, number>;

  // Bitiş istatistikleri
  endingsReached: Record<EndingType, number>;

  // Karakter istatistikleri
  characterInteractions: Record<string, number>;
  favoriteCharacter: string;

  // Dönem istatistikleri
  gamesPerEra: Record<Era, number>;
  favoriteEra: Era;

  // PP istatistikleri
  totalPPEarned: number;
  ppSpent: number;
  currentPP: number;

  // Başarım istatistikleri
  achievementsUnlocked: number;
  totalAchievements: number;

  // Zaman istatistikleri
  firstPlayedAt: number;
  lastPlayedAt: number;
  totalPlayTime: number;       // Milisaniye cinsinden
}

// Oturum istatistikleri (tek oyun için)
export interface SessionStatistics {
  gameNumber: number;
  era: Era;
  turnsSurvived: number;
  cardsPlayed: number;
  score: number;
  ppEarned: number;
  endingReached: EndingType;
  deathReason?: string;
  charactersMet: string[];
  flagsSet: string[];
  duration: number;            // Milisaniye
  startedAt: number;
  endedAt: number;
  resourceHistory: Resources[];
  newAchievements: string[];
}

// Meta kaydetme verisi
export interface MetaSaveData {
  statistics: GameStatistics;
  unlockedAchievements: UnlockedAchievement[];
  unlockedEras: Era[];
  sessionHistory: SessionStatistics[];
  version: number;
  savedAt: number;
}

// PP hesaplama sonucu
export interface PPCalculation {
  base: number;                // Temel PP (turlar)
  bonuses: PPBonus[];          // Bonuslar
  total: number;               // Toplam
}

// PP bonus türleri
export interface PPBonus {
  type: string;
  description: string;
  amount: number;
}

// Oyun sonu raporu
export interface GameEndReport {
  sessionStats: SessionStatistics;
  ppCalculation: PPCalculation;
  newAchievements: Achievement[];
  newEraUnlocks: Era[];
  previousPP: number;
  newTotalPP: number;
}
