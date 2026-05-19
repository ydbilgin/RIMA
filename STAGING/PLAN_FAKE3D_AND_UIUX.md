---
title: RIMA Plan — Fake-3D Visual System + Map Designer UI/UX
status: ACTIVE_S88_LATE
tarih: 2026-05-18
mode: Mutual QC with Codex (orchestrator dispatches consultation → Codex critique → orchestrator implements → Codex verify)
---

# Plan — Fake-3D Visual + Map Designer UI/UX

User goal (verbatim 2026-05-18):
> "alabaster dawn hades gibi görünüm olacak ona yakın olacak doğal yer ve oynanış"
> "map designer'da kullanıcı dostu UI UX yapılmalı ki orada map düzenlemesi yaparken çok rahat simste yapar gibi yapmak istiyorum"
> "önce fake 3d yaratan her şeyi codex ile beraber çöz. sonra map designer'ın UI UX ini codex ile konuşup onay alarak birbirinizi kontrol ederek yapmaya devam edeceksin"

This plan is split into PHASE A (fake-3D solved end-to-end) and PHASE B (Map Designer Sims-tarzı UX). Each phase uses **Mutual QC workflow** — orchestrator + Codex review each other.

## Reference visual targets

- **Alabaster Dawn** — pure 2D top-down + painterly continuous floor + lush vertical props (cliffs, trees, columns) + atmospheric lighting
- **Hades** — 2.5D (3D camera tilt + 2D billboard chars) BUT achievable in pure 2D via dense vertical content + angled-view sprites
- **Diablo 2** — orthographic + isometric-feel via angled sprite content
- **Hyper Light Drifter** — flat top-down + heavy ambient mood

RIMA architecture LOCK: pure 2D, PPU=32 IMMUTABLE, 30-35° angled-view sprite content, camera orthographic dik (no tilt). See `[[multi-projection-architecture-lock]]`.

## Current state (S88 LATE LIVE)

- `Assets/Scenes/Demo/RoomPipelineTest.unity` PlayableRoom hierarchy:
  - 1 big procedural floor sprite (1152×704, clean dark slate)
  - Decoration intentional 4-quadrant: focal ritual sigil + north moss + east rift + south debris + west natural + sparse trail
  - Vertical placeholders: 5 walls (N border) + 2 columns (flank focal) + 1 banner (E)
  - Warblade player + warm Light2D point glow
  - Camera ortho 7, follow Lerp 0.15 (TestCameraFollow)
  - WASD movement (TestPlayerMovement — new Input System after S88 fix)
- 84 sliced PNG parts at `Assets/Sprites/Environment/RIMA_AssetParts_v2/` + 7 PatchAtlasSO + 1 SpriteAtlas LIVE
- 40 new sliced PNG parts at `STAGING/RIMA_AssetParts_v3/sliced/` (walls/props/biome_floors/accents) — NOT YET imported to Assets
- Memory: `[[brush-v1-manual-composition-system]]`, `[[multi-projection-architecture-lock]]`, `[[room-composer-paint-intent-lock]]`, `[[hybrid-asset-pipeline-lock]]`, `[[3d-portability-strategy]]`

## PHASE A — Fake-3D solved (pure 2D, no tilt)

### A.1 — Codex v3 production import (immediate)

- Move 40 PNG from `STAGING/RIMA_AssetParts_v3/sliced/{walls,props,biome_floors,accents}/` → `Assets/Sprites/Environment/RIMA_AssetParts_v3/`
- Importer settings (same as v2): Sprite/Single, Point, no mipmaps, alphaIsTransparency, PPU=32, Uncompressed, spriteExtrude=1
- Create new PatchAtlasSO:
  - `Walls.asset` (PatchRole.MacroPatch or extend enum) — 12 wall variants
  - `VerticalProps.asset` (PatchRole.Accent or extend enum) — 8 props
  - `BiomeFloor_{Mossy,Sandy,Blood,Cave}.asset` — 4 atlases × 4 variants each
  - `AtmosphericAccents.asset` — 4 special focal accents
- Extend `RoomVisualProfileSO` to reference new atlases (likely need new field for walls + props)

### A.2 — Vertical content replace (placeholders → real sprites)

