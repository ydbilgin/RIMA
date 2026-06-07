# REVIEW TASK: Character pivot calibration + sorting unification (commit 917c8a6d) and socket-driven doors/spawn (commit 20d1f09c)

ACTIVE RULES: (1) think before reviewing (2) evidence with file:line (3) read-only — do NOT modify files (4) BLOCKED if unclear.

You are reviewing two commits in the RIMA Unity project (CWD = project root). Unity Editor is open; UnityMCP available for verification probes. Output VERDICT per commit: PASS / PASS-WITH-NOTES / FAIL with findings (severity MAJOR/MINOR, file:line evidence).

## Commit 1: `917c8a6d` — char pivot + sorting (written by Sonnet agent)
Claims:
- 82 rotation sprites under Assets/Art/Characters had pivot stuck at 0.5 → re-pivoted to alpha-scanned feet line; Assets/Resources/Characters idle sprites were already OK (~0.25).
- New editor window Assets/Editor/Tools/CharacterPivotCalibrationWindow.cs (menu RIMA/Characters/Calibration): per-class audit table + Apply Auto + nudge field (EditorPrefs).
- Player Body + WeaponSprite SpriteRenderers: Default → Entities layer; IsoSorter added to Body; HandAnchorAttach.UpdateWeaponSortOrder still valid (weapon sorts ±1 relative to body, same layer).
- 16 mob prefabs (ShatteredKeep_PixelLab): → Entities + IsoSorter.
- Dead "Player" sorting layer (uniqueID=0 dup) removed from TagManager.asset.
- Assets/Resources/Prefabs/Player.prefab deleted (zero-reference duplicate; keep = Assets/Prefabs/Player.prefab).

Review focus:
1. Pivot math: does the calibration window's alpha-scan handle the 64px-character-in-120/128px-canvas correctly (per-sprite scan, not assumed offset)? Multi-sprite sheets handled? Any sprite import settings broken (filterMode/PPU/compression must stay Point/64/uncompressed)?
2. Sorting: any renderer left on Default that should be Entities (projectiles? death decal interplay — MobDeathResidue copies sortingLayerName from source renderer: does a mob dying now put decals on Entities instead of Ground, breaking under-feet rendering)? IsoSorter on Body child vs root — does it use the BODY transform y or root y, and is that consistent with mobs?
3. TagManager edit: removing the layer entry — any scene/prefab referencing the removed layer ID? (grep)
4. Prefab deletion: confirm Assets/Prefabs/Player.prefab is the one referenced by Warblade prefabs and nothing loads "Prefabs/Player" from Resources at runtime anymore (ChamberSelectBootstrap.cs:257 has a dead fallback — note it).
5. PLAY PROBE (UnityMCP): load _Arena flow or use RoomRunDirector path — verify player feet sit on tile at spawn (player.position vs cell center), player renders IN FRONT of a prop when below it and BEHIND when above it.

## Commit 2: `20d1f09c` — socket-driven exit doors + validated south spawn (written by cx/Codex)
Claims: north-socket-anchored door row in IsoRoomBuilder.BuildExitDoors (fallback heuristic + warning when no north socket), RoomRunDirector spawn validation + bottom-center walkable fallback, RoomTemplateValidator socket checks, RoomSocketQCTool (Audit/Fix menu items), 15 Generated templates re-socketed, new EditMode test.

Review focus:
1. BuildExitDoors: row centering math around anchor; clamp when 3 doors exceed walkable width near anchor; returned-door contract intact (RoomRunDirector attaches triggers — check it still works for 1/2/3 doors).
2. Spawn fallback correctness: "bottom-center walkable" — ties broken deterministically? Could it pick an isolated walkable cell (disconnected from main floor)?
3. Validator: false-positive risk on corridor/odd shapes (zigzag, crescent)?
4. Template data spot-check: combat_large_donut_01 — spawn no longer adjacent to north door, spawn south-side walkable?
5. Anything that breaks Chamber_CharSelect (excluded from fixes but builder code is shared)?

## Output
Write your full review to `STAGING/_review_charcalib_doors_axopus.md`: verdict per commit + findings table (severity, file:line, suggested fix). Do not fix anything yourself.
