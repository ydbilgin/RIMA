# RIMA — Artist Brief / Sanatçı Rehberi
*TR + EN | 2026-04-01*

> Bu belge, RIMA projesine dahil olacak bir piksel sanatçısına verilmek üzere hazırlanmıştır.
> Hem üretilmiş örnekleri hem de üretilmesi gereken varlıkların tam listesini içerir.
>
> This document is prepared for a pixel artist joining the RIMA project.
> It contains both produced examples and the complete list of assets to be created.

---

## OYUN NEDİR / WHAT IS THE GAME

**TR:** RIMA, top-down 2D bir roguelite'tır. Dead Cells görselliği + Hades anlatısı + Guild Wars 1 sınıf sistemi kombinasyonu. Oyuncu her ölümde başa döner ama güçlenerek devam eder. 8 farklı sınıf, çapraz sınıf kombinasyonları, boyutsal parçalanma teması.

**EN:** RIMA is a top-down 2D roguelite. Think Dead Cells visuals + Hades narrative + Guild Wars 1 class system. Players die and restart but grow stronger. 8 classes, cross-class combos, dimensional fracture theme.

---

## GÖRSEL KİMLİK / VISUAL IDENTITY

### Referans Oyunlar / Reference Games
- **Dead Cells** → pixel art kalitesi, dinamik ışık, post-processing
- **Hades** → güçlü glow/bloom, sıcak vurgu ışıkları, karanlık ambient
- **Hollow Knight** → karanlık ama derinlikli atmosfer
- **Enter the Gungeon** → top-down okunabilirlik, karakter silüet netliği

### RIMA Tema Özeti / RIMA Theme Summary

**TR:** Oyunun merkezi teması "The Fracturing" — paralel boyutların kasıtlı olarak kırılması. Her karakter, her düşman, her ortam bu kırılmanın bir ürünü. Görsel dil bunu taşımalı:

**EN:** The core theme is "The Fracturing" — the intentional breaking of parallel dimensions. Every character, enemy, and environment is a product of this rupture. The visual language must carry this:

| Element | TR | EN |
|---------|----|----|
| **Rift cracks** | Mavi/mor parlayan çatlaklar her sprite'ta | Blue/purple glowing cracks on every sprite |
| **Dark palette** | Koyu ambient, vurgu ışıkları belirgin | Dark ambient with strong accent lights |
| **Worn & broken** | Hiçbir şey yeni ve temiz değil | Nothing is new and clean |
| **Dimensional bleed** | Gerçeklik kayıyor, sınırlar bulanık | Reality slipping, borders blurring |

### Renk Paleti / Color Palette

**Per-Act Rift Colors:**
- **Act 1:** Cold blue `#4A90D9` / `#1E3A5F` — soğuk, bilinmez / cold, unknown
- **Act 2:** Purple-gold `#8B4FD8` / `#C9A227` — farkındalık / awareness
- **Act 3:** White-gold `#F5E642` / `#FFFFFF` — gerçek ortaya çıkıyor / truth revealed

**Base Palettes:**
```
Dark base:      #0D0D0F  #1A1A1E  #2A2A30
Stone/metal:    #3D3D45  #5A5A65  #7A7A88
Skin tones:     #8B5E3C  #A67850  #C49468
Rift blue:      #1E3A5F  #2E5A9F  #4A90D9  #7AB8F5
Rift purple:    #2D1B4E  #4A2B7A  #7040C0  #A070E8
```

---

## TEKNİK ÖZELLIKLER / TECHNICAL SPECS

| Özellik | Değer |
|---------|-------|
| **Motor** | Unity 6.3 LTS + URP 2D |
| **Karakter sprite boyutu** | 48×48 px |
| **Mob sprite boyutu** | 32×32 px (grunt), 48×48 px (elite), 64×64+ (boss) |
| **PPU (Pixels Per Unit)** | 48 |
| **Filter Mode** | Point (no filter) — her piksel net olmalı |
| **Kamera açısı** | Low top-down (Hades tarzı) |
| **Yön sayısı** | 8 yön (S, SW, W, NW, N, NE, E, SE) — kod rotasyonu YOK |
| **Arka plan** | Şeffaf PNG |
| **Renk modu** | RGBA |

### Önemli Kural / Important Rule
**TR:** Karakterler KOD ile döndürülmez. Her yön ayrı sprite seti olmalı. 8 yön = 8 ayrı çizim. Bu, pixel art oyunlarının standart yaklaşımıdır (Dead Cells, Enter the Gungeon).

**EN:** Characters are NOT rotated in code. Each direction must be a separate sprite set. 8 directions = 8 separate drawings. This is the standard approach for pixel art games (Dead Cells, Enter the Gungeon).

---

## OYUNCU KARAKTERLERİ / PLAYER CHARACTERS