In PlayableRoom, replace procedural placeholders:
- 5 wall placeholders (N border) → 5 real wall sprites from `Walls.asset` (variants 01, 02, 03, etc.)
- 2 column placeholders → 2 real columns from `VerticalProps.asset` (intact + broken)
- 1 banner placeholder → 1 real torn banner from `VerticalProps.asset`
- Add: 2-3 more vertical props (brazier near focal, hanging chain east of rift, candelabra near banner)
- Add 1 atmospheric accent (portal puddle east of room or cursed obsidian cluster south)

### A.3 — Lighting + atmosphere

- Player light glow: warm (already LIVE)
- Add 1 dim cool ambient Light2D (top of scene, far reach) for hades-style background mood
- Add brazier glow Light2D (warm orange) attached to brazier prop
- Add rift glow Light2D (cold blue/violet) attached to rift accent
- Global ambient adjust to dim baseline (0.25 intensity)

### A.4 — Composition rules (intentional, not random)

Visual composition principles applied:
- Rule of thirds (focal at center but flanked at 1/3 positions)
- Visual weight balance (heavy NW counter-balanced by mid-density SE)
- Leading lines (sparse pebble trail connects focal → rift)
- Tonal grouping (warm cluster around braziers + cool cluster around rift)
- Negative space (open dark areas between zones for "breathing room")

### A.5 — Visual gate verdict ROUND 4

Game view screenshot capture (NOT Scene view). Codex review:
- PASS criteria: composition reads as "Hades/Alabaster Dawn-tarzı natural map", vertical depth visible, focal points rare-intentional, no per-tile grid
- FAIL criteria: still reads as top-down floor map, vertical content insufficient, composition random

If FAIL → escalate to PHASE A.6 (2.5D escape decision)

### A.6 — Escape hatch (only if A.5 fails)

Camera tilt + sprite shear approach (HD-2D pivot). Big scope, 1-week validation. See `STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md`.

## PHASE B — Pro UI/UX Map Designer (ASIL BÜYÜK İŞ)

> User directives (2026-05-18 night):
> - "asıl büyük iş o" — Phase B = the main work
> - "pro şekilde UI/UX designer ve işlevselliğini de eklesene"
> - "kullanıcı çok rahat şekilde asset pack'inden alıp yerleştirecek"
> - "sims gibi rahat" (kavramsal — sims örnek değil tam)

**REFERENCE BENCHMARK**: Sims 4 Build Mode + RimWorld map editor + Stardew Valley Stardew Editor + Townscaper drag-drop. Industri-pro tooling.

### Verdict applied (rima-design ace12576 sub-agent)

15 features → **9 features V1** (6 cut: selection rect, snap toggle, mirror, composition guides, density heatmap, AI suggestion). MVP = Phase B-1 + B-2 + B-3. B-4 + B-5 deferred to V2.

### V1 priority order

| # | Feature | Phase | Required | Notes |
|---|---|---|---|---|
| P0-a | Asset Pack Browser (category tree + sprite grid + search + pack switcher + hover preview) | B-1 | ✅ | Read-only first |
| P0-b | Click-to-Place + ghost preview + sortingOrder auto + right-click cancel + Esc deselect | B-2 | ✅ | Cursor UX critical |
| P0-c | Selected-sprite inspector (variant/scale/alpha/flip/rotation/sortingOrder) | B-2 | ✅ | Trivial Editor UI |
| P0-d | Auto-collider attach (PropDefinitionSO blocksMovement + shape) | B-2 | ✅ | SO schema extension |
| **P0-e** | **Room-as-Prefab save/load** (rima-design PROMOTED from P2) | B-3 | ✅ | Persistence anchor |
| P0-f | Active-target binding (window header "active room root" field) | B-3 | ✅ | Hierarchy integrity |
| P0-g | AssetPackManifestSO (silent P0 dependency — aggregate PatchAtlasSO+PropDefinitionSO) | B-1 | ✅ | Browser data backbone |
| P1 | Random variant toggle | B-3 | ✅ | Iteration speed |
| P1 | Layer visibility toggle (show/hide per category) | B-3 | ✅ | Dense scene workflow |
| P1 | Undo/Redo (Ctrl+Z/Y, Unity.Undo backed) | B-3 | ✅ | Mistake recovery |
| P1 | Eyedropper Alt+click quick-pick | B-3 | ✅ | Hades-modder pattern |

### Mutual QC workflow with Codex on each subsystem.

### B.1 — Architecture review (Codex consultation)

