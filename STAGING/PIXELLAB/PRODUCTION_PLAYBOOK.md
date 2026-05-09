# PixelLab Production Playbook
*Tek dosya, sırayla. Aç → Adım 1 → bitir → Adım 2.*
*Son güncelleme: 2026-05-10*

> **Kullanım:** Bu dosyanın sırasını takip et. Her adım için **tool**, **ayarlar**, **prompt** (kopyala-yapıştır), **kaydet path**'i ve **process komutu** verilmiş. Bittikçe `[ ]` → `[x]` işaretle.

> **REPO_ROOT:** `F:/Antigravity Projeler/2d roguelite/RIMA`
> Tüm path'ler bu kökten relative.

---

## 📋 Üst Düzey Checklist

- [ ] **A. Walls** (3 üretim) — `01_NEXT_walls/`
  - [ ] Adım 1: W1 wall (8 var, 64x128)
  - [ ] Adım 2: W2 wall (8 var, 64x128)
  - [ ] Adım 3: OBW wall (4 var, 64x128)
- [ ] **B. Floors** (5 üretim) — `02_NEXT_floors/`
  - [ ] Adım 4: F1 floor (16 var, 64x64)
  - [ ] Adım 5: F2 floor (16 var)
  - [ ] Adım 6: F3 floor (16 var)
  - [ ] Adım 7: Trans F1→F2 (8 var)
  - [ ] Adım 8: Trans F2→F3 (8 var)
- [ ] **C. Obstacles** (8 üretim) — `03_NEXT_obstacles/`
  - [ ] Adım 9: Pillar (4 var)
  - [ ] Adım 10: Rubble cluster (16 var)
  - [ ] Adım 11: Wall torch (8 var)
  - [ ] Adım 12: Floor crack decal (16 var)
  - [ ] Adım 13: Barrel/Crate (8 var)
  - [ ] Adım 14: Bone pile (8 var)
  - [ ] Adım 15: Broken pillar stump (8 var)
  - [ ] Adım 16: Large altar (4 var)
- [ ] **D. Warblade Anim** (10 alt adım, simetrik) — `04_NEXT_Warblade_anim/`
- [ ] **E. Ranger Anim** (10 alt adım, asimetrik) — `05_NEXT_Ranger_anim/`
- [ ] **F. Shadowblade Anim** (10 alt adım, asimetrik) — `06_NEXT_Shadowblade_anim/`
- [ ] **G. Elementalist Anim** (9 alt adım, asimetrik, weapon pass YOK) — `07_NEXT_Elementalist_anim/`

---

## 🔑 Genel Kurallar (her üretimde geçerli)

### PixelLab Web App ortak ayarlar

| Ayar | Değer |
|---|---|
| Pixel Art Mode | **ON** |
| Upscale | **OFF** |
| Anti-aliasing | **OFF** |

### Karakter anim için (Sınıflar)

- **Web App ZORUNLU** — MCP `animate_character` KULLANMA (4-frame limit + VFX bug)
- Canvas: PixelLab v3 otomatik 252x252 üretir, Unity'ye 128x128 kırp
- Hero anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png` her gen'de yükle

### Tile/wall için chromakey

- Background: **#00FF00** (saf yeşil)
- Process script filter: `G>200 AND R<60 AND B<60`

### Obstacle için

- Background: **Transparent ON** (chromakey gerekmez, process_tiles.py atlanır)

---

# A — WALLS

## ✅ Adım 1: W1 Wall (Ana Duvar)

**🛠️ Tool:** PixelLab → **Create Tile — Isometric** (Map bölümünde)

**⚙️ Ayarlar:**
- Boyut: **64x128**
- Variation: **8** (straight ×2, corner ×2, T-junction, end-cap ×2)
- Background: **#00FF00**
- Style Reference: F1 approved floor varsa yükle (yoksa boş bırak)

**📝 Prompt (kopyala):**
```
Isometric pixel art stone wall tile, 64x128 pixels, 2:1 isometric projection. Pure solid green #00FF00 background fills all pixels outside the wall shape.

The tile has three vertical zones:
TOP FACE (top 12px): wall surface viewed from isometric top — slightly lighter, shows stone top, ambient light catch.
FRONT FACE (middle 104px): vertical stone brick masonry, staggered courses, each brick 12-16px wide and 7-9px tall. Main surface.
BASE SHADOW (bottom 12px): ambient occlusion strip where wall meets floor, gradient from #1A1C20 to transparent.

Palette STRICTLY: #1A1C20 (mortar/shadow), #2A2D34 (dark stone face), #3A3D48 (mid stone), #4E5260 (lit face), #606575 (top face highlight). NO other colors.

Flat-shaded pixel art, NO smooth gradient, NO ambient occlusion render, NO 3D software look. Hand-pixeled appearance. Dithered shading only (2x2 checker). Hard pixel edges. No anti-aliasing on tile boundary. Pixel clusters minimum 4px.

Generate 8 connection variants: (1) straight north-facing, (2) straight south-facing, (3) outer corner NE, (4) outer corner NW, (5) inner corner, (6) T-junction, (7) end-cap north, (8) end-cap south. Each shares identical brick pattern and palette — only silhouette and corner geometry differ.

One weathering accent per tile max: hairline crack, chipped brick corner, or iron ring anchor.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_sheet_v1.png
```

**🐍 Process to Unity:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_sheet_v1.png --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 2 --width 64 --height 128 --prefix w1_
```

**✅ QC kontrol:**
- [ ] Background tam #00FF00 (gri sızma yok)
- [ ] 8 variant net görünüyor
- [ ] Pixel cluster ≥4px

---

## ✅ Adım 2: W2 Wall (Daha Bozuk)

**🛠️ Tool:** Create Tile — Isometric

