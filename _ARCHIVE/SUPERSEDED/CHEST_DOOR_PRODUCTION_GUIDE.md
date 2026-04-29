# RIMA — Chest + Gate Üretim Rehberi
> **Bütçe:** ~35 gen kaldı (reset: 18 Nisan → 5000 gen)
> **Perspektif:** True isometric — `high top-down` + `isometric ON`
> **Style referans:** `AI_Concept_References/rima_style_anchor.png`

---

## STANDARt ÜRETİM WORKFLOW (her asset için)

```
ADIM 1 — KEŞİF (20 gen, Create Image PRO)
  pixellab.ai → Create Image PRO
  Size: 32px → 64 variation  (VEYA 64px → 16 variation)
  Style image: character_menu_concept.png
  → En iyi silüet/tasarımı seç

ADIM 2 — FİNAL ÜRETİM
  Seçilen variation → init image olarak kaydet
  pixellab.ai → Edit Image PRO → 128px
  Style image: character_menu_concept.png
  → Final kalite çıktı
```

> **Neden 32px keşif?** 64 variation = tek çalıştırmada geniş tasarım uzayı görürsün.
> Silüet ve kompozisyon küçükte net belli olur, detayı Edit PRO'da kazanırsın.

---

## Araçlar

| Araç | Nerede | Maliyet | Ne için |
|------|--------|---------|---------|
| **Create Image PRO** | pixellab.ai site | 20 gen → 16-64 var | Keşif — 32px veya 64px |
| **Edit Image PRO** | pixellab.ai site | 20 gen → 1 çıktı | Final üretim — 128px |
| **Inpaint** | pixellab.ai site | ~1-2 gen | Var olan görselin bölümünü değiştir |
| **Interpolate Pro** | pixellab.ai site | ~2-5 gen/batch | 128×128 animasyon |
| **Create M-XL** | Aseprite extension | ~1-2 gen | Hızlı prototip — isometric ON |
| ~~Interpolate (extension)~~ | Aseprite extension | — | ❌ SADECE 64×64 |

---

## Gate Sistemi — Tasarım Kararı

**Gate = taş kemer eşiği** (kapı değil — içi boş portal çerçevesi).
Hades tarzı: L/C/R pozisyonlarda, oyuncuya dönük, aynı sprite.

| Durum | Görsel | Tetikleyici |
|-------|--------|-------------|
| **Kilitli** | Karanlık iç, demir zincir + kilit | Oda aktifken |
| **Açılıyor** | Zincir kırılma animasyonu | Oda bitti + ödül alındı |
| **Açık** | İçinde oda tipi görüntüsü | Animasyon bitti |

**Renk dili:**
- Kemer: koyu taş + cold blue rift çizgileri + torch amber yansıması
- Kilitli iç: demir gri zincir, siyah void
- Açık iç: her oda tipinin renk imzası

| Oda tipi | Renk imzası |
|----------|-------------|
| Combat | Nötr taş, kan kırmızısı, kılıç silüeti |
| Elite | Amber altın aura, iskelet silüeti, taç |
| Boss | Void mor + rift patlaması, kocaman karanlık |
| Chest | Sıcak altın parıltı, sandık silüeti |
| Merchant | Sıcak sarı mum ışığı, şişe/raf silüeti |
| Forge | Turuncu/kırmızı ateş, örs silüeti, kıvılcım |
| Event | Cold blue rift + soru işareti |
| Curse | Koyu kızıl, çatlak zemin, kafatası |

---

## GEN BÜTÇE TAKİP

### Faz A — 18 Nisan öncesi (~35 gen)

> ADIM 1 ve 5 (Create Image PRO keşif+final = 40 gen) 18 Nisan sonrasına taşındı.
> Kalan 35 gen ile sadece Inpaint varyantları yapılabilir — ama base sprite olmadan Inpaint yapılamaz.
> **Öneri: 35 geni harca + 18 Nisan'da chest/gate'i sıfırdan başlat.**

