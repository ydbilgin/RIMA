# KIRO TASK — KIRO_DASH_ANIMS
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

Generate class-specific dash animations for 4 player classes. Each dash must feel unique and characteristic to that class. Save individual frame PNGs. Claude Code builds .anim clips afterward.

**Dash designs (Claude's decisions):**
- **Warblade:** "Iron Surge" — heavy forward charge, body leaned hard into momentum
- **Elementalist:** "Blink" — dissolve/fade out flash, brief teleport visual
- **Ranger:** "Vault" — graceful backward leap away from danger
- **Shadowblade:** "Shadow Step" — instant shadow dissolve leaving a dark silhouette at origin

**Shadow Step has a special mechanic:** When Shadow Step is activated, a dark silhouette remains at the origin point. Pressing dash again teleports back to that silhouette. The silhouette sprite is a separate still frame — also generate it in TASK 4.

---

## FILES TOUCHED

- `Assets/Sprites/Characters/Warblade/dash/`
- `Assets/Sprites/Characters/Elementalist/dash/`
- `Assets/Sprites/Characters/Ranger/dash/`
- `Assets/Sprites/Characters/Shadowblade/dash/`
- `Assets/Sprites/Characters/Shadowblade/shadow_silhouette/`
- `Assets/_STAGING/DONE.txt`

Do not touch any file not listed above.

---

## STOP AND ESCALATE — Report to Claude if:

- Any step requires a decision or judgment
- Warblade dash looks light/floaty (must look HEAVY)
- Ranger dash goes forward instead of backward
- Shadowblade silhouette is colored instead of dark/transparent
- Black frame or wrong character

---

## MANDATORY QC — APPLY AFTER EVERY DIRECTION

1. Read `frame_000.png` after generation
2. Describe: character, motion type, direction
3. PASS: correct character, correct motion feel, direction correct
4. FAIL: wrong motion feel (Warblade looks light, Ranger goes forward, etc.), wrong direction, black frame
5. On FAIL: re-generate with `-v2` suffix + more explicit description
6. Never save failing frames

---

## STEP 0 — Find Character IDs

```
mcp__pixellab__list_characters()
```

Note IDs for: Warblade, Elementalist, Ranger, Shadowblade

---

## DIRECTIONS (4 per animation — diagonals handled by BlendTree interpolation)

south · north · west · south-west

---

## TASK 1 — Warblade "Iron Surge" Dash

**Character:** Warblade (heavily armored warrior, great sword or heavy weapon)
**Action:** Body leans aggressively forward into the dash direction, low center of gravity, almost a tackle/charge pose. Feet barely leave ground. Heavy and unstoppable feel. 3-4 frames only (it's fast but heavy). No loop.

```
mcp__pixellab__animate_character(
  character_id="[WARBLADE_ID]",
  animation_name="iron-surge-dash",
  direction="south",
  n_frames=4,
  action_description="Heavily armored warrior with large weapon, dashing/charging forward toward camera (facing downward). Body aggressively leaned far forward, low center of gravity like a tackle or bull charge — weapon pulled back for momentum, legs driving hard. Heavy unstoppable charge motion. NOT a light float or jump. Looks powerful and earth-shaking. No loop."
)
```

Repeat for: north · west · south-west

**QC pass:** Warblade visible, body heavily leaned forward in charge direction, looks heavy and powerful NOT floaty
**QC fail:** Light jump, floating, upright posture, wrong direction

**Save path:**
```
Assets/Sprites/Characters/Warblade/dash/south/frame_000.png ... frame_003.png
Assets/Sprites/Characters/Warblade/dash/north/frame_000.png ... frame_003.png
Assets/Sprites/Characters/Warblade/dash/west/frame_000.png ... frame_003.png
Assets/Sprites/Characters/Warblade/dash/south-west/frame_000.png ... frame_003.png
```

---

## TASK 2 — Elementalist "Blink" Dash

**Character:** Elementalist (mage, robes, magical energy)
**Action:** Body dissolves/fades with a bright elemental flash (frames 1-2), brief empty space with lingering energy particles (frame 3), body reappears with a flash at destination (frame 4). Teleport feel, not physical movement. No loop.

```
mcp__pixellab__animate_character(
  character_id="[ELEMENTALIST_ID]",
  animation_name="blink-dash",
  direction="south",
  n_frames=4,
  action_description="Mage/elementalist in robes, facing downward toward camera. The character's body begins dissolving/fading out rapidly with a bright flash of elemental fire/arcane energy surrounding them, then the body becomes transparent/invisible briefly, then reappears in a flash at the new location. Magical teleport visual — NOT a physical run or jump. The transition frames show the dissolve and reform process. No loop."
)
```

Repeat for: north · west · south-west

**QC pass:** Elementalist visible, clear dissolve/fade or reappear effect with magical energy, not a jump
**QC fail:** Looks like running or jumping, no magical effect visible, wrong character

**Save path:**
```
Assets/Sprites/Characters/Elementalist/dash/south/frame_000.png ... frame_003.png
Assets/Sprites/Characters/Elementalist/dash/north/frame_000.png ... frame_003.png
Assets/Sprites/Characters/Elementalist/dash/west/frame_000.png ... frame_003.png
Assets/Sprites/Characters/Elementalist/dash/south-west/frame_000.png ... frame_003.png
```

---

## TASK 3 — Ranger "Vault" Dash

**Character:** Ranger (archer, light armor, bow)
**Action:** Pushes off and leaps BACKWARD away from the facing direction — a graceful controlled retreat vault. Light and nimble feel. Body momentarily airborne. No loop.

**IMPORTANT:** The Ranger leaps AWAY from the camera (backward), even though direction=south. This is intentional — it's a retreat.

```
mcp__pixellab__animate_character(
  character_id="[RANGER_ID]",
  animation_name="vault-dash",
  direction="south",
  n_frames=5,
  action_description="Archer/ranger in light armor with bow, originally facing downward toward camera. Pushes off the ground and leaps BACKWARD — away from camera — in a graceful controlled vault/retreat jump. Light nimble airborne motion, body visibly leaves the ground and arcs backward. Bow tucked close during leap. Landing lightly. Graceful not clumsy. No loop. Final frame: landing pose."
)
```

Repeat for: north · west · south-west

**QC pass:** Ranger visible, body clearly airborne/leaping, motion feels light and graceful
**QC fail:** Looks heavy like Warblade, no airborne moment visible, wrong direction feel

**Save path:**
```
Assets/Sprites/Characters/Ranger/dash/south/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/dash/north/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/dash/west/frame_000.png ... frame_004.png
Assets/Sprites/Characters/Ranger/dash/south-west/frame_000.png ... frame_004.png
```

---

## TASK 4 — Shadowblade "Shadow Step" Dash

**Character:** Shadowblade (dual-blade rogue, dark leather, shadow energy)
**Action:** Character instantly dissolves into shadow/dark energy (frames 1-2) — body becomes a dark blur/shadow — then completely vanishes (frame 3). Instantaneous disappearance with shadow trail. No loop.

**Note for Claude:** This animation plays when the Shadowblade initiates Shadow Step. A separate silhouette sprite (Task 5) remains at the origin. Pressing dash again recalls to that silhouette.

```
mcp__pixellab__animate_character(
  character_id="[SHADOWBLADE_ID]",
  animation_name="shadow-step-dash",
  direction="south",
  n_frames=3,
  action_description="Dual-blade rogue in dark leather, facing downward toward camera. Instantly dissolves into dark shadow energy — body rapidly becomes a dark silhouette/shadow blur, shadow energy ripples outward, then the character completely vanishes leaving only a brief dark residue. Instantaneous shadow dissolution. No light effects — purely dark shadow energy. No loop. Final frame: empty (character gone, only faint shadow residue)."
)
```

Repeat for: north · west · south-west

**QC pass:** Shadowblade visible in frame_000, clearly dissolves into shadow, dark (not bright) effect, gone by frame_002
**QC fail:** Bright flash, looks like Elementalist blink, character still visible in final frame, wrong direction

**Save path:**
```
Assets/Sprites/Characters/Shadowblade/dash/south/frame_000.png ... frame_002.png
Assets/Sprites/Characters/Shadowblade/dash/north/frame_000.png ... frame_002.png
Assets/Sprites/Characters/Shadowblade/dash/west/frame_000.png ... frame_002.png
Assets/Sprites/Characters/Shadowblade/dash/south-west/frame_000.png ... frame_002.png
```

---

## TASK 5 — Shadowblade Shadow Silhouette (Static Sprite)

**Purpose:** A dark shadow silhouette that remains at the origin point when Shadowblade uses Shadow Step. Player can press dash again to recall to this position. This is a SINGLE STILL FRAME — the silhouette sprite used by Unity as a static sprite.

**Generate south direction only** — Claude Code will flip for other directions in code.

```
mcp__pixellab__animate_character(
  character_id="[SHADOWBLADE_ID]",
  animation_name="shadow-step-silhouette",
  direction="south",
  n_frames=1,
  action_description="Dual-blade rogue silhouette — a dark ghost/shadow imprint of the character in combat ready stance, facing downward toward camera. Entirely dark semi-transparent shadow, no detail visible, only the outline shape of the character. Like a dark shadow stain on the ground. No color except dark purple-black shadow. This is a ghost silhouette left behind after teleporting."
)
```

**QC pass:** Dark silhouette shape of Shadowblade visible, no colored details, clearly a shadow imprint
**QC fail:** Fully detailed colored character, too bright, no shadow effect

**Save path:**
```
Assets/Sprites/Characters/Shadowblade/shadow_silhouette/frame_000.png
```

---

## COMPLETION LOG

Append to `Assets/_STAGING/DONE.txt`:

```
[DONE-KIRO_DASH_ANIMS] Warblade iron-surge-dash — 4 directions | 2026-04-06
[DONE-KIRO_DASH_ANIMS] Elementalist blink-dash — 4 directions | 2026-04-06
[DONE-KIRO_DASH_ANIMS] Ranger vault-dash — 4 directions | 2026-04-06
[DONE-KIRO_DASH_ANIMS] Shadowblade shadow-step-dash — 4 directions | 2026-04-06
[DONE-KIRO_DASH_ANIMS] Shadowblade shadow-step-silhouette — 1 frame | 2026-04-06
[QC-RESULT] All outputs checked — correct motion feel per class confirmed | 2026-04-06
```

---

## AFTER KIRO FINISHES — Claude Code handles

When user says "dash anims hazır", Claude Code will:
1. `AssetDatabase.Refresh()`
2. Import sprites with correct PPU (96 for characters), Point, No compression
3. Build .anim clips for all 4 class dashes
4. Wire Dash state in each AnimatorController (IsDashing bool)
5. Implement Shadow Step Recall mechanic in Shadowblade_SkillController.cs:
   - On dash activate: save origin position, spawn silhouette sprite at origin
   - On second dash press while silhouette active: teleport to saved position, destroy silhouette
   - Silhouette auto-destroys after N seconds if recall not used
6. Add silhouette sprite to Shadowblade resources
7. Play test all 4 dashes
8. Update CURRENT_STATUS.md