**⚙️ Ayarlar:**
- Boyut: **64x128**, **8 var**
- Background: **#00FF00**
- Style Reference: **W1 approved tile yükle (Adım 1 çıktısı)**

**📝 Prompt:**
```
Isometric pixel art stone wall tile, 64x128 pixels, 2:1 isometric projection. Pure solid green #00FF00 background.

Same three-zone structure as W1: top face 12px, front face 104px, base shadow 12px.

Palette STRICTLY: #18181E (deep mortar), #26293A (very dark stone), #363A4A (dark mid stone), #464B5E (mid stone), #565C70 (lit face). Slightly cooler/bluer than W1 — deeper dungeon feeling.

Same flat-shaded pixel art rules: NO gradient, NO smooth shading, dithered only, hard pixel edges, pixel clusters min 4px.

Brick pattern shows MORE damage than W1: wider mortar cracks, missing brick corners, occasional horizontal fracture line crossing 2-3 bricks. One variation has a faint bioluminescent lichen vein (#1E3028, max 8px) along a crack.

Same 8 connection variants as W1: straight north, straight south, outer corner NE, outer corner NW, inner corner, T-junction, end-cap north, end-cap south.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/01_NEXT_walls/outputs/w2/w2_sheet_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/01_NEXT_walls/outputs/w2/w2_sheet_v1.png --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 128 --prefix w2_
```

---

## ✅ Adım 3: OBW Wall (Yüksek Arch Wall)

**🛠️ Tool:** Create Tile — Isometric

**⚙️ Ayarlar:**
- Boyut: **64x128**, **4 var**
- Background: **#00FF00**
- Style Reference: **W1 approved tile**

**📝 Prompt:**
```
Isometric pixel art tall architectural wall section, 64x128 pixels, 2:1 isometric. Pure solid green #00FF00 background. This is a TALLER wall section — no top face visible (wall extends above frame), no base shadow (wall extends below frame). Front face only: 128px vertical stone masonry. Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260. Flat-shaded, dithered, hard pixel edges. 4 variations: plain stone, with narrow window slit (4x12px dark void), with iron wall bracket, with carved rune (simple geometric glyph, 8x8px, raised relief).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/01_NEXT_walls/outputs/obw/obw_sheet_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/01_NEXT_walls/outputs/obw/obw_sheet_v1.png --output Assets/Art/Tiles/Act1/OBW --cols 2 --rows 2 --width 64 --height 128 --prefix obw_
```

> **Unity import sonrası:** PPU=64, Pivot=bottom-center (0.5, 0), Sorting layer=Walls.

---

# B — FLOORS

## ✅ Adım 4: F1 Floor (Soğuk Gri Granit)

**🛠️ Tool:** PixelLab → **Create Tiles Pro** (Map bölümünde, **Standard değil — Pro**)

**⚙️ Ayarlar:**
- Tile Type: **Isometric** (dropdown)
- View: **High top-down**
- Boyut: **64x64**, **16 variation**
- Background: **#00FF00**

**📝 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background fills every pixel outside the tile diamond. The tile is a perfect 2:1 isometric diamond rhombus viewed from top. Cold grey granite stone surface, staggered rectangular brick pattern with visible mortar lines. Palette STRICTLY 4 colors only: #1E2028 (darkest crack/mortar), #2E3038 (dark stone face), #424555 (mid stone), #555868 (lightest face, top-lit). Flat baked dungeon lighting, NO gradient, NO smooth shading, NO ambient occlusion render. Pixel clusters minimum 4px wide. Hard pixel edges, no anti-aliasing on tile boundary. Each of 16 variations has ONE subtle accent only: hairline crack, faint moss stain (1-3 pixels #2D4030 max), corner chip, or water mark — never two accents on same tile. Seamless — pattern continues flush to diamond edge so adjacent tiles tessellate without gap.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/02_NEXT_floors/outputs/f1/f1_sheet_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f1/f1_sheet_v1.png --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_
```

---

## ✅ Adım 5: F2 Floor (Çatlak + Lichen)

**🛠️ Tool:** Create Tiles Pro (Isometric, High top-down)

**⚙️ Ayarlar:**
- 64x64, **16 var**, **#00FF00**
- Style Reference: **F1 approved tile (Adım 4)**

**📝 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. Same 2:1 isometric diamond as F1 floor. Cracked stone surface, wider mortar fractures than F1. Palette STRICTLY 5 colors: #1A1C22 (deep crack), #2A2C35 (dark stone), #3C3F4E (mid stone), #4E5260 (lit face), #263530 (bioluminescent lichen accent — use sparingly, max 6 pixels per tile). Flat baked lighting only, NO gradient. Pixel clusters minimum 4px. Hard pixel edges. Each of 16 variations has one main crack feature (longer hairline crossing 20+ pixels) PLUS one optional lichen dot cluster. Seamless tessellation. The overall feel is darker and more stressed than F1.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/02_NEXT_floors/outputs/f2/f2_sheet_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f2/f2_sheet_v1.png --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_
```

---

## ✅ Adım 6: F3 Floor (Volkanik Boss Bölgesi)

**🛠️ Tool:** Create Tiles Pro

**⚙️ Ayarlar:**
- 64x64, **16 var**, **#00FF00**
- Style Reference: **F2 approved tile (Adım 5)**

