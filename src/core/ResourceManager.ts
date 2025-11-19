import {
  Resources,
  ResourceType,
  ResourceEffect,
  RESOURCE_MIN,
  RESOURCE_MAX,
  RESOURCE_DEFAULT
} from '../models/types.js';

export type ResourceChangeCallback = (
  resource: ResourceType,
  oldValue: number,
  newValue: number,
  change: number
) => void;

export type GameOverCallback = (resource: ResourceType, value: number) => void;

export class ResourceManager {
  private resources: Resources;
  private onChangeCallbacks: ResourceChangeCallback[] = [];
  private onGameOverCallbacks: GameOverCallback[] = [];

  constructor(initialResources?: Partial<Resources>) {
    this.resources = {
      [ResourceType.GOLD]: initialResources?.[ResourceType.GOLD] ?? RESOURCE_DEFAULT,
      [ResourceType.HAPPINESS]: initialResources?.[ResourceType.HAPPINESS] ?? RESOURCE_DEFAULT,
      [ResourceType.MILITARY]: initialResources?.[ResourceType.MILITARY] ?? RESOURCE_DEFAULT,
      [ResourceType.FAITH]: initialResources?.[ResourceType.FAITH] ?? RESOURCE_DEFAULT
    };
  }

  // Kaynak değerini al
  getResource(type: ResourceType): number {
    return this.resources[type];
  }

  // Tüm kaynakları al
  getAllResources(): Resources {
    return { ...this.resources };
  }

  // Kaynak değerini ayarla (sınırlar içinde)
  setResource(type: ResourceType, value: number): void {
    const oldValue = this.resources[type];
    const newValue = this.clampValue(value);

    if (oldValue !== newValue) {
      this.resources[type] = newValue;
      this.notifyChange(type, oldValue, newValue, newValue - oldValue);
      this.checkGameOver(type, newValue);
    }
  }

  // Kaynağı değiştir (artır/azalt)
  modifyResource(type: ResourceType, amount: number): void {
    const oldValue = this.resources[type];
    const newValue = this.clampValue(oldValue + amount);

    if (oldValue !== newValue) {
      this.resources[type] = newValue;
      this.notifyChange(type, oldValue, newValue, amount);
      this.checkGameOver(type, newValue);
    }
  }

  // Efektleri uygula
  applyEffects(effects: ResourceEffect[], turn?: number): void {
    for (const effect of effects) {
      const amount = this.calculateRandomEffect(effect, turn);
      this.modifyResource(effect.resource, amount);
    }
  }

  // Zorluk çarpanını hesapla (tur bazlı)
  private getDifficultyMultiplier(turn: number): number {
    if (turn <= 10) {
      return 1.0;      // Tur 1-10: Temel efektler
    } else if (turn <= 30) {
      return 1.5;      // Tur 11-30: Orta zorluk
    } else if (turn <= 50) {
      return 2.0;      // Tur 31-50: Yüksek zorluk
    } else {
      return 2.5;      // Tur 50+: Aşırı zorluk
    }
  }

  // Rastgele efekt hesapla (zorluk ölçeklemesi ile)
  private calculateRandomEffect(effect: ResourceEffect, turn?: number): number {
    const min = Math.min(effect.min, effect.max);
    const max = Math.max(effect.min, effect.max);
    let amount = Math.floor(Math.random() * (max - min + 1)) + min;

    // Zorluk ölçeklemesi uygula
    if (turn && turn > 10) {
      const multiplier = this.getDifficultyMultiplier(turn);
      // Pozitif ve negatif efektler için aynı şekilde ölçekle
      amount = Math.round(amount * multiplier);
    }

    return amount;
  }

  // Değeri sınırla
  private clampValue(value: number): number {
    return Math.max(RESOURCE_MIN, Math.min(RESOURCE_MAX, value));
  }

  // Game over kontrolü
  private checkGameOver(type: ResourceType, value: number): void {
    if (value <= RESOURCE_MIN || value >= RESOURCE_MAX) {
      this.notifyGameOver(type, value);
    }
  }

  // Kaynak belirli bir değerin üstünde mi?
  isAbove(type: ResourceType, value: number): boolean {
    return this.resources[type] > value;
  }

  // Kaynak belirli bir değerin altında mı?
  isBelow(type: ResourceType, value: number): boolean {
    return this.resources[type] < value;
  }

  // Game over durumu mu?
  isGameOver(): boolean {
    for (const type of Object.values(ResourceType)) {
      const value = this.resources[type];
      if (value <= RESOURCE_MIN || value >= RESOURCE_MAX) {
        return true;
      }
    }
    return false;
  }

  // Hangi kaynak game over'a neden oldu?
  getGameOverResource(): ResourceType | null {
    for (const type of Object.values(ResourceType)) {
      const value = this.resources[type];
      if (value <= RESOURCE_MIN || value >= RESOURCE_MAX) {
        return type;
      }
    }
    return null;
  }

  // Değişiklik callback'i ekle
  onResourceChange(callback: ResourceChangeCallback): void {
    this.onChangeCallbacks.push(callback);
  }

  // Game over callback'i ekle
  onGameOver(callback: GameOverCallback): void {
    this.onGameOverCallbacks.push(callback);
  }

  // Callback'leri temizle
  clearCallbacks(): void {
    this.onChangeCallbacks = [];
    this.onGameOverCallbacks = [];
  }

  // Değişiklik bildir
  private notifyChange(
    type: ResourceType,
    oldValue: number,
    newValue: number,
    change: number
  ): void {
    for (const callback of this.onChangeCallbacks) {
      callback(type, oldValue, newValue, change);
    }
  }

  // Game over bildir
  private notifyGameOver(type: ResourceType, value: number): void {
    for (const callback of this.onGameOverCallbacks) {
      callback(type, value);
    }
  }

  // Kaynakları sıfırla
  reset(initialResources?: Partial<Resources>): void {
    this.resources = {
      [ResourceType.GOLD]: initialResources?.[ResourceType.GOLD] ?? RESOURCE_DEFAULT,
      [ResourceType.HAPPINESS]: initialResources?.[ResourceType.HAPPINESS] ?? RESOURCE_DEFAULT,
      [ResourceType.MILITARY]: initialResources?.[ResourceType.MILITARY] ?? RESOURCE_DEFAULT,
      [ResourceType.FAITH]: initialResources?.[ResourceType.FAITH] ?? RESOURCE_DEFAULT
    };
  }

  // JSON'dan yükle
  loadFromJSON(data: Resources): void {
    this.resources = { ...data };
  }

  // JSON'a dönüştür
  toJSON(): Resources {
    return { ...this.resources };
  }
}
