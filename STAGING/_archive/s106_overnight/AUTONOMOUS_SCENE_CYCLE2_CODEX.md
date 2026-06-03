# Scene Composition Cycle 2 — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Cycle 1 (your previous dispatch) was a solid blockout but Antigravity reviewed and verdict = ITERATE-CYCLE-2. Specific gaps identified. This dispatch applies the top 5 changes per Antigravity verdict + targeted polish.

## INPUTS (read in order)

1. **`STAGING/s106_overnight/SCENE_V2_REVIEW_VERDICT_AGY.md`** — Antigravity verdict, score card, top 5 changes. **PRIMARY INPUT.**
2. **`STAGING/s106_overnight/walless_v1_batch2_M3.png`** — target reference
3. **`STAGING/s106_overnight/scene_v2_vs_M3.png`** — current vs target side-by-side
4. **`STAGING/PIXELLAB_INVENTORY_CATALOG_2026_05_25.md`** — existing 220-object PixelLab catalog (find paths for assets by ID)

## CURRENT STATE
- Scene: `Assets/Scenes/Test/PlayableArena.unity`
- 41-tile compact arena at origin (Cycle 1 output)
- Player at (0,0,0), Camera (0, -0.5, -10) ortho 3.5
- CliffRing 24 sprites, RoomBackgroundRig 6 layers
- Sorting: BG Ground/-800..-500, Cliff Ground/-50, Floor Floor/0, Player Characters/10
- Lights: Warm Torch NW/NE, Cyan Crystal SW/SE, Global Light 2D 0.3

## CYCLE 2 TARGETS (per Antigravity verdict)

### Change 1 — Four Corner Braziers (HIGH impact, LOW effort)
- **Find brazier asset:** search `Assets/Sprites/AssetPackV3/` or `PIXELLAB_INVENTORY_CATALOG_2026_05_25.md` for object `41342e20` or `mounting apparatus` or `brazier` keyword
- If not found, use a fallback: an existing prop SpriteRenderer with warm orange tint
- Place 4 braziers at arena corners: roughly `(-5.5, -3.5)`, `(5.5, -3.5)`, `(-5.5, 3.5)`, `(5.5, 3.5)` (depends on arena size)
- Each brazier = GameObject with:
  - SpriteRenderer (sortingLayer "Floor", order 5)
  - Child GameObject with Light2D (Point Light, color warm orange 1.0, 0.5, 0.2, intensity 2.0, falloff 0.5, radius 1.5)
