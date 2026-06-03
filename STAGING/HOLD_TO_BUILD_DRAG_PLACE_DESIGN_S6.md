# Press-and-Hold + Drag → Continuous/Auto Placement — Design Research & Ideas
**Source spark:** Conor Dart's factory game (X video, 2026-05-31) — transport belts are placed by **holding the mouse and dragging**; the belt auto-routes along the path and auto-orients its corners. The team wants this UX captured for **both RIMA (action-roguelite) and LaurethStudio (cozy farm/builder)**.
**Author:** Opus 4.8 (ideation) · ax/Gemini reference survey folded below when ready.

---

## 1. The pattern in one line
**Hold button → drag → system continuously places & auto-connects pieces along the path, showing a live ghost, committing on release.** Two sibling families:
- **DRAG-PLACE** (build/author): belts, fences, paths, tilled rows, walls, decor — auto-tiling (Wang) + auto-orienting corners.
- **HOLD-CHARGE/CHANNEL** (action): hold to charge a heavy, channel a beam, auto-fire, or hold-to-interact.

Same input grammar ("press → hold → release"), different payload. Knowing which family fits which game is the whole point.

## 2. Core UX rules that make it feel good (apply to both projects)
1. **Live ghost preview** the entire drag path (valid = tinted ok, invalid = red).
2. **Auto-routing + auto-orient corners** — the system figures out belt/fence/path direction; player never rotates manually.
3. **Auto-tiling (Wang)** for connected pieces — both projects already have a Wang pipeline ([[wang-tile-build-workflow-rima]]).
4. **Two drag modes:** free-path (follow cursor exactly) and straight-line / L-shape (hold Shift) — Factorio's key trick.
5. **Live cost/validity feedback** (count + resource cost shown during drag).
6. **Commit on release, cancel on right-click**, and **undo** the whole drag as one action.
7. **Snap to grid**, and don't double-place when the cursor jitters on one cell.

## 3. RIMA (action-roguelite, top-down 3/4, cursor-aim) — where it fits
RIMA is **combat, not building** — so the pattern splits cleanly:

