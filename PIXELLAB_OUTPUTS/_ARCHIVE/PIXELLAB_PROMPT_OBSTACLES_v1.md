# PixelLab Obstacle & Object Prompts — v1
# Oda İçi Objeler | Act 1: Shattered Keep
# Tarih: 2026-05-09 | Opus onaylı

---

## TOOL SEÇİMİ

**Tool: Create Image S-XL (new)** — TÜM objeler için (eski "Create Image Pro" değil)
- Map bölümünde değil, ana tool listesinde
- View: **Low top-down** (duvar/pillar/3D objeler için — daha fazla ön yüz görünür)
- View: **High top-down** (flat scatter objeler, crack decal için)
- **Transparent background: ✅ var** — chromakey pipeline gerekmez
- Boyutlar: 32 / 64 / 128 / 256 / 512 / 768 (square)
- Outline: Single color → pixel art kontur

| Obje tipi | View | Boyut | Varyasyon |
|---|---|---|---|
| Pillar (sütun) | Low top-down | 256px | 4 |
| Rubble cluster | High top-down | 128px | 16 |
| Wall torch | Low top-down | 64px | 8 |
| Floor crack decal | High top-down | 128px | 16 |
| Broken pillar stump | Low top-down | 128px | 8 |
| Barrel/crate | Low top-down | 128px | 8 |
| Bone pile | High top-down | 128px | 8 |
| Large altar | Low top-down | 256px | 4 |

> **Low top-down ne zaman:** 3D derinliği olan, yüksek objeler (pillar, barrel, altar).
> **High top-down ne zaman:** Zemine yapışık, flat görünmeli objeler (rubble, crack, bone).
> **ASLA Create Tiles Pro kullanma** — objeler tileable değil, bağlantısız sprite.
> **Transparent background** her obje için işaretle — process_tiles.py gerekmez.

---

## ORTAK AYARLAR

| Parametre | Değer |
|---|---|
| Pixel Art Mode | ON |
| Background | #00FF00 (chromakey) |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | W1 approved wall tile yükle |

**Shared palette (tüm objeler için):**
- Shadow/outline: `#1A1C20`
- Dark stone: `#2A2D34`
- Mid stone: `#3A3D48`
- Lit face: `#4E5260`
- Highlight: `#606575`
- Wood: `#3A2818` dark / `#5A4028` mid / `#7A5838` lit
- Iron: `#282830` dark / `#3A3A45` mid
- Flame: `#C84000` base / `#FF6800` mid (torch sadece)

---

## PILLAR — Taş Sütun (Ana Obstacle)

**Boyut:** 32x64px | **4 varyasyon** | Create S-XL New

**Prompt:**
```
Isometric pixel art stone pillar prop, 32x64 pixels. Pure solid green #00FF00 background. Viewed from 2:1 isometric angle, pillar stands vertically. Three sections: decorative capital (top 12px) with simple carved detail, smooth shaft (middle 40px) with subtle stone texture, square base (bottom 12px) slightly wider. Palette: #1A1C20 (shadow left face), #2A2D34 (dark face), #3A3D48 (front face), #4E5260 (lit right face), #606575 (top cap highlight). Two-face isometric shading: left face darker, right face lit. Hard pixel edges. No gradient. 4 variations: intact, cracked shaft, moss-streaked (#263530 max 6px), damaged capital.
```

---

## RUBBLE CLUSTER — Taş Moloz Yığını

**Boyut:** 64x48px | **16 varyasyon** | Create Image Pro

**Prompt:**
```
Isometric pixel art rubble and broken stone debris cluster, 64x48 pixels. Pure solid green #00FF00 background. Scattered broken stone fragments viewed from 2:1 isometric angle, irregular silhouette. Stones have same palette as dungeon walls: #1A1C20, #2A2D34, #3A3D48, #4E5260. Fragments range from 4px to 16px in length, randomly angled. Dust/gravel particles 1-2px dots around edges (#2E2E3A). Hard pixel edges. No gradient, no smooth shading. 16 variations: different fragment arrangements, sizes, and scatter patterns. All stay within the 64x48 bounding box with green bg.
```