### Genel Kural / General Rule
- Her karakterde zırhta/silahta **rift çatlak ışığı** (mavi veya mora)
- **Tek silah kuralı:** Hiçbir karakter iki bağımsız silah taşımaz (çift kılıç YOK)
- Yıpranmış, savaştan çıkmış görünüm — yeni ve parlak değil
- Görünür yüz veya en azından siluetten anlaşılır kimlik

---

### 1. WARBLADE
**TR:** Paralı asker, savaşçı sınıfı. Çift elle tutulan büyük greatsword, koyu demir zırh. Zırhta mavi rift çatlakları. Karmaşık saç, görünür savaş yorgunluğu.

**EN:** Mercenary warrior class. Two-handed greatsword held with both hands, dark iron plate armor. Blue rift cracks on armor. Messy hair, visible battle fatigue.

**Silah / Weapon:** Greatsword — iki el, her yönde tutarlı  
**Rift rengi / Rift color:** Mavi / Blue `#4A90D9`  
**Boyut / Size:** 48×48 px, 8 yön  

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle (nefes) | 4f | ⭐ Kritik |
| walk | 6-8f | ⭐ Kritik |
| run/dash | 6f | Yüksek |
| death | 4-6f | Yüksek |
| attack swing | 6-8f | Orta |

**Mevcut PixelLab üretimi / Current PixelLab output:**  
`ART/_ONIZLEME/warblade_S.png` — güney yönü örneği  
`ART/karakterler/warblade/pixellab/` — tüm yönler

---

### 2. ELEMENTALİST
**TR:** Büyücü sınıfı. Uzun staff, yırtık koyu roblar, ellerinde ve kollarında mavi/mor rift enerji çatlakları. Kapüşon aşağıda, yorgun yüz görünür.

**EN:** Mage class. Long staff, torn dark robes, blue/purple rift energy cracks on hands and forearms. Hood down, tired face visible.

**Silah / Weapon:** Staff — bir elde, her yönde tutarlı  
**Rift rengi / Rift color:** Mavi-mor / Blue-purple  
**Boyut / Size:** 48×48 px, 8 yön  

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle (nefes, enerji titreşimi) | 4f | ⭐ Kritik |
| walk | 6-8f | ⭐ Kritik |
| cast (fireball/spell) | 6f | Yüksek |
| death | 4-6f | Yüksek |

**Mevcut PixelLab üretimi / Current PixelLab output:**  
`ART/_ONIZLEME/elementalist_S.png`

---

### 3. SHADOWBLADE
**TR:** Suikastçı sınıfı. Tek hançer sağ elde, sol el serbest. Koyu deri zırh, kapüşon yüzü kısmen örtüyor. Bıçak kenarından mor rift enerjisi sızıyor.

**EN:** Assassin class. Single dagger in right hand, left hand free. Dark leather armor, hood partially covering face. Purple rift energy seeping along blade edge.

**Silah / Weapon:** Tek hançer — sağ elde, sol boş  
**Rift rengi / Rift color:** Mor / Purple `#7040C0`  
**Boyut / Size:** 48×48 px, 8 yön  

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle | 4f | ⭐ Kritik |
| walk (stealthy) | 6-8f | ⭐ Kritik |
| dash/blink | 4f | Yüksek |
| death | 4-6f | Yüksek |

**Mevcut PixelLab üretimi / Current PixelLab output:**  
`ART/_ONIZLEME/shadowblade_S.png`

---

### 4. RANGER
**TR:** Okçu/kaşif sınıfı. Uzun yay sol elde, sırtında sadak. Hafif deri zırh, kısa kapüşon, yıpranmış yüz görünür. Yay kirişinde mavi rift enerjisi parlar.

**EN:** Archer/scout class. Longbow in left hand, quiver on back. Light leather armor, short hood, weathered face visible. Blue rift energy glows on bowstring.

**Silah / Weapon:** Yay — sol elde, sadak sırtta  
**Rift rengi / Rift color:** Mavi / Blue  
**Boyut / Size:** 48×48 px, 8 yön  

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle | 4f | ⭐ Kritik |
| walk | 6-8f | ⭐ Kritik |
| shoot (draw+release) | 6f | Yüksek |
| death | 4-6f | Yüksek |

**Mevcut PixelLab üretimi / Current PixelLab output:**  
`ART/_ONIZLEME/ranger_S.png`

---

## DÜŞMANLAR / ENEMIES

### Tasarım Felsefesi / Design Philosophy

**TR:** RIMA'da genel canavar yok. Her düşman "The Fracturing"in bir ürünü. Soru: **"Kırılma sana ne yaptı?"** Üç kategori:
- **Fractured:** Bir zamanlar canlıydı, kırılma onu parçaladı ve yanlış birleştirdi
- **Rift-Born:** Çatlaklar olmadan var olamazdı
- **Emergent:** Hâlâ çıkıyor — tam burada değil, tam öte tarafta da değil

