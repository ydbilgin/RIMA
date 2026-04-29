# ASEPRITE + PIXELLAB — VARLIK LİSTESİ VE DOSYA ADLARI
*Bu dosya: Her sprite için dosya adı, boyut, frame sayısı, FAZ planı*

> ⚠️ **PROMPT VE WORKFLOW BİLGİSİ İÇİN:** `GORSEL_STiL_VE_URETIM_REHBERI.md` kullan.
> Bu dosyadaki PixelLab prompt örnekleri GÜNCEL DEĞİL — oradaki format doğru.
>
> **Düzeltmeler (resmi PixelLab dökümanından):**
> - "pixel art" prompta yazılmaz — model zaten biliyor
> - Boyut (64x64) prompta yazılmaz — arayüzden seçilir
> - Doğru format: doğal dil cümlesi ("A heavily armored warrior knight...")
> - Canvas boyutları 64×64 olarak güncellendi (48→64, PixelLab zorunluluğu)

---

## ÖNCE OKU — BU REHBER NE ANLATIYOR?

Bu rehber sana şunu söylüyor:
- Hangi dosyayı ne zaman yapacaksın
- Aseprite'ta canvas'ı nasıl açacaksın
- PixelLab'da hangi skeleton'ı seçeceksin
- Tam olarak ne yazacaksın (prompt)
- Kaç frame çıkacak
- Dosyayı nasıl adlandıracaksın
- Unity'e nasıl aktaracaksın

**Takip et: FAZ 1 → FAZ 2 → ... → FAZ 6. Sıra önemli.**

---

## KISIM 0 — KURULUM VE GENEL AYARLAR

### Aseprite Genel Ayarları (bir kere yap, hep böyle kal)

```
Aseprite'ı aç.
Edit → Preferences → New File:
  Width:  48
  Height: 48
  Color Mode: RGBA
  Background: Transparent

Edit → Preferences → Grid:
  Width:  16
  Height: 16
  (Grid görünür olsun — View → Show Grid [G tuşu])
```

### PixelLab Extension — Nasıl Açılır?

```
Aseprite'ta üst menü → Extensions → PixelLab
(Yoksa: Edit → Preferences → Extensions → "Add Extension" → pixellab.aseprite dosyasını seç)

PixelLab paneli açılınca 3 sekme görürsün:
  [Generate]   → Sıfırdan sprite üret (prompt gir, resim çıkar)
  [Animate]    → Mevcut sprite'a animasyon ekle (skeleton seç)
  [Img2Img]    → Mevcut sprite'ı değiştir/varyasyon üret
```

### PixelLab — Animate Sekmesi Nasıl Çalışır (Skeleton İşlemi)

```
1. Canvas'ta karakterin FRONT pose'u çizilmiş olmalı (tek frame, tek layer)
2. PixelLab → [Animate] sekmesini aç
3. "Skeleton Type" alanından seç:
     - "Humanoid"       → İnsan vücutlu karakterler (tüm sınıflar için bu)
     - "Quadruped"      → 4 ayaklı yaratıklar (bazı boss'lar için)
     - "Flying"         → Uçan yaratıklar
     - "Custom"         → Özel (gerek olmayacak)
4. "Fit Skeleton to Sprite" butonuna bas → PixelLab sprite'ına kemik yerleştirir
5. Kemikleri gerekirse elle ayarla (sürükle-bırak)
6. "Animation" dropdown'ından animasyon tipini seç
7. "Generate" butonuna bas
8. Frame'ler otomatik oluşur — her frame ayrı layer olarak gelir
9. File → Export Sprite Sheet → ayarlar aşağıda
```

### Export (Spritesheet) Ayarları — HEP AYNI KUL

```
File → Export Sprite Sheet:
  Layout:
    Sheet type: "By Rows"
    Columns: animasyona göre değişir (aşağıda her biri için yazılı)
  Sprite:
    Layers: Visible layers only
    Frames: All frames
  Output:
    Output file: evet (dosya adı aşağıda her biri için yazılı)
    JSON data: evet (Unity'de kullanmak için)
    JSON style: Hash
  ✅ "Trim" işaretleme (gereksiz boş alan kessin)
  ✅ "Extrude" 1px (edge artifact önler)
```

### Dosya İsimlendirme Kuralı (HİÇ BOZMA)

```
[sınıf/obje]_[yön]_[animasyon].aseprite     ← Aseprite çalışma dosyası
[sınıf/obje]_[yön]_[animasyon].png          ← Export edilmiş spritesheet
[sınıf/obje]_[yön]_[animasyon].json         ← Spritesheet verisi (Unity için)
[sınıf/obje]_[yön]_[animasyon]_NormalMap.png ← Normal map (Laigter ile)

Örnekler:
  warblade_front_idle.aseprite
  warblade_front_idle.png
  grunt_front_walk.png
  boss_01_front_attack.png
```

### Renk Paleti (HER DOSYADA KULLAN)

```
Ortam renkleri (arkaplan, zemin, duvarlar):
  #0d0d1a  (neredeyse siyah mavi)
  #1a1a2e  (koyu lacivert)
  #2d2d4a  (orta lacivert)
  #4a4a6a  (açık lacivert-gri)

Karakter vurgu renkleri (ortamdan ayrışmalı):
  #c8a84b  (altın)
  #e05252  (kırmızı)
  #52a8e0  (mavi parlak)
  #a852e0  (mor parlak)

Skill efekt renkleri (EN PARLAK BUNLAR):
  #ffffff  (beyaz flash)
  #ffe066  (sarı-altın enerji)
  #ff4444  (kırmızı ateş)
  #44aaff  (buz mavisi)
  #aa44ff  (mor büyü)
  #44ff88  (yeşil zehir)
```

---

## KISIM 1 — NORMAL MAP WORKFLOW (Laigter ile)

Her karakter ve boss sprite'ı için normal map zorunlu. Şöyle yapılır:

```
1. Laigter'ı aç (ücretsiz indir: github.com/azagaya/laigter)
2. Sprite PNG'yi Laigter'a sürükle
3. Sol panelde ayarlar:
     Depth: 0.6-0.8 (karakterler için)
     Depth: 0.3-0.5 (zemin tile'ları için — daha düz)
4. "Export" butonuna bas
5. Dosya adını: [orijinal_dosya_adı]_NormalMap.png olarak kaydet
6. Unity'de Sprite'ı seç → Inspector'da "Secondary Textures" → Normal Map slot'una ekle
```

