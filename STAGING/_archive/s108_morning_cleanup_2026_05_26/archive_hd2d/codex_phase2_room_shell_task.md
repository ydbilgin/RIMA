# Phase 2 — Modular Room Shell + Footprint Library

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.

---

## Context

Phase 1 (commit b502fb79) built modular textures + 5 prefabs (WallSegment_Straight/Cracked/Niche, PillarSegment, FloorTile_2x2/Sigil). Room_Sample composition placed hand-arranged in SampleScene.

Phase 2 goal: **footprint-driven modular shell builder** — designer authors a footprint asset, builder spawns walls + floor automatically. User wants **MANY room types** (7+ varieties) and **BIGGER rooms** (up to 24x18 units). Aligns with ChatGPT plan: footprint → boundary → wall module classification → instantiate.

---

## STEP 0 — Required reads

1. `CLAUDE.md` + `CODEX_DISPATCH.md` (rules)
2. `CURRENT_STATUS.md` (S103 + b502fb79)
3. `STAGING/codex_modular_room_task.md` (Phase 1 spec)
4. `Assets/Scenes/SampleScene.unity` hierarchy (find current Room_Sample structure)
5. Existing prefabs in `Assets/Prefabs/Environment/` — note prefab pivots
6. `STAGING/concepts/chatgpt_ref/` — confirm visual target

---

## STEP 1 — Create 2 missing corner wall prefabs

Existing prefabs cover Straight/Cracked/Niche/Pillar/Floor. Phase 2 needs corners for L-shapes:

1. **`Assets/Prefabs/Environment/WallSegment_NE_OuterCorner.prefab`**
   - Cube primitive scale (0.5, 4, 0.5) — short outer corner stub
   - Material: WallMat_StoneA
   - Pivot bottom-center via empty parent
   - BoxCollider auto
   - Purpose: connects N wall end to W wall end at outer corner

2. **`Assets/Prefabs/Environment/WallSegment_NW_InnerCorner.prefab`**
   - Two cube children forming L: one (1, 4, 0.5) along N axis + one (0.5, 4, 1) along W axis, both anchored at corner origin
   - Material: WallMat_StoneA
   - Single empty parent at corner pivot
   - BoxColliders on each
   - Purpose: L-shape concave inner corners (where room shape steps inward)

---

## STEP 2 — Data model (ScriptableObjects)

### 2a. `Assets/Scripts/Environment/Modular/RoomFootprint.cs`

```csharp
[CreateAssetMenu(fileName = "RF_NewFootprint", menuName = "RIMA/Environment/Room Footprint")]
public class RoomFootprint : ScriptableObject
{
    public int widthCells = 8;       // cells along X
    public int heightCells = 8;      // cells along Z
    public float cellSize = 2f;      // world units per cell

    [TextArea(4, 16)]
    public string occupancyAscii = ""; // '#' = filled cell, '.' = empty. Rows top to bottom.

    public bool[] GetOccupancyGrid()
    {
        var grid = new bool[widthCells * heightCells];
        if (string.IsNullOrEmpty(occupancyAscii))
        {
            for (int i = 0; i < grid.Length; i++) grid[i] = true; // default full rect
            return grid;
        }
        var rows = occupancyAscii.Replace("\r", "").Split('\n');
        for (int z = 0; z < heightCells && z < rows.Length; z++)
        {
            var row = rows[heightCells - 1 - z]; // ASCII top-to-bottom = world +Z to 0
            for (int x = 0; x < widthCells && x < row.Length; x++)
            {
                grid[z * widthCells + x] = (row[x] == '#');
            }
        }
        return grid;
    }
}
```

ASCII grid keeps authoring sane. Top row = +Z (north), bottom = 0. Left = -X, right = +X.

### 2b. `Assets/Scripts/Environment/Modular/WallModuleLibrary.cs`