Existing `Assets/Editor/MapDesigner/` + `MapDesignerBrushWindow` already provides Brush V1 paint mode. Audit:
- Current UI affordances (palette? brush size? layer toggle?)
- Missing Sims-tarzı features
- Performance baseline

### B.2 — Core features (priority-ordered)

**P0 — Asset Pack Browser (the primary feature)**

1. **Asset Pack Browser** — left dockable panel:
   - Category tree (collapsible): Floor / Macro / Decals (Moss/Dirt) / Scatter (Pebble/Cracks) / Accents (Rift/Ritual/Atmospheric) / Walls / Vertical Props / Biome Floors (4 biomes)
   - Per-category sprite grid (thumbnail + variant index)
   - Search bar (filter by name)
   - Pack switcher (RIMA_AssetParts_v2 / v3 / future PixelLab packs)
   - Sprite hover → enlarged preview + metadata (size, atlas, role)

2. **Click-to-Place workflow**:
   - Select sprite from browser → cursor becomes ghost-preview of that sprite
   - Move cursor over Scene View → ghost follows mouse (pixel-snapped to PPU grid)
   - Left-click → place sprite at cursor (parented under correct layer by sortingOrder)
   - Right-click → cancel selection (cursor returns to default)
   - Click+drag (with selected sprite) → paint stream of sprites along path (brush mode)

3. **Selected sprite inspector** (right panel):
   - Variant index slider (cycle through atlas variants)
   - Scale slider (0.3 - 2.0)
   - Alpha slider (0.0 - 1.0)
   - Flip X / Y toggles
   - Rotation 0/90/180/270 buttons
   - SortingOrder field (auto-set per category but editable)

**P1 — Workflow accelerators**

4. **Random variant toggle** — palette mode "pick random from category" instead of single sprite
5. **Layer visibility toggle** — show/hide each category layer (Floor/Macro/Decals/.../Walls/Props) — clean working
6. **Undo/redo** — Ctrl+Z / Ctrl+Y, ring buffer of last 50 operations
7. **Eraser mode** — alt-click or E key, hover over sprite to delete

**P2 — Power user**

8. **Selection rectangle** — drag to select region, mass operations (delete / replace variant / randomize)
9. **Save/load room preset** — name + save current PlayableRoom hierarchy as `.asset` preset (Combat Room / Treasure Vault / Shrine)
10. **Quick-pick from scene** — Alt+click on existing sprite to select that variant as brush
11. **Snap toggle** — pixel-perfect cell snap vs free placement (decimal positions)
12. **Mirror/symmetry mode** — paint operations mirror across X or Y axis

**P3 — Composition assist**

13. **Composition guides** — overlay rule-of-thirds grid, focal point markers, golden ratio guides
14. **Density heatmap** — visualization of decoration density (hot/cold spots) to spot empty zones
15. **Z-walking suggestion** — AI-suggest where to add scatter based on existing focal points

### B.3 — UI/UX wireframe (text-based, for Codex review)

```
┌─────────────────────────────────────────────────────────────────────┐
│  RIMA Map Designer  [File] [Edit] [View] [Help]                    │
├──────┬──────────────────────────────────────────────────┬───────────┤
│      │                                                  │           │
│ TABS │              SCENE VIEW VIEWPORT                 │ INSPECTOR │
│      │                                                  │           │
│ ▼Flr │       (room canvas + hover preview)              │ Brush:    │
│ Macr │                                                  │  Size: ▶3 │
│ Moss │                                                  │  Opc:  85%│
│ Dirt │                                                  │ Layer:    │
│ Peb  │                                                  │  [v] Flr  │
│ Crk  │                                                  │  [v] Deco │
│ Rift │                                                  │  [ ] Wall │
│ Walls│                                                  │ Selected: │
│ Prop │                                                  │  RitualSig│
│ Biom │                                                  │  variant 1│
│      │                                                  │           │
├──────┴──────────────────────────────────────────────────┤  History  │
│  SPRITE PALETTE (grid of selected category variants)    │  > paint  │
│  [□][□][□][□][□][□][□][□][□][□][□][□][□][□][□][□]      │  > erase  │
│  [□][□][□][□][□][□][□][□][□][□][□][□][□][□][□][□]      │  > paint  │
└─────────────────────────────────────────────────────────┴───────────┘
```

### B.4 — Phased delivery

