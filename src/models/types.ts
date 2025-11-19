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
