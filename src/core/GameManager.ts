import {
  GameState,
  GameStatus,
  Era,
  Resources,
  ResourceType,
  CharacterState,
  SaveData,
  Card,
  Choice,
  GameFlag,
  FlagType,
  Ending,
  EndingType,
  EndingCondition,
  RESOURCE_DEFAULT,
  RESOURCE_NAMES
} from '../models/types.js';
import { ResourceManager } from './ResourceManager.js';
import { CardManager } from './CardManager.js';
import { endings, defaultEnding } from '../data/endings.js';

export type GameEventCallback = (event: string, data?: any) => void;

export class GameManager {
  private resourceManager: ResourceManager;
  private cardManager: CardManager;
  private state: GameState;
  private eventCallbacks: GameEventCallback[] = [];
  private currentCard: Card | null = null;

  private static readonly SAVE_KEY = 'decision_kingdom_save';

  constructor() {
    this.resourceManager = new ResourceManager();
    this.cardManager = new CardManager();
    this.state = this.createInitialState();

    // Resource manager callback'lerini ayarla
    this.resourceManager.onResourceChange((resource, oldValue, newValue, change) => {
      this.emit('resourceChange', { resource, oldValue, newValue, change });
    });

    this.resourceManager.onGameOver((resource, value) => {
      this.handleGameOver(resource, value);
    });
  }

  // Başlangıç durumu oluştur
  private createInitialState(): GameState {
    return {
      resources: this.resourceManager.getAllResources(),
      turn: 1,
      era: Era.MEDIEVAL,
      status: GameStatus.PLAYING,
      characterStates: new Map(),
      flags: new Map(),
      eventHistory: [],
      score: 0,
      cardCooldowns: new Map(),
      totalCardsPlayed: 0
    };
  }

  // Oyunu başlat
  startGame(): void {
    this.resourceManager.reset();
    this.cardManager.resetPlayedCards();
    this.state = this.createInitialState();
    this.emit('gameStart');
    this.nextCard();
  }

  // Sonraki kart
  nextCard(): void {
    const card = this.cardManager.getNextCard(
      this.resourceManager,
      this.state.turn,
      this.state.flags,
      this.state.characterStates,
      this.state.era
    );

    if (card) {
      this.currentCard = card;

      // Karakter hafızası varsa metni güncelle
      const modifiedCard = this.applyCharacterMemory(card);

      this.emit('newCard', modifiedCard);
    } else {
      // Kart kalmadı - zafer!
      this.state.status = GameStatus.VICTORY;
      const ending = this.determineEnding();
      this.emit('victory', {
        turn: this.state.turn,
        score: this.state.score,
        ending
      });
    }
  }

  // Karakter hafızasını uygula
  private applyCharacterMemory(card: Card): Card {
    const charState = this.state.characterStates.get(card.character.id);

    if (!charState || charState.interactionCount === 0) {
      return card;
    }

    // Hafıza metni varsa ve karakter daha önce etkileşime girdiyse
    if (card.memoryText && charState.interactionCount > 0) {
      return {
        ...card,
        text: `${card.memoryText}\n\n${card.text}`
      };
    }

    // İlişki durumuna göre ek metin
    let relationshipPrefix = '';
    if (charState.relationship > 50) {
      relationshipPrefix = `${card.character.name} size güvenle yaklaşıyor. `;
    } else if (charState.relationship < -50) {
      relationshipPrefix = `${card.character.name} size mesafeli duruyor. `;
    } else if (charState.interactionCount >= 3) {
      relationshipPrefix = `${card.character.name} sizi tanıyor. `;
    }

    if (relationshipPrefix) {
      return {
        ...card,
        text: relationshipPrefix + card.text
      };
    }

    return card;
  }

