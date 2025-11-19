import {
  Achievement,
  AchievementCategory,
  AchievementCondition,
  AchievementConditionType,
  UnlockedAchievement,
  GameStatistics,
  SessionStatistics,
  MetaSaveData,
  PPCalculation,
  PPBonus,
  GameEndReport,
  Era,
  ResourceType,
  EndingType,
  Resources,
  CharacterState,
  GameFlag,
  ERA_UNLOCK_REQUIREMENTS
} from '../models/types.js';
import { achievements } from '../data/achievements.js';

export class MetaManager {
  private statistics: GameStatistics;
  private unlockedAchievements: Map<string, UnlockedAchievement> = new Map();
  private unlockedEras: Set<Era> = new Set([Era.MEDIEVAL]);
  private sessionHistory: SessionStatistics[] = [];
  private currentSessionStart: number = 0;
  private resourceHistory: Resources[] = [];

  private static readonly META_SAVE_KEY = 'decision_kingdom_meta';
  private static readonly META_VERSION = 1;

  constructor() {
    this.statistics = this.createInitialStatistics();
    this.load();
  }

  // Başlangıç istatistikleri
  private createInitialStatistics(): GameStatistics {
    return {
      totalGamesPlayed: 0,
      totalCardsPlayed: 0,
      totalTurnsSurvived: 0,
      longestRun: 0,
      highestScore: 0,

      averageGold: 50,
      averageHappiness: 50,
      averageMilitary: 50,
      averageFaith: 50,

      deathsByResource: {
        [ResourceType.GOLD]: 0,
        [ResourceType.HAPPINESS]: 0,
        [ResourceType.MILITARY]: 0,
        [ResourceType.FAITH]: 0
      },
      deathsByExcess: {
        [ResourceType.GOLD]: 0,
        [ResourceType.HAPPINESS]: 0,
        [ResourceType.MILITARY]: 0,
        [ResourceType.FAITH]: 0
      },

      endingsReached: {} as Record<EndingType, number>,
      characterInteractions: {},
      favoriteCharacter: '',

      gamesPerEra: {
        [Era.MEDIEVAL]: 0,
        [Era.RENAISSANCE]: 0,
        [Era.INDUSTRIAL]: 0,
        [Era.MODERN]: 0,
        [Era.FUTURE]: 0
      },
      favoriteEra: Era.MEDIEVAL,

      totalPPEarned: 0,
      ppSpent: 0,
      currentPP: 0,

      achievementsUnlocked: 0,
      totalAchievements: achievements.length,

      firstPlayedAt: 0,
      lastPlayedAt: 0,
      totalPlayTime: 0
    };
  }

  // Oturum başlat
  startSession(): void {
    this.currentSessionStart = Date.now();
    this.resourceHistory = [];

    if (this.statistics.firstPlayedAt === 0) {
      this.statistics.firstPlayedAt = this.currentSessionStart;
    }
  }

  // Kaynak geçmişine ekle
  recordResources(resources: Resources): void {
    this.resourceHistory.push({ ...resources });
  }

  // Oyun sonu işle
  processGameEnd(
    turn: number,
    score: number,
    era: Era,
    endingType: EndingType,
    characterStates: Map<string, CharacterState>,
    flags: Map<string, GameFlag>,
    deathResource?: ResourceType,
    deathValue?: number
  ): GameEndReport {
    const endedAt = Date.now();
    const duration = endedAt - this.currentSessionStart;
    const gameNumber = this.statistics.totalGamesPlayed + 1;

    // PP hesapla
    const ppCalculation = this.calculatePP(turn, score, endingType, characterStates.size);

    // Yeni başarımları kontrol et
    const newAchievements = this.checkAchievements(
      turn,
      score,
      endingType,
      characterStates,
      flags,
      gameNumber
    );

    // Oturum istatistikleri oluştur
    const sessionStats: SessionStatistics = {
      gameNumber,
      era,
      turnsSurvived: turn,
      cardsPlayed: turn - 1,
      score,
      ppEarned: ppCalculation.total,
      endingReached: endingType,
      deathReason: deathResource ? `${deathResource}: ${deathValue}` : undefined,
      charactersMet: Array.from(characterStates.keys()),
      flagsSet: Array.from(flags.keys()),
      duration,
      startedAt: this.currentSessionStart,
      endedAt,
      resourceHistory: this.resourceHistory,
      newAchievements: newAchievements.map(a => a.id)
    };

    // İstatistikleri güncelle
    const previousPP = this.statistics.currentPP;
    this.updateStatistics(sessionStats, deathResource, deathValue, characterStates);

    // Yeni dönem kilidini kontrol et
    const newEraUnlocks = this.checkEraUnlocks();

    // Kaydet
    this.save();

    return {
      sessionStats,
      ppCalculation,
      newAchievements,
      newEraUnlocks,
      previousPP,
      newTotalPP: this.statistics.currentPP
    };
  }