| # | Asset | Araç | Tahmini | Harcanan | Durum |
|---|-------|------|---------|----------|-------|
| — | — | 35 gen 18 Nisana sakla | — | — | ⬜ |

### Faz B — 18 Nisan sonrası (5000 gen)

| # | Asset | Araç | Gen | Durum |
|---|-------|------|-----|-------|
| 1A | chest_discovery.png | Create Image PRO 32px | 20 | ⬜ |
| 1B | chest_closed.png | Edit Image PRO 128px | 20 | ⬜ |
| 2 | chest_open_cursed.png | Inpaint | ~2 | ⬜ |
| 3 | chest_damaged.png | Edit Image PRO | 20 | ⬜ |
| 4 | chest_open_sheet.png (8f) | Interpolate Pro site | ~8 | ⬜ |
| 5A | gate_discovery.png | Create Image PRO 32px | 20 | ⬜ |
| 5B | gate_base.png | Edit Image PRO 128px | 20 | ⬜ |
| 6 | gate_locked.png | Inpaint | ~2 | ⬜ |
| 7 | gate_unlocked_base.png | Inpaint | ~2 | ⬜ |
| 8 | gate_unlock_sheet.png (8f) | Interpolate Pro site | ~8 | ⬜ |
| 9 | gate_combat.png | Inpaint | ~2 | ⬜ |
| 10 | gate_boss.png | Inpaint | ~2 | ⬜ |
| 11-16 | gate_[tipler].png | Inpaint ×6 | ~12 | ⬜ |
| | **Faz B Toplam** | | **~138 gen** | |

> Her adım bitince "Harcanan" sütununu doldur, Durum ✅ yap.

### Faz B — 18 Nisan sonrası (5000 gen'den)

| # | Asset | Araç | Tahmini | Durum |
|---|-------|------|---------|-------|
| 11 | gate_chest.png | Inpaint | ~2 | ⬜ |
| 12 | gate_curse.png | Inpaint | ~2 | ⬜ |
| 13 | gate_elite.png | Inpaint | ~2 | ⬜ |
| 14 | gate_merchant.png | Inpaint | ~2 | ⬜ |
| 15 | gate_forge.png | Inpaint | ~2 | ⬜ |
| 16 | gate_event.png | Inpaint | ~2 | ⬜ |
| | **Faz B Toplam** | | **~12** | |

---

## HEDEF DOSYA YAPISI (hepsi üretilecek — hiçbiri henüz yok)

```
Assets/Sprites/Environment/
  Chest/
    chest_closed.png           ⬜ ADIM 1
    chest_open_cursed.png      ⬜ ADIM 2
    chest_damaged.png          ⬜ ADIM 3
    chest_open_sheet.png       ⬜ ADIM 4 — 1×8 spritesheet
  Gate/
    gate_base.png              ⬜ ADIM 5 — tüm gate inpaintlerinin base'i
    gate_locked.png            ⬜ ADIM 6
    gate_unlocked_base.png     ⬜ ADIM 7 — animasyon son frame
    gate_unlock_sheet.png      ⬜ ADIM 8 — 1×8 spritesheet
    gate_combat.png            ⬜ ADIM 9
    gate_boss.png              ⬜ ADIM 10
    gate_chest.png             ⬜ FAZ B
    gate_curse.png             ⬜ FAZ B
    gate_elite.png             ⬜ FAZ B
    gate_merchant.png          ⬜ FAZ B
    gate_forge.png             ⬜ FAZ B
    gate_event.png             ⬜ FAZ B
```

---

## SABIT PARAMETRELER (her üretimde aynı)

```
View:        high top-down
Isometric:   ON
ai_freedom:  400  (tutarlılık için düşük)
Remove background: ON
```

> ⚠️ **Style image boyut uyarısı:** PixelLab "Style image" alanı output size'ı yüklenen görselin boyutuna kilitler.
> Büyük konsept görsel yüklersen → büyük çıktı, fazla gen.
> **Kural:** Style image kullanacaksan hedef boyuta (128px) küçültülmüş versiyon yükle.
> **Pratik:** Env. objeler için style_image BOŞALT — init_image + prompt yeterli.

