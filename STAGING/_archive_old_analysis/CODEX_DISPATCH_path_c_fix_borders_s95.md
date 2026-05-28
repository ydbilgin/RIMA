# CODEX_DISPATCH — Path C Floor Cluster Paint + Wall Pivot Fix (S95)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Context
Scene `Assets/Scenes/Demo/PathC_BaseTest.unity` v01 painted by previous sub-agent. QC FAIL:
- Floor tiles look like "every cell a different stone block" checkerboard (per-cell random material + per-cell random rotation = tile borders visible)
- Left/right walls offset by 4 world units due to bottom-center pivot + 90° rotation
- Camera ortho size 52 (should be ~40 for 16×10 cellSize=8 scene)

Tile sprite layout: 16 tiles (4 materials × 4 variants) at `Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/`, each 512×512 px, PPU=64 (= 8 world units per cell), pivot Center. Material naming: `floor_A_granite_v01_tile_{0..3}`, `floor_B_cracked_v01_tile_{0..3}`, `floor_C_dirt_v01_tile_{0..3}`, `floor_D_ritual_v01_tile_{0..3}`.

Wall sprite layout: 6 sprites at sprite sheet `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painted_v01/walls_set_v01.png`, 512×512 each, PPU=64, pivot Bottom-Center (0.5, 0). Pieces: 0=straight, 1=corner_NE, 2=corner_NW, 3=arch, 4=cyan_rift, 5=wooden_door (or similar — verify via slice).

## Task — execute via Unity-MCP (use the profile that has Unity-MCP loaded; you have UnityMCP per `cx_dispatch.py`).

### 1. Floor cluster repaint (CRITICAL)
Run Python (via UnityMCP `execute_code` or equivalent) inside Unity Editor:

- Open scene `Assets/Scenes/Demo/PathC_BaseTest.unity`
- Find `Floor_Tilemap` GameObject → its Tilemap component
- **Clear all existing tiles**, then repaint 16×10 grid (x: 0..15, y: 0..9) with this algorithm:

```
# Cluster paint: blob-based material assignment, per-cell variant + rotation
import random
random.seed(42)

# Step 1: Voronoi/blob seeds for 4 materials
# Target distribution: A_granite 50%, B_cracked 25%, C_dirt 20%, D_ritual 5%
seeds = []  # (cx, cy, material_index)
seeds += [(random.uniform(0,16), random.uniform(0,10), 0) for _ in range(5)]   # A granite — 5 blobs
seeds += [(random.uniform(0,16), random.uniform(0,10), 1) for _ in range(3)]   # B cracked — 3 blobs
seeds += [(random.uniform(0,16), random.uniform(0,10), 2) for _ in range(3)]   # C dirt — 3 blobs
seeds += [(random.uniform(2,14), random.uniform(2,8), 3) for _ in range(1)]    # D ritual — 1 blob (focal)

material_tiles = {
    0: [granite_0_GUID, granite_1_GUID, granite_2_GUID, granite_3_GUID],
    1: [cracked_0_GUID, cracked_1_GUID, cracked_2_GUID, cracked_3_GUID],
    2: [dirt_0_GUID, dirt_1_GUID, dirt_2_GUID, dirt_3_GUID],
    3: [ritual_0_GUID, ritual_1_GUID, ritual_2_GUID, ritual_3_GUID],
}

for y in range(10):
    for x in range(16):
        # Find nearest seed (Voronoi) → assigns material
        cx, cy = x + 0.5, y + 0.5
        nearest = min(seeds, key=lambda s: (s[0]-cx)**2 + (s[1]-cy)**2)
        material = nearest[2]
        # Pick variant within material
        variant = random.randint(0, 3)
        tile_asset = material_tiles[material][variant]
        # Random rotation (0/90/180/270) + 50% h-flip
        rot = random.choice([0, 90, 180, 270])
        flip_x = random.random() < 0.5
        # Set tile + transform matrix
        tilemap.SetTile((x, y, 0), tile_asset)
        # Build TRS matrix: scale (1.02, 1.02, 1) for overlap, rotation, optional flip
        scale_x = -1.02 if flip_x else 1.02
        m = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, rot), Vector3(scale_x, 1.02, 1))
        tilemap.SetTransformMatrix((x, y, 0), m)
```

