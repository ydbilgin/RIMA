# KIRO TASK — VOID THRALL & HALF THRALL
*Date: 2026-04-07 | Read this file, apply in order. Do not read other files.*

---

## DECISION
**Tool: PixelLab MCP**
Enemy animations → mob quality threshold, player sees these less than player animations. MCP sufficient.
HalfThrall does not exist yet in PixelLab — must be created first before any animation.

---

## RISK LEVEL: LOW
> Deterministic · Mechanical · Isolated · Bounded · Mechanically verifiable ✓

---

## CREDENTIALS

**PixelLab Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## CONTEXT

VoidThrall exists in PixelLab but has no attack or death animations. HalfThrall does NOT exist in PixelLab — create it first (Task 3), then generate all its animations. Final output: VoidThrall attack + death (5 dirs each), HalfThrall idle + walk + attack + death (5 dirs each). Claude Code builds .anim clips and AnimatorController afterward.

---

## FILES TOUCHED

- `Assets/Sprites/Enemies/VoidThrall/attack/` (new subfolder)
- `Assets/Sprites/Enemies/VoidThrall/death/` (new subfolder)
- `Assets/Sprites/Enemies/HalfThrall/` (new folder — character creation)
- `Assets/Sprites/Enemies/HalfThrall/idle/`
- `Assets/Sprites/Enemies/HalfThrall/walk/`
- `Assets/Sprites/Enemies/HalfThrall/attack/`
- `Assets/Sprites/Enemies/HalfThrall/death/`
- `Assets/_STAGING/DONE.txt`

Do not touch any file not listed above.

---

## STOP AND ESCALATE — Report to Claude if:

- HalfThrall creation fails or returns unexpected result
- HalfThrall visually does not match VoidThrall aesthetic (wrong colors, wrong size, not void-themed)
- Frame output is black, empty, or shows wrong character
- Any step requires a decision or judgment

---

## MANDATORY QC — APPLY AFTER EVERY DIRECTION

1. Read `frame_000.png` immediately after generation
2. Describe exactly: character, action, direction
3. PASS if: correct character visible, action clearly matches description, direction correct, no black frames
4. FAIL if: black frame, wrong direction, wrong action, wrong character
5. On FAIL: re-generate with `-v2` suffix appended to animation_name, more explicit description
6. Never save failing frames

---

## STEP 0 — Find VoidThrall ID

```
mcp__pixellab__list_characters()
```

Note VoidThrall character ID.

---

## DIRECTIONS (5 per animation)

south · north · west · south-west · north-west

---

## TASK 1 — VoidThrall "Void Pulse" Attack

