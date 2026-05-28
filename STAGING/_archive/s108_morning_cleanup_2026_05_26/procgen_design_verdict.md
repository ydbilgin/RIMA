# RIMA Procgen Design Verdict

**Author:** rima-design (Opus 4.7)
**Date:** 2026-05-23 (S102)
**Pairs with:** `STAGING/architecture_decision.md` (Option C lock), `STAGING/codex_arch_review.md`
**Parallel to:** rima-research (video/channel survey)

---

## TL;DR

- **L1 dungeon graph:** Hades-style **handcrafted graph + weighted room-slot picker**. NOT BSP, NOT WFC. RIMA has 8-15 chunk rooms per run, not 50+ tile cells — graph algorithms are wrong granularity.
- **L5 asset procgen:** SKIP WFC and "cut a giant LoRA painting into pieces." Both fight the locked template+overlay architecture. LoRA already IS the asset procgen.
- **PixelLab + Edit-Image refine workflow:** ADOPT NARROWLY for L3 decor-prop variants (urns, debris, breakable props) and floor-region patches inside templates. SKIP for templates themselves (LoRA wins).
- **Cellular automata:** SKIP for L1, RESEARCH_MORE for L5 rift-vein crack pattern generation (one tactical use case).
- **3D→2D render:** Skip. Wrong pipeline, kills LoRA style coherence, solo dev cost prohibitive.

The procgen story for RIMA is **shallow on purpose**: 90% of variation comes from authored anchor permutations + lighting variants + mirror flip, and 10% from a tiny dungeon-graph generator. WFC/BSP/CA are roguelite folklore but solve problems RIMA does not have.

---

## Per-question verdicts

### 1. WFC for L1 (dungeon layout) — **SKIP**
WFC is a tile-constraint propagator. RIMA's L1 is a **graph of 8-15 rooms**, not a constraint surface. Running WFC on room nodes works but is massive overkill — at this node count, a 50-line weighted random walk produces the same output. WFC pays off at 100+ cells with rich adjacency rules; RIMA has neither.

### 2. WFC for L5 (asset-level) — **SKIP**
The template+overlay lock already commits style coherence to one LoRA. WFC-generated tile patterns would be a SECOND visual source competing with the LoRA's training distribution — exactly the style-drift failure we already paid for once with PixelLab default style vs Shattered Keep palette. The 25 PixelLab tilesets on account are mostly **floor variation candidates only** (see #4), not WFC inputs.

### 3. "Generate large pattern, cut sections" — **SKIP**
Tempting but wrong. A 4096×4096 LoRA painting has ONE composition — cutting it into 16 sections gives 16 fragments of the same composition, not 16 distinct rooms. Each cut would also need:
- Manual wall-path collider re-tracing per cut (negates the speed win)
- Manual anchor placement per cut (same cost as a fresh template)
- Edge healing where cuts intersect baked decor

Compared to "LoRA-generate 6 templates at 1024×1024 each" — the cuts approach gives MORE assets but LESS variety per asset. The 6-template plan already produces hundreds of room permutations via lighting × mirror × anchor swap. Cutting buys raw count we don't need.

**Exception:** If LoRA gets unstable at 1024×1024 and we have to gen at 2048×2048 then downsample, that's a quality workaround, not procgen.

### 4. PixelLab Wang + Edit-Image refine — **ADOPT NARROWLY**
This has real value but ONLY at two layers:

**L3 — Decor object variants (ADOPT):** PixelLab `create_8_direction_object` or n_frames batches → Gemini/Flux Kontext edit toward RIMA palette → import as decor sprites. Could meaningfully expand the 28-decor list to 40-50 at low cost. Strong fit for breakable props (urns, crates, candelabra) where PixelLab's strength shines and LoRA's full-scene strength is wasted.

**L3 — Floor patch tiles inside templates (ADOPT):** The 25 existing PixelLab topdown_tilesets (dark slate, weathered granite, dark rubble, painterly mauve) ARE Shattered-Keep palette-adjacent. Edit-Image pass to tighten saturation and lighting, then use as **floor-region patches** painted over LoRA template floor via RimaWorldPainterWindow. This is exactly the role S101 wall sheets play, extended to floors.

**L2 — Templates themselves (SKIP):** Wang tiles + edit-image cannot match a LoRA's composition coherence. Templates must stay LoRA-only.

**Net:** PixelLab+edit pipeline is a **second-tier asset stream** feeding decor and floor patches, not templates. Useful, bounded, no architecture change.

### 5. BSP / random walk for L1 — **ADOPT (random walk only)**
BSP partitions a 2D plane into rectangles. RIMA L1 isn't a plane — it's a graph. Random walk on a small node grid (e.g., 5×5 logical grid, walk produces 10-13 room slots) is the right primitive. Specifically:
- Walk produces ordered slot list (start → boss)
- Each slot gets a TYPE tag (combat / elite / shop / rest / boss) via Hades-style hand-tuned distribution
- Each slot's type drives template-pool filtering at L2

50 lines of code. No BSP needed.

