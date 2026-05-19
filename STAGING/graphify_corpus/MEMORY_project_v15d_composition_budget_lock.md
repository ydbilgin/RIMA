---
name: v15d-composition-budget-lock
description: "2026-05-18 LOCK from 3-agent verdict (Opus + Codex + Gemini). Map composition contract — negative space %, floor hierarchy ratio, hero cluster cap, palette discipline, path-first. Replaces v15c uniform-scatter chaos."
metadata: 
  node_type: memory
  type: project
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# v15d Composition Budget LOCK (2026-05-18)

**Trigger:** v15c maps looked chaotic (purple crystals + blue runes + skulls uniform-scattered, player silhouette lost in floor noise). User asked for 3-agent review based on @boona11 Isle Builder tweet showing clean composition.

**Verdict:** 3-agent consensus (Opus + Codex + Gemini) on 8 hypotheses — 5 ACCEPT + 2 DEFER + 1 MODIFY. See `STAGING/CODEX_DONE_BOONA_REVIEW.md` + `STAGING/GEMINI_DONE_BOONA_REVIEW.md`.

## The Budget Contract

| Field | Value | Range |
|---|---|---|
| Negative space ratio | 0.20 (20%) | 0.18–0.22 |
| Floor weights (dominant / secondary / accent) | 0.70 / 0.20 / 0.10 | strict |
| Hero prop cluster cap per room | 3 | 1–6 |
| Cluster size (props per cluster) | 2–5 | strict |
| Cluster buffer (no-prop cells around cluster) | 2 | 1–4 |
| Palette per zone (color families) | 8 | curated via pool selection, not runtime check |
| Path cell ratio | 0.15 (15%) | strict |
| Path minimum width | 2 cells | strict |

## Implementation surface

| File | Change |
|---|---|
| `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs` | Add composition budget fields with backward compat defaults |
| `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs` | Two-pass planner — pass 1 reserve (path + neg space), pass 2 place (floor + clusters) |
| `Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs` | Metrics output: neg %, floor split %, path cells, cluster count, layer totals |
| `Assets/Tests/EditMode/MapDesigner/` | ~10 new tests for budget enforcement |
| `Assets/Data/Blueprint/Zones/*.asset` | Each zone .asset filled with composition budget fields |

## Why these numbers

**Negative space 20%:** 3-agent agreement. Boona uses ~60% water but combat arena needs more terrain than island map. 20% = breathing room without losing tactical interest.

**Floor 70/20/10:** Hades-style hierarchy. One dominant identity per zone, controlled accents. v15c had 25/25/25/25 = no signal.

**Hero cluster cap 3, size 2-5, buffer 2:** Combat readability requires 1-3 focal points (Boona had 1 island + clusters). v15c had 15+ competing.

**Path 15% min, 2 cells wide:** Visual flow + dash lane. Combat rooms only; treasure/fillers can disable via `pathProtect = false`.

**Palette 8/zone:** Curated pool selection, not algorithmic. Boona used ~6-8, RIMA tolerates 8 for richer biomes.

## What this is NOT

- NOT a shader-based solution (H5 REJECT, defer S90+)
- NOT a full POI capacity system (H7 MODIFY — cluster cap is the lightweight version)
- NOT a layer collapse (8 layers stay; budget produces 3-tier visual hierarchy)
- NOT enforced on Mode B manual MCP composition (designer-authored rooms bypass budget)

## Layer hierarchy under budget

| Layer | Role | Budget treatment |
|---|---|---|
| L1 macro backdrop | sparse biome ton | minimal (sparse) |
| **L2 base floor** | **dominant 70%** | **dominantFloorPool** |
| L3 overlay | secondary 20% + accent 10% | secondary/accent pools |
| L4-L5 small/medium props | cluster contents | within hero cluster cap |
| L6 large hero props | cluster anchors | within hero cluster cap |
| L7 tall focal | cluster anchors (alt) | same cap, not separate |
| L8 atmospheric | mood scatter | minimal |

→ Visually reads as 3-tier hierarchy (Boona-equivalent).

## Two modes for designer

- **Mode A (Semantic + Auto):** Brush V1 paints zone, AutoPopulator generates layers per budget. Fast, rule-enforced. Default for new rooms.
- **Mode B (Manual MCP):** `execute_code` per-prop placement bypasses AutoPopulator. Hand-authored for hero rooms. Bypasses budget intentionally. See [[brush-v1-manual-composition-system]].

## Escalation triggers (post-v15d screenshot)

| Outcome | Action |
|---|---|
| Budget enforces, visual clean | KEEP, ship v15d |
| Budget enforces, still 5+ tiers reading | Tighten budget (75/18/7, cap=2, neg=25%) |
| Budget fails to enforce | Layer collapse refactor (8→5 layers) — S90+ |
| Style drift (Codex tiles vs PixelLab props clash) | PixelLab tile pilot trigger ([[hybrid-asset-pipeline-lock]] fallback) |

## Related

- [[blueprint-first-map-design]] — process LOCK (zone → prop → decal 3-step)
- [[layered-terrain-mandatory]] — 3-layer base fill rule
- [[room-composer-paint-intent-lock]] — semantic brush 3-mode
- [[brush-v1-manual-composition-system]] — Mode B manual MCP path
- [[hybrid-asset-pipeline-lock]] — tile provider LOCK + fallback trigger
