---
name: pixellab-prompt-grammar
description: RIMA PixelLab prompt yazma kilavuzu - resmi guide ozeti + ogrenilen patterns
metadata:
  type: reference
---

# RIMA PixelLab Prompt Grammar

Bu dosya RIMA icin PixelLab prompt yazim referansidir. Basliklar Turkce tutuldu, prompt template'leri PixelLab daha iyi anladigi icin Ingilizce yazildi. Internal memory dosyalari ASCII-only kuralina uyar; Turkce karakterler bilerek transliteratedir.

## 1. Resmi PixelLab Guide Ozeti

### 1.1 Genel sonuc

Ayrica adlandirilmis tek bir resmi "Prompt Guide", "Prompt Tips" veya "Prompt Best Practices" sayfasi bulunamadi. Resmi guidance PixelLab dokumantasyonunda tool bazli dagitilmis durumda.

Kaynak tarama:
- Local docs: `PixelLabDocs/*.md` altindaki 63 markdown dosyasinin headingleri ve prompt/guidance gecisleri tarandi.
- Web docs: `https://www.pixellab.ai/docs`, `options/guidance`, `options/general`, `guides/map-tiles`, `guides/rotating-a-character` HTTP 200 ile kontrol edildi.
- MCP docs: `PixelLabDocs/mcp_docs.md` kontrol edildi.
- Discord: public invite `https://discord.gg/pBeyTBF8T7` authentication/login disinda pinned mesaj erisimi vermedi; pinned/featured mesaj dogrulanamadi.

### 1.2 `options/guidance`

Resmi prompt cekirdegi buradadir:
- `Description`: modelin ne uretmesi gerektigini kisa tarif eder.
- `Negative description`: uretimde olmamasi gereken seyleri tarif eder; modelin istenmeyen sonuclardan uzaklasmasina yardimci olabilir.
- `Guidance weight`: prompt'a ne kadar uyulacagini kontrol eder; fazla yuksek olursa over-saturation ve artifact riski artar.
- `Camera view`: high top-down, low top-down, side; zayif bir kontrol oldugu icin prompt ve init/reference ile desteklenmeli.
- `Fidelity`: referansa ne kadar sadik kalacagini belirler.
- `Style guidance weight`: style image stilini kopyalama gucunu belirler.
- `Depth strength`: reference depth yapisini kopyalama gucunu belirler.

RIMA icin anlami: description kisa ve net olmali, negative description default blok olarak her uretimde kullanilmali, guidance weight yukseltilirken artifact kontrolu yapilmali.

### 1.3 `options/general`

Resmi genel ayarlar:
- `Remove background`: transparent background uretir.
- `Seed`: kucuk degisikliklerle benzer sonuc almak icin ayni seed tekrar kullanilabilir; `0` random seed.
- `Tile size`: tile uretimi/extend icin 16x16, 32x32 gibi tile boyutu.
- `Paint in selection`: selected area icinde uretimi sinirlar.
- `Force symmetry`: rotation tool icin south/north dikey simetri, west/east mirror iliskisi kurabilir.

RIMA icin anlami: transparent output gerekiyorsa prompt'a yazmak yetmez, UI/MCP ayari da acik olmali. Aynilik gereken batchlerde seed + style lock birlikte kullanilir.

### 1.4 `options/camera`

Resmi camera notlari:
- View/direction controls zayiftir.
- Init image ve prompt keywordleri ile desteklemek daha iyi sonuc verir.
- West/east cogu durumda redundant olabilir; horizontal flip kullanilabilir.

RIMA icin anlami: 8-dir veya 5+3 mirror workflow'da mirror edilebilir yonleri AI'ya tekrar urettirmek yerine flipX kullan.

### 1.5 `options/color`

Resmi color notlari:
- Current palette veya selected image palette ile renk kisitlanabilir.
- Force colors palette adherence'i guclendirir.
- Color reduction icin auto/specify colors/use palette secenekleri var.

RIMA icin anlami: Shattered Keep style lock icinde HEX/RGB palette mutlaka tekrar edilmeli; final asset import oncesi renk sayisi ve anti-alias kontrol edilmeli.

### 1.6 `tools/consistent-style`