### 6. Cellular Automata — **SKIP L1, RESEARCH_MORE L5**
- L1: RIMA is dungeon, not cave. CA solves cave morphology. Wrong tool.
- L5: One legitimate use case — **rift-vein crack pattern generation**. Generating organic crack distributions across template floors at runtime is exactly what CA does well. Could replace some of the 4 hand-authored rift-vein decor sprites with CA-generated crack meshes. Defer until rima-research confirms a clean Unity 2D implementation path exists; otherwise stick with 4 authored sprites.

### 7. 3D → 2D render — **SKIP**
One reason: kills LoRA style coherence. Every 3D render introduces lighting/material that doesn't match the trained Flux-LoRA painterly chibi pixel style. Two pipelines = style drift = visible inconsistency in a close-up camera. Solo dev cannot maintain both.

### 8. L1 minimum-viable algorithm — **VERDICT**

```
function buildRunLayout(seed):
    slots = randomWalk(gridSize=5x5, length=10-13)
    typeSequence = [combat, combat, elite, combat, rest,
                    combat, elite, shop, combat, boss]  // Hades-style
    for slot, type in zip(slots, typeSequence):
        slot.templatePool = filterTemplates(type, biome=Act1)
        slot.lightingVariant = pickWeighted(seed)
        slot.mirror = bool(seed)
    return slots
```

- 10-13 rooms per run (Hades = 10-15; matches)
- Hand-curated type sequence per act (NOT random) — pacing matters more than novelty
- All actual variety comes from L2 (template pool pick) + L3 (anchor decor) + lighting/mirror

This is **40-80 LOC**, sits next to existing `RoomTemplate.cs` and `RoomDecorationSpawner.cs`, no new architecture.

---

## Recommended Procgen Stack for RIMA

| Level | Algorithm | LOC est. | Notes |
|---|---|---|---|
| **L1 dungeon layout** | Hand-curated type sequence + weighted slot picker | 40-80 | Random walk for slot positions only |
| **L2 template selection** | Filter by slot.type + biome, weighted pick | 20-40 | Trivial; lives in `RoomDecorationSpawner` or new `RunLayoutBuilder` |
| **L3 decor placement** | Already designed: `OverlayAnchor` weights + 50% mirror flip | Done | Add: per-anchor category-aware swap tables (already in arch decision) |
| **L4 encounter spawn** | Encounter banks tagged by slot.type, anchor-driven spawn positions | 60-120 | EncounterBank from S100 batch already exists — extend |
| **L5 asset-level** | LoRA generation (templates) + PixelLab+edit pipeline (decor/floor patches) | 0 (manual) | NO algorithmic procgen at asset level |

**Total new procgen code:** ~120-240 LOC, fits in `Assets/Scripts/Rooms/` and `Assets/Scripts/Run/`. Solo-dev tractable.

---

## What to research more after rima-research delivers

1. **Hades-style "heat" / difficulty modifiers on top of slot sequence.** If rima-research surfaces specific patterns from Hades layout reverse-engineering, we may want to add per-run modifier system to L1 — but only after MVP run loop works.
2. **Cellular Automata rift-vein at runtime in Unity 2D.** Specifically: can CA generate a connected crack texture per room and paint it via a sprite mask in URP 2D Renderer at acceptable cost? If yes, replaces 4 authored rift-vein sprites with infinite variation.
3. **PixelLab → Gemini-edit pipeline throughput.** Concrete numbers: how many decor variants per hour, and does the edit pass survive batch consistency? Affects whether decor count can grow from 28 → 50 in same dev time.

---

## Risks to flag

1. **Procgen scope creep.** Roguelite culture pressures devs toward "more procgen = better." For RIMA this is wrong: every layer of algorithmic procgen we add fights the locked template+overlay decision. Stay shallow on purpose.
2. **L4 encounter spawning is undesigned and the highest-risk procgen layer.** Spawn placement intersects collider geometry, lane-blocking risk (called out in codex_arch_review.md), and combat balance. If anchors carry encounter sockets that overlap blocker decor, the room is unplayable. Mitigation: anchor schema must separate `EncounterAnchor` from `DecorAnchor` with collision-class metadata; spawner validates non-overlap at runtime.
3. **Random-walk run layouts can produce backtracking dead ends.** Pure random walk on a grid often gives ugly snake-shaped runs. Mitigation: post-process the walk to enforce minimum branching factor 1.2 (every 3-4 slots, allow one optional side room). Cheap.

---

## Conflicts with Locked Rules: NONE

Consistent with:
- `STAGING/architecture_decision.md` Option C (extends rather than replaces)
- `STAGING/codex_arch_review.md` (no new auto-connect logic, no baked detail bloat)
- `project_topdown_pivot_lock.md` (no projection change)
- `project_modular_pipeline_lock.md` (PixelLab+edit pipeline IS modular Mod B + manual transition)
- `feedback_no_pixellab_night_autonomous.md` (any PixelLab calls in adopt list require web UI, user awake)

---

**Decision-maker:** rima-design (Opus 4.7)
**Next orchestrator step:** When user returns, present this verdict alongside rima-research's deliverable. After approval, dispatch rima-doc to fold L1/L4 design into a new `TASARIM/PROCGEN.md` or merge into existing run-loop doc.
