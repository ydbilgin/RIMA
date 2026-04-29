# RIMA — Hikaye, Akt Yapısı ve Oynanış Tasarımı
*Son güncelleme: 2026-04-01 —claude*

> Bu belge üç şeyi bir arada tutar: **anlatı**, **oynanış ritmi** ve **referans oyunlarla karşılaştırma**.
> Kod yazarken değil, tasarım kararı alırken oku.

---

## BÜYÜK RESİM — NE ANLATIYOR BU OYUN?

RIMA bir şey soruyor: **Yaptığın bir hatayı hatırlamıyorsan, hâlâ suçlu musun?**

Oyuncunun karakteri, boyutları kıran kararı veren kişinin parçası. Nexus Core ise o kararın donmuş, değişemeyen, pişman olamayan yarısı. Her run, bir hatırayı geri kazanma çabası. Her ölüm, bir tekrar — Loop bir ceza değil, mühür.

**Hades ile karşılaştırma:**
- Hades'te Zagreus babasından kaçmak ister. Her ölümde eve döner, konuşur, biraz daha öğrenir. Hikaye run'ların arasında ilerler.
- RIMA'da sen sebebini unutmuş birisin. Her ölümde The Threshold'a dönersin, NPC'ler sana biraz daha açılır. Hikaye run'ların arasında değil, **run'ların içinde** de ilerler — düşmanlar sana şeyler söyler, ortamda kırık anılar bulunur.

---

## AKT HİKAYESİ

### PROLOG — İlk Uyanış (Tutorial / Run 1)
**Oynanışta ne olur:** Karanlık. Bir çatlaktan geçiyorsun. The Threshold'a düşüyorsun.
**The Ferryman** sana bakar: *"Yine geldin. Ama sen bunu hatırlamıyorsun, değil mi?"*

Oyuncu hiçbir şey hatırlamıyor. The Ferryman açıklamıyor — sadece geçmeni sağlıyor.

**Tasarım notu:** İlk run kasıtlı olarak zorlanır. Oyuncu ölecek. Bu, Hades'teki gibi bir "seni sisteme alıştırma ölümü". Ama RIMA'da ölüm diyaloğu şöyle: *"Hâlâ hazır değilsin. Ama geri geleceksin."*

---

### AKT 1 — "Eşik" *(6-7 oda + Boss)*

**Atmosfer:** Soğuk mavi rift ışığı. Parçalanmış taş koridorlar. Yıkık duvarların arasından farklı boyutların parçaları görünüyor — bir orman tavanı, bir şehrin zemini, bir gökyüzü yan yana.

**Oyuncu ne hisseder:** "Nereden geliyorum? Neden buradayım? Bu yaratıklar ne?"

**Anlatı kırıntıları (run içinde):**
- Shard Walker'lar zaman zaman durup sana bakıyor, sonra saldırıyor — sanki tanıyorlar ama karar veremiyorlar.
- Bazı odaların duvarlarında eski yazılar: *"Bu yolu bilen biri vardı."*
- Oda sonlarında küçük Shattered Echo kırıkları — her biri bir flash anı: bir kılıç, bir karar, bir yüz.