  // Seçim yap
  makeChoice(isLeft: boolean): void {
    if (!this.currentCard || this.state.status !== GameStatus.PLAYING) {
      return;
    }

    const choice = isLeft ? this.currentCard.leftChoice : this.currentCard.rightChoice;

    // Efektleri uygula (zorluk ölçeklemesi ile)
    this.resourceManager.applyEffects(choice.effects, this.state.turn);

    // Event history'ye ekle
    this.state.eventHistory.push(this.currentCard.id);

    // Kartı oynanmış olarak işaretle
    this.cardManager.markCardAsPlayed(this.currentCard.id, this.state.turn);

    // Karakter durumunu güncelle
    this.updateCharacterState(this.currentCard.character.id, choice, this.currentCard.id);

    // Flag'leri işle
    this.processChoiceFlags(choice);

    // Triggered events ekle
    if (choice.triggeredEvents) {
      this.cardManager.addTriggeredEvents(choice.triggeredEvents);
    }

    // Toplam oynanan kart sayısını artır
    this.state.totalCardsPlayed++;

    // Turu artır
    this.state.turn++;

    // Skoru güncelle
    this.state.score = this.calculateScore();

    // Resources'u state'e kaydet
    this.state.resources = this.resourceManager.getAllResources();

    // Seçim eventini emit et
    this.emit('choiceMade', {
      card: this.currentCard,
      choice,
      isLeft,
      turn: this.state.turn
    });

    // Game over kontrolü
    if (this.resourceManager.isGameOver()) {
      return; // handleGameOver zaten çağrılacak
    }

    // Sonraki kart
    this.nextCard();
  }

  // Sol seçim
  chooseLeft(): void {
    this.makeChoice(true);
  }

  // Sağ seçim
  chooseRight(): void {
    this.makeChoice(false);
  }

  // Karakter durumunu güncelle
  private updateCharacterState(characterId: string, choice: Choice, cardId: string): void {
    let charState = this.state.characterStates.get(characterId);

    if (!charState) {
      charState = {
        characterId,
        interactionCount: 0,
        lastInteractionTurn: 0,
        relationship: 0,
        flags: [],
        decisions: []
      };
    }

    charState.interactionCount++;
    charState.lastInteractionTurn = this.state.turn;
    charState.decisions.push(cardId);

    // İlişki değişimini uygula
    if (choice.relationshipChange) {
      charState.relationship = Math.max(-100, Math.min(100,
        charState.relationship + choice.relationshipChange
      ));
    }

    this.state.characterStates.set(characterId, charState);

    // Karakter etkileşim eventini emit et
    this.emit('characterInteraction', {
      characterId,
      interactionCount: charState.interactionCount,
      relationship: charState.relationship
    });
  }

  // Seçim flag'lerini işle
  private processChoiceFlags(choice: Choice): void {
    // Flag'leri ayarla
    if (choice.setFlags) {
      for (const flagName of choice.setFlags) {
        this.setFlag(flagName, FlagType.PERSISTENT);
      }
    }

    // Flag'leri kaldır
    if (choice.removeFlags) {
      for (const flagName of choice.removeFlags) {
        this.removeFlag(flagName);
      }
    }
  }

  // Skor hesapla
  private calculateScore(): number {
    const resources = this.resourceManager.getAllResources();
    const balance = Object.values(resources).reduce((sum, val) => {
      // Dengeye göre skor (50'ye yakınlık)
      return sum + (50 - Math.abs(val - 50));
    }, 0);

    // Tur bonusu + denge bonusu + karakter çeşitliliği bonusu
    const characterBonus = this.state.characterStates.size * 5;
    return this.state.turn * 10 + balance + characterBonus;
  }

  // Game over
  private handleGameOver(resource: ResourceType, value: number): void {
    this.state.status = GameStatus.GAME_OVER;
    this.state.resources = this.resourceManager.getAllResources();

    const reason = value <= 0
      ? `${RESOURCE_NAMES[resource]} tükendi!`
      : `${RESOURCE_NAMES[resource]} çok yükseldi!`;

    // Bitişi belirle
    const ending = this.determineEnding();

    this.emit('gameOver', {
      resource,
      value,
      reason,
      turn: this.state.turn,
      score: this.state.score,
      ending
    });
  }

