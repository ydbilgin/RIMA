# KIRO TASK — Visual Sprint 2: Contact Shadows
*Date: 2026-04-11 | Read this file, apply in order. Do not read other files.*

> SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.

---

## RISK LEVEL: LOW
All steps are deterministic. Exact values given. Prefab creation + component wiring only.

---

## CONTEXT

Add a "contact shadow" (blob shadow) under the player and all enemy prefabs.
This is a dark semi-transparent circle sprite that sits under the character at ground level.
It gives the impression that characters are standing ON the floor, not floating above it.

There is no script needed — it's a static child GameObject with a SpriteRenderer.
Use Unity's built-in circle sprite (or create a 32x32 dark circle).

---

## FILES TOUCHED

- `RIMA/Assets/Prefabs/Player.prefab` (add child)
- `RIMA/Assets/Prefabs/Enemies/*.prefab` (add child to each)
- `RIMA/Assets/Prefabs/VFX/ContactShadow.prefab` (create new)

Do not touch any other file.

---

## STOP AND ESCALATE if:
- Player prefab path is different from listed above
- You cannot find enemy prefabs
- Unity shows errors after any step

---

## STEP 0 — Find prefab paths

```
mcp__UnityMCP__find_gameobjects(name_filter="Player", scene_only=true)
```

Also check: `Assets/Prefabs/Player.prefab` exists.

```
mcp__UnityMCP__manage_asset(action="list", path="Assets/Prefabs/Enemies")
```

Note all enemy prefab names.

---

## STEP 1 — Create ContactShadow prefab

Create a new GameObject with these exact properties:

Name: `ContactShadow`

Add SpriteRenderer component:
- Sprite: Unity built-in "Knob" sprite (circular, white) — use `Resources.Load` or assign via editor
- Color: R:0, G:0, B:0, A:0.35 (black, 35% opacity)
- Sorting Layer: `Default`
- Order in Layer: `-10` (below everything)

Scale: X:0.6, Y:0.3, Z:1 (flat oval — shadow looks flat on ground)

```
mcp__UnityMCP__manage_gameobject(
  action="create",
  name="ContactShadow",
  position={"x": 0, "y": -0.3, "z": 0}
)
```

```
mcp__UnityMCP__manage_components(
  action="add",
  object_path="ContactShadow",
  component_type="UnityEngine.SpriteRenderer",
  property_values={
    "color": {"r": 0, "g": 0, "b": 0, "a": 0.35},
    "sortingOrder": -10
  }
)
```

Set scale:
```
mcp__mcp-unity__scale_gameobject(
  object_path="ContactShadow",
  scale={"x": 0.6, "y": 0.3, "z": 1.0}
)
```

Save as prefab:
```
mcp__UnityMCP__manage_prefabs(
  action="create",
  gameobject_path="ContactShadow",
  prefab_path="Assets/Prefabs/VFX/ContactShadow.prefab"
)
```

---

## STEP 2 — Add ContactShadow to Player prefab

Open Player prefab, add ContactShadow as child at local position (0, -0.3, 0):

```
mcp__UnityMCP__manage_prefabs(
  action="modify",
  prefab_path="Assets/Prefabs/Player.prefab"
)
```

Add child GameObject named "ContactShadow" with:
- Local position: X:0, Y:-0.3, Z:0
- SpriteRenderer: same settings as Step 1

**QC:** Player prefab now has a child named "ContactShadow". PASS.

---

## STEP 3 — Add ContactShadow to each enemy prefab

For each enemy prefab found in Step 0:

Add a child named "ContactShadow" with:
- Local position: X:0, Y:-0.2, Z:0 (slightly less offset — enemies are smaller)
- SpriteRenderer: Color R:0, G:0, B:0, A:0.30, Order:-10
- Scale: X:0.5, Y:0.25, Z:1

Apply to each prefab one by one.

**QC after each:** Prefab has child "ContactShadow". No console errors. PASS.

---

## STEP 4 — Check console

```
mcp__UnityMCP__read_console(log_type="Error")
```

No errors = PASS.

---

## STEP 5 — Save scene

```
mcp__mcp-unity__save_scene()
```

---

## REPORT FILE

**Write to:** `F:\Antigravity Projeler\2d roguelite\KIRO_LAST_REPORT.md`

```
# KIRO REPORT — Visual Sprint 2: Contact Shadows
Date: 2026-04-11

STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - ContactShadow prefab created: YES/NO
  - Player prefab: shadow added YES/NO
  - Enemy prefabs (N total): [list names, each YES/NO]

ERRORS:
  - [exact error] or NONE

QC_RESULT:
  - Console: PASS/FAIL

NEXT_SIGNAL: "shadow sprint 2 bitti"
```