  // PP hesapla
  private calculatePP(
    turn: number,
    score: number,
    endingType: EndingType,
    characterCount: number
  ): PPCalculation {
    const bonuses: PPBonus[] = [];

    // Temel PP (her tur için 1 PP)
    const base = turn;

    // Skor bonusu
    if (score >= 500) {
      bonuses.push({
        type: 'score',
        description: 'Yuksek Skor (500+)',
        amount: 20
      });
    } else if (score >= 300) {
      bonuses.push({
        type: 'score',
        description: 'Iyi Skor (300+)',
        amount: 10
      });
    } else if (score >= 100) {
      bonuses.push({
        type: 'score',
        description: 'Orta Skor (100+)',
        amount: 5
      });
    }

    // Tur bonusları
    if (turn >= 100) {
      bonuses.push({
        type: 'survival',
        description: 'Efsane Hayatta Kalma (100+ tur)',
        amount: 50
      });
    } else if (turn >= 50) {
      bonuses.push({
        type: 'survival',
        description: 'Uzun Hayatta Kalma (50+ tur)',
        amount: 25
      });
    } else if (turn >= 30) {
      bonuses.push({
        type: 'survival',
        description: 'Iyi Hayatta Kalma (30+ tur)',
        amount: 10
      });
    }

    // Karakter çeşitliliği bonusu
    if (characterCount >= 8) {
      bonuses.push({
        type: 'characters',
        description: 'Tum Karakterler',
        amount: 15
      });
    } else if (characterCount >= 5) {
      bonuses.push({
        type: 'characters',
        description: 'Cok Karakter',
        amount: 8
      });
    }

    // Özel bitiş bonusları
    if (endingType === EndingType.CONQUEROR) {
      bonuses.push({
        type: 'ending',
        description: 'Fatih Sonu',
        amount: 20
      });
    } else if (endingType === EndingType.BELOVED) {
      bonuses.push({
        type: 'ending',
        description: 'Sevilen Sultan Sonu',
        amount: 15
      });
    } else if (endingType === EndingType.SCHOLAR) {
      bonuses.push({
        type: 'ending',
        description: 'Bilge Sultan Sonu',
        amount: 15
      });
    }

    const total = base + bonuses.reduce((sum, b) => sum + b.amount, 0);

    return { base, bonuses, total };
  }

  // Başarımları kontrol et
  private checkAchievements(
    turn: number,
    score: number,
    endingType: EndingType,
    characterStates: Map<string, CharacterState>,
    flags: Map<string, GameFlag>,
    gameNumber: number
  ): Achievement[] {
    const newAchievements: Achievement[] = [];

    for (const achievement of achievements) {
      // Zaten kazanılmış mı?
      if (this.unlockedAchievements.has(achievement.id)) {
        continue;
      }

      // Koşulu kontrol et
      if (this.checkAchievementCondition(
        achievement.condition,
        turn,
        score,
        endingType,
        characterStates,
        flags
      )) {
        // Başarımı kazandı!
        this.unlockedAchievements.set(achievement.id, {
          achievementId: achievement.id,
          unlockedAt: Date.now(),
          gameNumber
        });

        // PP ödülünü ekle
        this.statistics.currentPP += achievement.ppReward;
        this.statistics.totalPPEarned += achievement.ppReward;
        this.statistics.achievementsUnlocked++;

        newAchievements.push(achievement);
      }
    }

    return newAchievements;
  }