---

## ADIM 1 — chest_closed.png

**Gen tahmini:** 20 (keşif) + 20 (final) = **40 gen — 18 Nisan sonrası**

### 1A — Keşif (Create Image PRO, 32px → 64 variation)

`pixellab.ai → Create Image PRO`

| Alan | Değer |
|------|-------|
| Size | **32×32** |
| Camera view | `high top-down` |
| Isometric | ✅ ON |
| Direction | `south` |
| Outline | `single color black outline` |
| Shading | `detailed shading` |
| Style image | **BOŞALT** — style image output size'ı kilitler |
| Remove background | ✅ ON |

**PROMPT:**
```
dark fantasy iron treasure chest, fully closed,
heavy iron chains crossing diagonally over the lid,
large iron padlock at front center, skull and bone carvings on lid,
cold blue rift energy cracks running through the iron surface,
isometric perspective, hades game art style,
dark iron grey and tarnished cold blue palette,
pixel art, transparent background
```

→ 64 variation içinden **en güçlü silüet + zincir kompozisyonunu** seç.
→ `chest_discovery.png` kaydet.

### 1B — Final (Edit Image PRO, 128px)

`pixellab.ai → Edit Image PRO`

| Alan | Değer |
|------|-------|
| Image to edit | `chest_discovery.png` (1A seçimi) |
| Output size | **128×128** |
| Isometric | ✅ ON |
| Style image | **BOŞALT** — style image output size'ı kilitler |
| Remove background | ✅ ON |

**PROMPT:**
```
same dark fantasy iron treasure chest at full 128px detail,
heavy iron chains crossing diagonally, large iron padlock at front,
skull and bone relief carvings on lid and body,
cold blue rift energy cracks glowing through weathered iron surface,
worn leather straps, battle-aged metal texture,
isometric perspective, hades game art style,
dark iron grey and cold blue palette, pixel art, transparent background
```

### Üretim sonrası
- `Assets/Sprites/Environment/Chest/chest_closed.png` kaydet
- Beğenmezsen 1B'yi tekrar çalıştır (farklı seed, init image aynı)
- `Assets/Sprites/Environment/Chest/chest_closed.png` kaydet
- Bütçe tablosunu güncelle (Harcanan + ✅)

---

## ADIM 2 — chest_open_cursed.png

**Gen tahmini:** ~3  
**Araç:** pixellab.ai sitesi → **Inpaint**  
> ADIM 1 tamamlanmadan başlama. Bu adım chest_closed üzerine uygulanır.

### Nereye gidilir
`pixellab.ai` → sol menü → **Inpaint**

### Parametreler

| Alan | Değer |
|------|-------|
| Image | `chest_closed.png` yükle |
| Mask | **Tarayıcı içinde** — sadece kapak + iç alanı maskele (zincirlere dokunma) |
| Style Image | `RIMA_DarkFantasy_Concept.png` |
| ai_freedom | 400 |
| Remove background | ON |

### Promptlar

**KISA:**
```
open chest lid revealing glowing rift energy inside, cursed purple glow
```

**DETAYLI:**
```
Open the treasure chest lid, revealing glowing rift dimensional energy inside, 
purple-violet crackling light emanating from within, cursed dark energy swirling, 
ancient evil awakened, eerie glow casting light on the interior edges, 
same dark iron chest exterior unchanged, pixel art style
```

### Üretim sonrası
- 2-3 varyasyon dene, en dramatik iç ışıklamalı olanı seç
- `Assets/Sprites/Environment/Chest/chest_open_cursed.png` kaydet

---

## ADIM 3 — chest_damaged.png

**Gen tahmini:** ~2  
**Araç:** pixellab.ai sitesi → **Edit Image PRO**  
> chest_closed'ın yağmalanmış/kırık versiyonu. Aynı sandık, farklı durum.

### Nereye gidilir
`pixellab.ai` → sol menü → **Edit Image PRO**

