---
name: hybrid-asset-pipeline-lock
description: "S87 LOCK — Hybrid asset production: PixelLab discrete sprites + Codex imagegen tile/decal/wall sets. Karar #157 candidate."
metadata: 
  node_type: memory
  type: project
  originSessionId: 20cf7214-b515-4cce-814f-df3b0dd176f2
---

# Hybrid Asset Production Pipeline — LOCK (S87 2026-05-18)

**Karar:** #157 (candidate, user onayı sonrası LOCK)

## Pipeline Split

```
PIXELLAB (Create Image Pro V3 + Character States)
├── Character anchors (10 sınıf) — Karar #145 v2 state workflow LIVE
├── Mob anchors (Tier 1-3) — state workflow + enemy variant (Use #2)
└── Props (12+ discrete obje) — pixel-perfect, 16-variant pick

CODEX IMAGEGEN (gpt-image-1 backend)
├── L2 Floor tile set (6+ variant, seamless cohesion)
├── L3 Wall Wang16 (16-tile corner set, stylistic unity)
├── L4 Organic decals (moss, dirt, vegetation patches)
├── L5 Detail elements (cracks, pebbles, bones scatter)
└── L6 Accent overlays (rift scar, blood splatter, scorch — büyük)
```

## Why This Split

| PixelLab güçlü | Codex güçlü |
|---|---|
| Character States workflow (Karar #145 v2 — 6 use case) | Stil tutarlılığı tek dispatch'te (16 Wang16 set) |
| Pixel-perfect discrete sprites (64×64 chibi) | Painterly atmospheric mood (v1 quality) |
| 16-variant grid pick per asset | Edge-cohesion (tile-to-tile transitions) |
| Animation states production | Tek session batch tile/decal pack |
| Native size variant (Custom Size Beta) | Doğal görünüm (L4-L6 organic decals) |

## Pipeline'a Map'lemek

- PixelLab Output → PropDefinitionSO → PropPlacer (Sprint 12)
- Codex Output → AssetPool / BrushDefinition → Brush Editor paint (Brush V1 main)

İki branch, ikisi de Brush V1 Editor'da birleşir (Tabs: Brush + Props).

## Image Generation Model

- **Codex CLI `imagegen` skill** → gpt-image-1 backend (verified S87 via v1/v2 dispatches)
- v1 painterly mood: ✅ achieved (concept_art_rima_sample_room.png)
- v2 sharper pixel art: ✅ achieved (concept_art_rima_sample_room_v2.png)
- 8-panel layer breakdown: ✅ pipeline truth proven

## Eski drift'li tileset durumu

**`Assets/Sprites/Environment/StoneDungeon/`** (eski) — Karar #100 uyumsuz, isometric drift'li wall + floor tile'lar. Yeni v2 set üretilince eski set `_archive_v1/` altına taşınacak.

## Budget Considerations

**Codex imagegen cost:** Her image ~$0.04-0.16 (model'e göre). Tile set (6 floor + 16 wall + 12 decal/accent) = ~34 image = ~$2-5.
**PixelLab cost:** 5000 gen budget yarın geliyor. Character + Prop için kullanılacak (~500 gen tahmin).

## Cross-link

- Live prop guide (PixelLab v6): `STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md`
- Tile set Codex task: `STAGING/codex_imagegen_stonedungeon_v2_tiles.md`
- v1/v2 concept reference: `STAGING/concept_art_rima_sample_room*.png`
- Layer breakdown proof: `STAGING/concept_art_layer_breakdown.png`
- Real tileset drift demo: `STAGING/RIMA_REAL_TILESET_ROOM.png`
- Laureth Studio pipeline knowledge: `F:/LaurethStudio/01_PIPELINE/pixellab_production_knowledge.md`

## Style-Consistency Fallback (2026-05-18 ADDED)

Hibrit LOCK production-proven AMA stil tutarlılık check'i zorunlu — Codex tile output PixelLab char/prop ile yan yana doğal mı görünüyor?

**Trigger conditions** — 2+ TRUE olursa "revisit hybrid LOCK" memory'ye yazılır + PixelLab tile pilot dispatch:

1. PixelLab top-down tile kalitesi Codex'i geçer (corner/transition/wall/decal dahil)
2. Codex tile output `process_tiles` workflow'undan sonra hala fail eder
3. Unity import time Codex > PixelLab total (gen + cleanup)
4. PixelLab biome family consistent + character/mob/prop budget'ını tehdit etmez
5. Locked visual direction tek-provider stil zorunluluğu çıkarır + mevcut hibrit görünür şekilde çatışır

**İlk check noktası:** v15d screenshot (`PlayableRoom_combat_v15d_composition_LIVE.png`) — Codex tile + PixelLab character/prop yan yana görüntü. Style drift > eşik → fallback path tetiklenir.

**Fallback path (eğer trigger olursa):**
- Free plan'da 15 tile MVP PixelLab pilot (`STAGING/autosprite_tile_inventory_pilot.md` referans)
- A/B kalite check Codex tile vs PixelLab tile
- Kazanan → hybrid LOCK revize edilir veya full PixelLab unify kararı

## Studio-Wide Application

Bu split future Laureth Studio oyunlarında base pattern:
- Roguelite (RIMA): PixelLab characters + Codex tiles ✅ live test
- Topdown ARPG: Aynı split uygulanabilir
- Tactical RPG: PixelLab unit sprites + Codex map tiles
- Puzzle/Cozy: PixelLab object stamps + Codex background art

Laureth Studio Memory'sine eklendi: `F:/LaurethStudio/01_PIPELINE/pixellab_production_knowledge.md` (Section X: Hybrid with Codex).
