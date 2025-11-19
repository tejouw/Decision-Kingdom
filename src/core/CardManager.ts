import {
  Card,
  Condition,
  ConditionType,
  ResourceType,
  GameState
} from '../models/types.js';
import { ResourceManager } from './ResourceManager.js';

export class CardManager {
  private cards: Card[] = [];
  private playedCardIds: Set<string> = new Set();
  private triggeredEvents: string[] = [];

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
    flags: Set<string>
  ): boolean {
    if (!conditions || conditions.length === 0) {
      return true;
    }

    for (const condition of conditions) {
      if (!this.checkCondition(condition, resourceManager, turn, flags)) {
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
    flags: Set<string>
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

      default:
        return true;
    }
  }

  // Uygun kartları al
  getAvailableCards(
    resourceManager: ResourceManager,
    turn: number,
    flags: Set<string>
  ): Card[] {
    return this.cards.filter(card => {
      // Daha önce oynanan kartları çıkar (eğer tekrar oynanabilir değilse)
      // Şimdilik tüm kartlar tekrar oynanabilir

      // Koşulları kontrol et
      return this.checkConditions(card.conditions, resourceManager, turn, flags);
    });
  }

  // Sonraki kartı seç
  getNextCard(
    resourceManager: ResourceManager,
    turn: number,
    flags: Set<string>
  ): Card | null {
    // Önce triggered events'i kontrol et
    if (this.triggeredEvents.length > 0) {
      const eventId = this.triggeredEvents.shift()!;
      const eventCard = this.cards.find(c => c.id === eventId);
      if (eventCard && this.checkConditions(eventCard.conditions, resourceManager, turn, flags)) {
        return eventCard;
      }
    }

    // Uygun kartları al
    const availableCards = this.getAvailableCards(resourceManager, turn, flags);

    if (availableCards.length === 0) {
      return null;
    }

    // Önceliğe göre sırala
    const sortedCards = availableCards.sort((a, b) => {
      const priorityA = a.priority ?? 0;
      const priorityB = b.priority ?? 0;
      return priorityB - priorityA;
    });

    // En yüksek öncelikli kartlardan rastgele seç
    const highestPriority = sortedCards[0].priority ?? 0;
    const topPriorityCards = sortedCards.filter(
      card => (card.priority ?? 0) === highestPriority
    );

    const randomIndex = Math.floor(Math.random() * topPriorityCards.length);
    return topPriorityCards[randomIndex];
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

  // Oynanmış kartı işaretle
  markCardAsPlayed(cardId: string): void {
    this.playedCardIds.add(cardId);
  }

  // Kartın oynanıp oynanmadığını kontrol et
  hasBeenPlayed(cardId: string): boolean {
    return this.playedCardIds.has(cardId);
  }

  // Oynanmış kartları sıfırla
  resetPlayedCards(): void {
    this.playedCardIds.clear();
    this.triggeredEvents = [];
  }

  // Kart sayısını al
  getCardCount(): number {
    return this.cards.length;
  }

  // Kartı ID ile bul
  getCardById(cardId: string): Card | undefined {
    return this.cards.find(card => card.id === cardId);
  }

  // Tüm kartları temizle
  clearCards(): void {
    this.cards = [];
    this.playedCardIds.clear();
    this.triggeredEvents = [];
  }
}