**📝 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. Same 2:1 isometric diamond. Dark volcanic basalt floor, heavy fracture lines, glowing energy crack details. Palette STRICTLY 5 colors: #14141A (near-black volcanic), #222230 (dark basalt), #323240 (mid basalt), #424255 (lifted face), #4A1A1A (deep energy glow — dark crimson, max 8 pixels per tile for crack glow). Flat baked lighting. Pixel clusters minimum 4px. Hard pixel edges. Each variation has one prominent lava-crack vein running diagonally, glowing #4A1A1A at center darkening to stone. Some variations add crumbled edge. Seamless tessellation. Atmosphere: oppressive, corrupted, near the boss.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/02_NEXT_floors/outputs/f3/f3_sheet_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f3/f3_sheet_v1.png --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_
```

---

## ✅ Adım 7: Trans F1→F2

**🛠️ Tool:** Create Tiles Pro

**⚙️ Ayarlar:**
- 64x64, **8 var** (sheet: 2 cols × 4 rows)
- Style Reference: F1 + F2 approved tile

**📝 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. 2:1 isometric diamond. Transition stone: left half of tile is clean F1 grey granite (#2E3038, #424555), right half transitions into cracked F2 style (#2A2C35, #3C3F4E, #263530 lichen). The split is diagonal not vertical, creating a natural rock fault line. Flat baked lighting only. Hard pixel edges. 8 variations, each with slightly different fault line angle and lichen spread.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f1f2_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f1f2_v1.png --output Assets/Art/Tiles/Act1/Trans_F1F2 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f1f2_
```

---

## ✅ Adım 8: Trans F2→F3

**🛠️ Tool:** Create Tiles Pro

**⚙️ Ayarlar:** 64x64, 8 var, F2 + F3 ref

