import { GameManager } from './core/GameManager.js';
import { GameUI } from './ui/GameUI.js';
import { medievalCards } from './data/medievalCards.js';
import { renaissanceCards } from './data/renaissanceCards.js';
import { industrialCards } from './data/industrialCards.js';
import { modernCards } from './data/modernCards.js';
import { futureCards } from './data/futureCards.js';
import { Era, Card } from './models/types.js';

// Era bazlı kartları al
function getCardsByEra(era: Era): Card[] {
  switch (era) {
    case Era.MEDIEVAL:
      return medievalCards;
    case Era.RENAISSANCE:
      return renaissanceCards;
    case Era.INDUSTRIAL:
      return industrialCards;
    case Era.MODERN:
      return modernCards;
    case Era.FUTURE:
      return futureCards;
    default:
      return medievalCards;
  }
}

// Kartlara era bilgisi ekle
function addEraToCards(cards: Card[], era: Era): Card[] {
  return cards.map(card => ({ ...card, era }));
}

// Tüm kartları al (era bilgisi ile)
function getAllCards(): Card[] {
  return [
    ...addEraToCards(medievalCards, Era.MEDIEVAL),
    ...addEraToCards(renaissanceCards, Era.RENAISSANCE),
    ...addEraToCards(industrialCards, Era.INDUSTRIAL),
    ...addEraToCards(modernCards, Era.MODERN),
    ...addEraToCards(futureCards, Era.FUTURE)
  ];
}

// Era isimleri (Türkçe)
const ERA_NAMES: Record<Era, string> = {
  [Era.MEDIEVAL]: 'Ortaçağ',
  [Era.RENAISSANCE]: 'Rönesans',
  [Era.INDUSTRIAL]: 'Sanayi Devrimi',
  [Era.MODERN]: 'Modern Çağ',
  [Era.FUTURE]: 'Gelecek'
};

// Ana oyun başlatma fonksiyonu
function initGame(): void {
  console.log('Decision Kingdom başlatılıyor...');

  // GameManager oluştur
  const gameManager = new GameManager();

  // Tüm kartları yükle (CardManager era'ya göre filtreleyecek)
  const allCards = getAllCards();
  gameManager.loadCards(allCards);
  console.log(`Toplam ${allCards.length} kart yüklendi.`);
  console.log(`- Ortaçağ: ${medievalCards.length}`);
  console.log(`- Rönesans: ${renaissanceCards.length}`);
  console.log(`- Sanayi Devrimi: ${industrialCards.length}`);
  console.log(`- Modern Çağ: ${modernCards.length}`);
  console.log(`- Gelecek: ${futureCards.length}`);

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