**Düşman teması (The Fracturing'in eserleri):**
- **Shard Walker** — Parçalanmış bir savaşçının kalıntısı. Parçalar arasından mavi rift ışığı sızıyor.
- **Void Thrall** — Rift'e çekilmiş, yarı var olan ruh. Yarı saydam, mor enerji.
- **Seam Crawler** — Boyutlar arasındaki çatlakta yaşayan böcek varlık. Işıkçatlak bacaklar.

**Act 1 Boss — 3 rotasyonlu (hangisi geldiği gizli):**
| Boss | Tema | Anlatı |
|------|------|--------|
| **Iron Warden** | Eski düzeni koruyan, parçalanmış ama görevini sürdüren kale bekçisi | *"Sen bu kapıdan geçemezsin. Geçemezdin. Ama yine denedin."* |
| **Void Warden** | Rift enerjisine yenik düşmüş, artık boyutlar arasında yarı var | *"Biz aynı şeyiz sen ve ben. Sen sadece henüz fark etmedin."* |
| **Chain Warden** | Birden fazla varlığı zincirle bağlamış, onlardan besleniyor | *"Her şey bir şeye bağlı. Sen de."* |

**The Threshold'a dönüş sonrası:**
- The Ferryman: *"Act 1'i geçtin. Artık seçmen var."*
- İlk defa **secondary class seçimi** — sadece 2 seçenek, 1'ini al.
- Vrel sana ilk kez özel bir şey söyler: *"Bu silahı daha önce tutmuş biri vardı."*

---

### AKT 2 — "Çatlak Hattı" *(8-9 oda + Boss)*

**Atmosfer:** Mor-altın rift ışığı. Boyutlar artık daha fazla iç içe geçmiş. Bir odanın tavanı yağmurlu bir şehir, zemini yanmış bir orman. Gerçeklik tutarsız.

**Oyuncu ne hisseder:** "Bu yer beni tanıyor gibi. Düşmanlar değişti. Ben değiştim mi?"

**Anlatı kırıntıları:**
- Secondary class'ın skill'leri, karakterin geçmiş yüzlerini temsil eder — *Shattered Echoes sadece para değil, sen topladıkça daha bütün oluyorsun.*
- Bazı düşmanlar artık seni tanıyor ve kaçıyor.
- Sister Mourne sana şunu söyler: *"Senin kaybın farklı. Başkaları neyi kaybettiklerini biliyor."*

**Act 2 özel mekanizması — Fractured King:**
10 saniye içinde 4 farklı skill kullanırsan boss faz 2'ye geçer.
**Anlatı anlamı:** Fractured King birden fazla kral kırığından oluşuyor — her faz, başka bir versiyonu. Sana şunu söylüyor:
*"Sen de çokluğun içindesin. Hangisi gerçek olduğunu hâlâ seçmedin."*

**Faz 2 tetiklenince:** Ekran bir anlığına kararır. *"Karar verme vakti."* Sonra savaş devam eder — ama boss artık senin skill'lerine adapte olmuştur.

**The Threshold'a dönüş:**
- The Cartographer haritasında yeni bir alan açılır: *"Bu yol her zaman buradaydı. Sadece görmüyordun."*
- **Cross-class Ultimate** açılır.
- The Ferryman ilk kez oturur. Sana bakar. Bir şey söylemek ister ama söylemez.

---

### AKT 3 — "Öz" *(10-11 oda + Boss)*

**Atmosfer:** Altın-beyaz rift ışığı. Her şey daha az somut. Zeminler yarı saydam. Düşmanlar artık sana çok tanıdık geliyor — önceki run'lardaki versiyonların mı bunlar?

**Oyuncu ne hisseder:** "Bu benim anım. Bu benim hatam."

**Anlatı kırıntıları:**
- Anı flash'ları artık daha uzun. Bir karar anı görüyorsun — ama yüzler hâlâ bulanık.
- **Grudge sistemi burada anlam kazanır:** Belirli bir düşmanı yeterince kez öldürdüysen, o düşman sana bir şey söyler ölmeden önce. Bunu sadece sen duyarsun.
- The Ferryman: *"Run 9'a geldim. Sana söylemem gereken bir şey var."*

**Act 3 Boss — Hollow Sovereign:**
Build tag'lerini okur ve sana özgün counter oluşturur.
**Anlatı:** Hollow Sovereign "boşluk"tur — ne karar veremediysen o. Seni taklit eder ama duygusuz, sadece strateji. *"Sen hâlâ tereddüt ediyorsun. Ben edemiyorum."*

---

### FİNAL — Nexus Core

**Açıklama:** Nexus Core senin diğer yarın. Kararın donmuş iradesi. Değişemiyor, şüphe edemiyor. Seni "kopyalamıyor" — primary class'ını taklit eder, cross-class zayıflığını hedef alır. **Senden daha fazlasını biliyor çünkü aynı kaynaktan geldi.**

**Savaş öncesi diyalog:**
*"Sen hatırladın. Bu yeterliydi. Ama ne yapacaksın şimdi?"*

**Üç kapanış:**

| Seçim | Eylem | Sonuç |
|-------|-------|-------|
| **"Kal"** | Bilinçle nöbet. Loop'u kabul ediyorsun. | The Threshold devam eder. The Ferryman: *"İlk kez biri kendi isteğiyle kaldı."* |
| **"Kır"** | Mührü kır, dünyaları aç. | Bazı NPC'ler yok olur. Tüketen şeyin gerçekten gittiğini bilmiyorsun. Belirsiz özgürlük. |
| **"Taşı"** | Gerçeği öğrendin ve yine de devam etmeyi seçiyorsun. | Neden koştuğunu artık biliyorsun. En zor ama en dürüst kapanış. |

---

## META-PROGRESSION — CONFLUENCE ENGINE

**Hades karşılaştırması:**
- Hades: "Mirror of Night" → Darkness harcar, kalıcı stat boost alırsın
- RIMA: **"Confluence Engine"** → Shattered Echoes harcar, kalıcı unlock/boost alırsın

The Threshold'daki merkezi makine — parçalanmış dünyaların enerjisini toplar. Her run sonrası buraya Shattered Echoes yatırırsın.

### Confluence Engine Upgrade Ağacı

**Temel (her run için):**
| Upgrade | Açıklama | Maliyet |
|---------|----------|---------|
| Vital Core | Başlangıç HP +10 (max 5 kez) | 20 Echo / kez |
| Echo Attunement | Run başına +1 ekstra skill offer | 50 Echo |
| Rift Step | Dash CD -0.5s kalıcı | 40 Echo |
| Fracture Memory | Boss odalarında harita görünür | 60 Echo |

**NPC İlişkileri (Hades diyalog sistemi gibi):**
| NPC | Unlock koşulu | Kazanım |
|-----|--------------|---------|
| The Ferryman | 3 run tamamla | Nadir skill offer oranı +5% |
| Vrel | 5 run tamamla | Run başında 1 ücretsiz upgrade |
| Sister Mourne | 7 run, bir kez ölmeden Act 1 bitir | Ölüm anında 1 kez +20% HP recovery |
| The Cartographer | Tüm oda tiplerini gör | Harita her zaman açık |

**Sınıf Açma:**
- Başlangıçta: Warblade + Elementalist
- 10 Echo: Shadowblade
- 15 Echo: Ranger
- 25 Echo: Ravager, Hexer
- 40 Echo: Summoner, Brawler

---

## ZORLUK SİSTEMİ — RIFT SURGE

**Hades karşılaştırması:**
- Hades: "Heat" sistemi — her eklenen "Pact" zorluğu artırır, ama ödül de artar
- RIMA: **"Rift Surge"** — opsiyonel zorluk modifierleri, run başında seçilir

Her aktif modifier = +daha fazla Shattered Echoes + nadir skill offer şansı.

### Rift Surge Modifierleri

| Modifier | Etki | Echo Bonus |
|----------|------|------------|
| **Fractured Path** | Oda layoutları daha karmaşık, çıkışlar gizli | +15% |
| **Echo Hunger** | Düşmanlar öldürdüğünde Shattered Echo çalar (geri alınabilir) | +20% |
| **Rift Blind** | Harita gizli, sadece geçtiğin odalar görünür | +10% |
| **Grudge Storm** | Düşmanlar daha hızlı Grudge kazanır (run içinde güçlenirler) | +25% |
| **No Mercy** | Ölüm animasyonu yok, anında The Threshold | +15% |
| **Mirrored Core** | Nexus Core'un faz sayısı +1 | +40% |

**Maksimum Surge:** Aynı anda 3 aktif modifier (strateji: hangilerini birleştireceğin)

---

## OYNANIŞTA HİKAYE İLERLEMESİ

**Hades karşılaştırması:**
- Hades: Her NPC her run sonrası yeni bir şey söyler. Ölüm bile diyalog tetikler.
- RIMA: Aynı prensip + **run içi anlatı**

### Diyalog Tetikleyicileri

| Tetikleyici | Örnek diyalog |
|------------|---------------|
| İlk kez bir boss öldürürken | Boss son sözünü söyler — her boss için benzersiz |
| 5. kez aynı düşmanı öldürürsen | O düşman türü sana bir şey fısıldar |
| Bir odada 30 saniye geçirirsen | Ortam sesi değişir, bir anı kırıntısı oynar |
| Act 2'ye ilk kez geçince | The Ferryman ile otomatik kısa sahne |
| Run 9 tamamlandığında | The Ferryman oturur, uzun anlatı sahnesi |
| Tüm 3 kapanışı gördükten sonra | The Ferryman'ın gerçek ismi ortaya çıkar |

### Çevresel Anlatı (Environmental Storytelling)
Her act'in her odası, The Fracturing'in başka bir boyutundan bir kırık. Görsel detaylar:
- Act 1: Antik şehir koridorları — mavi rift çatlakları, soğuk taş
- Act 2: Boyutların birbirine geçtiği alanlar — mor enerji, tutarsız geometri
- Act 3: Saf rift uzayı — gerçeklik yok, sadece enerji ve anılar

---

## OYUN DÖNGÜSÜ — ÖZET (Hades Karşılaştırmalı)

```
[HADES]          [RIMA]
─────────────────────────────────────────────────────
Ev'den çık    →  The Threshold'dan çık
Boon topla    →  Skill draft yap
Ölürsen ev'e  →  The Threshold'a dön
Konuş/Öğren   →  Konuş/Öğren + Confluence Engine'e Echo yatır
Tekrar dene   →  Rift Surge seç → tekrar dene
Elysium/Chaos →  Act 1 / Act 2 / Act 3
Hades boss    →  Nexus Core
Epilog açılır →  Kapanış seçimi + yeni diyalog katmanı
```

**RIMA'nın Hades'ten farkı:**
1. **Dual class** — Act 1 sonunda secondary class eklenir, build dinamiği ikiye katlar
2. **Run içi anlatı** — düşmanlar konuşur, ortam konuşur, sadece hub değil
3. **Grudge** — düşmanlar seni hatırlar, uyum sağlar
4. **Rift Surge** — Heat sistemi ama daha modüler ve strateji odaklı
5. **Üç kapanış** — Hades'te tek kapanış var, RIMA'da seçim run 10+ sonrası

---

## PIXEL ART + HİKAYE UYUMU

Görsel dil, anlatıyı taşımalı:

| Anlatı öğesi | Görsel karşılığı |
|-------------|-----------------|
| The Fracturing | Her sprite'ta rift çatlak ışığı |
| Act 1 (bilinmezlik) | Soğuk mavi palet, düşük ışık |
| Act 2 (farkındalık) | Mor-altın mix, daha parlak |
| Act 3 (gerçek) | Altın-beyaz, neredeyse aşırı parlak |
| Nexus Core | Senin sprite'ının renklerini kullanır ama ters |
| Shattered Echoes | Her toplandığında karakterin biraz daha parlar |

---

*Detaylı sistem tasarımları: `SINIF_VE_SKILL_KARAR_BELGESI.md`, `BOSS_SISTEMI.md`, `MOB_TASARIMI.md`*
*Görsel detaylar: `GORSEL_YONERGE.md`, `ART/URETIM_PLANI.md`*
