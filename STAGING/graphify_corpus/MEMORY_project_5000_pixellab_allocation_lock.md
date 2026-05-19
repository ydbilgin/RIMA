---
name: 5000-pixellab-allocation-lock
description: 2026-05-18 LOCK after 5000 PixelLab gen refresh. Allocation across characters/mobs/props/VFX/UI/hazards/boss prep/reserve. Mid-checkpoint after 3150 spend. Opus+Codex synthesis.
metadata: 
  node_type: memory
  type: project
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# 5000 PixelLab Gen Allocation LOCK (2026-05-18)

**Trigger:** User credit refresh — 5000 PixelLab generations available. Question: "where best to spend?"

**Process:** Opus initial proposal → Codex independent counter-proposal → Opus synthesis. See `STAGING/CODEX_DONE_5000_ALLOCATION_REVIEW.md`.

**Key principle (Codex pushback):** Reserve > 50% = decision paralysis. Allocate aggressively to critical undefined categories, mid-checkpoint after first 3150 spend.

## Allocation table

| Category | Gen | % | Rationale |
|---|---:|---:|---|
| **Characters** (10 anchor + state + portrait) | 900 | 18% | 10 × 90/anchor — outfit + weapon + facing + idle/run/attack + state variants + portrait crops + regen for misses. Codex pushed back on Opus initial 500 (too tight). |
| **Mobs** (8 roster + readability variant) | 600 | 12% | Combat silhouette critical. Imp→Hulk + elite/size readability passes. |
| **Hero props** (cluster templates + biome variants) | 400 | 8% | v15d composition needs authored cluster anchors, not random scatter. Focal/support/blocker variants per biome. |
| **VFX provider ladder** (multi-candidate A/B) | 250 | 5% | Dash trail + hitspark + 1 class skill across PixelLab/autosprite/Codex. Not just token pilot. |
| **Item icons + pickups** | 200 | 4% | Skill offers, drops, class identity — missing from initial Opus list. |
| **HUD / skill icons** | 150 | 3% | 10 classes × ~3 skills minimum identity language. |
| **Environmental hazards** | 150 | 3% | RIMA has rift cracks already; expand carefully (spikes, cursed pools). |
| **Boss silhouette prep** | 200 | 4% | Exploration only — NOT full production (S90+). Lock scale + style early to avoid late shock. |
| **Reserve / regen / fail buffer** | 2150 | 43% | Codex pushed back on Opus 3700 (paralysis). 43% = healthy buffer without postponing direction-setting. |
| **TOTAL** | **5000** | **100%** | |

## Mid-checkpoint rule

After **first 3150 spend**, mandatory review:
- Failure rate per category (style misses, regen needs)
- Style drift check (PixelLab vs Codex tiles)
- Scope adds/removes
- Budget reallocation

Locked: do not exceed 3150 without explicit mid-checkpoint pass.

## Pipeline constraint

**Hybrid LOCK active:** PixelLab budget = characters, mobs, props, VFX. Tile/wall/decal generation stays on Codex gpt-image-1 ([[hybrid-asset-pipeline-lock]]).

**Switch trigger** (revisit hybrid LOCK): if any 2+ of these become TRUE:
1. PixelLab top-down tile quality > Codex (corner/transition/wall/decal)
2. Codex tile output fails `process_tiles` workflow repeatedly
3. Unity import time Codex > PixelLab total (gen + cleanup)
4. PixelLab biome family consistent + doesn't threaten character/mob/prop budget
5. Locked visual direction demands one-provider style + current hybrid visibly clashes

## Phase order — parallel workstreams

| Phase | Codex/Unity | PixelLab | Codex imagegen | VFX |
|---|---|---|---|---|
| **W1** | v15d composition LOCK (SO + AutoPopulator + metrics + tests) | 3 priority anchors (Warblade + Elementalist + Gunslinger) | v15d tile assets (combat biome floor + path + decals) | — |
| **W1-2** | v15d screenshot + metrics validation | 10 anchor silhouette lock | Hero prop cluster gpt-image-1 | — |
| **W2-3** | Style consistency check | Mob roster 4-8 + hero prop variants | — | A/B (PixelLab + autosprite + Codex) dash trail + hitspark + 1 skill |
| **W3+** | v15e tuning | Item icons + HUD icons | Hazard tile prototypes | Skill VFX expansion + telegraph |
| **S90+** | Shader blending + full POI + layer collapse | Boss silhouette + portrait expansion | Cinematic decals | Boss VFX |

## Phase 1 anchor selection (locked)

| Class | Why Phase 1 |
|---|---|
| **Warblade** | Exists already; state tweaks per [[character-state-tweaks-pending]]; fastest proof-of-concept |
| **Elementalist** | Signature class, baseline style ref for other 7; multi-element identity test |
| **Gunslinger** | User-approved outfit decision; long-range combat silhouette validation |

Locked 3 anchors → silhouette + style LOCK → other 7 anchors batch.

## Deferred categories (S90+)

- Shader-based biome blending (v15d 3-agent REJECT)
- Full POI capacity system (v15d 3-agent MODIFY, cluster cap is lightweight version)
- Boss production (silhouette prep only in this allocation)
- Cinematics
- Multi-style character bakeoff (could pilot 1-3 anchors if budget allows)

## Related

- [[v15d-composition-budget-lock]]
- [[hybrid-asset-pipeline-lock]]
- [[character-state-tweaks-pending]]
- [[canonical-character-roster-lock]]
- [[pixellab-character-states-workflow]]
- [[pixellab-character-via-web-ui-v3]]
- [[codex-parallel-profile-workflow]]