```csharp
[CreateAssetMenu(fileName = "WallModuleLibrary", menuName = "RIMA/Environment/Wall Module Library")]
public class WallModuleLibrary : ScriptableObject
{
    [Header("N wall (faces -Z toward camera)")]
    public GameObject[] northStraightPrefabs;     // pick at random per cell
    public GameObject[] northVariantPrefabs;      // niche/cracked mixed

    [Header("W wall (faces +X)")]
    public GameObject[] westStraightPrefabs;
    public GameObject[] westVariantPrefabs;

    [Header("Corners")]
    public GameObject neOuterCornerPrefab;
    public GameObject nwInnerCornerPrefab;

    [Header("Detail")]
    public GameObject pillarPrefab;
    public int pillarEveryNCells = 3;

    [Header("Floor")]
    public GameObject[] floorTilePrefabs;          // base + variants
    public GameObject sigilFloorPrefab;            // rare
    public float sigilFloorChance = 0.05f;

    [Header("Variation")]
    [Range(0f, 1f)] public float wallVariantChance = 0.25f;  // chance to use variant over straight
}
```

### 2c. Create asset `Assets/Data/Environment/WallLib_ShatteredKeep.asset`
- Populate references:
  - northStraightPrefabs = [WallSegment_Straight]
  - northVariantPrefabs = [WallSegment_Cracked, WallSegment_Niche]
  - westStraight = [WallSegment_Straight] (rotated by builder)
  - westVariant = [WallSegment_Cracked, WallSegment_Niche]
  - neOuterCorner = WallSegment_NE_OuterCorner
  - nwInnerCorner = WallSegment_NW_InnerCorner
  - pillar = PillarSegment
  - floorTilePrefabs = [FloorTile_2x2]
  - sigilFloorPrefab = FloorTile_Sigil

Note: For Phase 2 the SAME straight prefab is used for both N and W walls; the builder applies Y rotation (90° for W walls). Future Phase: separate N/W families per ChatGPT.

---

## STEP 3 — `RoomShellBuilder.cs`

`Assets/Scripts/Environment/Modular/RoomShellBuilder.cs`:

```csharp
[ExecuteAlways]
public class RoomShellBuilder : MonoBehaviour
{
    public RoomFootprint footprint;
    public WallModuleLibrary library;
    public int randomSeed = 0;

    [ContextMenu("Rebuild")]
    public void Rebuild()
    {
        Clear();
        if (footprint == null || library == null) return;
        var grid = footprint.GetOccupancyGrid();
        BuildFloor(grid);
        BuildNorthWalls(grid);
        BuildWestWalls(grid);
        BuildCorners(grid);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(child);
            else DestroyImmediate(child);
        }
    }

    // BuildFloor: for each occupied cell, instantiate floor tile at (x*cellSize, 0, z*cellSize) with cell-center offset
    // BuildNorthWalls: for each cell (x, z) where occupied(x,z) AND NOT occupied(x, z+1) — that cell's north edge is on the room boundary — place N wall segment at (cellX, 0, cellZ + cellSize/2)
    // BuildWestWalls: for each cell where occupied AND NOT occupied(x-1, z) — west edge boundary — place W wall segment rotated 90Y
    // BuildCorners: detect concave corners (occupied cell with neighbors empty on N+W simultaneously) → place inner corner. Place pillar at intervals along N walls.

    // ... (implementation follows the same edge-classification idea; full code in deliverable)
}
```

Notes:
- Coordinate convention: cell (x, z) world position = (transform.position + (x - widthCells/2) * cellSize, 0, (z - heightCells/2) * cellSize). Footprint centered at builder transform.
- Wall segment world Y position: y=0 for floor-anchored prefabs (pivot at bottom).
- Use `System.Random` seeded per builder for reproducible variants.
- Skip generating S/E walls (HD-2D camera-facing sides hidden).

---

## STEP 4 — Create 7 footprint .asset files

Path: `Assets/Data/Environment/Footprints/`

For each, set `cellSize=2`, `widthCells` + `heightCells` as below, and ASCII grid:

### 4a. `RF_SmallRect_8x8.asset` — 4x4 cells (8x8 units)
```
####
####
####
####
```

### 4b. `RF_MedRect_12x12.asset` — 6x6 cells (12x12 units)
```
######
######
######
######
######
######
```

### 4c. `RF_WideArena_24x16.asset` — 12x8 cells (24x16 units)
```
############
############
############
############
############
############
############
############
```

### 4d. `RF_LShape_16x16.asset` — 8x8 cells with 4x4 SE quadrant cut
```
########
########
########
########
####....
####....
####....
####....
```

