# RIMA — Aseprite + PixelLab Adım Adım Üretim Rehberi
*Her asset için: o prompta ulaşana kadar geçen her tıklama dahil*
*PixelLab resmi dokümantasyonuna dayalı — Son güncelleme: 2026-03-30*

---

## BAŞLAMADAN ÖNCE — BİR KEZ OKU

### PixelLab'ı Aseprite'a kurma (ilk kurulum, bir kez yapılır)
1. pixellab.ai sitesine git, hesap aç (ücretsiz deneme var)
2. Hesabın **Downloads** veya **Extensions** sayfasından `.aseprite-extension` dosyasını indir
3. Aseprite'ı aç
4. **Edit → Preferences → Extensions** tıkla
5. **Add Extension** butonuna bas
6. İndirdiğin `.aseprite-extension` dosyasını seç
7. Aseprite'ı tamamen kapat, tekrar aç
8. Kontrol: **Edit → PixelLab → Open plugin** — eğer bu menü çıkıyorsa kurulum tamam

---

### PixelLab penceresinin yapısı
`Edit → PixelLab → Open plugin` ile açılan pencerede **araç listesi** var.
Her üretim tipi için farklı araç seçilir — tek bir "Generate" sekmesi yok, ayrı araçlar:

| Araç | Ne zaman kullanılır |
|------|---------------------|
| **Create S-M image** | 16×16 – 64×64 sprite, karakter, düşman, ikon |
| **Create large image** | 64×64 – 128×128 (standart) |
| **Create medium-extra large image (Pixflux)** | 128×128 boss sprite'ları — daha iyi metin anlama |
| **Animation to Animation** | Tüm animasyonlar (idle, walk, attack...) |
| **Rotate** | Karakterin farklı yönleri (N, NE, E...) |
| **Remove background** | Arka plan kaldırma (Gemini çıktısı için) |

---

### Her üretimde değişmeyen SABİT AYARLAR
Her araç açıldığında bu alanları aynı değere getir (değişmedikçe bir daha dokunma):

| Alan adı | Değer |
|----------|-------|
| Camera view | `low top-down` |
| Direction | `S` |
| Outline | `selective outline` |
| Shading | `basic shading` |
| Detail | `medium detail` |
| Remove background | ✓ (işaretli) |
| Output method | `New layer` |

> ⚠️ **Logo üretiminde istisna:** Remove background = işaretsiz (siyah arkaplan istiyoruz)

---

## FAZ 0 — LOGO + PLACEHOLDER'LAR

---

### LOGO — `ART/logo/rima_logo_320x80.png`

320×80px boyutu PixelLab'ın limitini aşıyor (max ~400×400 ama logo için oransız).
**Gemini birincil yöntem. Sonuç iyi çıkmazsa Aseprite'ta elle çizim.**

---

#### YÖNTEM A — GEMİNİ (önce bunu dene)

**Adım 1 — Gemini'yi aç**
1. Tarayıcıda yeni sekme → **gemini.google.com** → oturum aç
2. Sol üstteki **"New chat"** (veya **"Yeni sohbet"**) butonuna tıkla
3. Alt kısımdaki metin kutusuna tıkla

**Adım 2 — Promptu gönder**
Aşağıdaki metni seç, kopyala (Ctrl+C), metin kutusuna yapıştır (Ctrl+V), Enter'a bas:

```
A dark fantasy pixel art game logo. The large uppercase letters "RIMA" are written
boldly in dark steel color, spanning the upper portion of the image.
Below and to the right of the letter "I", the small lowercase letters "ft" hang
downward at an angle — as if they cracked off from the word RIFT and are drooping,
falling debris from a broken seal. They are smaller, slightly rotated, cracked in texture.
Below and to the right of the letter "A", the small lowercase letters "rch" hang
downward at the same angle — as if they cracked off from the word MARCH, same drooping effect.
At the exact break points where "ft" detached from "RI" and "rch" detached from "MA",
brilliant gold light bleeds through the crack, color #FFD700.
Letter color: dark steel #1E1E32. Background: void black #080808.
The hidden full words RIFT and MARCH become readable on closer inspection.
Pixel art logo style, high contrast, kintsugi dark aesthetic.
```

**Adım 3 — Sonucu kontrol et**
- ✅ "ft" "I" harfinin altından sarkıyorsa → iyi
- ✅ "rch" "A" harfinin altından sarkıyorsa → iyi
- ✅ Kırılma noktalarında altın glow varsa → iyi
- ❌ Harfler düzgün sıraya sahip değilse → aynı chat'e şunu yaz:
  `"Regenerate with ft hanging lower below the I letter, and rch hanging lower below the A letter, with stronger gold glow at the break points"`

**Adım 4 — Kaydet ve boyutlandır**
1. Gemini'deki görsele sağ tıkla → **"Resmi farklı kaydet"**
2. Kaydet: `F:\Antigravity Projeler\2d roguelite\ART\logo\rima_logo_kaynak.png`
3. Terminali aç (Windows tuşu → "cmd" yaz → Enter):
```
cd "F:\Antigravity Projeler\2d roguelite"
python tools/logo_resize.py ART/logo/rima_logo_kaynak.png
```
→ `ART/logo/` klasörüne 3 boyut otomatik çıkar.

---

#### YÖNTEM B — ASEPRITE'TA ELLE ÇİZ (Gemini iyi çıkmazsa)

**B1 — Aseprite'ta dosya oluştur**
1. Aseprite'ı aç
2. Üst menüden **File → New** tıkla
3. Açılan pencerede:
   - **Width:** `320`
   - **Height:** `80`
   - **Color Mode:** `RGBA Color`
   - **Background:** `Black` (şeffaf DEĞİL — siyah arkaplan)
4. **OK** bas

**B2 — Zoom yap**
- Klavyeden `+` tuşuna birkaç kez bas → canvas büyür, rahat çizebilirsin
- Veya View menüsünde Zoom ayarla

