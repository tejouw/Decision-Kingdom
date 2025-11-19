# Decision Kingdom: Oyuncu Deneyimi & İçerik Tasarımı

## Oyunun Kalbi: Seçimlerinin Ağırlığı

Oyuncu bir krallığı, şirketi veya koloniyi yönetiyor - ama klasik strateji oyunlarından farklı olarak her karar bir kartla gelir ve sadece iki seçenek vardır. Sağa kaydır, sola kaydır. Evet veya hayır. Kabul et veya reddet. Bu sadelik oyunu erişilebilir yapıyor ama ardındaki karmaşıklık onu bağımlılık yapıyor.

## Dört Kaynak: Hassas Denge Sanatı

Her kararın dört temel kaynağı etkiliyor:

### PARA (Gold/Treasury): Krallığının ekonomik gücü
- 0'a düşerse: İflas, isyan başlar, oyun biter
- 100'e ulaşırsa: Enflasyon krizi, değersizleşme, oyun biter

### HALK MEMNUNİYETİ (Happiness/Approval): Halkın sana olan güveni
- 0'a düşerse: Devrim, tahtan indirilirsin
- 100'e ulaşırsa: Halk tembel olur, krallık çöker

### ASKERİ GÜÇ (Military/Defense): Ordunun ve savunmanın
- 0'a düşerse: Komşu ülke istila eder
- 100'e ulaşırsa: Askeri darbe, generaller yönetime el koyar

### İNANÇ/ETKİ (Faith/Influence): Dini/kültürel/politik etkinin
- 0'a düşerse: Kaos, anarşi, toplum dağılır
- 100'e ulaşırsa: Teokrasi/diktatörlük, sistem çöker

**Oyunun zekice kısmı:** Hiçbir kaynak "iyi" veya "kötü" değil. Ortada kalman lazım. Bir danışman sana "fakir köylülere yardım gönderelim mi?" diye soruyor. Evet dersen: +Happiness, -Gold. Hayır dersen: -Happiness, +Gold. Ama her iki seçenek de diğer kaynakları da etkileyebilir. Belki din adamları yardım edersen seni takdir eder (+Faith), belki askerler "biz maaş alamıyoruz ama köylüler para alıyor" der (-Military).

## Bir Oyun Turunda Neler Olur?

### Sonsuz Event Zinciri

Oyun başladığında kartlar gelmeye başlar. Her kart bir durum, bir kriz, bir fırsat, bir karakter. 200-300 farklı event template var ve bunlar senin önceki seçimlerine göre şekilleniyor.

**Örnek olay zinciri:**

**Kart 1:** Kraliyet danışmanı gelir
> "Majeste, hazine boş. Köylülerden daha fazla vergi mi toplayalım?"

- Sol (Hayır): -Gold, +Happiness
- Sağ (Evet): +Gold, -Happiness, -Faith

Sağa kaydırdın, vergi artırdın.

**Kart 2:** (Bir önceki kararın sonucu tetiklendi)
> "Köylüler ayaklandı! Liderleri krallık meydanında. Ne yapmalı?"

- Sol (Affet): +Happiness, -Military (askerler senin yumuşaklığını beğenmedi)
- Sağ (İdam et): -Happiness, +Military, -Faith (din adamları şiddeti kınadı)

İdam ettin. Askeri güçlü ama halk ve din adamları mutsuz.

**Kart 3:** (Zincir devam ediyor)
> "Rahip başı sarayda. 'Kral zalim' diyor halka. Onu susturmalı mıyız?"

- Sol (Serbest bırak): +Faith, -Military (askerler otoriten yok diyor)
- Sağ (Hapse at): -Faith, +Military, +Gold (kilise mallarına el koy)

Oyunun derinliği buradan geliyor: Her karar bir sonrakini etkiliyor. 5-6 kart sonra tamamen farklı bir krallığın var. Belki askeri diktatörlük kurmuşsun, belki barış içinde mutlu ama fakir bir krallık, belki zengin ama halk senden nefret ediyor.

