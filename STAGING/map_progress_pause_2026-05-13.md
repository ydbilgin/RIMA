# Map Progress Pause — 2026-05-13

User dedi: "Gemini (Antigravity) üzerinden deneyecem, bekle, olmazsa baştan yaparsın."

## ŞU ANDA NEREDE KALINDI

### LOCKED palette (Karar #30 + #77 + #100 + #116 + #118 — NLM 30ddffa5'den):
- Floor: `#2C2A2A` (charcoal warm grey)
- Wall: `#4A3F3F` (slightly warm dark grey)
- Accents: cold blue `#7BA7BC`, rift cyan `#00FFCC`, torch orange `#C4682A`, blood red `#8B1A1A`

### Antigravity 4 LOCKED specs (uygulanmayı bekleyen 3):
1. ✅ **Y-Axis Sort applied** — `GraphicsSettings.transparencySortAxis = (0, 1, 0)`, Custom Axis mode
2. ⏳ **Drop Shadow Layer 1.5** — wall/prop altına otomatik shadow tile (Codex iter 2)
3. ⏳ **Elevation Front/Top face** — wall iki yüzü (Codex iter 2)
4. ⏳ **1px Border outline** — Wang seam crisp line (Codex iter 2)

### Generated assets (Keep biome, charcoal palette):
- `Assets/Art/Tiles/Keep/Floor/` — 25 wang tiles (4c962284 tileset, ragged 1.0) + `floor_base_tile.asset` + `floor_rift_tile.asset`
- `Assets/Art/Tiles/Keep/Walls/` — `wall_0..3.png` + `_tile.asset` (collider Sprite)
- `Assets/Art/Tiles/Keep/Decals/` — 8 decals (crack/moss/rift) + `_tile.asset` (after chroma-key transparent)
- `Assets/Art/Tiles/Keep/Keep_Combat.asset` — RimaRoomBaselineTemplate, singleFloorTile=floor_base, accents=[floor_rift], singleWallTile=wall_0, wallVariants=[wall_0..3], decalSprites=8

### Code (Karar #122 iter 1):
- `Assets/Scripts/Systems/Map/LayeredRoomGenerator.cs` — CA cave (B5678/S45678, flood-fill, min 40% floor)
- `Assets/Scripts/Systems/Map/LayeredRoomPainter.cs` — single floor + accent ratio + single wall paint
- `Assets/Scripts/Systems/Map/RimaRoomBaselineTemplate.cs` — singleFloorTile, floorAccentTiles, floorAccentRatio, singleWallTile, useLayeredGenerator fields
- `Assets/Scripts/Demo/RoomPipelineTestController.cs` — `[ContextMenu] 0. Generate Layered (Karar #122)` method

### Scene state — `RoomPipelineTest.unity`:
- 32×20 cell CA cave room generated
- Grid cellSize = `(0.5, 0.5)` (PPU=64 uyumlu, gap'sız)
- Global Light 2D intensity = `1.0` warm white
- ScatterRoot mevcut, scatter children CLEARED (son adımda — user "saçma sapan" dedi, temizlendi)
- DecalsTilemap = 36 cell painted (transparent decals)
- BaseTilemap = jittered (rotation/flip + HSV ±%4V ±%3H, Karar #123)

### Bekleyen Codex iter 2 spec (henüz dispatch edilmedi):
- Oval/Circle brush mode (radius 1-8 cell)
- Soft brush (falloff for decal serpme)
- Multi-layer tilemap stack (Base / Decal / Wall / Prop)
- Hook LayeredRoomGenerator → RoomDesigner Generate button
- Fix canvas hover bug (preview camera ray, not Scene View ray)
- Populate tile library left panel (template thumbnails)
- Antigravity #2 Drop Shadow Layer 1.5
- Antigravity #3 Wall Front/Top face elevation
- Antigravity #4 1px border outline on Wang seam
- Karar #123 polish: dust mote particles, URP 2D torch flicker, sub-tile cross-cell fragments

### Background agent çıktıları (referans):
- `STAGING/natural_map_techniques_research.md` — Slynyrd Pixelblog 20/43, HLD/Hades/Loop Hero analizleri, Unity 2D Light setup link'leri
- `STAGING/karar_123_natural_polish_additions.md` — 5 polish öneri (per-tile rotation+flip ✓, HSV jitter ✓, lighting, fragments, dust)
- `STAGING/room_designer_qc_plan.md` — 14 partial PASS/FAIL test plan
- `STAGING/character_anim_state_graphs.md` — 10 sınıf state graph + paragraf prompts
- `STAGING/alabaster_layered_pixellab_prompts.md` — eski prompt'lar (artık reference)
- `STAGING/alabaster_reference.png` — user'ın paylaştığı Alabaster Dawn screenshot

### Bilinen sorunlar:
- Floor sprite texture'da vertical hatching pattern var — PixelLab yorumu (regenerate gerekirse prompt'a "no hatching, smooth stone surface")
- Scatter sprites çok büyük scale, çok yoğun → düşük density + smaller scale gerekir
- Room Designer canvas → scene leak bug (RoomDesignerCanvas.UpdateHoveredCell preview camera kullanmalı)
- DecalPainter `bp` (RoomBlueprint) null olduğu için atlatıldı — inline paint kullanıldı geçici

### Gen budget kalanı: ~250-300 (800'den)

## Devam Etmek İçin

Eğer Gemini denemesi sonrası dönülecekse:
1. Bu MD'yi oku
2. `Assets/Scenes/Demo/RoomPipelineTest.unity` aç
3. ContextMenu "0. Generate Layered (Karar #122)" çalıştır → CA cave
4. Codex iter 2 task drafti (bu MD'deki spec listesinden) yaz + dispatch

Bekliyorum.