**EN:** There are no generic monsters in RIMA. Every enemy is a product of The Fracturing. The question: **"What did the breaking do to you?"** Three categories:
- **Fractured:** Was once alive, the rupture shattered and wrongly reassembled them
- **Rift-Born:** Could not exist without the dimensional cracks
- **Emergent:** Still emerging — not fully here, not fully there

---

### GRUNT TİER — 32×32 px, 4 yön (Act 1)

#### 1. SHARD WALKER
**Kategori:** Fractured  
**TR:** Parçalanmış bir savaşçının kalıntısı. Bedeni birden fazla boyuta dağıldı, geri dönerken parçalar yanlış sıraya girdi. Ne tam burada ne öte tarafta.

**EN:** Remnant of a shattered warrior. Their body scattered across dimensions; when it returned, the pieces came back in the wrong order. Neither here nor there.

**Görsel tarif / Visual description:**
- 8-12 adet kırık taş/kemik parçası insansı şekilde dizilmiş
- Parçalar arasında boşluklar — aralar karanlık
- Aralardan soğuk mavi rift ışığı sızıyor `#4A90D9`
- Baş kısmı en büyük parça, kollar ince şeritler
- Yürümede parçalar hafif sallanıyor, birbirinden ayrılıp toplanıyor

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle (parçalar titreşiyor) | 4f | ⭐ Kritik |
| walk (parçalar sallanıyor) | 6f | ⭐ Kritik |
| attack (parça fırlatma) | 4f | Yüksek |
| death (parçalar dağılıyor) | 4-6f | Yüksek |

---

#### 2. VOID THRALL
**Kategori:** Rift-Born  
**TR:** Rift'e çekilmiş bir ruh. Mor enerji içinde yarı görünür, etekleri boyutsal boşluğa dönüşüyor.

**EN:** A soul pulled into the rift. Half-visible in purple energy, robes dissolving into dimensional void.

**Görsel tarif / Visual description:**
- Yarı saydam siluet, `%60-70 opacity`
- Mor void enerjisi `#7040C0` vücuttan dışarı sızıyor
- Oyuk gözler mor parıltıyla
- Alt kısım çözülüyor, "zemin" yok ayakları için
- Hareket süzülme gibi, adım atmıyor

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle (süzülme, titreşim) | 4f | ⭐ Kritik |
| move (float) | 4-6f | ⭐ Kritik |
| attack (void blast) | 4f | Yüksek |
| death (çözülme) | 4-6f | Yüksek |

---

#### 3. SEAM CRAWLER
**Kategori:** Rift-Born  
**TR:** Boyutlar arasındaki çatlakta yaşayan böcek varlık. Bacaklarında mavi rift elektriği. Zemine yapışık, hızlı hareket eder.

**EN:** Insectoid creature living inside dimensional cracks. Blue rift electricity along legs. Flat and ground-hugging, fast movement.

**Görsel tarif / Visual description:**
- Altı köşeli bacak, eklemli ve ince
- Bacaklarda mavi elektrik çatlak efekti `#4A90D9`
- Koyu kitin kabuk, neredeyse siyah `#1A1A1E`
- Mandibles/çenede rift ışığı
- Çok alçak profil — top-down görüşte gövde düz

**Animasyonlar / Animations:**
| Animasyon | Frame | Öncelik |
|-----------|-------|---------|
| idle (bacaklar titreşiyor) | 4f | ⭐ Kritik |
| walk (hızlı böcek yürüyüşü) | 6f | ⭐ Kritik |
| attack (atılma) | 4f | Yüksek |
| death | 4f | Orta |

---

### ELİTE TİER — 48×48 px, 8 yön (Act 1-2)

#### 4. REMNANT HOST
**Kategori:** Fractured  
**TR:** İki varlık aynı bedende. Görsel: iki siluet üst üste, titreyen, birbiriyle çelişen hareketler.

**EN:** Two beings in one body. Visual: two silhouettes overlapping, flickering, conflicting movements.

**Boyut / Size:** 48×48 px  
*(Detaylı brief ilerleyen fazda eklenecek)*

---

## BOSS KARAKTERLERİ / BOSS CHARACTERS

