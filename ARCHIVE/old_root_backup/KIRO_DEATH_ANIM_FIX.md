# KIRO TASK — Death Animation Fix
*Date: 2026-04-05 | Read this file, apply in order. Do not read other files.*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## CONTEXT

The current death animations for RelicCaster, ChainWarden, and VoidThrall are wrong:
- **RelicCaster** `falling-back-death/` folder: contains magic-casting poses, NOT death frames
- **ChainWarden** `falling-back-death/` folder: character only crouches, doesn't fall
- **VoidThrall**: no death animation folder exists at all

All three need correct death animations: **character stumbles and falls backward to the ground**.

---

## MANDATORY QC PROTOCOL — APPLY AFTER EVERY DIRECTION

After each `animate_character` call completes:

1. **Read `frame_000.png`** of that direction using your file read tool
2. **Visually inspect**: Is the character in a falling/collapsing pose? Or is it standing, attacking, casting?
3. **PASS criteria**: Character is visibly falling backward OR stumbling toward the ground
4. **FAIL criteria**: Character is standing upright, casting a spell, punching, walking, or idle
5. If **FAIL**: call `animate_character` again with a different `animation_name` suffix (e.g. `-v2`, `-v3`) and stricter description. Then re-check.
6. If **PASS**: overwrite the target files and move to next direction.

**Do not save frames that fail QC.**

---

## STEP 0 — Find Character IDs

```
mcp__pixellab__list_characters()
```

From the result, note the character IDs for:
- "RelicCaster" → save as RELICASTER_ID
- "ChainWarden" → save as CHAINWARDEN_ID
- "VoidThrall" → save as VOIDTHRALL_ID

---

## TASK 1 — RelicCaster / death animation (all 8 directions)

Use animation name `"death-fall"` (different from the existing broken `"falling-back-death"` name to avoid cache issues).

For each of the 8 directions, call:

```
mcp__pixellab__animate_character(
  character_id=RELICASTER_ID,
  animation_name="death-fall",
  directions=["[DIRECTION]"],
  action_description="robed mage character death sequence: STARTS standing upright holding staff, gets hit and recoils backward, staggers and loses balance, falls backward collapsing toward the ground, ENDS lying flat on ground with staff dropped, body completely horizontal on floor, death animation from standing to lying down, no loop"
)
```

Directions (do all 8 in order):
`south` · `north` · `west` · `east` · `south-west` · `south-east` · `north-west` · `north-east`

**After each direction: apply QC protocol above.**

**Save path (overwrite existing files):**
```
Assets/Sprites/Enemies/RelicCaster/animations/falling-back-death/[direction]/frame_000.png
Assets/Sprites/Enemies/RelicCaster/animations/falling-back-death/[direction]/frame_001.png
...
Assets/Sprites/Enemies/RelicCaster/animations/falling-back-death/[direction]/frame_006.png
```

---

## TASK 2 — ChainWarden / death animation (all 8 directions)

```
mcp__pixellab__animate_character(
  character_id=CHAINWARDEN_ID,
  animation_name="death-fall",
  directions=["[DIRECTION]"],
  action_description="heavily armored guardian warrior death sequence: STARTS standing upright in combat stance with weapon ready, gets hit and recoils backward with armor clanking, staggers losing balance, falls backward collapsing toward ground with heavy impact, ENDS lying completely flat on ground with body horizontal, armor settled, death animation from standing to lying down, no loop"
)
```

Directions: `south` · `north` · `west` · `east` · `south-west` · `south-east` · `north-west` · `north-east`

**After each direction: apply QC protocol.**

**Save path:**
```
Assets/Sprites/Enemies/ChainWarden/animations/falling-back-death/[direction]/frame_000.png
...
Assets/Sprites/Enemies/ChainWarden/animations/falling-back-death/[direction]/frame_006.png
```

---

## TASK 3 — VoidThrall / death animation (all 8 directions)

This folder does NOT exist yet — you must create it.

```
mcp__pixellab__animate_character(
  character_id=VOIDTHRALL_ID,
  animation_name="death-fall",
  directions=["[DIRECTION]"],
  action_description="undead zombie creature death sequence: STARTS standing upright in threatening pose with void energy crackling, gets hit and recoils backward with void energy flickering, staggers and loses balance, collapses backward falling toward ground as void energy dissipates and fades, ENDS lying completely flat on ground with body horizontal, void energy gone, death animation from standing to lying down, no loop"
)
```

Directions: `south` · `north` · `west` · `east` · `south-west` · `south-east` · `north-west` · `north-east`

**After each direction: apply QC protocol.**

**Save path (create new directories):**
```
Assets/Sprites/Enemies/VoidThrall/animations/falling-back-death/[direction]/frame_000.png
...
Assets/Sprites/Enemies/VoidThrall/animations/falling-back-death/[direction]/frame_006.png
```

---

## TASK 4 — Verify attack animations (quick check only)

Read `frame_000.png` from each of these folders:
- `Assets/Sprites/Enemies/RelicCaster/animations/cross-punch/south/frame_000.png`
- `Assets/Sprites/Enemies/ChainWarden/animations/cross-punch/south/frame_000.png`

For each:
- If the character looks like it's attacking (punching, swinging, casting) → write `[PASS] [name]/cross-punch looks correct` in DONE.txt
- If it looks wrong (standing idle, dying, walking) → write `[FAIL] [name]/cross-punch WRONG — [describe what you see]` in DONE.txt

Do NOT regenerate attack animations in this task. Just report.

---

## COMPLETION LOG

After finishing all tasks, append to `Assets/STAGING/DONE.txt`:

```
[DONE-DEATH] RelicCaster/falling-back-death — 8 directions — YYYY-MM-DD
[DONE-DEATH] ChainWarden/falling-back-death — 8 directions — YYYY-MM-DD
[DONE-DEATH] VoidThrall/falling-back-death — 8 directions (NEW) — YYYY-MM-DD
[QC-ATTACK] RelicCaster/cross-punch: [PASS/FAIL — describe]
[QC-ATTACK] ChainWarden/cross-punch: [PASS/FAIL — describe]
```

---

## AFTER KIRO FINISHES — Claude Code will handle

When the user says "kiro death fix bitti" (or similar), Claude Code will:
1. Run `AssetDatabase.Refresh()` — import new sprites
2. Rebuild enemy animation clips via RIMA menu
3. Assign new death clips to VoidThrall controller's Death state
4. Reset all mob SR colors to white (safety pass)
5. Enter play mode, take screenshot
6. Update CURRENT_STATUS — close BUG-7, BUG-8, BUG-9
