import {
  Card,
  Resources,
  ResourceType,
  RESOURCE_NAMES,
  RESOURCE_ICONS,
  GameStatus,
  Choice,
  ResourceEffect
} from '../models/types.js';
import { GameManager } from '../core/GameManager.js';
import { SwipeHandler } from './SwipeHandler.js';

export class GameUI {
  private gameManager: GameManager;
  private swipeHandler: SwipeHandler | null = null;

  // DOM Elements
  private container: HTMLElement;
  private resourceBars: Map<ResourceType, HTMLElement> = new Map();
  private cardElement: HTMLElement | null = null;
  private choicePreview: HTMLElement | null = null;
  private turnDisplay: HTMLElement | null = null;
  private scoreDisplay: HTMLElement | null = null;

  constructor(containerId: string, gameManager: GameManager) {
    const container = document.getElementById(containerId);
    if (!container) {
      throw new Error(`Container with id "${containerId}" not found`);
    }

    this.container = container;
    this.gameManager = gameManager;

    this.setupEventListeners();
    this.render();
  }

  private setupEventListeners(): void {
    this.gameManager.on((event, data) => {
      switch (event) {
        case 'newCard':
          this.renderCard(data as Card);
          break;
        case 'resourceChange':
          this.updateResourceBar(data.resource, data.newValue);
          this.animateResourceChange(data.resource, data.change);
          break;
        case 'gameOver':
          this.showGameOver(data);
          break;
        case 'victory':
          this.showVictory(data);
          break;
        case 'gameStart':
          this.hideOverlays();
          this.updateAllResources();
          break;
        case 'choiceMade':
          this.updateTurnDisplay();
          this.updateScoreDisplay();
          break;
      }
    });
  }

  private render(): void {
    this.container.innerHTML = `
      <div class="game-container">
        <div class="header">
          <div class="turn-score">
            <span id="turn-display">Tur: 1</span>
            <span id="score-display">Skor: 0</span>
          </div>
          <div class="menu-buttons">
            <button id="save-btn" class="menu-btn">Kaydet</button>
            <button id="menu-btn" class="menu-btn">Menü</button>
          </div>
        </div>

        <div class="resources">
          ${this.renderResourceBars()}
        </div>

        <div class="card-area">
          <div class="choice-preview" id="choice-preview"></div>
          <div class="card" id="card">
            <div class="card-content">
              <div class="character-avatar" id="character-avatar"></div>
              <div class="character-name" id="character-name"></div>
              <div class="character-title" id="character-title"></div>
              <div class="card-text" id="card-text"></div>
            </div>
          </div>
          <div class="choice-hints">
            <div class="hint hint-left" id="hint-left">← Sol</div>
            <div class="hint hint-right" id="hint-right">Sağ →</div>
          </div>
        </div>

        <div class="choice-buttons">
          <button id="btn-left" class="choice-btn btn-left">Sol Seçim</button>
          <button id="btn-right" class="choice-btn btn-right">Sağ Seçim</button>
        </div>

        <div class="overlay" id="game-over-overlay" style="display: none;">
          <div class="overlay-content">
            <h2 id="overlay-title">Oyun Bitti</h2>
            <p id="overlay-message"></p>
            <p id="overlay-stats"></p>
            <button id="restart-btn" class="primary-btn">Yeniden Başla</button>
          </div>
        </div>

        <div class="overlay" id="menu-overlay" style="display: none;">
          <div class="overlay-content">
            <h2>Menü</h2>
            <button id="resume-btn" class="primary-btn">Devam Et</button>
            <button id="new-game-btn" class="secondary-btn">Yeni Oyun</button>
            <button id="load-btn" class="secondary-btn">Yükle</button>
          </div>
        </div>
      </div>
    `;

    this.cacheElements();
    this.setupButtonListeners();
    this.setupSwipeHandler();
  }

