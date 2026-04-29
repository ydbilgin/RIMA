# KIRO TASK — KIRO_CHAR_ATTACKS
*Date: 2026-04-06 | Read this file, apply in order. Do not read other files.*

---

## RISK LEVEL: LOW
> Deterministic · Mechanical · Isolated · Bounded · Mechanically verifiable ✓

---

## CREDENTIALS

**PixelLab Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## CONTEXT

Generate new attack animations for Elementalist, Ranger, and Shadowblade. Current animations show a punch — wrong. Each class needs a unique, class-appropriate attack. Shadowblade has 3 combo steps (3 separate animations). Save individual frame PNGs. Claude Code builds .anim clips and AnimatorController states afterward.

---

## FILES TOUCHED

- `Assets/Sprites/Characters/Elementalist/attack/` (new subfolder)
- `Assets/Sprites/Characters/Ranger/attack/` (new subfolder)
- `Assets/Sprites/Characters/Shadowblade/attack_step1/` (new subfolder)
- `Assets/Sprites/Characters/Shadowblade/attack_step2/` (new subfolder)
- `Assets/Sprites/Characters/Shadowblade/attack_step3/` (new subfolder)
- `Assets/STAGING/DONE.txt`

Do not touch any file not listed above.

---

## STOP AND ESCALATE — Report to Claude if:

- Any step requires a decision or judgment
- Frame shows punch/melee instead of specified cast/ranged action
- Black frames or wrong character visible
- Any unexpected error

---

## MANDATORY QC — APPLY AFTER EVERY DIRECTION

1. Read `frame_000.png` after generation
2. Describe: character, action, direction
3. PASS: correct character, correct action type (cast/ranged/blade), direction correct
4. FAIL: punch/melee visible when cast expected, wrong direction, black frame
5. On FAIL: re-generate with `-v2` suffix + more explicit description
6. Never save failing frames

---

## STEP 0 — Find Character IDs

```
mcp__pixellab__list_characters()
```

Note IDs for: Elementalist, Ranger, Shadowblade

---

## DIRECTIONS (5 per animation — east side mirrored by Claude)

south · north · west · south-west · north-west

---

## TASK 1 — Elementalist "Elemental Cast" Attack

**Character:** Elementalist (mage/caster, staff or open hands, magical robes)
**Action:** Extends one arm forward (frames 1-2), fire/elemental energy visibly gathers in palm (frames 3-4), releases a concentrated elemental bolt forward in the stated direction (frame 5), arm recoils slightly (frame 6). Clear casting motion, not melee. No loop.

```
mcp__pixellab__animate_character(
  character_id="[ELEMENTALIST_ID]",
  animation_name="elemental-cast-attack",
  direction="south",
  n_frames=6,
  action_description="Mage/elementalist character in magical robes, facing downward toward camera. Extends right arm forward toward camera, bright fire/elemental energy visibly concentrates and gathers in the open palm, then releases a glowing elemental energy bolt/projectile directly toward camera. The projectile launch is clearly visible. Ranged casting motion — NOT a punch, NOT melee. No loop. Final frame: arm extended, energy released."
)
```

Repeat for: north · west · south-west · north-west

**QC pass:** Elementalist visible, arm extended for cast, energy/bolt release clearly visible toward camera/direction, no punch motion
**QC fail:** Punch or melee swing, no energy visible, wrong direction

**Save path:**
```
Assets/Sprites/Characters/Elementalist/attack/south/frame_000.png ... frame_005.png
Assets/Sprites/Characters/Elementalist/attack/north/frame_000.png ... frame_005.png
Assets/Sprites/Characters/Elementalist/attack/west/frame_000.png ... frame_005.png
Assets/Sprites/Characters/Elementalist/attack/south-west/frame_000.png ... frame_005.png
Assets/Sprites/Characters/Elementalist/attack/north-west/frame_000.png ... frame_005.png
```

---

## TASK 2 — Ranger "Arrow Shot" Attack

