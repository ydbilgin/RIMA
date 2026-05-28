# RIMA Room Generation — Architecture Decision

**Date:** 2026-05-23 | **Session:** S101 → S102 transition
**Authority:** rima-design (Opus) decision, Codex review pending, orchestrator synthesized

---

## DECISION: Option C — Hybrid Template + Decor Overlay

**Confidence: HIGH** (Opus)

Patterns from Hades / Dead Cells / Cult of the Lamb. **Adds two-tier architecture on top of S101 wall sheets, does NOT supersede.**

---

## Rationale (RIMA-specific)

### 1. Visual evidence forces it
chatgpt_ref images (16_12_46, 16_12_48 #4, 16_12_48 #7) + `rima_master_room.png` side-by-side show:
- **Base architecture** (stone walls, broken keep shell, floor crack pattern) is essentially the SAME painted environment across all three shots — same wall tile texture, same lighting direction, same floor mosaic
- What **changes** between rooms is **overlay content**: banner positions, torch placement, statue alcoves, broken pillar debris, rift crack density, enemy/altar props

This is exactly the Hades / Dead Cells composition language: **baked architecture + scripted decor**.

### 2. S101 already commits to it (extended)
CURRENT_STATUS S101 LOCK (lines 28-51):
- Walls = PixelLab 2x2 grid sheets (architecture baseline, pillar-less, NO decor baked in)
- Torches/banners/statues/rift cracks = separate overlay prop sheets, transparent BG

S101 is already a degenerate form of C at the wall-tile granularity. Decision: lift the "base" granularity from 2×2 wall sheets up to whole-room templates. **S101 wall sheets become template-extension/repair layer.**

### 3. Pure A fails twice on RIMA evidence
- Memory `project_wall_chunk_pixellab_pipeline_2026_05_23.md` already REVOKED tiny modular tiles ("seam problems, alignment issues")
- LoRA trained on 335 PixelLab pieces inherits PixelLab's baked-composition strength + modular-edge weakness. Forcing modular output fights the training distribution
- Camera close-up: player sees 5-8 char widths = ~one room per screen. "Infinite shape variation" benefit of tilemaps wasted when each visible scene is one screen-sized chunk

### 4. Pure B fails the roguelite contract
- 5-10 baked rooms become recognizable within 3 runs
- Pure B has zero gameplay-affordance flexibility (spawn points, breakables, altars, rifts must be re-painted per room)
- C lets all gameplay anchors live on overlay layer + reuse the template

### 5. LoRA training data is shaped for C
335 images = characters + props + walls + decals + gameplay screenshots. Distribution is TWO distinct asset classes baked into one model: (a) full painted scenes (b) isolated transparent props. **Same LoRA generates both templates and decor, style coherence guaranteed via shared training weights.** A or B uses only half the LoRA's competency.

---

## Phase 2 Production Split (34 generations total)

### Templates: 6
| # | Template | Purpose |
|---|---|---|
| 1 | Shattered Keep small chamber | Standard combat room (small) |
| 2 | Shattered Keep medium chamber | Standard combat room (medium) |
| 3 | Keep corridor connector | Long thin, between chambers |
| 4 | Keep boss antechamber | Large, altar focal point |
| 5 | Rift-corrupted variant | Lit + rift-veined chamber (separate template — preserves bake quality) |
| 6 | Outdoor courtyard | Broken keep exterior, Act 2 transition setup |

Why 6: 3 lighting variants × mirror flip × ~6 decor permutations per anchor = hundreds of visually distinct rooms. <5 = patterns visible <3 runs; >8 = ROI dropoff for solo dev.

### Decor objects: 28 (≥3 swap options per anchor category)
| Category | Count | Items |
|---|---|---|
| Torches | 4 | wall sconce, freestanding brazier, hanging chandelier, broken/unlit |
| Banners | 3 | intact, torn, fallen on floor |
| Statues | 3 | intact alcove, decapitated, rubble pile |
| Debris piles | 5 | small/medium/large rubble, broken pillar, collapsed wall |
| Rift veins | 4 | thin crack, branching crack, vein cluster, full rift pool |
| Altars | 3 | intact, cracked active, destroyed (objective anchors) |
| Breakable props | 6 | urns, chests, crates, bone piles, candelabra, weapon racks |

---

## Unity Implementation Outline (5 steps)

### Step 1 — Schema (1 file)
Create `RoomTemplate` ScriptableObject:
- `Sprite baseImage`
- `PolygonCollider2D wallPath` (serialized points)
- `OverlayAnchor[] anchors`

Each `OverlayAnchor`:
- `Vector2 localPos`
- `DecorCategory category` (enum: Torch / Banner / Statue / Debris / RiftVein / Altar / BreakableProp)
- `bool required`
- `float spawnWeight`

### Step 2 — Generate 6 templates + 28 decor sprites via trained RIMA LoRA
- Templates 1024×1024 (matches camera close-up; one screen ≈ one template)
- Decor 64-192px transparent BG
- Same LoRA, same seed family for style coherence
- 3 lighting variants per template (day-keep, dusk-keep, rift-corrupted) — free run-variation

### Step 3 — `RoomDecorationSpawner` runtime component
On room enter:
- Takes RoomTemplate + run seed → picks decor variants per anchor
- Instantiates prefabs as children
- Adds light/SFX components for braziers/rifts
- 50% random horizontal mirror flip of entire template + anchor list (doubles variety at zero asset cost)

### Step 4 — Extend RimaWorldPainterWindow with "Template Authoring" mode
- Import LoRA output PNG → user clicks to drop anchors → choose category per anchor → save as RoomTemplate asset
- This is the only NEW tool work; everything else is runtime

### Step 5 — Wire S101 wall-sheet pipeline as template-extension layer
- Template needs doorway shifted / corridor opened / wall variant for boss-room: 2×2 wall chunks paint over the template's wall path via the painter
- **S101 work is NOT WASTED** — it becomes the editing surface for templates rather than primary rendering surface

---

## Risk Mitigation (C's main weakness: anchor-authoring overhead)

**Risk:** 6 templates × ~12 anchors each = 72 hand-placed anchors. At 30sec each = 36min total — acceptable, but every new template later compounds.

**Mitigation:** Build painter's Template Authoring mode with **decor-category auto-detector hint**:
- One-shot scan on import: dark sockets in walls = torch candidates, large floor regions = altar candidates, corners = debris candidates
- Pre-drops anchor suggestions; user accepts/rejects
- Converts 30 sec/anchor → 5 sec/anchor accept-or-skip
- Heuristic-based (no ML), half an afternoon's Codex task

**Secondary risk:** Template LoRA output has bad walls / broken silhouette on some gens.
**Mitigation:** S101 wall-sheet kit can be painted over template's edge to repair. Template-extension layer doubles as template-repair layer.

---

## Conflicts with Locked Rules: NONE

Fully consistent with:
- `project_wall_chunk_pixellab_pipeline_2026_05_23.md` — wall chunks valid as template extension
- `project_painter_consolidation_lock.md` — RimaWorldPainterWindow stays primary
- `project_topdown_pivot_lock.md` — 85-90° top-down, templates authored at that angle
- `project_modular_pipeline_lock.md` — modular object batches (n_frames) = decor pipeline

---

## Next Steps (orchestrator)

1. ✅ Write this file
2. ⏳ Wait for Codex review (technical/Unity perspective)
3. ⏳ Update CURRENT_STATUS.md for S102 two-tier architecture
4. ⏳ LoRA training (re-)start (currently restarting after torch CUDA fix)
5. After training: dispatch Codex to scaffold `RoomTemplate`, `OverlayAnchor`, `RoomDecorationSpawner` (Assets/Scripts/Rooms/ only)
6. User manual (web UI on return): first template via trained LoRA → validate

---

**Decision-maker:** rima-design (Opus 4.7)
**Reviewed by:** Codex review pending (architectural validation)
**Approved by:** [pending user return]
