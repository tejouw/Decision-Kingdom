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
  RESOURCE_DEFAULT,
  RESOURCE_NAMES
} from '../models/types.js';
import { ResourceManager } from './ResourceManager.js';
import { CardManager } from './CardManager.js';

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
      flags: new Set(),
      eventHistory: [],
      score: 0
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
      this.state.flags
    );

    if (card) {
      this.currentCard = card;
      this.emit('newCard', card);
    } else {
      // Kart kalmadı - zafer!
      this.state.status = GameStatus.VICTORY;
      this.emit('victory', { turn: this.state.turn, score: this.state.score });
    }
  }

  // Seçim yap
  makeChoice(isLeft: boolean): void {
    if (!this.currentCard || this.state.status !== GameStatus.PLAYING) {
      return;
    }

    const choice = isLeft ? this.currentCard.leftChoice : this.currentCard.rightChoice;

    // Efektleri uygula
    this.resourceManager.applyEffects(choice.effects);

    // Event history'ye ekle
    this.state.eventHistory.push(this.currentCard.id);

    // Karakter durumunu güncelle
    this.updateCharacterState(this.currentCard.character.id);

    // Triggered events ekle
    if (choice.triggeredEvents) {
      this.cardManager.addTriggeredEvents(choice.triggeredEvents);
    }

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
  private updateCharacterState(characterId: string): void {
    let charState = this.state.characterStates.get(characterId);

    if (!charState) {
      charState = {
        characterId,
        interactionCount: 0,
        lastInteractionTurn: 0,
        relationship: 0
      };
    }

    charState.interactionCount++;
    charState.lastInteractionTurn = this.state.turn;

    this.state.characterStates.set(characterId, charState);
  }

  // Skor hesapla
  private calculateScore(): number {
    const resources = this.resourceManager.getAllResources();
    const balance = Object.values(resources).reduce((sum, val) => {
      // Dengeye göre skor (50'ye yakınlık)
      return sum + (50 - Math.abs(val - 50));
    }, 0);

    return this.state.turn * 10 + balance;
  }

  // Game over
  private handleGameOver(resource: ResourceType, value: number): void {
    this.state.status = GameStatus.GAME_OVER;
    this.state.resources = this.resourceManager.getAllResources();

    const reason = value <= 0
      ? `${RESOURCE_NAMES[resource]} tükendi!`
      : `${RESOURCE_NAMES[resource]} çok yükseldi!`;

    this.emit('gameOver', {
      resource,
      value,
      reason,
      turn: this.state.turn,
      score: this.state.score
    });
  }

  // Flag ayarla
  setFlag(flag: string): void {
    this.state.flags.add(flag);
    this.emit('flagSet', flag);
  }

  // Flag kaldır
  removeFlag(flag: string): void {
    this.state.flags.delete(flag);
    this.emit('flagRemoved', flag);
  }

  // Flag kontrol
  hasFlag(flag: string): boolean {
    return this.state.flags.has(flag);
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
      flags: Array.from(this.state.flags),
      eventHistory: this.state.eventHistory,
      score: this.state.score,
      savedAt: Date.now()
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
        flags: new Set(saveData.flags),
        eventHistory: saveData.eventHistory,
        score: saveData.score
      };

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
}