**B3 — Gerekli renkler**
Sol alttaki büyük renkli kareye **çift tıkla** → renk seçici açılır → **Hex** alanına yaz:
- Ana harf rengi: `1E1E32`
- Sarkık harf rengi: `2A2A45`
- Altın glow: `FFD700`
- Glow çevresi (soluk): `AA8800`

**B4 — Kalem boyutunu ayarla**
Sol panelde kalem aracı seçili (B tuşu ile seç) → üstte **"Size"** alanına `3` yaz → 3px kalın çizgi

**B5 — "RIMA" harflerini çiz**
Tüm harfler y=8 başlar, y=52'de biter (44px yüksek). Renk: `1E1E32`

```
R harfi — x=20 başlar
  Sol dikey:  x=20-22, y=8-52  (yukarıdan aşağıya çiz)
  Üst yatay:  y=8-10, x=20-44
  Orta yatay: y=27-29, x=20-44
  Sağ üst:    x=42-44, y=8-29
  Bacak:      x=30'dan x=44'e köşegen, y=29-52

I harfi — x=58 başlar
  Üst yatay: y=8-10, x=56-70
  Orta dikey: x=62-64, y=8-52
  Alt yatay: y=50-52, x=56-70

M harfi — x=84 başlar
  Sol dikey:  x=84-86, y=8-52
  Sağ dikey:  x=114-116, y=8-52
  Sol köşegen: x=87-99, y=9-30 (aşağı-sağa)
  Sağ köşegen: x=101-113, y=30-9 (yukarı-sağa)

A harfi — x=130 başlar
  Sol köşegen: x=130-155, y=52-8 (yukarı-sağa)
  Sağ köşegen: x=132-157, y=8-52
  Orta yatay: y=28-30, x=138-150
```

> 💡 Daha hızlısı: **Edit → Insert Text** tıkla → font seç → "RIMA" yaz (büyük font, ~40px) → sonra elle düzeltme yap.

**B6 — "ft" sarkık harfleri**
Konum: x=62-88, y=50-68. Renk: `2A2A45`. Kalem size: `2`

```
f: x=64-66 dikey y=52-60 | x=64-70 yatay y=52-54 | x=64-68 yatay y=55-57
t: x=76-78 dikey y=50-60 | x=74-82 yatay y=53-55
```