### 3A. Level authoring tools = DRAG-PLACE (highest-value, direct analog) ⭐
The factory belt = RIMA's **room/level painting**. RIMA already has the F2 in-play Map-Paint overlay + Room Painter (BrushExecutorRouter) + Wang tiles + cliff auto-placer.
- **Hold+drag to paint** floor / cliff / decor tiles with auto-tiling + auto-cliff-edge orientation — exactly the belt feel, applied to authoring. Big speedup on the thing we iterate most.
- **Drag a line of walls/cliffs** that auto-orient corners (the belt's literal mechanic).
- **Drag-to-scatter prop rows** (pillars, braziers, chains) auto-spaced along the dragged path.
- Lowest friction, biggest payoff — turns tedious cell-by-cell clicking into one stroke. **Recommend: add continuous hold-drag + ghost to the F2 overlay & Room Painter.**

### 3B. Combat = HOLD-CHARGE / CHANNEL (the action translation) ⭐
Do NOT force "drag-build" into combat. The combat form of "press-and-hold" is charge/channel:
- **Hold-to-charge heavy** — Warblade holds attack → charges a **Sundered Beat heavy** (BREAK→EXECUTE signature verb); release = big hit. Natural fit for the signature mechanic ([[project-skill-mechanic-final-report-s6]]).
- **Channeled beam/aura** — Elementalist/Hexer: hold to channel a continuous beam/drain toward the cursor (ticks while held), release to stop.
- **Hold-to-auto-fire** — Ranger/Gunslinger: hold fire → repeats toward cursor at fire-rate (cursor-aim already there).
- **Hold-to-interact** — hold G to revive / channel a shrine / deliberate pickup (tap stays for the map fragment).

### 3C. RIMA caution
Keep the two families separate: **drag-place = authoring tools only; hold-charge = combat.** Mixing "build by dragging" into action combat reads wrong for the genre.

## 4. LaurethStudio (cozy farm/builder) — where it fits ⭐⭐ (core UX, not optional)
This is the pattern's home turf — a cozy farm/builder lives on repeated tile actions, so hold-drag is a massive QoL win:
- **Drag-to-till / plant / water ROWS** — hold+drag across tiles → auto-till a row, auto-plant at spacing, auto-water. The single biggest farming-sim QoL.
- **Drag-to-place fences / paths / irrigation** — auto-connecting via Wang, exactly like belts (auto-corner).
- **Drag-to-harvest** a row/field in one stroke.
- **Drag-zone** building/decor areas.
- **Auto-routing logistics** (carts/conveyors/pipes) if Studio has them — the literal Conor-Dart belt.
- Layers cleanly on Studio's existing **Wang pipeline + Auto-Decor Brush (Syrup pattern, transfer KARAR_G)** — same ghost/auto-tile machinery, new verb.
- **Recommend: make hold-drag the DEFAULT for till/plant/water/fence/path in Studio.**

## 5. Shared implementation note
Both projects already own the hard parts (Wang auto-tiling, ghost-preview brushes). The new work is the **input layer**: pointer-down starts a drag-stroke, accumulate cells along the path (dedupe per cell), live ghost, on pointer-up commit as one undoable action; Shift = straight-line mode; right-click = cancel. RIMA: bolt onto F2 overlay + Room Painter. Studio: bolt onto the farm/tile interaction layer.

## 6. Verdict
- **RIMA:** adopt DRAG-PLACE for **authoring tools** (F2 overlay / Room Painter) + HOLD-CHARGE/CHANNEL for **combat**. Two separate features, same grammar.
- **LaurethStudio:** adopt hold-drag as the **default farm/build interaction** (till/plant/water/fence/path/harvest). Highest fit of the two projects.

---

## 7. Reference-game survey (ax/Gemini, folded)

| Game | Held & dragged | Key trick |
|---|---|---|
| **Factorio** | belts, pipes, power poles | smart axis-lock + auto-90° corners; poles auto-space to max wire range |
| **Mindustry** | belts, conduits, walls | live pathfinder previews shortest route *around* obstacles before commit |
| **Townscaper** | blocks, paint | drag paints raw voxels → **WFC** auto-builds roofs/arches/stilts |
| **Satisfactory** | belts, foundations | **"Zoop"** mode drags a line (≤10) with volumetric ghost |
| **Stardew** | hoe/water/seed/harvest | tool charge shows 1×3/3×3 grid; **hold + walk = "paint" tiles as you pass** |
| **Shapez / DSP / Timberborn** | belts/roads | frictionless auto-direction to ports; multi-level vertical snap |
| **Cities: Skylines** | roads, zoning | curve-fit arcs + live angle/length; zoning = drag-fill like spreadsheet |

**Implementation must-haves (ax):**
- **Interpolate between frames** — draw cells along the vector between last & current grid cell so fast drags leave no gaps.
- **Grid-crossing throttle** — recompute auto-tile/route only when the cursor enters a NEW cell, not every frame (perf).
- **Transactional undo** — one drag = one undo entry.
- **Axis-lock / Manhattan routing** (Shift) to kill zig-zag "whip" corners.
- **Non-destructive by default** — route around existing infra; Shift to force-overwrite.
- **Graceful resource stop** — place up to the affordable index, color the rest red; never hard-lock.

**Cozy-builder juice (→ LaurethStudio):** squash-stretch drop + dust puff on place; **soft-snap** cursor toward interactables; **pitch-ascending audio ramp** per tick across a sweep (musical reward for long drags) — cheap, huge feel win.

**Action-game adaptations (→ RIMA combat), beyond charge/channel:**
- **Drag-to-Slash / chained dash** — hold dash → brief bullet-time → drag a path across enemies → release = high-speed dash-attack along the drawn path. **Strong fit for Shadowblade + the cross-class Echo dash-strike** ([[overnight-bulk-wave]] WAVE2). A genuinely new RIMA combat verb worth prototyping.
- **Hold-to-lock / drag-to-retarget** — hold fire to lock nearest enemy; drag reticle to override target (ranged classes).