**📝 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. 2:1 isometric diamond. Transition: F2 cracked stone (#3C3F4E) bleeding into F3 volcanic basalt (#222230, #4A1A1A energy). Diagonal fault line with glowing crack appears at transition. 8 variations, varied crack thickness and glow intensity.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f2f3_v1.png
```

**🐍 Process:**
```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f2f3_v1.png --output Assets/Art/Tiles/Act1/Trans_F2F3 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f2f3_
```

> **Unity import:** PPU=64, Pivot=center (0.5, 0.5), Sorting layer=Ground.

---

# C — OBSTACLES

> **Önemli:** Bu bölümün TÜMÜ için tool: **Create Image S-XL (new)** (Map'te DEĞİL, ana tool).
> **Background: Transparent ON** — process_tiles.py GEREKMEZ. PNG'ler direkt Unity'ye drag-drop.
> Style Reference: W1 approved wall tile yükle (palette tutarlılık).

## ✅ Adım 9: Pillar (Taş Sütun)

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **256px**
- View: **Low top-down**
- Variation: **4**
- Background: **Transparent ON**
- Outline: Single color

**📝 Prompt:**
```
Isometric pixel art stone pillar prop, 32x64 pixels. Pure transparent background. Viewed from 2:1 isometric angle, pillar stands vertically. Three sections: decorative capital (top 12px) with simple carved detail, smooth shaft (middle 40px) with subtle stone texture, square base (bottom 12px) slightly wider. Palette: #1A1C20 (shadow left face), #2A2D34 (dark face), #3A3D48 (front face), #4E5260 (lit right face), #606575 (top cap highlight). Two-face isometric shading: left face darker, right face lit. Hard pixel edges. No gradient. 4 variations: intact, cracked shaft, moss-streaked (#263530 max 6px), damaged capital.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/pillar_var01.png
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/pillar_var02.png
... (4 dosya)
```

**Unity import:**
- Klasör: `Assets/Art/Props/Act1/Pillar/`
- Sprite Mode: Single, Pivot: Bottom Center, PPU: 64
- Sorting layer: Entities

---

## ✅ Adım 10: Rubble Cluster

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **128px**, View: **High top-down**
- Variation: **16**, Transparent ON

**📝 Prompt:**
```
Isometric pixel art rubble and broken stone debris cluster, 64x48 pixels. Pure transparent background. Scattered broken stone fragments viewed from 2:1 isometric angle, irregular silhouette. Stones have same palette as dungeon walls: #1A1C20, #2A2D34, #3A3D48, #4E5260. Fragments range from 4px to 16px in length, randomly angled. Dust/gravel particles 1-2px dots around edges (#2E2E3A). Hard pixel edges. No gradient, no smooth shading. 16 variations: different fragment arrangements, sizes, and scatter patterns.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/rubble_var01..16.png
```

**Unity import:** `Assets/Art/Props/Act1/Rubble/`, Pivot: Bottom Center, Sorting: Entities.

---

## ✅ Adım 11: Wall Torch

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **64px**, View: **Low top-down**, **8 var**, Transparent ON

**📝 Prompt:**
```
Isometric pixel art wall-mounted torch prop, 16x32 pixels. Pure transparent background. Torch viewed from 2:1 isometric side. Iron bracket (bottom 10px): #282830, #3A3A45. Wooden handle (middle 12px): #3A2818, #5A4028. Flame head (top 10px): #C84000 base, #FF6800 mid flame, #FFAA00 hot tip — flame is 8px wide tapering to 4px, 3-frame implied motion captured in still. Hard pixel edges. No gradient outside the flame. 8 variations: flame slightly different shapes (left-leaning, right-leaning, tall, wide, dimmer versions with less #FFAA00).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/torch_var01..08.png
```

**Unity import:** `Assets/Art/Props/Act1/Torch/`, Pivot: Bottom Center.

---

## ✅ Adım 12: Floor Crack Decal

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **128px**, View: **High top-down**, **16 var**, Transparent ON

**📝 Prompt:**
```
Isometric pixel art floor crack decal overlay, 64x64 pixels. Pure transparent background fills ALL pixels except the crack itself. The crack is a single thin irregular fracture line, 1-3px wide, crossing the 2:1 isometric diamond area diagonally. Crack color: #1A1C20 (black crack) with #2A2A30 edge pixels. NO stone texture — this overlays on top of floor tiles as a transparent decal. 16 variations: different diagonal directions, lengths (half-diamond to full-crossing), branching vs straight. Crack never touches the diamond edges (leave 4px buffer) so it floats naturally on floor.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/crack_var01..16.png
```

**Unity import:** `Assets/Art/Props/Act1/Decal/`, Pivot: Center (decal floor üzerine), Sorting: Ground+1.

---

## ✅ Adım 13: Barrel / Crate

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **128px**, View: **Low top-down**, **8 var** (4 barrel + 4 crate), Transparent ON

**📝 Prompt:**
```
Isometric pixel art storage barrel/crate, 32x32 pixels. Pure transparent background. Viewed from 2:1 isometric angle showing top face and front face. Barrel variant (4 vars): wooden staves #3A2818 / #5A4028, iron bands #282830 / #3A3A45, lid top face slightly lighter. Crate variant (4 vars): wooden plank box, nail heads visible as 1px dots, iron corner brackets. Isometric shading: top face lightest, front face mid, left face darkest. Hard pixel edges, no gradient. Each variant shows different damage level: intact, cracked, broken open, charred.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/barrel_var01..04.png
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/crate_var01..04.png
```

**Unity import:** `Assets/Art/Props/Act1/Barrel/` (barrel) + `Assets/Art/Props/Act1/Crate/` (crate).

---

## ✅ Adım 14: Bone Pile

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **128px**, View: **High top-down**, **8 var**, Transparent ON

**📝 Prompt:**
```
Isometric pixel art scattered bones and skull pile, 48x32 pixels. Pure transparent background. Viewed from 2:1 isometric top-down angle. Bones: off-white #C8C0A8, shadow side #8A8070, crack lines #5A5048. Small skull visible in half the variations (12px diameter, simplified — 2 dark eye socket pixels). Irregular silhouette. Hard pixel edges. 8 variations: different bone arrangements, with/without skull, different pile density.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/bone_var01..08.png
```

**Unity import:** `Assets/Art/Props/Act1/Bone/`, Pivot: Bottom Center.

---

## ✅ Adım 15: Broken Pillar Stump

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **128px**, View: **Low top-down**, **8 var**, Transparent ON
- Style Reference: **Adım 9 Pillar (sağlam) yükle**

**📝 Prompt:**
```
Isometric pixel art broken stone pillar stump, 32x32 pixels. Pure transparent background. Lower half of a pillar after collapse — base (12px) intact, shaft (20px) sheared off at irregular angle. Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260. Top fracture surface shows broken stone texture (lighter, exposed core #4E5260 with cracks). Hard pixel edges. 8 variations: different fracture angles, varying degrees of damage, occasional moss patch.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/stump_var01..08.png
```

**Unity import:** `Assets/Art/Props/Act1/PillarStump/`, Pivot: Bottom Center.

---

## ✅ Adım 16: Large Altar (Altar of Resonance)

**🛠️ Tool:** Create Image S-XL (new)

**⚙️ Ayarlar:**
- Canvas: **256px**, View: **Low top-down**, **4 var**, Transparent ON

**📝 Prompt:**
```
Isometric pixel art ritual altar prop, 64x64 pixels. Pure transparent background. Three-tiered stone altar viewed from 2:1 isometric angle. Wide square base (bottom 20px), narrower middle tier (middle 20px), top altar surface with central recess (top 24px). Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260, #606575. Cyan rift glow accent in central recess: #00FFCC max 8 pixels (faint pulsing inlay). Hard pixel edges, no gradient outside cyan glow. Two-face shading. 4 variations: intact altar, weathered (chipped corners), runic glow stronger (cyan crack web on top), partially collapsed (middle tier missing).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/03_NEXT_obstacles/outputs/altar_var01..04.png
```

**Unity import:** `Assets/Art/Props/Act1/Altar/`, Pivot: Bottom Center.

---

# D — WARBLADE ANIM (Simetrik, 3 yön + W flip)

> **Sınıf bilgisi:**
> - Type: **Simetrik** → S, E, N üret. **W = Unity flipX**, üretme.
> - Accent: **Cold blue #7BA7BC**
> - Weapon: **Greatsword** (two-handed)
> - Yasak: el glow, mor renk
>
> **Tüm anim için ortak ayarlar:**
> - Web App ZORUNLU (MCP DEĞİL)
> - Hero anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
> - Canvas v3 otomatik 252x252
> - **Body-only** prompt (silah hep YOK, Adım 25 weapon pass'a kadar)

## ✅ Adım 17: Warblade Base 4-yön (Body-only)

**🛠️ Tool:** **Create Character Pro New**

**⚙️ Ayarlar:**
- Canvas: 252x252
- 4 yön: South / East / North → 3 ayrı gen call (W üretme)
- Hero Anchor yükle

**📝 Prompt (her yön için aynı, "facing X" değiştir):**
```
Pixel art warrior character, body-only, no weapon, 128x128 sprite on 252x252 canvas. High top-down view 30-35° elevation. Heavy plate armor, broad shoulders, cold blue cloth accent #7BA7BC at sash and shoulder straps. Palette: armor steel #4A4E5A / #5C6070 / #6E7280, accent blue #7BA7BC, leather #3A2818 / #5A4028, skin #C9A084 / #A07858, hair dark brown. Stoic stance, feet shoulder-width, arms relaxed. Hard pixel edges, no anti-aliasing, pixel cluster min 4px. NO embedded glow, NO VFX, NO weapon. [FACING SOUTH | FACING EAST | FACING NORTH] (face camera for south).
```

**💾 Kaydet (3 dosya):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/warblade_base_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/warblade_base_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/warblade_base_N.png
```

> W (West) üretme — Unity'de `flipX` ile E'den otomatik üretilir.

---

## ✅ Adım 18: Warblade Idle Anim

**🛠️ Tool:** **Animate with Text NEW**

**⚙️ Ayarlar:**
- Input: Adım 17'nin S/E/N sprite'ları
- Frame: 6-8
- Yön sayısı: 3 (S, E, N) → 3 gen call

**📝 Prompt (her yön için):**
```
Subtle breathing motion, 6-8 frames. Character chest rises and falls slowly, weight shifts subtly between feet. Same pose as base sprite, no weapon. [FACING SOUTH | EAST | NORTH].
```

**💾 Kaydet (3 dosya):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_idle_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_idle_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_idle_N.png
```

---

## ✅ Adım 19: Warblade Hurt Anim

**🛠️ Tool:** Animate with Text NEW

**⚙️ Ayarlar:** 3 frame, 3 yön (S/E/N)

**📝 Prompt:**
```
Flinch backwards, 3 frames. Character's torso jerks back from impact, head tilts away, no weapon. Cold blue accent (#7BA7BC) flickers slightly. Frame 1: idle pose. Frame 2: peak flinch (max backward lean). Frame 3: recovery toward idle.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_hurt_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_hurt_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_hurt_N.png
```

---

## ✅ Adım 20: Warblade Death Anim

**🛠️ Tool:** Animate with Text NEW

**⚙️ Ayarlar:** 6 frame, 3 yön

**📝 Prompt:**
```
Collapse to ground, 6 frames. Heavy character falls forward to knees then face-down. No weapon. Frame 1: stagger. Frame 2: knees buckle. Frame 3: kneeling. Frame 4: torso falls forward. Frame 5: arm catches ground. Frame 6: prone, motionless.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_death_S.png (+E, +N)
```

---

## ✅ Adım 21: Warblade Walk Cycle (Brian's Extreme Pose)

**🛠️ Tool:** Animate with Text NEW + **Interpolate NEW**

**Adım 21a — Extreme Pose A üret:**
```
Walking forward, right leg fully extended in stride, weight shifted to front foot, arms swing in counter-rhythm, body lean slight forward. Heavy warrior gait, no weapon. [FACING S | E | N].
```
12 frame al → en uç pozu (diz en yukarıda) **seç** → Pose A.

**Adım 21b — Pose B = A flip:**
Aseprite'ta pose A'yı yatay flip → Pose B kaydet.

**Adım 21c — Interpolate A → B:**
Tool: **Interpolate NEW**, Input: Pose A + Pose B, Output: 4-6 frame.

**💾 Kaydet (her yön için 6 frame walk cycle):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/03_run_cycle/warblade_walk_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/03_run_cycle/warblade_walk_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/03_run_cycle/warblade_walk_N.png
```

---

## ✅ Adım 22: Warblade Attack_LMB (3-Segment Greatsword Slash)

**🛠️ Tool:** Animate with Text NEW (PEAK) + **Interpolate NEW** (segments)

**Adım 22a — PEAK frame:**
```
Greatsword horizontal slash at full extension, arms parallel to ground, sword tip past character silhouette right edge. Body twisted 30° to follow slash, weight on back foot, full commitment. Cold blue accent flickers at sword wake (#7BA7BC).
```
> Body-only kuralı: PEAK frame'i silahla üretiyorsun (3-segment'in tamamı silahlı çekilir, weapon pass yok). **DİKKAT:** Karar #71 LOCKED — Warblade silah hep elde, sheath/draw YOK. Yani LMB/RMB anim'lerinde silah görünür ama base/idle/walk/hurt/death silah olmayabilir.

**Adım 22b — START → PEAK:**
- Tool: Interpolate NEW
- Input: idle sprite (Adım 18) + PEAK frame
- Output: 4 frame windup

**Adım 22c — PEAK → END:**
- Input: PEAK + recovery pose (idle benzeri)
- Output: 4 frame follow-through

**💾 Kaydet (her yön için 9 frame total):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/04_attack_LMB/warblade_lmb_S.png (+E, +N)
```

---

## ✅ Adım 23: Warblade Attack_RMB (Heavy Slam)

**🛠️ Tool:** Animate with Text NEW + Interpolate NEW

**PEAK frame:**
```
Greatsword slammed into ground, both hands gripping hilt at chest level, blade vertical with tip at character's feet. Body fully forward, knees bent, weight committed downward. Impact frame — peak commitment.
```

**START → PEAK:** 4 frame (sword raised overhead, max windup)
**PEAK → END:** 4 frame (recovery, pull sword from ground)

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/05_attack_RMB/warblade_rmb_S.png (+E, +N)
```

---

## ✅ Adım 24: Warblade Dash

**🛠️ Tool:** Animate with Text NEW (4 frame)

**📝 Prompt:**
```
Quick forward lunge, 4 frames. Frame 1: anticipation crouch (knees bent). Frame 2: explosive forward push, leading leg extended, body horizontal. Frame 3: airborne mid-dash, arms back. Frame 4: landing crouch, recovery. No weapon.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/06_dash/warblade_dash_S.png (+E, +N)
```

---

## ✅ Adım 25: Warblade Weapon Pass (Edit Image Pro)

**🛠️ Tool:** **Edit Image Pro**

> Bu adım body-only sprite setine **silah ekler**. Adım 22-23'teki LMB/RMB için silahlı PEAK zaten çekildi — bu adım idle/walk/dash/hurt/death için silah ekler.

**⚙️ Ayarlar:**
- Input: Adım 17 base S sprite (body-only)
- Yön: 3 ayrı pass (S, E, N)

**📝 Prompt:**
```
Add greatsword on right shoulder, two-handed grip when raised. Sword: 3.5 head-tall blade, steel #6E7280 / #8A8E98 / #A6AAB4, hilt wrapped leather #3A2818, crossguard iron #282830. Cold blue cloth wrap on hilt (#7BA7BC). NO glow, NO embedded VFX. Apply per direction: S, E, N (W = flip E with sword in correct hand — no separate weapon paint).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/07_weapon_pass/warblade_weapon_S.png (+E, +N)
```

> Sonra Aseprite'ta tüm idle/walk/dash/hurt/death frame'lerine bu silah katmanını propagate et (kopyala-yapıştır).

---

# E — RANGER ANIM (Asimetrik, 4 yön ayrı)

> **Sınıf bilgisi:**
> - Type: **Asimetrik** → S, E, N, **W** dördü ayrı (yay tek elde)
> - Accent: **Cold blue #7BA7BC** (Warblade ile aynı), Yasak: mor
> - Weapon: **Compound bow** (sol elde)
> - Body-only adımlar 26-33, weapon pass adım 34

## ✅ Adım 26: Ranger Base 4-yön (Body-only)

**🛠️ Tool:** Create Character Pro New | 4 yön ayrı (S, E, N, W)

**📝 Prompt (her yön için):**
```
Pixel art ranger character, body-only, no weapon, 128x128 sprite on 252x252 canvas. High top-down view 30-35°. Lean agile build, hooded cloak, cold blue undertunic (#7BA7BC), forest green cloak (#3A4A38 / #4E5E48). Quiver visible on back (leather strap). Palette: cloak green #3A4A38 / #4E5E48, leather #3A2818 / #5A4028, accent blue #7BA7BC, skin #C9A084. Light leather armor, flexible stance, feet hip-width. Hood up, partial face shadow. NO weapon held. [FACING S | E | N | W]. Hard pixel edges, no anti-aliasing.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/01_base_4dir/ranger_base_S.png (+E, +N, +W)
```

---

## ✅ Adım 27: Ranger Idle (4 yön)

**🛠️ Tool:** Animate with Text NEW | 6-8 frame, 4 yön

**📝 Prompt:**
```
Alert breathing, 6-8 frames. Hood slightly sways, head subtly scans, weight subtly shifts. Tense posture but relaxed shoulders. No weapon.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/02_idle_hit_death/ranger_idle_S.png (+E, +N, +W)
```

---

## ✅ Adım 28: Ranger Hurt (4 yön)

**📝 Prompt:**
```
Flinch sideways, 3 frames. Light agile recoil — body twists rather than falls back. Cape flares from motion. Frame 1: idle. Frame 2: peak twist (45° body turn). Frame 3: recovery.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/02_idle_hit_death/ranger_hurt_S.png (+E, +N, +W)
```

---

## ✅ Adım 29: Ranger Death (4 yön)

**📝 Prompt:**
```
Collapse sideways, 6 frames. Light body falls to one side, hood slips off head. No weapon. Frame 1: stagger. Frame 2-3: knees buckle and lean. Frame 4: hand catches ground. Frame 5: lying on side. Frame 6: motionless, hood on ground.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/02_idle_hit_death/ranger_death_S.png (+E, +N, +W)
```

---

## ✅ Adım 30: Ranger Walk Cycle (Brian's Extreme Pose, 4 yön)

**Tool:** Animate with Text NEW + Interpolate NEW

**Extreme Pose A prompt:**
```
Walking forward light-footed, right leg extended in stride, body lean very slight forward, cape sways behind. Quick agile gait. No weapon. [FACING S | E | N | W].
```

**Walk pipeline:** Pose A → Aseprite flip → Pose B → Interpolate (4-6 frame).

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/03_run_cycle/ranger_walk_S.png (+E, +N, +W)
```

---

## ✅ Adım 31: Ranger Attack_LMB (Bow Shot, 4 yön)

**🛠️ Tool:** Animate with Text NEW + Interpolate NEW (3-segment, 9 frame)

**PEAK prompt:**
```
Bow drawn fully, left arm extended forward holding bow, right hand at cheek anchor, arrow knocked. Body twisted 45° (asymmetric stance), bow vertical. Cold blue accent (#7BA7BC) glints on bowstring. Full draw commitment.
```

**START → PEAK:** 4 frame (bow raised, drawing motion)
**PEAK → END:** 4 frame (release — string snaps forward)

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/04_attack_LMB/ranger_lmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 32: Ranger Attack_RMB (Aim Shot 2-Stage, 4 yön)

**PEAK prompt:**
```
Slow aim — bow at full draw with extra time, breath held, body very still and centered. Arrow tip glows faintly cold blue (#7BA7BC charge). Pose more deliberate than LMB.
```

**Pipeline:** 3-segment, 9 frame.

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/05_attack_RMB/ranger_rmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 33: Ranger Dash (4 yön)

**📝 Prompt:**
```
Quick agile roll, 4 frames. Frame 1: crouch. Frame 2: forward dive low to ground, body horizontal. Frame 3: tucked roll mid-air. Frame 4: emerge upright with bow ready. No weapon.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/06_dash/ranger_dash_S.png (+E, +N, +W)
```

---

## ✅ Adım 34: Ranger Weapon Pass (Edit Image Pro, 4 yön)

**📝 Prompt:**
```
Add compound bow held in LEFT hand. Bow: vertical orientation when at rest, ~1.2x character height. Wood riser #5A4028 / #7A5838, limbs darker #3A2818, string thin off-white #C8C0A8. Cold blue grip wrap (#7BA7BC). Quiver on back already in base sprite. Apply per direction: S, E, N, W (each painted separately — flip changes weapon hand).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/07_weapon_pass/ranger_weapon_S.png (+E, +N, +W)
```

---

# F — SHADOWBLADE ANIM (Asimetrik, 4 yön)

> **Sınıf bilgisi:**
> - Type: **Asimetrik** → 4 yön ayrı
> - Accent: **Void purple #5A2A8A** (Shadowblade kendi violet)
> - Weapon: **Twin short blades** (her elde 1)
> - Yasak: embedded glow karakter sprite'ında — VFX engine-side

## ✅ Adım 35: Shadowblade Base 4-yön (Body-only)

**🛠️ Tool:** Create Character Pro New | 4 yön ayrı

**📝 Prompt:**
```
Pixel art shadowblade assassin character, body-only, no weapon, 128x128 sprite on 252x252 canvas. High top-down view 30-35°. Slim agile build, full dark hooded cloak, body almost entirely silhouetted in dark with violet undertones. Palette: cloak black-purple #1A0E2A / #2A1A3A, mid #3A2A4E, accent violet #5A2A8A, skin partial visible #C9A084 (only chin and jawline below hood), leather straps #3A2818. Crouched ready stance, body lean forward, weight on balls of feet. Hood deep — no eyes visible (silhouette only). NO weapon, NO embedded glow. [FACING S | E | N | W]. Hard pixel edges.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/01_base_4dir/shadowblade_base_S.png (+E, +N, +W)
```

---

## ✅ Adım 36: Shadowblade Idle (4 yön)

**📝 Prompt:**
```
Predator stillness, 6-8 frames. Very subtle motion — only cloak hem flutters slightly, head turns by 5-10° to scan. Body stays low and tense. No weapon. Less obvious breathing than other classes — assassin stillness.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/02_idle_hit_death/shadowblade_idle_S.png (+E, +N, +W)
```

---

## ✅ Adım 37: Shadowblade Hurt (4 yön)

**📝 Prompt:**
```
Sharp recoil, 3 frames. Body twists hard from impact, cloak flares dramatically. Violet accent (#5A2A8A) flashes on cloak edge. Frame 1: idle. Frame 2: peak recoil (cloak full flare, body 60° twist). Frame 3: recovery.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/02_idle_hit_death/shadowblade_hurt_S.png (+E, +N, +W)
```

---

## ✅ Adım 38: Shadowblade Death (4 yön)

**📝 Prompt:**
```
Dissolve / sink, 6 frames. Character collapses but with shadowy fade — last 2 frames show partial dissolution into ground (cloak fades to silhouette). Frame 1: stagger. Frame 2: knees buckle. Frame 3: lying on side. Frame 4: cloak fades (alpha 70%). Frame 5: only silhouette remains (alpha 40%). Frame 6: faint shadow stain on ground (alpha 20%).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/02_idle_hit_death/shadowblade_death_S.png (+E, +N, +W)
```

---

## ✅ Adım 39: Shadowblade Walk (4 yön)

**📝 Prompt (Extreme Pose A):**
```
Walking forward predator-style, low and silent, body crouched, knees bent slightly, weight always on balls of feet. Cloak sways behind. No weapon. Subtle compared to Warblade — less stride distance, more compressed posture. [FACING S | E | N | W].
```

**Pipeline:** Pose A → flip → Pose B → Interpolate.

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/03_run_cycle/shadowblade_walk_S.png (+E, +N, +W)
```

---

## ✅ Adım 40: Shadowblade Attack_LMB (Twin Blade Chain, 4 yön)

**PEAK prompt (1st hit):**
```
Right blade slash horizontal, arm extended at full slash, body 30° twisted. Left arm pulled back ready for next strike. Violet accent at blade trail (#5A2A8A streak). Body fully committed forward, weight on front foot.
```

**3-segment, 9 frame.**

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/04_attack_LMB/shadowblade_lmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 41: Shadowblade Attack_RMB (Shadow Step / VeilStrike, 4 yön)

**PEAK prompt:**
```
Phase-strike — character mid-teleport, body half-dissolved into shadow, blade emerging at target side with violet streak (#5A2A8A intense). Anticipation pose: original location shows fading silhouette, peak position shows blade impact frame. Two visual elements in single frame — shadow trail behind, character at strike point.
```

**START → PEAK:** 4 frame (crouch, fade into shadow at original spot)
**PEAK → END:** 4 frame (re-materialize, blade lower, recovery)

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/05_attack_RMB/shadowblade_rmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 42: Shadowblade Dash (4 yön)

**📝 Prompt:**
```
Shadow dash, 4 frames. Frame 1: crouch with violet wisp at feet. Frame 2: body partially dissolves, blur streak forward. Frame 3: body re-forms ahead, mid-arrival. Frame 4: landing crouch, blades ready. Cloak trails violet smoke (#5A2A8A faint, max 4px wisp).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/06_dash/shadowblade_dash_S.png (+E, +N, +W)
```

---

## ✅ Adım 43: Shadowblade Weapon Pass (Edit Image Pro, 4 yön)

**📝 Prompt:**
```
Add twin short blades — one per hand. Each blade: ~0.6x character height, narrow silhouette, dark steel #4A4E5A / #5C6070, hilt wrapped black-violet (#1A0E2A / #5A2A8A). Curved or straight short-sword profile. Apply per direction: S, E, N, W (each painted separately).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/07_weapon_pass/shadowblade_weapon_S.png (+E, +N, +W)
```

---

# G — ELEMENTALIST ANIM (Asimetrik, 4 yön, SİLAHSIZ)

> **Sınıf bilgisi:**
> - Type: **Asimetrik** → 4 yön ayrı (el jestleri tek tarafta belirgin)
> - Accent: **Element bazlı** — sprite element-agnostic kalır (cool neutral robe), VFX engine-side
> - Weapon: **YOK** → **Adım 44'ten Adım 51'e kadar 8 adım, weapon pass YOK**
> - Yasak: void purple (Shadowblade'in mor'u), kitap/odiyak

## ✅ Adım 44: Elementalist Base 4-yön (Silahsız, eller serbest)

**🛠️ Tool:** Create Character Pro New | 4 yön ayrı

**📝 Prompt:**
```
Pixel art elementalist mage character, body-only, no weapon, NO book, NO staff (hands free for spell gestures), 128x128 sprite on 252x252 canvas. High top-down view 30-35°. Long flowing robe, hood NOT up (face visible — confident mage), short hair. Robe palette: deep blue-grey #2A3848 / #3E4C5E / #525E74 (cool neutral default — element accent only on spell anims). Trim accent: faint cool #B8C8D0. Sash at waist #3A2818 leather. Skin #C9A084. Body pose: slightly forward, hands held at chest level, palms angled outward (ready to cast). Robe hem sways. NO weapon, NO held object. [FACING S | E | N | W]. Hard pixel edges, no anti-aliasing.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/01_base_4dir/elementalist_base_S.png (+E, +N, +W)
```

---

## ✅ Adım 45: Elementalist Idle (4 yön)

**📝 Prompt:**
```
Subtle channeling stance, 6-8 frames. Hands hover at chest, fingers slightly curl and uncurl as if shaping invisible energy. Robe hem sways. Hair barely moves. Confident mage breathing.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/02_idle_hit_death/elementalist_idle_S.png (+E, +N, +W)
```

---

## ✅ Adım 46: Elementalist Hurt (4 yön)

**📝 Prompt:**
```
Stagger backwards, 3 frames. Mage recoils, hands pulled to chest defensively, robe flares from sudden motion. Frame 1: idle. Frame 2: peak recoil (body bent backward 30°, hands up). Frame 3: recovery stance.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/02_idle_hit_death/elementalist_hurt_S.png (+E, +N, +W)
```

---

## ✅ Adım 47: Elementalist Death (4 yön)

**📝 Prompt:**
```
Knees buckle slowly, 6 frames. Mage falls to knees first (caster's last stand), then forward. Robe billows. Frame 1: stagger. Frame 2: knees give. Frame 3: kneeling. Frame 4: hands fall to floor. Frame 5: forward fall. Frame 6: prone.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/02_idle_hit_death/elementalist_death_S.png (+E, +N, +W)
```

---

## ✅ Adım 48: Elementalist Walk (4 yön)

**📝 Prompt:**
```
Walking forward mage gait, robe trailing behind, smooth deliberate step (not as quick as Ranger, not as heavy as Warblade). Hands held loosely at sides or chest. No weapon. [FACING S | E | N | W].
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/03_run_cycle/elementalist_walk_S.png (+E, +N, +W)
```

---

## ✅ Adım 49: Elementalist Attack_LMB (CastRhythm Spell, 4 yön)

**PEAK prompt:**
```
Both hands extended forward, palms outward, fingers spread — peak cast moment. Element energy gathers at palms (small VFX placeholder area, ~12x12px each, intentionally blank for engine VFX overlay). Body lean slightly forward, weight balanced. NO embedded glow on character — only blank palm zones marked for engine particle. Confident mage release pose.
```

> **DİKKAT:** Karakter sprite'ında element rengi YOK. Sprite element-agnostic, VFX overlay runtime'da Fire/Frost/Lightning/Light farkını ekler.

**3-segment, 9 frame.**

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/04_attack_LMB/elementalist_lmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 50: Elementalist Attack_RMB (Element Ranged Spell, 4 yön)

**PEAK prompt:**
```
One hand thrust forward (right hand), palm out, the other hand pulled back at hip channeling. Body twisted 30° to support thrust. Robe billows from cast force. Blank palm zone for VFX (16x16px on extended hand). NO embedded glow.
```

**3-segment, 9 frame.**

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/05_attack_RMB/elementalist_rmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 51: Elementalist Dash (4 yön)

**📝 Prompt:**
```
Mage step / blink, 4 frames. Frame 1: crouch with hand gesture (palm down at side, channeling). Frame 2: body briefly distorts (motion blur with robe streak forward). Frame 3: body re-forms ahead, robe still trailing. Frame 4: landing pose, hands return to chest. NO embedded glow — engine VFX adds element trail per active element.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/06_dash/elementalist_dash_S.png (+E, +N, +W)
```

> **❌ Weapon Pass YOK — Elementalist silahsız.** Adım 25/34/43 muadili Adım'ı YOK. Bu sınıf 51 adımda biter.

---

# 🏁 BİTİŞ

Tüm 51 adım tamamlandığında:
1. Klasör adlarına `__DONE_` prefix ekle (örn: `__DONE_01_NEXT_walls`) **VEYA** `_DONE/` altına taşı
2. Bana haber ver — `CURRENT_STATUS.md` güncellenecek
3. Yeni Act / sınıf üretimleri için yeni numaralı klasör aç (`08_NEXT_Brawler_anim` vs.)

## Hızlı Tahmin

| Bölüm | Süre (solo) | Kredit |
|---|---|---|
| A. Walls (3 üretim) | ~1-2 saat | ~150-200 |
| B. Floors (5 üretim) | ~1-2 saat | ~120-180 |
| C. Obstacles (8 üretim) | ~2-3 saat | ~100-150 |
| D. Warblade (Sym) | ~1-1.5 hafta | ~640-940 |
| E. Ranger (Asym) | ~1-1.5 hafta | ~750-1100 |
| F. Shadowblade (Asym) | ~1-1.5 hafta | ~750-1100 |
| G. Elementalist (Asym, no weapon) | ~1 hafta | ~660-960 |

> Tier 2 abonelik (Yasin'in mevcut planı) ~2000-3000 kredit/ay → 4 sınıf P0 ≈ 2 ay.

## Referanslar (sub-agent'lara gerekirse)

- Master Pipeline: `GUIDES/RIMA_MASTER_ART_PIPELINE.md`
- Animation Bible: `TASARIM/ANIMATION_BIBLE.md`
- Style anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
- Klasör HOWTO/PROMPT detayları: her klasörün kendi içinde