**Character:** Ranger (archer, bow and quiver, light armor or leather)
**Action:** Draws bowstring back with arrow nocked (frames 1-2), holds at full draw aiming in stated direction (frame 3), releases — arrow visibly launches forward (frame 4), bow relaxes (frame 5). Clear archery motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[RANGER_ID]",
  animation_name="arrow-shot-attack",
  direction="south",
  n_frames=5,
  action_description="Archer/ranger character with bow and quiver in light armor, facing downward toward camera. Nocks arrow and draws bowstring back to full draw aimed toward camera, holds briefly at full draw, then releases the bowstring — arrow visibly launches forward toward camera as a projectile. Clear archery firing motion — NOT a punch. No loop. Final frame: bow returned to rest position."
)
```

Repeat for: north · west · south-west · north-west

**QC pass:** Ranger with bow visible, arrow draw and release motion, arrow projectile visible launching toward direction
**QC fail:** No bow visible, punch motion, arrow in wrong direction

**Save path:**
```
Assets/Sprites/Characters/Ranger/attack/south/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/attack/north/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/attack/west/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/attack/south-west/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/attack/north-west/frame_000.png ... frame_004.png
```

---

## TASK 3 — Shadowblade "Shadow Slash Step 1" (Left Horizontal)

**Character:** Shadowblade (dual-blade rogue, dark leather, shadow energy)
**Action:** Left blade sweeps horizontally from right to left across the body (frames 1-3), a brief shadow trail follows the blade, returns to ready stance (frames 4-5). Fast precise horizontal slash. No loop.

```
mcp__pixellab__animate_character(
  character_id="[SHADOWBLADE_ID]",
  animation_name="shadow-slash-step1",
  direction="south",
  n_frames=5,
  action_description="Dual-blade rogue in dark leather with shadow energy, facing downward toward camera. Left blade performs a fast horizontal slash sweeping right-to-left across body — shadow energy briefly trails behind the blade as a dark afterimage. Sharp precise motion. No loop. Final frame: back in combat ready stance."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Characters/Shadowblade/attack_step1/south/frame_000.png ... frame_004.png
(repeat per direction)
```

---

## TASK 4 — Shadowblade "Shadow Slash Step 2" (Right Diagonal)

**Action:** Right blade slashes diagonally from upper-left to lower-right (frames 1-3), shadow trail follows, returns (frames 4-5). No loop.

```
mcp__pixellab__animate_character(
  character_id="[SHADOWBLADE_ID]",
  animation_name="shadow-slash-step2",
  direction="south",
  n_frames=5,
  action_description="Dual-blade rogue in dark leather with shadow energy, facing downward toward camera. Right blade performs a fast diagonal slash from upper-left to lower-right — shadow energy trails behind blade as dark afterimage. Different motion from step 1 (diagonal vs horizontal). No loop. Final frame: back in combat ready stance."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Characters/Shadowblade/attack_step2/south/frame_000.png ... frame_004.png
(repeat per direction)
```

---

## TASK 5 — Shadowblade "Shadow Slash Step 3" (Dual Forward Thrust)

**Action:** Both blades together thrust forward toward target (frames 1-3), shadow energy concentrates at blade tips, retracts sharply (frames 4-5). Finishing combo move, most aggressive motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[SHADOWBLADE_ID]",
  animation_name="shadow-slash-step3",
  direction="south",
  n_frames=5,
  action_description="Dual-blade rogue in dark leather with shadow energy, facing downward toward camera. Both blades simultaneously thrust forward toward camera — shadow energy concentrating at the blade tips creating a brief intense flash, then sharply retracts. Aggressive finishing motion, most powerful-looking of the three combo steps. No loop. Final frame: back in combat ready stance."
)
```

Repeat for: north · west · south-west · north-west

**Save path:**
```
Assets/Sprites/Characters/Shadowblade/attack_step3/south/frame_000.png ... frame_004.png
(repeat per direction)
```

---

## COMPLETION LOG

Append to `Assets/STAGING/DONE.txt`:

```
[DONE-KIRO_CHAR_ATTACKS] Elementalist elemental-cast-attack — 5 directions | 2026-04-06
[DONE-KIRO_CHAR_ATTACKS] Ranger arrow-shot-attack — 5 directions | 2026-04-06
[DONE-KIRO_CHAR_ATTACKS] Shadowblade shadow-slash-step1 — 5 directions | 2026-04-06
[DONE-KIRO_CHAR_ATTACKS] Shadowblade shadow-slash-step2 — 5 directions | 2026-04-06
[DONE-KIRO_CHAR_ATTACKS] Shadowblade shadow-slash-step3 — 5 directions | 2026-04-06
[QC-RESULT] All 25 direction outputs checked — no punch motions, correct characters | 2026-04-06
```

---

## AFTER KIRO FINISHES — Claude Code handles

When user says "char attacks hazır", Claude Code will:
1. `AssetDatabase.Refresh()`
2. Import sprites PPU=96, Point, No compression
3. Build .anim clips from frames
4. Wire Elementalist + Ranger AnimatorController attack states
5. Wire Shadowblade 3-step combo (ComboStep 0/1/2 → step1/2/3 animations)
6. Play test all 3 classes
7. Update CURRENT_STATUS.md
