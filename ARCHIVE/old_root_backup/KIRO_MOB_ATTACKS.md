# KIRO TASK — MOB ATTACKS
*Date: 2026-04-07 | Read this file, apply in order. Do not read other files.*

---

## DECISION
**Tool: PixelLab MCP**
Mob attack animations → player sees these less frequently than player animations, and mob diversity matters more than per-frame perfection. MCP quality is sufficient.

---

## RISK LEVEL: LOW
> Deterministic · Mechanical · Isolated · Bounded · Mechanically verifiable ✓

---

## CREDENTIALS

**PixelLab Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## CONTEXT

Generate new attack animations for 4 existing enemy characters: Penitent, ChainWarden, RelicCaster, FractureImp. The old "cross-punch" animations are wrong — each enemy now has a thematically appropriate unique attack. Save individual frame PNGs to the staging paths below. Claude Code will build the .anim clips afterward.

---

## FILES TOUCHED

- `Assets/Sprites/Enemies/Penitent/attack/` (new subfolder, frames only)
- `Assets/Sprites/Enemies/ChainWarden/attack/` (new subfolder, frames only)
- `Assets/Sprites/Enemies/RelicCaster/attack/` (new subfolder, frames only)
- `Assets/Sprites/Enemies/FractureImp/attack/` (new subfolder, frames only)
- `Assets/STAGING/DONE.txt`

Do not touch any file not listed above.

---

## STOP AND ESCALATE — Report to Claude if:

- Any step requires a decision or judgment
- Frame output is black, empty, or shows wrong character
- An unexpected error occurs that isn't covered below
- You are about to touch a file not in the list above

---

## MANDATORY QC — APPLY AFTER EVERY DIRECTION

1. Read `frame_000.png` immediately after generation
2. Describe exactly: character, action, direction
3. PASS if: correct enemy visible, attack motion is clearly in the stated direction, no black or empty frames
4. FAIL if: wrong direction, black frame, placeholder, wrong character, wrong action type
5. On FAIL: re-generate with `-v2` appended to animation_name, more explicit description
6. Never save failing frames

---

## STEP 0 — Find Character IDs

```
mcp__pixellab__list_characters()
```

Note the IDs for: Penitent, ChainWarden, RelicCaster, FractureImp

---

## DIRECTIONS (5 per animation — Claude mirrors east side)

For each animation: south · north · west · south-west · north-west

---

## TASK 1 — Penitent "Holy Condemnation" Attack

**Character:** Penitent (dark fantasy religious fanatic enemy, robed, armored gauntlets)
**Action:** Raises both arms skyward, holy energy gathers above head (frames 1-3), then slams both fists down into the ground releasing a holy shockwave pulse at feet (frames 4-7). Heavy, deliberate motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[PENITENT_ID]",
  animation_name="holy-condemnation-attack",
  direction="south",
  n_frames=7,
  action_description="Dark-robed religious fanatic enemy in armored gauntlets, facing downward toward camera. Raises both arms high above head (wind-up), holy energy visibly gathering overhead, then slams both fists hard into the ground directly in front (toward camera), releasing a shockwave. Heavy powerful downward slam motion. No loop. Final frame: crouched forward, fists on ground."
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

**QC pass:** Penitent visible, arms raise then slam down, motion in correct direction
**QC fail:** Punch motion, wrong direction, invisible, black frame

**Save path:**
```
Assets/Sprites/Enemies/Penitent/attack/south/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/Penitent/attack/north/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/Penitent/attack/west/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/Penitent/attack/south-west/frame_000.png ... frame_006.png
Assets/Sprites/Enemies/Penitent/attack/north-west/frame_000.png ... frame_006.png
```

---

## TASK 2 — ChainWarden "Chain Lash" Attack