**Result expected:** material A forms 5 organic blobs (avg 16 cells each → ~80 cells = 50%), B 3 blobs (~40 cells = 25%), C 3 blobs (~32 cells = 20%), D 1 small blob (~5-8 cells, focal). Same material across multiple touching cells = no checkerboard. Per-cell rotation/flip + variant pick still varies texture detail.

Distribution must satisfy: A 45-55%, B 20-30%, C 15-25%, D 3-8% (random seed 42 should hit). If wildly off, adjust seed count.

### 2. Sprite extrude (hide seams)
For all 4 floor PNG meta files (`floor_A_granite_v01.png.meta`, `floor_B_cracked_v01.png.meta`, `floor_C_dirt_v01.png.meta`, `floor_D_ritual_v01.png.meta`):
- Change `spriteExtrude: 1` → `spriteExtrude: 8`
- Reimport (AssetDatabase.ImportAsset)

**Why:** Sprite atlas extrude pads edge pixels into adjacent atlas regions, hiding the dark vignette at AI-gen tile borders. The TRS matrix scale 1.02 from step 1 then overlaps cells slightly to fully hide single-pixel gaps.

### 3. Wall pivot offset fix
Current bug: Left walls at x=-68 (4-unit gap to floor edge at x=-64), Right walls at x=68 (4-unit gap to floor edge at x=64).

Fix:
- For every `Wall_Left_y*` GameObject: change `m_LocalPosition.x` from -68 to **-64**
- For every `Wall_Right_y*` GameObject: change `m_LocalPosition.x` from 68 to **64**

**Why:** Wall sprite is 8 units wide, pivot (0.5, 0). After +90° rotation around pivot, sprite extends to LEFT of pivot for left walls; pivot at x=-64 places sprite snug against floor edge.

### 4. Wall coverage gap fix
Verify 10 left + 10 right + 16 top + 16 bot walls exist. If left/right are only 9 walls (currently y1..y9), add y0 and y10 OR shift to cover y range -36 to +36 (step 8, 10 instances at y={-36,-28,-20,-12,-4,4,12,20,28,36}).

Floor y range: cellSize=8, 10 cells = -40 to +40 world units. Wall after rotation is 8 units tall, so position.y is wall center → need centers at y={-36,-28,...,36}.

### 5. Camera ortho size
Find `Main Camera` GameObject → set Camera component `orthographic size` from 52 → **40**.

### 6. Screenshot QC
- Set Game view aspect 16:10 (or 4:3 close)
- Capture screenshot to `STAGING/codex_floor_walls_v01/scene_compose_v02.png`
- Verify 4 gates:
  - **G1 Tile borders:** visible grid lines gone (or significantly muted)
  - **G2 Material clustering:** can see 3-5 large granite regions, not random scatter
  - **G3 Wall flush:** left/right walls touch floor edge (no gap)
  - **G4 Camera framing:** scene fills viewport, walls visible at top/bottom/left/right

If G1 still fails (borders visible despite extrude+overlap), report so user can decide: micro-texture re-gen ($5) vs accept-and-overlay-L2.

### 7. Console errors
Confirm 0 new errors after changes. Pre-existing `WangTileSetWizard` error has been fixed (file moved to `_archive_S73/Wang16/` + wrapped `#if RIMA_WANG16_LIVE`).

## Deliverable
Write `STAGING/CODEX_DONE_path_c_fix_borders_s95.md` with:
- Per-step PASS/FAIL
- Screenshot path
- 4-gate verdict per the criteria above
- Distribution counts (granite/cracked/dirt/ritual cell counts and %)
- Any deviations / blockers

## Files in scope
- `Assets/Scenes/Demo/PathC_BaseTest.unity` (Floor_Tilemap, Wall_*, Main Camera)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/floor_{A,B,C,D}_*.png.meta` (spriteExtrude only)
- `STAGING/codex_floor_walls_v01/scene_compose_v02.png` (new screenshot)
- `STAGING/CODEX_DONE_path_c_fix_borders_s95.md` (new report)

DO NOT modify: tile .asset files, wall sprite sheet, other scenes, scripts outside Editor.