  // Bitişi belirle
  determineEnding(): Ending {
    // Bitişleri önceliğe göre sırala (yüksekten düşüğe)
    const sortedEndings = [...endings].sort((a, b) => b.priority - a.priority);

    for (const ending of sortedEndings) {
      if (this.checkEndingConditions(ending.conditions)) {
        return ending;
      }
    }

    return defaultEnding;
  }

  // Bitiş koşullarını kontrol et
  private checkEndingConditions(conditions: EndingCondition[]): boolean {
    if (conditions.length === 0) {
      return false;
    }

    for (const condition of conditions) {
      if (!this.checkEndingCondition(condition)) {
        return false;
      }
    }

    return true;
  }

  // Tek bitiş koşulunu kontrol et
  private checkEndingCondition(condition: EndingCondition): boolean {
    switch (condition.type) {
      case 'resource_above':
        if (condition.resource && condition.value !== undefined) {
          return this.resourceManager.isAbove(condition.resource, condition.value);
        }
        return true;

      case 'resource_below':
        if (condition.resource && condition.value !== undefined) {
          return this.resourceManager.isBelow(condition.resource, condition.value);
        }
        return true;

      case 'flag_set':
        if (condition.flag) {
          return this.state.flags.has(condition.flag);
        }
        return true;

      case 'flag_not_set':
        if (condition.flag) {
          return !this.state.flags.has(condition.flag);
        }
        return true;

      case 'turn_above':
        if (condition.value !== undefined) {
          return this.state.turn > condition.value;
        }
        return true;

      case 'character_relationship':
        if (condition.characterId && condition.value !== undefined) {
          const charState = this.state.characterStates.get(condition.characterId);
          return charState ? charState.relationship > condition.value : false;
        }
        return true;

      default:
        return true;
    }
  }

  // Mevcut bitiş durumunu al (oyun sırasında)
  getCurrentEnding(): Ending {
    return this.determineEnding();
  }

  // Flag ayarla (geliştirilmiş)
  setFlag(flagName: string, type: FlagType = FlagType.PERSISTENT): void {
    const flag: GameFlag = {
      name: flagName,
      type,
      value: true,
      setAt: this.state.turn
    };
    this.state.flags.set(flagName, flag);
    this.emit('flagSet', flag);
  }

  // Flag kaldır
  removeFlag(flagName: string): void {
    this.state.flags.delete(flagName);
    this.emit('flagRemoved', flagName);
  }

  // Flag kontrol
  hasFlag(flagName: string): boolean {
    return this.state.flags.has(flagName);
  }

  // Flag al
  getFlag(flagName: string): GameFlag | undefined {
    return this.state.flags.get(flagName);
  }

  // Tüm flag'leri al
  getAllFlags(): Map<string, GameFlag> {
    return new Map(this.state.flags);
  }

  // Geçici flag'leri temizle
  clearTemporaryFlags(): void {
    for (const [name, flag] of this.state.flags) {
      if (flag.type === FlagType.TEMPORARY) {
        this.state.flags.delete(name);
      }
    }
  }

  // Karakter durumunu al
  getCharacterState(characterId: string): CharacterState | undefined {
    return this.state.characterStates.get(characterId);
  }

  // Karakter ilişkisini değiştir
  modifyCharacterRelationship(characterId: string, change: number): void {
    const charState = this.state.characterStates.get(characterId);
    if (charState) {
      charState.relationship = Math.max(-100, Math.min(100,
        charState.relationship + change
      ));
      this.emit('relationshipChange', {
        characterId,
        newRelationship: charState.relationship,
        change
      });
    }
  }

  // Karakter flag'i ayarla
  setCharacterFlag(characterId: string, flag: string): void {
    const charState = this.state.characterStates.get(characterId);
    if (charState && !charState.flags.includes(flag)) {
      charState.flags.push(flag);
    }
  }

