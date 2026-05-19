---
name: karar-143-layered-pipeline
description: Karar
metadata: 
  node_type: memory
  type: project
  originSessionId: 90de2c61-1b77-4aa4-bfe5-406a7748a183
---

# Karar #143 Layered Pipeline — LOCK 2026-05-16

**Status:** Aşama 1 LOCK + Aşama 2 LOCK (Codex 156 PASS Unity EditMode runner).

## 6 Layer Mimarisi

L1 Floor Base (Tilemap 32x32) → L2 Floor Variation (Tilemap) → **L3 Wall Overlay** (SpriteRenderer Hades cap, 384x216 / 424x632 / 341x341 — RIMA EXTRA) → L4 Transition Brush (256/512 oval) → L5 Detail Decals (32-128) → L6 Accent (rift sparse).

**Wang tileset = SADECE feature edge (water/cliff/elevation), Karar #143-B.** Wall asla tileset değil (143-C).

## ChatGPT 5-layer Entegrasyon (143-M..R, 2026-05-16)

| Karar | Madde | Field/SO |
|---|---|---|
| 143-M | allowFlipX/Y random | `PatchEntry.allowFlipX/allowFlipY` |
| 143-N | minDistance tile-arası | `PatchEntry.minDistance` |
| 143-O | sortingOrderRange random | `PatchEntry.sortingOrderRange` (Vector2Int) |
| 143-P | encounterAvoidRadius | `PatchAtlasSO.encounterAvoidRadius` |
| 143-Q | centerPathDensityReduction explicit | `PatchAtlasSO.centerPathDensityReduction` |
| 143-R | 8 PixelLab prompt template (A-H) | `STAGING/asset_gen_asama1_batch.md` |

**Why:** ChatGPT explicit min-distance + flip + avoid-zones gameplay readability + tile repetition gizleme için kritik. Eskiden density curve dolaylı yapıyordu, şimdi atlas-level tunable.

**How to apply:** Yeni PatchAtlasSO asset oluştururken:
- Transition (L4): density 0.08, edgeBiased, wallProximityFactor 1.5, minDistance 6, allowFlipX, centerReduction 0.05
- Detail (L5): density 0.18, edgeBiased, wallProximityFactor 1.2, minDistance 2, allowFlipX+Y, centerReduction 0.08
- Accent (L6): density 0.03, edgeBiased=false, encounterAvoidRadius 4, minDistance 12

## Kodlama Kuralları

- Walkable filter (143-D) — TÜM painter'larda ilk check. Wall cell'e patch düşmez.
- `MapLayerOrchestrator.Paint(floorTilemap, biome, room, seed)` 6 layer toggle (LayerToggles struct).
- L4/L5/L6 painter'lar `TransitionBrushPainter`'dan inherit (PaintAtlas shared).
- Aşama 2 feature mask: `NaturalFeatureGraph.SampleFeatureProximity(cell, room.naturalFeatures, room.size)` density'ye multiplier.

## Asset Wireup

`Assets/Art/Tiles/F1/` (Shattered Keep):
- WallBrushSetSO → L3 14 sprite (horizontal/vertical/corner/doorway)
- TransitionAtlas → L4 6 oval moss/dirt + 2-3 large biome
- DecalAtlas → L5 9 (3 reuse mevcut + 3 crack + 3 rubble)
- AccentAtlas → L6 3 rift fracture/bloom/scar

Hero props (STUDIO_KARAR_009 aday) 3-5 anchor prop biome başına ayrı sistem.

## L3 Wang Full 16 Corner Set (S86 PREP-3 LOCK)

**Why:** Eski 7-type semantic plan (horizontal/vertical/4 corner/doorway) Wang topolojisini eksik modelliyordu. 4-bit corner encoding 16 case = tam set. Bu LOCK onceki L3 wall planini override eder.
**How to apply:** L3 wall asset uretirken sadece Wang 16 schema kullan. SliceLayoutTemplateSO = `L3_Wang16_Topdown.asset`. 7 eski tip artik referans alinmaz.

**IPTAL (eski plan):** 7 semantic type — horizontal / vertical / corner_NE / corner_NW / corner_SE / corner_SW / doorway

**YENİ LOCK: Wang Full 16 corner set**
- Encoding: Her hucre 4 kosesinin (NE/NW/SE/SW) wall|floor durumu → 2^4 = 16 case
- Tag pattern: `wang_{NESW_bitstring}` (ornek: `wang_0011` = south edge, `wang_1101` = U-shape open E)
- Bucket: tum Wang tiles **Micro** (32x32) → L3 tile-grid placement
- heroAllowed: FALSE (L3 Wang non-hero)

**Uretim: PixelLab MCP `create_topdown_tileset`**
- Mode: Lower (floor) → Upper (wall) chain, `transition_size 1.0` (full wall transition)
- tile_size: 32
- Output: 16 base tile + transition slice
- SliceLayoutTemplate: `L3_Wang16_Topdown.asset` (wangAware = true)

**Vertical slice scope:** Sadece 1 tileset (ShatteredKeep biome, Floor → Wall basic). 4 ek biome DEFER → vertical slice loop PASS sonrasi.

**Fallback (Wang generator FAIL):** Strategy A explicit — 16 ayri 32x32 PNG dispatch. BrushAtlasValidator FAIL detector tetiklenirse otomatik fallback hint user'a gosterilir.

## Cross-link

- LaurethStudio: [[layered-environment-pipeline]] STUDIO_KARAR_003 (universal pattern)
- Baglanti: [[project_map_system]] (STS2 DungeonMapUI ust seviye flow), [[reference_pixellab_knowledge_map]] (endpoint listesi)
- Sprint 9-13: [[brush-tool-v1-design]] [[room-library-architecture]]