---

## FAZ 1 — CORE LOOP ART LİSTESİ (Ay 1)

> Hedef: Oynanabilir prototip. Warblade + 1 düşman tipi + 1 oda görünümü.
> Sanatın %70'i placeholder olabilir — sistemi çalıştırmak önce gelir.

---

### 1A. WARBLADE — Oyuncu Karakteri

**Canvas:** 64×64px | **Yönler:** Front, Back, Left (Right = flip) | **Toplam dosya:** 24

#### 1A-1. WARBLADE — FRONT POSE (Referans / Iskelet Kurulum Dosyası)

```
Dosya adı: warblade_front_BASE.aseprite
Canvas: 64×64px, transparent background

Aseprite'ta çiz:
  - Kalın, kısa boylu bir savaşçı figürü
  - Omuzlarda zırh plakaları
  - Sağ elde büyük kılıç (greatsword, gövdenin 1/3'i kadar uzun)
  - Sol elde kalkan (omzuna yaslanmış pozisyon)
  - Renkler: #c8a84b (zırh kenarları altın), #4a4a6a (zırh plakaları gri-mavi), #e05252 (pelerin veya kemer detayı)
  - Yüz: 2-3 piksel göz, maske veya miğfer
  - Bu dosyayı KAYDET — diğer tüm Warblade animasyonlarının başlangıç noktası

PixelLab → [Generate] ile alternatif:
  Prompt: "top-down 2d pixel art warrior knight, heavy armor, greatsword, shield on back,
           front view, 64x64, dark fantasy, medieval plague aesthetic, transparent background,
           Children of Morta style, detailed, no background"
  Size: 64×64
  Style: Pixel Art
```

---

#### 1A-2. WARBLADE FRONT — İDLE (Bekleme Animasyonu)

```
Dosya adı: warblade_front_idle.aseprite → warblade_front_idle.png

Canvas: 64×64px
Frame sayısı: 4 frame
FPS: 6 (her frame 166ms)

PixelLab → [Animate]:
  Skeleton Type: Humanoid
  "Fit Skeleton to Sprite" bas (warblade_front_BASE sprite'ı açıkken)
  Animation: "Idle Breathing"
  Frames to generate: 4
  Intensity: Low (hafif nefes hareketi, çok sallama)
  Generate bas

El ile düzeltilecekler:
  - Kılıç ucunun hafif titremesi (frame 2 ve 4'te 1px aşağı)
  - Pelerin hafif sallanma (varsa)

Export: 4 columns × 1 row = 192×48px spritesheet
Spritesheet dosyası: warblade_front_idle.png
```

---

#### 1A-3. WARBLADE FRONT — WALK (Yürüyüş)

```
Dosya adı: warblade_front_walk.aseprite → warblade_front_walk.png

Canvas: 64×64px
Frame sayısı: 6 frame
FPS: 8

PixelLab → [Animate]:
  Skeleton Type: Humanoid
  Animation: "Walk Cycle"
  Frames: 6
  Intensity: Medium
  Direction: Forward (kameraya doğru yürüyüş — zaten front view)
  Generate bas

El ile düzeltilecekler:
  - Kalkan yürüyüşe göre hafif sallanmalı
  - Zırh sesi varmış gibi ağır adım hissi (frame 1 ve 4'te vücut 1px aşağı)

Export: 6 columns × 1 row = 288×48px spritesheet
```

---

#### 1A-4. WARBLADE FRONT — ATTACK 1 (Temel Saldırı / Cleave)

```
Dosya adı: warblade_front_attack1.aseprite → warblade_front_attack1.png

Canvas: 64×64px
Frame sayısı: 5 frame
FPS: 12 (hızlı animasyon)

PixelLab → [Animate]:
  Skeleton Type: Humanoid
  Animation: "Sword Slash Horizontal"
  Frames: 5
  Intensity: High
  Generate bas

Frame yapısı (el ile düzeltmek istersen referans):
  Frame 1: Hazırlık (kılıç geri çekilmiş, vücut gergin)
  Frame 2: Swing başlangıcı (kılıç soldan sağa gidiyor)
  Frame 3: Swing ortası (kılıç maksimum hızda — HITBOX FRAME BU)
  Frame 4: Swing bitiş (kılıç sağda, vücut ileri eğilmiş)
  Frame 5: Recovery (yavaş normale dönüş)

NOT: Frame 3'ün üstüne ayrı bir layer aç, "HITBOX" yaz ve kırmızı dikdörtgen çiz.
     Bu Unity'de hitbox zamanlaması için referans — export'ta bu layer'ı kapat.

Export: 5 columns × 1 row = 240×48px spritesheet
```

---

#### 1A-5. WARBLADE FRONT — ATTACK 2 (Ground Stomp / CC Saldırı)

```
Dosya adı: warblade_front_attack2.aseprite → warblade_front_attack2.png

Canvas: 64×64px
Frame sayısı: 6 frame
FPS: 10

PixelLab → [Animate]:
  Skeleton Type: Humanoid
  Animation: "Ground Slam"
  Frames: 6
  Intensity: High
  Generate bas

Frame yapısı:
  Frame 1-2: Kılıç yukarı kaldırılıyor (slow windup)
  Frame 3: Kılıç en üstte (neredeyse durmuş — 2 frame bu pozisyonda kalabilir)
  Frame 4: Zemine iniyor (HITBOX FRAME)
  Frame 5: Zemine vurdu — impact pose (vücut öne eğilmiş)
  Frame 6: Recovery

Export: 6 columns × 1 row = 288×48px
```

---

#### 1A-6. WARBLADE FRONT — DASH (Kaçış)

```
Dosya adı: warblade_front_dash.aseprite → warblade_front_dash.png

Canvas: 64×64px
Frame sayısı: 4 frame
FPS: 16 (çok hızlı)

PixelLab → [Animate]:
  Skeleton Type: Humanoid
  Animation: "Forward Dash"
  Frames: 4
  Intensity: High
  Generate bas

El ile ekle:
  - Frame 2-3: Hafif motion blur etkisi — karakterin arkasına 2-3px'lik ghost copy (opacity %30)
  - Renk: #52a8e0 (mavi enerji parlaması)

Export: 4 columns × 1 row = 192×48px
```

---

#### 1A-7. WARBLADE FRONT — HIT (Hasar Alınca)

