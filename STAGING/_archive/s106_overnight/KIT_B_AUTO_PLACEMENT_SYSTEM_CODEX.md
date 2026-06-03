# Kit B Auto-Placement System — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Long-term reusable system. Currently CliffRing has 24 manually placed cliff sprites — works for ONE arena shape but breaks if user paints different. User wants: "ground tile'larının kenarlarına otomatik mantıksal olarak yerleştir, uzun vadede her arena için çalışsın." Auto-detect floor edges + place appropriate cliff sprite (S/N/E/W + corners) based on neighbor cells. ScriptableObject rule config for future flexibility.

## DESIGN OUTLINE

### Components

1. **`CliffPlacementRules.cs`** (ScriptableObject)
   - Asset path: `Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset`
   - Fields:
     ```csharp
     public Sprite cliffS, cliffN, cliffE, cliffW;
     public Sprite cliffNE, cliffNW, cliffSE, cliffSW;
     public Sprite cliffAccent;  // e.g. cyan_glow variant
     public float accentChance = 0.15f;
     public Vector2 spriteScale = Vector2.one;
     public Vector2 worldOffset;  // optional per-direction overrides via array
     public int sortingOrder = -50;
     public string sortingLayer = "Ground";
     public int pixelsPerUnit = 64;
     public Vector2 spritePivot = new Vector2(0.5f, 1f); // top-center default
     ```
   - Pre-populated with current Kit B sprites from `Assets/Sprites/Environment/KitB_Cliff/`

2. **`CliffAutoPlacer.cs`** (MonoBehaviour, Editor + runtime callable)
   - Path: `Assets/Scripts/Environment/CliffAutoPlacer.cs`
   - Public fields:
     ```csharp
     public Tilemap floorTilemap;
     public CliffPlacementRules rules;
     public Transform cliffParent;  // where to spawn cliff GameObjects (e.g. existing "CliffRing")
     public bool clearExistingOnRegenerate = true;
     ```
   - Method: `[ContextMenu("Regenerate Cliff Ring")] public void Regenerate()`
   - Logic:
     1. Iterate every painted cell in `floorTilemap`
     2. For each cell, check 4 cardinal + 4 diagonal neighbors via `tilemap.HasTile(neighbor)`
     3. For each empty neighbor direction, place a cliff GameObject:
        - South empty → place cliff_S at cell's south rim world position
        - North empty → cliff_N at north rim
        - East/West similar
        - Diagonals (e.g. SE empty + S and E painted) → only if BOTH S and E are also empty (true corner)
     4. Compute world position: `tilemap.GetCellCenterWorld(cell) + dirOffset`
     5. Spawn `new GameObject` with `SpriteRenderer` configured per rules
     6. Parent to `cliffParent`
     7. Randomly swap to accent sprite with `accentChance` probability (for south-facing cliffs in rune zones if accent flag set)

3. **`CliffAutoPlacerEditor.cs`** (Editor, in `Assets/Editor/Environment/`)
   - Custom inspector with "Regenerate" button
   - Validation: warn if floorTilemap or rules unset
   - Show preview count of cliffs to be placed

### Logical placement math (iso grid, cellSize 1×0.609375)

For iso tilemap, cardinal directions in CELL coords:
- S = (0, -1, 0)
- N = (0, +1, 0)
- E = (+1, 0, 0)
- W = (-1, 0, 0)
- SE = (+1, -1, 0)
- NE = (+1, +1, 0)
- SW = (-1, -1, 0)
- NW = (-1, +1, 0)

World offset (where to place the cliff sprite relative to the cell):
- S → cell center + (0, -0.305, 0) (half of cellSize.y = 0.305)
- N → cell center + (0, +0.305, 0)
- E → cell center + (+0.5, 0, 0) (half of cellSize.x = 0.5)
- W → cell center + (-0.5, 0, 0)
- SE → cell center + (+0.5, -0.305, 0)
- etc.

With top-center sprite pivot, the cliff visually hangs DOWN from the rim into the void below.

### Corner detection
A corner cliff (e.g. SE) is placed at a cell where BOTH the south AND east neighbors are empty (or all 3: S+E+SE). If only SE is empty but S and E are painted, NO corner placement (it's not actually a corner edge).

### Optional accent
For each placed cliff (especially south-facing), random chance to swap base sprite with accent (e.g. cliff_cyan_glow) → visual variety. Default 15%.

## DELIVERABLES

1. **`Assets/ScriptableObjects/Environment/CliffPlacementRules.cs`** — ScriptableObject class definition
2. **`Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset`** — preset asset with current Kit B sprites filled
3. **`Assets/Scripts/Environment/CliffAutoPlacer.cs`** — placement component
4. **`Assets/Editor/Environment/CliffAutoPlacerEditor.cs`** — custom inspector with "Regenerate" button
5. **PlayableArena.unity update:**
   - Add `CliffAutoPlacer` component to existing `CliffRing` GameObject
   - Wire references: floorTilemap = scene's Floor/Tilemap, rules = the Hades preset, cliffParent = CliffRing
   - Call Regenerate ONCE in this dispatch to replace manual placement
   - VERIFY result visually matches or improves Cycle 5 output
6. **Done report:** `STAGING/s106_overnight/KIT_B_AUTOPLACER_REPORT.md`
   - Component code outline
   - Number of cliffs generated (vs old 24)
   - Visual comparison (game view before/after)
   - Console 0/0

## CONSTRAINTS
- Single-file per component (don't over-split)
- No new dependencies
- Preserve existing CliffRing GameObject (just clear children + regenerate)
- 0 error 0 warning required
- Don't touch other scene objects (lights, BG, player)

## TEST CRITERIA

1. Open `RIMA/Environment/CliffAutoPlacer` inspector → "Regenerate" button visible
2. Click Regenerate → CliffRing populated with auto-detected cliffs
3. Delete a row of tiles from floor → regenerate → cliff placement updates to match new shape
4. ScriptableObject reusable: create a 2nd preset with different cliff sprites + swap on placer → result uses new sprites
5. Save scene, take screenshot `STAGING/s106_overnight/scene_v7_autoplaced_cliffs.png`

## TIME ESTIMATE
~60-90 min at xhigh.

## DESIGN NOTES (for future)
- This system can be extended later to:
  - Auto-place props (braziers, statues) at corner cells via similar rule system
  - Per-tile-type rules (e.g. cliff_cyan_glow only under rune cells)
  - Multi-Kit support (Kit B for Hades arenas, KitB_Wood for forest arenas, etc.)
- Save the rule asset path in memory for future arena composition reuse.
