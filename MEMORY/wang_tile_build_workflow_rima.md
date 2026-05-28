---
name: wang-tile-build-workflow-rima
description: "RIMA adapte 2026-05-26 — Studio S15 universal Wang tile build workflow'unun RIMA-spesifik uygulaması. PixelLab Create Image S-XL Pro KULLAN, Create Tiles Pro YASAK. Yöntem A (tek tile + assembly) küçük scope; Yöntem B (büyük composition + grid böl + komşuluk + inpaint) biome/duvar/dekor scope. Aseprite CLI palette remap %100 otomatize. Karar #131 (16-key Wang) + Karar #143 (6-layer) + STUDIO_KARAR_005 (QA Gate v2) korunur."
metadata:
  type: project
  scope: rima
  source: studio-universal-s15
  status: ACTIVE
---

# Wang Tile Build Workflow — RIMA Adapted (2026-05-26)

**Studio kaynak (üst-otorite):** `F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow.md` (S15 LOCK)
**Studio research arka plan:** `F:/LaurethStudio/05_RESEARCH/2026_05_26_pixellab_wang_tile_workflow.md`
**RIMA adapte tarihi:** 2026-05-26
**Felsefe:** "Studio pattern universal; RIMA bunun action-roguelite uygulaması. Studio'da değişiklik olursa bu dosya güncellenir."

---

## I. Karar — RIMA için bu workflow'un yeri

