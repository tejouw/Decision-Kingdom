# Decision Kingdom - Implementasyon Planı

Bu doküman, Decision Kingdom oyununun teknik implementasyon planını ve geliştirme fazlarını içerir.

---

## Genel Bakış

**Tahmini Toplam Süre:** 16-20 hafta
**Teknoloji Stack Önerisi:** Unity (C#) veya React Native/Flutter (mobil cross-platform)
**Hedef Platform:** iOS & Android

---

## Faz 1: Temel Altyapı (Core Foundation)
**Süre:** 2-3 hafta

### 1.1 Proje Yapısı ve Mimari
- [ ] Proje scaffold oluşturma
- [ ] Folder structure belirleme
- [ ] Asset pipeline kurulumu
- [ ] Version control (Git) yapılandırması
- [ ] CI/CD pipeline kurulumu

### 1.2 Kaynak Sistemi (Resource System)
- [ ] Resource Manager class oluşturma
- [ ] 4 temel kaynak tanımlama:
  - `Gold` (Para)
  - `Happiness` (Halk Memnuniyeti)
  - `Military` (Askeri Güç)
  - `Faith` (İnanç/Etki)
- [ ] Kaynak limitleri (0-100) ve boundary checks
- [ ] Game Over koşulları:
  - Her kaynağın 0'a düşmesi
  - Her kaynağın 100'e ulaşması
- [ ] Resource UI Bar komponenti
- [ ] Kaynak değişim animasyonları

### 1.3 Kart Sistemi (Card System)
- [ ] Card base class/interface
- [ ] Card data model:
  ```
  Card {
    id: string
    character: Character
    text: string
    leftChoice: Choice
    rightChoice: Choice
    conditions: Condition[]
    priority: number
  }
  ```
- [ ] Choice data model:
  ```
  Choice {
    text: string
    effects: ResourceEffect[]
    triggeredEvents: string[]
  }
  ```
- [ ] Card rendering sistemi
- [ ] Swipe gesture detection (sol/sağ)
- [ ] Swipe preview (kaydırırken efektleri göster)
- [ ] Kart animasyonları (giriş, çıkış, kaydırma)

### 1.4 Game State Management
- [ ] GameManager singleton
- [ ] Turn counter
- [ ] Game state enum (Playing, Paused, GameOver, Victory)
- [ ] Save/Load sistemi (PlayerPrefs veya JSON)
- [ ] Session data tracking

---

## Faz 2: Event Sistemi (Event Engine)
**Süre:** 3-4 hafta

### 2.1 Event Template Sistemi
- [ ] Event scriptable object/JSON schema
- [ ] Event kategorileri:
  - `Story` - Ana hikaye olayları
  - `Random` - Rastgele krizler
  - `Character` - Karakter spesifik
  - `Chain` - Zincirleme olaylar
  - `Rare` - Nadir olaylar
- [ ] Event weight/priority sistemi
- [ ] Event pool management

### 2.2 Koşullu Event Tetikleme
- [ ] Condition system:
  ```
  Condition {
    type: ConditionType
    resource?: ResourceType
    operator: Operator
    value: number
    characterId?: string
    flag?: string
  }
  ```
- [ ] Condition types:
  - `ResourceThreshold` (Gold < 30)
  - `CharacterInteraction` (Marcus ile 5+ etkileşim)
  - `TurnCount` (20+ tur)
  - `Flag` (belirli olay gerçekleşti)
  - `Era` (dönem kontrolü)
- [ ] Event selector algoritması
- [ ] Chain event sistemi (A → B → C)

### 2.3 Karakter Hafızası Sistemi
- [ ] Character data model:
  ```
  Character {
    id: string
    name: string
    portrait: Sprite
    interactionCount: number
    relationship: number (-100 to 100)
    flags: string[]
  }
  ```
- [ ] Character memory tracking
- [ ] Relationship progression
- [ ] Karakter spesifik event trigger'ları
- [ ] "Hatırlama" mekanizması (Marcus seni hatırlar)

### 2.4 Flag ve State Sistemi
- [ ] Global flag manager
- [ ] Persistent flags (oyun boyunca)
- [ ] Temporary flags (oturum boyunca)
- [ ] Flag-based branching

---

## Faz 3: Ortaçağ Dönemi (Medieval Era)
**Süre:** 3-4 hafta

### 3.1 Ortaçağ Event İçeriği
- [ ] 50+ temel event template yazımı
- [ ] Event kategorileri:
  - Şövalye turnuvaları
  - Cadı avı
  - Haçlı seferi
  - Prens/prenses evlilikleri
  - Köylü isyanları
  - Dini olaylar
  - Veba/hastalık

### 3.2 Ortaçağ Karakterleri
- [ ] Ana karakterler:
  - **Danışman Marcus** - Sadık/hain çizgisi
  - **Tüccar Miriam** - Risk/ödül
  - **General Valerius** - Askeri güç
  - **Rahip Başı** - Dini otorite
  - **Kraliçe** (evlilik sonrası)
  - **Veliaht** (çocuk sonrası)
- [ ] Karakter portreleri
- [ ] Her karakter için 5-10 event chain

### 3.3 Zincirleme Hikayeler
- [ ] Ana storyline branch'leri:
  - Barışçıl yol
  - Askeri diktatörlük
  - Teokratik yönetim
  - Tüccar oligarşisi
- [ ] 20+ unique ending
- [ ] Event chain test ve balancing

### 3.4 Zorluk Dengesi
- [ ] Kaynak değişim değerleri balancing
- [ ] Turn-based difficulty scaling:
  - Tur 1-10: Küçük etkiler (±5)
  - Tur 11-30: Orta etkiler (±10)
  - Tur 31-50: Büyük etkiler (±15-20)
  - Tur 50+: Ekstrem (±20-30)

---

## Faz 4: Progression ve Meta Sistemler
**Süre:** 2-3 hafta

### 4.1 Prestige Points Sistemi
- [ ] PP hesaplama formülü:
  - Hayatta kalınan tur sayısı
  - Final kaynak dengesi bonusu
  - Özel başarımlar
- [ ] PP persistence (kalıcı kayıt)
- [ ] PP store/shop sistemi

### 4.2 Unlock Sistemi
- [ ] Dönem unlocks:
  - Ortaçağ: Başlangıçta açık
  - Rönesans: 100 PP
  - Sanayi Devrimi: 250 PP
  - Modern: 500 PP
  - Gelecek: 1000 PP
- [ ] Senaryo unlocks:
  - İyi Kral (default)
  - Genç Varis
  - Darbe Lideri
  - Zengin Tüccar
  - Halkın Sevgilisi
- [ ] Özel karakter unlocks
- [ ] Unlock notification UI

### 4.3 Achievement Sistemi
- [ ] Achievement data model
- [ ] Achievement kategorileri:
  - Survival (10, 30, 50, 100 tur)
  - Extreme (spesifik stratejiler)
  - Story (karakter etkileşimleri)
  - Secret (gizli başarımlar)
- [ ] 100+ achievement tanımlama
- [ ] Achievement tracking logic
- [ ] Achievement UI ve popup'lar
- [ ] Achievement sharing

### 4.4 İstatistik Sistemi
- [ ] Tracked metrics:
  - Toplam oynanan kart
  - En uzun survival
  - Ölüm sebepleri dağılımı
  - Ortalama kaynak değerleri
  - Favori dönem
  - Karşılaşılan karakterler
- [ ] İstatistik görüntüleme UI
- [ ] Lifetime stats vs session stats

---

## Faz 5: Ek Dönemler
**Süre:** 4-5 hafta (dönem başına ~1 hafta)

### 5.1 Rönesans Dönemi
- [ ] Tema: Sanat, keşif, bilim, tüccarlar
- [ ] 40+ event template
- [ ] Özel karakterler:
  - Sanatçı (Leonardo tipi)
  - Kaşif
  - Bilim adamı (Galileo tipi)
  - Tüccar ailesi (Medici tipi)
- [ ] Resource rebalancing (Gold ve Influence önemli)

### 5.2 Sanayi Devrimi Dönemi
- [ ] Tema: Fabrikalar, işçiler, sömürgecilik
- [ ] 40+ event template
- [ ] Özel karakterler:
  - Fabrikatör
  - İşçi lideri
  - Sömürge valisi
  - Mucit
- [ ] Yeni dinamikler: Çevre, işçi hakları

### 5.3 Modern Dönem
- [ ] Tema: Demokrasi, medya, küreselleşme
- [ ] 40+ event template
- [ ] Resource renaming:
  - Faith → Approval Rating
  - Gold → Economy
- [ ] Özel karakterler:
  - Medya patronu
  - Aktivist
  - Tech CEO
  - Diplomat

### 5.4 Gelecek Dönemi
- [ ] Tema: Uzay, AI, post-human
- [ ] 40+ event template
- [ ] Yeni resource'lar:
  - Technology
  - Ethics
- [ ] Özel karakterler:
  - AI varlık
  - Mars koloni lideri
  - Gen mühendisi

### 5.5 Dönem Geçiş Sistemi
- [ ] Transition event'leri
- [ ] Visual/audio geçiş efektleri
- [ ] Legacy sistemi (önceki dönem kararları etkisi)

---

## Faz 6: Sosyal Özellikler
**Süre:** 2 hafta

### 6.1 Daily Challenge Sistemi
- [ ] Günlük seed generation
- [ ] Sabit event sırası (aynı gün = aynı oyun)
- [ ] Global leaderboard
- [ ] Daily challenge UI

### 6.2 Share Mekanizması
- [ ] Shareable result card:
  ```
  🏰 Decision Kingdom - [Tarih]

  💰 [start] → [end] [↑/↓]
  😊 [start] → [end] [↑/↓]
  ⚔️ [start] → [end] [↑/↓]
  ✨ [start] → [end] [↑/↓]

  Survived: [X] turns
  Cause: [Game Over Reason]
  ```
- [ ] Native share integration
- [ ] Spoiler-free format

### 6.3 Profil Sistemi
- [ ] Oyuncu profil sayfası
- [ ] Karakter koleksiyonu vitrini
- [ ] Dönem mastery göstergeleri
- [ ] Play style analizi
- [ ] Nadir event koleksiyonu

### 6.4 Leaderboard
- [ ] Günlük sıralama
- [ ] Haftalık/aylık sıralama
- [ ] Arkadaşlar arası sıralama
- [ ] Backend entegrasyonu

---

## Faz 7: Monetization
**Süre:** 1-2 hafta

### 7.1 IAP (In-App Purchase) Sistemi
- [ ] Store entegrasyonu (iOS/Android)
- [ ] Ürün tanımları:
  - Dönem unlocks ($0.99-$2.99)
  - Özel senaryolar ($0.99)
  - Cosmetic paketler ($0.99)
  - Ad removal ($2.99)
  - Complete bundle ($6.99)
- [ ] Purchase validation
- [ ] Restore purchases

### 7.2 Reklam Sistemi
- [ ] Reklam SDK entegrasyonu (AdMob/Unity Ads)
- [ ] Interstitial ads (her 3 game over)
- [ ] Rewarded ads:
  - Revive (1 kez)
  - Resource boost (+10)
- [ ] Ad frequency capping
- [ ] Ad-free premium kontrol

### 7.3 Cosmetic Sistem
- [ ] Kart arka yüzü tasarımları
- [ ] UI tema seçenekleri
- [ ] Karakter portre paketleri (varsa)

---

## Faz 8: Polish ve Launch Hazırlığı
**Süre:** 2-3 hafta

### 8.1 UI/UX İyileştirmeleri
- [ ] Tutorial sistemi (ilk oyun)
- [ ] Onboarding flow
- [ ] Haptic feedback
- [ ] Sound effects
- [ ] Background music (dönem bazlı)
- [ ] UI animasyonları
- [ ] Loading screens
- [ ] Error handling UI

### 8.2 Localization
- [ ] Türkçe (varsayılan)
- [ ] İngilizce
- [ ] String externalization
- [ ] RTL support (gelecekte Arapça için)

### 8.3 Testing
- [ ] Unit tests (core systems)
- [ ] Integration tests
- [ ] Event balance testing
- [ ] Playtest sessions
- [ ] Bug fixing sprint
- [ ] Performance optimization
- [ ] Memory leak check

### 8.4 Analytics Entegrasyonu
- [ ] Firebase/Amplitude setup
- [ ] Tracked events:
  - Session start/end
  - Card decisions
  - Game over reasons
  - Purchase events
  - Achievement unlocks
- [ ] Funnel analysis setup

### 8.5 Launch Checklist
- [ ] App Store assets (screenshots, video)
- [ ] App Store description
- [ ] Privacy policy
- [ ] Terms of service
- [ ] Age rating
- [ ] Beta testing (TestFlight/Play Console)
- [ ] Soft launch (belirli ülkeler)
- [ ] Global launch

---

## Teknik Notlar

### Veri Yapıları

```typescript
// Resource System
interface Resources {
  gold: number;      // 0-100
  happiness: number; // 0-100
  military: number;  // 0-100
  faith: number;     // 0-100
}

// Card/Event System
interface GameEvent {
  id: string;
  era: Era;
  category: EventCategory;
  character?: Character;
  text: string;
  leftChoice: Choice;
  rightChoice: Choice;
  conditions: Condition[];
  priority: number;
  weight: number;
  isRare: boolean;
}

interface Choice {
  text: string;
  effects: ResourceEffect[];
  triggeredEventIds: string[];
  flags: string[];
}

interface ResourceEffect {
  resource: ResourceType;
  min: number;
  max: number;
}

// Character System
interface Character {
  id: string;
  name: string;
  title: string;
  portrait: string;
  era: Era[];
}

interface CharacterState {
  characterId: string;
  interactionCount: number;
  relationship: number;
  flags: string[];
}

// Game State
interface GameState {
  resources: Resources;
  turn: number;
  era: Era;
  characterStates: Map<string, CharacterState>;
  flags: Set<string>;
  eventHistory: string[];
}
```

### Event Seçim Algoritması

```
1. Tüm event'leri filtrele (era, conditions)
2. Öncelik sıralaması yap
3. Chain event varsa önce onu seç
4. Yoksa weight'e göre random seç
5. Nadir event şansı kontrol (%0.1)
```

### Performans Hedefleri

- Kart geçişi: < 16ms (60 FPS)
- Event seçimi: < 50ms
- Save/Load: < 100ms
- Memory footprint: < 150MB

---

## Risk Analizi

### Yüksek Risk
- **İçerik miktarı:** 200+ event yazımı zaman alıcı
  - Çözüm: Modüler event sistemi, şablon kullanımı
- **Balancing:** Kaynak dengeleri çok hassas
  - Çözüm: Playtest, analytics, hotfix capability

### Orta Risk
- **Monetization balance:** Pay-to-win algısı
  - Çözüm: Sadece content/cosmetic satışı
- **Retention:** İlk hafta sonrası düşüş
  - Çözüm: Daily challenge, achievement depth

### Düşük Risk
- **Teknik zorluk:** Swipe mekanikleri standart
- **Platform:** Unity/React Native mature

---

## Öncelik Sıralaması

Eğer zaman kısıtlı ise MVP (Minimum Viable Product):

### MVP (8-10 hafta)
1. Faz 1: Core Foundation
2. Faz 2: Event Engine
3. Faz 3: Ortaçağ Dönemi (30 event)
4. Faz 4: Temel progression (PP, 3 achievement)
5. Faz 8: Minimal polish

### V1.1 (Post-launch)
- Faz 5: Rönesans Dönemi
- Faz 6: Daily challenge
- Faz 7: Monetization

### V1.2+
- Faz 5: Diğer dönemler
- Faz 6: Full social features

---

## Sonuç

Decision Kingdom, teknik olarak basit ama içerik olarak zengin bir proje. Başarının anahtarı:

1. **Sağlam event sistemi** - Esnek ve genişletilebilir
2. **İyi yazılmış içerik** - Duygusal bağ kuran hikayeler
3. **Hassas balancing** - Ne çok kolay ne çok zor
4. **Retention mekanikleri** - Tekrar oynatma motivasyonu

Bu plan doğrultusunda, 16-20 haftalık bir geliştirme süreciyle tam kapsamlı bir ürün ortaya çıkarılabilir.

---

*Doküman Tarihi: 2025-11-19*
*Versiyon: 1.0*