  private renderResourceBars(): string {
    const resources = Object.values(ResourceType);
    return resources.map(type => `
      <div class="resource-bar-container">
        <div class="resource-icon">${RESOURCE_ICONS[type]}</div>
        <div class="resource-bar">
          <div class="resource-fill" id="resource-${type}" style="width: 50%"></div>
        </div>
        <div class="resource-value" id="resource-value-${type}">50</div>
      </div>
    `).join('');
  }

  private cacheElements(): void {
    this.cardElement = document.getElementById('card');
    this.choicePreview = document.getElementById('choice-preview');
    this.turnDisplay = document.getElementById('turn-display');
    this.scoreDisplay = document.getElementById('score-display');

    // Resource bars
    for (const type of Object.values(ResourceType)) {
      const bar = document.getElementById(`resource-${type}`);
      if (bar) {
        this.resourceBars.set(type, bar);
      }
    }
  }

  private setupButtonListeners(): void {
    // Choice buttons
    document.getElementById('btn-left')?.addEventListener('click', () => {
      this.swipeHandler?.triggerSwipe('left');
    });

    document.getElementById('btn-right')?.addEventListener('click', () => {
      this.swipeHandler?.triggerSwipe('right');
    });

    // Menu buttons
    document.getElementById('save-btn')?.addEventListener('click', () => {
      this.gameManager.save();
      alert('Oyun kaydedildi!');
    });

    document.getElementById('menu-btn')?.addEventListener('click', () => {
      this.gameManager.pause();
      this.showMenu();
    });

    // Overlay buttons
    document.getElementById('restart-btn')?.addEventListener('click', () => {
      this.gameManager.startGame();
    });

    document.getElementById('resume-btn')?.addEventListener('click', () => {
      this.gameManager.resume();
      this.hideOverlays();
    });

    document.getElementById('new-game-btn')?.addEventListener('click', () => {
      this.hideOverlays();
      this.gameManager.startGame();
    });

    document.getElementById('load-btn')?.addEventListener('click', () => {
      if (this.gameManager.hasSave()) {
        this.gameManager.load();
        this.hideOverlays();
        this.updateAllResources();
        this.updateTurnDisplay();
        this.updateScoreDisplay();
      } else {
        alert('Kayıtlı oyun bulunamadı!');
      }
    });
  }

  private setupSwipeHandler(): void {
    if (!this.cardElement) return;

    this.swipeHandler = new SwipeHandler({
      element: this.cardElement,
      onSwipe: (direction) => {
        if (direction === 'left') {
          this.gameManager.chooseLeft();
        } else {
          this.gameManager.chooseRight();
        }
      },
      onSwipeProgress: (progress) => {
        this.updateChoicePreview(progress);
      }
    });
  }

  private renderCard(card: Card): void {
    const avatar = document.getElementById('character-avatar');
    const name = document.getElementById('character-name');
    const title = document.getElementById('character-title');
    const text = document.getElementById('card-text');
    const hintLeft = document.getElementById('hint-left');
    const hintRight = document.getElementById('hint-right');

    if (avatar) avatar.textContent = card.character.avatar || '👤';
    if (name) name.textContent = card.character.name;
    if (title) title.textContent = card.character.title;
    if (text) text.textContent = card.text;
    if (hintLeft) hintLeft.textContent = `← ${card.leftChoice.text}`;
    if (hintRight) hintRight.textContent = `${card.rightChoice.text} →`;

    // Kart animasyonu
    if (this.cardElement) {
      this.cardElement.style.opacity = '0';
      this.cardElement.style.transform = 'scale(0.8)';

      requestAnimationFrame(() => {
        if (this.cardElement) {
          this.cardElement.style.transition = 'all 0.3s ease';
          this.cardElement.style.opacity = '1';
          this.cardElement.style.transform = 'scale(1)';
        }
      });
    }
  }