## Çeşitli Event Kategorileri

### Karakter Kartları: Tekrar Eden Yüzler

Bazı karakterler tekrar tekrar gelir, önceki davranışını hatırlar:

**Danışman Marcus:**
- İlk gelişinde: "Size sadık bir danışman olmak istiyorum"
- Ona güvendiysen 10 kart sonra: "Sağolun majeste, size harika bir yatırım fırsatı buldum" (gerçekten iyi)
- Onu reddettiydiysen: "Bir ihanet komplosu keşfettim... ama size söylemeli miyim?" (intikam peşinde)

**Tüccar Miriam:**
- İlk gelişinde: "Ucuza mal satayım mı?"
- Kabul ettiysen: İlerleyen turda "Bir sorun var, o mallar çalıntıymış" (baş belası)
- Reddettiysen: Başka ülkeye gitmiş, rakibin zenginleşiyor

**General Valerius:**
- İlk gelişinde: "Ordumuz güçlenmeli"
- Sürekli ret ettin: Darbe yapıyor
- Sürekli kabul ettin: Askeri diktatör oluyor, ama seni koruyor

Bu "hikaye hafızası" oyuncuya anlamlı seçimler yaptığını hissettiriyor. "Acaba o tüccarla çalışsaydım şimdi ne olurdu?" diye düşünüyor.

### Rastgele Krizler: Beklenmedik Darbeler

Her türlü neden krallığın çökebilir:

**Doğal Afetler:**
- Veba salgını: Halk ölüyor, ne yapacaksın?
  - Karantina: -Happiness, -Gold (ekonomi durdu)
  - Görmezden gel: -Faith, -Happiness (daha çok ölüm)

**Komşu Ülkeler:**
- Savaş tehdidi
- Ticaret anlaşması
- Evlilik teklifi (diplomatik birleşme)
- Ültimatom (ya kabul et ya savaş)

**Saray Entrikaları:**
- Vezirin komplotu
- Kraliçenin ihaneti (evliysen)
- Veliahdın isyanı
- Gizli polis raporları

**Dini Olaylar:**
- Mucize iddiası (gerçek mi, dolandırıcılık mı?)
- Yeni din yayılıyor
- Rahip başı güç istiyor
- Reformasyon hareketi

## Dönemler: Tarih Boyunca Yolculuk

Oyun 5 farklı dönemde geçiyor. Her dönem farklı atmosfer, farklı event'ler, farklı zorluklar.

### 1. ORTAÇAĞ (Medieval Era)

**Tema:** Feodalizm, din, şövalyeler, veba

**Karakteristik olaylar:**
- Şövalyelerin turnuvaları
- Cadı avı istekleri
- Haçlı seferi çağrısı
- Prens/prenses evlilikleri
- Köylü isyanları
- Ejderhalar ve efsaneler (fantastik unsurlar hafif)

**Resource önceliği:** Faith çok önemli (din adamları güçlü), Military gerekli (sürekli savaş)

**Atmosfer:** Karanlık, mistik, tehlikeli. Her karar hayat-ölüm meselesi.

### 2. RÖNESANS (Renaissance Era)

**Tema:** Sanat, keşif, bilim, tüccarlar

**Karakteristik olaylar:**
- Sanatçı patronluğu (Leonardo da Vinci benzeri)
- Keşif gezileri (Yeni Dünya)
- Bilim adamları (Galileo gibi - kilise vs bilim)
- Tüccar aileleri (Medici tarzı güç)
- Matbaa devrimi
- Saray komploları incelikle

**Resource önceliği:** Gold önem kazanıyor, Faith azalıyor, Influence yükseliyor

**Atmosfer:** Umut, yenilik, entrika. Daha sofistike kararlar.

### 3. SANAYİ DEVRİMİ (Industrial Era)

**Tema:** Fabrikalar, işçiler, sömürgecilik, modernleşme