Resmi consistent-style notlari:
- Style reference images ile yeni image'lar ayni gorsel stile yaklastirilir.
- 32px civari cok sayida frame, 64px civari 16 frame, 128px civari 4 frame, 171px+ tek frame verir.
- Style image sayisi boyuta gore sinirlanir.
- No Background default on olabilir ama yine de kontrol edilmeli.
- Max image size 512x512.

RIMA icin anlami: batch asset pack icin iyi, ama cok farkli assetleri tek batchte karistirmak stil algisini bulandirir.

### 1.7 `tools/create-sl-image-pro`

Resmi S-XL Pro notlari:
- 16px-512px square araliginda calisir.
- Non-square desteklenir; 16:9 icin ornek max 688x384.
- 342-512 square tek frame ve 40 generations.
- Reference images max 4, style image opsiyonu var.
- No Background transparent output icin var.

RIMA icin anlami: 768/1024 canvas bekleme; buyuk sheet gerekiyorsa 512 square veya supported non-square kullan.

### 1.8 `tools/create-image-flux`

Resmi M-XL pixflux notlari:
- Daha genel modeldir, text understanding ve large image handling daha iyidir.
- Scenery icin yaklasik 16:9, character icin 2:3 onerilir.
- Free/Tier limitlerine gore max area 200x200, 320x320, 400x400.

RIMA icin anlami: composition-aware scene snippetlerde kullanilabilir; final modular asset icin Pro/style-reference daha kontrolludur.

### 1.9 `tools/create-tiles-pro`

Resmi Create Tiles Pro notlari:
- Square top-down, hex, isometric, octagon tile variations uretir.
- Description, tile type, tile size, view angle, thickness ve style tiles destekler.
- Paid subscription gerektirir.

RIMA icin kritik yorum: Bu tool tile variation generator gibi davranir; structured Wang16 semantic layout'u garanti etmez. Wang logic icin MCP `create_topdown_tileset` daha dogru secimdir.

### 1.10 `tools/create-8-rotations-pro` ve `guides/rotating-a-character`

Resmi rotation notlari:
- 8 directional sprite tek 3x3 gridde uretilebilir.
- Max frame size 168x168.
- View: low top-down, high top-down, side.
- Rotating guide iki yontem verir: tek south reference'tan tum yonler veya her 45 derece adimda reference update.
- Hata mitigation: regenerate, inpaint, manual fix + init image, veya symmetric direction'i mirror etmek.

RIMA icin anlami: 5 yon uret, 3 yon mirror et; south, south-east, east, north-east, north AI uretimi yeterli olabilir.

### 1.11 `guides/map-tiles` ve `tools/inpaint-v3`

Resmi map/inpaint notlari:
- Create Map workflow'da selection + inpaint layer birlikte kullanilir.
- Model sadece selection icini gorur; smooth transition icin selection'in tamamini inpaint etme, context birak.
- Description selected area'nin ortasindaki seyi tarif etmeli.
- Inpaint v3 maskeli alanda content uretir; min 32x32, max 256x256; larger canvas icin selection kullan.

RIMA icin anlami: junction fix ve wall seam polish icin kucuk maskeli inpaint kullan; tum sheet'i yeniden urettirme.

### 1.12 MCP docs

MCP docs prompt guidance'i tool parametrelerinde verir:
- Creation jobs async; create returns id, get polls status.
- `create_topdown_tileset` 16 Wang tileset uretir ve base tile id ile chain edilebilir.
- `create_tiles_pro` promptta tile'lari numaralandirmayi onerir.
- `create_isometric_tile` icin 32px recommended.
- `create_map_object` style matching mode background image ile calisir, transparent object icin uygundur.
- Pro custom animation high cost gerektirir; explicit user approval olmadan `confirm_cost=true` kullanilmaz.

## 2. Tool Capabilities (RIMA-Relevant)