Kırılma noktası (I'nın sağ alt, x=70, y=52):
→ Rengi `FFD700` yap → kalem size `2` → o koordinata 4-5 piksel koy
→ Rengi `AA8800` yap → 1px daha dışa 3-4 piksel daha (soluk glow halkası)

**B7 — "rch" sarkık harfleri**
Konum: x=153-195, y=50-68. Renk: `2A2A45`. Kalem size: `2`

```
r: x=154-156 dikey y=53-62 | x=157-162 yatay y=53-55
c: x=164 dikey y=53-62 | x=165-170 yatay y=53-55 | x=165-170 yatay y=60-62
h: x=175-177 dikey y=50-62 | x=178-183 yatay y=55-57 | x=181-183 dikey y=55-62
```

Kırılma noktası (A'nın sağ alt, x=155, y=52):
→ Aynı altın glow prosedürü

**B8 — Kaydet**
1. **File → Save As** → `rima_logo_kaynak.aseprite` → kaydet
2. **File → Export → Export As** → `rima_logo_kaynak.png` → kaydet
3. Terminalde: `python tools/logo_resize.py ART/logo/rima_logo_kaynak.png`

---

### PLACEHOLDER'LAR — `ART/placeholder/`

**warblade_placeholder.png (64×64)**

1. Aseprite → **File → New** → Width: `64`, Height: `64`, Color Mode: `RGBA`, Background: `Transparent` → OK
2. Sol panelde **Paint Bucket** aracına tıkla (kova ikonu, kısayol: `G`)
3. Sol alttaki renk karesine çift tıkla → Hex: `1E1E32` → OK
4. Canvas'ın ortasına tıkla → tüm alan doldu
5. **Edit → Insert Text** tıkla → font seçin, size: `30` → `W` yaz → OK
6. Text rengi için önce renk değiştir: Hex `FFD700` → text aracıyla ortaya yaz
7. **File → Export → Export As** → `warblade_placeholder.png` → kaydet

**grunt_placeholder.png (32×32)**
Aynı adımlar: 32×32, arka plan rengi `3D0000` (koyu kırmızı), harf `G` rengi `FFFFFF`

**floor_placeholder.png (16×16)**
16×16, arka plan `1E1E32`, kalemle (B) `2A2A4A` rengiyle çapraz çizgi

**wall_placeholder.png (16×16)**
16×16, arka plan `080808`, `1E1E32` ile ortaya yatay çizgi

---

## FAZ 1 — CORE LOOP

---

### WARBLADE BASE — `ART/karakterler/warblade/warblade_S_BASE.png`
**64×64px | Araç: Create S-M image**
*Bu asset FULL DETAYLA anlatılıyor — sonrakiler bu adımları referans alır*

---

**1 — Aseprite'ta dosya oluştur**
1. Aseprite'ı aç
2. **File → New** tıkla
3. **Width:** `64`
4. **Height:** `64`
5. **Color Mode:** `RGBA Color`
6. **Background:** `Transparent`
7. **OK** bas
8. Ortada gri-beyaz damalı boş bir canvas açılır (dama = şeffaflık)

---

**2 — Gemini'de top-down referans üret**
1. Tarayıcıda **gemini.google.com** → Yeni sohbet
2. Aşağıdaki metni kopyalayıp yapıştır ve gönder:

```
A heavily armored warrior viewed strictly from directly above, bird's eye aerial
top-down perspective — as if a camera is mounted on the ceiling looking straight down.
The warrior holds a large greatsword in their right hand, blade pointing downward
at their side. Their dark iron plate armor has a crack across the chest with cold
blue light bleeding through. Short torn cape visible behind the shoulders.
Heavy angular pauldrons visible from above. Head small at top, wide shoulders,
no face visible — only the top of the helmet. Feet barely visible or hidden.
The art style is retro pixel art, limited color palette: dark iron grey, cold blue,
worn gold. Transparent background, no background elements.
```

3. **Sonuç kontrolü — ZORUNLU:**
   - ✅ Omuzlar geniş, tepeden görünüş belli, yüz görünmüyor → kullanabilirsin
   - ❌ Yüz görünüyor / 3/4 açı / yan profil → aynı chat'te şunu yaz:
   `"Same character but strictly from directly above (bird's eye view). Camera on ceiling looking straight down. Head at top, feet barely visible, no face visible."`
4. Onaylı görseli **sağ tıkla → Resmi farklı kaydet** → `warblade_ref.png` olarak kaydet (herhangi bir yere)

---

**3 — PixelLab'ı aç**
1. Aseprite'ta üst menüden **Edit → PixelLab → Open plugin** tıkla
2. PixelLab penceresi açılır — sol tarafta araç listesi görürsün

---

**4 — Doğru aracı seç**
PixelLab penceresinin sol tarafındaki araç listesinde:
**"Create S-M image"** üzerine tıkla

→ Sağ tarafta o aracın ayar alanları açılır

---

**5 — Sabit parametreleri ayarla (bir kez yap, hep aynı kalacak)**
Açılan alanda şu dropdown'ları bul ve değiştir:

| Alan | Tıkla | Seç |
|------|-------|-----|
| **Camera view** | dropdown'a tıkla | `low top-down` |
| **Direction** | dropdown'a tıkla | `S` |
| **Outline** | dropdown'a tıkla | `selective outline` |
| **Shading** | dropdown'a tıkla | `basic shading` |
| **Detail** | dropdown'a tıkla | `medium detail` |
| **Remove background** | kutucuğa tıkla | ✓ (işaretli) |
| **Output method** | dropdown'a tıkla | `New layer` |

---

**6 — Gemini referansını ver**
1. **Init image** alanını bul (genelde "Init image" başlığı altında veya "Reference" butonu)
2. O alana tıkla veya yanındaki **klasör/yükle ikonuna** tıkla
3. Açılan dosya seçicide az önce kaydettiğin `warblade_ref.png` dosyasını seç ve aç
4. Küçük önizleme görünecek
5. **Init image strength** slider'ını bul → değeri `500` civarına getir (orta güçlü etki)

---

**7 — Promptu yaz**
1. En üstteki büyük **Description** metin kutusunu bul
2. İçini temizle (varsa eski metni sil)
3. Aşağıdaki metni kopyalayıp yapıştır:

```
Heavily armored dark iron plate warrior, large greatsword in right hand pointing
downward, cracked breastplate glowing cold blue, torn cape, heavy pauldrons,
facing downward (south), dark fantasy, top-down view
```

---

**8 — Üret**
1. Panelin alt kısmındaki **Generate** butonuna bas
2. Birkaç saniye bekle
3. Sonuç Aseprite canvas'ında yeni bir layer olarak belirir

---

**9 — Değerlendir**
- ✅ Top-down açı belli, kılıç sağda, mavi glow var, arka plan yok → ilerle
- ❌ Beğenmiyorsan → **Generate** butonuna tekrar bas (yeni sonuç üretir)
- ❌ 3 denemede de kötüyse → Gemini'ye dön, farklı referans üret, `warblade_ref.png` üzerine kaydet, adım 6'ya dön

---

**10 — Kaydet (.aseprite formatı)**
1. **File → Save As** tıkla
2. Dosya adı: `warblade_S_BASE.aseprite`
3. Konum: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\`
4. **Kaydet** bas

**11 — PNG olarak export et**
1. **File → Export → Export As** tıkla (veya `Ctrl+Alt+Shift+S`)
2. Dosya adı: `warblade_S_BASE.png`
3. Aynı klasöre kaydet
4. **Export** bas

---

### WARBLADE IDLE — `ART/karakterler/warblade/warblade_S_idle.png`
**4 frame / 6 FPS | Araç: Animation to Animation**

*BASE sprite Aseprite'ta açık olmalı. BASE ürettikten hemen sonra aynı oturumda yap.*

---

**1 — PixelLab'ı aç (zaten açıksa devam et)**
**Edit → PixelLab → Open plugin**

**2 — Aracı seç**
Sol araç listesinde **"Animation to Animation"** tıkla

**3 — Ayarları yap**

| Alan | Değer |
|------|-------|
| **Canvas size** dropdown | `64x64` |
| **Camera view** | `low top-down` |
| **Direction** | `S` |
| **Outline** | `selective outline` |
| **Shading** | `basic shading` |
| **Number of frames** slider/alan | `4` |

**4 — Karakter tanımını yaz**
**Description** (üstteki) metin kutusuna:
```
heavily armored dark iron warrior with cracked glowing breastplate and greatsword
```

**5 — Eylemi yaz**
**Action description** (alttaki) metin kutusuna:
```
standing idle, slow breathing, slight weapon sway, cold blue light pulsing from chest crack
```

**6 — Init image olarak BASE sprite'ı ver**
- **Init image** alanında frame seçici var → `warblade_S_BASE` frame'ini seç
- **Init image strength** → `600` (karakteri tutarlı tut)

**7 — Üret**
**Generate** butonuna bas → Timeline'da 4 frame oluşacak
→ Alt kısımdaki **▶ Play** butonuna basarak önizle

**8 — Sprite Sheet olarak export et**
1. **File → Export Sprite Sheet** tıkla
2. Açılan pencerede:
   - **Layout** bölümünde **Sheet type:** `By Rows` seç
   - **Columns:** `4`
   - **Rows:** `1`
3. **Output** bölümünde:
   - **Output file** kutusunu değiştir: `warblade_S_idle.png`
   - Klasör: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\`
4. **Borders** bölümünde:
   - **Trim sprite** kutucuğu: ✓
   - **Extrude:** `1`
5. **Data** bölümünde:
   - **Output file** (JSON): `warblade_S_idle.json` (aynı klasöre)
6. **Export** butonuna bas

---

### WARBLADE DİĞER ANİMASYONLAR
*Idle ile TAM AYNI workflow. Sadece Description + Action + frame sayısı değişiyor.*
*Her animasyon için: Animation to Animation aracı → aynı ayarlar → farklı action metni → Export*

**WALK — 6 frame / 10 FPS — `warblade_S_walk.png`**
- Description: `heavily armored dark iron warrior with cracked breastplate and greatsword`
- Action: `walking south, heavy armored footsteps, armor plates shifting with weight, cape moving`
- Number of frames: `6`
- Export: Columns `6`

**ATTACK 1 (Cleave) — 6 frame / 12 FPS — `warblade_S_attack1.png`**
- Description: `heavily armored dark iron warrior with cracked breastplate and greatsword`
- Action: `powerful horizontal greatsword slash, windup pulling sword back then strong swing`
- Number of frames: `6`
- Export: Columns `6`

**ATTACK 2 (Ground Slam) — 6 frame / 10 FPS — `warblade_S_attack2.png`**
- Description: `heavily armored dark iron warrior with cracked breastplate and greatsword`
- Action: `raising greatsword overhead with both hands then slamming into the ground, shockwave`
- Number of frames: `6`
- Export: Columns `6`

**DASH — 4 frame / 16 FPS — `warblade_S_dash.png`**
- Description: `heavily armored dark iron warrior with cracked breastplate and greatsword`
- Action: `explosive forward dash, body leaning aggressively forward, blur of motion`
- Number of frames: `4`
- Export: Columns `4`

**HURT — 3 frame / 12 FPS — `warblade_S_hurt.png`**
- Description: `heavily armored dark iron warrior with cracked breastplate and greatsword`
- Action: `staggering sharply backward from impact, brief recoil flinch`
- Number of frames: `3`
- Export: Columns `3`

**DEATH — 8 frame / 6 FPS — `warblade_S_death.png`**
- Description: `heavily armored dark iron warrior with cracked breastplate and greatsword`
- Action: `slowly collapsing forward, armor cracking apart, cold blue energy dispersing`
- Number of frames: `8`
- Export: Columns `8`

---

### SHARD WALKER BASE — `ART/duşmanlar/grunt_shard/grunt_shard_S_BASE.png`
**32×32px | Araç: Create S-M image**
*Warblade BASE ile AYNI workflow. Şu farklar:*

**Aseprite New dosya:** Width `32`, Height `32`, RGBA, Transparent

**Gemini referans promptu:**
```
A humanoid enemy assembled entirely from floating broken stone shards and bone
fragments. There is no solid body — only hovering pieces arranged in a vague
warrior shape, with cold blue light bleeding through the gaps between the shards.
Viewed from directly above. Retro pixel art, dark stone and cold blue palette.
Transparent background.
```

**PixelLab → Create S-M image → Description:**
```
Humanoid creature assembled from floating broken stone shards, cold blue
light bleeding through the gaps, no solid body, hovering shard warrior shape,
facing downward, dark fantasy enemy
```

*Sabit parametreler, Init image, Generate, Export — Warblade BASE adım 5-11 ile aynı.*

**Animasyonlar — Animation to Animation, 32×32 canvas size:**

| Dosya | Action metni | Frames |
|-------|-------------|--------|
| `grunt_shard_S_idle.png` | `hovering in place, stone shards slowly rotating, cold blue light pulsing` | 4 |
| `grunt_shard_S_walk.png` | `moving forward, shards separating and rejoining with each step` | 6 |
| `grunt_shard_S_attack.png` | `one arm shard launches forward as projectile then snaps back` | 5 |
| `grunt_shard_S_death.png` | `all shards exploding outward simultaneously, cold blue burst then gone` | 6 |

*Her animasyonda Description: `humanoid creature assembled from floating stone shards with cold blue light`*

---

### VOID THRALL BASE (Tam Form) — `ART/duşmanlar/grunt_thrall/grunt_thrall_S_BASE.png`
**32×32px | Araç: Create S-M image**
*Warblade BASE ile AYNI workflow. Şu farklar:*

**Aseprite New:** 32×32, RGBA, Transparent

**Gemini referans promptu:**
```
A dark armored humanoid soldier stands menacingly. Running vertically through
the center of their chest is a deep glowing crack, with void purple energy
seeping out from the fissure. Sinister medieval silhouette.
Viewed from directly above. Retro pixel art, dark iron and void purple #9E4FE0.
Transparent background.
```

**PixelLab → Create S-M image → Description:**
```
Dark armored soldier with deep glowing vertical crack through chest,
void purple energy seeping from the fissure, sinister medieval enemy,
facing downward, dark fantasy
```

---

### VOID THRALL BASE (Sol Yarı) — `ART/duşmanlar/grunt_thrall/grunt_thrall_half_left_S_BASE.png`
**32×32px | Araç: Create S-M image**
*Warblade BASE ile AYNI workflow. Şu farklar:*

**Gemini referans promptu:**
```
LEFT HALF ONLY of a dark armored soldier, cleanly split vertically down the center.
Void purple energy glowing at the split edge #9E4FE0. Still aggressive. Facing south.
Right side of canvas is completely empty/transparent. Retro pixel art, transparent background.
```

**PixelLab → Description:**
```
Left half only of dark armored soldier, split vertically, void purple energy
at split edge, aggressive stance, right half absent/transparent, facing downward
```

*Sağ yarı için aynı adımlar: promptlarda LEFT → RIGHT. Dosya: `grunt_thrall_half_right_S_BASE.png`*

---

### IRON WARDEN BASE — `ART/boss/iron_warden/boss_iron_warden_S_BASE.png`
**128×128px | Araç: Create medium-extra large image (Pixflux)**

*Warblade BASE ile aynı workflow AMA şu farklar kritik:*

**Aseprite New:** Width `128`, Height `128`, RGBA, Transparent

**Gemini referans promptu:**
```
A massive iron golem guardian towers over the battlefield. Full dark plate armor
heavily damaged, covered in deep cracks from which cold blue energy seeps through.
Several broken sword shards are embedded in its back and shoulders like trophies.
Radiates an overwhelming, slow, unstoppable presence.
Viewed from directly above. Retro pixel art, dark iron and cold blue #7BA7BC.
Transparent background.
```
*Açı kontrolü yap — yüz görünmemeli, geniş dev omuzlar tepeden görünmeli.*

**PixelLab aracı farkı:**
- Sol araç listesinde **"Create medium-extra large image (Pixflux)"** seç (Create S-M değil)
- Pixflux büyük sprite için daha iyi — bu aracı boss'lar için kullan

**Pixflux → Description:**
```
Massive heavily armored iron golem guardian, full dark plate armor with deep cracks,
cold blue energy seeping through cracked breastplate, broken swords embedded
in shoulders, imposing top-down view, overwhelming dark fantasy boss
```

*Sabit parametreler, Init image (Gemini referansı), Generate, Export — Warblade BASE gibi.*

**Animasyonlar — Animation to Animation, canvas size: 128×128:**

| Dosya | Action metni | Frames |
|-------|-------------|--------|
| `boss_iron_warden_S_idle.png` | `standing menacingly still, very slow heavy breathing, armor barely shifting, cold blue glow pulsing` | 6 |
| `boss_iron_warden_S_attack1.png` | `raising massive sword then devastating overhead slam, ground shockwave on impact` | 8 |
| `boss_iron_warden_S_charge.png` | `slow windup pulling shield back, explosive forward shield charge, heavy collision` | 10 |
| `boss_iron_warden_S_hurt.png` | `massive body barely flinching from hit, minor stagger, shrugging it off` | 4 |
| `boss_iron_warden_S_death.png` | `slowly collapsing forward, massive armor cracking open, cold blue energy erupting then dispersing` | 14 |

*Her animasyonda Description: `massive heavily armored iron golem boss with cracked glowing breastplate and embedded sword shards`*

---

### ACT 1 FLOOR TİLELER — `act1_floor_01/02/03.png`
**16×16px | Araç: Create S-M image (veya Gemini)**

**Aseprite New:** 16×16, RGBA, Transparent

**Gemini promptu (3 tile birden iste):**
```
Pixel art tile, 16x16 pixels, seamless tileable, top-down 2D dark fantasy
dungeon floor, dark grey cracked cobblestone #1E1E32 with thin cold blue
glowing fissures #7BA7BC, dungeon atmosphere, transparent background,
3 slight variations of the same tile design
```
*3 farklı sonuç üretecek — her birini ayrı kaydet, logo_resize yerine direkt kullan.*

**PixelLab → Create S-M image → Description (Gemini yeterli çıkmazsa):**
```
Dark grey cracked cobblestone dungeon floor tile, thin cold blue glowing fissures,
seamless tileable, top-down dark fantasy, single tile
```

**Export:** File → Export → Export As → `act1_floor_01.png` (Sprite Sheet değil, düz PNG)

---

### ACT 1 WALL TİLELER — `act1_wall_01/02.png`
**16×32px | Araç: Create S-M image**

*Floor tile ile AYNI workflow. Şu farklar:*
- **Aseprite New:** Width `16`, Height `32`

**PixelLab → Description:**
```
Crumbling dark fortress stone wall tile, cold blue light barely in cracks,
seamless tileable, top-down dark fantasy, 16x32 wall tile
```

---

### ACT 1 CRACK OVERLAY — `act1_crack_01/02/03/04.png`
**16×16px | Gemini yeterli, PixelLab gerekmez**

**Aseprite New:** 16×16, RGBA, Transparent

**Gemini promptu:**
```
Pixel art overlay sprite, 16x16 pixels, transparent background,
thin crack line pattern only, 1-2 pixel wide, cold blue glow #7BA7BC,
floor decoration overlay for a dark fantasy game, 4 different crack angles
```
*4 farklı çıktı üretecek — her birini ayrı kaydet.*

**Export:** File → Export → Export As → `act1_crack_01.png`

---

### VFX HIT SPARK — `ART/vfx/vfx_hit_spark.png`
**32×32px, 5 frame | Gemini → Aseprite'ta import**

**Gemini promptu:**
```
Pixel art VFX sprite sheet, 5 frames horizontal, 32x32px each (total 160x32),
hit impact spark expanding then fading, frame 1 small white center 8px,
frame 2 burst 16px, frame 3 24px 80% opacity, frame 4 fading, frame 5 gone,
transparent background
```

**Aseprite'ta import:**
1. **File → Import** → `vfx_hit_spark_sheet.png` seç
2. Açılan Import dialog'unda:
   - **Import as sprite sheet** seç
   - **Frame width:** `32`
   - **Frame height:** `32`
3. → 5 frame ayrılır

**Export Sprite Sheet:** Columns `5`, `vfx_hit_spark.png`

---

### VFX SWORD TRAIL — `ART/vfx/vfx_sword_trail.png`
**64×32px, 4 frame | Gemini → import**

**Gemini promptu:**
```
Pixel art VFX sprite sheet, 4 frames horizontal, 64x32px each (total 256x32),
sword slash arc trail, white to gold #FFD700 fading, frame 1 bright white arc,
frames 2-3 gold fading, frame 4 barely visible, transparent background
```

**Aseprite'ta import:** Frame width `64`, height `32` → 4 frame
**Export Sprite Sheet:** Columns `4`, `vfx_sword_trail.png`

---

### SKILL İKONLARI — `ART/ui/icons/skills/skill_[isim].png`
**32×32px | Araç: Create S-M image**

**Aseprite New:** 32×32, RGBA, Transparent
**PixelLab → Create S-M image**
**Remove background:** ✓ (açık)

**Her ikon için Description şablonu:**
```
Pixel art skill icon, 32x32, centered composition, dark background #1E1E32,
[SKILL GÖRSELI VE RENK VURGU], clear silhouette at small size, game UI icon
```

| Dosya | Description tamamlama |
|-------|----------------------|
| `skill_iron_charge.png` | `armored gauntlet with forward momentum arrow, cold blue impact burst #7BA7BC accent` |
| `skill_death_blow.png` | `greatsword vertical downstroke, blood red edge glow, dramatic dark` |
| `skill_bladestorm.png` | `sword silhouette in circular spinning motion, gold energy trail #FFD700` |
| `skill_cleave.png` | `horizontal sword slash arc with shockwave lines, steel grey and cold blue` |
| `skill_gravity_cleave.png` | `sword slamming ground, circular pull force lines, cold blue vortex` |
| `skill_war_stomp.png` | `armored boot slamming down, shockwave rings expanding outward, gold dust` |

*Diğer Warblade ikonları için aynı şablonu kullan, görsel tanımı değiştir.*

---

## FAZ 2 — DEMO HAZIRLIK

---

### ELEMENTALİST BASE — `ART/karakterler/elementalist/elementalist_S_BASE.png`
**64×64px | Araç: Create S-M image**
*Warblade BASE ile AYNI workflow (tüm adımlar). Şu farklar:*

**Aseprite New:** 64×64, RGBA, Transparent

**Gemini referans promptu:**
```
A dark-robed mage stands with two arcane tomes floating and orbiting around them.
Left hand emanates cold blue frost crystals, right hand crackles with orange-red fire.
Elaborate hood, staff topped with a split glowing crystal.
Viewed from directly above. Retro pixel art, dark robes, cold blue, fire orange.
Transparent background.
```

**PixelLab → Create S-M image → Description:**
```
Dark-robed mage with two floating arcane tomes, left hand frost crystals,
right hand fire sparks, elaborate hood, split crystal staff, facing downward
```

**Animasyonlar — Animation to Animation, 64×64:**

| Dosya | Action metni | Frames |
|-------|-------------|--------|
| `elementalist_S_idle.png` | `standing still, tomes slowly orbiting, elemental energy flickering between hands` | 4 |
| `elementalist_S_walk.png` | `gliding forward, robes flowing, tomes trailing behind` | 6 |
| `elementalist_S_attack1.png` | `casting ice shards forward, frost cone launching from left hand` | 6 |
| `elementalist_S_attack2.png` | `slamming both hands down, fire explosion erupting forward` | 6 |
| `elementalist_S_dash.png` | `blinking forward in burst of frost energy` | 4 |
| `elementalist_S_hurt.png` | `staggering, magical aura flickering` | 3 |
| `elementalist_S_death.png` | `collapsing, tomes crashing, energy dispersing` | 8 |

*Description: `dark-robed mage with floating arcane tomes, frost left hand, fire right hand, split crystal staff`*

---

### SHADOWBLADE BASE — `ART/karakterler/shadowblade/shadowblade_S_BASE.png`
**64×64px | Araç: Create S-M image**
*Warblade BASE ile AYNI workflow. Şu farklar:*

**Gemini referans promptu:**
```
A sleek assassin crouches in a tense, ready stance. Dark leather armor,
half-face mask, dual daggers at hips. Cloak absorbs light rather than reflects it.
Viewed from directly above. Retro pixel art, near-black and deep purple, minimal color.
Transparent background.
```

**PixelLab → Description:**
```
Sleek assassin in dark leather armor, dual daggers at hips, half-face mask,
cloak absorbing light, crouching aggressive ready stance, facing downward
```

**Animasyonlar — 64×64:**

| Dosya | Action metni | Frames |
|-------|-------------|--------|
| `shadowblade_S_idle.png` | `crouching still, cloak barely shifting, daggers glinting` | 4 |
| `shadowblade_S_walk.png` | `gliding forward silently, nearly weightless steps` | 6 |
| `shadowblade_S_attack1.png` | `quick double dagger slash, blur of motion` | 6 |
| `shadowblade_S_attack2.png` | `lunge stab forward, retract` | 5 |
| `shadowblade_S_dash.png` | `disappear frame 1, reappear frame 3 blink dash` | 4 |
| `shadowblade_S_hurt.png` | `sharp recoil, cloak flaring` | 3 |
| `shadowblade_S_death.png` | `crumpling silently, form dissolving into shadow particles` | 8 |

*Description: `sleek assassin in dark leather armor with dual daggers and light-absorbing cloak`*

---

### RANGER BASE — `ART/karakterler/ranger/ranger_S_BASE.png`
**64×64px | Araç: Create S-M image**
*Warblade BASE ile AYNI workflow. Şu farklar:*

**Gemini referans promptu:**
```
A lean archer in an alert, ready stance with longbow half-drawn.
Worn leather armor with bone and earth accents, quiver on back, hood pulled back.
Viewed from directly above. Retro pixel art, earthy brown, bone white, leather tan.
Transparent background.
```

**PixelLab → Description:**
```
Lean archer in worn leather armor, longbow drawn halfway, quiver on back,
hood pulled back, bone and earth accents, alert stance, facing downward
```

**Animasyonlar — 64×64:**

| Dosya | Action metni | Frames |
|-------|-------------|--------|
| `ranger_S_idle.png` | `standing alert, bow at half-draw, scanning the area` | 4 |
| `ranger_S_walk.png` | `moving carefully, bow ready, light careful footsteps` | 6 |
| `ranger_S_attack1.png` | `drawing and releasing arrow, snap bow release` | 6 |
| `ranger_S_attack2.png` | `rapid triple shot, three quick releases in succession` | 7 |
| `ranger_S_dash.png` | `rolling sideways, quick dodge roll` | 4 |
| `ranger_S_hurt.png` | `stumbling back, quiver rattling` | 3 |
| `ranger_S_death.png` | `falling backward, bow dropping from grip` | 8 |

*Description: `lean archer in worn leather armor with longbow and quiver on back`*

---

### FAZ 2 DÜŞMANLAR — Özet Tablo
**Hepsi 32×32px | Araç: Create S-M image | Warblade BASE workflow**

#### SEAM CRAWLER
**Gemini promptu:**
```
Horror creature inside a floor crack, only two long dark claws and spine ridge
visible above the surface, pressed flat, lurking. Viewed from directly above.
Void black #080808, void purple glow #9E4FE0 from crack, bone claws. Transparent background.
```
**PixelLab Description:** `horror creature inside floor crack, only dark claws and spine ridge above surface, void purple glow, lurking enemy, facing downward`

**Animasyonlar:**
| Action | Frames | Dosya |
|--------|--------|-------|
| `claws slowly scraping stone surface` | 4 | `grunt_seam_S_idle.png` |
| `claws shooting upward from crack, striking, retracting` | 6 | `grunt_seam_S_attack.png` |
| `claws retracting, crack sealing shut` | 5 | `grunt_seam_S_death.png` |

---

#### ECHO HOUND
**Gemini promptu:**
```
Ghostly wolf predator with semi-transparent indigo energy form #3D1F6B #9E4FE0,
white glowing eyes #D4D4F0, no solid flesh — just energy lines and silhouette.
Low predatory crouching stance. Viewed from directly above. Transparent background.
```
**PixelLab Description:** `ghostly wolf, semi-transparent indigo energy form, white glowing eyes, no solid body just energy lines, predatory crouching stance, facing downward`

**Animasyonlar:**
| Action | Frames | Dosya |
|--------|--------|-------|
| `form flickering, hovering in place, eyes glowing` | 4 | `grunt_echo_S_idle.png` |
| `bounding forward with strong motion trail` | 6 | `grunt_echo_S_walk.png` |
| `blinking forward snapping, blinking back` | 5 | `grunt_echo_S_attack.png` |
| `form dispersing into indigo particles` | 6 | `grunt_echo_S_death.png` |

---

#### HOLLOW MITE
*⚠️ Sprite alanının sadece %30'unu doldursun — bilerek küçük*

**PixelLab Description:** `tiny hollow insect creature occupying only 30 percent of canvas, transparent exoskeleton, six legs, glowing core inside, small swarm enemy, facing downward`

**Animasyonlar:** idle 4f, walk 6f yeterli

---

### THE TWICE-BORN ELITE — `elite_twice_born_A/B_S_BASE.png`
**64×64px | Araç: Create S-M image**

**SPRITE A (Saldırgan) — Description:**
```
Elite warrior with two sword arms — one armored physical arm, one spectral cold blue energy arm #7BA7BC,
cracked iron armor with cold blue glow, one eye glowing cold blue, aggressive battle stance, facing downward
```

**SPRITE B (Savunmacı) — Description:**
```
Elite guardian warrior companion, large shield raised defensively,
cracked iron armor with cold blue cracks matching Sprite A, protective stance, facing downward
```

---

### FRACTURE-BORN ELITE — `elite_fracture_phase1/2/3/4.png`
**64×64px | Araç: Create S-M image | Her faz ayrı üretim**

| Faz | Description |
|-----|-------------|
| Phase 1 | `single dark clawed hand and forearm reaching up from floor crack, most canvas empty/transparent, void purple energy at crack edges #9E4FE0` |
| Phase 2 | `dark armored creature half-emerged from floor crack, upper body visible, lower body still inside, void purple energy at edges, reaching upward` |
| Phase 3 | `fully emerged dark armored elite enemy standing, cracked armor with void purple energy #9E4FE0, powerful stance, facing downward` |
| Phase 4 | `dark armored elite in full rage, armor cracked fully open, void purple energy #9E4FE0 erupting from every crack, glowing eyes, fully enraged, facing downward` |

---

### ACT 2 TİLE SETİ — `ART/tiles/act2/`
*Act 1 tile workflow ile aynı*

**Floor (16×16) — Gemini promptu:**
```
Pixel art tile, 16x16, seamless tileable, top-down dark fantasy,
decayed swamp dungeon floor, rotting dark stone, void purple glowing veins #9E4FE0,
dark organic decay, transparent background
```

**Wall (16×32) — Gemini promptu:**
```
Pixel art tile, 16x32, seamless tileable, top-down dark fantasy,
corrupted dark temple stone wall, purple energy veins #9E4FE0, moss, decay
```

---

## FAZ 3+ — FULL CONTENT

---

### SPORE HOLLOW ELITE — `elite_spore_S_BASE.png` (64×64)
**PixelLab → Create S-M image → Description:**
```
Bloated hollow creature made of fungal growth, multiple spore vents on back
releasing toxic purple clouds #9E4FE0, swollen dangerous form like a living bomb,
facing downward, dark organic brown and toxic purple
```

---

### RIFT MAW ELITE — `elite_rift_maw_S_BASE.png` (64×64)
**PixelLab → Create S-M image → Description:**
```
Living dimensional rift creature, body is a tear in reality shaped like a predator,
gold glowing edges #FFD700, void black inside, no solid form, otherworldly, facing downward
```

---

### HOLLOW SOVEREIGN BOSS — `boss_hollow_sovereign_S_BASE.png`
**128×128px | Araç: Create medium-extra large image (Pixflux)**
*Iron Warden Boss ile AYNI workflow. Şu farklar:*

**Gemini referans promptu:**
```
The Hollow Sovereign — tall imposing ruler made of void, hollow transparent body
you can see through, only outline and royal accessories visible,
floating gold crown #FFD700 above empty head, royal armor outline only.
Viewed from directly above. Act 3 palette: gold #FFD700, void black, ghost white #D4D4F0.
Transparent background.
```

**Pixflux → Description:**
```
Hollow void sovereign ruler, transparent empty body only outline visible,
floating gold crown, royal armor outline, shifting adaptive form,
dark fantasy Act 3 boss, facing downward
```

**Animasyonlar — 128×128:**
| Action | Frames | Dosya |
|--------|--------|-------|
| `hollow form slowly shifting and warping, crown floating slightly` | 6 | `boss_hollow_sovereign_S_idle.png` |
| `void energy beam projecting from hollow body` | 8 | `boss_hollow_sovereign_S_attack1.png` |
| `form splitting and rejoining around player position` | 10 | `boss_hollow_sovereign_S_attack2.png` |
| `body reshaping in visual morph sequence` | 6 | `boss_hollow_sovereign_S_adapt.png` |
| `form collapsing inward then exploding outward in gold light` | 14 | `boss_hollow_sovereign_S_death.png` |

---

### NEXUS CORE FINAL BOSS — `boss_nexus_core_S_BASE.png`
**128×128px | Araç: Pixflux**

**Pixflux → Description (Base):**
```
Massive dark crystal lattice structure, gold energy radiating from every crack #FFD700,
cold blue #7BA7BC and void purple #9E4FE0 also visible, frozen geometric form,
shape subtly echoes a humanoid silhouette, dark fantasy final boss, facing downward
```

**Faz sprite'ları (ayrı dosyalar, aynı base Description'a ekle):**
- Phase 1: `+ fully closed, sealed form, minimal glow`
- Phase 2: `+ beginning to crack open, humanoid silhouette faintly appearing inside`
- Phase 3: `+ half open, player's class form clearly mirrored inside, intense multi-color glow`

---

### ACT 3 TİLE SETİ
**Floor (16×16) — Gemini:**
```
Pixel art tile, 16x16, seamless tileable, top-down dark fantasy,
void floor — absolute darkness with deep gold glowing cracks #FFD700,
kintsugi aesthetic on void material, transparent background
```

**Wall (16×32) — Gemini:**
```
Pixel art tile, 16x32, seamless tileable, void wall,
pure void material with gold structural energy lines #FFD700, impossible geometry
```

---

## FAZ 4 — HUB + NPC'LER

---

### HUB TİLE SETİ
**Floor (16×16) — Gemini:**
```
Pixel art tile, 16x16, seamless tileable, top-down dark fantasy Hub floor,
ancient stone worn smooth, warm candlelight amber glow in cracks #D4956A,
melancholic atmosphere, transparent background
```

**Wall (16×32) — Gemini:**
```
Pixel art tile, 16x32, seamless tileable, Hub walls,
ancient stone with worn barely-visible reliefs, warm amber cracks #D4956A
```

---

### HUB NPC'LER — 64×64px
*Warblade BASE workflow ile aynı — animasyon yok, sadece BASE sprite*

**THE FERRYMAN — Description:**
```
Ancient hooded figure in floor-length dark robes, face completely in shadow under deep hood,
unnaturally long fingers, perfectly still eternal posture, no color accents, facing downward
```
Gemini: `Ancient figure in floor-length dark robes, face completely shadowed under deep hood, unnaturally long fingers, perfectly still posture as if has stood here eternally. Viewed from directly above. Palette: void black only. Transparent background.`

**VREL — Description:**
```
Semi-transparent ghost blacksmith, one solid armored arm with spectral hammer,
rest of body drifting smoke, dark green-grey ghost form, focused posture, facing downward
```

**SISTER MOURNE — Description:**
```
Pale grey-robed mourning sister, ash markings around eyes, slight head bow,
warm amber robe trim #D4956A, compassionate heavy presence, facing downward
```

**THE CARTOGRAPHER — Description:**
```
Obsessive map-maker with maps and paper scraps pinned all over clothes,
ink-stained hands, magnifying glass, leaning forward intently, facing downward
```

---

## HIZLI BAŞVURU

### Hangi aracı ne zaman seç

| Asset tipi | PixelLab aracı |
|-----------|---------------|
| 16×16 – 64×64 BASE sprite | Create S-M image |
| 64×64 – 128×128 BASE sprite | Create large image |
| 128×128 boss BASE | Create medium-extra large image (Pixflux) |
| Tüm animasyonlar | Animation to Animation |
| Arka plan kaldırma | Remove background |

### Export Sprite Sheet — Her animasyon için hızlı ayar
File → Export Sprite Sheet:
- Sheet type: `By Rows`
- Columns: `[frame sayısı]`
- Rows: `1`
- Trim sprite: ✓
- Extrude: `1`
- JSON output file: ✓
- Export bas

### Sorun giderme

| Sorun | Çözüm |
|-------|-------|
| Edit → PixelLab yok | Edit → Preferences → Extensions → eklenti kurulu mu? Aseprite'ı yeniden başlat |
| Sprite 3/4 açıda çıktı | Gemini referansını yenile, "strictly from directly above, bird's eye" vurgula |
| Arka plan kaldırılmadı | Remove background tool'u ayrıca çalıştır, veya remove.bg sitesini kullan |
| 128×128 Create S-M'e sığmıyor | Pixflux kullan (Create medium-extra large) |
| Animasyon frame'leri tutarsız | Init image olarak BASE sprite'ı ver, Init strength'i 600'e çek |
| Tile seamless değil | Aseprite'ta View → Tiled Mode aktif et → görsel olarak kontrol et |