**Karakteristik olaylar:**
- Fabrika kurma vs çevre
- İşçi grevleri
- Sömürge fethetme fırsatları
- Demiryolu yatırımları
- Çocuk işçiliği tartışması
- Sendika hareketleri
- Buhar teknolojisi

**Resource önceliği:** Gold dominant (kapitalizm), Faith düşüyor, Happiness kritik (işçi sınıfı)

**Atmosfer:** İlerleme ve bedeli. Ahlaki ikilemlerin yükselişi.

### 4. MODERN DÖNEM (Modern Era)

**Tema:** Demokrasi, medya, küreselleşme, teknoloji

**Karakteristik olaylar:**
- Seçim kampanyaları
- Medya skandalları
- Sosyal medya krizleri
- Uluslararası zirveler
- Ekonomik krizler
- Terör tehditleri
- Çevre krizleri
- Teknoloji şirketleri

**Resource önceliği:** Approval Rating (artık Faith değil), Economy, Security, Influence (soft power)

**Atmosfer:** Karmaşık, hızlı, medya baskısı. Her şey kamuoyunda tartışılıyor.

### 5. GELECEK (Future Era)

**Tema:** Uzay, yapay zeka, post-human, distopya/ütopya

**Karakteristik olaylar:**
- Mars kolonisi kararları
- AI hakları tartışması
- Gen düzenleme etiği
- Ölümsüzlük teknolojisi
- Hologram toplumsal düzen
- Android isyanı
- Klima mültecileri
- Siber savaşlar

**Resource önceliği:** Technology, Ethics, Security, Resources (artık sadece para değil)

**Atmosfer:** Spekülatif, filozofik, egzistansiyel. "İnsanlık ne demek?" soruları.

## Progression: Nasıl İlerliyorsun?

### Oyun İçi Progression (Tek Oturum)

Her oyun oturumu 20-50 kart sürüyor (10-30 dakika). Hedef: olabildiğince uzun hayatta kalmak, mümkünse sonu görmek.

**Zorluk kademeli artıyor:**
- İlk 10 kart: Kolay kararlar, küçük değişimler (+5, -5)
- 11-30 kart: Orta zorluk, event'ler birbirine bağlanıyor
- 31-50 kart: Kriz üstüne kriz, büyük değişimler (+15, -20)
- 50+: Kaos modu, ekstrem durumlar

**Her oturumda bir hikaye oluşuyor:**
- "Barış kralı" oldun ama savaşta yenildin
- "Tiran" oldun ama krallık 100 yıl ayakta kaldı
- "Dengeleyici" oldun, mükemmel krallık ama sıkıcı
- "Kaos lordu" oldun, sürekli kriz ama eğlenceli

### Meta Progression: Kalıcı İlerleme

**Unlock Sistemi:**

Oyun bittiğinde (game over veya başarıyla tamamlama) Prestige Points kazanıyorsun. Bunlarla:

**Yeni Dönemler Açılıyor:**
- Başlangıç: Sadece Ortaçağ açık
- 100 PP: Rönesans unlock
- 250 PP: Sanayi Devrimi unlock
- 500 PP: Modern Dönem unlock
- 1000 PP: Gelecek unlock

**Yeni Başlangıç Senaryoları:**
- "İyi Kral" (Balanced kaynaklar, kolay başlangıç)
- "Genç Varis" (Az deneyim, halk şüpheci)
- "Darbe Lideri" (Yüksek Military, düşük her şey)
- "Zengin Tüccar" (Yüksek Gold, düşük Faith)
- "Halkın Sevgilisi" (Yüksek Happiness, düşük Gold)

**Karakterler Unlock:**