| Need | Best tool | Size | RIMA use | Notes |
|---|---|---:|---|---|
| Single floor tile | Create S-XL Pro or MCP `create_1_direction_object` | 32-128 | approved single asset | Highest quality when one asset only |
| True Wang terrain | MCP `create_topdown_tileset` | 16/32 | terrain transitions | Structured 16-corner logic |
| Random tile variants | Create Tiles Pro | 32/64 | exploration | Not reliable for structured Wang layout |
| Wall/backwall sheet | Create S-XL Pro | 512 sheet | A6 or 2x2 wall packs | Keep cells few and edge rules repeated |
| Single prop | Create S-XL Pro / MCP object | 64-170 | chest, brazier, statue | transparent background explicit |
| Decal overlay | Create S-XL Pro | 64-128 | rift cracks, stains | transparent background mandatory |
| Character 8-dir | Create 8-directional sprite Pro | 64x64 | chibi chars | 5+3 mirror for economy |
| Animation | PixelLab UI Animate with Text New | 64 ref | user manual | action prompt only |
| Style pack | Consistent-style Pro | 64/128 | similar props | focused refs, not random dump |
| Local seam fix | Inpaint v3 | 32-256 mask | junction polish | keep context visible |
| Important edit | Edit Image Pro | 32-512 | weapon/pass detail | describe only target edit |

## 3. RIMA-Spesifik Prompt Templates

### 3.1 Floor Tile (single asset)

Use for one approved floor asset. Do not ask for a sheet.

```text
Single 32x32 top-down pixel art floor tile for RIMA Shattered Keep.

- flat walkable dark slate flagstone floor
- uneven charcoal grey slabs with worn mortar
- subtle cold blue shadow tint
- sparse tiny stone chips and hairline cracks
- tileable on all four edges
- no walls, no raised rim, no bevel, no props
- no characters, no UI, no text

Style lock: dark gritty Shattered Keep, matte hand-pixeled clusters, hard pixel edges, limited palette, no anti-aliasing.

Negative: no text, no labels, no captions, no numbers, no watermarks, no logo, no UI frame, no character, no weapon, no object, no perspective wall, no soft blur, no anti-aliasing.
```

### 3.2 Floor Tile (Wang16 attempt)

Use only if forced to test a sheet prompt. Prefer MCP `create_topdown_tileset` for real Wang logic.

```text
Create a 4x4 Wang terrain test sheet, 16 cells total, each cell is 32x32 pixels.
Use a visible grid but do not write any text or numbers.

- terrain A: dark rubble stone floor, charcoal grey slate, cracked mortar
- terrain B: cyan rift-fractured stone floor, same elevation, same stone base, thin cyan-violet fracture lines
- every cell must be a flat walkable floor tile
- all outer edges must tile seamlessly
- terrain transitions must follow corner-based Wang logic
- no raised borders, no walls, no cliffs, no props
- shared palette, shared lighting, shared pixel density across all 16 cells

Negative: no text, no labels, no captions, no numbers, no watermarks, no cell names, no letters, no icons, no UI, no characters, no objects, no walls, no bevel, no height change.
```

Neden daha iyi: `Cell 1: NAME` formatini kullanmaz; AI'nin baked label cizme bias'ini dusurur. Bullet format semantigi korur ama text rendering trigger'ini azaltir. Yine de structured Wang logic garanti degildir.

### 3.3 Wall/Backwall (modular sheet)

Use for a controlled 2x2 wall sheet, not for 16 different wall ideas.

```text
2x2 grid asset pack. Pixel art dark fantasy dungeon wall modules for RIMA Shattered Keep.
Each cell is 256x256 pixels. Total sheet is 512x512 pixels.

- NW-SE straight wall, pillar-less continuous weathered dark stone
- NE-SW straight wall, mirrored wall angle, same edge profile
- top apex corner where both wall directions converge
- bottom apex corner where both wall directions converge

CRITICAL EDGE LOCK:
- all four cells share identical dark stone palette
- same brick size, same mortar spacing, same top cap height
- top, bottom, left, and right edge pixels must match between compatible cells
- no decor baked into wall center
- no torches, no banners, no doors, no props

Style lock: hand-pixeled dark slate stone, cold blue shadows, sparse cyan rift cracks, hard pixel edges, no anti-aliasing, no labels.

Negative: no text, no labels, no captions, no numbers, no watermarks, no UI, no floating symbols, no characters, no loose props, no background scene.
```

### 3.4 Character (8-dir reuse, S99 pattern)

Production target: 64x64 chibi, PPU 64. Generate 5 directions, mirror 3.