### Parametreler

| Alan | Değer |
|------|-------|
| Image to edit | `chest_closed.png` yükle |
| Method | Edit with text |
| ai_freedom | 400 |
| Remove background | ON |

### Promptlar

**KISA:**
```
same chest but looted, broken lid, empty inside, chains broken
```

**DETAYLI:**
```
Same dark iron treasure chest but looted and abandoned, lid broken and hanging open 
at an angle, chains broken and fallen to the sides, shattered wood panels visible, 
empty dark interior, battle-worn and ransacked, same art style and perspective, 
no glow inside, cold and empty feel
```

### Üretim sonrası
- `Assets/Sprites/Environment/Chest/chest_damaged.png` kaydet

---

## ADIM 4 — chest_open_sheet.png (Açılış Animasyonu)

**Gen tahmini:** ~8  
**Araç:** pixellab.ai site → **Interpolate Pro**  
**Çıktı:** 8 frame spritesheet (1024×128 — 8 panel yan yana)  
> ADIM 1 ve ADIM 2 tamamlanmadan başlama.

> ⚠️ **Neden site?** Aseprite extension'daki temel Interpolate SADECE 64×64. 128×128 chest için **site Interpolate Pro** kullanmak zorunlu.

### Nereye gidilir
`pixellab.ai` → sol menü → **Interpolate Pro**

| Alan | Değer |
|------|-------|
| **From image** | `chest_closed.png` — başlangıç frame |
| **To image** | `chest_open_cursed.png` — bitiş frame |
| Number of frames | **6** (araya 6 = toplam 8 frame) |
| View | `low top-down` |
| Direction | `south` |

**Action description:**
```
treasure chest slowly opening, lid rising, rift energy emerging from inside, 
chains loosening and falling, cursed glow intensifying
```

> Enhance Prompt basma — bu description yeterince detaylı.

### Aseprite birleştirme
1. Yeni Aseprite dosyası: 128×128, **8 frame**
2. Frame 1: `chest_closed.png` kopyala-yapıştır
3. Frame 2-7: Interpolate çıktısı 6 frame sırayla kopyala
4. Frame 8: `chest_open_cursed.png` kopyala-yapıştır
5. Tüm frame delay: **80ms**
6. `File → Export Sprite Sheet`:
   - Layout: **Horizontal Strip** (1 row)
   - Output: `chest_open_sheet.png`
7. `Assets/Sprites/Environment/Chest/chest_open_sheet.png` kaydet

**Unity import notu (Kiro yapacak):**
- Sprite Mode: Multiple
- Slice: Grid By Cell Size → 128×128
- PPU: 64
- Filter Mode: Point (no filter)

---

## ADIM 5 — gate_base.png

**Gen tahmini:** 20 (keşif) + 20 (final) = **40 gen — 18 Nisan sonrası**

> Gate_base tüm gate varyantlarının temeli. **Bu dosyayı silme.**

### 5A — Keşif (Create Image PRO, 32px → 64 variation)

`pixellab.ai → Create Image PRO`

| Alan | Değer |
|------|-------|
| Size | **32×32** |
| Camera view | `high top-down` |
| Isometric | ✅ ON |
| Direction | `south` |
| Outline | `single color black outline` |
| Shading | `detailed shading` |
| Style image | **BOŞALT** — style image output size'ı kilitler |
| Remove background | ✅ ON |

**Prompt:**
```
dark fantasy dungeon stone arch gateway, thick carved dark stone blocks,
skull relief carvings, cold blue rift energy cracks, empty black interior,
isometric perspective, hades game art style, dark stone palette,
pixel art, transparent background
```

→ En güçlü silüeti seç → `gate_discovery.png` kaydet.

### 5B — Final (Edit Image PRO, 128px)

`pixellab.ai → Edit Image PRO`

| Alan | Değer |
|------|-------|
| Image to edit | `gate_discovery.png` |
| Output size | **128×128** |
| Isometric | ✅ ON |
| Style image | **BOŞALT** — style image output size'ı kilitler |
| Remove background | ✅ ON |