*(Act 1 boss'ları için detaylı brief Faz 1 tamamlandıktan sonra hazırlanacak)*

**Planned:**
- **Iron Warden** — 64×64 px, 8 yön, Act 1 Boss A
- **Void Warden** — 64×64 px, 8 yön, Act 1 Boss B  
- **Chain Warden** — 64×64 px, 8 yön, Act 1 Boss C
- **Fractured King** — 128×128 px, 4 yön, Act 2 Boss

---

## ÜRETİM AKIŞI / PRODUCTION WORKFLOW

### PixelLab MCP ile Mevcut Workflow (Claude)
```
1. create_character(description, size, view="low top-down", n_directions=8)
   → ~3-5 dakika bekleme
   
2. animate_character(character_id, template_animation_id)
   → template: 1 gen/yön = 8 gen/animasyon
   → custom: 20-40 gen/yön (pahalı, sadece özel animasyonlar için)
   
3. ZIP indir → ART/karakterler/{isim}/pixellab/ ve
              RIMA/Assets/Sprites/Characters/{İsim}/
```

### Sanatçı için Önerilen Workflow (Manuel)
```
1. PixelLab BASE sprite üret (veya bu repo'daki örnekleri kullan)
2. Aseprite'ta aç → renk düzeltme, detay ekle, rift çatlakları güçlendir
3. 8 yön × her animasyon = ayrı frame set
4. Export: PNG sprite sheet veya ayrı frame'ler
5. Unity import: Pixels Per Unit = 48, Filter = Point, No Compression
```

### Dosya İsimlendirme / File Naming Convention
```
{karakter}_{yön}_{animasyon}_{frame}.png

Örnekler:
warblade_S_idle_01.png
warblade_SE_walk_03.png
shard_walker_N_attack_02.png

Kısaltmalar:
S=south, N=north, E=east, W=west
SE=south-east, SW=south-west, NE=north-east, NW=north-west
```

---

## ÖNCELIK LİSTESİ / PRIORITY LIST

### Faz 1 Demo için kritik (önce bunlar) / Critical for Phase 1 Demo
- [ ] Warblade: idle, walk, run, death (8 yön)
- [ ] Shard Walker: idle, walk, attack, death (4 yön)
- [ ] En az 1 environment tileset (Act 1 dungeon — soğuk taş, mavi rift çatlakları)

### Faz 1 tamamlanma için / For Phase 1 completion
- [ ] Elementalist, Shadowblade, Ranger: idle + walk (8 yön)
- [ ] Void Thrall + Seam Crawler: idle + walk (4 yön)
- [ ] Skill efekt sprite'ları (Iron Charge flash, Cleave arc)

### İlerleyen fazlar için / For later phases
- [ ] Tüm 8 class tam animasyon seti
- [ ] Boss sprite'ları
- [ ] Act 2-3 environment tileset'leri
- [ ] UI elemanları (HP bar dekorasyonu, skill ikonları)
- [ ] VFX sprite'ları (rift açılma, parçacık efektleri)

---

## MEVCUT ÖRNEK DOSYALAR / EXISTING EXAMPLE FILES

```
ART/_ONIZLEME/
├── warblade_S.png       ← güney (sana bakan)
├── warblade_E.png       ← doğu (sağa bakan)  
├── warblade_N.png       ← kuzey (sırt)
├── elementalist_S.png
├── elementalist_E.png
├── shadowblade_S.png
├── shadowblade_E.png
├── ranger_S.png
└── ranger_E.png

ART/karakterler/warblade/pixellab/
├── warblade_south_BASE.png
├── warblade_north_BASE.png
├── warblade_east_BASE.png
├── warblade_west_BASE.png
└── ... (tüm 8 yön)

RIMA/Assets/Sprites/Characters/Warblade/
└── (Unity'ye hazır kopyalar)
```

---

## NOTLAR / NOTES

**TR:**
- PixelLab ile üretilen tüm sprite'lar **başlangıç referansı** olarak kullanılabilir. Aseprite'ta üzerinden geçmek, detay eklemek, tutarsızlıkları düzeltmek bekleniyor.
- Warblade'in mevcut sprite'ında silah tutuşu bazı yönlerde tutarsız olabilir — bu düzeltilmeli.
- Tüm karakterlerde rift çatlak ışığı `zırh/silah üzerinde` olmalı, kumaş üzerinde değil (kumaş sadece renk olarak koyudur).
- Animasyonlar loop'lamalı (idle, walk) veya tek seferlik (attack, death) olarak açıkça belirtilmiştir.

**EN:**
- All PixelLab-generated sprites are **starting references**. Touching up in Aseprite, adding detail, and fixing inconsistencies is expected.
- Warblade's current sprite may have inconsistent weapon grip across some directions — this needs fixing.
- Rift crack glow should be on **armor/weapons**, not fabric (fabric is just dark in color).
- Animations are either looping (idle, walk) or one-shot (attack, death) as specified above.

---

*İletişim için: Laureth (solo dev) | Proje: RIMA roguelite | Motor: Unity 6.3 LTS*
*For contact: Laureth (solo dev) | Project: RIMA roguelite | Engine: Unity 6.3 LTS*