```text
Create a 64x64 chibi top-down pixel art character sprite for RIMA.

TYPE: humanoid game character
STYLE: dark fantasy ARPG chibi, readable at 64x64, clean silhouette
VIEW: high top-down action RPG view, approximately 70-80 degrees production angle
DIRECTION: south
BODY: compact heroic chibi proportions, oversized readable head, short limbs
CLOTHING: [class outfit here]
WEAPON: [weapon here], visible and readable
SILHOUETTE: strong outline, weapon silhouette separated from body
COLOR: limited dark fantasy palette with one clear accent color

CONSTRAINTS:
- exact 64x64 sprite footprint
- transparent background
- point-pixel hard edges
- no anti-aliasing
- no embedded VFX
- no floor shadow outside sprite footprint

Negative: no text, no labels, no captions, no numbers, no watermark, no UI, no portrait frame, no extra weapons, no background, no blur.
```

Generate directions:
- South
- South-east
- East
- North-east
- North

Mirror:
- South-west = flipX(south-east)
- West = flipX(east)
- North-west = flipX(north-east)

### 3.5 Prop (single asset, transparent BG)

```text
Single transparent-background pixel art prop for RIMA Shattered Keep.

- subject: [prop name]
- camera: high top-down game view, readable as a standalone sprite
- size target: [64x64 or 128x128]
- material: weathered dark stone, blackened iron, old wood, or cold cyan rift crystal as appropriate
- silhouette: centered object, no crop, clear outline
- lighting: dim top-left torch light with cold blue ambient shadow
- palette: charcoal grey, blue-grey shadow, muted brown, rare cyan-violet magical accent

Style lock: matte hand-pixeled dark fantasy, clean clusters, hard edges, no anti-aliasing, transparent background.

Negative: no text, no labels, no captions, no numbers, no watermark, no floor tile, no wall behind it, no UI frame, no soft blur.
```

### 3.6 Decal (single asset, transparent overlay)

```text
Single transparent overlay decal for RIMA Shattered Keep.

- subject: thin cyan rift crack decal
- shape: irregular branching hairline fracture, asymmetrical, not circular
- camera: flat top-down overlay
- size target: 64x64
- background: fully transparent alpha
- color: cold cyan core with very subtle violet edge pixels
- use: placed over existing stone floor or wall

Style lock: crisp pixel clusters, limited palette, no anti-aliasing, no glow cloud, no smoke.

Negative: no text, no labels, no captions, no numbers, no watermark, no stone slab, no floor base, no black rectangle, no UI, no object.
```

### 3.7 Sheet Composition (4-cell, 2x2)

Use for related variants only. Do not mix unrelated subjects.

```text
2x2 grid asset sheet, four related variants, each cell 128x128 pixels.
No text, no labels, no numbers in any cell.

- plain iron brazier, unlit, centered, transparent background
- same brazier with small warm orange flame
- broken brazier, cracked bowl, no flame
- rift-corrupted brazier with subtle cyan crack, no large VFX

Shared constraints:
- identical camera angle
- identical scale and pixel density
- identical outline thickness
- identical palette and lighting
- transparent background for every cell

Negative: no text, no labels, no captions, no numbers, no watermark, no UI, no floor tiles, no wall background, no scene.
```

## 4. Hard Rules (DO and DO NOT)

### DO

- Use bullet lists for multi-requirement prompts.
- Keep one prompt focused on one asset family.
- Repeat camera, size, palette, lighting, background, and edge rules in every batch.
- Use `transparent background` in prompt and enable `No Background` / `Remove background` in the tool.
- Use negative prompt: `no text, no labels, no captions, no numbers, no watermarks`.
- Prefer single-asset generation for final quality.
- Prefer reference-first production when an approved visual anchor exists.
- Use same style image, seed, palette block, and perspective block for a batch.
- Use small inpaint masks for junction fixes.
- For character direction economy, generate 5 directions and mirror 3.
- For tileset logic, use MCP `create_topdown_tileset` instead of trying to force Wang16 through a generic sheet.
- For style chaining, prefer `n_frames + reference_image_base64` when available and dimension-compatible.

### DO NOT

- Do not write `Cell 1: NAME, Cell 2: NAME` in prompts. It increases baked text/label risk.
- Do not ask Create Tiles Pro to guarantee structured Wang16 logic. It is a variation generator.
- Do not use `create_object_state` for production iteration. It is too expensive for RIMA's frame-count economics.
- Do not mix sprite, floor, wall, UI, and VFX goals in one prompt.
- Do not omit transparent background when alpha is required.
- Do not rely on view/direction dropdowns alone; also write direction and camera in prompt.
- Do not over-detail 32px/64px assets. Readability beats micro-detail.
- Do not regenerate a whole wall sheet for a 2px seam; inpaint the seam.
- Do not bake torches, banners, doors, and cracks into base walls unless the sheet is explicitly a decor variant sheet.
- Do not expect 768/1024 output from S-XL Pro; use supported sizes or split sheets.

