import { GameManager } from './core/GameManager.js';
import { GameUI } from './ui/GameUI.js';
import { medievalCards } from './data/medievalCards.js';

// Ana oyun başlatma fonksiyonu
function initGame(): void {
  console.log('Decision Kingdom başlatılıyor...');

  // GameManager oluştur
  const gameManager = new GameManager();

  // Kartları yükle
  gameManager.loadCards(medievalCards);
  console.log(`${medievalCards.length} kart yüklendi.`);

  // UI oluştur
  const gameUI = new GameUI('game-root', gameManager);

  // Debug için global erişim
  (window as any).gameManager = gameManager;
  (window as any).gameUI = gameUI;

  // Başlangıç ekranını göster
  showStartScreen(gameManager);
}

function showStartScreen(gameManager: GameManager): void {
  const startScreen = document.createElement('div');
  startScreen.id = 'start-screen';
  startScreen.className = 'overlay';
  startScreen.innerHTML = `
    <div class="overlay-content start-content">
      <h1>Karar Krallığı</h1>
      <p class="subtitle">Kararlarınız krallığınızın kaderini belirleyecek</p>
      <div class="start-buttons">
        <button id="start-new-game" class="primary-btn">Yeni Oyun</button>
        ${gameManager.hasSave() ? '<button id="start-continue" class="secondary-btn">Devam Et</button>' : ''}
      </div>
      <div class="game-info">
        <p>🎮 Kartları sola veya sağa kaydırarak karar verin</p>
        <p>⚖️ 4 kaynağı dengede tutun: Hazine, Mutluluk, Ordu, İnanç</p>
        <p>💀 Herhangi bir kaynak 0'a düşerse veya 100'e çıkarsa oyun biter</p>
      </div>
    </div>
  `;

  document.body.appendChild(startScreen);

  // Event listeners
  document.getElementById('start-new-game')?.addEventListener('click', () => {
    startScreen.remove();
    gameManager.startGame();
  });

  document.getElementById('start-continue')?.addEventListener('click', () => {
    startScreen.remove();
    gameManager.load();
  });
}

// Sayfa yüklendiğinde oyunu başlat
document.addEventListener('DOMContentLoaded', initGame);

// Export for module usage
export { initGame };