| Konu | Studio universal | RIMA-spesifik |
|---|---|---|
| Game türü | Çoklu (cozy farm dahil) | Action-roguelite (dungeon/arena, hızlı combat) |
| Biome blend kritikliği | YÜKSEK (cozy farm grass-dirt transitions) | ORTA (dungeon floor + duvar overlay, daha az organik) |
| Wang formatı | 16-Wang veya 47-Wang gerekebilir | **16-Wang yeterli** (Karar #131 ile uyumlu) |
| Palette accent | Brand-specific (yeşil farm, sarı altın) | **Cyan #00FFCC** (Karar #98) + violet rune + warm brazier |
| Tile scope | Floor + biome + biome edges + props | **Dungeon floor + duvar overlay + portal/lattice mekanik** |
| Asset target | `F:/LaurethStudio/02_GAMES/.../Assets/_Studio/Art/Tiles/` | `Assets/Sprites/Environment/` + `_RIMA/Art/Tiles/` (yeni) |

**RIMA'nın bu workflow'a katılım gerekçesi:**
- Karar #131 (16-key Wang lookup) zaten LIVE → bu workflow Karar #131'in **somut PixelLab uygulaması**, çakışma yok.
- Karar #143 (6-layer environment) ile uyumlu → tile katmanı bir alt-set olarak değerlendirilir.
- STUDIO_KARAR_005 (AI Asset QA Gate v2) → tüm RIMA tile çıktıları QA gate'ten geçer.
- STUDIO_KARAR_017 (borrow değil twist) → PixelLab Tileset Pro reddet, kendi Wang aracını inşa et.

---

## II. Kullanılan / Kullanılmayan PixelLab Ürünleri (RIMA için aynı Studio)

| Ürün | RIMA Karar | Neden |
|---|---|---|
| **Create Image S-XL Pro** (text-to-image) | ✅ **KULLAN** | RIMA dungeon tile + duvar + portal asset üretiminin ANA kaynağı |
| **Edit Image / Inpaint** | ✅ **KULLAN** | Seam fix + duvar köşe alignment + cyan rune detail injection |
| **Map Object** | ✅ Kısıtlı | Brazier, sütun, altar — non-tile diegetik nesneler |
| **Character States** | ✅ Karakter pipeline | RIMA v11 PROVEN template (cyan/violet base) |
| **Create Tiles Pro** | ❌ **KULLANMA** | Studio S15 verdict universal — geometrik Wang sapması + cyan accent drift garantili |

---

## III. RIMA Tile Scope — Hangi Yöntem Ne İçin

| Scope | Yöntem | Neden |
|---|---|---|
| **Dungeon floor tile set (cobblestone, cyan rune, dirt)** | Yöntem B | 16-Wang biome geçişleri + tek üretim stil tutarlılığı |
| **Duvar overlay parçaları (KitB cliff face style)** | Yöntem A | Sadece 6-10 sprite, hassas pivot/drop face kontrolü gerek |
| **Portal/Rift/Lattice mekanik tiles** | Yöntem A + Map Object | Diegetik + animasyonlu, tile değil object |
| **Parallax BG cliff varyantları (256px+ shadow blend)** | Yöntem B | Geniş scope + atmospheric perspective consistency |
| **Particle/VFX (cyan glow, sparks, ember)** | N/A (Wang konusu değil) | Tek sprite + animator clip |

---

## IV. Yöntem A — RIMA Adapte (Tek-tek tile + assembly + inpaint)

**Süreç (RIMA paths ile):**
1. RIMA Game Artist Prompt Template (Studio template'ten RIMA palette/scope adapte) ile **4 base tile** ayrı ayrı çizdir:
   - Tile 1: tam dungeon floor (cobblestone center)
   - Tile 2: tam cyan rune floor (Karar #98 accent)
   - Tile 3: cobblestone-rune yatay birleşim
   - Tile 4: cobblestone-rune dikey birleşim
2. Reference Image Lock + Style Weight 0.85-0.95 ile palette + light disiplini
3. Matematiksel assembly: Tilesetter veya Python script (RIMA Codex dispatch)
4. PixelLab Inpaint API → seam fix (4-px Hard Edge, Denoising 0.30, Guidance 8.0)
5. **Aseprite CLI palette remap → `rima_palette.gpl` force apply** (path: `RIMA/Art/Palettes/rima_palette.gpl`)
6. Wang Validator Python script (RIMA Tools/, RGB distance > 5 = hata)
7. Unity Rule Tile import → mevcut RIMA tilemap'ler

**Output target:** `Assets/Sprites/Environment/` veya `Assets/_RIMA/Art/Tiles/` (henüz yok, ihtiyaç durumunda yarat)

---

## V. Yöntem B — RIMA Adapte (Büyük composition + grid böl + komşuluk + inpaint)

**RIMA için en uygun yöntem:** Floor biome geçişleri, duvar parçaları, parallax BG cliff seti.

**Süreç:**
1. **Tek büyük composition** çizdir:
   - 128×128 = 4×4 grid (16 tile) → 16-Wang full set
   - 192×128 = 6×4 grid (24 tile) → 16-Wang + 8 variant
2. **Tek prompt** Create Image S-XL Pro → AI **bir kerede 16+ tile aynı stil** çıkar
3. Aseprite veya Python (RIMA Tools/) ile grid'e böl
4. **Komşuluk mantığı belirle:**
   - Her cell için 4-yön edge tipi (N/E/S/W = 0:cobble veya 1:rune)
   - Binary encoding ID = `N×8 + E×4 + S×2 + W×1` (0-15) → Karar #131 16-key lookup
   - Eksik slot kontrol (16-Wang full set için 16 slot dolmalı)
5. **Kenar mantıksal birleştirme:** PixelLab Inpaint 4-px band
6. **Aseprite CLI palette remap → `rima_palette.gpl`**
7. Wang Validator Python script
8. Unity Rule Tile import

**RIMA-spesifik composition prompt şablonu:**
```text
---
ASSET_TYPE: RIMA Dungeon Tilesheet (4×4 grid, 16 cells, 32×32 each)
RESOLUTION: 128x128
CAMERA_POV: Top-down 3/4 (70-80° from horizon, Hades/CoM/D3 reference)
LIGHTING: Top-left 45° UNIFORM
PALETTE: RIMA 16-color (cyan #00FFCC accent, granite stone base, violet rune accent)
BRUSH_THICKNESS: 1-px clean black outlines uniform
STYLE_REF: Hades Elysium aesthetic + dungeon stone floor
NEGATIVE_ABSOLUTE: no anti-aliasing, no blur, no double-pixels, no gradient drift, no perspective distortion, no mixels, no cyan saturation drift
---
A 128×128 pixel tilesheet divided into a 4×4 grid of 32×32 dungeon floor tiles for a top-down 2D action roguelite. Each cell shows a transition pattern between cobblestone and cyan-rune-engraved tiles.
Row 1 (top): cobblestone-to-rune north-edge transitions
Row 2: rune-to-cobblestone inner-corner transitions
Row 3: pure cobblestone center cells with subtle stone debris variations
Row 4 (bottom): pure cyan rune cells with glowing engraving variations
All cells share identical light direction, palette, brush thickness. Edge cells designed to tile seamlessly horizontally and vertically.
```

---

## VI. Aseprite Palette-Lock Daemon (RIMA Instance)

**Studio universal daemon** RIMA için ayrı instance olarak çalışır — Studio Hibrit daemon ile karışmaması için ayrı watch folder.

| Parameter | RIMA değeri |
|---|---|
| Aseprite executable | `C:/Program Files/Aseprite/Aseprite.exe` (Studio ile aynı) |
| RIMA palette | `F:/Antigravity Projeler/2d roguelite/RIMA/Art/Palettes/rima_palette.gpl` |
| Watch inbox | `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/asset_inbox/` |
| Outbox (Unity ready) | `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Environment/_remapped/` |
| Processed archive | `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/asset_processed/` |
| Script path | `F:/Antigravity Projeler/2d roguelite/RIMA/Tools/palette_lock_daemon.py` |

**Kullanım:**
```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/Tools/palette_lock_daemon.py"
```
Background'da çalışır, 2 saniyede bir inbox'ı tarar, Aseprite CLI ile remap eder, Unity Assets'e atar.

**Dithering algoritmaları (RIMA için):**
- `none` — sert sınır, dungeon stone (default)
- `ordered` — Bayer 4×4 (parallax BG soft gradient)
- `error-diffusion` — hand-painted feel (sadece portal/rift mistik geçişler)

---

## VII. RIMA Palette Standard

**Path (master):** `F:/Antigravity Projeler/2d roguelite/RIMA/Art/Palettes/rima_palette.gpl`

**RIMA 16-color baseline (Hades Elysium estetiği + cyan #00FFCC + violet rune):**

| # | Hex | Rol |
|---|---|---|
| 1 | #1A1822 | Charcoal — outline/shadow |
| 2 | #2E2B3A | Dark slate — deep shadow |
| 3 | #3F3A4D | Granite shadow |
| 4 | #5A5466 | Granite mid |
| 5 | #7A7488 | Granite light |
| 6 | #9890A4 | Stone highlight |
| 7 | #C0B8CC | Pale stone |
| 8 | #00FFCC | **Cyan accent (Karar #98)** — rune/portal/glow |
| 9 | #4DFFE0 | Cyan light — bloom |
| 10 | #00B894 | Cyan deep — runescript shadow |
| 11 | #7C5BCB | Violet rune — magic accent |
| 12 | #4B3E80 | Violet deep — rune shadow |
| 13 | #F4A742 | Warm amber — brazier/torch |
| 14 | #C46818 | Warm deep — flame shadow |
| 15 | #E8D8C0 | Off-white — skin highlight |
| 16 | #FFFAE0 | Pale cream — extreme highlight |

Final palette user + agy collaboration ile lock edilecek. Bu placeholder — user değişiklikleri ile finalize edilir.

---

## VIII. PainterSuite v2 Otomasyon (RIMA için aday)

RIMA'da `cx_dispatch.py` zaten yaşıyor → Aseprite daemon Tools/ klasörüne kopyalandı (yapıldı).

**3 yeni tool aday (Codex dispatch sırada — Phase A Day 6+):**
1. **CompositionSlicer.py** — Büyük composition PNG'i grid'e böl + meta JSON
2. **NeighborAnalyzer.py** — 16-Wang ID hesapla + missing slot raporu (Karar #131 lookup)
3. **HybridWangBuilder.py** — Yöntem A + B birleştir orchestrator

Bunlar **Room Painter (Day 5b sonrası) ile entegre** olabilir — Window'da "Generate Wang Set" butonu ile tetiklenir, PixelLab API çağırır, daemon işler.

---

## IX. RIMA-spesifik Anti-Pattern

- ❌ PixelLab `Create Tiles Pro` (Studio S15 + RIMA aynı yasak)
- ❌ Yöntem A'da cyan saturation drift (RIMA #00FFCC sabit kalmalı, AI hue kaydırırsa palette remap eler)
- ❌ Yöntem B composition'da uniform light olmadan compile — cliff drop face shadow tutarsızlığı
- ❌ Inpaint > 2 pass — pixel keskinliği kaybolur, dungeon detayı çamur olur
- ❌ AI'a "16-Wang autotile" sıfırdan istemek — difüzyon modeli matematiksel Wang yapamaz
- ❌ Aseprite palette remap atlama — kirli ara renk, 16-color dışı sızıntı

---

## X. STUDIO_KARAR uyumu (üst-otorite check)

- **STUDIO_KARAR_005 (AI Asset QA Gate v2):** ✅ RIMA tüm tile çıktıları 10-test gate'ten geçer
- **STUDIO_KARAR_017 (borrow değil twist):** ✅ PixelLab Tiles Pro reddedildi, kendi Wang aracı inşa edilir
- **RIMA Karar #131 (16-key Wang lookup):** ✅ Bu workflow Karar #131'in pratik uygulaması, çakışma yok
- **RIMA Karar #143 (6-layer environment):** ✅ Tile katmanı 6-layer alt-set, uyumlu
- **RIMA Karar #98 (cyan #00FFCC):** ✅ Palette'in 8/16 slotu cyan ailesinde, accent korunur

---

## XI. Cross-link

- [[F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow]] — Studio universal kaynak, üst-otorite
- [[F:/LaurethStudio/05_RESEARCH/2026_05_26_pixellab_wang_tile_workflow]] — üçlü AI Wang research
- [[character_anchor_prompt_PROVEN]] — RIMA cyan/violet base palette referansı
- [[reference_pixellab_prompt_grammar]] — RIMA PixelLab prompt template
- [[lauretthstudio-2d-illusion-kb-locked]] — 2D illusion KB (PainterSuite v1.1+ seed'leri)
- [[painter-suite-v1-1-roadmap-seeds]] — Wang tool entegrasyonu için adaylar

---

## XII. Doğrulama checklist (yapılma durumu)

- [x] `MEMORY/wang_tile_build_workflow_rima.md` yazıldı (bu dosya)
- [x] `MEMORY/INDEX.md` güncelleme
- [x] `Tools/palette_lock_daemon.py` Studio'dan kopyalandı + RIMA path'leri
- [x] `Art/Palettes/rima_palette.gpl` placeholder yaratıldı (cyan #00FFCC base, 16-color)
- [x] `CURRENT_STATUS.md` 1-satır entegrasyon notu
- [ ] **User onay + palette finalize** — placeholder hex'leri review, beğenilmezse override
- [ ] **Aseprite CLI test** — basit PNG → remap → Unity drop denemesi (deferred Phase A Day 6+)
- [ ] **PainterSuite v2 entegrasyonu** — Room Painter Window'da "Generate Wang Set" butonu (Phase A Day 6+ veya Phase B)