Bazı özel danışmanlar/karakterler sonradan unlock oluyor:
- "Zeki Casusu" (sana gelecek olayları hafif spoiler eder)
- "Sadık General" (Military hiç 0'a düşmez)
- "Mucize İşçi" (Faith kaynaklarına bonus)

### Achievement Sistemi: Koleksiyonculuk

100+ Achievement var, her biri bir hikaye:

**Survival Achievements:**
- "İlk Kan": İlk kez 10 kart hayatta kal
- "Deneyimli": 30 kart hayatta kal
- "Efsane": 50 kart hayatta kal
- "Ölümsüz": 100 kart hayatta kal (çok zor)

**Extreme Achievements:**
- "Tiryaki": Sadece Military üzerinden kazan
- "Barış Elçisi": Hiç Military 50'nin üstüne çıkma
- "Soytarı Kral": Gold hiç 30'un altına düşmesin
- "Fakir Ama Mutlu": Gold 20'nin altında kal, kazanmaya bak

**Story Achievements:**
- "İhanet": Danışmanını 3 kez reddet
- "Sadakat": Aynı generale 10 kez evet de
- "Katil Kral": 10 farklı karakteri idam et
- "Barış Yapıcı": Hiç kimseyi öldürme

**Secret Achievements:**
- "Ejderha Avcısı": Ortaçağ'da rastgele ejderha event'ini bul ve yen
- "Zaman Yolcusu": Tüm 5 dönemi bitir
- "Tam Denge": 4 kaynak 50'de biter

## Social & Viral Mekanikler

### Günlük Meydan Okuma

Her gün sabit bir seed var. Yani tüm dünya aynı event sırasını oynuyor.

**Nasıl çalışıyor:**
- Günün challenge'ı: "Ortaçağ, 30 kart, Genç Varis"
- Herkes aynı event'leri aynı sırayla görüyor
- Ama herkes farklı kararlar veriyor
- Sonunda: "Sen 28 kart hayatta kaldın, dünya ortalaması 19"

**Shareability:**
```
🏰 Decision Kingdom - 4 Ocak 2025

💰 42 → 67 ⬆️
😊 61 → 12 ⬇️
⚔️ 78 → 89 ⬆️
✨ 45 → 31 ⬇️

Survived: 28 turns
Cause of death: Revolution

Senin kaçta öldün?
```

Wordle tarzı share, ama spoiler yok. Sadece resource grafikleri ve sonuç.

### Profil & Koleksiyon Vitrini

Oyuncu profilinde:
- Karakter Kartı Koleksiyonu: 200+ karakterle karşılaştın mı? Hepsini topla
- Dönem Mastery: Her dönemde kaç kez kazandın?
- Play Style Analysis: "Sen %68 Diplomatik, %22 Agresif, %10 Ekonomik"
- Rare Events: "Ultra nadir 'Anka Kuşu' olayını gördün (oyuncuların %0.3'ü)"

### İstatistik Fetişizmi

Oyuncular veri sever:
- Toplam kaç kart gördün: 3,847
- En uzun hayatta kalma: 67 kart
- En çok öldüğün sebep: Askeri Darbe (%34)
- En az kullandığın kaynak: Faith (ortalama 34/100)
- Favori dönem: Modern (12 kez tamamladın)

## İçerik Derinliği: Neden Tekrar Tekrar Oynanır?

### Procedural Storytelling'in Gücü

200 event template var diyelim. Her biri 2 sonuç. Ama her sonuç sonraki event'leri etkiliyor.

**Örnek dallanma:**
```
Event A: Tüccar mal satıyor
  → Kabul et → B: Mal çalıntıymış, polisler geldi
    → Tüccarı sat → C: Tüccar intikam peşinde
    → Tüccarı koru → D: Polis sana güvenmiyor
  → Reddet → E: Tüccar rakibine gitti
    → Rakip güçlendi → F: Ekonomik savaş başladı
```

6 farklı event bu zincirde. 200 template × ortalama 3 bağlantı = 600+ benzersiz durum.

**Ama sistem daha da zeki:**

- **Karakter hafızası:** Marcus'a 5 kez evet dedin mi? 6. gelişinde "En sadık danışmanım, sana özel bir teklif" event'i tetikleniyor.
- **Resource thresholds:** Gold < 30 iken belirli "yoksulluk" event'leri tetikleniyor. Military > 80 iken "darbe riski" event'leri geliyor.
- **Dönem cross-over:** Rönesans'ta bilim adamını koruduysan, Sanayi Devriminde onun torunu mühendis olarak geliyor ve seni hatırlıyor.

### Yeniden Oynanabilirlik Katmanları

- **Katman 1:** Farklı seçimler yap ("Bu sefer barışçıl kral olayım", "Bu sefer tiran olayım")
- **Katman 2:** Farklı dönemler keşfet (Her dönem kendi atmosferi, karakterleri, dilemmaları)
- **Katman 3:** Farklı başlangıçlar (Genç Varis ile başlamak ≠ Darbe Lideri ile başlamak)
- **Katman 4:** Achievement hunting ("Fakir ama mutlu" achievement'ini almak için spesifik strateji lazım)
- **Katman 5:** Günlük challenge (Her gün yeni seed, yeni rekor kovalama)
- **Katman 6:** Nadir event'leri görme (1000 oyunda 1 görünen "Gizli Hazine" event'ini bulabilir misin?)

## Duygusal Bağ: Neden Önemseriz?

### Anlamlı Seçimler Yanılsaması

Oyuncu bilmiyor ki aslında çoğu seçim önceden yazılmış. Ama her seçim o an için çok önemli hissettiriyor.

"Danışmanımı koruyayım mı yoksa halkı mı dinlerim?" → Bu seçim o karakterin senin değerlerine göre tepki vermesini sağlıyor.

**Gerçekte:** Her iki seçenek de 5 farklı gelecek event'e yol açabilir. Ama oyuncu "bu BENİM kararım, BENİM krallığım" hissediyor.

### Kayıp Korkusu

4 kaynak barı sürekli görüyorsun. Biri 10'a düşünce panik oluyorsun. "Bir sonraki kart onu 0'a götürürse öldüm!"

Bu tansiyon oyunu gerilimli yapıyor. Her kart potansiyel son.

### "Ya ... Olsaydı?" Sorgulaması

Oyun bitince düşünüyorsun:
- "Ya o tüccarla çalışsaydım?"
- "Ya generali dinleseydim?"
- "Ya evlenmeyi kabul etseydim?"

Bu sorular seni tekrar oynamaya itiyor. Farklı yolu görme merakı.

### Hikayene Sahiplenme

"Benim krallığım 47 kart sürdü ve askeri darbeyle yıkıldı" → Bu sadece rastgele bir oyun değil, senin hikayenin.

Arkadaşına anlatıyorsun: "Ya çok komik bir şey oldu, danışmanım bana ihanet etti ama sonra onu affettim, o da bana sadık kaldı, sonunda beraber krallığı kurtardık!"

## Özel Event'ler ve Sürprizler

### Ultra Nadir Olaylar (1/1000 Şans)

**"Zaman Yolcusu":**
Gelecekten biri geliyor, "yakında büyük bir kriz" diyor. Gerçekten de 3 kart sonra kriz geliyor. Bu olayı görmek efsane.

**"Halk Kahramanı":**
Rastgele bir köylü kahramanlık yapıyor, halk onu kral yapmak istiyor. Sen onu ne yaparsın?

**"Paralel Evren":**
Bir an için alternatif gerçekliğe kayıyorsun, farklı kararlar almış krallığını görüyorsun. Sonra geri dönüyorsun.

**"Tanrılar Müdahalesi":**
(Ortaçağ'da) Gerçek bir mucize oluyor, herkes şahit. Faith aniden +30.

### Dönem Geçiş Anları

Bir dönemi başarıyla bitirdiğinde özel "Transition Event" geliyor:

**Ortaçağ → Rönesans:**
> "Yüzyıllar geçti. Torununun torunu şimdi tahtında. Dünya değişti. Sanat ve bilim yükseliyor. Sen hazır mısın?"

**Sanayi → Modern:**
> "İki dünya savaşı geride kaldı. Artık diktatörler değil, demokratlar yönetiyor. Oy toplama zamanı."

Bu geçişler sinematik hissettiriyor (sadece text ama güçlü yazımla).

## Monetization İçeriği

### Unlock Seçenekleri

**Free Tier:**
- Ortaçağ dönemi tam erişim
- 3 başlangıç senaryosu
- Daily challenge
- Reklam (her 3 game over'da bir, 15-30 saniyelik video)

**Premium Unlocks ($0.99-$2.99 each):**
- Rönesans Dönemi
- Sanayi Devrimi Dönemi
- Modern Dönem
- Gelecek Dönemi

**Special Scenarios ($0.99):**
- "Cadı Avı Başlangıcı" (Ortaçağ'da farklı event chain)
- "Fransız Devrimi" (Rönesans'ta alternatif başlangıç)
- "Nükleer Çağ" (Modern'de özel kriz)

**Cosmetic ($0.99):**
- Kart arka yüzü tasarımları (10+ çeşit)
- UI temaları (dark mode, minimalist, royal vb)
- Karakter portre paketleri (AI-generated veya illustrated)

**Ad Removal ($2.99):**
Ama rewarded ads kalıyor (oyuncunun seçimi):
- Game over'da revive (bir kez)
- Resource boost (bir kaynağı +10 yap acil durumda)

**Bundle ($6.99):**
- Tüm dönemler
- Tüm senaryolar
- Ad removal
- Exclusive "Time Lord" achievement

## Neden Bu Oyun Tutar?

### Psikolojik Çengeller

- **Zeigarnik Etkisi:** "Bir tur daha, bu sefer kazanacağım" → Kesintili görev tamamlama arzusu
- **Loss Aversion:** 4 barı dengeleme stresi → Kayıptan kaçınma içgüdüsü
- **Narrative Transportation:** Her oyun küçük bir hikaye → İnsanlar hikaye sever
- **Variable Rewards:** Her kart farklı event → Slot machine dopamine
- **Mastery Desire:** "Bu sefer mükemmel oynayacağım" → Gelişme hissi
- **Social Proof:** Daily challenge ve share → FOMO ve rekabet

### Erişilebilirlik

- Tek elle oynanır (swipe only)
- Kısa oturumlar (10-30 dakika)
- Öğrenmesi 30 saniye (kaydır, kaynakları dengele)
- Derinlik saatler sürer (200+ event, 5 dönem, stratejik derinlik)
- Dil bağımsız değil ama translation kolay (text-based)

### Viral Potansiyel

- **Shareable moments:** "Ya bu oldu bana" hikayeleri
- **Daily challenge:** Sosyal FOMO ve rekabet
- **Rare events:** "Bunu gördün mü?" FOMO
- **Discussion worthy:** "Hangi dönemi bitirdin?" "Ne stratejisi kullanıyorsun?"

## Özet: Oyuncu Deneyimi

Oyuncu Decision Kingdom'ı açıyor:

- **Dakika 1:** "Basit, kart kaydırıyorum."
- **Dakika 5:** "Vay be, kararlarım birbirini etkiliyor."
- **Dakika 15:** "Hay Allah, Military düşük, savaş gelirse bittim!"
- **Game Over:** "Şşş bir tur daha, bu sefer farklı yapacağım."
- **10. Oyun:** "Ortaçağ'ı bitirdim! Rönesans unlock oldu, yeni dönem görmek istiyorum!"
- **50. Oyun:** "Daily challenge'da rank 1'im, arkadaşıma attım screenshotu."
- **100. Oyun:** "O nadir 'Ejderha' event'ini hala görmedim, bulacağım!"

**Bu oyunun gücü:** Basit mekanik, derin duygusal deneyim, sonsuz hikaye olasılığı.