  // Tek başarım koşulunu kontrol et
  private checkAchievementCondition(
    condition: AchievementCondition,
    turn: number,
    score: number,
    endingType: EndingType,
    characterStates: Map<string, CharacterState>,
    flags: Map<string, GameFlag>
  ): boolean {
    switch (condition.type) {
      case AchievementConditionType.TURNS_SURVIVED:
        return condition.value !== undefined && turn >= condition.value;

      case AchievementConditionType.TOTAL_CARDS_PLAYED:
        return condition.value !== undefined &&
          this.statistics.totalCardsPlayed + turn >= condition.value;

      case AchievementConditionType.SPECIFIC_SCORE:
        return condition.value !== undefined && score >= condition.value;

      case AchievementConditionType.ENDING_REACHED:
        return condition.endingType === endingType;

      case AchievementConditionType.FLAG_SET:
        return condition.flag !== undefined && flags.has(condition.flag);

      case AchievementConditionType.CHARACTER_INTERACTION_COUNT:
        if (condition.characterId && condition.value !== undefined) {
          const charState = characterStates.get(condition.characterId);
          return charState !== undefined &&
            charState.interactionCount >= condition.value;
        }
        return false;

      case AchievementConditionType.ALL_CHARACTERS_MET:
        return characterStates.size >= 10;

      case AchievementConditionType.GAMES_COMPLETED:
        return condition.value !== undefined &&
          this.statistics.totalGamesPlayed + 1 >= condition.value;

      case AchievementConditionType.TOTAL_PP_EARNED:
        return condition.value !== undefined &&
          this.statistics.totalPPEarned >= condition.value;

      default:
        return false;
    }
  }

  // İstatistikleri güncelle
  private updateStatistics(
    session: SessionStatistics,
    deathResource?: ResourceType,
    deathValue?: number,
    characterStates?: Map<string, CharacterState>
  ): void {
    const stats = this.statistics;

    // Genel istatistikler
    stats.totalGamesPlayed++;
    stats.totalCardsPlayed += session.cardsPlayed;
    stats.totalTurnsSurvived += session.turnsSurvived;
    stats.lastPlayedAt = session.endedAt;
    stats.totalPlayTime += session.duration;

    // Rekorlar
    if (session.turnsSurvived > stats.longestRun) {
      stats.longestRun = session.turnsSurvived;
    }
    if (session.score > stats.highestScore) {
      stats.highestScore = session.score;
    }

    // PP
    stats.currentPP += session.ppEarned;
    stats.totalPPEarned += session.ppEarned;

    // Ölüm sebepleri
    if (deathResource) {
      if (deathValue !== undefined && deathValue <= 0) {
        stats.deathsByResource[deathResource]++;
      } else if (deathValue !== undefined && deathValue >= 100) {
        stats.deathsByExcess[deathResource]++;
      }
    }

    // Bitiş istatistikleri
    if (!stats.endingsReached[session.endingReached]) {
      stats.endingsReached[session.endingReached] = 0;
    }
    stats.endingsReached[session.endingReached]++;

    // Dönem istatistikleri
    stats.gamesPerEra[session.era]++;
    stats.favoriteEra = this.getFavoriteEra();

    // Karakter istatistikleri
    if (characterStates) {
      for (const [charId, state] of characterStates) {
        if (!stats.characterInteractions[charId]) {
          stats.characterInteractions[charId] = 0;
        }
        stats.characterInteractions[charId] += state.interactionCount;
      }
      stats.favoriteCharacter = this.getFavoriteCharacter();
    }

    // Kaynak ortalamaları (son 10 oyun ile hesapla)
    this.updateResourceAverages(session.resourceHistory);

    // Oturum geçmişine ekle (son 50 oyun)
    this.sessionHistory.push(session);
    if (this.sessionHistory.length > 50) {
      this.sessionHistory.shift();
    }
  }

  // Favori dönem
  private getFavoriteEra(): Era {
    let maxGames = 0;
    let favorite = Era.MEDIEVAL;

    for (const [era, games] of Object.entries(this.statistics.gamesPerEra)) {
      if (games > maxGames) {
        maxGames = games;
        favorite = era as Era;
      }
    }

    return favorite;
  }

  // Favori karakter
  private getFavoriteCharacter(): string {
    let maxInteractions = 0;
    let favorite = '';

    for (const [charId, count] of Object.entries(this.statistics.characterInteractions)) {
      if (count > maxInteractions) {
        maxInteractions = count;
        favorite = charId;
      }
    }

    return favorite;
  }

