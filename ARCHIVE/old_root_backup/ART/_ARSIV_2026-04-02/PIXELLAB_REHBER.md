# ⚠️ ARŞİV — Bu dosya artık kullanılmıyor
**TEK rehber: `URETIM_PLANI.md`** — tüm adımlar orada. Bu dosya eski format referansı olarak tutuluyor.

# RIMA — PixelLab Aseprite Extension — Tıklama Rehberi (ARŞİV)
*PixelLab resmi dokümantasyonundan doğrulanmış, sıfırdan yazıldı — 2026-03-31 —claude*
*Kaynak: pixellab.ai/docs — Hiçbir şeyi elle çizmiyorsun. Hepsi AI.*

### 📋 İlerleme Takibi
Bir asset'i bitirdiğinde başlığın yanına **✅** ekle. Örnek:
- `## WARBLADE` → `## ✅ WARBLADE`
- Böylece kaldığın yeri hemen bulursun.

---

## REFERANS ZİNCİRİ — NASIL ÇALIŞIYOR?

Her karakter/düşman için 3 aşama var. Önemli olan **neyi referans olarak kullandığın:**

```
┌─────────────────────────────────────────────────────────────────┐
│                                                                 │
│   1. GEMİNİ  ──referans──▶  2. BASE SPRİTE  ──referans──▶  3. ANİMASYONLAR  │
│   (konsept)     (Init Image)   (tek frame)     (Reference)    (çoklu frame)   │
│                                                                 │
│   Gemini'den      PixelLab          BASE'i          Her animasyon      │
│   top-down        "Create S-M"      referans alıp    (idle, walk,      │
│   görsel al       ile pixel art      "Animate"        attack...) hep    │
│                   BASE üret          ile animasyon     BASE'den türer    │
│                                      üret                               │
└─────────────────────────────────────────────────────────────────┘
```

**KRİTİK KURALLAR:**

1. **Gemini görseli → sadece BASE üretimi için** kullanılır (Init Image olarak)
2. **BASE sprite → TÜM animasyonlar için** referans olarak kullanılır
3. **Animasyonlar birbirinden bağımsız** — idle'dan attack türetme, her şey BASE'den
4. Her animasyonu export ettikten sonra **Ctrl+Z ile BASE'e geri dön**, sonra sırakini üret

---

## ÖNEMLİ — Plugin'i ilk kez açıyorsan

1. Aseprite açık olmalı (lisanslı versiyon, 1.3.7+)
2. Üst menüden: **Edit → PixelLab → Open plugin** tıkla
3. PixelLab paneli açılır — genellikle **sağ tarafta** görünür
4. Panel içinde üst kısımda **hesap giriş durumu** görünür — giriş yapmış olman lazım

---

## PANELİN YAPISI — Ne görüyorsun?

PixelLab panelini açtığında **sağ tarafta bir araç listesi** görürsün.
Bunların hepsi farklı şeyler yapar. RIMA için kullanacakların:

| Panel'de gördüğün isim | RIMA'da ne için kullanılır |
|-----------------------|--------------------------|
| **Create S-M image** | Karakter, düşman, ikon BASE sprite (16–140px) |
| **Create M-XL image** | Boss sprite'ları (128px — Pixflux modeli) |
| **Animate with text** | TÜM animasyonlar (idle, walk, attack...) |
| **Create tileset** | Floor ve wall tile'ları |
| **Remove background** | Gemini çıktısında arkaplan varsa kaldır |

> Listedeki diğer araçlar (Sketch to Image, Re-pose, Transfer outfit, Edit image, Unzoom, vb.) RIMA'da şimdilik kullanmıyoruz.

---

# STANDART WORKFLOW'LAR

Aşağıdaki 4 workflow tekrar tekrar kullanılacak.
Her asset için "hangi workflow → hangi parametreler" diyeceğim.

---

## WORKFLOW A — BASE Sprite (Karakter / Düşman)
*Tüm 48×48-64×64 karakterler ve 32×32 düşmanlar için*

### A1 — Gemini'de top-down referans üret

1. Tarayıcında **gemini.google.com** aç → yeni sohbet
2. Prompt kutusuna `URETIM_PLANI.md`'deki **Gemini Prompt**'u kopyala yapıştır → Enter
3. Gelen görsele bak:
   - ✅ **Omuzlar geniş, kafa tepeden görünüyor, yüz yok, ayaklar küçük/yok** → iyi
   - ❌ Yüz görünüyor / yan profil / ayaklar büyük → aynı chat'e şunu yaz:
     `Now show the exact same character but viewed strictly from directly above, bird's eye view, head at top, feet barely visible or hidden`
4. Beğendiğin görsele **sağ tıkla → Resmi farklı kaydet**
   - İstediğin bir yere kaydet (masaüstü olabilir), adını hatırla

---

### A2 — Aseprite'ta yeni canvas aç

1. Aseprite'ı aç
2. Üst menü: **File → New...** (veya `Ctrl+N`)
3. Açılan pencerede:
   - **Width:** `64` (düşmanlar için `32`)
   - **Height:** `64` (düşmanlar için `32`)
   - **Color Mode:** `RGBA Color` seç
   - **Background:** `Transparent` seç
4. **OK** bas → damalı boş canvas açılır (dama = şeffaflık)

---

### A3 — Gemini referansını canvas'a ekle

1. Az önce kaydettiğin Gemini görselini dosya gezgininde bul
2. Görseli **Aseprite penceresine sürükle bırak** (drag & drop)
   - VEYA: **File → Open** ile ayrı sekme aç → `Ctrl+A` (seç) → `Ctrl+C` (kopyala) → 64×64 canvas sekmesine geç → `Ctrl+V` (yapıştır)
3. Görsel canvas'a geldi — boyutu canvas'a uymuyorsa:
   - Üst menü: **Sprite → Sprite Size...** → Width ve Height'ı canvas boyutuna eşitle → OK

---

### A4 — PixelLab ile BASE sprite üret

1. Üst menü: **Edit → PixelLab → Open plugin** tıkla
2. Sağ taraftaki PixelLab paneli açılır
3. Panelde araç listesini gör — **"Create S-M image"** üzerine tıkla
   - (Boss için: **"Create M-XL image"** — ileride ayrı açıklanıyor)
4. Sağ tarafta o aracın parametreleri açılır. Sırayla doldur:

**Description** metin kutusu (büyük, en üstteki):
→ `URETIM_PLANI.md`'deki "PixelLab Description" metnini kopyala yapıştır

**Negative Description** metin kutusu (hemen altında):
→ şunu yaz: `side view, face visible, front facing, 3/4 view`

**Camera View** dropdown'u:
→ tıkla → **Low top-down** seç

**Direction** dropdown'u:
→ tıkla → **South** seç

**Outline** dropdown'u:
→ tıkla → **Selective outline** seç