- **Phase B-1** (1 day): **Asset Pack Browser** read-only — category tree + sprite grid + hover preview. No placement yet. Codex review.
- **Phase B-2** (1 day): **Click-to-Place** — ghost preview at cursor + left-click placement + right-click cancel + sortingOrder auto-assign. Codex review.
- **Phase B-3** (1 day): **Selected sprite inspector** + undo/redo + eraser mode. Codex review.
- **Phase B-4** (1 day): Power user features (selection rect + preset save/load + quick-pick + snap + mirror). Codex review.
- **Phase B-5** (1 day): Composition assist (guides + heatmap + suggestions). Codex review.

Each phase MUST: orchestrator implement → Codex review → orchestrator fix → Codex re-verify → user playtest → commit.

Each phase: orchestrator implement → Codex review → orchestrator fix → Codex re-verify → user playtest → commit.

## Mutual QC Workflow (Orchestrator + Codex)

For each PHASE A / PHASE B subsystem:

1. **Orchestrator drafts spec** in STAGING/
2. **Codex consultation dispatched** (read-only, returns critique + approve/reject + risks)
3. **Orchestrator addresses risks** + re-spec
4. **Codex approval** before implement
5. **Orchestrator implements** (direct MCP or Codex dispatch)
6. **Codex verifies** implementation (read-only, returns PASS/FAIL with evidence)
7. **User playtest** (Game view + Play mode)
8. **Commit** when user PASS

Key principle: **No single agent unilateral decision**. Orchestrator can propose; Codex can veto/critique. User has final say.

## Workflow LOCK (S88 night)

User delegation 2026-05-18 night: "siz tatmin olana kadar yapın", "sorma", "kararı sen ver Codex'e sorarak git", "planları netleştir benden onay beklemeden geç direkt".

**Decision authority chain:**
1. **Orchestrator (Sonnet/Opus)** has final say on phase transitions + scope changes
2. **Codex** has authority on implementation specifics within a phase (full autonomy for current Phase A iteration `bi9s1bxna`)
3. **Mutual QC** required at every phase boundary (consultation → critique → approve → implement → verify)
4. **User** intervenes only when Codex+Orchestrator deadlock OR for high-level scope (e.g., "skip Warblade")

**Phase transition rules (no user approval needed):**
- Phase A → Phase B: when Codex DONE marker reads "PASS" + orchestrator screenshot review confirms
- Phase B-1 → B-2: when Phase B-1 user-facing test passes (asset browser opens, sprites browse-able)
- Phase B-2 → B-3: when Click-to-Place + auto-collider tests pass
- Etc.

**Iteration limit per phase:** max 3 Codex internal iterations before orchestrator switches strategy (different approach, escape hatch, or escalate to user).

## Open work checklist

### Immediate (this session)

- [x] User reported Unity error — root cause: legacy `Input.GetAxisRaw` vs new Input System package → fix `TestPlayerMovement.cs`
- [ ] Codex v3 production import (40 PNG → Assets) — needs Codex dispatch
- [ ] Replace vertical placeholders with real Codex v3 wall/column/banner sprites
- [ ] Phase A.5 visual gate verdict (Game view screenshot, Codex review)
- [ ] Commit this plan + memory updates

### Next session (2026-05-19, PixelLab 5000 gen budget arrives)

- [ ] PixelLab characters via Web UI V3 (Antigravity 10 anchors → 4-direction sprites + animations)
- [ ] PixelLab props (4 P0 from `RIMA_MAP_PRODUCTION_SEQUENCE.md` if Codex v3 props insufficient)
- [ ] Phase A.6 decision (if A.5 failed): HD-2D escape hatch or polish current
- [ ] Begin Phase B.1 (Sprite palette UI Codex consultation)

## Related memory

- `[[brush-v1-manual-composition-system]]` (S88 LIVE system map)
- `[[multi-projection-architecture-lock]]` (6 hard rules)
- `[[room-composer-paint-intent-lock]]` (Brush V1 semantic 3-mode)
- `[[hybrid-asset-pipeline-lock]]` (Karar #157 — PixelLab + Codex split)
- `[[pixellab-character-states-workflow]]` (6 use cases LOCK)
- `[[codex-parallel-profile-workflow]]` (3-profile parallel dispatch)
- `[[map-system]]` (STS2 DungeonMapUI room graph)