```
Dosya adı: warblade_front_hit.aseprite → warblade_front_hit.png

Canvas: 64×64px
Frame sayısı: 3 frame
FPS: 12

PixelLab → [Animate]:
  Animation: "Hit Stagger"
  Frames: 3
  Generate bas

El ile ekle:
  - Frame 1: Tüm sprite #ffffff (beyaz flash — damage almış hissi)
  - Frame 2: Normal sprite ama hafif geri itilmiş (knockback)
  - Frame 3: Normale dönüş

Export: 3 columns × 1 row = 144×48px
```

---

#### 1A-8. WARBLADE FRONT — DEATH (Ölüm)

```
Dosya adı: warblade_front_death.aseprite → warblade_front_death.png

Canvas: 64×64px
Frame sayısı: 8 frame
FPS: 8 (yavaş, dramatik)

PixelLab → [Animate]:
  Animation: "Death Fall Forward"
  Frames: 8
  Intensity: Medium
  Generate bas

Son frame: Sadece "corpse" — yerde yatan sprite. Bu frame 4-5 saniye kalır, sonra yavaşça opacity 0'a iner (kod tarafında yapılır).

Export: 8 columns × 1 row = 384×48px
```

---

#### 1A-9 ile 1A-24: BACK VE LEFT POZ ANIMASYONLARI

Yukarıdaki tüm 8 animasyonu (idle, walk, attack1, attack2, dash, hit, death + 1 ekstra: rage_activate) Back ve Left yönleri için tekrarla.

```
Back pose için:
  PixelLab prompt eklentisi: "back view, seen from behind"
  Veya front sprite'ı aynada çevir, arkasını yeniden çiz (daha doğru olur)

Left pose için:
  PixelLab → [Animate]: "Turn 90 degrees left" veya
  Front sprite'ı yan profile çevir, elle düzelt

Dosya isimleri:
  warblade_back_idle.png
  warblade_back_walk.png
  warblade_back_attack1.png
  warblade_back_attack2.png
  warblade_back_dash.png
  warblade_back_hit.png
  warblade_back_death.png
  warblade_left_idle.png
  warblade_left_walk.png
  warblade_left_attack1.png
  warblade_left_attack2.png
  warblade_left_dash.png
  warblade_left_hit.png
  warblade_left_death.png

NOT: Right = Left'in yatay flip'i. Unity'de SpriteRenderer.flipX = true ile yapılır.
     Right için ayrı dosya üretmene gerek YOK.
```

---

#### 1A-BONUS: WARBLADE — RAGE ACTIVATE

```
Dosya adı: warblade_front_rage.aseprite → warblade_front_rage.png

Canvas: 64×64px
Frame sayısı: 6 frame
FPS: 10

PixelLab → [Animate]:
  Animation: "Power Up Stance"
  Frames: 6
  Generate bas

El ile ekle:
  - Frame 1-2: Normal
  - Frame 3-4: Vücuttan kırmızı enerji parlaması (#e05252, opacity düşük büyük glow)
  - Frame 5-6: Tam Rage pose — kılıç yukarı, vücut genişlemiş, kırmızı aura sabit

Export: 6 columns × 1 row = 288×48px
```

---

### 1B. GRUNT — İlk Düşman (Temel Wave Düşmanı)

**Canvas:** 32×32px | **Yönler:** Front, Left (Back genellikle gerek yok, Front flip olur)

**Konsept:** Basit zombi/veba kurbanı. Maske yok, yırtık kıyafet, yavaş ama ısrarcı.

```
Renk paleti (düşman):
  #2a1a1a  (koyu kahve — giysi)
  #8a6a5a  (soluk ten rengi)
  #4a2a2a  (kan/yara rengi)
```

#### 1B-1. GRUNT FRONT — BASE SPRITE

```
Dosya adı: grunt_front_BASE.aseprite

PixelLab → [Generate]:
  Prompt: "top-down 2d pixel art zombie peasant enemy, plague victim, ragged clothes,
           front view, 32x32, dark medieval fantasy, transparent background,
           simple design, clear silhouette"
  Size: 32×32
  Style: Pixel Art

Üret, beğenmezsen "Regenerate" bas (birkaç kez dene).
En net siluetli olanı seç.
```

#### 1B-2. GRUNT FRONT — İDLE

```
Dosya adı: grunt_front_idle.png
Frame sayısı: 4 frame | FPS: 5

PixelLab → [Animate]:
  Skeleton: Humanoid
  Animation: "Idle Swaying" (hafif ileri geri sallanma — zombi hissi)
  Frames: 4
  Intensity: Low

Export: 4 columns = 128×32px
```

#### 1B-3. GRUNT FRONT — WALK

```
Dosya adı: grunt_front_walk.png
Frame sayısı: 6 frame | FPS: 6

PixelLab → [Animate]:
  Animation: "Shuffle Walk" (yavaş, sürünen yürüyüş)
  Frames: 6
  Intensity: Low (zombi yavaş yürür)

Export: 6 columns = 192×32px
```

#### 1B-4. GRUNT FRONT — ATTACK

```
Dosya adı: grunt_front_attack.png
Frame sayısı: 5 frame | FPS: 8

PixelLab → [Animate]:
  Animation: "Melee Lunge" (öne hamle, kolları uzatılmış)
  Frames: 5

Export: 5 columns = 160×32px
```

#### 1B-5. GRUNT FRONT — HIT

```
Dosya adı: grunt_front_hit.png
Frame sayısı: 3 frame | FPS: 12

(Warblade hit ile aynı workflow, sadece zombi sprite üzerinde)
Frame 1: Beyaz flash
Frame 2: Geri itilmiş
Frame 3: Normale dönüş

Export: 3 columns = 96×32px
```

#### 1B-6. GRUNT FRONT — DEATH

```
Dosya adı: grunt_front_death.png
Frame sayısı: 6 frame | FPS: 8

PixelLab → [Animate]:
  Animation: "Death Collapse"
  Frames: 6

Son 2 frame: Sadece corpse sprite (yerde yatan).
Export: 6 columns = 192×32px
```

```
Left yönleri için aynı adımları tekrarla:
  grunt_left_idle.png
  grunt_left_walk.png
  grunt_left_attack.png
  grunt_left_hit.png
  grunt_left_death.png
```

---

### 1C. ENVIRONMENT — İlk Oda (Zindanın İlk Görünümü)

**Canvas boyutu:** 16×16px per tile

**Bu pakette sadece 1 biyom: ZINDAN (koyu taş, ortaçağ)**