  // Karakter flag'i kontrol
  hasCharacterFlag(characterId: string, flag: string): boolean {
    const charState = this.state.characterStates.get(characterId);
    return charState ? charState.flags.includes(flag) : false;
  }

  // Oyunu duraklat
  pause(): void {
    if (this.state.status === GameStatus.PLAYING) {
      this.state.status = GameStatus.PAUSED;
      this.emit('gamePaused');
    }
  }

  // Oyuna devam et
  resume(): void {
    if (this.state.status === GameStatus.PAUSED) {
      this.state.status = GameStatus.PLAYING;
      this.emit('gameResumed');
    }
  }

  // Kaydet
  save(): void {
    const saveData: SaveData = {
      resources: this.resourceManager.getAllResources(),
      turn: this.state.turn,
      era: this.state.era,
      status: this.state.status,
      characterStates: Array.from(this.state.characterStates.entries()),
      flags: Array.from(this.state.flags.entries()),
      eventHistory: this.state.eventHistory,
      score: this.state.score,
      savedAt: Date.now(),
      cardCooldowns: Array.from(this.cardManager.getCooldowns().entries()),
      totalCardsPlayed: this.state.totalCardsPlayed
    };

    try {
      localStorage.setItem(GameManager.SAVE_KEY, JSON.stringify(saveData));
      this.emit('gameSaved', saveData);
    } catch (error) {
      this.emit('saveError', error);
    }
  }

  // Yükle
  load(): boolean {
    try {
      const savedJson = localStorage.getItem(GameManager.SAVE_KEY);
      if (!savedJson) {
        return false;
      }

      const saveData: SaveData = JSON.parse(savedJson);

      this.resourceManager.loadFromJSON(saveData.resources);
      this.state = {
        resources: saveData.resources,
        turn: saveData.turn,
        era: saveData.era,
        status: saveData.status,
        characterStates: new Map(saveData.characterStates),
        flags: new Map(saveData.flags),
        eventHistory: saveData.eventHistory,
        score: saveData.score,
        cardCooldowns: new Map(saveData.cardCooldowns || []),
        totalCardsPlayed: saveData.totalCardsPlayed || 0
      };

      // CardManager state'ini yükle
      this.cardManager.loadCooldowns(this.state.cardCooldowns);

      this.emit('gameLoaded', saveData);
      this.nextCard();
      return true;
    } catch (error) {
      this.emit('loadError', error);
      return false;
    }
  }

  // Kayıt var mı?
  hasSave(): boolean {
    return localStorage.getItem(GameManager.SAVE_KEY) !== null;
  }

  // Kaydı sil
  deleteSave(): void {
    localStorage.removeItem(GameManager.SAVE_KEY);
    this.emit('saveDeleted');
  }

  // Kartları yükle
  loadCards(cards: Card[]): void {
    this.cardManager.addCards(cards);
    this.emit('cardsLoaded', cards.length);
  }

  // Event callback ekle
  on(callback: GameEventCallback): void {
    this.eventCallbacks.push(callback);
  }

  // Event emit et
  private emit(event: string, data?: any): void {
    for (const callback of this.eventCallbacks) {
      callback(event, data);
    }
  }

  // Getter'lar
  getState(): GameState {
    return { ...this.state };
  }

  getResources(): Resources {
    return this.resourceManager.getAllResources();
  }

  getTurn(): number {
    return this.state.turn;
  }

  getStatus(): GameStatus {
    return this.state.status;
  }

  getScore(): number {
    return this.state.score;
  }

  getCurrentCard(): Card | null {
    return this.currentCard;
  }

  getResourceManager(): ResourceManager {
    return this.resourceManager;
  }

  getCardManager(): CardManager {
    return this.cardManager;
  }

  getEra(): Era {
    return this.state.era;
  }

  getTotalCardsPlayed(): number {
    return this.state.totalCardsPlayed;
  }

  getAllCharacterStates(): Map<string, CharacterState> {
    return new Map(this.state.characterStates);
  }
}
