import {
  Card,
  Condition,
  ConditionType,
  ResourceType,
  GameState,
  CharacterState,
  EventCategory,
  Era,
  GameFlag
} from '../models/types.js';
import { ResourceManager } from './ResourceManager.js';

export class CardManager {
  private cards: Card[] = [];
  private playedCardIds: Set<string> = new Set();
  private triggeredEvents: string[] = [];
  private cardCooldowns: Map<string, number> = new Map();

  // Nadir event şansı
  private static readonly RARE_EVENT_CHANCE = 0.05; // %5

  constructor() {}

  // Kart ekle
  addCard(card: Card): void {
    this.cards.push(card);
  }

  // Birden fazla kart ekle
  addCards(cards: Card[]): void {
    this.cards.push(...cards);
  }

  // Tüm kartları al
  getAllCards(): Card[] {
    return [...this.cards];
  }

  // Koşulları kontrol et
  private checkConditions(
    conditions: Condition[] | undefined,
    resourceManager: ResourceManager,
    turn: number,
    flags: Map<string, GameFlag>,
    characterStates: Map<string, CharacterState>,
    era: Era
  ): boolean {
    if (!conditions || conditions.length === 0) {
      return true;
    }

    for (const condition of conditions) {
      if (!this.checkCondition(condition, resourceManager, turn, flags, characterStates, era)) {
        return false;
      }
    }

    return true;
  }

  // Tek koşulu kontrol et
  private checkCondition(
    condition: Condition,
    resourceManager: ResourceManager,
    turn: number,
    flags: Map<string, GameFlag>,
    characterStates: Map<string, CharacterState>,
    era: Era
  ): boolean {
    switch (condition.type) {
      case ConditionType.RESOURCE_ABOVE:
        if (condition.resource && condition.value !== undefined) {
          return resourceManager.isAbove(condition.resource, condition.value);
        }
        return true;

      case ConditionType.RESOURCE_BELOW:
        if (condition.resource && condition.value !== undefined) {
          return resourceManager.isBelow(condition.resource, condition.value);
        }
        return true;

      case ConditionType.TURN_ABOVE:
        if (condition.value !== undefined) {
          return turn > condition.value;
        }
        return true;

      case ConditionType.TURN_BELOW:
        if (condition.value !== undefined) {
          return turn < condition.value;
        }
        return true;

      case ConditionType.FLAG_SET:
        if (condition.flag) {
          return flags.has(condition.flag);
        }
        return true;

      case ConditionType.FLAG_NOT_SET:
        if (condition.flag) {
          return !flags.has(condition.flag);
        }
        return true;

      case ConditionType.CHARACTER_INTERACTION:
        if (condition.characterId && condition.value !== undefined) {
          const charState = characterStates.get(condition.characterId);
          return charState ? charState.interactionCount >= condition.value : false;
        }
        return true;

      case ConditionType.CHARACTER_RELATIONSHIP_ABOVE:
        if (condition.characterId && condition.value !== undefined) {
          const charState = characterStates.get(condition.characterId);
          return charState ? charState.relationship > condition.value : false;
        }
        return true;

      case ConditionType.CHARACTER_RELATIONSHIP_BELOW:
        if (condition.characterId && condition.value !== undefined) {
          const charState = characterStates.get(condition.characterId);
          return charState ? charState.relationship < condition.value : false;
        }
        return true;

      case ConditionType.ERA:
        if (condition.era) {
          return era === condition.era;
        }
        return true;

      default:
        return true;
    }
  }

  // Uygun kartları al
  getAvailableCards(
    resourceManager: ResourceManager,
    turn: number,
    flags: Map<string, GameFlag>,
    characterStates: Map<string, CharacterState>,
    era: Era
  ): Card[] {
    return this.cards.filter(card => {
      // Cooldown kontrolü
      const cooldownEnd = this.cardCooldowns.get(card.id);
      if (cooldownEnd && turn < cooldownEnd) {
        return false;
      }

      // Tekrar oynanabilirlik kontrolü
      if (!card.isRepeatable && this.playedCardIds.has(card.id)) {
        return false;
      }

      // Koşulları kontrol et
      return this.checkConditions(card.conditions, resourceManager, turn, flags, characterStates, era);
    });
  }