#### 1C-1. FLOOR TILES (Zemin)

```
Dosya adı: tileset_dungeon_floor.aseprite → tileset_dungeon_floor.png

Tek dosyada 4 varyasyon yan yana:
  Canvas boyutu: 64×16px (4 tile yan yana)

PixelLab → [Generate]:
  Prompt: "top-down 2d pixel art dungeon floor tile, dark stone, seamless, 16x16,
           worn texture, medieval, no border"
  Size: 16×16
  4 kez üret → yan yana diz (varyasyon için)

Varyasyonlar:
  tile 1: Düz taş (#2d2d4a)
  tile 2: Hafif çatlak
  tile 3: Kan lekesi (#4a1a1a)
  tile 4: Yosun/rutubet lekesi

Export: 64×16px single row (her tile Unity'de 16px olarak kesilir)
```

#### 1C-2. WALL TILES (Duvarlar — Top-Down 2D'de sadece üst kenar görünür)

```
Dosya adı: tileset_dungeon_wall.aseprite → tileset_dungeon_wall.png

Canvas: 16×32px per wall tile (duvar yukarı taşar — top-down hissi için)

PixelLab → [Generate]:
  Prompt: "top-down 2d pixel art dungeon wall tile, dark stone bricks,
           16x32, medieval, thick wall edge visible from above, dark shadow at bottom"
  Size: 16×32
  3 varyasyon: düz / köşe / kapı kenarı

Dosyalar:
  tileset_dungeon_wall_straight.png    (düz duvar)
  tileset_dungeon_wall_corner.png      (köşe)
  tileset_dungeon_wall_doorframe.png   (kapı kenarı)

Export: Her biri ayrı PNG, 16×32px
```

#### 1C-3. PROPS — Ortam Objeleri

```
Canvas: 16×16px veya 32×32px (büyük olanlar için)

Gerekli prop'lar (FAZ 1 için minimum):
```

| Dosya adı | Boyut | Açıklama | PixelLab Prompt |
|-----------|-------|----------|-----------------|
| `prop_torch_wall.png` | 16×32px | Duvara asılı meşale, 4 frame animasyon (alev) | "wall torch, medieval dungeon, pixel art, 16x32, flame, top-down 2d" |
| `prop_barrel.png` | 16×16px | Tahta fıçı, statik | "wooden barrel, dungeon prop, pixel art, 16x16, top-down view" |
| `prop_chest_closed.png` | 16×16px | Kapalı sandık | "treasure chest closed, pixel art, 16x16, dark wood, top-down" |
| `prop_chest_open.png` | 16×16px | Açık sandık | "treasure chest open, pixel art, 16x16, top-down" |
| `prop_pillar.png` | 16×32px | Sütun | "stone pillar, dungeon, pixel art, 16x32, top-down" |
| `prop_door_closed.png` | 32×32px | Kapalı kapı | "dungeon door closed, dark wood iron bolts, pixel art, 32x32" |
| `prop_door_open.png` | 32×32px | Açık kapı | "dungeon door open, pixel art, 32x32" |

```
Meşale animasyonu için:
  PixelLab → [Animate]:
    Skeleton: Custom / Particle
    Animation: "Fire Flicker"
    Frames: 4
    Dosya adı: prop_torch_wall_anim.png (4 frame, 16×32px, 4 columns = 64×32px)
```

---

### 1D. VFX — Temel Efektler (FAZ 1)

**Canvas:** 32×32px (VFX için standart)

#### 1D-1. HIT SPARKS (Vuruş Kıvılcımı)

```
Dosya adı: vfx_hit_sparks.png
Canvas: 32×32px
Frame sayısı: 5 frame | FPS: 16

PixelLab → [Generate] × [Animate]:
  Prompt: "hit spark effect, pixel art, 32x32, bright white-yellow particles, impact burst,
           transparent background, 5 frame animation"

Alternatif (Generate ile):
  Her frame'i ayrı ayrı üret:
    Frame 1: Küçük patlama merkezi (4-6 piksel)
    Frame 2: Dağılan kıvılcımlar (en geniş frame)
    Frame 3: Kıvılcımlar küçülüyor
    Frame 4: Son parıltılar
    Frame 5: Boş (opacity 0)

Export: 5 columns = 160×32px
```

#### 1D-2. SLASH VFX (Kılıç İzi)

```
Dosya adı: vfx_slash_white.png
Canvas: 64×64px (kılıç izi geniş olmalı)
Frame sayısı: 4 frame | FPS: 16

PixelLab → [Generate]:
  Prompt: "sword slash trail effect, pixel art, 64x64, white arc, motion trail,
           transparent background, 4 frame disappearing animation"

Export: 4 columns = 192×48px
Renk versiyonları (aynı şekli farklı renkle):
  vfx_slash_white.png  → Warblade için
  vfx_slash_red.png    → Berserker için (kopyala, renk değiştir)
  vfx_slash_blue.png   → Mage için
```

#### 1D-3. RAGE AURA (Rage Aktivasyon Efekti)

```
Dosya adı: vfx_rage_aura.png
Canvas: 64×64px (karakterden büyük — etrafını sarar)
Frame sayısı: 8 frame | FPS: 10

PixelLab → [Generate]:
  Prompt: "rage power aura effect, pixel art, 64x64, red energy, circular burst,
           intense glow, transparent background, 8 frame loop animation"

Export: 8 columns = 512×64px
```

#### 1D-4. BLOOD SPLATTER (Düşman Ölüm Efekti)

```
Dosya adı: vfx_blood_splatter.png
Canvas: 32×32px
Frame sayısı: 5 frame | FPS: 12

PixelLab → [Generate]:
  Prompt: "blood splatter effect, pixel art, 32x32, dark red drops, impact burst,
           transparent background, 5 frame dissipating animation"

Export: 5 columns = 160×32px
```

---

## FAZ 2 — SİSTEMLER: UI + SKİLL İKONLARI (Ay 2)

> FAZ 2'de oyuna yeni sistemler ekleniyor: skill acquisition ekranı, dual class seçimi, Rage bar UI.
> Bu aşamada bu sanat varlıkları gerekiyor.

---

### 2A. SKILL İKONLARI — Warblade (FAZ 2 için minimum, diğer sınıflar FAZ 4'te)

**Canvas:** 32×32px per icon

Warblade'in 7 skill/pasif/ultimate'i var. Her biri için 1 ikon:

