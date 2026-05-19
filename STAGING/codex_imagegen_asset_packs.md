# Codex Task — Asset Pack Visualization for v1 and v2

## Gorev
User v1 ve v2 concept art'larin **yapitaslari**'ni gormek istiyor — uretilecek her assetin ayri sprite gorunumu + ust uste binerek final image'i nasil olusturdugu.

## Reference Final Images
- v1: `STAGING/concept_art_rima_sample_room.png` (more painterly, atmospheric, "rich detail")
- v2: `STAGING/concept_art_rima_sample_room_v2.png` (sharper pixel art, more pipeline-true)

## Hedef: 2 Asset Pack Image + 1 Stacking Demo

### Image A: v1 Asset Pack (painterly style)
Compositional reference card showing ALL the assets that compose the v1 final image.

Layout: 4x4 grid of asset sprites, each labeled.

**13 Assets to show (each rendered in v1 painterly style):**
1. **L2 Floor Tile** (64x64) — weathered stone, dark gray with subtle variation, single seamless tile
2. **L3 Wall Tile** (64x96) — Wang16-style stone block wall, top edge cap visible
3. **L4 Moss Patch** (96x64) — irregular oval, deep moss green organic blob, semi-transparent edges
4. **L5 Cracks** (small) — pixel art crack lines, irregular pattern
5. **L5 Pebbles + Bones** (32x32) — scatter element
6. **L6 Rift Scar** (128x128) — large dark crimson irregular oval, multi-blob organic, radial crack lines, mood: catastrophic
7. **Wooden Crate** (64x64) — weathered dark brown planks, iron banding, brass rivets
8. **Stone Urn (broken)** (64x64) — dark gray weathered stone, hairline cracks, top rim chipped, hollow interior
9. **Candle + Iron Holder** (32x48) — tall wax candle, warm orange flame, cast iron base
10. **Burning Brazier** (64x96) — iron three-legged base, dish with ember coals, contained warm glow
11. **Hanging Banner (torn)** (64x96) — dark crimson cloth, frayed edges, faint emblem
12. **Stone Column (intact)** (64x128) — tall thin temple column, weathered, moss base
13. **Character: RIMA Ranger** (64x64 chibi) — adult female, tan skin, bleached-ivory ponytail, dark forest green asymmetric armor, weapon-free idle pose, big-head chibi

Each asset on transparent or dark background. Clean labeled grid.

Caption header: "v1 Asset Pack — Painterly Concept Style"

### Image B: v2 Asset Pack (sharp pixel art, pipeline-true style)
Same 13 assets but rendered in **sharper pixel art**, more like what PixelLab v6 prompts would actually produce — 64x64 chibi pixel discipline, hard edges, max 2 tones, less painterly. This is **closer to the real game output**.

Caption header: "v2 Asset Pack — Sharper Pixel Art (Pipeline-True)"

### Image C: Stacking Demo
A single wide image showing **horizontal progression** of how the assets stack into the final room:

```
[Empty Room] -> [+ Floor] -> [+ Walls] -> [+ Moss] -> [+ Detail] -> [+ Rift] -> [+ Props + Char] -> [+ Lighting = Final]
```

8 panels horizontal, each showing the same room view progressively built up. Final panel = v1 or v2 final reproduction.

Caption header: "Layer Stacking - How Assets Compose Into Final Room"

## Style Direktifleri

### Image A (v1 painterly)
- Atmospheric, rich detail, mood-lit
- Painterly pixel art with concept-art polish
- Same Vivid Vulnerability palette
- Each asset feels "concept art quality"

### Image B (v2 sharp)
- Hard pixel edges, max 2 tones per color
- True 64x64 chibi pixel art discipline
- Same palette but more discrete colors
- Each asset feels "game-engine pixel sprite"

### Image C
- Same view in all 8 panels (consistent camera/composition)
- Show layer additions progressively
- Same style throughout (pick v2 style for consistency)

## Negative
- NO text inside assets (labels OUTSIDE the asset boxes)
- NO weapons in character hands
- NO modern UI
- NO grid lines visible in the final stacked panel
- NO assets stretched or distorted

## Output
- `STAGING/asset_pack_v1_painterly.png` (1920x1080 or 4x4 grid composition)
- `STAGING/asset_pack_v2_pixel_art.png`
- `STAGING/layer_stacking_demo.png` (wide horizontal, 1920x540 or similar)

## Pipeline Sahibligi (Codex feedback gerekli)
v1 painterly style **bizim PixelLab v6 prompt'larimizla** uretilebilir mi gercekten? Yoksa v2 daha gerceci mi?
Codex `codex_imagegen_asset_packs_DONE.md` icinde 2-3 cumle ile bunu degerlendirsin.

## Reference Path (Codex inspect)
- v1 final: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concept_art_rima_sample_room.png`
- v2 final: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concept_art_rima_sample_room_v2.png`
- Character anchor: `F:/Antigravity Projeler/2d roguelite/RIMA/ANCHORS/characters/04_ranger.png`
- Prop production prompts: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md`

## Sonuc
User assetlerin **ayri ayri ve birlesik** halini gorerek "üst üste boyamak" mantigini somut sekilde gorur. v1 vs v2 farki da net olur.