**Character:** ChainWarden (armored warden enemy carrying heavy chains)
**Action:** Pulls heavy chain back (frames 1-2), then whips/lashes it forward toward the target direction — chain visibly extends and reaches forward (frames 3-5), then snaps back (frame 6). Fast whip motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[CHAINWARDEN_ID]",
  animation_name="chain-lash-attack",
  direction="south",
  n_frames=6,
  action_description="Armored warden enemy carrying heavy iron chains, facing downward toward camera. Pulls chain back behind body (windup), then violently whips the chain forward and downward toward camera — the chain visibly extends and reaches toward camera, then recoils. Fast whipping motion. No loop. Final frame: chain pulled back, standing upright."
)
```

Repeat for: north · west · south-west · north-west

**QC pass:** ChainWarden visible, chain whip motion clearly extending in stated direction
**QC fail:** No chain visible, punch motion, wrong direction

**Save path:**
```
Assets/Sprites/Enemies/ChainWarden/attack/south/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/ChainWarden/attack/north/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/ChainWarden/attack/west/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/ChainWarden/attack/south-west/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/ChainWarden/attack/north-west/frame_000.png ... frame_005.png
```

---

## TASK 3 — RelicCaster "Relic Bolt" Cast

**Character:** RelicCaster (dark robed spellcaster enemy holding a glowing relic/artifact)
**Action:** Raises glowing relic forward with both hands (frames 1-2), energy visibly charges/pulses around it (frames 3-4), then releases a bolt of energy toward target direction (frame 5), relic dims and lowers (frame 6). Casting spellcaster pose. No loop.

```
mcp__pixellab__animate_character(
  character_id="[RELICCASTER_ID]",
  animation_name="relic-bolt-cast",
  direction="south",
  n_frames=6,
  action_description="Dark robed spellcaster enemy holding a glowing ancient relic artifact, facing downward toward camera. Extends the relic forward with both hands toward camera, glowing energy visibly charges and pulses around the relic, then fires/releases a bolt of energy toward camera. Clear casting/channeling motion. No loop. Final frame: relic lowered, arms relaxed."
)
```

Repeat for: north · west · south-west · north-west

**QC pass:** RelicCaster visible, relic extended forward, casting/energy release motion in correct direction
**QC fail:** No relic visible, punch motion, wrong direction

**Save path:**
```
Assets/Sprites/Enemies/RelicCaster/attack/south/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/RelicCaster/attack/north/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/RelicCaster/attack/west/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/RelicCaster/attack/south-west/frame_000.png ... frame_005.png
Assets/Sprites/Enemies/RelicCaster/attack/north-west/frame_000.png ... frame_005.png
```

---

## TASK 4 — FractureImp "Shard Slash" Attack

**Character:** FractureImp (small fast demonic imp enemy with sharp crystalline claws)
**Action:** Two quick cross slashes — left claw swipes (frame 1), right claw swipes opposite direction (frame 2), both claws retract (frame 3). Very fast, aggressive, compact motion. No loop.

```
mcp__pixellab__animate_character(
  character_id="[FRACTUREIMP_ID]",
  animation_name="shard-slash-attack",
  direction="south",
  n_frames=3,
  action_description="Small fast demonic imp creature with sharp crystalline claws, facing downward toward camera. Performs two rapid cross-slash motions toward camera — first left claw slashes across (lower-left to upper-right), then right claw slashes back (upper-right to lower-left), forming an X slash pattern. Extremely fast compact motion. No loop."
)
```

Repeat for: north · west · south-west · north-west

**QC pass:** FractureImp visible, two fast claw slashes visible, X-pattern motion, direction correct
**QC fail:** Single punch motion, too slow, wrong direction, wrong character size

**Save path:**
```
Assets/Sprites/Enemies/FractureImp/attack/south/frame_000.png ... frame_002.png
Assets/Sprites/Enemies/FractureImp/attack/north/frame_000.png ... frame_002.png
Assets/Sprites/Enemies/FractureImp/attack/west/frame_000.png ... frame_002.png
Assets/Sprites/Enemies/FractureImp/attack/south-west/frame_000.png ... frame_002.png
Assets/Sprites/Enemies/FractureImp/attack/north-west/frame_000.png ... frame_002.png
```

---

## COMPLETION LOG

Append to `Assets/STAGING/DONE.txt`:

```
[DONE-MOB_ATTACKS] Penitent holy-condemnation-attack — 5 directions | 2026-04-07
[DONE-MOB_ATTACKS] ChainWarden chain-lash-attack — 5 directions | 2026-04-07
[DONE-MOB_ATTACKS] RelicCaster relic-bolt-cast — 5 directions | 2026-04-07
[DONE-MOB_ATTACKS] FractureImp shard-slash-attack — 5 directions | 2026-04-07
[QC-RESULT] Each direction checked: character visible, action type correct, direction correct | 2026-04-07
```

---

## AFTER KIRO FINISHES — Claude Code handles

Tell Claude: **"mob attacks hazır"**

Claude will:
1. `AssetDatabase.Refresh()`
2. Import sprites, set PPU=64, Point filter, No compression
3. Build .anim clips: `{mob}_attack_{direction}.anim` from individual frames
4. Rebuild Attack BlendTree in each controller (5 clips + east mirrors)
5. Play test in Unity
6. Update CURRENT_STATUS.md