- Replace the existing 2 Cyan Crystal Lights with warm braziers (unify color palette per agy verdict #3)

### Change 2 — Central Portal & Glow (HIGH impact, MEDIUM effort)
- **Find decal:** search for object `5ccc5721` in PixelLab inventory or `Assets/Sprites/AssetPackV3/` (look for "portal", "rune circle", "ritual circle" keyword)
- If found: create GameObject "CentralPortal" at (0, 0, 0):
  - SpriteRenderer (sortingLayer "Floor", order 1 — above floor tiles but below props)
  - Scale to ~2×2 world units
- Add child Light2D (additive blend, intensity 2.5, radius 2.0, color cyan #00FFCC)
- If no decal asset found: use the **central 5 rune tiles** as the "circle" — paint them in a tight 3-cell radius pattern AND add the bright cyan Light2D at (0, 0)

### Change 3 — Corner Framing Pillars (HIGH impact, MEDIUM effort)
- **Find pillar asset:** search for `6b52751d-67eb-4684-b7e4-f4a0a00c7831` or `stone pillar` / `column` / `broken pillar` in inventory + AssetPackV3
- Place 4 pillars at outer arena bounds (just OUTSIDE the floor edge, framing the diamond):
  - `(-6.5, 2.5)`, `(6.5, 2.5)`, `(-6.5, -2.5)`, `(6.5, -2.5)`
- Each pillar:
  - SpriteRenderer (sortingLayer "Floor", order 8 — above floor, above brazier)
  - Scale 1× (or whatever the asset's natural size is)

### Change 4 — High-Contrast Lighting (HIGH impact, LOW effort)
- Set Global Light 2D intensity from 0.3 → **0.15**
- Set the 4 corner brazier lights intensity to **2.2** (warm orange)
- Add cyan rim Light2D under each cliff edge cardinal side (S/E/W) — Point Light, color cyan, intensity 0.8, radius 1.0, positioned just outside the floor rim, below sprite
- Optional: add LightPulse component to one brazier for variety

### Change 5 — Purple Storm BG (MEDIUM impact, MEDIUM effort)
- Tint `L1_Nebula` SpriteRenderer color to magenta-violet: RGB (0.45, 0.20, 0.65) at alpha 1.0
- Tint `L0_Void` SpriteRenderer to dark violet: RGB (0.10, 0.05, 0.18)
- Tint `L2_Ruins` to dim purple-grey: RGB (0.30, 0.25, 0.40)
- Optional: Add Particle System "LightningStreaks" as child of RoomBackgroundRig:
  - Emission rate 2/s
  - Lifetime 0.5s
  - Speed 4-6 unit/s
  - Stretched billboard shape, magenta color, alpha fade
  - Random position within (-15, -15) to (15, 15) range
- Skip particle if too complex; tint shifts alone should add purple feel

### Change 6 — Arena slight expansion (per agy #1)
- Current 41 cells → repaint slightly larger: oval mask `Mathf.Abs(x) / 7f + Mathf.Abs(y) / 4f < 1f` → ~14×9 cell footprint, ~58-60 cells

### Change 7 — Cliff variety (per agy #4)
- Existing CliffRing 24 cliffs all uniform. Make corners use NE/NW/SE/SW sprites (already correct), but sides should mix S/N/E/W with occasional cyan_glow variant
- For 4-6 of the south cliff sprites, swap to `cliff_cyan_glow` for under-rune-zone glow effect
- Slight position offset (±0.1-0.2 unit) for visual variation, NOT a perfect line

## PROCESS

1. Phase 0: Read inputs (M3 ref, verdict, side-by-side, inventory)
2. Phase 1: Find asset paths for brazier / portal / pillar in inventory + Assets/Sprites/AssetPackV3/. Note found vs missing.
3. Phase 2: Apply changes 6 (expand floor) + 1 (braziers) + 3 (pillars) + 4 (lighting contrast) — these are highest impact
4. Phase 3: Apply change 2 (central portal) + 5 (purple tint)
5. Phase 4: Apply change 7 (cliff variety)
6. Phase 5: Save scene, take screenshots
7. Phase 6: Done report

## DELIVERABLES

- Modified `Assets/Scenes/Test/PlayableArena.unity`
- `STAGING/s106_overnight/scene_v3_match_attempt.png` (1280×720 game view)
- `STAGING/s106_overnight/scene_v3_vs_M3.png` (1280×720 left + M3 right side-by-side)
- `STAGING/s106_overnight/SCENE_V3_REPORT.md`:
  - Asset paths used (which were found, which fell back)
  - Per-change checklist (1-7) completed / partial / skipped
  - Honest self-assessment vs M3 (1-10 score)
  - Console verify (0/0)
- Final `CODEX_DONE_<profile>.md` with `STATUS: DONE`

## CONSTRAINTS
- **NO autonomous PixelLab API calls** (user halted)
- Reuse existing PixelLab inventory assets (search catalog by ID)
- 0 error 0 warning required
- Preserve Player WASD + combat components
- Preserve Tilemap iso settings (cellSize 1, 0.609375, 1; CellLayout Isometric)
- Single scene save at end

## TIME ESTIMATE
~60-90 min at xhigh.

After this DONE: Opus will dispatch agy review on `scene_v3_vs_M3.png`. If close → green-light + final memory + status update. If gaps → Cycle 3 (likely just polish).