**Shading** dropdown'u:
→ tıkla → **Basic shading** seç

**Details** dropdown'u:
→ tıkla → **Medium detail** seç

**Init Image** bölümünü bul:
→ yanındaki **Set** butonuna tıkla
→ dosya seçici açılır → Gemini'den indirdiğin görseli seç → Aç
→ küçük önizleme görünür
→ altındaki **Init Image Strength** slider'ını bul → değeri **500** civarına getir

**Background Removal** toggle'ı:
→ **açık** konuma getir (etkinleştir)

**Output Method** dropdown'u:
→ tıkla → **Add new layer** seç

5. Panelin en altındaki **Generate** butonuna bas
6. Birkaç saniye bekle → sonuç canvas'ta yeni bir layer olarak görünür

**Değerlendirme:**
- ✅ Top-down açı, karakter tanınıyor, arkaplan yok → iyisin
- ❌ Beğenmiyorsan → hiçbir şeyi değiştirmeden tekrar **Generate** bas (farklı sonuç üretir)
- ❌ 3-4 denemede de olmuyorsa → A1'e dön, Gemini'de farklı referans üret

---

### A5 — Kaydet ve export et

1. **File → Save As...**
   - Konum: `URETIM_PLANI.md`'deki "Aseprite Kayıt" yolu
   - Format: `.aseprite` (otomatik seçili)
   - **Save** bas

2. **File → Export → Export As...** (`Ctrl+Alt+Shift+S`)
   - Konum: `URETIM_PLANI.md`'deki "Export BASE" yolu
   - Format: `.png` (uzantıyı .png yap)
   - **Export** bas

BASE sprite tamamlandı. Şimdi animasyonlar için → **WORKFLOW B**

---

## WORKFLOW B — Animasyon (Animate with text)
*BASE sprite açıkken yapılır. Her animasyon için tekrarla.*

⚠️ **ÖNEMLİ:** Her animasyonu üretip export ettikten sonra **Ctrl+Z ile BASE'e geri döneceksin.** Böylece bir sonraki animasyonu yine tek-frame BASE'den üretirsin. Detay → B6 adımına bak.

### B1 — BASE sprite Aseprite'ta açık olmalı

Eğer az önce A5'te kaydettin, zaten açık. Değilse:
- **File → Open** → `.aseprite` dosyasını aç

---

### B2 — Animate with text aracını aç

1. **Edit → PixelLab → Open plugin** (zaten açıksa devam et)
2. Araç listesinde **"Animate with text"** üzerine tıkla
3. Sağda parametreler açılır

---

### B3 — Parametreleri doldur