| Dosya adı | Skill adı | Prompt |
|-----------|-----------|--------|
| `icon_warblade_ground_stomp.png` | Ground Stomp | "ground stomp skill icon, pixel art, 32x32, shockwave, cracked earth, dark fantasy" |
| `icon_warblade_cleave.png` | Cleave | "cleave sword swing icon, pixel art, 32x32, wide arc slash, greatsword, dark fantasy" |
| `icon_warblade_focused_strike.png` | Focused Strike | "focused strike icon, pixel art, 32x32, glowing sword tip, critical hit, golden highlight" |
| `icon_warblade_battle_cry.png` | Battle Cry | "battle cry skill icon, pixel art, 32x32, knight shouting, red aura, shield raised" |
| `icon_warblade_passive_bloodlust.png` | Bloodlust (Pasif) | "bloodlust passive icon, pixel art, 32x32, red drops, feral eyes, dark" |
| `icon_warblade_passive_ironwill.png` | Iron Will (Pasif) | "iron will passive icon, pixel art, 32x32, cracked shield, glowing core, endurance" |
| `icon_warblade_ultimate.png` | Berserker's Rage (Ultimate) | "berserk ultimate icon, pixel art, 32x32, skull with flame, red glow, dark fantasy, ULTIMATE border" |

```
PixelLab → [Generate] ile üret:
  Her ikonu için ayrı prompt gir (yukarıdaki tablo)
  Boyut: 32×32
  Style: Pixel Art

Ultimate ikonlarına özel:
  Kenara altın/turuncu çerçeve ekle (Aseprite'ta elle çiz)
  Pasif ikonlarına mor köşe işareti ekle
```

---

### 2B. UI ELEMANLARI

#### 2B-1. SKILL SLOT FRAME (Skill Çerçevesi)

```
Dosya adı: ui_skill_slot_empty.png / ui_skill_slot_active.png / ui_skill_slot_cooldown.png
Canvas: 40×40px (32×32 ikon + 4px çerçeve etrafında)

Aseprite'ta elle çiz (basit ama önemli):
  ui_skill_slot_empty.png:
    - Koyu arka plan (#0d0d1a)
    - Altın çerçeve (#c8a84b), 1px kalınlık
    - Köşeler hafif yuvarlak (2px köşe boşluğu)

  ui_skill_slot_active.png:
    - Aynı ama çerçeve parlak beyaz (#ffffff) + glow efekti

  ui_skill_slot_cooldown.png:
    - Slot karanlık (#1a1a2e overlay %60 opacity)
    - Üstünde saat yönünde dolup boşalan daire animasyonu
    - 12 frame (tam tur)
    → dosya adı: ui_skill_slot_cooldown_anim.png (12 frame)

Export: Her biri 40×40px tek frame
       Cooldown: 12 columns = 480×40px
```

#### 2B-2. HP BAR + RAGE BAR

```
Dosya adı: ui_hpbar_frame.png
Canvas: 200×16px

Aseprite'ta elle çiz:
  - Sol: Kafatası veya kalp ikonu (12×12px)
  - Orta: Bar frame, koyu arka plan (#0d0d1a), altın kenar (#c8a84b)
  - Doldurma rengi: HP için #e05252 (kırmızı), Rage için #ff6600 (turuncu)
  - Bar dolumu Unity'de kod ile yapılır (Image.fillAmount)
  - Bu sadece frame'i

Dosyalar:
  ui_hpbar_frame.png      (boş bar çerçevesi, 200×16px)
  ui_hpbar_fill.png       (düz kırmızı dikdörtgen — kod uzatır, 180×8px)
  ui_ragebar_frame.png    (aynı ama rage ikonu ile)
  ui_ragebar_fill.png     (turuncu — aynı şekilde)
```

#### 2B-3. SKILL ACQUISITION KARTI (Oda Sonrası Seçim)

```
Dosya adı: ui_skill_card_frame.png
Canvas: 80×112px (standart kart boyutu — küçük ama okunabilir)

Aseprite'ta elle çiz:
  - Koyu arka plan (#0d0d1a)
  - Dış çerçeve: altın (#c8a84b), 2px
  - Üst %40: İkon alanı (32×32 boş kutu ortada)
  - Alt %60: İsim alanı + mekanik özet
  - (Yazı Unity'de TextMeshPro ile eklenir — bu sadece arka plan frame)

Varyasyonlar:
  ui_skill_card_frame_normal.png     (altın çerçeve)
  ui_skill_card_frame_rare.png       (mor çerçeve — #a852e0)
  ui_skill_card_frame_ultimate.png   (turuncu çerçeve + parlak kenar)

3 varyasyon → 3 ayrı dosya, hepsi 80×112px
```

#### 2B-4. SINIF SEÇİM EKRANI KART ÇERÇEVESI

```
Dosya adı: ui_class_card_frame.png
Canvas: 96×128px

Aseprite'ta:
  - Büyük dikey kart
  - Üst kısım: Sınıf portresi alanı (64×64px)
  - Alt kısım: Sınıf ismi + kısa açıklama alanı
  - Her sınıf için renk kodu (çerçeve rengi):
    Warblade:     #c8a84b  (altın)
    Elementalist: #52a8e0  (mavi)
    Rogue:        #2a2a2a  (siyah-gri)
    Ranger:       #52e088  (yeşil)
    Brawler:      #e08852  (turuncu-bakır)
    Paladin:      #e0e052  (sarı-altın)
    Summoner:     #aa44ff  (mor)
    Hexer:        #44ff88  (zehir yeşili)
```

---

### 2C. SINIF PORTRESİ (Seçim Ekranı İçin)

Her sınıf için 1 adet bust portrait (omuz üstü, duygusal ifade):

```
Canvas: 64×64px
Stil: Biraz daha detaylı (karakter spritelarından farklı — daha büyük yüz)

PixelLab → [Generate]:
  Her sınıf için prompt:
```

