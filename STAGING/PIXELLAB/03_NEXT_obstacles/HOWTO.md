# 03 — Obstacles & Props
*Source: `STAGING/PIXELLAB_PROMPT_OBSTACLES_v1.md` (LOCKED 2026-05-09)*

## Tool

**PixelLab → Create Image S-XL (new)** — TÜM objeler için.
- Map bölümünde **DEĞİL** — ana tool listesinde
- View: **Low top-down** → 3D derinlikli (pillar, barrel, altar)
- View: **High top-down** → flat scatter (rubble, crack, bone)
- **Transparent background ON** → chromakey gerekmez
- Boyutlar: 32 / 64 / 128 / 256 / 512 / 768 (square)
- Outline: Single color → pixel art kontur

> **ASLA Create Tiles Pro kullanma** — obje tileable değil.
> **ASLA Create Tile Isometric kullanma** — connection-aware sprite gerekmiyor.

## Üretim Sırası

| # | Obje | Boyut | Var | View | Neden bu sırada |
|---|---|---|---|---|---|
| 1 | **Pillar** | 256px (32x64 logical) | 4 | Low top-down | En göz alıcı → style anchor |
| 2 | **Rubble cluster** | 128px (64x48 logical) | 16 | High top-down | Pillar'ı style ref olarak yükle |
| 3 | **Wall torch** | 64px (16x32 logical) | 8 | Low top-down | Flame kontrast palette test |
| 4 | **Floor crack decal** | 128px (64x64 logical) | 16 | High top-down | Tile üzeri overlay |
| 5 | **Barrel/Crate** | 128px (32x32 logical) | 8 | Low top-down | Combat room dolgu |
| 6 | **Bone pile** | 128px (48x32 logical) | 8 | High top-down | F2/F3 atmosfer |
| 7 | **Broken pillar stump** | 128px | 8 | Low top-down | Pillar variant — son |
| 8 | **Large altar** | 256px | 4 | Low top-down | Altar of Resonance node — son |

## Settings (her gen call'da)

| Param | Value |
|---|---|
| Tool | Create Image S-XL (new) |
| Background | **Transparent ON** (chromakey gerekmez) |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | W1 approved wall tile yükle (palette tutarlılık için) |

## Shared Palette

```
Shadow/outline:  #1A1C20
Dark stone:      #2A2D34
Mid stone:       #3A3D48
Lit face:        #4E5260
Highlight:       #606575

Wood:            #3A2818 / #5A4028 / #7A5838
Iron:            #282830 / #3A3A45
Flame:           #C84000 / #FF6800 / #FFAA00 (sadece torch)
Bone:            #C8C0A8 / #8A8070 / #5A5048
Lichen:          #263530
```

## Output

PNG'leri `outputs/<obje_adı>/` (yoksa oluştur) altına kaydet.
- `outputs/pillar/pillar_v1_var01..04.png`
- `outputs/rubble/rubble_v1_var01..16.png`
- vb.

## Process

Transparent bg olduğu için **`process_tiles.py` GEREKMEZ.** Direkt Unity'ye drag-drop:
- Klasör: `Assets/Art/Props/Act1/<obje>/`
- Sprite Mode: **Single**
- Pivot: **Bottom Center (0.5, 0.0)**
- PPU: **64**
- Sorting layer: **Entities** (Y-sort aktif)
- Collider: **Polygon (Physics Shape)** — sadece görünen pikseller

## QC Checklist

- [ ] Background tam transparent (alpha 0 her tile'ın etrafında)
- [ ] Palette shared listesinden taşmıyor
- [ ] Pixel cluster min 4px
- [ ] Hard pixel edge, anti-alias yok
- [ ] Low top-down: 3D objeler için (top + front face görünür)
- [ ] High top-down: flat objeler (sadece üst görüş)
- [ ] Pillar/altar: bottom center pivot uygun (ground'a oturuyor)
- [ ] Rubble/crack/bone: irregular silhouette doğru
- [ ] Torch: 3-frame implied motion (still frame, gameplay'de animate edilecek)