## 5. Common Failure Modes + Mitigation

| Failure | Cause | Mitigation |
|---|---|---|
| Baked cell labels | Numbered `Cell 1: NAME` wording | Use bullets and negative text block |
| Missing alpha | Prompt says asset but tool background not removed | Enable No Background and write transparent background |
| Wang16 logic wrong | Generic sheet model lacks corner-state structure | Use `create_topdown_tileset` |
| Style drift in batch | Too many unrelated cells | Keep family narrow, repeat style lock |
| Bad wall seams | Edge profile not specified | Add CRITICAL EDGE LOCK block |
| Character silhouette unreadable | Dark skin/clothing/weapon merge | Add high-contrast accent and silhouette constraints |
| Over-saturated output | Guidance too high | Lower guidance, limit palette |
| Inpaint changes too much | Mask too large or no context | Small mask, keep adjacent pixels visible |
| Rotation identity drift | One-shot 8 dirs too ambitious | Use reference, inpaint fixes, mirror symmetric dirs |
| Prompt ignored direction | Direction controls weak | Add profile/facing words and init/reference |

## 6. Negative Prompt Library (RIMA defaults)

### 6.1 Default all assets

```text
no text, no labels, no captions, no numbers, no watermarks, no logo, no UI, no frame, no blurry edges, no anti-aliasing, no smooth vector gradients
```

### 6.2 Floor tiles

```text
no text, no labels, no captions, no numbers, no watermarks, no wall, no raised rim, no bevel, no cliff, no props, no characters, no UI, no height change
```

### 6.3 Walls

```text
no text, no labels, no captions, no numbers, no watermarks, no characters, no loose props, no baked torch, no baked banner, no UI, no full room scene
```

### 6.4 Props

```text
no text, no labels, no captions, no numbers, no watermark, no wall background, no floor tile base, no UI frame, no scene, no crop
```

### 6.5 Decals

```text
no text, no labels, no captions, no numbers, no watermark, no stone base, no black rectangle, no smoke cloud, no object body, no UI
```

### 6.6 Characters

```text
no text, no labels, no captions, no numbers, no watermark, no portrait, no UI, no background, no embedded VFX, no extra limbs, no duplicate weapon, no blur
```

### 6.7 Sheets

```text
no text, no labels, no captions, no numbers, no watermarks, no cell names, no letters, no UI, no title, no legend, no annotation
```

## 7. Tool Selection Decision Tree

```text
Need true terrain transition logic?
-> use MCP create_topdown_tileset.

Need one final prop/decal/floor?
-> use single-asset S-XL Pro or object creator, transparent background.

Need several related variants?
-> use 2x2 sheet max, bullet list only, no cell labels.

Need many random tile variants?
-> use Create Tiles Pro, but do not assume Wang structure.

Need wall modules?
-> use S-XL Pro 512x512 sheet, 2x2 or A6, with edge lock.

Need wall seam repair?
-> use Inpaint v3 with small mask and adjacent context.

Need character directions?
-> use Create 8-directional sprite Pro or UI rotation, 64x64, generate 5 and mirror 3.

Need character animation?
-> use PixelLab UI Animate with Text New; action prompt only; no MCP animate_character for RIMA production.

Need important edit?
-> use Edit Image Pro; describe target change only.

Need style-consistent pack?
-> use focused style references; do not dump unrelated images.
```

## 8. Style Lock Block

Append this to RIMA Shattered Keep prompts unless a more specific style lock overrides it.

```text
RIMA Shattered Keep style lock: dark gritty fantasy pixel art, matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, limited desaturated palette, charcoal slate stone (#2C2A2A to #4E5260), cold blue-grey shadows, sparse cyan-violet rift accent, subtle warm torch highlight only when explicitly requested, high top-down ARPG production view around 70-80 degrees, readable silhouettes, no text, no labels, transparent background when asset alpha is required.
```

## 9. Update Log

- 2026-05-23: Initial draft (S102 close). Sources: PixelLab local docs, official web docs check, MCP docs, RIMA S99-S102 learned patterns.