  // Kaynak ortalamalarını güncelle
  private updateResourceAverages(history: Resources[]): void {
    if (history.length === 0) return;

    let totalGold = 0, totalHappiness = 0, totalMilitary = 0, totalFaith = 0;

    for (const resources of history) {
      totalGold += resources[ResourceType.GOLD];
      totalHappiness += resources[ResourceType.HAPPINESS];
      totalMilitary += resources[ResourceType.MILITARY];
      totalFaith += resources[ResourceType.FAITH];
    }

    const count = history.length;
    const stats = this.statistics;
    const gamesPlayed = stats.totalGamesPlayed;

    // Kümülatif ortalama hesaplama
    stats.averageGold = Math.round(
      (stats.averageGold * (gamesPlayed - 1) + totalGold / count) / gamesPlayed
    );
    stats.averageHappiness = Math.round(
      (stats.averageHappiness * (gamesPlayed - 1) + totalHappiness / count) / gamesPlayed
    );
    stats.averageMilitary = Math.round(
      (stats.averageMilitary * (gamesPlayed - 1) + totalMilitary / count) / gamesPlayed
    );
    stats.averageFaith = Math.round(
      (stats.averageFaith * (gamesPlayed - 1) + totalFaith / count) / gamesPlayed
    );
  }

  // Dönem kilidini kontrol et
  private checkEraUnlocks(): Era[] {
    const newUnlocks: Era[] = [];

    for (const [era, requiredPP] of Object.entries(ERA_UNLOCK_REQUIREMENTS)) {
      const eraEnum = era as Era;
      if (!this.unlockedEras.has(eraEnum) && this.statistics.currentPP >= requiredPP) {
        this.unlockedEras.add(eraEnum);
        newUnlocks.push(eraEnum);
      }
    }

    return newUnlocks;
  }

  // Dönem açık mı?
  isEraUnlocked(era: Era): boolean {
    return this.unlockedEras.has(era);
  }

  // Tüm açık dönemleri al
  getUnlockedEras(): Era[] {
    return Array.from(this.unlockedEras);
  }

  // Başarım açık mı?
  isAchievementUnlocked(achievementId: string): boolean {
    return this.unlockedAchievements.has(achievementId);
  }

  // Tüm başarımları al (açılanlar ile birlikte)
  getAllAchievements(): { achievement: Achievement; unlocked: UnlockedAchievement | null }[] {
    return achievements.map(achievement => ({
      achievement,
      unlocked: this.unlockedAchievements.get(achievement.id) || null
    }));
  }

  // İstatistikleri al
  getStatistics(): GameStatistics {
    return { ...this.statistics };
  }

  // Mevcut PP
  getCurrentPP(): number {
    return this.statistics.currentPP;
  }

  // PP harca (gelecekte mağaza için)
  spendPP(amount: number): boolean {
    if (this.statistics.currentPP >= amount) {
      this.statistics.currentPP -= amount;
      this.statistics.ppSpent += amount;
      this.save();
      return true;
    }
    return false;
  }

  // Oturum geçmişini al
  getSessionHistory(): SessionStatistics[] {
    return [...this.sessionHistory];
  }

  // Kaydet
  save(): void {
    const saveData: MetaSaveData = {
      statistics: this.statistics,
      unlockedAchievements: Array.from(this.unlockedAchievements.values()),
      unlockedEras: Array.from(this.unlockedEras),
      sessionHistory: this.sessionHistory,
      version: MetaManager.META_VERSION,
      savedAt: Date.now()
    };

    try {
      localStorage.setItem(MetaManager.META_SAVE_KEY, JSON.stringify(saveData));
    } catch (error) {
      console.error('Meta save error:', error);
    }
  }

  // Yükle
  load(): void {
    try {
      const savedJson = localStorage.getItem(MetaManager.META_SAVE_KEY);
      if (!savedJson) return;

      const saveData: MetaSaveData = JSON.parse(savedJson);

      this.statistics = saveData.statistics;
      this.unlockedAchievements = new Map(
        saveData.unlockedAchievements.map(ua => [ua.achievementId, ua])
      );
      this.unlockedEras = new Set(saveData.unlockedEras);
      this.sessionHistory = saveData.sessionHistory || [];

      // Toplam başarım sayısını güncelle (yeni başarımlar eklenmiş olabilir)
      this.statistics.totalAchievements = achievements.length;
    } catch (error) {
      console.error('Meta load error:', error);
    }
  }

  // Sıfırla (debug için)
  reset(): void {
    this.statistics = this.createInitialStatistics();
    this.unlockedAchievements.clear();
    this.unlockedEras = new Set([Era.MEDIEVAL]);
    this.sessionHistory = [];
    localStorage.removeItem(MetaManager.META_SAVE_KEY);
  }
}