  // Sonraki kartı seç (gelişmiş algoritma)
  getNextCard(
    resourceManager: ResourceManager,
    turn: number,
    flags: Map<string, GameFlag>,
    characterStates?: Map<string, CharacterState>,
    era?: Era
  ): Card | null {
    const charStates = characterStates || new Map();
    const currentEra = era || Era.MEDIEVAL;

    // Önce triggered events'i kontrol et
    if (this.triggeredEvents.length > 0) {
      const eventId = this.triggeredEvents.shift()!;
      const eventCard = this.cards.find(c => c.id === eventId);
      if (eventCard && this.checkConditions(eventCard.conditions, resourceManager, turn, flags, charStates, currentEra)) {
        return eventCard;
      }
    }

    // Uygun kartları al
    const availableCards = this.getAvailableCards(resourceManager, turn, flags, charStates, currentEra);

    if (availableCards.length === 0) {
      return null;
    }

    // Nadir event şansı
    const rareCards = availableCards.filter(c => c.category === EventCategory.RARE);
    if (rareCards.length > 0 && Math.random() < CardManager.RARE_EVENT_CHANCE) {
      return this.selectWeightedCard(rareCards);
    }

    // Önceliğe göre grupla
    const priorityGroups = this.groupByPriority(availableCards);
    const highestPriority = Math.max(...Array.from(priorityGroups.keys()));
    const topPriorityCards = priorityGroups.get(highestPriority) || [];

    // Ağırlıklı seçim yap
    return this.selectWeightedCard(topPriorityCards);
  }

  // Kartları önceliğe göre grupla
  private groupByPriority(cards: Card[]): Map<number, Card[]> {
    const groups = new Map<number, Card[]>();

    for (const card of cards) {
      const priority = card.priority ?? 0;
      if (!groups.has(priority)) {
        groups.set(priority, []);
      }
      groups.get(priority)!.push(card);
    }

    return groups;
  }

  // Ağırlıklı rastgele seçim
  private selectWeightedCard(cards: Card[]): Card {
    if (cards.length === 0) {
      throw new Error('No cards to select from');
    }

    if (cards.length === 1) {
      return cards[0];
    }

    // Toplam ağırlık hesapla
    const totalWeight = cards.reduce((sum, card) => sum + (card.weight ?? 1), 0);

    // Rastgele değer seç
    let random = Math.random() * totalWeight;

    // Ağırlığa göre kart seç
    for (const card of cards) {
      random -= (card.weight ?? 1);
      if (random <= 0) {
        return card;
      }
    }

    // Fallback
    return cards[cards.length - 1];
  }

  // Triggered event ekle
  addTriggeredEvent(eventId: string): void {
    if (!this.triggeredEvents.includes(eventId)) {
      this.triggeredEvents.push(eventId);
    }
  }

  // Triggered events ekle
  addTriggeredEvents(eventIds: string[]): void {
    for (const eventId of eventIds) {
      this.addTriggeredEvent(eventId);
    }
  }

  // Oynanmış kartı işaretle ve cooldown ayarla
  markCardAsPlayed(cardId: string, turn: number): void {
    this.playedCardIds.add(cardId);

    // Cooldown ayarla
    const card = this.getCardById(cardId);
    if (card && card.cooldown) {
      this.cardCooldowns.set(cardId, turn + card.cooldown);
    }
  }

  // Kartın oynanıp oynanmadığını kontrol et
  hasBeenPlayed(cardId: string): boolean {
    return this.playedCardIds.has(cardId);
  }

  // Oynanmış kartları sıfırla
  resetPlayedCards(): void {
    this.playedCardIds.clear();
    this.triggeredEvents = [];
    this.cardCooldowns.clear();
  }

  // Kart sayısını al
  getCardCount(): number {
    return this.cards.length;
  }

  // Kartı ID ile bul
  getCardById(cardId: string): Card | undefined {
    return this.cards.find(card => card.id === cardId);
  }

  // Kategoriye göre kartları al
  getCardsByCategory(category: EventCategory): Card[] {
    return this.cards.filter(card => card.category === category);
  }

  // Karaktere göre kartları al
  getCardsByCharacter(characterId: string): Card[] {
    return this.cards.filter(card => card.character.id === characterId);
  }

  // Tüm kartları temizle
  clearCards(): void {
    this.cards = [];
    this.playedCardIds.clear();
    this.triggeredEvents = [];
    this.cardCooldowns.clear();
  }

  // Cooldown'ları al (save için)
  getCooldowns(): Map<string, number> {
    return new Map(this.cardCooldowns);
  }

  // Cooldown'ları yükle (load için)
  loadCooldowns(cooldowns: Map<string, number>): void {
    this.cardCooldowns = new Map(cooldowns);
  }

  // Oynanmış kartları al (save için)
  getPlayedCardIds(): Set<string> {
    return new Set(this.playedCardIds);
  }

  // Oynanmış kartları yükle (load için)
  loadPlayedCardIds(ids: Set<string>): void {
    this.playedCardIds = new Set(ids);
  }
}
