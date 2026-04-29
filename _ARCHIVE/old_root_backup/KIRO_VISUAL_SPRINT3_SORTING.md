# KIRO TASK — Visual Sprint 3: Sorting Layers + Depth
*Date: 2026-04-11 | Read this file, apply in order. Do not read other files.*

> SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.

---

## RISK LEVEL: LOW
All steps are deterministic. Exact sorting values given. No judgment required.

---

## CONTEXT

RIMA uses Unity 2D. Currently all Tilemap Renderers and prefabs may be on the same
sorting layer, causing depth confusion (characters appear behind walls, etc.).

Goal: establish a clear depth order so the scene reads as layered 3D space.

**Target sorting order (bottom to top):**
```
ContactShadow   → Order -10  (below everything)
Ground Tilemap  → Order  0
Floor Decals    → Order  5
Wall Tilemap    → Order  10
Props (barrels, etc.) → Order 15  (Y-sorted via script if needed)
Character       → Order  20  (player + enemies, Y-sorted)
VFX             → Order  30
UI / Foreground → Order  100
```

---

## FILES TOUCHED

- `RIMA/Assets/Scenes/_Sandbox.unity` (Tilemap Renderer sorting values)
- `RIMA/Assets/Prefabs/Enemies/*.prefab` (SpriteRenderer sorting order)
- `RIMA/Assets/Prefabs/Player.prefab` (SpriteRenderer sorting order)

Do not touch scripts, animations, or any other files.

---

## STOP AND ESCALATE if:
- You find more than 4 Tilemap objects (unexpected — ask Claude)
- A Tilemap is named ambiguously (you can't tell if it's ground or wall)
- Player prefab has multiple SpriteRenderer components at root level

---

## STEP 0 — Find all Tilemaps in scene

```
mcp__UnityMCP__find_gameobjects(name_filter="Tilemap", scene_only=true)
```

Note all names. Expected: Ground, Walls, (maybe Decals/Props).

---

## STEP 1 — Set Ground Tilemap sorting

Find the ground tilemap (named "Ground" or "GroundTilemap" or similar).

```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="[GROUND TILEMAP PATH]",
  component_type="UnityEngine.Tilemaps.TilemapRenderer",
  property_values={
    "sortingOrder": 0
  }
)
```

---

## STEP 2 — Set Wall Tilemap sorting

Find the wall tilemap (named "Walls" or "WallTilemap" or similar).

```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="[WALL TILEMAP PATH]",
  component_type="UnityEngine.Tilemaps.TilemapRenderer",
  property_values={
    "sortingOrder": 10
  }
)
```

---

## STEP 3 — Set Player SpriteRenderer sorting

```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="Player/Sprite",
  component_type="UnityEngine.SpriteRenderer",
  property_values={
    "sortingOrder": 20
  }
)
```

If path is wrong, find Player in scene first:
```
mcp__UnityMCP__find_gameobjects(name_filter="Player", scene_only=true)
```

---

## STEP 4 — Set enemy SpriteRenderer sorting

For each enemy prefab, set their SpriteRenderer sortingOrder to 20.

Find enemies in scene:
```
mcp__UnityMCP__find_gameobjects(name_filter="Enemy", scene_only=true)
```

For each:
```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="[ENEMY PATH]/Sprite",
  component_type="UnityEngine.SpriteRenderer",
  property_values={
    "sortingOrder": 20
  }
)
```

---

## STEP 5 — Set VFX prefab sorting

For each VFX prefab: HitSpark, DeathBurst, ShadowSilhouette
Set their Particle System Renderer or SpriteRenderer sortingOrder to 30.

```
mcp__UnityMCP__manage_asset(action="list", path="Assets/Prefabs/VFX")
```

For each VFX prefab found, set sorting order 30 via Renderer component.

---

## STEP 6 — Save scene

```
mcp__mcp-unity__save_scene()
```

---

## STEP 7 — Final console check

```
mcp__UnityMCP__read_console(log_type="Error")
```

---

## REPORT FILE

**Write to:** `F:\Antigravity Projeler\2d roguelite\KIRO_LAST_REPORT.md`

```
# KIRO REPORT — Visual Sprint 3: Sorting Layers
Date: 2026-04-11

STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - Tilemaps found: [list names]
  - Ground Tilemap → Order 0: YES/NO
  - Wall Tilemap → Order 10: YES/NO
  - Player → Order 20: YES/NO
  - Enemies (N): [list, each YES/NO]
  - VFX → Order 30: YES/NO

ERRORS:
  - [exact error] or NONE

QC_RESULT:
  - Console: PASS/FAIL

NEXT_SIGNAL: "sorting sprint 3 bitti"
```