### 4e. `RF_TShape_18x14.asset` — 9x7 cells, T topology
```
.........
.........
.........
.........
###.###..  → fix to: ...###...
...###...
...###...
```
Use clean T:
```
.........
.........
.........
.........
...###...
...###...
...###...
```
Wait — T has crossbar + stem. Use:
```
#########
#########
#########
###...###
...#.....   → cleaner:
```
Final clean T (9 cols, 7 rows):
```
#########
#########
#########
....#....
....#....
....#....
....#....
```

### 4f. `RF_PlusShape_14x14.asset` — 7x7 cells, symmetric plus
```
..###..
..###..
..###..
#######
#######
..###..
..###..
```
Use 7x7:
```
..###..
..###..
..###..
#######
..###..
..###..
..###..
```

### 4g. `RF_BigArena_30x22.asset` — 15x11 cells (30x22 units) — BIG arena per user request
```
###############
###############
###############
###############
###############
###############
###############
###############
###############
###############
###############
```

---

## STEP 5 — Update SampleScene with BIG arena demo

1. Delete current `Room_Sample` GameObject (composed manually in Phase 1) from SampleScene.
2. Create new empty GO `Room_Demo` at (0, 0, 0).
3. Add `RoomShellBuilder` component.
4. Assign `footprint = RF_BigArena_30x22.asset` (the 30x22 big room — user wants bigger).
5. Assign `library = WallLib_ShatteredKeep.asset`.
6. Call `Rebuild()` via context menu (or in OnValidate when Application.isEditor).
7. Re-position Player + mobs INSIDE the new room footprint:
   - Player at (0, 0.97, 0)
   - Mob_Zombie at (-10, 1, 8)
   - Mob_Skeleton at (10, 1, 8)
   - Mob_Bat at (-3, 3, 9)
   - Boss_Zombie at (3, 1.5, 9)
8. Adjust Main Camera: position (16, 12, -16), rotation (35, 315, 0), orthographic size 12. Per ChatGPT camera band 35-45° X tilt.
9. Save SampleScene.

---

## STEP 6 — Verify

1. `read_console` MCP — must be clean.
2. Screenshot via `manage_camera` action=screenshot, save to `Assets/Screenshots/codex_phase2_bigarena_v1.png`.
3. Visual check: BIG room visible, modular walls assembled from footprint, pillars at intervals, floor tiles with variants, ChatGPT_ref aesthetic preserved.
4. Test other footprints: in a separate scene `Assets/Scenes/RoomShowcase.unity`, lay out 6 builders side-by-side (small/med/wide/L/T/plus footprints, spacing 40 units between), one Main Camera high+wide ortho to see all 6. Take screenshot `Assets/Screenshots/codex_phase2_showcase.png`.

---

## STEP 7 — Commit + report

Commit message:
```
[Codex] [S103 PHASE2 ROOM SHELL] Footprint SO + WallModuleLibrary + RoomShellBuilder + 7 footprints + big arena demo

- RoomFootprint with ASCII occupancy authoring
- WallModuleLibrary references all wall/floor prefabs + variation rules
- RoomShellBuilder.cs reads footprint, classifies boundary edges, instantiates modules
- 2 new corner prefabs (NE outer, NW inner) for L-shapes
- 7 footprints: SmallRect/MedRect/WideArena/LShape/TShape/PlusShape/BigArena
- SampleScene: BigArena 30x22 demo replaces Room_Sample
- RoomShowcase scene: 6 footprints side-by-side
- Camera tilt bumped 30 → 35 (ChatGPT plan band)

Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
```

Write summary to `CODEX_DONE.md`:
- STATUS, COMMIT, FILES_TOUCHED
- FOOTPRINTS_CREATED: 7 paths
- SCREENSHOTS: 2 paths
- ISSUES
- NEXT_SIGNAL: "phase2_room_shell_complete"

---

## Constraints

- HD-2D ONLY: build N + W walls (toward back-left of camera). NO S or E walls.
- Camera tilt 35° (bump from 30°).
- Pivot convention: prefab world Y=0 = floor surface. Walls grow up from Y=0.
- Coordinate: builder origin = footprint center. All cells offset relative to center.
- Do NOT create animation, dialogue, props, lighting, or movement scripts. Phase 3 covers those.
- Do NOT delete existing Phase 1 textures/materials/prefabs.
- Do NOT modify Player / Mob_* / Boss_* sprites or scale — only their positions.
- If ASCII parser logic gets complex, keep it under 30 lines; document the convention inline.
- STOP after STEP 7. Phase 3 (connectors + room chains) is separate dispatch.