**Prompt:**
```
same dark fantasy stone arch gateway at full detail, thick stone blocks,
skull and bone relief carvings, cold blue rift cracks, empty void interior,
isometric perspective, hades game art style, dark stone and cold blue palette,
pixel art, transparent background
```

### Prompt
```
dark fantasy dungeon stone arch gateway set into stone wall,
visible front face of arch frame and stone thickness, Hero Siege ARPG angle,
thick carved dark stone blocks, skull and bone relief carvings,
cold blue rift energy cracks through stone, faint torch amber glow,
interior of arch is pure black void — completely empty, no door no chains,
dark fantasy pixel art, transparent background, dark stone and cold blue palette
```

### Üretim sonrası
- **Kontrol:** İç kısım tamamen boş/siyah mı? Taş çerçeve net mi?
- Beğenmezsen tekrar üret (1 gen)
- `Assets/Sprites/Environment/Gate/gate_base.png` kaydet

---

## ADIM 6 — gate_locked.png

**Gen tahmini:** ~2  
**Araç:** pixellab.ai sitesi → **Inpaint**  
> gate_base iç kısmına zincir + kilit ekler. Oda aktifken gösterilen sprite.

### Nereye gidilir
`pixellab.ai` → sol menü → **Inpaint**

### Parametreler

| Alan | Değer |
|------|-------|
| Image | `gate_base.png` yükle |
| Mask | Tarayıcı içinde — **sadece kemer açıklığının içini** maskele (taş çerçeveye dokunma) |
| Style Image | `RIMA_DarkFantasy_Concept.png` |
| ai_freedom | 400 |
| Remove background | ON |

### Promptlar

**KISA:**
```
heavy iron chains crossing the arch opening, large padlock at center, dark void behind
```

**DETAYLI:**
```
Fill the arch opening interior with heavy iron chains crossing diagonally, 
a large iron padlock at the center intersection point, 
rusty dark metal chains with thick links, 
deep black void and dark fog behind the chains, 
no light passes through, completely sealed and foreboding,
chains match the dark iron color palette of the stone arch,
pixel art style, consistent with the gate frame
```