**Reference Image** bölümünü bul:
→ **Set** butonuna tıkla
→ dosya seçici açılır → **BASE sprite'ın `.png` export'unu seç** (A5'te export ettiğin dosya, örn: `warblade_S_BASE.png`)
→ Aç → küçük önizleme görünür
→ ⚡ **Bu adım kritik!** Gemini görselini DEĞİL, PixelLab ile ürettiğin BASE sprite'ı seçiyorsun. Çünkü animasyonlar BASE'den türeyecek.

**Description** kutusu (karakter tanımı):
→ Animasyon tablosundaki **Description** sütununu yaz
→ (Genellikle BASE ile aynı karakter tanımı — örn: `heavily armored dark iron warrior with greatsword`)

**Action Description** kutusu (ne yapıyor):
→ `URETIM_PLANI.md`'deki animasyon tablosundaki **Action** sütununu kopyala yapıştır

**Camera View** dropdown:
→ **Low top-down** seç

**Direction** dropdown:
→ **South** seç

**No Background** toggle:
→ **açık** yap

**Output Method** dropdown:
→ **Add new layer** seç

---

### B4 — Üret

1. **Generate** butonuna bas
2. Bekle → Aseprite'ın **alt kısmındaki Timeline**'a bak
   - Birden fazla frame oluştu mu? → oluştuysa iyi
   - Frame sayısı: araç otomatik belirler (canvas 64×64 ise genellikle 16 frame üretir)
3. Alt kısımdaki **▶ Play** butonuna bas → animasyonu önizle

**Değerlendirme:**
- ✅ Karakter tanınıyor, hareket mantıklı, arkaplan yok → ilerle
- ❌ Beğenmiyorsan → tekrar **Generate** bas
- ❌ Fazla frame var, bir kısmını kullanmak istiyorsan → Timeline'da istemediğin frame'lere tıkla → sağ tık → Delete Frame

---

### B5 — Sprite Sheet olarak export et

1. Üst menü: **File → Export Sprite Sheet...**
2. Açılan pencerede **Layout** bölümünü bul:
   - **Sheet type:** `By Rows` seç
   - **Columns:** kullanacağın frame sayısını yaz (örn. `6`)
   - **Rows:** `1`
3. **Sprite** bölümünü bul:
   - **Trim sprite:** kutucuğu işaretle ✓
   - **Extrude:** `1` yaz
4. **Output** bölümünü bul:
   - **Output file** kutusundaki yolu değiştir → `URETIM_PLANI.md`'deki animasyon tablosundaki export yolunu yaz
   - **JSON data:** kutucuğu işaretle ✓ (aynı klasöre .json kaydeder)
5. **Export** butonuna bas

---

### B6 — ⚠️ BASE'e geri dön (sonraki animasyondan ÖNCE)

Export bitti. Canvas'ta şu an çoklu animasyon frame'leri var. **Diğer animasyona geçmeden önce BASE'e dönmen LAZIM:**

**Yol A — Ctrl+Z (en hızlı):**
1. `Ctrl+Z` bas — birden fazla kez (frame sayısı + alpha kadar)
2. Canvas'ta tekrar **tek frame** kalana kadar bas
3. Timeline'da sadece 1 frame görünüyorsa → BASE'e döndün ✅
4. Sıradaki animasyon için → B2'ye geri dön

**Yol B — Dosyayı kaydetmeden kapat, tekrar aç:**
1. `File → Close` → "Don't Save" (Kaydetme) seç
2. `File → Open` → `xxxxx_S_BASE.aseprite` tekrar aç
3. Sıradaki animasyon için → B2'ye geri dön

⚡ **Yol A tavsiye** — daha hızlı. Sadece Ctrl+Z basıyorsun.

**NEDEN?** Eğer idle frame'leri canvas'ta duruyorken attack üretirsen, PixelLab idle'ın son frame'ini referans alır → sonuç beklediğin gibi olmaz. BASE = her zaman en temiz referans noktası.

---

## WORKFLOW C — Tileset (Create tileset)
*Floor ve wall tile'ları için*

### C1 — Yeni canvas aç

1. **File → New...**
   - Floor için: Width `16`, Height `16`
   - Wall için: Width `16`, Height `32`
   - Color Mode: `RGBA Color`
   - Background: `Transparent`
2. OK

---

### C2 — Create tileset aracını aç

1. **Edit → PixelLab → Open plugin**
2. Araç listesinde **"Create tileset"** üzerine tıkla
3. Sağda parametreler açılır

---

### C3 — Parametreleri doldur

**Tileset type** seçimi (en üstte):
→ **Top-Down** seç (yan kaydırıcı bölüm platformer için, biz top-down yapıyoruz)

**Inner Description** kutusu:
→ Tile'ın ortasının ne olduğunu yaz → `URETIM_PLANI.md`'deki PixelLab Description metnini yaz

**Transition Description** kutusu:
→ Tile kenar geçişi için → boş bırakabilirsin veya "cracked edge" yaz

**Outer Description** kutusu:
→ Tile çevresinin ne olduğunu yaz → genellikle Inner ile aynı stil

**Tile Size** dropdown:
→ Floor için: **16x16** seç
→ Wall için: 16×32 option yoksa 32×32 seç ve sonra Aseprite'ta boyutlandır

**Outline** dropdown:
→ **Selective outline** seç

**Shading** dropdown:
→ **Basic shading** seç

**Details** dropdown:
→ **Medium detail** seç

**Output Method** dropdown:
→ **Modify current layer** seç

---

### C4 — Üret ve export et

1. **Generate** bas → tile canvas'ta görünür
2. Seamless mi kontrol et: Üst menü **View → Tiled Mode** aç → tile yan yana durduğunda boşluk var mı bak
3. ✅ Seamless görünüyorsa → export et
4. ❌ Seamless değilse → **Generate** tekrar bas (farklı seed, farklı sonuç)
5. Export: **File → Export → Export As...** → `URETIM_PLANI.md`'deki export yolunu yaz → Export

**Varyasyon üretmek için:** Hiçbir şeyi değiştirmeden tekrar **Generate** bas → farklı isimle export et (örn. `act1_floor_02.png`)

---

## WORKFLOW D — Boss Sprite (Create M-XL image)
*128×128 boss sprite'ları için — Pixflux modeli kullanır*

### Workflow A ile TAMAMEN AYNI adımlar, TEK fark:

**A2'de Canvas:**
- Width: `128`, Height: `128`

**A4'te araç seçimi:**
- "Create S-M image" yerine **"Create M-XL image"** tıkla

**⚠️ Önemli uyarı:**
- 128×128 Create S-M image'a SIĞMAZ — muhakkak Create M-XL image kullan
- Animasyonlarda frame sayısını 8'de tut (büyük canvas = fazla hesaplama)

Geri kalan tüm adımlar (parametreler, Generate, export) Workflow A ile aynı.

---

# FAZ 0

---

## LOGO

Logo PixelLab'ın boyut sınırını aşıyor. **Sadece Gemini ile üretilecek.**

1. Tarayıcıda **gemini.google.com** → yeni sohbet
2. Şu promptu kopyala yapıştır:
```
A dark fantasy pixel art game logo. The large uppercase letters "RIMA" are written boldly in dark steel color, spanning the upper portion of the image. Below and to the right of the letter "I", the small lowercase letters "ft" hang downward at an angle — as if they cracked off from the word RIFT and are drooping, falling debris from a broken seal. They are smaller, slightly rotated, cracked in texture. Below and to the right of the letter "A", the small lowercase letters "rch" hang downward at the same angle — as if they cracked off from the word MARCH, same drooping effect. At the exact break points where "ft" detached from "RI" and "rch" detached from "MA", brilliant gold light bleeds through the crack, color #FFD700. Letter color: dark steel #1E1E32. Background: void black #080808. The hidden full words RIFT and MARCH become readable on closer inspection. Pixel art logo style, high contrast, kintsugi dark aesthetic.
```
3. Görsele bak — "ft" ve "rch" harfleri aşağı sarkmalı, kırılma noktalarında altın glow olmalı
4. ❌ İstediğin gibi çıkmadıysa: `Regenerate. The letters ft must hang visibly below the I letter, rch below the A letter, with bright gold #FFD700 glow at the exact break points`
5. Beğendiğin görseli **sağ tıkla → Resmi farklı kaydet** → `ART/logo/rima_logo_kaynak.png`
6. Terminali aç:
   ```
   cd "F:\Antigravity Projeler\2d roguelite"
   python tools/logo_resize.py "ART/logo/rima_logo_kaynak.png"
   ```
   → 3 dosya otomatik üretilir:
   - `ART/logo/rima_logo_320x80.png`
   - `ART/logo/rima_logo_160x40.png`
   - `ART/logo/rima_icon_64x64.png`

---

## PLACEHOLDER'LAR

Aseprite gerekmez — Gemini'den direkt indir.

| Dosya | Gemini Prompt |
|-------|---------------|
| `ART/placeholder/warblade_placeholder.png` | `Simple pixel art placeholder sprite, 64x64, dark blue filled square with large gold letter W centered, transparent background` |
| `ART/placeholder/grunt_placeholder.png` | `Simple pixel art placeholder sprite, 32x32, dark red filled square with white letter G centered, transparent background` |
| `ART/placeholder/floor_placeholder.png` | `Simple pixel art placeholder tile, 16x16, dark grey stone texture with single crack line, no background` |
| `ART/placeholder/wall_placeholder.png` | `Simple pixel art placeholder tile, 16x16, near-black stone block, horizontal mortar line, no background` |

Her biri: Gemini'ye prompt at → indir → klasöre kaydet.

---

# FAZ 1

---

## WARBLADE

| | |
|--|--|
| Canvas | 64×64 |
| Klasör | `ART/karakterler/warblade/` |
| Workflow (BASE) | **Workflow A** |
| Workflow (Anim) | **Workflow B** |

### BASE — Workflow A'yı uygula, şu parametrelerle:

**Gemini prompt:**
```
A heavily armored warrior viewed strictly from directly above, bird's eye aerial top-down perspective — as if a camera is mounted on the ceiling looking straight down. The warrior holds a large greatsword in their right hand, blade pointing downward at their side. Their dark iron plate armor has a crack across the chest with cold blue light bleeding through. Short torn cape visible behind the shoulders. Heavy angular pauldrons visible from above. Head small at top, wide shoulders, no face visible — only the top of the helmet. Feet barely visible or hidden. The art style is retro pixel art, limited color palette: dark iron grey, cold blue, worn gold. Transparent background, no background elements.
```

**PixelLab — Description:**
```
Heavily armored dark iron plate warrior, large greatsword in right hand pointing downward, cracked breastplate glowing cold blue, torn cape, heavy pauldrons, facing downward (south), dark fantasy, top-down view
```

**Aseprite Kayıt:** `ART/karakterler/warblade/warblade_S_BASE.aseprite`
**Export:** `ART/karakterler/warblade/warblade_S_BASE.png`

---

### ANİMASYONLAR — Workflow B'yi uygula

BASE sprite açıkken her satır için Workflow B'yi baştan sona uygula:

| Animasyon | Description (B3'te) | Action Description (B3'te) | Frames (B5'te Columns) | Export Yolu |
|-----------|--------------------|-----------------------------|------------------------|-------------|
| idle | `heavily armored dark iron warrior with cracked glowing breastplate and greatsword` | `standing idle, slow breathing, slight weapon sway, cold blue light pulsing from chest crack` | 4 | `ART/karakterler/warblade/warblade_S_idle.png` |
| walk | aynı | `walking south, heavy armored footsteps, armor plates shifting with weight, cape moving` | 6 | `ART/karakterler/warblade/warblade_S_walk.png` |
| attack1 | aynı | `powerful horizontal greatsword slash, windup pulling sword back then strong swing` | 6 | `ART/karakterler/warblade/warblade_S_attack1.png` |
| attack2 | aynı | `raising greatsword overhead with both hands then slamming into the ground, shockwave` | 6 | `ART/karakterler/warblade/warblade_S_attack2.png` |
| dash | aynı | `explosive forward dash, body leaning aggressively forward, blur of motion` | 4 | `ART/karakterler/warblade/warblade_S_dash.png` |
| hurt | aynı | `staggering sharply backward from impact, brief recoil flinch` | 4 | `ART/karakterler/warblade/warblade_S_hurt.png` |
| death | aynı | `slowly collapsing forward, armor cracking apart, energy dissipating` | 8 | `ART/karakterler/warblade/warblade_S_death.png` |

---

## SHARD WALKER

| | |
|--|--|
| Canvas | 32×32 |
| Klasör | `ART/duşmanlar/grunt_shard/` |
| Workflow (BASE) | **Workflow A** (canvas 32×32) |
| Workflow (Anim) | **Workflow B** |

**Gemini prompt:**
```
A humanoid enemy assembled entirely from floating broken stone shards and bone fragments. There is no solid body — only hovering pieces arranged in a vague warrior shape, with cold blue light bleeding through the gaps between the shards. Viewed from directly above. The art style is retro pixel art with a dark stone and cold blue color palette. The background must be transparent.
```

**PixelLab — Description:**
```
Humanoid creature assembled from floating broken stone shards, cold blue light bleeding through the gaps, no solid body, hovering shard warrior shape, facing downward, dark fantasy enemy
```

**Aseprite Kayıt:** `ART/duşmanlar/grunt_shard/grunt_shard_S_BASE.aseprite`
**Export:** `ART/duşmanlar/grunt_shard/grunt_shard_S_BASE.png`

| Animasyon | Description | Action | Columns | Export |
|-----------|-------------|--------|---------|--------|
| idle | `humanoid creature assembled from floating stone shards with cold blue light` | `hovering in place, stone shards slowly rotating, cold blue light pulsing` | 4 | `ART/duşmanlar/grunt_shard/grunt_shard_S_idle.png` |
| walk | aynı | `moving forward, shards separating and rejoining with each step` | 6 | `ART/duşmanlar/grunt_shard/grunt_shard_S_walk.png` |
| attack | aynı | `one arm shard launches forward as projectile then snaps back` | 6 | `ART/duşmanlar/grunt_shard/grunt_shard_S_attack.png` |
| death | aynı | `all shards exploding outward simultaneously, cold blue burst then gone` | 6 | `ART/duşmanlar/grunt_shard/grunt_shard_S_death.png` |

---

## VOID THRALL

| | |
|--|--|
| Canvas | 32×32 |
| Klasör | `ART/duşmanlar/grunt_thrall/` |
| Workflow | **Workflow A** (canvas 32×32) |

### Tam Form

**Gemini prompt:**
```
A dark armored humanoid soldier stands menacingly. Running vertically through the center of their chest is a deep glowing crack, with void purple energy seeping out from the fissure. The overall silhouette is sinister and medieval. Viewed from directly above. The art style is retro pixel art with a dark iron and void purple color palette. The background must be transparent.
```

**PixelLab — Description:**
```
Dark armored soldier with deep glowing vertical crack through chest, void purple energy seeping from the fissure, sinister medieval enemy, facing downward, dark fantasy
```

**Export:** `ART/duşmanlar/grunt_thrall/grunt_thrall_S_BASE.png`

### Sol Yarı (ayrı canvas, ayrı Generate)

**Gemini prompt:**
```
Pixel art sprite, top-down 2D dark fantasy game, transparent background, LEFT HALF ONLY of a dark armored soldier, cleanly split vertically down the center, void purple energy glowing at the split edge #9E4FE0, still aggressive, facing south
```

**PixelLab — Description:**
```
Left half of a dark armored soldier split vertically, void purple energy glowing at split edge, right half absent/transparent, still aggressive, facing downward
```

**Export:** `ART/duşmanlar/grunt_thrall/grunt_thrall_half_left_S_BASE.png`

### Sağ Yarı — yukarıdaki ile AYNI, şunu değiştir:
- Gemini'de `LEFT HALF` → `RIGHT HALF`
- PixelLab'de `Left half` → `Right half` | `right half absent` → `left half absent`
- **Export:** `ART/duşmanlar/grunt_thrall/grunt_thrall_half_right_S_BASE.png`

---

## IRON WARDEN — Act 1 Boss

| | |
|--|--|
| Canvas | 128×128 |
| Klasör | `ART/boss/iron_warden/` |
| Workflow (BASE) | **Workflow D** ⚠️ |
| Workflow (Anim) | **Workflow B** (canvas 128×128) |

**Gemini prompt:**
```
A massive iron golem guardian towers over the battlefield. Its full dark plate armor is heavily damaged, covered in deep cracks from which cold blue energy seeps through. Several broken sword shards are embedded in its back and shoulders like trophies. The figure radiates an overwhelming, slow, unstoppable presence. Viewed from directly above. The art style is retro pixel art with a dark iron and cold blue color palette. The background must be transparent.
```

**PixelLab — Description (Create M-XL image!):**
```
Massive heavily armored iron golem guardian, full dark plate armor with deep cracks, cold blue energy seeping through cracked breastplate, broken swords embedded in shoulders, imposing top-down view, overwhelming dark fantasy boss
```

**Export BASE:** `ART/boss/iron_warden/boss_iron_warden_S_BASE.png`

| Animasyon | Action | Columns | Export |
|-----------|--------|---------|--------|
| idle | `massive iron armored golem, standing menacingly still, very slow heavy breathing, armor plates barely shifting, cold blue glow pulsing` | 6 | `ART/boss/iron_warden/boss_iron_warden_S_idle.png` |
| attack1 | `raising massive sword then devastating overhead slam, ground shockwave on impact` | 8 | `ART/boss/iron_warden/boss_iron_warden_S_attack1.png` |
| charge | `slow windup pulling shield back, explosive forward shield charge, heavy collision` | 8 | `ART/boss/iron_warden/boss_iron_warden_S_charge.png` |
| hurt | `massive body barely flinching from hit, minor stagger, shrugging it off` | 4 | `ART/boss/iron_warden/boss_iron_warden_S_hurt.png` |
| death | `slowly collapsing forward, massive armor cracking open, cold blue energy erupting then dispersing and fading` | 8 | `ART/boss/iron_warden/boss_iron_warden_S_death.png` |

---

## ACT 1 TİLE SETİ — Workflow C

### Floor Tile (3 varyasyon)

**Workflow C adımlarını uygula:**
- Canvas: **16×16**
- Tileset type: **Top-Down**
- **Inner Description:** `Dark grey cracked cobblestone dungeon floor tile, thin cold blue glowing fissures, seamless tileable, top-down dark fantasy`
- **Tile Size:** `16x16`

3 varyasyon: Generate → export `act1_floor_01.png` → Generate → export `act1_floor_02.png` → Generate → export `act1_floor_03.png`

| Export |
|--------|
| `ART/tiles/act1/act1_floor_01.png` |
| `ART/tiles/act1/act1_floor_02.png` |
| `ART/tiles/act1/act1_floor_03.png` |

### Wall Tile (2 varyasyon)

- Canvas: **16×32**
- **Inner Description:** `Crumbling dark fortress stone wall tile, cold blue light barely in cracks, seamless tileable, top-down dark fantasy`

| Export |
|--------|
| `ART/tiles/act1/act1_wall_01.png` |
| `ART/tiles/act1/act1_wall_02.png` |

### Crack Overlay (4 varyasyon)

⚠️ Crack overlay için **Create tileset değil → Create S-M image** kullan (Workflow A adımları, ama Gemini adımı atla):
- Canvas: **16×16**
- **Description:** `Thin crack line pattern only, no fill, 1-2 pixel wide, cold blue glow, dark fantasy floor decoration overlay`
- **Camera View:** `None` | **Direction:** `None`
- **Background Removal:** açık

| Export |
|--------|
| `ART/tiles/act1/act1_crack_01.png` |
| `ART/tiles/act1/act1_crack_02.png` |
| `ART/tiles/act1/act1_crack_03.png` |
| `ART/tiles/act1/act1_crack_04.png` |

---

## VFX

### Hit Spark

- Canvas: **32×32** | Workflow A (BASE) + Workflow B (anim)
- **Description:** `White impact spark burst, pixel art VFX, transparent background`
- **Camera View:** `None` | **Direction:** `None`
- **Action:** `hit impact spark, white burst expanding then fading to nothing`
- **Columns:** `6`
- **Export:** `ART/vfx/vfx_hit_spark.png`

### Sword Trail

- Canvas: **64×32** | Workflow A + Workflow B
- **Description:** `Sword slash arc trail, white to gold fading arc, pixel art VFX, transparent background`
- **Camera View:** `None` | **Direction:** `None`
- **Action:** `sword slash arc trail fading from bright white to gold then invisible`
- **Columns:** `4`
- **Export:** `ART/vfx/vfx_sword_trail.png`

---

## SKILL İKONLARI

- Canvas: **32×32** | Workflow A adımları (Gemini atla, direkt PixelLab)
- **Camera View:** `None` | **Direction:** `None`

| Export | Description |
|--------|-------------|
| `ART/ui/icons/skills/skill_iron_charge.png` | `Pixel art skill icon, armored gauntlet with forward momentum arrow, cold blue impact glow, dark background, centered game UI icon` |
| `ART/ui/icons/skills/skill_death_blow.png` | `Pixel art skill icon, greatsword vertical downstroke, blood red edge glow, dark dramatic, dark background, centered` |
| `ART/ui/icons/skills/skill_bladestorm.png` | `Pixel art skill icon, sword silhouette in circular spinning motion, gold energy trail, dark background, centered` |
| `ART/ui/icons/skills/skill_cleave.png` | `Pixel art skill icon, horizontal sword slash arc with shockwave lines, steel grey and cold blue, centered` |
| `ART/ui/icons/skills/skill_gravity_cleave.png` | `Pixel art skill icon, sword slamming ground with circular pull vortex lines, cold blue, centered` |
| `ART/ui/icons/skills/skill_war_stomp.png` | `Pixel art skill icon, armored boot slamming down with shockwave rings expanding, gold dust, centered` |

---

# FAZ 2

---

## ELEMENTALIST

| | |
|--|--|
| Canvas | 64×64 |
| Klasör | `ART/karakterler/elementalist/` |
| Workflow | A + B |

**Gemini prompt:**
```
A dark-robed mage stands with two arcane tomes floating and orbiting around them. Their left hand emanates cold blue frost crystals, while their right hand crackles with orange-red fire. They wear an elaborate hood and carry a staff topped with a split glowing crystal. Viewed from directly above. The art style is retro pixel art with a limited palette of dark robes, cold blue, and fire orange. The background must be transparent.
```

**PixelLab — Description:** `Dark-robed mage with two floating arcane tomes, left hand frost crystals, right hand fire sparks, elaborate hood, split crystal staff, facing downward`

**Export BASE:** `ART/karakterler/elementalist/elementalist_S_BASE.png`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `standing still, tomes slowly orbiting, elemental energy flickering between hands` | 4 | `ART/karakterler/elementalist/elementalist_S_idle.png` |
| walk | `gliding forward, robes flowing, tomes trailing behind` | 6 | `ART/karakterler/elementalist/elementalist_S_walk.png` |
| attack1 | `casting ice shards forward, frost cone launching from left hand` | 6 | `ART/karakterler/elementalist/elementalist_S_attack1.png` |
| attack2 | `slamming both hands down, fire explosion erupting forward` | 6 | `ART/karakterler/elementalist/elementalist_S_attack2.png` |
| dash | `blinking forward in burst of frost energy` | 4 | `ART/karakterler/elementalist/elementalist_S_dash.png` |
| hurt | `staggering, magical aura flickering` | 4 | `ART/karakterler/elementalist/elementalist_S_hurt.png` |
| death | `collapsing, tomes crashing, energy dispersing` | 8 | `ART/karakterler/elementalist/elementalist_S_death.png` |

---

## SHADOWBLADE

| | |
|--|--|
| Canvas | 64×64 |
| Klasör | `ART/karakterler/shadowblade/` |
| Workflow | A + B |

**Gemini prompt:**
```
A sleek assassin crouches in a tense, ready stance. They wear dark leather armor and a half-face mask, with dual daggers resting at their hips. Their cloak seems to absorb light rather than reflect it. Viewed from directly above. The art style is retro pixel art with a near-black and deep purple color palette, minimal color, maximum silhouette readability. The background must be transparent.
```

**PixelLab — Description:** `Sleek assassin in dark leather armor, dual daggers at hips, half-face mask, cloak absorbing light, crouching aggressive ready stance, facing downward`

**Export BASE:** `ART/karakterler/shadowblade/shadowblade_S_BASE.png`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `crouching still, cloak barely shifting, daggers glinting` | 4 | `ART/karakterler/shadowblade/shadowblade_S_idle.png` |
| walk | `gliding forward silently, nearly weightless steps` | 6 | `ART/karakterler/shadowblade/shadowblade_S_walk.png` |
| attack1 | `quick double dagger slash, blur of motion` | 6 | `ART/karakterler/shadowblade/shadowblade_S_attack1.png` |
| attack2 | `lunge stab forward, retract` | 6 | `ART/karakterler/shadowblade/shadowblade_S_attack2.png` |
| dash | `blink dash, disappear frame 1, reappear frame 3` | 4 | `ART/karakterler/shadowblade/shadowblade_S_dash.png` |
| hurt | `sharp recoil, cloak flaring` | 4 | `ART/karakterler/shadowblade/shadowblade_S_hurt.png` |
| death | `crumpling silently, form dissolving into shadow particles` | 8 | `ART/karakterler/shadowblade/shadowblade_S_death.png` |

---

## RANGER

| | |
|--|--|
| Canvas | 64×64 |
| Klasör | `ART/karakterler/ranger/` |
| Workflow | A + B |

**Gemini prompt:**
```
A lean archer stands in an alert, ready stance with a longbow half-drawn. They wear worn leather armor decorated with bone and earth accents, a quiver on their back, and their hood is pulled back. Viewed from directly above. The art style is retro pixel art with earthy brown, bone white, and leather tan tones. The background must be transparent.
```

**PixelLab — Description:** `Lean archer in worn leather armor, longbow drawn halfway, quiver on back, hood pulled back, bone and earth accents, alert stance, facing downward`

**Export BASE:** `ART/karakterler/ranger/ranger_S_BASE.png`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `standing alert, bow at half-draw, scanning the area` | 4 | `ART/karakterler/ranger/ranger_S_idle.png` |
| walk | `moving carefully, bow ready, light careful footsteps` | 6 | `ART/karakterler/ranger/ranger_S_walk.png` |
| attack1 | `drawing and releasing arrow, snap bow release` | 6 | `ART/karakterler/ranger/ranger_S_attack1.png` |
| attack2 | `rapid triple shot, three quick releases in succession` | 8 | `ART/karakterler/ranger/ranger_S_attack2.png` |
| dash | `rolling sideways, quick dodge roll` | 4 | `ART/karakterler/ranger/ranger_S_dash.png` |
| hurt | `stumbling back, quiver rattling` | 4 | `ART/karakterler/ranger/ranger_S_hurt.png` |
| death | `falling backward, bow dropping from grip` | 8 | `ART/karakterler/ranger/ranger_S_death.png` |

---

## FAZ 2 DÜŞMANLAR

Hepsi **Workflow A** (32×32 canvas). Animasyon varsa **Workflow B**.

### Seam Crawler

**Export BASE:** `ART/duşmanlar/grunt_seam/grunt_seam_S_BASE.png`

**Gemini prompt:** `Pixel art sprite, top-down 2D dark fantasy, transparent background, horror creature inside a floor crack — only two long dark claws and spine ridge visible above the crack, pressed flat, lurking, void purple glow #9E4FE0, bone claws`

**PixelLab — Description:** `Horror creature inside floor crack, only long dark claws and spine ridge above the surface, lurking enemy, void purple glow from crack, facing downward`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `claws slowly scraping the stone surface` | 4 | `ART/duşmanlar/grunt_seam/grunt_seam_S_idle.png` |
| attack | `claws shooting upward from crack, striking, retracting` | 6 | `ART/duşmanlar/grunt_seam/grunt_seam_S_attack.png` |
| death | `claws retracting, crack sealing shut` | 6 | `ART/duşmanlar/grunt_seam/grunt_seam_S_death.png` |

---

### Echo Hound

**Export BASE:** `ART/duşmanlar/grunt_echo/grunt_echo_S_BASE.png`

**Gemini prompt:** `Pixel art sprite, top-down 2D dark fantasy, transparent background, ghostly wolf predator, semi-transparent indigo form #3D1F6B #9E4FE0, white glowing eyes #D4D4F0, no solid flesh — just energy lines and silhouette, predatory low crouching stance, facing south`

**PixelLab — Description:** `Ghostly wolf creature, semi-transparent indigo energy form, white glowing eyes, no solid body just energy lines, predatory crouching stance, facing downward`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `form flickering, hovering in place, eyes glowing` | 4 | `ART/duşmanlar/grunt_echo/grunt_echo_S_idle.png` |
| walk | `bounding forward with strong motion trail` | 6 | `ART/duşmanlar/grunt_echo/grunt_echo_S_walk.png` |
| attack | `blinking forward snapping, blinking back` | 6 | `ART/duşmanlar/grunt_echo/grunt_echo_S_attack.png` |
| death | `form dispersing into indigo particles` | 6 | `ART/duşmanlar/grunt_echo/grunt_echo_S_death.png` |

---

### Hollow Mite

⚠️ **Sprite alanının %30'unu doldursun** — bilinçli küçük

**Export BASE:** `ART/duşmanlar/grunt_mite/grunt_mite_S_BASE.png`

**Gemini prompt:** `Pixel art sprite, 32x32, top-down dark fantasy, transparent background, SMALL insect creature occupying only 30% of canvas, hollow transparent exoskeleton, six legs, tiny glowing core inside, facing south, dark exoskeleton dim purple glow`

**PixelLab — Description:** `Tiny hollow insect creature, transparent exoskeleton, six legs, glowing core inside, small swarm enemy, occupying only 30 percent of canvas, facing downward`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `tiny insect hovering, legs twitching, core pulsing` | 4 | `ART/duşmanlar/grunt_mite/grunt_mite_S_idle.png` |
| walk | `scuttling forward quickly, six legs moving` | 6 | `ART/duşmanlar/grunt_mite/grunt_mite_S_walk.png` |
| death | `exoskeleton cracking open, glowing core fading` | 6 | `ART/duşmanlar/grunt_mite/grunt_mite_S_death.png` |

---

## THE TWICE-BORN ELITE (2 sprite, animasyon yok)

Canvas 64×64, Workflow A.

**Sprite A — Export:** `ART/duşmanlar/elite_twice_born/elite_twice_born_A_S_BASE.png`
**Gemini prompt:** `Pixel art, 64x64, top-down dark fantasy, transparent background, elite warrior with two sword arms — one armored physical arm, one spectral blue #7BA7BC energy arm, cracked iron armor with cold blue glow, one eye glowing #7BA7BC, aggressive battle-ready stance, facing south`
**PixelLab — Description:** `Elite warrior with two sword arms, one armored one spectral blue energy arm, cracked armor with cold blue glow, one glowing eye, aggressive battle stance, facing downward`

**Sprite B — Export:** `ART/duşmanlar/elite_twice_born/elite_twice_born_B_S_BASE.png`
**Gemini prompt:** `Pixel art, 64x64, top-down dark fantasy, transparent background, elite warrior companion with large shield raised defensively, matching cracked iron armor with cold blue glow #7BA7BC, protective guardian stance, facing south`
**PixelLab — Description:** `Elite guardian warrior with large shield raised defensively, cracked iron armor with cold blue cracks, protective stance, facing downward`

---

## FRACTURE-BORN ELITE (4 faz, animasyon yok)

Canvas 64×64, Workflow A, her faz için ayrı ayrı.

| Faz | Export | PixelLab Description |
|-----|--------|---------------------|
| 1 | `ART/duşmanlar/elite_fracture/elite_fracture_phase1.png` | `Single dark clawed hand and forearm reaching up from floor crack, most canvas empty transparent, void purple energy from crack edges, horror emergence` |
| 2 | `ART/duşmanlar/elite_fracture/elite_fracture_phase2.png` | `Dark armored creature half-emerged from floor crack, upper body visible lower body in crack, void purple energy at crack edges, threatening` |
| 3 | `ART/duşmanlar/elite_fracture/elite_fracture_phase3.png` | `Fully emerged dark armored elite standing, cracked armor with void purple energy seeping, powerful stance, floor crack still at feet, facing downward` |
| 4 | `ART/duşmanlar/elite_fracture/elite_fracture_phase4.png` | `Dark armored elite rage form, armor cracked fully open, void purple energy erupting from every crack, glowing purple eyes, fully enraged, facing downward` |

---

## FRACTURED KING — Act 2 Boss

| | |
|--|--|
| Canvas | 128×128 |
| Klasör | `ART/boss/fractured_king/` |
| Workflow | **D** + B |

**Gemini prompt:**
```
A corrupted sorcerer king stands before his fractured throne. He wears a shattered gold crown, his upper body in half-regal dark armor and half-flowing void robes. The crown is split with void purple energy #9E4FE0 bleeding through each crack. One hand rests on a jagged spectral sword, the other channels void energy. Viewed from directly above. Retro pixel art, dark fantasy Act 2 boss. Transparent background.
```

**PixelLab — Description (Create M-XL image!):**
```
Corrupted sorcerer king, shattered gold crown with void purple energy bleeding through cracks, half dark armor half flowing void robes, one hand on spectral sword one channeling void energy, facing downward, Act 2 dark fantasy boss
```

**Export BASE:** `ART/boss/fractured_king/boss_fractured_king_S_BASE.png`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `corrupted king standing imposingly, void robes slowly billowing, shattered crown pulsing purple energy` | 6 | `ART/boss/fractured_king/boss_fractured_king_S_idle.png` |
| attack1 | `raising spectral sword and launching void slash forward, purple energy arc` | 8 | `ART/boss/fractured_king/boss_fractured_king_S_attack1.png` |
| attack2 | `channeling both hands, void energy explosion radiating outward` | 8 | `ART/boss/fractured_king/boss_fractured_king_S_attack2.png` |
| phase2 | `crown shattering further, void energy erupting, transformation surge` | 6 | `ART/boss/fractured_king/boss_fractured_king_S_phase2.png` |
| hurt | `staggering, crown flickering, robes disrupted` | 4 | `ART/boss/fractured_king/boss_fractured_king_S_hurt.png` |
| death | `crown exploding, body dissolving into shards of void light` | 8 | `ART/boss/fractured_king/boss_fractured_king_S_death.png` |

---

## ACT 2 TİLE SETİ — Workflow C

| Export | Canvas | Inner Description |
|--------|--------|-------------------|
| `ART/tiles/act2/act2_floor_01.png` | 16×16 | `Decayed dungeon floor, dark rotting stone with void purple glowing veins, seamless tileable, top-down dark fantasy Act 2` |
| `ART/tiles/act2/act2_floor_02.png` | 16×16 | Aynı — tekrar Generate |
| `ART/tiles/act2/act2_wall_01.png` | 16×32 | `Corrupted dark temple stone wall, purple energy veins, moss decay ancient crumbling architecture, seamless tileable` |

---

# FAZ 3

---

## SPORE HOLLOW ELITE

Canvas 64×64 | Workflow A | Export: `ART/duşmanlar/elite_spore/elite_spore_S_BASE.png`

**Gemini prompt:** `Pixel art sprite, 64x64, top-down dark fantasy, transparent background, bloated hollow creature made of fungal growth, multiple spore vents on back releasing purple toxic clouds #9E4FE0, dangerous swollen form like a living bomb, dark organic brown, toxic purple, sickly yellow`

**PixelLab — Description:** `Bloated fungal hollow creature with spore vents on back releasing toxic purple clouds, dangerous swollen form like a living bomb, facing downward, dark fantasy elite`

---

## RIFT MAW ELITE

Canvas 64×64 | Workflow A | Export: `ART/duşmanlar/elite_rift_maw/elite_rift_maw_S_BASE.png`

**Gemini prompt:** `Pixel art sprite, 64x64, top-down dark fantasy, transparent background, creature that IS a dimensional rift — body is a walking tear in reality, jagged crack shaped like a predator, gold energy #FFD700 at rift edges, void black inside, no solid form, facing south`

**PixelLab — Description:** `Living dimensional rift creature, body is a tear in reality shaped like a predator, gold glowing edges, void black inside, no solid form, facing downward`

---

## CLASS MİMİC

Canvas 64×64 | Workflow A | Export: `ART/duşmanlar/grunt_mimic/grunt_mimic_S_BASE.png`

**Gemini prompt:** `Pixel art sprite, 64x64, top-down dark fantasy, transparent background, shapeshifter enemy mid-transformation between two warrior forms, unstable blurring silhouette, void energy seeping from transformation seams, familiar but wrong appearance, facing south`

**PixelLab — Description:** `Shapeshifter enemy mid-transformation between two warrior forms, unstable blurring silhouette, void energy at transformation seams, familiar but wrong appearance, facing downward`

---

## HOLLOW SOVEREIGN — Act 3 Boss

Canvas 128×128 | **Workflow D** | Export BASE: `ART/boss/hollow_sovereign/boss_hollow_sovereign_S_BASE.png`

**Gemini prompt:** `Pixel art boss sprite, 128x128, top-down dark fantasy, transparent background, The Hollow Sovereign — hollow transparent body only outline visible, floating gold crown #FFD700 above empty head, royal armor outline only, shifting form, gold void black ghost white palette`

**PixelLab — Description (Create M-XL image!):** `Hollow void sovereign ruler, transparent empty body only outline visible, floating gold crown, royal armor outline, shifting adaptive form, dark fantasy Act 3 boss, top-down`

| Anim | Action | Columns | Export |
|------|--------|---------|--------|
| idle | `hollow form slowly shifting and warping, crown floating slightly` | 6 | `ART/boss/hollow_sovereign/boss_hollow_sovereign_S_idle.png` |
| attack1 | `void energy beam projecting from hollow body center, sweeping arc` | 8 | `ART/boss/hollow_sovereign/boss_hollow_sovereign_S_attack1.png` |
| attack2 | `form splitting and rejoining around player position` | 8 | `ART/boss/hollow_sovereign/boss_hollow_sovereign_S_attack2.png` |
| adapt | `body reshaping, form morphing in response to player build` | 6 | `ART/boss/hollow_sovereign/boss_hollow_sovereign_S_adapt.png` |
| death | `form collapsing inward then exploding outward in gold light` | 8 | `ART/boss/hollow_sovereign/boss_hollow_sovereign_S_death.png` |

---

## NEXUS CORE — Final Boss (3 faz)

Canvas 128×128 her faz için | **Workflow D** | Klasör: `ART/boss/nexus_core/`

**Gemini prompt (her faz için aynı base, aşağıdaki faz açıklamasını sona ekle):**
```
Pixel art final boss sprite, 128x128, top-down dark fantasy, transparent background, The Nexus Core — massive crystallized frozen decision, geometric dark crystal lattice structure, gold #FFD700 primary glow, cold blue #7BA7BC, void purple #9E4FE0 radiating from cracks, shape subtly echoes player character form
```

| Faz | Export | Faz açıklaması (promptun sonuna ekle) |
|-----|--------|--------------------------------------|
| BASE | `boss_nexus_core_S_BASE.png` | *(ekstra yok)* |
| 1 | `boss_nexus_phase1.png` | `Fully intact sealed form, gold energy contained, ominous and dormant` |
| 2 | `boss_nexus_phase2.png` | `Beginning to crack open, humanoid silhouette faintly appearing inside, gold energy leaking` |
| 3 | `boss_nexus_phase3.png` | `Half-open, warrior class form clearly mirrored inside, gold and void colors erupting simultaneously` |

**PixelLab — Description:** `Massive dark crystal lattice structure, gold energy radiating from every crack, frozen geometric form, all void colors present, dark fantasy final boss, top-down`

---

## ACT 3 TİLE SETİ — Workflow C

| Export | Canvas | Inner Description |
|--------|--------|-------------------|
| `ART/tiles/act3/act3_floor_01.png` | 16×16 | `Act 3 void floor, absolute darkness with deep gold glowing cracks, kintsugi aesthetic, seamless tileable` |
| `ART/tiles/act3/act3_wall_01.png` | 16×32 | `Act 3 void wall, pure void material with gold structural energy lines, impossible geometry, seamless tileable` |

---

# FAZ 4 — Hub + NPC'ler

---

## HUB TİLE SETİ — Workflow C

| Export | Canvas | Inner Description |
|--------|--------|-------------------|
| `ART/tiles/hub/hub_floor_01.png` | 16×16 | `Hub floor, ancient stone worn smooth, warm candlelight amber glow in cracks #D4956A, melancholic between-worlds atmosphere, seamless tileable` |
| `ART/tiles/hub/hub_floor_02.png` | 16×16 | Aynı — tekrar Generate |
| `ART/tiles/hub/hub_wall_01.png` | 16×32 | `Hub wall, ancient stone with worn barely-visible relief carvings, warm amber cracks #D4956A, timeless heavy between-worlds architecture, seamless tileable` |

---

## HUB NPC'LER (4 karakter, animasyon yok)

Hepsi: Canvas 64×64 | **Workflow A** | Klasör: `ART/npc/`

### The Ferryman
**Export:** `ART/npc/npc_ferryman_S_BASE.png`
**Gemini:** `Pixel art NPC, 64x64, top-down dark fantasy, transparent background, ancient figure in floor-length dark robes, face completely in shadow under deep hood, unnaturally long fingers, perfectly still posture as if eternal, no color accents, palette void black only, facing south`
**PixelLab — Description:** `Ancient hooded figure in floor-length dark robes, face completely in shadow, unnaturally long fingers, perfectly still eternal posture, no color accents, facing downward`

### Vrel
**Export:** `ART/npc/npc_vrel_S_BASE.png`
**Gemini:** `Pixel art NPC, 64x64, top-down dark fantasy, transparent background, semi-transparent ghost blacksmith, one solid armored arm holding a spectral hammer, rest of body is drifting smoke, dark green-grey ghost form, hammer faintly glowing orange, obsessively focused posture, facing south`
**PixelLab — Description:** `Ghost blacksmith, one solid armored arm with spectral hammer, rest of body drifting smoke, dark green-grey ghost form, focused posture, facing downward`

### Sister Mourne
**Export:** `ART/npc/npc_mourne_S_BASE.png`
**Gemini:** `Pixel art NPC, 64x64, top-down dark fantasy, transparent background, pale grey-robed mourning figure, nun-like with dark ash markings on face and around eyes, slight bow of the head, warm amber trim #D4956A on grey robe, compassionate but heavy presence, facing south`
**PixelLab — Description:** `Pale grey-robed mourning sister, ash markings on face, slight head bow, warm amber robe trim, compassionate heavy presence, facing downward`

### The Cartographer
**Export:** `ART/npc/npc_cartographer_S_BASE.png`
**Gemini:** `Pixel art NPC, 64x64, top-down dark fantasy, transparent background, obsessive map-maker, clothes covered in small paper scraps and maps pinned everywhere, ink-stained hands, magnifying glass, leaning forward studying, warm amber ink #D4956A, slightly chaotic academic, facing south`
**PixelLab — Description:** `Obsessive cartographer with maps pinned all over clothes, ink-stained hands, magnifying glass, leaning forward intently, facing downward`

---

# SORUN GİDERME

| Sorun | Ne yapacaksın |
|-------|---------------|
| Edit menüsünde PixelLab yok | Edit → Preferences → Extensions → eklenti kurulu mu? Aseprite'ı kapat-aç |
| Sprite 3/4 açıda çıktı | Gemini referansını yenile — "strictly from directly above, bird's eye view" ekle |
| Arkaplan kaldırılmadı | Background Removal toggle'ı açık mı kontrol et. Değilse: araç listesinden "Remove background"ı ayrıca çalıştır |
| 128px için Create S-M çalışmıyor | ⚠️ 128px için "Create M-XL image" kullanmak zorundasın |
| Animasyon frame'leri karakter değiştirdi | Reference Image'a BASE sprite'ı set et, Init Image Strength'i 600'e çek |
| Tile seamless değil | View → Tiled Mode ile kontrol et. Generate tekrar bas (farklı seed) |
| Create tileset Top-Down seçeneği yok | Abonelik gerektiriyor — alternatif: Create S-M image ile manuel tile üret |
| PixelLab kredisi bitti | pixellab.ai → hesap sayfası → plan kontrol et |