**Character:** VoidThrall (large bloated void-corrupted creature, dark purple/black void energy emanating from its chest)
**Action:** Arms spread wide opening chest (frames 1-2), void energy visibly builds and pulses from chest (frames 3-4), energy bursts outward in a radial wave from chest (frames 5-6). Slow heavy motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[VOIDTHRALL_ID]",
  animation_name="void-pulse-attack",
  direction="south",
  n_frames=6,
  action_description="Large bloated void-corrupted creature facing downward toward camera, dark purple-black void energy emanating from body. Spreads arms wide outward to expose chest, dark void energy visibly builds and pulses intensely from the chest area, then releases/bursts outward in a short-range radial shockwave from the chest. Heavy slow deliberate motion. No loop. Final frame: arms still spread, energy dissipating."
)
```

Repeat for: north · west · south-west · north-west

**Direction word map:**
| direction param | Write in description |
|---|---|
| north | "facing upward / away from camera" |
| west | "facing left" |
| south-west | "toward the lower-left diagonal" |
| north-west | "toward the upper-left diagonal" |

**QC pass:** VoidThrall visible, arms spread, void energy burst from chest, direction correct
**QC fail:** Punch motion, no energy effect visible, wrong direction

**Save path:**
```
Assets/Sprites/Enemies/VoidThrall/attack/south/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/VoidThrall/attack/north/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/VoidThrall/attack/west/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/VoidThrall/attack/south-west/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/VoidThrall/attack/north-west/frame_000.png ... frame_005.png
```

---

## TASK 2 — VoidThrall Death Animation

**Action:** Body convulses, void energy leaks and flickers wildly (frames 1-3), then collapses forward/sideways like a bag of flesh losing all tension (frames 4-7). Final frame: collapsed heap on ground. No loop.

```
mcp__pixellab__animate_character(
  character_id="[VOIDTHRALL_ID]",
  animation_name="void-thrall-death-fall",
  direction="south",
  n_frames=7,
  action_description="Large bloated void-corrupted creature facing downward toward camera. Body convulses violently, dark void energy flickering and leaking chaotically from its body, then the creature's massive body collapses forward toward camera — losing all structural integrity like a deflating sack, falling face-forward. No loop. Final frame: creature lying flat on the ground."
)
```

Repeat for: north · west · south-west · north-west

**QC pass:** VoidThrall visible, body collapses, direction correct, final frame on ground
**QC fail:** Standing at end, wrong direction, black frame

**Save path:**
```
Assets/Sprites/Enemies/VoidThrall/death/south/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/VoidThrall/death/north/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/VoidThrall/death/west/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/VoidThrall/death/south-west/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/VoidThrall/death/north-west/frame_000.png ... frame_006.png
```

---

## TASK 3 — Create HalfThrall Character

HalfThrall is a smaller, faster, weaker version of VoidThrall. Same void corruption aesthetic, but smaller body, less bloated, more agile-looking. Canvas: 32px — intentionally tiny, appears in swarms.

```
mcp__pixellab__create_character(
  name="HalfThrall",
  description="Very small void-corrupted creature, a tiny diminutive version of VoidThrall. Hunched compact posture, dark purple-black skin with void energy cracks glowing faintly. Much smaller and faster than VoidThrall, barely half the size. Sharp claws, hollow void eyes. Appears in swarms. Top-down 2D pixel art game character.",
  size=32,
  outline="selective",
  shading="medium",
  detail="medium",
  ai_freedom=450
)
```

**QC pass:** Small creature visible, clearly void-themed (dark colors, glowing cracks), clearly smaller/different from VoidThrall, matches overall aesthetic
**QC fail:** Same size as VoidThrall, no void aesthetic, completely different visual style

Note the returned character_id as **HALFTHRALL_ID**.

**Save reference sprite:**
```
Assets/Sprites/Enemies/HalfThrall/HalfThrall_S.png
```

---

## TASK 4 — HalfThrall Idle

```
mcp__pixellab__animate_character(
  character_id="[HALFTHRALL_ID]",
  animation_name="half-thrall-idle",
  direction="south",
  n_frames=6,
  action_description="Small hunched void-corrupted creature facing downward toward camera, dark void energy faintly pulsing from body cracks. Slight swaying idle stance, claws loosely at sides, faint breathing motion visible, subtle void energy flicker. Loop animation."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Enemies/HalfThrall/idle/{direction}/frame_000.png ... frame_005.png
```

---

## TASK 5 — HalfThrall Walk

```
mcp__pixellab__animate_character(
  character_id="[HALFTHRALL_ID]",
  animation_name="half-thrall-walk",
  direction="south",
  n_frames=6,
  action_description="Small hunched void-corrupted creature walking toward camera (facing downward), quick skittering movement, low center of gravity, claws forward, dark void energy trailing slightly. Loop animation."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Enemies/HalfThrall/walk/{direction}/frame_000.png ... frame_005.png
```

---

## TASK 6 — HalfThrall "Void Grab" Attack

**Action:** Lunges forward with both claws outstretched (frames 1-2), swipes claws in grabbing/tearing motion (frame 3), retracts (frame 4). Fast compact motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[HALFTHRALL_ID]",
  animation_name="half-thrall-void-grab",
  direction="south",
  n_frames=4,
  action_description="Small hunched void-corrupted creature facing downward toward camera. Lunges forward toward camera with both claws outstretched aggressively, performs a quick grabbing/slashing claw swipe motion directly toward camera, then retracts. Fast aggressive compact motion. No loop."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Enemies/HalfThrall/attack/{direction}/frame_000.png ... frame_003.png
```

---

## TASK 7 — HalfThrall Death Animation

```
mcp__pixellab__animate_character(
  character_id="[HALFTHRALL_ID]",
  animation_name="half-thrall-death-fall",
  direction="south",
  n_frames=6,
  action_description="Small hunched void-corrupted creature facing downward toward camera. Gets hit, staggers backward briefly, then collapses sideways onto the ground — small body crumpling and going still. No loop. Final frame: creature lying flat on the ground, motionless."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Enemies/HalfThrall/death/{direction}/frame_000.png ... frame_005.png
```

---

## COMPLETION LOG

Append to `Assets/_STAGING/DONE.txt`:

```
[DONE-VOID_HALFTHRALL] VoidThrall void-pulse-attack — 5 directions | 2026-04-07
[DONE-VOID_HALFTHRALL] VoidThrall void-thrall-death-fall — 5 directions | 2026-04-07
[DONE-VOID_HALFTHRALL] HalfThrall created — ID: [ID] | 2026-04-07
[DONE-VOID_HALFTHRALL] HalfThrall idle — 5 directions | 2026-04-07
[DONE-VOID_HALFTHRALL] HalfThrall walk — 5 directions | 2026-04-07
[DONE-VOID_HALFTHRALL] HalfThrall void-grab attack — 5 directions | 2026-04-07
[DONE-VOID_HALFTHRALL] HalfThrall death-fall — 5 directions | 2026-04-07
[QC-RESULT] Each direction checked, character correct, action correct | 2026-04-07
```

---

## AFTER KIRO FINISHES — Claude Code handles

Tell Claude: **"void halfthrall hazır"**

Claude will:
1. `AssetDatabase.Refresh()`
2. Import sprites PPU=64, Point, No compression
3. Build .anim clips for VoidThrall attack + death (5 dirs + east mirrors)
4. Build all .anim clips for HalfThrall (idle/walk/attack/death, 5 dirs + east mirrors)
5. Build HalfThrall AnimatorController with all states + BlendTrees
6. Wire VoidThrall controller attack + death states
7. Assign to prefabs in scene
8. Update CURRENT_STATUS.md
