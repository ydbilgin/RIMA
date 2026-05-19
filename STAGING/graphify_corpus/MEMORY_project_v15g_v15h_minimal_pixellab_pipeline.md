---
name: v15g-v15h-minimal-pixellab-pipeline
description: "**[STALE 2026-05-18 — superseded by map-plan-v1-lock]** 2026-05-18 — Q2 FULL PixelLab test path. v15g clean redesign + Wang v2 + v15h playable map dispatch. Codex hybrid LOCK rollback safety preserved."
metadata: 
  node_type: memory
  type: project
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

> **SUPERSEDED 2026-05-18 S91 by [[map-plan-v1-lock]]:** v15h tile composition pipeline production'dan DEPRECATED edildi. Visual layer = monolithic painted background per RoomTemplate (Hades model, Option C). v15h composer kept ONLY for procedural Unknown room + test scaffold. Bu memory archive-eligible.

# v15g + v15h — Minimal PixelLab Pipeline Test (2026-05-18)

**Trigger:** User feedback after v15d/v15e screenshots — "şu an mapde çok kalabalık şeyler var bunları kaldıralım. pixellabdakilerle tekrar dizayn edelim bizim mantığımızda temiz şekilde". Plus Codex Q2 verdict (FULL test, MED confidence).

## v15g (LIVE)

**Scene:** `Pro_Redesign_v15g_Minimal_PixelLab_CombatRoom` (RoomPipelineTest.unity)
**Profile:** `Assets/Data/Blueprint/Profiles/profile_v15g_minimal_pixellab.asset`
**Tiles wired:** PixelLab `create_tiles_pro` v1 cobble pool + PixelLab dirt v1 pool
**Composition:** v15d budget STRICTER (75/18/7 floor, cluster cap 2, neg space 25%, L8 cap 5)

**Screenshot:** `Assets/Screenshots/PlayableRoom_combat_v15g_minimal_pixellab_LIVE.png`

**Result:**
- ✓ Filigree pattern GONE (clean cobble + dirt)
- ✓ Purple crystal scatter 0 (no rune/crystal pool referenced)
- ✓ Atmospheric mist 0 (no L8 pool)
- ✓ Player silhouette pop net
- ⚠️ **TOO SPARSE** — ~50% cell coverage, large black gaps
- ⚠️ Hard tile cell boundaries visible (no Wang transitions)

## v15h (in flight, dispatched 2026-05-18)

**Goal:** Fix v15g sparse + add Wang transitions + make PLAYABLE.

**Codex dispatch:** `bty2oujf8` (laurethayday, xhigh, 1-2 hr ETA)
**Task file:** `STAGING/CODEX_TASK_v15h_playable_map.md`
**DONE marker (expected):** `STAGING/CODEX_TASK_v15h_playable_map_DONE.md`

**Scope:**
1. Import Wang v2 32×32 (`STAGING/pixellab_wang_v2_32px/wang_dirt_cobble_32px.png`) as Unity Tile assets + RuleTile
2. Wire RuleTile to profile_v15h zones (dirt↔cobble smooth edges)
3. Zone density 0.50 → 0.85 (fill black gaps)
4. AdjacencyRuleSO.decalsPerRoomCap = 8 (prevent rune circle scatter)
5. Spawn Warblade anchor in scene (canonical `2656075d`)
6. Render `PlayableRoom_combat_v15h_playable_LIVE.png` + PlayMode WASD test

## Comparison v15 series progression

| Version | Screenshot | Wins | Gaps |
|---|---|---|---|
| v15c | chaos | nothing | everything |
| v15d | composition LOCK | budget enforced | mist cover-up |
| v15e-A | L8 cap | mist 67→32, player visible | purple crystal scatter |
| v15e-B | secondary cluster cap | crystal 7-8 → 2-3 | blue rune circles still scatter |
| **v15g** | minimal PixelLab clean | NO rune NO crystal NO mist, clean tile | TOO sparse, hard edges |
| **v15h** | playable target | Wang smooth + 85% fill + playable | TBD |

## Rollback safety (Codex hybrid LOCK preserved)

v15d Codex gpt-image-1 tiles + zone .asset files **UNTOUCHED**. If v15h fails or full PixelLab test underperforms:
- Switch back to v15d hybrid (Codex tiles + PixelLab chars/props)
- v15g/v15h scenes can be deactivated, not deleted

This satisfies Codex's Q2 verdict ("keep v15d as rollback, do not keep splitting providers until full PixelLab Wang test fails").

## Decisions LOCKED in this pipeline

| Q | Verdict | Confidence |
|---|---|---|
| Q1 Oval feel | **E (Wang autotile + organic decal)** | HIGH |
| Q2 Full PixelLab vs Hybrid | **FULL test PARALLEL** (Opus modified — keep v15d rollback) | MED |
| Q3 Unity render approach | **v15f/g/h: Standard Tilemap** · Phase 1.5: chunked sprite mesh | HIGH |

## PixelLab tile assets available (current)

| Asset | Source | Status |
|---|---|---|
| cobble v1 (16 var) | `create_tiles_pro` `d625d42a` | ✓ in `STAGING/pixellab_tiles_pro_pilot/` + Unity import |
| dirt v1 (16 var) | `create_tiles_pro` `227d5510` | ✓ in `STAGING/pixellab_dirt_v1/` + Unity import (Codex copied 4 unique into 16 slots) |
| Wang v2 32×32 (dirt→cobble) | `create_topdown_tileset` `4235c9c1` | ✓ in `STAGING/pixellab_wang_v2_32px/` — pending Unity import in v15h |
| Wang v1 16×16 | `create_topdown_tileset` `aa7ca5bb` | ⚠️ TOO SMALL, dropped (had grass drift) |
| Transition pro tiles v3 | `create_tiles_pro` `e918995f` | ⏳ STUCK 49% queue, may not complete |

## Next steps (after v15h DONE)

1. Visual review v15h vs v15g (same scene base, with Wang + density)
2. PlayMode WASD test (Warblade + sword 96)
3. If v15h clean + playable → LOCK as "current baseline"
4. Then: Codex weapons spec resume (priority order) + character animations (Warblade first)
5. Phase 1.5 RoomData refactor (draft spec in `STAGING/PHASE_1_5_ROOMDATA_SPEC_DRAFT.md`)

## Related

- [[v15d-composition-budget-lock]] — base composition rules
- [[hybrid-asset-pipeline-lock]] — Codex tile rollback + style fallback
- [[5000-pixellab-allocation-lock]] — budget LOCK
- [[canonical-character-roster-v2]] — 10 anchor IDs