### Üretim sonrası
- `Assets/Sprites/Environment/Gate/gate_locked.png` kaydet
- Bu adımda kullandığın maskeyi not al (aynı maske ADIM 7, 9, 10'da tekrar kullanılacak)

---

## ADIM 7 — gate_unlocked_base.png

**Gen tahmini:** ~2  
**Araç:** pixellab.ai sitesi → **Inpaint**  
> gate_base iç kısmına zincir kırılmış + cold blue rift. Animasyon son frame'i ve oda tipi spriteların base'i.

### Nereye gidilir
`pixellab.ai` → sol menü → **Inpaint**

### Parametreler

| Alan | Değer |
|------|-------|
| Image | `gate_base.png` yükle |
| Mask | ADIM 6 ile aynı — kemer açıklığının içini maskele |
| Style Image | `RIMA_DarkFantasy_Concept.png` |
| ai_freedom | 400 |
| Remove background | ON |

### Promptlar

**KISA:**
```
chains shattered and fallen, open arch glowing with cold blue rift energy inside
```

**DETAYLI:**
```
The arch opening interior now open and free — chains shattered, broken pieces 
fallen to the bottom edge of the frame, 
interior filled with swirling cold blue rift dimensional energy, 
crackling void light emanating from within, deep blue-white glow, 
mysterious and inviting yet dangerous, 
slight depth haze suggesting passage into another realm,
pixel art style, consistent with gate stone frame
```

### Üretim sonrası
- `Assets/Sprites/Environment/Gate/gate_unlocked_base.png` kaydet

---

## ADIM 8 — gate_unlock_sheet.png (Kilit Açılış Animasyonu)

**Gen tahmini:** ~8  
**Araç:** pixellab.ai site → **Interpolate Pro**  
**Çıktı:** 8 frame spritesheet (1024×128)  
> ADIM 6 ve ADIM 7 tamamlanmadan başlama.

> ⚠️ **Neden site?** Aseprite extension Interpolate SADECE 64×64. 128×128 gate için site Interpolate Pro zorunlu.

### Nereye gidilir
`pixellab.ai` → sol menü → **Interpolate Pro**

| Alan | Değer |
|------|-------|
| **From image** | `gate_locked.png` |
| **To image** | `gate_unlocked_base.png` |
| Number of frames | **6** (araya 6 = toplam 8 frame) |
| View | `low top-down` |
| Direction | `south` |

**Action description:**
```
iron chains shattering and falling away from the stone arch, 
cold blue rift energy bursting through the opening as chains break,
dramatic unlock reveal with rift light explosion
```

> Enhance Prompt basma — bu description yeterince spesifik.

### Aseprite birleştirme
1. Yeni Aseprite dosyası: 128×128, **8 frame**
2. Frame 1: `gate_locked.png`
3. Frame 2-7: Interpolate çıktısı 6 frame (sırayla)
4. Frame 8: `gate_unlocked_base.png`
5. Tüm frame delay: **60ms** (hızlı ve dramatik)
6. `File → Export Sprite Sheet`:
   - Layout: **Horizontal Strip** (1 row)
   - Output: `gate_unlock_sheet.png`
7. `Assets/Sprites/Environment/Gate/gate_unlock_sheet.png` kaydet

**Unity import notu (Kiro yapacak):**
- Sprite Mode: Multiple
- Slice: Grid By Cell Size → 128×128
- PPU: 64
- Filter Mode: Point (no filter)

---

## ADIM 9 — gate_combat.png

**Gen tahmini:** ~2  
**Araç:** pixellab.ai sitesi → **Inpaint**  
**Renk imzası:** Nötr taş, kan kırmızısı, kılıç çapraz silüeti

### Nereye gidilir
`pixellab.ai` → sol menü → **Inpaint**

### Parametreler

| Alan | Değer |
|------|-------|
| Image | `gate_unlocked_base.png` yükle |
| Mask | Kemer açıklığının içini maskele (ADIM 6 ile aynı alan) |
| Style Image | `RIMA_DarkFantasy_Concept.png` |
| ai_freedom | 400 |
| Remove background | ON |

### Promptlar

**KISA:**
```
combat room beyond arch: crossed swords silhouette, stone floor, blood stains, dim torch light
```

**DETAYLI:**
```
Through the arch opening: a dark dungeon combat room visible in the distance, 
two crossed swords silhouette in the center foreground, 
dark stone floor with old blood stains, 
dim amber torch light on the walls, 
ominous and dangerous atmosphere, 
slight depth perspective showing the room receding, 
cold blue rift energy still framing the arch edges, 
muted dark color palette — stone grey, dried blood dark red, torch amber,
pixel art style, consistent with gate frame
```

### Üretim sonrası
- `Assets/Sprites/Environment/Gate/gate_combat.png` kaydet

---

## ADIM 10 — gate_boss.png

**Gen tahmini:** ~2  
**Araç:** pixellab.ai sitesi → **Inpaint**  
**Renk imzası:** Void mor + rift patlaması, kocaman karanlık siluet, yoğun glow

### Parametreler

| Alan | Değer |
|------|-------|
| Image | `gate_unlocked_base.png` yükle |
| Mask | Kemer açıklığının içini maskele |
| Style Image | `RIMA_DarkFantasy_Concept.png` |
| ai_freedom | 400 |
| Remove background | ON |

### Promptlar

**KISA:**
```
boss room beyond arch: massive dark silhouette, void purple rift energy, overwhelming presence
```

**DETAYLI:**
```
Through the arch opening: a massive boss chamber visible beyond, 
enormous dark silhouette of a boss creature looming in the center background, 
violent void purple and deep violet rift energy crackling around the figure, 
ground cracked with rift lines glowing purple, 
overwhelming and terrifying atmosphere, 
much brighter and more chaotic than other room types, 
the purple rift contrasts sharply with the cold blue gate frame edges,
pixel art style, dramatic lighting, consistent with gate frame
```

### Üretim sonrası
- `Assets/Sprites/Environment/Gate/gate_boss.png` kaydet

---

## ADIM 11-16 — Faz B (18 Nisan Sonrası)

> Aşağıdaki 6 oda tipi 18 Nisan reset sonrası üretilecek (5000 gen bütçesinden).
> Her biri ADIM 9-10 ile aynı workflow: `gate_unlocked_base.png` + kemer içi mask + Inpaint.

| Adım | Asset | Renk imzası | Kısa prompt |
|------|-------|-------------|-------------|
| 11 | gate_chest.png | Sıcak altın, sandık parıltısı | `treasure chest glowing gold, warm candlelight, rich interior` |
| 12 | gate_curse.png | Koyu kızıl, kafatası, çatlak zemin | `cursed altar, dark crimson cracked floor, skull, malevolent red aura` |
| 13 | gate_elite.png | Amber altın, taçlı iskelet silüeti | `elite enemy chamber, golden aura, skeletal champion with crown silhouette` |
| 14 | gate_merchant.png | Sıcak sarı mum, şişeler/raflar | `merchant room, warm candlelight, potion bottles and item shelves silhouette` |
| 15 | gate_forge.png | Turuncu/kırmızı ateş, örs, kıvılcım | `forge room, orange forge fire, anvil silhouette, sparks flying` |
| 16 | gate_event.png | Cold blue rift, soru işareti | `mysterious event room, cold blue rift energy, large question mark glyph` |

---

## SORUN GİDERME

| Sorun | Çözüm |
|-------|-------|
| Sandık bozuk orantı / bodur | Prompt'a "heroic proportions, tall chest, full height" ekle |
| Kemer içi dolmadı (siyah kaldı) | Mask'ı biraz daha geniş çiz, çerçeve kenarlarını da hafif dahil et |
| Animasyon drift (obje kayıyor) | ai_freedom'u 350'ye düşür |
| Interpolate kötü çıktı | Number of frames'i 6'dan 4'e düşür — daha stabil geçiş |
| Gate renk tutarsızlığı | Tüm adımlarda Style Image'ın aynı dosya olduğunu kontrol et |
| Enhanced with AI RIMA detaylarını atladı | DETAYLI prompt'tan eksik kısımları manuel ekle |

---

## SONRASI — Kiro'ya Devredilecek

### Faz A sonrası (spritelar hazır olunca):

```
IMPORT SETTINGS (tüm Gate/ ve Chest/ klasörü):
- Sprite Mode: Single (spritesheetler hariç)
- PPU: 64 | Filter Mode: Point (no filter) | Compression: None

SPRITESHEETLER — Multiple mode + slice:
- chest_open_sheet.png → Grid By Cell Size 128×128
- gate_unlock_sheet.png → Grid By Cell Size 128×128

PREFABLAR:
- Gate_Prefab.prefab → GateBehavior + Animator + SpriteRenderer
  → L/C/R için aynı prefab, RuntimeRoomManager pozisyon atar
- Chest_Prefab.prefab → ChestBehavior + Animator + SpriteRenderer
```

### Faz B sonrası (18 Nisan sonrası):
```
- Gate_Prefab'a kalan 6 sprite slotunu doldur (gate_chest → gate_event)
- GateBehavior.cs'de tüm RoomType case'lerini aktif et
```

---

## NOTLAR

- Her gen harcamasından sonra bütçe tablosunu güncelle
- ADIM 2 (inpaint) en deneysel adım — bütçe aşarsa ADIM 4 buffer'dan karşıla
- Interpolate üretimi ~5-10 dakika sürebilir, bekleme normal
- Kötü interpolasyon çıkarsa: Number of frames'i 4'e düşür → daha stabil geçiş
- ADIM 5 (gate_base) çok önemli — bu dosya tüm gate varyantlarının temel referansı. Kaliteli gelene kadar tekrar üret (~3 gen bütçe yeterli)