---

## WALL TORCH — Duvar Meşalesi

**Boyut:** 16x32px | **8 varyasyon** | Create Image Pro

**Prompt:**
```
Isometric pixel art wall-mounted torch prop, 16x32 pixels. Pure solid green #00FF00 background. Torch viewed from 2:1 isometric side. Iron bracket (bottom 10px): #282830, #3A3A45. Wooden handle (middle 12px): #3A2818, #5A4028. Flame head (top 10px): #C84000 base, #FF6800 mid flame, #FFAA00 hot tip — flame is 8px wide tapering to 4px, 3-frame implied motion captured in still. Hard pixel edges. No gradient outside the flame. 8 variations: flame slightly different shapes (left-leaning, right-leaning, tall, wide, dimmer versions with less #FFAA00).
```

---

## FLOOR CRACK DECAL — Zemin Çatlak Detayı

**Boyut:** 64x64px | **16 varyasyon** | Create Image Pro

**Prompt:**
```
Isometric pixel art floor crack decal overlay, 64x64 pixels. Pure solid green #00FF00 background fills ALL pixels except the crack itself. The crack is a single thin irregular fracture line, 1-3px wide, crossing the 2:1 isometric diamond area diagonally. Crack color: #1A1C20 (black crack) with #2A2A30 edge pixels. NO stone texture — this overlays on top of floor tiles as a transparent decal. 16 variations: different diagonal directions, lengths (half-diamond to full-crossing), branching vs straight. Crack never touches the diamond edges (leave 4px buffer) so it floats naturally on floor.
```

---

## BARREL / CRATE — Fıçı / Sandık

**Boyut:** 32x32px | **8 varyasyon** | Create Image Pro

**Prompt:**
```
Isometric pixel art storage barrel/crate, 32x32 pixels. Pure solid green #00FF00 background. Viewed from 2:1 isometric angle showing top face and front face. Barrel variant (4 vars): wooden staves #3A2818 / #5A4028, iron bands #282830 / #3A3A45, lid top face slightly lighter. Crate variant (4 vars): wooden plank box, nail heads visible as 1px dots, iron corner brackets. Isometric shading: top face lightest, front face mid, left face darkest. Hard pixel edges, no gradient. Each variant shows different damage level: intact, cracked, broken open, charred.
```

---

## BONE PILE — Kemik Yığını

**Boyut:** 48x32px | **8 varyasyon** | Create Image Pro

**Prompt:**
```
Isometric pixel art scattered bones and skull pile, 48x32 pixels. Pure solid green #00FF00 background. Viewed from 2:1 isometric top-down angle. Bones: off-white #C8C0A8, shadow side #8A8070, crack lines #5A5048. Small skull visible in half the variations (12px diameter, simplified — 2 dark eye socket pixels). Irregular silhouette. Hard pixel edges. 8 variations: different bone arrangements, with/without skull, different pile density.
```

---

## ÜRETIM WORKFLOW

1. **Önce pillar üret** (Create S-XL New, 4 var) → en çok göze batan obje → style anchor
2. Pillar'ı style reference olarak yükle → **rubble cluster** (16 var)
3. Palette consistency check: pillar + rubble + W1 wall Unity'de yan yana
4. Devam: torch → crack decal → barrel/crate → bone pile
5. Her obje grubundan **8 var QC onayı** → sonra 16'ya çıkar

## UNITY IMPORT NOTU

- Tüm objeler: Sprite Mode = Single, Pivot = Bottom Center
- PPU: 64 (32px objeler küçük görünür — Unity scale 2x yapılabilir)
- Sorting layer: Entities (Y-sort aktif)
- Collider: Physics Shape (Polygon) — sadece görünen pikseller
