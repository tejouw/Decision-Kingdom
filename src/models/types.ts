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
}

// Koşul türleri
export enum ConditionType {
  RESOURCE_ABOVE = 'resource_above',
  RESOURCE_BELOW = 'resource_below',
  TURN_ABOVE = 'turn_above',
  FLAG_SET = 'flag_set',
  FLAG_NOT_SET = 'flag_not_set'
}

// Koşul
export interface Condition {
  type: ConditionType;
  resource?: ResourceType;
  value?: number;
  flag?: string;
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
}

// Oyun durumu
export interface GameState {
  resources: Resources;
  turn: number;
  era: Era;
  status: GameStatus;
  characterStates: Map<string, CharacterState>;
  flags: Set<string>;
  eventHistory: string[];
  score: number;
}

// Kaydetme verisi
export interface SaveData {
  resources: Resources;
  turn: number;
  era: Era;
  status: GameStatus;
  characterStates: [string, CharacterState][];
  flags: string[];
  eventHistory: string[];
  score: number;
  savedAt: number;
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
