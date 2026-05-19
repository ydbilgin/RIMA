---
name: pixellab-create-image-pro
description: "PixelLab web UI \"Create Image Pro\" parametre spec — boyut tablosu, description/reference/style image kuralları. User manuel üretir, Claude prompt sağlar."
metadata: 
  node_type: memory
  type: reference
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# PixelLab Create Image Pro — Web UI Spec (2026-05-16 S86)

**Üretim akışı:** User PixelLab web UI > "Create Image Pro" sekmesinde manuel çalıştırır. MCP'de YOK (bkz: [[pixellab-tool-inventory]] — create_object 256px square max, create_map_object 400px max). Claude'un rolü: **prompt + boyut + ref/style önerisi**.

## Boyut tablosu (LOCKED — sadece bu seçenekler)

| Boyut | Aspect | Kullanım önerisi |
|---|---|---|
| 32×32 | 1:1 | Mini icon, tiny decal |
| 64×64 | 1:1 | Small object, sprite icon |
| 128×128 | 1:1 | Standard tile, small decoration, character ref input |
| 256×256 | 1:1 | Medium decoration, L4-L6 decal hero |
| **344×192** | **16:9** | Wide landscape decal, banner |
| 341×341 | 1:1 | Large symmetric (rare) |
| **384×216** | **16:9** | Wide environment piece |
| **512×512** | **1:1** | **Large hero asset — L3 wall corner, big decoration** |
| **512×288** | **16:9** | **Wide hero — L3 wall horizontal cap, banner sprite** |
| 632×424 | 3:2 | Landscape composition |
| **424×632** | **2:3** | **Portrait — L3 wall vertical cap, tall decoration** |
| 688×384 | 16:9 | Largest wide — hero environment / map section |

**MCP karşılaştırma (referans için):**
- create_object: 256 max square ONLY → web UI'nın 512×512 / 688×384 yeteneklerini karşılayamıyor
- create_map_object: 400 max width/height → 512 ve üstünü karşılayamıyor
- **512px sprite gerekiyorsa create image pro şart** (MCP'de yok)

## Description alanı

> "Describe what you want to generate. Be specific about style and details."

**Prompt formülü (RIMA için):**
1. Subject + scene context (örn. "horizontal stone wall cap, shattered keep dungeon")
2. Camera angle ("top-down ~30-35° high overhead view" — RIMA LOCK)
3. Style adjectives ("Hades-style painter pixel art, organic painterly brush strokes")
4. Silhouette rule ("ORGANIC IRREGULAR bottom edge, NO straight lines, NO grid")
5. Palette hex'leri açık ("#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575")
6. Shadow/depth rule ("dramatic dark shadow band at inner bottom edge")
7. Opacity/dither rule ("fully opaque stone body, crisp pixel dithering, no anti-aliasing")
8. Negative ifadeler ("NO bright colors, NO Gaussian blur, NO warm brown")

## Reference image (4 slot max)

> "Upload up to 4 images. Refer to them in your description as 'reference image 1', 'reference image 2', etc."

**Kullanım kuralı:** İhtiyaç olmadığında kullanma. Sadece şu durumlarda:
- Karakter identity ref (Warblade silhouette korumak için)
- Spesifik silhouette / pose reference
- Color study reference

Prompt'ta açık çağrı şart: "Match the silhouette of reference image 1, use the color palette from reference image 2."

## Style image (1 slot, ÖZEL DAVRANIŞ)

> "Upload a pixel art image to guide the art style. **Output size will be set based on the style image size.**"

**Kritik:** Style image konulursa **output size dropdown ignore edilir** — çıktı boyutu style image'in boyutuna eşitlenir. Yani style image 256×256 ise output 256×256.

**Kullanım kuralı:**
- Style consistency için (örn. mevcut Hades spritesheet ile match)
- RIMA L3 wall için: `Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png` style ref olarak konabilir
- İhtiyaç olmadığında kullanma — description daha esnek

## RIMA mapping (Sprint 3 sprite üretim önerisi)

| Sprite | Önceki batch boyutu | Create Image Pro boyutu |
|---|---|---|
| L3 wall horizontal | 256×128 | **512×288 (16:9)** ← daha geniş + 2x detay |
| L3 wall vertical | 128×256 | **424×632 (2:3)** ← daha uzun + detay |
| L3 wall corner NE/NW/SE/SW | 128×128 | **256×256** veya **512×512** (kalite tercihi) |
| L3 wall doorway | 128×96 | **344×192 (16:9)** |
| L4 moss decal | TBD | **128×128** veya **256×256** |
| L5 dirt crack | TBD | **256×256** (single hero crack) |
| L6 rubble | TBD | **128×128** (multiple variants) |
| L6 rift fracture | TBD | **256×256** veya **512×288** (hero) |

## Negative prompt (RIMA universal)

```
bright colors, smooth gradient, Gaussian blur, modern UI chrome, repeating tileset grid pattern,
uniform rectangular block edges, anti-aliased rounded shapes, cartoon outline, warm brown palette,
transparent pixels inside main body, symmetrical surface pattern, modern detail, sci-fi sheen
```

## See Also

- [[pixellab-tool-inventory]] — MCP endpoint matrisi (512 max desteklemiyor)
- [[pixellab-character-via-web-ui-v3]] — Karakter de web UI'da (V3 ile)
- [[visual-quality]] — 128px native + Hades painter tone
- [[camera-angle-revisit-s43]] — 30-35° top-down LOCKED
- [[karar-143-pipeline]] — 6-layer painter pipeline