  private updateChoicePreview(progress: number): void {
    if (!this.choicePreview) return;

    const card = this.gameManager.getCurrentCard();
    if (!card) return;

    const choice = progress < 0 ? card.leftChoice : card.rightChoice;
    const absProgress = Math.abs(progress);

    if (absProgress > 0.1) {
      this.choicePreview.innerHTML = this.formatEffects(choice.effects);
      this.choicePreview.style.opacity = String(absProgress);
      this.choicePreview.classList.toggle('preview-left', progress < 0);
      this.choicePreview.classList.toggle('preview-right', progress > 0);
    } else {
      this.choicePreview.style.opacity = '0';
    }
  }

  private formatEffects(effects: ResourceEffect[]): string {
    return effects.map(effect => {
      const icon = RESOURCE_ICONS[effect.resource];
      const avg = Math.round((effect.min + effect.max) / 2);
      const sign = avg >= 0 ? '+' : '';
      return `<span class="effect ${avg >= 0 ? 'positive' : 'negative'}">${icon} ${sign}${avg}</span>`;
    }).join(' ');
  }

  private updateResourceBar(type: ResourceType, value: number): void {
    const bar = this.resourceBars.get(type);
    const valueDisplay = document.getElementById(`resource-value-${type}`);

    if (bar) {
      bar.style.width = `${value}%`;

      // Renk değişimi
      if (value <= 20 || value >= 80) {
        bar.classList.add('danger');
      } else if (value <= 35 || value >= 65) {
        bar.classList.add('warning');
        bar.classList.remove('danger');
      } else {
        bar.classList.remove('danger', 'warning');
      }
    }

    if (valueDisplay) {
      valueDisplay.textContent = String(value);
    }
  }

  private animateResourceChange(type: ResourceType, change: number): void {
    const bar = this.resourceBars.get(type);
    if (!bar) return;

    const animClass = change > 0 ? 'increase' : 'decrease';
    bar.classList.add(animClass);

    setTimeout(() => {
      bar.classList.remove(animClass);
    }, 300);
  }

  private updateAllResources(): void {
    const resources = this.gameManager.getResources();
    for (const [type, value] of Object.entries(resources)) {
      this.updateResourceBar(type as ResourceType, value);
    }
  }

  private updateTurnDisplay(): void {
    if (this.turnDisplay) {
      this.turnDisplay.textContent = `Tur: ${this.gameManager.getTurn()}`;
    }
  }

  private updateScoreDisplay(): void {
    if (this.scoreDisplay) {
      this.scoreDisplay.textContent = `Skor: ${this.gameManager.getScore()}`;
    }
  }

  private showGameOver(data: any): void {
    const overlay = document.getElementById('game-over-overlay');
    const title = document.getElementById('overlay-title');
    const message = document.getElementById('overlay-message');
    const stats = document.getElementById('overlay-stats');

    if (overlay) overlay.style.display = 'flex';
    if (title) title.textContent = 'Oyun Bitti!';
    if (message) message.textContent = data.reason;
    if (stats) stats.textContent = `Tur: ${data.turn} | Skor: ${data.score}`;
  }

  private showVictory(data: any): void {
    const overlay = document.getElementById('game-over-overlay');
    const title = document.getElementById('overlay-title');
    const message = document.getElementById('overlay-message');
    const stats = document.getElementById('overlay-stats');

    if (overlay) overlay.style.display = 'flex';
    if (title) title.textContent = 'Zafer!';
    if (message) message.textContent = 'Tüm kartları tamamladınız!';
    if (stats) stats.textContent = `Tur: ${data.turn} | Skor: ${data.score}`;
  }

  private showMenu(): void {
    const overlay = document.getElementById('menu-overlay');
    if (overlay) overlay.style.display = 'flex';
  }

  private hideOverlays(): void {
    const gameOver = document.getElementById('game-over-overlay');
    const menu = document.getElementById('menu-overlay');

    if (gameOver) gameOver.style.display = 'none';
    if (menu) menu.style.display = 'none';
  }
}