| Dosya adı | Sınıf | Prompt |
|-----------|-------|--------|
| `portrait_warblade.png` | Warblade | "warrior knight portrait, pixel art, 64x64, heavy armor, determined face, dark fantasy medieval" |
| `portrait_elementalist.png` | Elementalist | "mage elementalist portrait, pixel art, 64x64, glowing eyes, arcane robes, fire and ice motifs" |
| `portrait_rogue.png` | Rogue | "rogue assassin portrait, pixel art, 64x64, hood, masked face, daggers, dark shadowy" |
| `portrait_ranger.png` | Ranger | "ranger archer portrait, pixel art, 64x64, leather hood, quiver, forest-battle worn" |
| `portrait_brawler.png` | Brawler | "brawler monk portrait, pixel art, 64x64, no weapons, bandaged fists, intense gaze, battle-worn" |
| `portrait_paladin.png` | Paladin | "paladin holy knight portrait, pixel art, 64x64, gleaming armor, holy symbol, solemn face" |
| `portrait_summoner.png` | Summoner | "summoner necromancer portrait, pixel art, 64x64, staff, floating minion spirits, dark robes" |
| `portrait_hexer.png` | Hexer | "hexer witch portrait, pixel art, 64x64, plague doctor mask, vials, dark cloak, sinister" |

---

## FAZ 3 — DÜŞMAN ÇEŞİTLERİ + BOSS (Ay 3)

### 3A. ELİTE DÜŞMAN (Grudge Sistemi — Hafızalı Düşman)

