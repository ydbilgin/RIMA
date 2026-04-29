# KIRO TASK — Visual Sprint 1: Lighting Pass
*Date: 2026-04-11 | Read this file, apply in order. Do not read other files.*

> SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.

---

## RISK LEVEL: LOW
All steps are deterministic. Exact values given. No judgment required. Unity scene only.

---

## CONTEXT

RIMA's `_Sandbox.unity` has lighting that needs adjustment:
- Center light is weak/blue (needs warm yellow, intensity 0.8)
- Global ambient too bright (needs dark cool purple-grey, 0.25)
- Corner torches too far (radius/intensity adjustment)
- A `LightPulse` component needs to be added to the center point light

The script `Assets/Scripts/VFX/LightPulse.cs` already exists and compiles.

---

## FILES TOUCHED

- `RIMA/Assets/Scenes/_Sandbox.unity` (via Unity MCP tools only)

Do not touch any other file.

---

## STOP AND ESCALATE if:
- You cannot find a named GameObject after trying both names listed
- A light component is missing where it should exist
- You are about to modify anything not listed here
- Unity shows compilation errors before you begin

---

## STEP 0 — Verify compilation is clean

```
mcp__UnityMCP__read_console(log_type="Error")
```

If there are errors: STOP. Report to Claude.
If clean: proceed.

---

## STEP 1 — Find all Light2D GameObjects in scene

```
mcp__UnityMCP__find_gameobjects(name_filter="Light", scene_only=true)
```

Note down the names and paths of all found GameObjects.
Also run:

```
mcp__UnityMCP__find_gameobjects(name_filter="Torch", scene_only=true)
```

Report what you find in your REPORT FILE at the end.

---

## STEP 2 — Adjust Global Light 2D (ambient)

Find the Global Light 2D in the scene (likely named "Global Light 2D" or "GlobalLight").

Set:
- `intensity` = 0.25
- `color` = R:0.38, G:0.38, B:0.63 (cool purple-grey: #606080)

```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="[FULL PATH FROM STEP 1]",
  component_type="UnityEngine.Rendering.Universal.Light2D",
  property_values={
    "intensity": 0.25,
    "color": {"r": 0.38, "g": 0.38, "b": 0.63, "a": 1.0}
  }
)
```

---

## STEP 3 — Adjust Center Point Light

Find the center point light (likely named "CenterLight" or "PointLight" or "RoomLight").

Set:
- `intensity` = 0.8
- `color` = R:1.0, G:0.82, B:0.50 (warm yellow: #FFD080)
- `pointLightOuterRadius` = 7.0 (if it has this property)

```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="[FULL PATH FROM STEP 1]",
  component_type="UnityEngine.Rendering.Universal.Light2D",
  property_values={
    "intensity": 0.8,
    "color": {"r": 1.0, "g": 0.82, "b": 0.50, "a": 1.0}
  }
)
```

---

## STEP 4 — Add LightPulse component to center light

On the same center point light GameObject from Step 3:

```
mcp__UnityMCP__manage_components(
  action="add",
  object_path="[CENTER LIGHT PATH]",
  component_type="RIMA.LightPulse"
)
```

QC: Read console. No errors = PASS. If "RIMA.LightPulse not found" = STOP, report.

---

## STEP 5 — Adjust corner torches

For each torch found in Step 1 (there should be 4):

Set:
- `intensity` = 0.6
- `pointLightOuterRadius` = 3.5
- `color` = R:1.0, G:0.55, B:0.2 (warm orange: #FF8C33)

Apply to each torch one by one:

```
mcp__UnityMCP__manage_components(
  action="modify",
  object_path="[TORCH PATH]",
  component_type="UnityEngine.Rendering.Universal.Light2D",
  property_values={
    "intensity": 0.6,
    "color": {"r": 1.0, "g": 0.55, "b": 0.2, "a": 1.0}
  }
)
```

---

## STEP 6 — Save scene

```
mcp__mcp-unity__save_scene()
```

---

## STEP 7 — Check console

```
mcp__UnityMCP__read_console(log_type="Error")
```

No new errors = PASS.

---

## REPORT FILE — Write before saying anything to user

**Write to:** `F:\Antigravity Projeler\2d roguelite\KIRO_LAST_REPORT.md`

```
# KIRO REPORT — Visual Sprint 1: Lighting
Date: 2026-04-11

STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - Step 1 — Found lights: [list names found]
  - Step 2 — Global ambient: [done/skipped: reason]
  - Step 3 — Center light: [done/skipped: reason]
  - Step 4 — LightPulse added: [done/failed: reason]
  - Step 5 — Torches (N found): [done/skipped: reason]
  - Step 6 — Scene saved

ERRORS:
  - [exact error] or NONE

QC_RESULT:
  - Console: PASS/FAIL — [details]

NEXT_SIGNAL: "lighting sprint 1 bitti"
```
