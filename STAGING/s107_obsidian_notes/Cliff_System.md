# Cliff System (S107 FINAL)

**Status:** LOCKED 2026-05-26 (agy verdict applied)
**Related:** [[Walkability_Dash]] [[S107_Overnight_Log]] [[Open_Decisions]]

## Configuration

- **Directions:** 3 (S + SE + SW) — was 8, reduced via agy verdict (Hades pattern: only south-facing edges render cliff)
- **Tile count:** 262 (down from 413, 36% reduction)
- **DeterministicVariantTile:**
  - `offset.y = 1.5` (drop face sags into void below floor)
  - `scale = (1, 1)` (no stretch)
  - PPU `64` native (PPU 128 override REVOKED)
- **Sorting:** `CliffTilemap` on `Floor` layer, `order = -1` (renders behind floor sprites, so top deck hides cliff top edge)

## Sprite Assets

Path: `Assets/Sprites/Environment/KitB_Cliff/`

9 PNG files (128×192 RGBA, PPU 64, top-center pivot):

```
cliff_E.png    cliff_N.png    cliff_NE.png
cliff_NW.png   cliff_S.png    cliff_SE.png
cliff_SW.png   cliff_W.png    cliff_cyan_glow.png
```

Source: Codex `$imagegen` pixelified from HD refs (preview at `STAGING/s106_overnight/kit_b_pixelified_preview.png`). Active set uses 3 directions; remaining 5 directional sprites available for future expansion.

## Code Stack

- `Assets/Scripts/Environment/CliffPlacementRules.cs` — ScriptableObject (RIMA.Runtime asmdef)
- `Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset` — current preset
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` — MonoBehaviour, neighbor-based edge detection
- `Assets/Scripts/Environment/DeterministicVariantTile.cs` — variant selector with deterministic hash
- `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` — Regenerate button

## Visual Result

Hades pattern net — south-facing cliff edges only, top deck floor masks cliff tops, drop face sags into the void. Matches `STAGING/s106_overnight/walless_v1_batch2_M3.png` reference.

## Open Items

- Cliff sprite v2 (Python cliff_generator dimetric mocks → PixelLab S-XL Web UI init image) — see [[Open_Decisions]]
- Reachability constraint: portal spawn must not land on cliff-blocked areas — see [[Reward_Portal_Flow]]