**Canvas:** 40×40px (Grunt'tan biraz büyük)
**Fark:** Elite düşmanda "Grudge Badge" (hafıza rozeti) görünür — kafası üstünde küçük ikon.

#### 3A-1. ELİTE SPRITE (PLAGUE KNIGHT)

```
Dosya adı: elite_plagueknight_front_BASE.aseprite

PixelLab → [Generate]:
  Prompt: "top-down 2d pixel art elite enemy plague knight, dark armor, plague doctor mask,
           sword and shield, 40x40, menacing, dark fantasy medieval, transparent background"
  Size: 40×40

Animasyonlar (aynı workflow):
  elite_plagueknight_front_idle.png      (4 frame)
  elite_plagueknight_front_walk.png      (6 frame)
  elite_plagueknight_front_attack.png    (6 frame — daha ağır, 2 frame daha fazla)
  elite_plagueknight_front_hit.png       (3 frame)
  elite_plagueknight_front_death.png     (8 frame — dramatik)
  (Left versiyonları aynı şekilde)
```

#### 3A-2. GRUDGE BADGE İKONLARI

```
Dosya adı: ui_grudge_badge_[element].png
Canvas: 12×12px (kafanın üstünde küçük ikon)

Grudge sistemi: Düşman nasıl öldürüldüğünü hatırlıyor → o özelliğe direnç kazanıyor.
Her element için küçük rozet:

  ui_grudge_badge_fire.png       → Ateş alevi simgesi, #ff4444
  ui_grudge_badge_ice.png        → Buz kristali, #44aaff
  ui_grudge_badge_slash.png      → Kılıç çizgisi, #c8a84b
  ui_grudge_badge_blunt.png      → Yumruk/çekiç, #8a6a5a
  ui_grudge_badge_dark.png       → Gözler/mor duman, #aa44ff
  ui_grudge_badge_holy.png       → Haç/ışık, #e0e052
  ui_grudge_badge_poison.png     → Damla/zehir, #44ff88
  ui_grudge_badge_generic.png    → Genel direnç (kafatası), #ffffff

Aseprite'ta elle çiz — bu kadar küçük sprite'ı PixelLab'la üretme, elle daha hızlı.
```

---

### 3B. BOSS — FAZ 3 BOSS'U

**Canvas:** 80×80px | **Önemli: Boss büyük, animasyonlar ağır ve yavaş**

```
Dosya adı: boss_01_front_BASE.aseprite

PixelLab → [Generate]:
  Prompt: "top-down 2d pixel art boss enemy, giant plague lord, enormous dark armor,
           glowing plague mask, multiple arms or appendages, 80x80, intimidating,
           dark fantasy, transparent background"
  Size: 80×80

Boss animasyonları (daha fazla frame — ağır his için):
```

| Dosya adı | Frame sayısı | FPS | PixelLab Animation |
|-----------|-------------|-----|-------------------|
| `boss_01_front_idle.png` | 6 | 5 | "Menacing Idle Breathe" |
| `boss_01_front_walk.png` | 8 | 6 | "Heavy Stomp Walk" |
| `boss_01_front_attack_slam.png` | 10 | 8 | "Ground Slam Heavy" |
| `boss_01_front_attack_sweep.png` | 8 | 10 | "Wide Arc Sweep" |
| `boss_01_front_special.png` | 12 | 8 | "Summon / Channel Special" |
| `boss_01_front_hit.png` | 4 | 12 | "Hit Stagger" |
| `boss_01_front_death.png` | 16 | 6 | "Death Explosion Collapse" |
| `boss_01_front_enrage.png` | 10 | 8 | "Power Up Enrage" |

```
NOT: Boss'u her yönde çizmene gerek yok (çok iş). Sadece Front.
     Boss genellikle arenada sabit veya küçük alan hareket eder.
     Sol/sağ hareket için: Front sprite'ı hafif döndür (Unity'de kod ile).
```

---

## FAZ 4 — TÜM 8 SINIF + SKİLL İKONLARI (Ay 3-4)

FAZ 1'de sadece Warblade yaptık. FAZ 4'te kalan 7 sınıf ekleniyor.
**Her sınıf için tam liste:**

### Sınıf Başına Üretilecekler

Her sınıf için aynı animasyon setini üret:

```
[sınıf]_front_BASE.aseprite        (referans sprite)
[sınıf]_front_idle.png             4 frame
[sınıf]_front_walk.png             6 frame
[sınıf]_front_attack1.png          5-6 frame (temel saldırı)
[sınıf]_front_attack2.png          6 frame (ikinci saldırı / skill)
[sınıf]_front_skill_ultimate.png   8-10 frame (ultimate animasyonu)
[sınıf]_front_dash.png             4 frame
[sınıf]_front_hit.png              3 frame
[sınıf]_front_death.png            8 frame
[sınıf]_back_[aynılar].png
[sınıf]_left_[aynılar].png
```

### Sınıf Başına PixelLab Prompt'ları

```
elementalist_front_BASE:
  "top-down 2d pixel art mage elementalist, flowing robes, orb or staff, glowing runes,
   front view, 64x64, dark fantasy, fire/ice/lightning aura, transparent background"

rogue_front_BASE:
  "top-down 2d pixel art rogue assassin, dark leather armor, dual daggers, hood,
   front view, 64x64, dark fantasy, shadow cloak, transparent background"

ranger_front_BASE:
  "top-down 2d pixel art ranger archer, leather armor, bow ready, quiver,
   front view, 64x64, dark fantasy, hood, forest scout, transparent background"

brawler_front_BASE:
  "top-down 2d pixel art brawler monk, no weapons, bandaged fists, martial arts stance,
   front view, 64x64, dark fantasy, chi energy glow, transparent background"

paladin_front_BASE:
  "top-down 2d pixel art paladin holy knight, gleaming armor, holy symbol on chest,
   mace or sword+shield, front view, 64x64, dark fantasy, divine light aura, transparent background"

summoner_front_BASE:
  "top-down 2d pixel art summoner necromancer, dark robes, staff with skull,
   small ghost/spirit hovering, front view, 64x64, dark fantasy, purple aura, transparent background"

hexer_front_BASE:
  "top-down 2d pixel art hexer witch plague doctor, plague mask, long dark coat,
   vials and syringes, front view, 64x64, dark fantasy, toxic green aura, transparent background"
```

### FAZ 4 Skill İkonları — Tüm Sınıflar

Her sınıfın 7 skill/pasif/ultimate'i için ikon:

```
Toplam: 7 sınıf × 7 ikon = 49 ikon
Boyut: 32×32px her biri
Format: icon_[sınıf]_[skill_kısa_adı].png
```

Her ikon için PixelLab → [Generate] ile üret.
Genel kural:
- Aktif skill: parlak, eylem içeren görsel
- Pasif: daha sessiz, sembol tabanlı
- Ultimate: en büyük, en dramatik, altın veya parlak çerçeveli

---

## FAZ 5 — META PROGRESSION + POLİSHİNG (Ay 4-5)

### 5A. HUB EKRANI (Ana Üs / Hub Sahnesi)

```
Hub sahnesi: Oyuncunun run'lar arası döndüğü yer.
Tematik: Bir veba zamanı kasabası veya sığınak.

Gerekli sanat:
  background_hub.png      → 480×270px (1920×1080 / 4 — pixel art scale)
  İçerik: Arka planda harap bina, ön planda NPC duruyor
  Aseprite'ta büyük canvas üzerinde çiz (çok zaman alır — en son yap)

Minimum:
  prop_hub_campfire.png   → 32×32px, 6 frame animasyon (ateş)
  prop_hub_upgradeanvil.png → 32×32px (upgrade noktası)
  prop_hub_door_dungeon.png → 48×64px (zindan girişi)
```

### 5B. VFX — SKILL EFEKTLERİ (Her Sınıf İçin)

Her sınıfın imza skill'leri için VFX sprite'ları:

```
Canvas: 32×32 veya 64×64px
Frame sayısı: 4-8 frame
FPS: 16
```

| Dosya adı | Sınıf | Efekt | PixelLab Prompt |
|-----------|-------|-------|-----------------|
| `vfx_warblade_groundstomp.png` | Warblade | Shockwave dalgası | "ground stomp shockwave vfx, pixel art, 64x64, circular crack wave, 6 frame" |
| `vfx_elementalist_fireball.png` | Elementalist | Ateş topu | "fireball projectile vfx, pixel art, 32x32, orange-red flame sphere, 4 frame" |
| `vfx_elementalist_icenova.png` | Elementalist | Buz patlaması | "ice nova burst vfx, pixel art, 64x64, blue crystal explosion, 6 frame" |
| `vfx_elementalist_blink.png` | Elementalist | Teleport | "teleport blink vfx, pixel art, 32x32, blue flash disappear appear, 4 frame" |
| `vfx_rogue_shadowstrike.png` | Rogue | Gölge teleport | "shadow teleport vfx, pixel art, 32x32, dark smoke burst, 5 frame" |
| `vfx_rogue_eviscerate.png` | Rogue | Finisher | "eviscerate finisher vfx, pixel art, 64x64, blood red slash multiply, 6 frame" |
| `vfx_ranger_arrow_trail.png` | Ranger | Ok izi | "arrow trail vfx, pixel art, 32x8, green-white streak, 4 frame fade" |
| `vfx_brawler_chi_burst.png` | Brawler | Chi patlaması | "chi energy burst vfx, pixel art, 64x64, golden energy ring expand, 6 frame" |
| `vfx_paladin_holy_light.png` | Paladin | Kutsal ışık | "holy light pillar vfx, pixel art, 32x64, golden beam descend, 6 frame" |
| `vfx_summoner_summon.png` | Summoner | Minyon çağırma | "summon portal vfx, pixel art, 32x32, purple circle open, 8 frame" |
| `vfx_hexer_hexblast.png` | Hexer | Hexblast patlama | "hex explosion vfx, pixel art, 64x64, toxic green burst, skull motif, 8 frame" |

### 5C. LEVEL UP + REWARD UI

```
vfx_levelup.png          → 64×64px, 10 frame, altın ışık patlaması
ui_reward_background.png → 320×180px, koyu overlay (run bitiş ekranı arkaplanı)
ui_banner_victory.png    → 240×32px, "RUN COMPLETE" banner
ui_banner_defeat.png     → 240×32px, "YOU DIED" banner (kırmızı)
```

---

## FAZ 6 — STEAM HAZIRLIKLARI (Ay 5-6)

### 6A. STEAM STORE GÖRSELLER

```
NOT: Bunlar Aseprite'ta yapılmaz — büyük çözünürlüklü görüntüler.
     Aseprite'ta pixel art sahne çiz → upscale et → metin ekle (Photoshop/GIMP).

Gerekli Steam görselleri:
  steam_capsule_small.png     → 231×87px
  steam_capsule_large.png     → 460×215px
  steam_header.png            → 460×215px
  steam_screenshot_01.png     → 1280×720px (gameplay screenshot)
  steam_screenshot_02-5.png   → 1280×720px (4 daha)

Çekim sahneleri (önce oyunu bitir):
  - Warblade vs Boss sahnesi (büyük düşman, skill efektleri aktif)
  - Dual class seçim ekranı
  - Zindan oda içi görünüm
  - Rage aktivasyon anı
```

### 6B. İKON VE LOGO

```
icon_game_logo.png    → 512×512px
  Konsept: Veba doktoru maskesi + kılıç veya rune
  Aseprite'ta 128×128px çiz → 4× upscale → Unity/Steam'e yükle

icon_steam_app.png    → 512×512px (Steam uygulama ikonu — aynı logo)
```

---

## HIZLI REFERANS — TÜM DOSYALAR ÖZET

### ART/ klasör yapısı ve dosyalar:

```
ART/
├── karakterler/
│   ├── warblade/         ← FAZ 1
│   │   ├── warblade_front_BASE.aseprite
│   │   ├── warblade_front_idle.png       (4f)
│   │   ├── warblade_front_walk.png       (6f)
│   │   ├── warblade_front_attack1.png    (5f)
│   │   ├── warblade_front_attack2.png    (6f)
│   │   ├── warblade_front_rage.png       (6f)
│   │   ├── warblade_front_dash.png       (4f)
│   │   ├── warblade_front_hit.png        (3f)
│   │   ├── warblade_front_death.png      (8f)
│   │   ├── warblade_back_[aynılar].png
│   │   └── warblade_left_[aynılar].png
│   ├── elementalist/     ← FAZ 4
│   ├── rogue/            ← FAZ 4
│   ├── ranger/           ← FAZ 4
│   ├── brawler/          ← FAZ 4
│   ├── paladin/          ← FAZ 4
│   ├── summoner/         ← FAZ 4
│   └── hexer/            ← FAZ 4
│
├── dusmanlar/
│   ├── grunt/            ← FAZ 1
│   │   ├── grunt_front_BASE.aseprite
│   │   ├── grunt_front_idle.png         (4f)
│   │   ├── grunt_front_walk.png         (6f)
│   │   ├── grunt_front_attack.png       (5f)
│   │   ├── grunt_front_hit.png          (3f)
│   │   ├── grunt_front_death.png        (6f)
│   │   └── grunt_left_[aynılar].png
│   ├── elite/            ← FAZ 3
│   │   ├── elite_plagueknight_front_BASE.aseprite
│   │   └── [animasyonlar]
│   └── boss/             ← FAZ 3
│       ├── boss_01_front_BASE.aseprite
│       └── [animasyonlar]
│
├── environment/
│   ├── tileset/
│   │   ├── tileset_dungeon_floor.png     ← FAZ 1
│   │   ├── tileset_dungeon_wall_straight.png
│   │   ├── tileset_dungeon_wall_corner.png
│   │   └── tileset_dungeon_wall_doorframe.png
│   └── props/
│       ├── prop_torch_wall_anim.png      ← FAZ 1
│       ├── prop_barrel.png
│       ├── prop_chest_closed.png
│       ├── prop_chest_open.png
│       ├── prop_pillar.png
│       ├── prop_door_closed.png
│       ├── prop_door_open.png
│       ├── prop_hub_campfire.png         ← FAZ 5
│       ├── prop_hub_upgradeanvil.png
│       └── prop_hub_door_dungeon.png
│
├── ui/
│   ├── ui_skill_slot_empty.png           ← FAZ 2
│   ├── ui_skill_slot_active.png
│   ├── ui_skill_slot_cooldown_anim.png   (12f)
│   ├── ui_hpbar_frame.png
│   ├── ui_hpbar_fill.png
│   ├── ui_ragebar_frame.png
│   ├── ui_ragebar_fill.png
│   ├── ui_skill_card_frame_normal.png
│   ├── ui_skill_card_frame_rare.png
│   ├── ui_skill_card_frame_ultimate.png
│   ├── ui_class_card_frame.png
│   ├── ui_reward_background.png          ← FAZ 5
│   ├── ui_banner_victory.png
│   ├── ui_banner_defeat.png
│   └── ui_grudge_badge_[element].png × 8 ← FAZ 3
│
├── skill_iconlari/
│   ├── icon_warblade_[skill].png × 7    ← FAZ 2
│   └── icon_[sınıf]_[skill].png × 49   ← FAZ 4
│
└── vfx/
    ├── vfx_hit_sparks.png               ← FAZ 1
    ├── vfx_slash_white.png
    ├── vfx_slash_red.png
    ├── vfx_slash_blue.png
    ├── vfx_rage_aura.png
    ├── vfx_blood_splatter.png
    ├── vfx_warblade_groundstomp.png     ← FAZ 5
    ├── vfx_elementalist_fireball.png
    ├── vfx_elementalist_icenova.png
    ├── vfx_elementalist_blink.png
    ├── vfx_rogue_shadowstrike.png
    ├── vfx_rogue_eviscerate.png
    ├── vfx_ranger_arrow_trail.png
    ├── vfx_brawler_chi_burst.png
    ├── vfx_paladin_holy_light.png
    ├── vfx_summoner_summon.png
    ├── vfx_hexer_hexblast.png
    └── vfx_levelup.png
```

---

## UNITY'E AKTARMA — KISA HATIRLATICI

```
Her PNG için Unity Inspector ayarları:
  Texture Type: Sprite (2D and UI)
  Sprite Mode: Multiple (spritesheet için) / Single (tek sprite için)
  Pixels Per Unit: 16 (tile için) veya 48 (karakter için — sprite boyutuna eşit)
  Filter Mode: Point (No Filter) ← BUNU UNUTMA, piksel bulanık görünür
  Compression: None ← Küçük dosyalar, sıkıştırma gerek yok

Spritesheet kesmek için:
  Sprite Editor → Slice → Grid By Cell Size
  Hücre boyutu: sprite'ın boyutuna göre (örn. 64×64)

Normal map için:
  Aynı sprite'ı seç → Secondary Textures → + ekle → Name: _NormalMap → Texture: NormalMap PNG
```

---

## KISIM SON — ÖNCE BUNLARI YAP (FAZ 1 MİNİMUM LİSTESİ)

Oyunu başlatmak için sadece bunlar yeterli:

```
[ ] warblade_front_idle.png
[ ] warblade_front_walk.png
[ ] warblade_front_attack1.png
[ ] warblade_front_dash.png
[ ] warblade_front_hit.png
[ ] warblade_back_idle.png + walk + hit
[ ] warblade_left_idle.png + walk + attack1 + hit
[ ] grunt_front_idle.png + walk + attack + hit + death
[ ] grunt_left_idle.png + walk
[ ] tileset_dungeon_floor.png
[ ] tileset_dungeon_wall_straight.png
[ ] prop_torch_wall_anim.png
[ ] prop_door_closed.png + open
[ ] vfx_hit_sparks.png
[ ] vfx_slash_white.png
[ ] vfx_blood_splatter.png
[ ] ui_hpbar_frame.png + fill
[ ] ui_ragebar_frame.png + fill
[ ] ui_skill_slot_empty.png
[ ] icon_warblade_ground_stomp.png (test için 1 ikon yeterli)
```

Bu liste tamamlanınca Unity'de ilk playtest yapılabilir.
